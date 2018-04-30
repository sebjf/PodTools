using System;
using System.Collections.Generic;
using System.IO;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.Core;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents BL4 files.
    /// </summary>
    public class Circuit : EncryptedDataFile
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private byte[] _unparsedData;

        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Circuit"/> class.
        /// </summary>
        public Circuit() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Circuit"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to create the instance from.</param>
        public Circuit(string fileName) : base(fileName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Circuit"/> class from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to create the instance from.</param>
        /// <param name="leaveOpen"><c>true</c> to leave <paramref name="stream"/> open after creating the instance.
        /// </param>
        public Circuit(Stream stream, bool leaveOpen = false) : base(stream, leaveOpen) { }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public IList<Event> Events { get; set; }

        public IList<Macro> MacrosBase { get; set; }
        public IList<Macro> Macros { get; set; }
        public IList<Macro> MacrosInit { get; set; }
        public IList<Macro> MacrosActive { get; set; }
        public IList<Macro> MacrosInactive { get; set; }
        public IList<Macro> MacrosReplace { get; set; }
        public IList<Macro> MacrosExchange { get; set; }

        public string TrackName { get; set; }
        public IList<uint> LevelOfDetail { get; set; }
        public string ProjectName { get; set; }

        public IList<Texture> Textures { get; set; }

        public Model Model { get; set; }

        /// <summary>
        /// Gets or sets the XOR key used to encrypt or decrypt data with.
        /// </summary>
        protected override uint Key => 0x00000F7E; // ????? Why not return actual key instead of this hard-coded one?

        /// <summary>
        /// Gets or sets the size of a data chunk at which end a checksum follows.
        /// </summary>
        protected override int BlockSize => 0x00004000; // ????? Same here

        // ---- METHODS (PROTECTED) ------------------------------------------------------------------------------------

        /// <summary>
        /// Called before data is loaded to reset properties to default values.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();
        }

        /// <summary>
        /// Loads strongly typed data from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> storing the raw data, positioned behind the file header.
        /// </param>
        protected override void LoadData(Stream stream)
        {
            uint reserved1 = stream.ReadUInt32(); // always 3
            uint reserved2 = stream.ReadUInt32(); // not used

            LoadEvents(stream);
            LoadMacros(stream);

            TrackName = stream.ReadPodString();
            LevelOfDetail = stream.ReadUInt32s(16);
            ProjectName = stream.ReadPodString();

            LoadTextures(stream);
            LoadModel(stream);

            _unparsedData = stream.ReadBytes((int)(stream.Length - stream.Position));
        }

        /// <summary>
        /// Saves strongly typed data in the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> in which to store the raw data, positioned behind the file
        /// header.</param>
        protected override void SaveData(Stream stream)
        {
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private void LoadEvents(Stream stream)
        {
            int count = stream.ReadInt32();
            uint bufferSize = stream.ReadUInt32();

            Events = new List<Event>(count);
            while (count-- > 0)
            {
                Event evnt = new Event();
                Events.Add(evnt);

                evnt.Name = stream.ReadPodString();
                uint paramSize = stream.ReadUInt32();
                int paramCount = stream.ReadInt32();
                evnt.ParamData = new byte[paramSize][];
                for (int i = 0; i < paramSize; i++)
                    evnt.ParamData[i] = stream.ReadBytes(paramCount);
            }
        }

        private void LoadMacros(Stream stream)
        {
            int countBase = stream.ReadInt32();
            MacrosBase = new List<Macro>(countBase);
            while (countBase-- > 0)
                MacrosBase.Add(new Macro(stream.ReadUInt32s(3)));

            int count = stream.ReadInt32();
            Macros = new List<Macro>(count);
            while (count-- > 0)
                Macros.Add(new Macro(stream.ReadUInt32s(1)));

            int countInit = stream.ReadInt32();
            MacrosInit = new List<Macro>(countInit);
            while (countInit-- > 0)
                MacrosInit.Add(new Macro(stream.ReadUInt32s(1)));

            int countActive = stream.ReadInt32();
            MacrosActive = new List<Macro>(countActive);
            while (countActive-- > 0)
                MacrosActive.Add(new Macro(stream.ReadUInt32s(1)));

            int countInactive = stream.ReadInt32();
            MacrosInactive = new List<Macro>(countInactive);
            while (countInactive-- > 0)
                MacrosInactive.Add(new Macro(stream.ReadUInt32s(1)));

            int countReplace = stream.ReadInt32();
            MacrosReplace = new List<Macro>(countReplace);
            while (countReplace-- > 0)
                MacrosReplace.Add(new Macro(stream.ReadUInt32s(2)));

            int countExchange = stream.ReadInt32();
            MacrosExchange = new List<Macro>(countExchange);
            while (countExchange-- > 0)
                MacrosExchange.Add(new Macro(stream.ReadUInt32s(2)));
        }

        private void LoadTextures(Stream stream)
        {
            int count = stream.ReadInt32();
            uint reserved3 = stream.ReadUInt32();

            Textures = new List<Texture>(count);
            while (count-- > 0)
            {
                Texture texture = new Texture();
                Textures.Add(texture);

                // Read areas.
                int areaCount = stream.ReadInt32();
                texture.Areas = new List<TextureArea>(areaCount);
                while (areaCount-- > 0)
                {
                    texture.Areas.Add(new TextureArea
                    {
                        Name = stream.ReadFixedString(32),
                        Left = stream.ReadUInt32(),
                        Top = stream.ReadUInt32(),
                        Right = stream.ReadUInt32(),
                        Bottom = stream.ReadUInt32(),
                        Index = stream.ReadUInt32()
                    });
                }
            }
            foreach (Texture texture in Textures)
                texture.Data = stream.ReadUInt16s(256 * 256);
        }

        private void LoadModel(Stream stream)
        {
            Model = new Model();
            Model.HasNamedFaces = stream.ReadBoolean(BooleanCoding.Dword);

            // Read meshes.
            int count = stream.ReadInt32();
            Model.Meshes = new List<Mesh>(count);
            while (count-- > 0)
            {
                Mesh mesh = new Mesh();
                Model.Meshes.Add(mesh);

                int vertexCount = stream.ReadInt32();
                mesh.Positions = stream.ReadMany(vertexCount, () => stream.ReadVector3U());
                int faceCount = stream.ReadInt32();
                int triCount = stream.ReadInt32();
                int quadCount = stream.ReadInt32();

                // Read faces.
                mesh.Faces = new List<Face>(faceCount);
                while (faceCount-- > 0)
                {
                    Face face = new Face();
                    mesh.Faces.Add(face);

                    if (Model.HasNamedFaces)
                        face.Name = stream.ReadPodString();

                    uint faceVertexCount; // Warning: Not stored with face
                    if (Key == 0x00005CA8)
                    {
                        face.Indices[3] = stream.ReadUInt32();
                        face.Indices[0] = stream.ReadUInt32();
                        faceVertexCount = stream.ReadUInt32();
                        face.Indices[2] = stream.ReadUInt32();
                        face.Indices[1] = stream.ReadUInt32();
                    }
                    else
                    {
                        faceVertexCount = stream.ReadUInt32();
                        for (int i = 0; i < 4; i++)
                            face.Indices[i] = stream.ReadUInt32();
                    }

                    face.Normal = stream.ReadVector3F16x16();
                    face.MaterialType = stream.ReadPodString();
                    face.ColorOrTexIndex = stream.ReadUInt32();
                    for (int i = 0; i < 4; i++)
                        face.TexCoords[i] = stream.ReadVector2U();
                    face.Reserved = stream.ReadUInt32();
                    if (faceVertexCount == 4)
                        face.QuadReserved = stream.ReadVector3F16x16();
                    if (face.Normal == Vector3U.Zero)
                    {
                        // invalid
                        face.Reserved = stream.ReadUInt32(); // Warning: overriding Reserved from above
                    }
                    else
                    {
                        face.Unknown = stream.ReadUInt32();
                        face.Properties = stream.ReadUInt32();
                    }
                }

                mesh.Normals = stream.ReadMany(vertexCount, () => stream.ReadVector3F16x16());
                mesh.Unknown = stream.ReadUInt32();

                mesh.VertexLights = stream.ReadBytes(vertexCount);
                mesh.BoundingBoxMin = stream.ReadVector3F16x16();
                mesh.BoundingBoxMax = stream.ReadVector3F16x16();
            }
        }
    }
}
