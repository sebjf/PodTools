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

        public EnvironmentSection EnvironmentSection { get; set; }

        public LightSection LightSection { get; set; }

        public Anim1Section Anim1Section { get; set; }

        public SoundSection SoundSection { get; set; }

        public Background Background { get; set; }

        public Sky Sky { get; set; }

        public Anim2SectionList Anim2Sections { get; set; }

        public RepairZoneSection RepairZoneSection { get; set; }

        public Designation DesignationForward { get; set; }

        public DifficultySection DifficultyForwardEasy { get; set; }
        public LevelSection LevelForwardEasy { get; set; } 

        public DifficultySection DifficultyForwardNormal { get; set; }
        public LevelSection LevelForwardNormal { get; set; }

        public DifficultySection DifficultyForwardHard { get; set; }
        public LevelSection LevelForwardHard { get; set; }

        public Designation DesignationReverse { get; set; }
        public DifficultySection DifficultyReverseEasy { get; set; }
        public LevelSection LevelReverseEasy { get; set; }

        public DifficultySection DifficultyReverseNormal { get; set; }
        public LevelSection LevelReverseNormal { get; set; }

        public DifficultySection DifficultyReverseHard { get; set; }
        public LevelSection LevelReverseHard { get; set; }

        public CompetitorSection CompetitorsEasy { get; set; }
        public CompetitorSection CompetitorsNormal { get; set; }
        public CompetitorSection CompetitorsHard { get; set; }

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
            // Load general data.
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
            EnvironmentSection = loader.LoadSection<EnvironmentSection>();
            LightSection = loader.LoadSection<LightSection>();
            Anim1Section = loader.LoadSection<Anim1Section>();
            SoundSection = loader.LoadSection<SoundSection>();
            Background = loader.Load<Background>();
            Sky = loader.Load<Sky>();
            Anim2Sections = loader.Load<Anim2SectionList>();
            RepairZoneSection = loader.LoadSection<RepairZoneSection>();

            // Load forward specifications.
            DesignationForward = loader.Load<Designation>();

            DifficultyForwardEasy = loader.LoadDifficultySection<DifficultySection>();
            LevelForwardEasy = loader.LoadSection<LevelSection>();

            loader.Position = Offsets[(int)Offset.DifficultyForwardNormal];
            DifficultyForwardNormal = loader.LoadDifficultySection<DifficultySection>();
            LevelForwardNormal = loader.LoadSection<LevelSection>();

            loader.Position = Offsets[(int)Offset.DifficultyForwardHard];
            DifficultyForwardHard = loader.LoadDifficultySection<DifficultySection>();
            LevelForwardHard = loader.LoadSection<LevelSection>();

            // Load reverse specifications.
            loader.Position = Offsets[(int)Offset.DesignationReverse];
            DesignationReverse = loader.Load<Designation>();

            DifficultyReverseEasy = loader.LoadDifficultySection<DifficultySection>();
            LevelReverseEasy = loader.LoadSection<LevelSection>();

            loader.Position = Offsets[(int)Offset.DifficultyReverseNormal];
            DifficultyReverseNormal = loader.LoadDifficultySection<DifficultySection>();
            LevelReverseNormal = loader.LoadSection<LevelSection>();

            loader.Position = Offsets[(int)Offset.DifficultyReverseHard];
            DifficultyReverseHard = loader.LoadDifficultySection<DifficultySection>();
            LevelReverseHard = loader.LoadSection<LevelSection>();

            // Load competitors.
            loader.Position = Offsets[(int)Offset.CompetitorsEasy];
            CompetitorsEasy = loader.LoadDifficultySection<CompetitorSection>();
            loader.Position = Offsets[(int)Offset.CompetitorsNormal];
            CompetitorsNormal = loader.LoadDifficultySection<CompetitorSection>();
            loader.Position = Offsets[(int)Offset.CompetitorsHard];
            CompetitorsHard = loader.LoadDifficultySection<CompetitorSection>();
        }

        // ---- CLASSES, STRUCTS & ENUMS -------------------------------------------------------------------------------

        private enum Offset
        {
            Default,
            DifficultyForwardNormal,
            DifficultyForwardHard,
            DesignationReverse,
            DifficultyReverseNormal,
            DifficultyReverseHard,
            CompetitorsEasy,
            CompetitorsNormal,
            CompetitorsHard
        }
    }
}
