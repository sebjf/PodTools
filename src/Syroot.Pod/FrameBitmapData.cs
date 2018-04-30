using System.IO;
using Syroot.BinaryData;

namespace Syroot.Pod
{
    /// <summary>
    /// Represents the BMP data a 2-dimensional image consists of.
    /// </summary>
    public class FrameBitmapData
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        internal FrameBitmapData(byte[] data)
        {
            Data = data;
            // TODO: Parse the header and create palettes / pixel data.
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the raw data, starting with a BITMAPINFOHEADER structure.
        /// </summary>
        public byte[] Data { get; set; }

        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        internal byte[] ToBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(Data);
                return stream.ToArray();
            }
        }
    }
}
