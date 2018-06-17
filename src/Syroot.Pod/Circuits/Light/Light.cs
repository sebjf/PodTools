using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Light : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public uint Type { get; set; }

        public byte[] Data { get; set; }

        public uint Value2 { get; set; }

        public uint Value3 { get; set; }

        public uint Diffusion { get; set; }

        public uint Values { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Type = loader.ReadUInt32();
            Data = loader.ReadBytes(48);
            Value2 = loader.ReadUInt32();
            Value3 = loader.ReadUInt32();
            Diffusion = loader.ReadUInt32();
            Values = loader.ReadUInt32();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteUInt32(Type);
            saver.WriteBytes(Data);
            saver.WriteUInt32(Value2);
            saver.WriteUInt32(Value3);
            saver.WriteUInt32(Diffusion);
            saver.WriteUInt32(Values);
        }
    }
}