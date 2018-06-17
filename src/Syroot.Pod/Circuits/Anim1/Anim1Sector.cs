using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Anim1Sector : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public int Index { get; set; }

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
            Index = loader.ReadInt32();
            Unknown1 = loader.ReadUInt32(); // as UInt16
            Unknown2 = loader.ReadUInt32(); // as UInt16
            Unknown3 = loader.ReadUInt32(); // as UInt16
            Unknown4 = loader.ReadUInt32(); // as UInt16
            Vectors = loader.ReadMany(loader.ReadInt32(), () => loader.ReadVector2U());
            Position = loader.ReadVector3F16x16();
            Rotation = loader.ReadMatrix3F16x16();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(Index);
            saver.WriteUInt32(Unknown1);
            saver.WriteUInt32(Unknown2);
            saver.WriteUInt32(Unknown3);
            saver.WriteUInt32(Unknown4);
            saver.WriteInt32(Vectors.Count);
            saver.WriteMany(Vectors, x => saver.WriteVector2U(x));
            saver.WriteVector3F16x16(Position);
            saver.WriteMatrix3F16x16(Rotation);
        }
    }
}
