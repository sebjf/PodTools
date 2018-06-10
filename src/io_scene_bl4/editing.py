import bpy
import bmesh
from bpy.props import BoolProperty, IntProperty, StringProperty
from bpy.types import Operator, Panel, WindowManager

BL4_LAYER_NAME = "bl4_name"
BL4_LAYER_PROPS = "bl4_props"
current_bm = None
ignore_layer_update = False


def register():
    bpy.utils.register_class(BL4EditPanel)
    bpy.utils.register_class(BL4AddNameLayerOperator)
    bpy.utils.register_class(BL4AddPropsLayerOperator)
    bpy.app.handlers.scene_update_post.append(scene_update_post_handler)
    WindowManager.bl4_layer_name = StringProperty(name="Name",
                                                  update=layer_name_update)
    WindowManager.bl4_layer_props = IntProperty(name="Props",
                                                default=0, min=0,
                                                update=layer_props_update)
    WindowManager.bl4_layer_prop_visible = BoolProperty(name="Visible",
                                                        get=layer_prop_visible_get, set=layer_prop_visible_set)
    WindowManager.bl4_layer_prop_road = BoolProperty(name="Road",
                                                     get=layer_prop_road_get, set=layer_prop_road_set)
    WindowManager.bl4_layer_prop_wall = BoolProperty(name="Wall",
                                                     get=layer_prop_wall_get, set=layer_prop_wall_set)
    WindowManager.bl4_layer_prop_deco = BoolProperty(name="Decoration", description="Marks the polygon as unsolid decoration",
                                                     get=layer_prop_deco_get, set=layer_prop_deco_set)
    WindowManager.bl4_layer_prop_black = BoolProperty(name="Transparent", description="Renders black as transparency",
                                                      get=layer_prop_black_get, set=layer_prop_black_set)
    WindowManager.bl4_layer_prop_2side = BoolProperty(name="2-sided", description="Renders the polygon from both sides",
                                                      get=layer_prop_2side_get, set=layer_prop_2side_set)
    WindowManager.bl4_layer_prop_dural = BoolProperty(name="Dural", description="Renders a metallic-like texture",
                                                      get=layer_prop_dural_get, set=layer_prop_dural_set)
    WindowManager.bl4_layer_prop_slip = IntProperty(name="Slipperyness",
                                                    default=0, min=0, max=255,
                                                    get=layer_prop_slip_get, set=layer_prop_slip_set)


def unregister():
    bpy.utils.unregister_class(BL4EditPanel)
    bpy.utils.unregister_class(BL4AddNameLayerOperator)
    bpy.utils.unregister_class(BL4AddPropsLayerOperator)
    bpy.app.handlers.scene_update_post.remove(scene_update_post_handler)
    del WindowManager.bl4_layer_name
    del WindowManager.bl4_layer_props
    del WindowManager.bl4_layer_prop_visible
    del WindowManager.bl4_layer_prop_road
    del WindowManager.bl4_layer_prop_wall
    del WindowManager.bl4_layer_prop_deco
    del WindowManager.bl4_layer_prop_black
    del WindowManager.bl4_layer_prop_2side
    del WindowManager.bl4_layer_prop_dural
    del WindowManager.bl4_layer_prop_slip


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


def layer_prop_visible_get(self):
    return self.bl4_layer_props & 0b1 != 0


def layer_prop_visible_set(self, value):
    if value:
        self.bl4_layer_props |= 0b1
    else:
        self.bl4_layer_props &= ~0b1


def layer_prop_road_get(self):
    return self.bl4_layer_props & 0b1000 != 0


def layer_prop_road_set(self, value):
    if value:
        self.bl4_layer_props |= 0b1000
    else:
        self.bl4_layer_props &= ~0b1000


def layer_prop_wall_get(self):
    return self.bl4_layer_props & 0b100000 != 0


def layer_prop_wall_set(self, value):
    if value:
        self.bl4_layer_props |= 0b100000
    else:
        self.bl4_layer_props &= ~0b100000


def layer_prop_deco_get(self):
    return self.bl4_layer_props & 0b10000000 != 0


def layer_prop_deco_set(self, value):
    if value:
        self.bl4_layer_props |= 0b10000000
    else:
        self.bl4_layer_props &= ~0b10000000


def layer_prop_black_get(self):
    return self.bl4_layer_props & 0b100000000 != 0


def layer_prop_black_set(self, value):
    if value:
        self.bl4_layer_props |= 0b100000000
    else:
        self.bl4_layer_props &= ~0b100000000


def layer_prop_2side_get(self):
    return self.bl4_layer_props & 0b10000000000 != 0


def layer_prop_2side_set(self, value):
    if value:
        self.bl4_layer_props |= 0b10000000000
    else:
        self.bl4_layer_props &= ~0b10000000000


def layer_prop_dural_get(self):
    return self.bl4_layer_props & 0b10000000000000 != 0


def layer_prop_dural_set(self, value):
    if value:
        self.bl4_layer_props |= 0b10000000000000
    else:
        self.bl4_layer_props &= ~0b10000000000000


def layer_prop_slip_get(self):
    return self.bl4_layer_props >> 16 & 0xFF


def layer_prop_slip_set(self, value):
    new = self.bl4_layer_props
    new &= ~(0xFF << 16)
    new |= (value & 0xFF) << 16
    self.bl4_layer_props = new


class BL4EditPanel(Panel):
    bl_label = "UbiSoft BL4"
    bl_region_type = 'UI'
    bl_space_type = 'VIEW_3D'

    @classmethod
    def poll(cls, context):
        return current_bm is not None

    def draw(self, context):
        wm = context.window_manager
        # Draw name layer.
        layer = current_bm.faces.layers.string.get(BL4_LAYER_NAME)
        if layer:
            self.layout.prop(wm, "bl4_layer_name")
        else:
            self.layout.operator(BL4AddNameLayerOperator.bl_idname)
        # Draw props layer.
        layer = current_bm.faces.layers.int.get(BL4_LAYER_PROPS)
        if layer:
            row = self.layout.row()
            row.prop(wm, "bl4_layer_prop_road")
            row.prop(wm, "bl4_layer_prop_wall")
            self.layout.prop(wm, "bl4_layer_prop_deco")
            self.layout.prop(wm, "bl4_layer_prop_slip")
            row = self.layout.row()
            row.prop(wm, "bl4_layer_prop_visible")
            row.prop(wm, "bl4_layer_prop_2side")
            row = self.layout.row()
            row.prop(wm, "bl4_layer_prop_black")
            row.prop(wm, "bl4_layer_prop_dural")
            # DEBUG
            self.layout.label("Flag Debug")
            self.layout.prop(wm, "bl4_layer_props")
            self.layout.label(format(wm.bl4_layer_props >> 24 & 0xFF, "#010b"))
            self.layout.label(format(wm.bl4_layer_props >> 16 & 0xFF, "#010b"))
            self.layout.label(format(wm.bl4_layer_props >> 8 & 0xFF, "#010b"))
            self.layout.label(format(wm.bl4_layer_props & 0xFF, "#010b"))
        else:
            self.layout.operator(BL4AddPropsLayerOperator.bl_idname)


class BL4AddNameLayerOperator(Operator):
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


class BL4AddPropsLayerOperator(Operator):
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
