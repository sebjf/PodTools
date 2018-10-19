using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Syroot.Pod.Circuits;
using Syroot.Pod.IO;
using SJF.Pod.Converter;

namespace Syroot.Pod.Scratchpad
{
    internal class Program
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private static readonly string[] _competitorNames = new[]
        {
            "svfn",
            "pnzr667",
            "Cyantusk",
            "Darkriot",
            "Kenno",
            "glhrmfrt",
            "katywing"
        };

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void Main(string[] args)
        {
            string inFolder = @"C:\Users\Sebastian\Dropbox\Projects\POD\Original Tracks\AlderOEM";
            string outFolder = @"D:\Archive\Games\Pod\Installation\Data\Binary\Circuits";

            foreach (string inFilePath in Directory.GetFiles(inFolder, "*.bl4"))
            {
                // Ignore partly broken files.
                if (inFilePath.Contains("Arcade++"))
                    continue;
                if (inFilePath.Contains("Forest"))
                    continue;

                if (!inFilePath.Contains("BELTANE.BL4"))
                    continue;

                string fileName = Path.GetFileName(inFilePath);

                Circuit circuit = new Circuit(inFilePath);

                Converter converter = new Converter();
                converter.Add(circuit);
                converter.Save(@"C:\Users\Sebastian\Documents\Unity Projects\PoD\Assets\Circuits\Beltane\Beltane.obj");


            }
        }

        private static void PlayWithTrack(Circuit circuit)
        {
            circuit.Background.Visible = true;
            circuit.Sky.Visible = true;

            // Remove invisible walls, make remaining polygons solid.
            foreach (Sector sector in circuit.Sectors)
            {
                Mesh mesh = sector.Mesh;
                foreach (MeshFace face in mesh.Faces)
                {
                    if ((face.Properties & 0b1) == 0)
                    {
                        // If not visible, not collidable!
                        face.Properties = 0;
                    }
                    else if ((face.Properties & 0b101000) == 0)
                    {
                        // If not road, make road.
                        // TODO: Calculate normal to decide if wall or road.
                        face.Properties |= 0b1000;
                        face.Unknown = 0;
                    }
                }
            }
            // Make sectors visibile and collidable at all times.
            for (int i = 0; i < circuit.Visibilities.Count; i++)
            {
                // Only set up uncollidable sectors, as cross sections crash the game otherwise?
                //if (circuit.Visibilities[i].VisibleSectorIndices == null)
                //{
                    IList<int> indices = Enumerable.Range(0, circuit.Visibilities.Count).Where(x => x != i).ToList();
                    circuit.Visibilities[i].VisibleSectorIndices = indices;
                //}
            }

            // Rename opponents to POD Discord server members.
            for (int i = 0; i < 7; i++)
            {
                if (circuit.CompetitorsEasy != null)
                    circuit.CompetitorsEasy.Competitors[i].Name = _competitorNames[i];
                if (circuit.CompetitorsNormal != null)
                    circuit.CompetitorsNormal.Competitors[i].Name = _competitorNames[i];
                if (circuit.CompetitorsHard != null)
                    circuit.CompetitorsHard.Competitors[i].Name = _competitorNames[i];
            }
        }

        private static void ResaveTracks()
        {
            string folder = @"D:\Archive\Games\Pod\Installation\Data\Binary\Circuits";
            string newFolder = @"D:\Pictures\Circuits_new";
            Directory.CreateDirectory(newFolder);

            foreach (string filePath in Directory.GetFiles(folder, "*.bl4"))
            {
                // Partly broken files.
                if (filePath.Contains("Arcade++") || filePath.Contains("Forest"))
                    continue;

                string fileName = Path.GetFileName(filePath);
                string newFilePath = Path.Combine(newFolder, fileName);

                Console.WriteLine($"Loading {fileName}...");
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                using (FileStream newFile = new FileStream(newFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Circuit circuit = new Circuit(file);
                    circuit.Save(newFile);
                }
            }
        }

        private static void DecryptTracks(string inFolder, string outFolder)
        {
            Directory.CreateDirectory(outFolder);

            foreach (string inFilePath in Directory.GetFiles(inFolder, "*.bl4"))
            {
                string fileName = Path.GetFileName(inFilePath);
                string outFilePath = Path.Combine(outFolder, fileName.Replace(".bl4", ".dec.bl4"));

                Console.WriteLine($"Decrypting {fileName}...");
                using (FileStream inFile = new FileStream(inFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                using (FileStream outFile = new FileStream(outFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Pbdf.Decrypt(inFile, outFile, 0x00000F7E, 0x00004000);
                }
            }
        }

        private static void EncryptAllTracks()
        {
            string decFolder = @"D:\Pictures\Circuits";
            string encFolder = @"D:\Archive\Games\Pod\Installation\Data\Binary\Circuits";

            Directory.CreateDirectory(decFolder);
            Directory.CreateDirectory(encFolder);

            foreach (string decFilePath in Directory.GetFiles(decFolder, "*.dec.bl4"))
            {
                string fileName = Path.GetFileName(decFilePath);
                string encFilePath = Path.Combine(encFolder, fileName.Replace("dec.bl4", "bl4"));

                Console.WriteLine($"Updating {encFilePath}...");
                using (FileStream decFile = new FileStream(decFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                using (FileStream encFile = new FileStream(encFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Pbdf.Encrypt(decFile, encFile, 0x00000F7E, 0x00004000);
                }
            }
        }

        private static void ReEncryptAllTracks()
        {
            string circuitFolder = @"D:\Archive\Games\Pod\Installation\Data\Binary\Circuits\Backup";
            string decFolder = @"D:\Pictures\Circuits";
            string encFolder = @"D:\Archive\Games\Pod\Installation\Data\Binary\Circuits";

            Directory.CreateDirectory(decFolder);
            Directory.CreateDirectory(encFolder);

            foreach (string filePath in Directory.GetFiles(circuitFolder, "*.bl4"))
            {
                string fileName = Path.GetFileName(filePath);
                string decFilePath = Path.ChangeExtension(Path.Combine(decFolder, fileName), "dec.bl4");
                string encFilePath = Path.Combine(encFolder, fileName);

                Console.WriteLine($"Updating {fileName}...");
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                using (FileStream decFile = new FileStream(decFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                using (FileStream encFile = new FileStream(encFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    uint key = Pbdf.RetrieveKey(file);
                    file.Position = 0;
                    int blockSize = Pbdf.RetrieveBlockSize(file, key);
                    file.Position = 0;
                    Pbdf.Decrypt(file, decFile, key, blockSize);
                    decFile.Position = 0;
                    Pbdf.Encrypt(decFile, encFile, key, blockSize);
                }
            }
        }

        private static void TestRawPbdf()
        {
            RawPbdf rawPbdf = new RawPbdf(@"D:\Archive\Games\Pod\Installation\Data\Binary\Circuits\Backup\Heaven.bl4");
            rawPbdf.Save(@"D:\Pictures\new.bl4");
        }
    }

    internal class RawPbdf : PbdfFile
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private IList<int> _offsets;

        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        public RawPbdf(string fileName) : base(fileName) { }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public byte[] Data { get; set; }

        // ---- METHODS (PROTECTED) ------------------------------------------------------------------------------------

        protected override void LoadData(Stream stream)
        {
            _offsets = Offsets;
            Data = new byte[stream.Length];
            stream.Read(Data, 0, Data.Length);
        }

        protected override void SaveData(Stream stream)
        {
            Offsets = _offsets;
            stream.Write(Data, 0, Data.Length);
        }
    }
}
