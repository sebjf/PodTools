using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Syroot.BinaryData;
using Syroot.Pod.IO;
using Syroot.Maths;

namespace Syroot.Pod.Cars
{
    public class Characteristics : IData<Car>
    {
        UInt32 Acceleration;
        UInt32 Brakes;
        UInt32 Grip;
        UInt32 Handling;
        UInt32 Speed;

        public void Load(DataLoader<Car> loader, object parameter = null)
        {
            Acceleration = loader.ReadUInt32();
            Brakes = loader.ReadUInt32();
            Grip = loader.ReadUInt32();
            Handling = loader.ReadUInt32();
            Speed = loader.ReadUInt32();
        }

        public void Save(DataSaver<Car> saver, object parameter = null)
        {
            throw new NotImplementedException();
        }
    }
}
