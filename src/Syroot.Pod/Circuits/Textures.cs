using System.Collections.Generic;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents 2-dimensional image data possibly storing multiple <see cref="TextureArea"/> instances.
    /// </summary>
    public class Texture
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public IList<TextureArea> Areas { get; set; }

        public ushort[] Data { get; set; } // 256x256, RGB565
    }

    /// <summary>
    /// Represents a frame on a <see cref="Texture"/> which forms its own visual image.
    /// </summary>
    public class TextureArea
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; } // 32 characters
        public uint Left { get; set; }
        public uint Top { get; set; }
        public uint Right { get; set; }
        public uint Bottom { get; set; }
        public uint Index { get; set; }
    }
}
