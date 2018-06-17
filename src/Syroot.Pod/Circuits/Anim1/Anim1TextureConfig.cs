using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim1TextureConfig : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public uint Unknown1 { get; set; }

        public uint Unknown2 { get; set; }

        public uint Unknown3 { get; set; }

        public uint Unknown4 { get; set; }

        public uint Unknown5 { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Unknown1 = loader.ReadUInt32();
            Unknown2 = loader.ReadUInt32();
            Unknown3 = loader.ReadUInt32();
            Unknown4 = loader.ReadUInt32();
            Unknown5 = loader.ReadUInt32();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteUInt32(Unknown1);
            saver.WriteUInt32(Unknown2);
            saver.WriteUInt32(Unknown3);
            saver.WriteUInt32(Unknown4);
            saver.WriteUInt32(Unknown5);
        }
    }
}