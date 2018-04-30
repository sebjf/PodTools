using System;
using System.IO;
using Syroot.BinaryData;
using Syroot.Pod.Core;

namespace Syroot.Pod
{
    /// <summary>
    /// Represents BMD files.
    /// </summary>
    public class MessageDictionary : EncryptedDataFile
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDictionary"/> class.
        /// </summary>
        public MessageDictionary() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDictionary"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to create the instance from.</param>
        public MessageDictionary(string fileName) : base(fileName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDictionary"/> class from the given
        /// <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to create the instance from.</param>
        /// <param name="leaveOpen"><c>true</c> to leave <paramref name="stream"/> open after creating the instance.
        /// </param>
        public MessageDictionary(Stream stream, bool leaveOpen = false) : base(stream, leaveOpen) { }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the XOR key used to encrypt or decrypt data with.
        /// </summary>
        protected override uint Key
        {
            get { return 0x0000EA1E; }
        }

        /// <summary>
        /// Gets or sets the size of a data chunk at which end a checksum follows.
        /// </summary>
        protected override int BlockSize
        {
            get { return 0x00000200; }
        }

        // ---- METHODS (PROTECTED) ------------------------------------------------------------------------------------

        /// <summary>
        /// Loads strongly typed data from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> storing the raw data, positioned behind the file header.
        /// </param>
        protected override void LoadData(Stream stream)
        {
            uint typeCount = stream.ReadUInt32();
            uint textCount = stream.ReadUInt32();
            uint textBufferSize = stream.ReadUInt32();
            byte[] textBuffer = stream.ReadBytes((int)textBufferSize);
        }

        /// <summary>
        /// Saves strongly typed data in the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> in which to store the raw data, positioned behind the file
        /// header.</param>
        protected override void SaveData(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
