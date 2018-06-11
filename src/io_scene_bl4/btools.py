from typing import Tuple
import bpy


def create_empty_material(name: str, render_engine: str = None):
    """ Creates an empty material for the given render engine.
    Args:
        name: The name of the resulting material.
        render_engine: The render engine to set the material up for, or None to use the currently active engine.
    Returns:
        The created material.
    """
    render_engine = render_engine or bpy.context.scene.render.engine

    material = bpy.data.materials.new(name)

    if render_engine in ('BLENDER_RENDER', 'BLENDER_GAME'):
        material.diffuse_intensity = 1
        material.specular_intensity = 0

    elif render_engine == 'CYCLES':
        material.use_nodes = True
        material.node_tree.nodes.clear()

    else:
        raise ValueError("Cannot create material: Unsupported render engine.")

    return material


def create_texture_material(name: str, image: bpy.types.Image, render_engine: str = None):
    """ Creates a textured material for the given render engine.
    Args:
        name: The name of the resulting material.
        image: The image to use as the texture.
        render_engine: The render engine to set the material up for, or None to use the currently active engine.
    Returns:
        The created material.
    """
    render_engine = render_engine or bpy.context.scene.render.engine

    material = create_empty_material(name, render_engine)

    if render_engine in ('BLENDER_RENDER', 'BLENDER_GAME'):
        tex = bpy.data.textures.new(name, type = 'IMAGE')
        tex.image = image

        slot = material.texture_slots.add()
        slot.texture = tex

    elif render_engine == 'CYCLES':
        nodes = material.node_tree.nodes
        links = material.node_tree.links

        texture_node = nodes.new("ShaderNodeTexImage")
        texture_node.image = image
        diffuse_node = nodes.new("ShaderNodeBsdfDiffuse")
        output_node = nodes.new("ShaderNodeOutputMaterial")

        links.new(texture_node.outputs['Color'], diffuse_node.inputs['Color'])
        links.new(diffuse_node.outputs['BSDF'], output_node.inputs['Surface'])

    else:
        raise ValueError("Cannot create material: Unsupported render engine.")

    return material


def create_color_material(name: str, color: Tuple[float, float, float, float], render_engine: str = None):
    """ Creates a color material for the given render engine.
    Args:
        name: The name of the resulting material.
        color: The color to use.
        render_engine: The render engine to set the material up for, or None to use the currently active engine.
    Returns:
        The created material.
    """
    render_engine = render_engine or bpy.context.scene.render.engine

    material = create_empty_material(name, render_engine)

    if render_engine in ('BLENDER_RENDER', 'BLENDER_GAME'):
        material.diffuse_color = color[:3]

    elif render_engine == 'CYCLES':
        nodes = material.node_tree.nodes
        links = material.node_tree.links

        diffuse_node = nodes.new("ShaderNodeBsdfDiffuse")
        diffuse_node.inputs['Color'].default_value = color
        output_node = nodes.new("ShaderNodeOutputMaterial")

        links.new(diffuse_node.outputs['BSDF'], output_node.inputs['Surface'])

    else:
        raise ValueError("Cannot create material: Unsupported render engine.")

    return material
