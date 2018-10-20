﻿using System.Collections.Generic;
using System.IO;

namespace Syroot.Pod.IO
{
    /// <summary>
    /// Represents the abstract base class for any file using the <see cref="Pbdf"/> encryption.
    /// </summary>
    /// <remarks>The class name is redundant on purpose to clarify class meaning, reading "Pod Binary Data File File" if
    /// fully expanded.</remarks>
    public abstract class PbdfFile
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="PbdfFile"/> class.
        /// </summary>
        public PbdfFile() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PbdfFile"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to load data from.</param>
        public PbdfFile(string fileName, object parameter = null)
            : this(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read), parameter) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PbdfFile"/> class from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load data from.</param>
        public PbdfFile(Stream stream, object parameter = null)
        {
            Parameter = parameter;

            // Retrieve encryption secrets.
            Key = Pbdf.RetrieveKey(stream);
            stream.Position = 0;
            BlockSize = Pbdf.RetrieveBlockSize(stream, Key);
            stream.Position = 0;

            // Decrypt header and data into a temporary stream.
            byte[] buffer = new byte[stream.Length - (stream.Length / BlockSize * sizeof(uint))];
            using (MemoryStream decStream = new MemoryStream(buffer))
            {
                Pbdf.Decrypt(stream, decStream, Key, BlockSize);

                // Read the header and adjusted offsets.
                decStream.Position = 0;
                Offsets = Pbdf.ReadHeader(decStream, BlockSize);
                int headerSize = (int)decStream.Position;

                // Create a stream view on the data excluding the header.
                using (MemoryStream dataStream = new MemoryStream(buffer, headerSize, buffer.Length - headerSize))
                    LoadData(dataStream);
            }
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public object Parameter; 

        /// <summary>
        /// Gets the XOR encryption key to use when saving data.
        /// </summary>
        public uint Key { get; protected set; }

        /// <summary>
        /// Gets the block size to use when saving data.
        /// </summary>
        public int BlockSize { get; protected set; }

        /// <summary>
        /// Gets the list of offsets at which specific data is stored. They are relative to the start of the data, e.g.
        /// starting behind the file header, and are adjusted upon loading and saving.
        /// </summary>
        public IList<int> Offsets { get; protected set; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Saves the data of the instance in the file with the given <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to save data in.</param>
        public void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                Save(stream);
        }

        /// <summary>
        /// Saves the data of the instance in the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to save data in.</param>
        public void Save(Stream stream)
        {
            // Compose the decrypted file contents in a temporary stream.
            using (MemoryStream decStream = new MemoryStream())
            {
                // Write out the file data into a temporary stream.
                Offsets = new List<int>();
                using (MemoryStream dataStream = new MemoryStream())
                {
                    SaveData(dataStream);
                    Pbdf.WriteHeader(decStream, BlockSize, Offsets, (int)dataStream.Position);
                    dataStream.Position = 0;
                    dataStream.CopyTo(decStream);
                }
                // Encrypt the data into the provided stream.
                decStream.Position = 0;
                Pbdf.Encrypt(decStream, stream, Key, BlockSize);
            }
        }

        // ---- METHODS (PROTECTED) ------------------------------------------------------------------------------------

        /// <summary>
        /// Loads instance data from the given <paramref name="stream"/> storing the decrypted data (without the
        /// header).
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load the data from.</param>
        protected abstract void LoadData(Stream stream);

        /// <summary>
        /// Saves instance data (without the header) in the given <paramref name="stream"/>. The list of
        /// <see cref="Offsets"/> should be filled to store offsets into the written data.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to save the data to.</param>
        protected abstract void SaveData(Stream stream);
    }
}
