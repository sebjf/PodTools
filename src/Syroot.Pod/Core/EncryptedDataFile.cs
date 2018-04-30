using System;
using System.IO;
using Syroot.BinaryData;

namespace Syroot.Pod.Core
{
    /// <summary>
    /// Represents the contents and the encryption configuration of a Pod game data file.
    /// </summary>
    public abstract class EncryptedDataFile
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedDataFile"/> class.
        /// </summary>
        public EncryptedDataFile()
        {
            Reset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedDataFile"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to create the instance from.</param>
        public EncryptedDataFile(string fileName)
        {
            Load(fileName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedDataFile"/> class from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to create the instance from.</param>
        /// <param name="leaveOpen"><c>true</c> to leave <paramref name="stream"/> open after creating the instance.
        /// </param>
        public EncryptedDataFile(Stream stream, bool leaveOpen = false)
        {
            Load(stream, leaveOpen);
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the XOR key used to encrypt or decrypt data with.
        /// </summary>
        protected virtual uint Key { get; private set; }

        /// <summary>
        /// Gets or sets the size of a data chunk at which end a checksum follows.
        /// </summary>
        protected virtual int BlockSize { get; private set; }

        /// <summary>
        /// Gets or sets the offsets in the file header. When loaded, they are adjusted to the decrypted data which
        /// excludes the header, and will be adjusted to the encrypted data which includes the header upon saving.
        /// </summary>
        protected uint[] Offsets { get; set; }
        
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Loads the instance data from the file with the given <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to load the instance data from.</param>
        public void Load(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Load(fileStream);
            }
        }

        /// <summary>
        /// Loads the instance data from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load the instance data from.</param>
        /// <param name="leaveOpen"><c>true</c> to leave <paramref name="stream"/> open after loading the instance.
        /// </param>
        public void Load(Stream stream, bool leaveOpen = false)
        {
            try
            {
                Reset();
                // Get the key and block size required to decrypt the data.
                RetrieveKey(stream);
                RetrieveBlockSize(stream);
                // Retrieve adjusted offsets in the file header and store the remaining data.
                using (MemoryStream outStream = new MemoryStream())
                {
                    CryptData(stream, outStream, false);
                    outStream.Position = sizeof(int); // Skip file size.
                    ReadOffsets(outStream);
                    LoadData(outStream);
                }
            }
            finally
            {
                if (!leaveOpen)
                    stream.Dispose();
            }
        }

        /// <summary>
        /// Saves the instance data in the file with the given <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to save the instance data in.</param>
        public void Save(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                Save(fileStream);
            }
        }

        /// <summary>
        /// Saves the instance data in the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to save the instance data in.</param>
        /// <param name="leaveOpen"><c>true</c> to leave <paramref name="stream"/> open after saving the instance.
        /// </param>
        public void Save(Stream stream, bool leaveOpen = false)
        {
            try
            {
                using (MemoryStream inStream = new MemoryStream())
                {
                    // Compose the data to encrypt.
                    inStream.Position = sizeof(int); // Reserve space for file size.
                    WriteOffsets(inStream);
                    SaveData(inStream);
                    inStream.Align(BlockSize - sizeof(uint));
                    inStream.SetLength(inStream.Position);
                    // Write file size.
                    int blockCount = (int)inStream.Length / BlockSize + 1;
                    int encryptedLength = (int)inStream.Length + sizeof(uint) * blockCount; // Pad for checksums.
                    inStream.Position = 0;
                    inStream.Write(encryptedLength);
                    // Encrypt and store the data, write the reserved file size.
                    inStream.Position = 0;
                    CryptData(inStream, stream, true);
                }
            }
            finally
            {
                if (!leaveOpen)
                    stream.Dispose();
            }
        }

        // ---- METHODS (PROTECTED) ------------------------------------------------------------------------------------

        /// <summary>
        /// Called before data is loaded to reset properties to default values.
        /// </summary>
        protected virtual void Reset()
        {
            Key = default(uint);
            BlockSize = default(int);
            Offsets = null;
        }

        /// <summary>
        /// Loads strongly typed data from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> storing the raw data, positioned behind the file header.
        /// </param>
        protected abstract void LoadData(Stream stream);

        /// <summary>
        /// Saves strongly typed data in the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> in which to store the raw data, positioned behind the file
        /// header.</param>
        protected abstract void SaveData(Stream stream);

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private void CryptData(Stream inStream, Stream outStream, bool encrypting)
        {
            int blockNumber = 0;
            while (!inStream.IsEndOfStream())
            {
                // Process a block.
                uint checksum = 0;
                if (blockNumber == 0 || (Key != 0x00005CA8 && Key != 0x0000D13F))
                {
                    // First block and most keys always use the default XOR encryption.
                    for (int i = 0; i < BlockSize / 4 - 1; i++)
                    {
                        uint inValue = inStream.ReadUInt32();
                        uint outValue = inValue ^ Key;
                        outStream.Write(outValue);
                        unchecked { checksum += encrypting ? inValue : outValue; }
                    }
                }
                else
                {
                    // Starting with the second block, specific keys use a special encryption.
                    uint lastValue = 0;
                    for (int i = 0; i < BlockSize / 4 - 1; i++)
                    {
                        uint outValue = 0;
                        uint keyValue = 0;
                        uint inValue = inStream.ReadUInt32();
                        switch (lastValue >> 16 & 0b00000011)
                        {
                            case 0: keyValue = lastValue - 0x50A4A89D; break;
                            case 1: keyValue = 0x3AF70BC4 - lastValue; break;
                            case 2: keyValue = (lastValue + 0x07091971) << 1; break;
                            case 3: keyValue = (0x11E67319 - lastValue) << 1; break;
                        }
                        switch (lastValue & 0b00000011)
                        {
                            case 0: outValue = ~inValue ^ keyValue; break;
                            case 1: outValue = ~inValue ^ ~keyValue; break;
                            case 2: outValue = inValue ^ ~keyValue; break;
                            case 3: outValue = inValue ^ keyValue ^ 0x0000FFFF; break;
                        }
                        outStream.Write(outValue);
                        lastValue = encrypting ? outValue : inValue;
                        unchecked { checksum += encrypting ? inValue : outValue; }
                    }
                }
                // Validate or write the checksum.
                if (encrypting)
                {
                    outStream.Write(checksum);
                }
                else
                {
                    if (checksum != inStream.ReadUInt32())
                        throw new InvalidDataException("Invalid checksum.");
                }
                blockNumber++;
            }

            outStream.Position = 0;
        }

        private void RetrieveKey(Stream stream)
        {
            // The XOR encryption key can be reversed since the file header always contains the file size first.
            Key = stream.ReadUInt32() ^ (uint)stream.Length;
            stream.Position = 0;
        }

        private void RetrieveBlockSize(Stream stream)
        {
            // Retrieve the block size in which a checksum is stored at the end of each block.
            uint checksum = 0;
            while (!stream.IsEndOfStream())
            {
                // If the checksum was found, check if this would be a possible block size.
                uint value = stream.ReadUInt32();
                if (value == checksum && stream.Length % stream.Position == 0)
                {
                    BlockSize = (int)stream.Position;
                    stream.Position = 0;
                    return;
                }
                // The checksum is the overflowing sum of all decrypted 32-bit values.
                unchecked { checksum += value ^ Key; }
            }
            throw new InvalidDataException("Could not determine block size.");
        }

        private void ReadOffsets(Stream stream)
        {
            Offsets = stream.ReadUInt32s((int)stream.ReadUInt32());
            // Adjust the offsets to point to decrypted data positions.
            uint headerSize = Offsets[0];
            for (int i = 0; i < Offsets.Length; i++)
            {
                uint offset = Offsets[i];
                offset -= (uint)(offset / BlockSize * sizeof(uint));
                offset -= headerSize;
                Offsets[i] = offset;
            }
        }

        private void WriteOffsets(Stream stream)
        {
            stream.Write(Offsets.Length);
            // Adjust the offsets to point to encrypted data positions.
            uint headerSize = (uint)((2 + Offsets.Length) * sizeof(uint));
            int blockDataSize = BlockSize - sizeof(uint);
            for (int i = 0; i < Offsets.Length; i++)
            {
                uint offset = Offsets[i];
                offset += headerSize;
                offset += (uint)(offset / blockDataSize * sizeof(uint));
                stream.Write(offset);
            }
        }
    }
}
