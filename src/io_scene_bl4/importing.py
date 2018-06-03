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
                circuit = bl4.Circuit.load(dec_file, key, block_size)
        self.convert(circuit)
        return {'FINISHED'}

    def convert(self, circuit: bl4.Circuit):
        for i, sector in enumerate(circuit.sectors):
            self.convert_sector(sector, "Sector{0:03d}".format(i))

    def convert_textures(self):
        pass

    def convert_sector(self, sector: bl4.Sector, name: str):
        bm = bmesh.new()

        # Add vertex positions (TODO: use normals with normals_split_custom_set()).
        for position in sector.mesh.positions:
            bm.verts.new(position)
        bm.verts.ensure_lookup_table()
        bm.verts.index_update()

        # Create faces (provided as triangle list). Ignore duplicate faces for now (what's the deal with them?)
        for face in sector.mesh.faces:
            if face.vertex_count == 3:
                verts = (bm.verts[face.indices[0]], bm.verts[face.indices[1]], bm.verts[face.indices[2]])
            elif face.vertex_count == 4:
                verts = (bm.verts[face.indices[0]], bm.verts[face.indices[1]], bm.verts[face.indices[2]], bm.verts[face.indices[3]])
            else:
                raise ValueError("Unsupported number of face vertices.")
            try:
                bm.faces.new(verts)
            except ValueError as e:
                print(e)


        # Link the BMesh to a mesh object in the scene.
        mesh = bpy.data.meshes.new(name)
        bm.to_mesh(mesh)
        bm.free()
        obj = bpy.data.objects.new(name, mesh)
        bpy.context.scene.objects.link(obj)
