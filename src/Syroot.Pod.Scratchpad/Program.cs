using System;
using System.IO;
using Syroot.Pod.Circuits;
using Syroot.Pod.Core;

namespace Syroot.Pod.Scratchpad
{
    internal class Program
    {
        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static void Main(string[] args)
        {
            Circuit circuit = new Circuit(@"C:\Games\Pod\DATA\BINARY\CIRCUITS\BELTANE.BL4");

            RawDataFile file = new RawDataFile(@"C:\Games\Pod\DATA\BINARY\CIRCUITS\BELTANE.BL4");
            File.WriteAllBytes(@"D:\Pictures\BELTANE.dec.bl4", file.Data);
            file.Data = File.ReadAllBytes(@"D:\Pictures\BELTANE.dec.bl4");
            file.Save(@"D:\Pictures\BELTANE.BL4");
        }

        class RawDataFile : EncryptedDataFile
        {
            public byte[] Data;

            public RawDataFile(string fileName) : base(fileName)
            {
            }

            protected override void LoadData(Stream stream)
            {
                stream.Position = 0;
                Data = new byte[stream.Length];
                stream.Read(Data, 0, (int)stream.Length);
            }

            protected override void SaveData(Stream stream)
            {
                stream.Position = 0;
                stream.Write(Data, 0, Data.Length);
            }
        }
    }
}
