using System.Collections.Generic;

namespace Syroot.Pod.Circuits
{
    public class Texture
    {
        public IList<TextureArea> Areas { get; set; }

        public ushort[] Data { get; set; } // 256x256, RGB565
    }

    public class TextureArea
    {
        public string Name; // 32 characters
        public uint Left;
        public uint Top;
        public uint Right;
        public uint Bottom;
        public uint Index;
    }
}
