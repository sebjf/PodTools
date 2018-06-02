import os
import io
import bpy
import bpy_extras
from .bl4 import *
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
        self.circuit = None

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
                self.circuit = Circuit.load(dec_file, key, block_size)
        self.convert()
        return {'FINISHED'}

    def convert(self):
        pass
