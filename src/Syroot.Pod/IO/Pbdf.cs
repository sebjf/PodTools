using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Syroot.BinaryData;

namespace Syroot.Pod.IO
{
    /// <summary>
    /// Represents the PBDF (Pod Binary Data File) encryption and provides methods to decrypt it.
    /// </summary>
    public static class Pbdf
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Determines the XOR encryption key used to decrypt the data available in the given <paramref name="stream"/>.
        /// This changes the current position in the stream.
        /// </summary>
        /// <param name="stream">The <see cref="Data"/> to retrieve the encryption key from.</param>
        /// <returns>The XOR encryption key.</returns>
        public static uint RetrieveKey(Stream stream)
        {
            // The XOR encryption key can be reversed since the file header always contains the file size first.
            return stream.ReadUInt32() ^ (uint)stream.Length;
        }

        /// <summary>
        /// Determines the block size used to decrypt the data available in the given <paramref name="stream"/>. This
        /// changes the current position in the stream.
        /// </summary>
        /// <param name="stream">The <see cref="Data"/> to retrieve the block size from.</param>
        /// <param name="key">The XOR encryption key used to decrypt the block checksum yielding the block size.</param>
        /// <returns>The block size.</returns>
        /// <exception cref="InvalidDataException">The block size could not be determined.</exception>
        public static int RetrieveBlockSize(Stream stream, uint key)
        {
            // Retrieve the block size in which a checksum is stored at the end of each block.
            uint checksum = 0;
            while (!stream.IsEndOfStream())
            {
                // If the checksum was found, check if this would be a possible block size.
                uint value = stream.ReadUInt32();
                if (value == checksum && stream.Length % stream.Position == 0)
                    return (int)stream.Position;
                // The checksum is the overflowing sum of all decrypted 32-bit values.
                unchecked { checksum += value ^ key; }
            }
            throw new InvalidDataException("Could not determine PBDF block size.");
        }

        /// <summary>
        /// Decrypts the data available in the <paramref name="inStream"/> and writes it to the
        /// <paramref name="outStream"/>.
        /// </summary>
        /// <param name="inStream">The input <see cref="Stream"/> storing the encrypted data.</param>
        /// <param name="outStream">The output <see cref="Stream"/> storing the decrypted data.</param>
        /// <param name="key">The XOR encryption key.</param>
        /// <param name="blockSize">The size of a block in bytes at which end a checksum follows.</param>
        public static void Decrypt(Stream inStream, Stream outStream, uint key, int blockSize)
        {
            int blockIndex = 0;
            int blockDataSize = blockSize - sizeof(uint);
            int blockDataDwordCount = blockDataSize / sizeof(uint);
            byte[] block = new byte[blockSize];
            Span<uint> blockDword = MemoryMarshal.Cast<byte, uint>(block); // Little endian
            while (!inStream.IsEndOfStream())
            {
                // Process a block.
                inStream.Read(block, 0, blockSize);
                int i = 0;
                uint checksum = 0;
                if (blockIndex++ == 0 || (key != 0x00005CA8 && key != 0x0000D13F))
                {
                    // First block and most keys always use the default XOR encryption.
                    for (; i < blockDataDwordCount; i++)
                    {
                        ref uint value = ref blockDword[i];
                        unchecked { checksum += (value ^= key); }
                    }
                }
                else
                {
                    // Starting with the second block, specific keys use a special encryption.
                    uint lastValue = 0;
                    for (; i < blockDataDwordCount; i++)
                    {
                        ref uint value = ref blockDword[i];
                        uint keyValue = 0;
                        switch (lastValue >> 16 & 3)
                        {
                            case 0: keyValue = lastValue - 0x50A4A89D; break;
                            case 1: keyValue = 0x3AF70BC4 - lastValue; break;
                            case 2: keyValue = (lastValue + 0x07091971) << 1; break;
                            case 3: keyValue = (0x11E67319 - lastValue) << 1; break;
                        }
                        lastValue = value;
                        switch (lastValue & 3)
                        {
                            case 0: value = ~value ^ keyValue; break;
                            case 1: value = ~value ^ ~keyValue; break;
                            case 2: value = value ^ ~keyValue; break;
                            case 3: value = value ^ keyValue ^ 0xFFFF; break;
                        }
                        unchecked { checksum += value; }
                    }
                }
                // Validate the checksum and write the decrypted block.
                if (checksum != blockDword[i])
                    throw new InvalidDataException("Invalid PBDF block checksum.");
                outStream.Write(block, 0, blockDataSize);
            }
        }

        /// <summary>
        /// Encrypts the data available in the <paramref name="inStream"/> and writes it to the
        /// <paramref name="outStream"/>.
        /// </summary>
        /// <param name="inStream">The input <see cref="Stream"/> storing the decrypted data.</param>
        /// <param name="outStream">The output <see cref="Stream"/> storing the encrypted data.</param>
        /// <param name="key">The XOR encryption key.</param>
        /// <param name="blockSize">The size of a block in bytes at which end a checksum follows.</param>
        public static void Encrypt(Stream inStream, Stream outStream, uint key, int blockSize)
        {
            int blockIndex = 0;
            int blockDataSize = blockSize - sizeof(uint);
            int blockDataDwordCount = blockSize / sizeof(uint) - 1;
            byte[] block = new byte[blockSize];
            Span<uint> blockDword = MemoryMarshal.Cast<byte, uint>(block); // Little endian
            while (!inStream.IsEndOfStream())
            {
                // Process a block.
                inStream.Read(block, 0, blockDataSize);
                int i = 0;
                uint checksum = 0;
                if (blockIndex++ == 0 || (key != 0x00005CA8 && key != 0x0000D13F))
                {
                    // First block and most keys always use the default XOR encryption.
                    for (; i < blockDataDwordCount; i++)
                    {
                        ref uint value = ref blockDword[i];
                        unchecked { checksum += value; }
                        value ^= key;
                    }
                }
                else
                {
                    // Starting with the second block, specific keys use a special encryption.
                    uint lastValue = 0;
                    for (; i < blockDataDwordCount; i++)
                    {
                        ref uint value = ref blockDword[i];
                        unchecked { checksum += value; }
                        uint keyValue = 0;
                        switch (lastValue >> 16 & 3)
                        {
                            case 0: keyValue = lastValue - 0x50A4A89D; break;
                            case 1: keyValue = 0x3AF70BC4 - lastValue; break;
                            case 2: keyValue = (lastValue + 0x07091971) << 1; break;
                            case 3: keyValue = (0x11E67319 - lastValue) << 1; break;
                        }
                        switch (lastValue & 3)
                        {
                            case 0: value = ~value ^ keyValue; break;
                            case 1: value = ~value ^ ~keyValue; break;
                            case 2: value = value ^ ~keyValue; break;
                            case 3: value = value ^ keyValue ^ 0xFFFF; break;
                        }
                        lastValue = value;
                    }
                }
                // Add the checksum and write the encrypted block.
                blockDword[i] = checksum;
                outStream.Write(block, 0, blockSize);
            }
        }

        /// <summary>
        /// Reads the PBDF file header (not checking the included file size) and returns the list of offsets adjusted to
        /// match decrypted data positions.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read decrypted data from.</param>
        /// <param name="blockSize">The size of a block in bytes at which end a checksum follows.</param>
        /// <returns>The list of read and adjusted offsets.</returns>
        public static IList<int> ReadHeader(Stream stream, int blockSize)
        {
            // Read header data.
            stream.ReadInt32(); // File size.

            // Read and adjusts offsets to point to decrypted data positions.
            List<int> offsets = new List<int>(stream.ReadInt32s(stream.ReadInt32()));
            for (int i = 0; i < offsets.Count; i++)
                offsets[i] -= offsets[i] / blockSize * sizeof(uint);
            return offsets;
        }

        /// <summary>
        /// Writes the PBDF file header with adjusted offsets to point to encrypted data positions.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read decrypted data from.</param>
        /// <param name="blockSize">The size of a block in bytes at which end a checksum follows.</param>
        /// <param name="offsets">The list of offsets which will be adjusted to point to encrypted data positions.
        /// </param>
        /// <param name="dataSize">The size of the file data (excluding the header) in bytes.</param>
        public static void WriteHeader(Stream stream, int blockSize, IList<int> offsets, int dataSize)
        {
            // Write header data.
            // Calculate the final file size to write them into the data stream.
            int headerSize = (2 + offsets.Count) * sizeof(int); // FileSize + OffsetCount + Offsets
            int fileSize = headerSize + dataSize;
            int blockCount = dataSize / blockSize + 1; // Adjust to next complete block.
            fileSize += blockCount * sizeof(uint); // Adjust for checksums at the end of each block.
            stream.WriteInt32(fileSize);

            // Adjust and write offsets to point to encrypted data positions.
            int blockDataSize = blockSize - sizeof(uint);
            for (int i = 0; i < offsets.Count; i++)
            {
                int offset = offsets[i];
                offset += headerSize;
                offset += offset / blockDataSize * sizeof(int);
                stream.WriteInt32(offset);
            }
        }
    }
}
