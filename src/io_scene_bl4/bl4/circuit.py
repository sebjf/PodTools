from .. import pbdf
from ..binary import *


class Circuit:
    @classmethod
    def load(cls, file, block_size):
        offsets = pbdf.read_header_offsets(file, block_size)
        file.seek(offsets[0])

        # Header
        if read_int32(file) != 3:
            raise AssertionError("Check value is not 3")
        read_int32(file)

        # Events
        event_count = read_int32(file)
        event_buffer_size = read_int32(file)
        cls.events = [Event.load(file) for _ in range(event_count)]

        # Macros
        cls.macros = MacroList.load(file)

        cls.track_name = pbdf.read_string(file)
        print(cls.track_name)
        cls.texture_lod = read_int32s(file, 16)
        cls.project_name = pbdf.read_string(file)
        print(cls.project_name)


class Event:
    @classmethod
    def load(cls, file):
        cls.name = pbdf.read_string(file)
        cls.siz_param = read_int32(file)
        cls.num_param = read_int32(file)
        cls.param_data = file.read(cls.siz_param * cls.num_param)
        return cls


class MacroList:
    @classmethod
    def load(cls, file):
        cls.base = [Macro.load(file, 1) for _ in range(read_int32(file))]
        cls.main = [Macro.load(file, 1) for _ in range(read_int32(file))]
        cls.init = [Macro.load(file, 1) for _ in range(read_int32(file))]
        cls.active = [Macro.load(file, 1) for _ in range(read_int32(file))]
        cls.desactive = [Macro.load(file, 1) for _ in range(read_int32(file))]
        cls.remplace = [Macro.load(file, 2) for _ in range(read_int32(file))]
        cls.echange = [Macro.load(file, 2) for _ in range(read_int32(file))]
        return cls


class Macro:
    @classmethod
    def load(cls, file, value_count):
        cls.values = read_int32s(file, value_count)
        return cls
