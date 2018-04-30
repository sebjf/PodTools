using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Syroot.Maths;

namespace Syroot.Pod.Circuits
{
    public class Model
    {
        public bool HasNamedFaces;
        public IList<Mesh> Meshes;
    }

    public class Mesh
    {
        public IList<Vector3U> Positions;
        public IList<Face> Faces;
        public IList<Vector3F> Normals;
        public uint Unknown;
        public IList<byte> VertexLights;
        public Vector3F BoundingBoxMin; // Z -= 2
        public Vector3F BoundingBoxMax; // Z += 10
    }

    [DebuggerDisplay(nameof(Face) + " {Name}")]
    public class Face
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        public Face()
        {
            Indices = new uint[4];
            TexCoords = new Vector2U[4];
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }
        public uint[] Indices { get; }
        public Vector3F Normal { get; set; }
        public string MaterialType { get; set; }
        public uint ColorOrTexIndex { get; set; }
        public Vector2U[] TexCoords { get; }
        public uint Reserved { get; set; }
        public Vector3F QuadReserved { get; set; }
        public uint Unknown { get; set; }
        public uint Properties { get; set; }
    }
}
