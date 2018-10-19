using Syroot.BinaryData;
using Syroot.Pod.IO;
using Syroot.Maths;

namespace Syroot.Pod.Circuits
{
    public class DesignationStart : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public Vector3F Data { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Data = loader.ReadVector3F16x16();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteVector3F16x16(Data);
        }
    }
}