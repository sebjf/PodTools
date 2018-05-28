using System.IO;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod
{
    /// <summary>
    /// Represents BBM files.
    /// </summary>
    public class FrameBitmap : PbdfFile
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private const int _blockSize = 0x00002000;
        private const uint _key = 0x00000F3A;

        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBitmap"/> class.
        /// </summary>
        public FrameBitmap() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBitmap"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to load data from.</param>
        public FrameBitmap(string fileName) : base(fileName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBitmap"/> class from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load data from.</param>
        public FrameBitmap(Stream stream) : base(stream) { }

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

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        protected override void LoadData(Stream stream)
        {
            FrameTime = stream.ReadUInt32();
            WhiteBackground = stream.ReadBoolean();
            uint bitmapDataSize = stream.ReadUInt32();
            Data = new FrameBitmapData(stream.ReadBytes((int)bitmapDataSize));
        }

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
