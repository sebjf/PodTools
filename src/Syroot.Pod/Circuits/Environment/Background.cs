using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Background : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public int FogDistance { get; set; }

        public int FogIntensity { get; set; }

        public int BackDepth { get; set; }

        public int BackBottom { get; set; }

        public bool Visible { get; set; }

        public uint Color { get; set; }

        public string Name { get; set; }

        public TextureList Textures { get; set; }

        public int YStart { get; set; }

        public int YEnd { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            FogDistance = loader.ReadInt32();
            FogIntensity = loader.ReadInt32();
            BackDepth = loader.ReadInt32();
            BackBottom = loader.ReadInt32();
            Visible = loader.ReadBoolean(BooleanCoding.Dword);
            Color = loader.ReadUInt32();
            Name = loader.ReadPodString();
            Textures = loader.Load<TextureList>(256);
            YStart = loader.ReadInt32();
            YEnd = loader.ReadInt32();
        }
    }
}