using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Cars
{
    /// <summary>
    /// Represents a single polygon of a <see cref="Mesh"/>.
    /// </summary>
    public class MeshFace : IData<Car>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public int FaceVertexCount { get; set; }

        public int[] Indices { get; set; } = new int[4];

        public Vector3F Normal { get; set; }

        public string MaterialType { get; set; }

        public uint ColorOrTexIndex { get; set; }

        public Vector2U[] TexCoords { get; set; } = new Vector2U[4];

        public uint Reserved1 { get; set; }

        public Vector3F QuadReserved { get; set; }

        public uint Reserved2 { get; set; }

        public uint Unknown { get; set; }

        public uint Properties { get; set; }

        public sbyte Flags1;
        public sbyte Flags2;
        public sbyte[] Unknowns;

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Car>.Load(DataLoader<Car> loader, object parameter)
        {
            MeshFaceParameters parameters = (MeshFaceParameters)parameter;

            if (parameters.HasNamedFaces)
            {
                Name = loader.ReadPodString();
            }

            if (loader.Instance.Key == 0x00005CA8)
            {
                Indices[3] = loader.ReadInt32();
                Indices[0] = loader.ReadInt32();
                FaceVertexCount = loader.ReadInt32();
                Indices[2] = loader.ReadInt32();
                Indices[1] = loader.ReadInt32();
            }
            else
            {
                FaceVertexCount = loader.ReadInt32();
                Indices = loader.ReadInt32s(4);
            }

            Normal = loader.ReadVector3F16x16();
            MaterialType = loader.ReadPodString();
            ColorOrTexIndex = loader.ReadUInt32();
            TexCoords = loader.ReadMany(4, () => loader.ReadVector2U());
            Reserved1 = loader.ReadUInt32();
            if (FaceVertexCount == 4)
            {
                QuadReserved = loader.ReadVector3F16x16();
            }

            Flags1 = loader.ReadSByte();
            Flags2 = loader.ReadSByte();
            Unknowns = loader.ReadSBytes(2);
        }

        void IData<Car>.Save(DataSaver<Car> saver, object parameter)
        {
            throw new System.NotImplementedException();
        }
    }

    public struct MeshFaceParameters
    {
        public bool HasNamedFaces;
        public bool HasPrism;
    }
}
