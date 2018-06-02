import struct

_pack_int32 = struct.Struct("i").pack
_unpack_byte = struct.Struct("B").unpack
_unpack_int32 = struct.Struct("i").unpack
_unpack_uint16 = struct.Struct("H").unpack
_unpack_uint32 = struct.Struct("I").unpack


def read_byte(file):
    return _unpack_byte(file.read(1))[0]


def read_int32(file):
    return _unpack_int32(file.read(4))[0]


def read_int32s(file, count):
    return struct.unpack(str(count) + "i", file.read(4 * count))


def read_f16x16(file):
    return _unpack_int32(file.read(4))[0] / (1 << 16)


def read_f16x16s(file, count):
    return tuple(i / (1 << 16) for i in struct.unpack(str(count) + "i", file.read(4 * count)))


def read_uint16(file):
    return _unpack_uint16(file.read(2))[0]


def read_uint16s(file, count):
    return struct.unpack(str(count) + "H", file.read(2 * count))


def read_uint32(file):
    return _unpack_uint32(file.read(4))[0]


def read_uint32s(file, count):
    return struct.unpack(str(count) + "I", file.read(4 * count))


def write_int32(file, value):
    file.write(_pack_int32(value))
