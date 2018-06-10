from typing import Tuple
import bpy


def create_texture_material(name: str, image: bpy.types.Image):
    """ Creates a textured cycles material.
    Args:
        name: The name of the resulting material.
        image: The image to use as the texture.
    Returns:
        The created Cycles material.
    """
    material = bpy.data.materials.new(name)
    material.use_nodes = True
    nodes = material.node_tree.nodes
    links = material.node_tree.links
    nodes.clear()

    texture_node = nodes.new("ShaderNodeTexImage")
    texture_node.image = image
    diffuse_node = nodes.new("ShaderNodeBsdfDiffuse")
    output_node = nodes.new("ShaderNodeOutputMaterial")

    links.new(texture_node.outputs['Color'], diffuse_node.inputs['Color'])
    links.new(diffuse_node.outputs['BSDF'], output_node.inputs['Surface'])
    return material


def create_color_material(name: str, color: Tuple[float, float, float, float]):
    material = bpy.data.materials.new(name)
    material.use_nodes = True
    nodes = material.node_tree.nodes
    links = material.node_tree.links
    nodes.clear()

    diffuse_node = nodes.new("ShaderNodeBsdfDiffuse")
    diffuse_node.inputs['Color'].default_value = color
    output_node = nodes.new("ShaderNodeOutputMaterial")

    links.new(diffuse_node.outputs['BSDF'], output_node.inputs['Surface'])
    return material
