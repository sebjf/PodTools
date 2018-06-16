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
    }
}