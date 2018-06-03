import os
import io
import bmesh
import bpy
import bpy_extras
from . import bl4
from . import pbdf


def menu_func_import(self, context):
    self.layout.operator(ImportOperator.bl_idname, text="UbiSoft BL4 (.bl4)")


class ImportOperator(bpy.types.Operator, bpy_extras.io_utils.ImportHelper):
    """Load a BL4 model file"""
    bl_idname = "import_scene.bl4"
    bl_label = "Import BL4"
    bl_options = {'UNDO'}
    filename_ext = ".bl4"
    filter_glob = bpy.props.StringProperty(default="*.bl4", options={'HIDDEN'})
    filepath = bpy.props.StringProperty(
        name="File Path",
        description="Filepath used for importing the BL4 file",
        maxlen=1024)

    def __init__(self):
        self.circuit = None  # type: bl4.Circuit

    def execute(self, context):
        file_name = self.properties.filepath
        file_size = os.path.getsize(file_name)
        with io.BytesIO() as dec_file:
            with open(file_name, "rb") as f:
                key = pbdf.retrieve_key(f, file_size)
                f.seek(0)
                block_size = pbdf.retrieve_block_size(f, file_size, key)
                f.seek(0)
                pbdf.decrypt(f, dec_file, key, block_size)
                dec_file.seek(0)
                self.circuit = bl4.Circuit.load(dec_file, key, block_size)
        self.convert()
        return {'FINISHED'}

    def convert(self):
        for i, texture in enumerate(self.circuit.textures):
            self.convert_texture(texture, "Image{0:03d}".format(i), self.circuit.textures.size)
        for i, sector in enumerate(self.circuit.sectors):
            self.convert_sector(sector, "Sector{0:03d}".format(i))

    def convert_texture(self, texture: bl4.Texture, name: str, size: int):
        # Create image and convert upside-down RGB565 pixel data.
        img = bpy.data.images.new(name, width=size, height=size)
        pixels = [0.0] * size * size * 4
        data = memoryview(texture.data).cast("H")
        for y in range(size):
            for x in range(size):
                pixel = data[y * size + x]
                idx = ((size + ~y) * size + x) * 4
                pixels[idx] = (pixel >> 11) / 0b11111
                pixels[idx + 1] = (pixel >> 5 & 0b111111) / 0b111111
                pixels[idx + 2] = (pixel & 0b11111) / 0b11111
                pixels[idx + 3] = 1.0
        img.pixels = pixels


    def convert_sector(self, sector: bl4.Sector, name: str):
        bm = bmesh.new()
        # Add vertex positions (TODO: use normals with normals_split_custom_set()).
        for position in sector.mesh.positions:
            bm.verts.new(position)
        bm.verts.ensure_lookup_table()
        bm.verts.index_update()
        # Create faces (provided as triangle / quad list). Ignore duplicate faces for now (apparently not required).
        for face in sector.mesh.faces:
            try:
                bm.faces.new((bm.verts[face.indices[i]] for i in range(face.vertex_count)))
            except ValueError as e:
                print(e)
        # Link the BMesh to a mesh object in the scene.
        mesh = bpy.data.meshes.new(name)
        bm.to_mesh(mesh)
        bm.free()
        obj = bpy.data.objects.new(name, mesh)
        bpy.context.scene.objects.link(obj)
