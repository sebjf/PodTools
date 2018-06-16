using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents a single polygon of a <see cref="Mesh"/>.
    /// </summary>
    public class MeshFace : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public uint[] Indices { get; } = new uint[4];

        public Vector3F Normal { get; set; }

        public string MaterialType { get; set; }

        public uint ColorOrTexIndex { get; set; }

        public Vector2U[] TexCoords { get; } = new Vector2U[4];

        public uint Reserved1 { get; set; }

        public Vector3F QuadReserved { get; set; }

        public uint Reserved2 { get; set; }

        public uint Unknown { get; set; }

        public uint Properties { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            MeshFaceParameters parameters = (MeshFaceParameters)parameter;

            if (parameters.HasNamedFaces)
                Name = loader.ReadPodString();

            uint faceVertexCount;
            if (loader.Instance.Key == 0x00005CA8)
            {
                Indices[3] = loader.ReadUInt32();
                Indices[0] = loader.ReadUInt32();
                faceVertexCount = loader.ReadUInt32();
                Indices[2] = loader.ReadUInt32();
                Indices[1] = loader.ReadUInt32();
            }
            else
            {
                faceVertexCount = loader.ReadUInt32();
                for (int i = 0; i < 4; i++)
                    Indices[i] = loader.ReadUInt32();
            }

            Normal = loader.ReadVector3F16x16();
            MaterialType = loader.ReadPodString();
            ColorOrTexIndex = loader.ReadUInt32();
            for (int i = 0; i < 4; i++)
                TexCoords[i] = loader.ReadVector2U();
            Reserved1 = loader.ReadUInt32();
            if (faceVertexCount == 4)
                QuadReserved = loader.ReadVector3F16x16();
            if (Normal == Vector3U.Zero)
            {
                Reserved2 = loader.ReadUInt32();
            }
            else
            {
                if (parameters.HasUnkProperty)
                    Unknown = loader.ReadUInt32();
                Properties = loader.ReadUInt32();
            }
        }
    }

    public struct MeshFaceParameters
    {
        public bool HasNamedFaces;
        public bool HasUnkProperty;
    }
}
