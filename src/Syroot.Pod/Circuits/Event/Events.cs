using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Event : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public byte[][] ParamData { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Name = loader.ReadPodString();
            uint paramSize = loader.ReadUInt32();
            int paramCount = loader.ReadInt32();
            ParamData = new byte[paramSize][];
            for (int i = 0; i < paramSize; i++)
                ParamData[i] = loader.ReadBytes(paramCount);
        }
    }
}
