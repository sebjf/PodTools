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

            Good.RearR = loader.Load<Mesh>(parameters);
            Good.RearL = loader.Load<Mesh>(parameters);
            Good.SideR = loader.Load<Mesh>(parameters);
            Good.SideL = loader.Load<Mesh>(parameters);
            Good.FrontR = loader.Load<Mesh>(parameters);
            Good.FrontL = loader.Load<Mesh>(parameters);
            Damaged.RearR = loader.Load<Mesh>(parameters);
            Damaged.RearL = loader.Load<Mesh>(parameters);
            Damaged.SideR = loader.Load<Mesh>(parameters);
            Damaged.SideL = loader.Load<Mesh>(parameters);
            Damaged.FrontR = loader.Load<Mesh>(parameters);
            Damaged.FrontL = loader.Load<Mesh>(parameters);
            Ruined.RearR = loader.Load<Mesh>(parameters);
            Ruined.RearL = loader.Load<Mesh>(parameters);
            Ruined.SideR = loader.Load<Mesh>(parameters);
            Ruined.SideL = loader.Load<Mesh>(parameters);
            Ruined.FrontR = loader.Load<Mesh>(parameters);
            Ruined.FrontL = loader.Load<Mesh>(parameters);

            Wheels.FrontR = loader.Load<Mesh>(parameters);
            Wheels.RearR = loader.Load<Mesh>(parameters);
            Wheels.FrontL = loader.Load<Mesh>(parameters);
            Wheels.RearL = loader.Load<Mesh>(parameters);

            parameters.HasPrism = false;

            ShadowGood.Front = loader.Load<Mesh>(parameters);

            if ((parameter as CarFileInfo).PodVersion != PodVersion.POD1)
            {
                ShadowGood.Rear = loader.Load<Mesh>(parameters);
                ShadowRuined.Front = loader.Load<Mesh>(parameters);
                ShadowRuined.Rear = loader.Load<Mesh>(parameters);
            }

            CollisionMeshes = loader.LoadMany<CollisionMesh>(2).ToArray();
        }

        public void Save(DataSaver<Car> saver, object parameter = null)
        {
            throw new NotImplementedException();
        }
    }
}
