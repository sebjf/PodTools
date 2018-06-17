using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Sky : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public bool Visible { get; set; }

        public int YEffect { get; set; }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public int FadeAmount { get; set; }

        public int Speed { get; set; }

        public string Name { get; set; }

        public TextureList Textures { get; set; }

        public ushort[] LensFlareTextureData { get; set; }

        public int Unknown3 { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Visible = loader.ReadBoolean(BooleanCoding.Dword);
            YEffect = loader.ReadInt32();
            Unknown1 = loader.ReadInt32();
            Unknown2 = loader.ReadInt32();
            FadeAmount = loader.ReadInt32();
            Speed = loader.ReadInt32();
            Name = loader.ReadPodString();
            Textures = loader.Load<TextureList>(128);
            LensFlareTextureData = loader.ReadUInt16s(128 * 128);
            Unknown3 = loader.ReadInt32();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteBoolean(Visible, BooleanCoding.Dword);
            saver.WriteInt32(YEffect);
            saver.WriteInt32(Unknown1);
            saver.WriteInt32(Unknown2);
            saver.WriteInt32(FadeAmount);
            saver.WriteInt32(Speed);
            saver.WritePodString(Name);
            saver.Save(Textures);
            saver.WriteUInt16s(LensFlareTextureData);
            saver.WriteInt32(Unknown3);
        }
    }
}