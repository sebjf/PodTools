using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents the surface of a polygonal model.
    /// </summary>
    public class Mesh : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public IList<Vector3U> Positions { get; set; }

        public IList<MeshFace> Faces { get; set; }

        public IList<Vector3F> Normals { get; set; }

        public uint Unknown { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            MeshFaceParameters parameters = (MeshFaceParameters)parameter;

            int vertexCount = loader.ReadInt32();
            Positions = loader.ReadMany(vertexCount, () => loader.ReadVector3U());
            int faceCount = loader.ReadInt32();
            int triCount = loader.ReadInt32();
            int quadCount = loader.ReadInt32();
            Faces = loader.LoadMany<MeshFace>(faceCount, parameters).ToList();
            Normals = loader.ReadMany(vertexCount, () => loader.ReadVector3F16x16());
            Unknown = loader.ReadUInt32();
        }
    }
}
