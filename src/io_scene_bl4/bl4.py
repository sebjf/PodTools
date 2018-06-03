from . import pbdf
from .binary import *
from typing import *


class Circuit:
    def __init__(self):
        self.events = None
        self.macros = None
        self.track_name = None
        self.texture_lod = None
        self.project_name = None
        self.textures = None
        self.sectors = None

    @classmethod
    def load(cls, file, key, block_size):
        new = cls.__new__(cls)
        offsets = pbdf.read_header_offsets(file, block_size)
        file.seek(offsets[0])
        # Header
        if read_int32(file) != 3:
            raise AssertionError("Check value is not 3")
        read_int32(file)
        # Events
        event_count = read_int32(file)
        _ = read_int32(file)  # Buffer size
        new.events = [Event.load(file) for _ in range(event_count)]
        # Macros
        new.macros = MacroList.load(file)
        # General
        new.track_name = pbdf.read_string(file)
        new.texture_lod = read_int32s(file, 16)
        new.project_name = pbdf.read_string(file)
        # Textures
        new.textures = TextureList.load(file, 256, 2)
        # Sectors
        new.sectors = SectorList.load(file, key)
        return new


class Event:
    @classmethod
    def load(cls, file):
        new = cls.__new__(cls)
        new.name = pbdf.read_string(file)
        new.siz_param = read_int32(file)
        new.num_param = read_int32(file)
        new.param_data = file.read(new.siz_param * new.num_param)
        return new


class MacroList:
    @classmethod
    def load(cls, file):
        new = cls.__new__(cls)
        new.base = [Macro.load(file, 1) for _ in range(read_int32(file))]
        new.main = [Macro.load(file, 1) for _ in range(read_int32(file))]
        new.init = [Macro.load(file, 1) for _ in range(read_int32(file))]
        new.active = [Macro.load(file, 1) for _ in range(read_int32(file))]
        new.desactive = [Macro.load(file, 1) for _ in range(read_int32(file))]
        new.remplace = [Macro.load(file, 2) for _ in range(read_int32(file))]
        new.echange = [Macro.load(file, 2) for _ in range(read_int32(file))]
        return new


class Macro:
    @classmethod
    def load(cls, file, value_count):
        new = cls.__new__(cls)
        new.values = read_int32s(file, value_count)
        return new


class TextureList:
    @classmethod
    def load(cls, file, texture_size, pixel_size):
        new = cls.__new__(cls)
        count = read_int32(file)
        _ = read_int32(file)
        new.textures = [Texture.load(file) for _ in range(count)]
        for texture in new.textures:
            texture.data = file.read(texture_size * texture_size * pixel_size)
        return new


class Texture:
    @classmethod
    def load(cls, file):
        new = cls.__new__(cls)
        new.regions = [TextureRegion.load(file) for _ in range(read_int32(file))]
        new.data = None  # RGB565
        return new


class TextureRegion:
    @classmethod
    def load(cls, file):
        new = cls.__new__(cls)
        new.name = file.read(32).decode('cp1252')
        new.left = read_int32(file)
        new.top = read_int32(file)
        new.right = read_int32(file)
        new.bottom = read_int32(file)
        new.index = read_int32(file)
        return new


class SectorList:
    def __init__(self):
        self.has_named_faces = False
        self.sectors = []

    def __iter__(self):
        return self.sectors.__iter__()

    @classmethod
    def load(cls, file, key):
        new = cls.__new__(cls)
        has_named_faces = read_int32(file) != 0
        new.sectors = [Sector.load(file, key, has_named_faces) for _ in range(read_int32(file))]
        return new


class Sector:
    def __init__(self):
        self.mesh = None  # type: Mesh
        self.vertex_lights = None
        self.bounding_box_min = None
        self.bounding_box_max = None

    @classmethod
    def load(cls, file, key, has_named_faces):
        new = cls.__new__(cls)
        new.mesh = Mesh.load(file, key, has_named_faces, True)
        new.vertex_lights = file.read(len(new.mesh.positions))
        new.bounding_box_min = read_vec3_f16x16(file)  # z -= 2
        new.bounding_box_max = read_vec3_f16x16(file)  # z += 10
        return new


class Mesh:
    def __init__(self):
        self.positions = None  # type: List[Tuple(float, float, float)]
        self.faces = None  # type: List[MeshFace]
        self.normals = None  # type: List[Tuple(float, float, float)]
        self.unknown = None  # type: int

    @classmethod
    def load(cls, file, key, has_named_faces, has_unk_prop):
        new = cls.__new__(cls)
        new.positions = [read_vec3_f16x16(file) for _ in range(read_int32(file))]
        face_count = read_int32(file)
        _ = read_int32(file)  # tri_count
        _ = read_int32(file)  # quad_count
        new.faces = [MeshFace.load(file, key, has_named_faces, has_unk_prop) for _ in range(face_count)]
        new.normals = [read_vec3_f16x16(file) for _ in range(len(new.positions))]
        new.unknown = read_int32(file)  # Color?
        return new


class MeshFace:
    def __init__(self):
        self.name = None  # type: str
        self.indices = None  # type: List[int, int, int, int]
        self.vertex_count = None  # type: int
        self.normal = None  # type: tuple(float, float, float)
        self.material_name = None  # type: str
        self.face_color = None
        self.texture_index = None
        self.texture_uvs = None
        self.reserved = None
        self.quad_reserved = None
        self.unk_prop = None
        self.properties = None
        self.unk_reserved = None

    @classmethod
    def load(cls, file, key, has_name, has_unk_prop):
        new = cls.__new__(cls)
        if has_name:
            new.name = pbdf.read_string(file)
        if key == 0x00005CA8:
            new.indices = [0, 0, 0, 0]
            new.indices[3] = read_int32(file)
            new.indices[0] = read_int32(file)
            new.vertex_count = read_int32(file)
            new.indices[2] = read_int32(file)
            new.indices[1] = read_int32(file)
        else:
            new.vertex_count = read_int32(file)
            new.indices = read_int32s(file, 4)
        new.normal = read_vec3_f16x16(file)
        new.material_name = pbdf.read_string(file)
        if new.material_name in ["FLAT", "GOURAUD"]:
            new.face_color = read_int32(file)
        else:
            new.texture_index = read_int32(file)
        new.texture_uvs = [read_vec2_uint32(file) for _ in range(4)]
        new.reserved = read_int32(file)  # Color?
        if new.vertex_count == 4:
            new.quad_reserved = read_vec3_f16x16(file)
        if any(new.normal):
            if has_unk_prop:
                new.unk_prop = read_int32(file)  # byte
            new.properties = read_int32(file)  # byte[3]
        else:
            new.unk_reserved = read_int32(file)
        return new


def read_vec2_uint32(file):
    return read_uint32s(file, 2)


def read_vec3_f16x16(file):
    return read_f16x16s(file, 3)
