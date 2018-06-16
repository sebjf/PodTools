using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class LevelPoint : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public int PositionIndex { get; set; }

        public int Unknown3 { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Unknown1 = loader.ReadInt32();
            Unknown2 = loader.ReadInt32();
            PositionIndex = loader.ReadInt32();
            Unknown3 = loader.ReadInt32();
        }
    }
}