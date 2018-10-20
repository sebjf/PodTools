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
    public class Body
    {
        public Mesh RearL;
        public Mesh RearR;
        public Mesh SideL;
        public Mesh SideR;
        public Mesh FrontL;
        public Mesh FrontR;
    }

    public class Wheels
    {
        public Mesh FrontL;
        public Mesh FrontR;
        public Mesh RearL;
        public Mesh RearR;
    }

    public class Shadows
    {
        public Mesh Front;
        public Mesh Rear;
    }

    public class Objects : IData<Car>
    {
        public Body Good;
        public Body Damaged;
        public Body Ruined;

        public Wheels Wheels;

        public Shadows ShadowGood;
        public Shadows ShadowRuined;

        public CollisionMesh[] CollisionMeshes;

        public Objects()
        {
            Good = new Body();
            Damaged = new Body();
            Ruined = new Body();
            Wheels = new Wheels();
            ShadowGood = new Shadows();
            ShadowRuined = new Shadows();
        }

        public void Load(DataLoader<Car> loader, object parameter = null)
        {
            MeshFaceParameters parameters = new MeshFaceParameters();
            parameters.HasNamedFaces = loader.ReadUInt32() > 0;
            parameters.HasPrism = true;

            Good.RearR = loader.Load<Mesh<Car>>(parameters);
            Good.RearL = loader.Load<Mesh<Car>>(parameters);
            Good.SideR = loader.Load<Mesh<Car>>(parameters);
            Good.SideL = loader.Load<Mesh<Car>>(parameters);
            Good.FrontR = loader.Load<Mesh<Car>>(parameters);
            Good.FrontL = loader.Load<Mesh<Car>>(parameters);
            Damaged.RearR = loader.Load<Mesh<Car>>(parameters);
            Damaged.RearL = loader.Load<Mesh<Car>>(parameters);
            Damaged.SideR = loader.Load<Mesh<Car>>(parameters);
            Damaged.SideL = loader.Load<Mesh<Car>>(parameters);
            Damaged.FrontR = loader.Load<Mesh<Car>>(parameters);
            Damaged.FrontL = loader.Load<Mesh<Car>>(parameters);
            Ruined.RearR = loader.Load<Mesh<Car>>(parameters);
            Ruined.RearL = loader.Load<Mesh<Car>>(parameters);
            Ruined.SideR = loader.Load<Mesh<Car>>(parameters);
            Ruined.SideL = loader.Load<Mesh<Car>>(parameters);
            Ruined.FrontR = loader.Load<Mesh<Car>>(parameters);
            Ruined.FrontL = loader.Load<Mesh<Car>>(parameters);

            Wheels.FrontR = loader.Load<Mesh<Car>>(parameters);
            Wheels.RearR = loader.Load<Mesh<Car>>(parameters);
            Wheels.FrontL = loader.Load<Mesh<Car>>(parameters);
            Wheels.RearL = loader.Load<Mesh<Car>>(parameters);

            parameters.HasPrism = false;

            ShadowGood.Front = loader.Load<Mesh<Car>>(parameters);

            if (loader.Instance.PodVersion != PodVersion.POD1)
            {
                ShadowGood.Rear = loader.Load<Mesh<Car>>(parameters);
                ShadowRuined.Front = loader.Load<Mesh<Car>>(parameters);
                ShadowRuined.Rear = loader.Load<Mesh<Car>>(parameters);
            }

            CollisionMeshes = loader.LoadMany<CollisionMesh>(2).ToArray();
        }

        public void Save(DataSaver<Car> saver, object parameter = null)
        {
            throw new NotImplementedException();
        }
    }
}
