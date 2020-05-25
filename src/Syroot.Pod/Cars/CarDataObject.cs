using System.Collections.Generic;
using System.IO;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;
using Syroot.Maths;

using uint8_t = System.SByte;
using uint32_t = System.UInt32;
using fp1616_t = Syroot.Maths.Vector3F;

namespace Syroot.Pod.Cars
{
    public class CarDataObject : IData<Car>
    {
        uint8_t[]  Unknown_0000; //[57];
        uint8_t    Unknown_0039;
        uint8_t[]  Unknown_003A; //[2];
        uint32_t   Unknown_003C;
        uint32_t   Unknown_0040;
        uint32_t   Unknown_0044;
        public fp1616_t   Position;     // [0..3].xy, -[4].z // < what does this mean? possibly mirror in z? or offset by chassis?
        uint32_t   Unknown_0054;
        uint8_t[]  Unknown_0058; //[56];
        uint32_t   Unknown_0090;
        uint32_t   Unknown_0094;
        uint32_t   Unknown_0098;
        uint32_t   Unknown_009C;
        uint32_t   Unknown_00A0;
        uint8_t[]  Unknown_00A4; //[8];
        uint32_t   Unknown_00AC;
        uint32_t   Unknown_00B0;
        uint32_t   Unknown_00B4;
        uint32_t   Unknown_00B8;
        uint32_t   Unknown_00BC;
        uint32_t   Unknown_00C0;
        uint8_t[]  Unknown_00C4; //[20];

        public void Load(DataLoader<Car> loader, object parameter = null)
        {
            Unknown_0000 = loader.ReadSBytes(57).ToArray();
            Unknown_0039 = loader.ReadSByte();
            Unknown_003A = loader.ReadSBytes(2).ToArray();
            Unknown_003C = loader.ReadUInt32();
            Unknown_0040 = loader.ReadUInt32();
            Unknown_0044 = loader.ReadUInt32();
            Position = loader.ReadVector3F16x16();
            Unknown_0054 = loader.ReadUInt32();
            Unknown_0058 = loader.ReadSBytes(56).ToArray();
            Unknown_0090 = loader.ReadUInt32();
            Unknown_0094 = loader.ReadUInt32();
            Unknown_0098 = loader.ReadUInt32();
            Unknown_009C = loader.ReadUInt32();
            Unknown_00A0 = loader.ReadUInt32();
            Unknown_00A4 = loader.ReadSBytes(8).ToArray();
            Unknown_00AC = loader.ReadUInt32();
            Unknown_00B0 = loader.ReadUInt32();
            Unknown_00B4 = loader.ReadUInt32();
            Unknown_00B8 = loader.ReadUInt32();
            Unknown_00BC = loader.ReadUInt32();
            Unknown_00C0 = loader.ReadUInt32();
            Unknown_00C4 = loader.ReadSBytes(20).ToArray();
        }

        public void Save(DataSaver<Car> saver, object parameter = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
