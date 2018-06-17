using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class DifficultyConstraint : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public int DesignationIndex { get; set; }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            DesignationIndex = loader.ReadInt32();
            Unknown1 = loader.ReadInt32();
            Unknown2 = loader.ReadInt32();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(DesignationIndex);
            saver.WriteInt32(Unknown1);
            saver.WriteInt32(Unknown2);
        }
    }
}