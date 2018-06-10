import bpy
import bmesh

BL4_LAYER_NAME = "bl4_name"
BL4_LAYER_PROPS = "bl4_props"
current_bm = None
ignore_layer_update = False


def register():
    bpy.utils.register_class(BL4EditPanel)
    bpy.utils.register_class(BL4AddNameLayerOperator)
    bpy.utils.register_class(BL4AddPropsLayerOperator)
    bpy.app.handlers.scene_update_post.append(scene_update_post_handler)
    bpy.types.WindowManager.bl4_layer_name = bpy.props.StringProperty(name="Name", update=layer_name_update)
    bpy.types.WindowManager.bl4_layer_props = bpy.props.IntProperty(name="Props", update=layer_props_update)


def unregister():
    bpy.utils.unregister_class(BL4EditPanel)
    bpy.utils.unregister_class(BL4AddNameLayerOperator)
    bpy.utils.unregister_class(BL4AddPropsLayerOperator)
    bpy.app.handlers.scene_update_post.remove(scene_update_post_handler)
    del bpy.types.WindowManager.bl4_layer_name
    del bpy.types.WindowManager.bl4_layer_props


@bpy.app.handlers.persistent
def scene_update_post_handler(scene):
    global current_bm, ignore_layer_update
    if bpy.context.mode == 'EDIT_MESH':
        # Get the current BMesh instance.
        current_bm = current_bm or bmesh.from_edit_mesh(bpy.context.object.data)
        try:
            active_face = current_bm.faces.active
        except ReferenceError:  # BMesh instance changed
            current_bm = bmesh.from_edit_mesh(bpy.context.object.data)
            active_face = current_bm.faces.active
        # Update the displayed properties according to selected faces.
        if active_face:
            ignore_layer_update = True
            layer = current_bm.faces.layers.string.get(BL4_LAYER_NAME)
            if layer:
                bpy.context.window_manager.bl4_layer_name = active_face[layer].decode()
            layer = current_bm.faces.layers.int.get(BL4_LAYER_PROPS)
            if layer:
                bpy.context.window_manager.bl4_layer_props = active_face[layer]
            ignore_layer_update = False
    else:
        current_bm = None


def layer_name_update(self, context):
    if not ignore_layer_update:
        selected_faces = (face for face in current_bm.faces if face.select)
        layer = current_bm.faces.layers.string[BL4_LAYER_NAME]
        for face in selected_faces:
            face[layer] = self.bl4_layer_name.encode()


def layer_props_update(self, context):
    if not ignore_layer_update:
        selected_faces = (face for face in current_bm.faces if face.select)
        layer = current_bm.faces.layers.int[BL4_LAYER_PROPS]
        for face in selected_faces:
            face[layer] = self.bl4_layer_props


class BL4EditPanel(bpy.types.Panel):
    bl_label = "UbiSoft BL4"
    bl_region_type = 'UI'
    bl_space_type = 'VIEW_3D'

    @classmethod
    def poll(cls, context):
        return current_bm is not None

    def draw(self, context):
        wm = context.window_manager
        # Draw name layer.
        row = self.layout.row()
        layer = current_bm.faces.layers.string.get(BL4_LAYER_NAME)
        if layer:
            self.layout.prop(wm, "bl4_layer_name")
        else:
            row.operator(BL4AddNameLayerOperator.bl_idname)
        # Draw props layer.
        row = self.layout.row()
        layer = current_bm.faces.layers.int.get(BL4_LAYER_PROPS)
        if layer:
            self.layout.prop(wm, "bl4_layer_props")
        else:
            row.operator(BL4AddPropsLayerOperator.bl_idname)


class BL4AddNameLayerOperator(bpy.types.Operator):
    bl_idname = "mesh.add_bl4_face_name_layer"
    bl_label = "Add name layer"

    @classmethod
    def poll(cls, context):
        return current_bm is not None

    def execute(self, context):
        # Ensured the object data is a mesh.
        layer = current_bm.faces.layers.string.get(BL4_LAYER_NAME)
        if not layer:
            current_bm.faces.layers.string.new(BL4_LAYER_NAME)
        return {'FINISHED'}


class BL4AddPropsLayerOperator(bpy.types.Operator):
    bl_idname = "mesh.add_bl4_face_props_layer"
    bl_label = "Add properties layer"

    @classmethod
    def poll(cls, context):
        return current_bm is not None

    def execute(self, context):
        # Ensured the object data is a mesh.
        layer = current_bm.faces.layers.int.get(BL4_LAYER_PROPS)
        if not layer:
            current_bm.faces.layers.int.new(BL4_LAYER_PROPS)
        return {'FINISHED'}
