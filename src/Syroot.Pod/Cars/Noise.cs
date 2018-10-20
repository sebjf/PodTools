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
    public class Noise : IData<Car>
    {
        public UInt32 Runtime;
        public UInt16[] Unknown;
        public UInt16 Reserved;

        public void Load(DataLoader<Car> loader, object parameter = null)
        {
            Runtime = loader.ReadUInt32();
            Unknown = loader.ReadUInt16s(15);
            Reserved = loader.ReadUInt16();
        }

        public void Save(DataSaver<Car> saver, object parameter = null)
        {
            throw new NotImplementedException();
        }
    }
}
