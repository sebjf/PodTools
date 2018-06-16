using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class Decoration : IData<Circuit>
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }

        public string ObjectName { get; set; }

        public TextureList Textures { get; set; }

        public Mesh Mesh { get; set; }

        public Vector3F CollisionPrism1 { get; set; }

        public uint CollisionPrism2 { get; set; }

        public Vector3F CollisionPrism3 { get; set; }

        public uint Unknown1 { get; set; }

        public Vector3U Unknown2 { get; set; }

        public uint Unknown3 { get; set; }

        public uint Unknown4 { get; set; }

        public IList<DecorationContact> Contacts { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            Name = loader.ReadPodString();
            ObjectName = loader.ReadPodString();
            Textures = loader.Load<TextureList>(128);
            bool hasNamedFaces = loader.ReadBoolean(BooleanCoding.Dword);
            Mesh = loader.Load<Mesh>(new MeshFaceParameters
            {
                HasNamedFaces = hasNamedFaces,
                HasUnkProperty = false
            });
            CollisionPrism1 = loader.ReadVector3F16x16();
            CollisionPrism2 = loader.ReadUInt32();
            CollisionPrism3 = loader.ReadVector3F16x16();
            Unknown1 = loader.ReadUInt32();
            Unknown2 = loader.ReadVector3U();
            Unknown3 = loader.ReadUInt32();
            Unknown4 = loader.ReadUInt32();
            Contacts = loader.LoadMany<DecorationContact>(loader.ReadInt32()).ToList();
        }
    }
}