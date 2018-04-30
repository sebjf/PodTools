using System.IO;
using Syroot.BinaryData;
using Syroot.Pod.Core;

namespace Syroot.Pod
{
    /// <summary>
    /// Represents BBM files.
    /// </summary>
    public class FrameBitmap : EncryptedDataFile
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBitmap"/> class.
        /// </summary>
        public FrameBitmap() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBitmap"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to create the instance from.</param>
        public FrameBitmap(string fileName) : base(fileName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBitmap"/> class from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to create the instance from.</param>
        /// <param name="leaveOpen"><c>true</c> to leave <paramref name="stream"/> open after creating the instance.
        /// </param>
        public FrameBitmap(Stream stream, bool leaveOpen = false) : base(stream, leaveOpen) { }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Gets or sets the frame time in milliseconds.
        /// </summary>
        public uint FrameTime { get; set; }

        /// <summary>
        /// Gets or sets a value whether the image has a white background or a black one.
        /// </summary>
        public bool WhiteBackground { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FrameBitmapData"/> instance stored by this file.
        /// </summary>
        public FrameBitmapData Data { get; set; }
        
        /// <summary>
        /// Gets or sets the XOR key used to encrypt or decrypt data with.
        /// </summary>
        protected override uint Key
        {
            get { return 0x00000F3A; }
        }

        /// <summary>
        /// Gets or sets the size of a data chunk at which end a checksum follows.
        /// </summary>
        protected override int BlockSize
        {
            get { return 0x00002000; }
        }

        // ---- METHODS (PROTECTED) ------------------------------------------------------------------------------------

        /// <summary>
        /// Called before data is loaded to reset properties to default values.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();
            FrameTime = default(uint);
            WhiteBackground = default(bool);
            Data = null;
        }

        /// <summary>
        /// Loads strongly typed data from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> storing the raw data, positioned behind the file header.
        /// </param>
        protected override void LoadData(Stream stream)
        {
            FrameTime = stream.ReadUInt32();
            WhiteBackground = stream.ReadBoolean();
            uint bitmapDataSize = stream.ReadUInt32();
            Data = new FrameBitmapData(stream.ReadBytes((int)bitmapDataSize));
        }

        /// <summary>
        /// Saves strongly typed data in the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> in which to store the raw data, positioned behind the file
        /// header.</param>
        protected override void SaveData(Stream stream)
        {
            stream.Write(FrameTime);
            stream.Write(WhiteBackground);
            byte[] dataBytes = Data.ToBytes();
            stream.Write(dataBytes.Length);
            stream.Write(dataBytes);
        }
    }
}
