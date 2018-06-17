using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents a mesh which visibility is toggled to increase performance.
    /// </summary>
    public class Sector : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public Mesh Mesh { get; set; }

        public IList<byte> VertexGamma { get; set; }

        public Vector3F BoundingBoxMin { get; set; } // Z -= 2

        public Vector3F BoundingBoxMax { get; set; } // Z += 10

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Mesh = loader.Load<Mesh>(new MeshFaceParameters
            {
                HasNamedFaces = loader.Instance.HasNamedSectorFaces,
                HasUnkProperty = true
            });
            VertexGamma = loader.ReadBytes(Mesh.Positions.Count);
            BoundingBoxMin = loader.ReadVector3F16x16();
            BoundingBoxMax = loader.ReadVector3F16x16();
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.Save(Mesh, new MeshFaceParameters
            {
                HasNamedFaces = saver.Instance.HasNamedSectorFaces,
                HasUnkProperty = true
            });
            saver.WriteBytes(VertexGamma);
            saver.WriteVector3F16x16(BoundingBoxMin);
            saver.WriteVector3F16x16(BoundingBoxMax);
        }
    }
}
