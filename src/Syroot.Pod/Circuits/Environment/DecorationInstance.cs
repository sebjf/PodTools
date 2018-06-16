using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class DecorationInstance : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public uint Index { get; set; }

        public Vector2U[] Vectors { get; set; }

        public Vector3F Position { get; set; }

        public Matrix3 Rotation { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Index = loader.ReadUInt32();
            Vectors = loader.ReadMany(loader.ReadInt32(), () => loader.ReadVector2U());
            Position = loader.ReadVector3F16x16();
            Rotation = loader.ReadMatrix3F16x16();
        }
    }
}