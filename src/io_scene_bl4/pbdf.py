"""
Provides methods to work with the PBDF (Pod Binary Data File) encryption.
"""
from .binary import *


def retrieve_key(file, file_size):
    """Retrieves the XOR encryption key from the given file.
    Args:
        file: The encrypted input file.
        file_size (int): The size of the input file in bytes.

    Returns:
        int: The XOR encryption key.
    """
    return read_int32(file) ^ file_size


def retrieve_block_size(file, file_size, key):
    """Retrieves the size of blocks the given file is using.
    Args:
        file: The encrypted input file.
        file_size (int): The size of the input file in bytes.
        key (int): The XOR encryption key. Use retrieve_key() if unknown.

    Returns:
        int: The block size in bytes at which end a checksum is placed.
    """
    file_pos = file.tell()
    checksum = 0
    while file_pos < file_size - 4:
        dword = read_uint32(file)
        file_pos = file.tell()
        if dword == checksum & 0xFFFFFFFF and file_size % file_pos == 0:
            return file_pos
        checksum = (checksum + (dword ^ key))
    raise AssertionError("Could not determine PBDF block size.")


def decrypt(in_file, out_file, key, block_size):
    """Decrypts the data in the input file and writes it to the output file.
    Args:
        in_file: The encrypted input file.
        out_file: The output file receiving the decrypted content.
        key (int): The XOR encryption key. Use retrieve_key() if unknown.
        block_size (int): The block size in bytes at which end a checksum is placed.
                          Use retrieve_block_size() if unknown.
    """
    block = memoryview(bytearray(block_size)).cast("I")  # Requires little endian
    block_index = 0
    block_data_dword_count = block_size // 4 - 1
    while True:
        # Process a block.
        if in_file.readinto(block) != block_size:
            break
        checksum = 0
        if block_index == 0 or key not in [0x00005CA8, 0x0000D13F]:
            # First block and most keys always use the default XOR encryption.
            for i in range(block_data_dword_count):
                block[i] ^= key
                checksum += block[i]
        else:
            # Starting with the second block, specific keys use a special encryption.
            last_value = 0
            for i in range(block_data_dword_count):
                key_value = 0
                command = last_value >> 16 & 3
                if command == 0:
                    key_value = last_value - 0x50A4A89D
                elif command == 1:
                    key_value = 0x3AF70BC4 - last_value
                elif command == 2:
                    key_value = (last_value + 0x07091971) << 1
                elif command == 3:
                    key_value = (0x11E67319 - last_value) << 1
                last_value = block[i]
                command = last_value & 3
                if command == 0:
                    block[i] = (~block[i] ^ key_value) & 0xFFFFFFFF
                elif command == 1:
                    block[i] = (~block[i] ^ ~key_value) & 0xFFFFFFFF
                elif command == 2:
                    block[i] = (block[i] ^ ~key_value) & 0xFFFFFFFF
                elif command == 3:
                    block[i] = (block[i] ^ key_value ^ 0xFFFF) & 0xFFFFFFFF
                checksum += block[i]
        # Validate the checksum and write the decrypted block.
        if checksum & 0xFFFFFFFF != block[-1]:
            raise AssertionError("Invalid PBDF block checksum.")
        out_file.write(block[:-1])
        block_index += 1


def encrypt(in_file, out_file, key, block_size):
    """Encrypts the data in the input file and writes it to the output file.
    Args:
        in_file: The decrypted input file.
        out_file: The output file receiving the encrypted content.
        key (int): The XOR encryption key.
        block_size (int): The block size in bytes at which end a checksum is placed.
    """
    block_data_size = block_size - 4
    block = memoryview(bytearray(block_data_size)).cast("I")  # Requires little endian
    block_index = 0
    block_data_dword_count = block_data_size // 4
    while True:
        # Process a block.
        if in_file.readinto(block) != block_data_size:
            break
        checksum = 0
        if block_index == 0 or key not in [0x00005CA8, 0x0000D13F]:
            # First block and most keys always use the default XOR encryption.
            for i in range(block_data_dword_count):
                checksum += block[i]
                block[i] ^= key
        else:
            # Starting with the second block, specific keys use a special encryption.
            last_value = 0
            for i in range(block_data_dword_count):
                checksum += block[i]
                key_value = 0
                command = last_value >> 16 & 3
                if command == 0:
                    key_value = last_value - 0x50A4A89D
                elif command == 1:
                    key_value = 0x3AF70BC4 - last_value
                elif command == 2:
                    key_value = (last_value + 0x07091971) << 1
                elif command == 3:
                    key_value = (0x11E67319 - last_value) << 1
                command = last_value & 3
                if command == 0:
                    block[i] = (~block[i] ^ key_value) & 0xFFFFFFFF
                elif command == 1:
                    block[i] = (~block[i] ^ ~key_value) & 0xFFFFFFFF
                elif command == 2:
                    block[i] = (block[i] ^ ~key_value) & 0xFFFFFFFF
                elif command == 3:
                    block[i] = (block[i] ^ key_value ^ 0xFFFF) & 0xFFFFFFFF
                last_value = block[i]
        # Add the checksum and write the encrypted block.
        out_file.write(block)
        write_int32(out_file, checksum & 0xFFFFFFFF)
        block_index += 1


def read_header_offsets(file, block_size):
    """Reads the PBDF header (not checking the included file size) and returns the list of offsets adjusted to match
    decrypted data positions.
    Args:
        file: The decrypted input file.
        block_size (int): The block size in bytes at which end a checksum is placed.
    Returns:
        The list of offsets, adjusted to point to decrypted file positions.
    """
    _ = read_int32(file)
    num_offsets = read_int32(file)
    offsets = list(read_int32s(file, num_offsets))
    for i in range(num_offsets):
        offsets[i] -= offsets[i] // block_size * 4
    return offsets


def write_header_offsets(file, block_size, offsets, data_size):
    """Writes the PBDF file header with adjusted offsets to point to encrypted data positions.
    Args:
        file: The file to write the header to.
        block_size (int): The block size in bytes at which end a checksum is placed.
        offsets: The list of offsets which will be adjusted to point to encrypted data positions.
        data_size: The size of the file data (excluding the header) in bytes.
    """
    header_size = (2 + len(offsets)) * 4  # file_size + num_offsets + offsets
    file_size = header_size + data_size
    num_blocks = data_size // block_size + 1  # Adjust to next complete block
    file_size += num_blocks * 4  # Adjust for checksums at the end of each block.
    write_int32(file, file_size)

    block_data_size = block_size - 4
    for i in range(offsets):
        offset = offsets[i]
        offset += header_size
        offset += offset // block_data_size * 4
        write_int32(offset, offset)


def read_string(file):
    return bytes(
        (c ^ ~i) & 0xFF for i, c in enumerate(file.read(read_byte(file)))
    ).decode("cp1252")
