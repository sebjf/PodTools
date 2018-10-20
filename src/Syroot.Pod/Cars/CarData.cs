using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Syroot.BinaryData;
using Syroot.Pod.IO;
using Syroot.Maths;

using uint8_t = System.SByte;
using uint32_t = System.UInt32;
using fp1616_t = Syroot.Maths.Vector3F;

namespace Syroot.Pod.Cars
{
    public class CarData : IData<Car>
    {
        public List<CarDataObject> DataObjects;

        public int PodVersion { get; set; }

        public void Load(DataLoader<Car> loader, object parameter = null)
        {
            DataObjects = loader.LoadMany<CarDataObject>(5).ToList();

            uint8_t[] unknown_0438 = loader.ReadSBytes(4).ToArray();
            uint32_t unknown_043C = loader.ReadUInt32();
            uint8_t[] unknown_0440 = loader.ReadSBytes(8).ToArray();
            uint8_t Unknown_0448 = loader.ReadSByte();
            uint8_t[] unknown_0449 = loader.ReadSBytes(2).ToArray();
            uint8_t Unknown_044B = loader.ReadSByte();
            uint8_t[] Unknown_044C = loader.ReadSBytes(36).ToArray();
            uint8_t[] unknown_0470 = loader.ReadSBytes(4).ToArray();
            uint32_t Unknown_0474 = loader.ReadUInt32();
            uint8_t[] unknown_0478 = loader.ReadSBytes(4).ToArray();
            uint32_t Unknown_047C = loader.ReadUInt32();
            uint32_t Unknown_0480 = loader.ReadUInt32();
            uint32_t Unknown_0484 = loader.ReadUInt32();
            uint32_t Unknown_0488 = loader.ReadUInt32();
            uint32_t Unknown_048C = loader.ReadUInt32();

            {
                //POD1: this block is 4 bytes larger
                uint8_t[] unknown_0490 = loader.ReadSBytes(100).ToArray();
                uint32_t unknown_04F4 = loader.ReadUInt32();
                uint8_t[] unknown_04F8 = loader.ReadSBytes(28).ToArray();
                uint32_t Unknown_0514 = loader.ReadUInt32();
                //...
            }

            uint32_t Unknown_0518 = loader.ReadUInt32();
            uint32_t Unknown_051C = loader.ReadUInt32();
            uint32_t Unknown_0520 = loader.ReadUInt32();
            uint8_t[] unknown_0524 = loader.ReadSBytes(4).ToArray();
            uint32_t Unknown_0528 = loader.ReadUInt32();
            uint8_t[] unknown_052C = loader.ReadSBytes(12).ToArray();
            uint32_t Unknown_0538 = loader.ReadUInt32();
            uint32_t Unknown_053C = loader.ReadUInt32();
            uint32_t Unknown_0540 = loader.ReadUInt32();
            uint32_t Unknown_0544 = loader.ReadUInt32();
            uint32_t Unknown_0548 = loader.ReadUInt32();
            uint32_t Unknown_054C = loader.ReadUInt32();  //POD1: 00000002, POD2: 00000001

            switch (Unknown_054C)
            {
                case 1:
                    (parameter as CarFileInfo).PodVersion = Cars.PodVersion.POD2;
                    break;
                case 2:
                    (parameter as CarFileInfo).PodVersion = Cars.PodVersion.POD1;
                    break;
            }

            uint8_t[] unknown_0550 = loader.ReadSBytes(8).ToArray();

            uint32_t[] List = loader.ReadUInt32s((int)loader.ReadUInt32());
        }

        public void Save(DataSaver<Car> saver, object parameter = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
