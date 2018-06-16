using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class DifficultyConfig2 : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public IList<int> Unknown3 { get; set; }

        public int Unknown4 { get; set; }

        public IList<int> Unknown5 { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Unknown1 = loader.ReadInt32();
            Unknown2 = loader.ReadInt32();
            Unknown3 = loader.ReadInt32s(loader.ReadInt32());
            Unknown4 = loader.ReadInt32();
            Unknown5 = loader.ReadInt32s(loader.ReadInt32());
        }
    }
}