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
            int paramSize = loader.ReadInt32();
            int paramCount = loader.ReadInt32();
            ParamData = new byte[paramSize][];
            for (int i = 0; i < paramSize; i++)
                ParamData[i] = loader.ReadBytes(paramCount);
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WritePodString(Name);
            saver.WriteInt32(ParamData.Length);
            if (ParamData.Length == 0)
                saver.WriteInt32(0);
            else
                saver.WriteInt32(ParamData[0].Length);
            foreach (byte[] param in ParamData)
                saver.WriteBytes(param);
        }
    }
}
