using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class LevelConfig1 : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public Vector3U Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public int Unknown3 { get; set; }

        public IList<uint> Unknown4 { get; set; }

        public IList<uint> Unknown5 { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Unknown1 = loader.ReadVector3U();
            Unknown2 = loader.ReadInt32();
            Unknown3 = loader.ReadInt32();
            Unknown4 = loader.ReadUInt32s(loader.ReadInt32());
            Unknown5 = loader.ReadUInt32s(loader.ReadInt32());
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteVector3U(Unknown1);
            saver.WriteInt32(Unknown2);
            saver.WriteInt32(Unknown3);
            saver.WriteInt32(Unknown4.Count);
            saver.WriteUInt32s(Unknown4);
            saver.WriteInt32(Unknown5.Count);
            saver.WriteUInt32s(Unknown5);
        }
    }
}