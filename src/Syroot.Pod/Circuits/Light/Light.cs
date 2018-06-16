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
    }
}