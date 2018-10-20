using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Syroot.Pod.IO;
using SJF.Pod.Converter;

namespace Syroot.Pod.Scratchpad
{
    internal class Program
    {
        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void Main(string[] args)
        {
            ReadCars();            
        }

        private static void ReadCars()
        {
            string inFolder = @"D:\Program Files\GOG Games\POD GOLD\DATA\BINARY\VOITURES\";

            foreach (string inFilePath in Directory.GetFiles(inFolder, "*.bv4"))
            {
                // Ignore partly broken files.
                if (inFilePath.Contains("Arcade++"))
                    continue;

                if (!inFilePath.Contains("ALIEN.BV4"))
                    continue;

                string fileName = Path.GetFileName(inFilePath);

                var car = new Cars.Car(inFilePath);

            }
        }

        private static void ReadTracks()
        {
            string inFolder = @"C:\Users\Sebastian\Dropbox\Projects\POD\Original Tracks\";

            foreach (string inFilePath in Directory.GetFiles(inFolder, "*.bl4"))
            {
                // Ignore partly broken files.
                if (inFilePath.Contains("Arcade++"))
                    continue;
                if (inFilePath.Contains("Forest"))
                    continue;

                if (!inFilePath.Contains("AlderOEM.bl4"))
                    continue;

                string fileName = Path.GetFileName(inFilePath);

                var circuit = new Circuits.Circuit(inFilePath);

                Converter converter = new Converter();
                converter.Add(circuit);
                //converter.Save(@"C:\Users\Sebastian\Documents\Unity Projects\PoD\Assets\Circuits\Alderon\Alderon.obj");
                converter.Save(@"C:\Users\Sebastian\Dropbox\Projects\POD\Export\Alderon.obj");
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
