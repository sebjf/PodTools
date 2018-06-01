import struct

_unpack_byte = struct.Struct("B").unpack
_unpack_int32 = struct.Struct("I").unpack
_pack_int32 = struct.Struct("I").pack


def read_byte(file):
    return _unpack_byte(file.read(1))[0]


def read_int32(file):
    return _unpack_int32(file.read(4))[0]


def read_int32s(file, count):
    return struct.unpack(str(count) + "I", file.read(4 * count))


def write_int32(file, value):
    file.write(_pack_int32(value))
