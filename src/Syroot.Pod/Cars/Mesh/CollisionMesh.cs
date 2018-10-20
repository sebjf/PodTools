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
    public class CollisionMesh : IData<Car>
    {
        public string Name;
        public Mesh Mesh;

        public void Load(DataLoader<Car> loader, object parameter = null)
        {
            Name = loader.ReadPodString();

            MeshFaceParameters parameters = new MeshFaceParameters();
            parameters.HasNamedFaces = loader.ReadUInt32() > 0;
            parameters.HasPrism = true;

            Mesh = loader.Load<Mesh<Car>>(parameters);

            var Unknown_001C = loader.ReadUInt32();
            var Unknown_0024 = loader.ReadUInt32s(3);
            var Unknown_003C = loader.ReadUInt32();
            var Unknown_0020 = loader.ReadUInt32();
            var Unknown_0030 = loader.ReadUInt32();
            var Unknown_00B8 = loader.ReadSBytes((int)Unknown_0030 * 64);

        }

        public void Save(DataSaver<Car> saver, object parameter = null)
        {
            throw new NotImplementedException();
        }
    }
}
