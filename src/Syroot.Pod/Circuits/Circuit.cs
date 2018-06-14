using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    /// <summary>
    /// Represents BL4 files.
    /// </summary>
    public class Circuit : PbdfFile, IData<Circuit>
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private const int _blockSize = 0x00004000;
        private const uint _key = 0x00000F7E;

        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Circuit"/> class.
        /// </summary>
        public Circuit() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Circuit"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to load data from.</param>
        public Circuit(string fileName) : base(fileName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Circuit"/> class from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load data from.</param>
        public Circuit(Stream stream) : base(stream) { }

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
        public TextureList Textures { get; set; }
        public bool HasNamedSectorFaces { get; set; }
        public IList<Sector> Sectors { get; set; }
        public IList<Visibility> Visibilities { get; set; }
        public EnvironmentSection Environment { get; set; }
        public LightSection Lights { get; set; }

        // ---- METHODS (PROTECTED) ------------------------------------------------------------------------------------

        protected override void LoadData(Stream stream)
        {
            new DataLoader<Circuit>(stream, this).Execute();
        }

        protected override void SaveData(Stream stream)
        {
            throw new NotImplementedException();
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private void LoadEventAndMacros(DataLoader<Circuit> loader)
        {
            int count = loader.ReadInt32();
            uint bufferSize = loader.ReadUInt32();
            Events = loader.LoadMany<Event>(count).ToList();

            MacrosBase = loader.LoadMany<Macro>(loader.ReadInt32(), 3).ToList();
            Macros = loader.LoadMany<Macro>(loader.ReadInt32(), 1).ToList();
            MacrosInit = loader.LoadMany<Macro>(loader.ReadInt32(), 1).ToList();
            MacrosActive = loader.LoadMany<Macro>(loader.ReadInt32(), 1).ToList();
            MacrosInactive = loader.LoadMany<Macro>(loader.ReadInt32(), 1).ToList();
            MacrosReplace = loader.LoadMany<Macro>(loader.ReadInt32(), 2).ToList();
            MacrosExchange = loader.LoadMany<Macro>(loader.ReadInt32(), 2).ToList();
        }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            uint checksum = loader.ReadUInt32(); // must be 3
            uint reserved = loader.ReadUInt32(); // not used
            LoadEventAndMacros(loader);
            TrackName = loader.ReadPodString();
            LevelOfDetail = loader.ReadUInt32s(16);
            ProjectName = loader.ReadPodString();
            Textures = loader.Load<TextureList>(256);
            HasNamedSectorFaces = loader.ReadBoolean(BooleanCoding.Dword);
            Sectors = loader.LoadMany<Sector>(loader.ReadInt32()).ToList();
            Visibilities = loader.LoadMany<Visibility>(loader.ReadInt32()).ToList();
            Environment = loader.LoadSection<EnvironmentSection>();
            Lights = loader.LoadSection<LightSection>();
        }
    }
}
