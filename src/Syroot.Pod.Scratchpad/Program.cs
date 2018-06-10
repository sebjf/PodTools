using System;
using System.IO;
using Syroot.Pod.IO;

namespace Syroot.Pod.Scratchpad
{
    internal class Program
    {
        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void Main(string[] args)
        {
            EncryptAllTracks();
            //ReEncryptAllTracks();
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
