using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class DecorationContact : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public byte[] Data { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Data = loader.ReadBytes(64);
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteBytes(Data);
        }
    }
}