using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim2ObjectKey : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public int Looping { get; set; }

        public int FaceType { get; set; }

        public int FaceIndex { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Looping = loader.ReadInt32();
            FaceType = loader.ReadInt32();
            FaceIndex = loader.ReadInt32();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(Looping);
            saver.WriteInt32(FaceType);
            saver.WriteInt32(FaceIndex);
        }
    }
}