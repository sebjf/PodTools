using System;
using System.IO;
using Syroot.Pod.Circuits;
using Syroot.Pod.IO;

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
            PlayWithTrack();
        }

        private static void PlayWithTrack()
        {
            string fileName = "Beltane";
            string backupFilePath = $@"D:\Archive\Games\Pod\Installation\Data\Binary\Circuits\Backup\{fileName}.bl4";
            string filePath = $@"D:\Archive\Games\Pod\Installation\Data\Binary\Circuits\{fileName}.bl4";
            string decFilePath = $@"D:\Pictures\Circuits\dec_new\{fileName}.dec.bl4";

            Circuit circuit = new Circuit(backupFilePath);
            for (int i = 0; i < 7; i++)
            {
                circuit.CompetitorsEasy.Competitors[i].Name = _competitorNames[i];
                circuit.CompetitorsNormal.Competitors[i].Name = _competitorNames[i];
                circuit.CompetitorsHard.Competitors[i].Name = _competitorNames[i];
            }
            circuit.Save(filePath);

            // Save a decrypted copy for analyzation purposes.
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (FileStream outStream = new FileStream(decFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Pbdf.Decrypt(stream, outStream, 0x00000F7E, 0x00004000);
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
    }

    internal class RawPbdf : PbdfFile
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        public RawPbdf(string fileName) : base(fileName) { }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public byte[] Data { get; set; }

        // ---- METHODS (PROTECTED) ------------------------------------------------------------------------------------

        protected override void LoadData(Stream stream)
        {
            Data = new byte[stream.Length];
            stream.Read(Data, 0, (int)stream.Length);
        }

        protected override void SaveData(Stream stream)
        {
            stream.Write(Data, 0, Data.Length);
        }
    }
}
