using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim1Sector : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public uint Index { get; set; }

        public uint Unknown1 { get; set; }

        public uint Unknown2 { get; set; }

        public uint Unknown3 { get; set; }

        public uint Unknown4 { get; set; }

        public IList<Vector2U> Vectors { get; set; }

        public Vector3F Position { get; set; }

        public Matrix3 Rotation { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Index = loader.ReadUInt32();
            Unknown1 = loader.ReadUInt32(); // as UInt16
            Unknown2 = loader.ReadUInt32(); // as UInt16
            Unknown3 = loader.ReadUInt32(); // as UInt16
            Unknown4 = loader.ReadUInt32(); // as UInt16
            int vectorCount = loader.ReadInt32();
            Vectors = loader.ReadMany(vectorCount, () => loader.ReadVector2U());
            Position = loader.ReadVector3F16x16();
            Rotation = loader.ReadMatrix3F16x16();
        }
    }
}
