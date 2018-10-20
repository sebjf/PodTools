using System.Collections.Generic;
using System.IO;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;

namespace Syroot.Pod.Cars
{
    public class CarFileInfo
    {
        public FileType FileVersion;
        public PodVersion PodVersion;
        public bool PodDemo;
    }

    public class Car : PbdfFile, IData<Car>, IAssetFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class.
        /// </summary>
        public Car() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to load data from.</param>
        public Car(string fileName) : base(fileName, MakeFileInfo(fileName)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load data from.</param>
        public Car(Stream stream, CarFileInfo info) : base(stream, info) { }

        protected override void LoadData(Stream stream)
        {
            new DataLoader<Car>(stream, this).Execute();
        }

        protected override void SaveData(Stream stream)
        {
            new DataSaver<Car>(stream, this).Execute();
        }

        public string Name { get; set; }
        public CarData Data { get; set; }
        public Material Material { get; set; }
        public Objects Geometry { get; set; }
        public Noise Noise { get; set; }
        public Characteristics Characteristics { get; set; }

        public FileType FileType { get; private set; }
        public PodVersion PodVersion { get; set; }

        public void Load(DataLoader<Car> loader, object parameter = null)
        {
            FileType = (Parameter as CarFileInfo).FileVersion;
            PodVersion = (Parameter as CarFileInfo).PodVersion;
            bool PodDemo = (Parameter as CarFileInfo).PodDemo;

            loader.Position = Offsets[0];

            if (!PodDemo)
            {
                Name = loader.ReadPodString();
            }

            Data = loader.Load<CarData>();
            Material = loader.Load<Material>(256);
            Geometry = loader.Load<Objects>();
            Noise = loader.Load<Noise>();
            Characteristics = loader.Load<Characteristics>();

            var Unknown_0000 = loader.ReadUInt32();  // (0..2)
            var Unknown_0004 = loader.ReadSBytes(48);
            var Unknown_000C = loader.ReadUInt32();
            var Unknown_0010 = loader.ReadUInt32();
            var Unknown_0014 = loader.ReadUInt32();
            var Unknown_0018 = loader.ReadUInt32();

            if (PodDemo)
            {
                Name = loader.ReadPodString();
            }
        }

        public void Save(DataSaver<Car> saver, object parameter = null)
        {
            throw new System.NotImplementedException();
        }

        private static CarFileInfo MakeFileInfo(string filename)
        {
            CarFileInfo info = new CarFileInfo();

            info.PodDemo = false;

            if (filename.EndsWith("bv4", System.StringComparison.CurrentCultureIgnoreCase))
            {
                info.FileVersion = FileType.BV4;
            }
            if (filename.EndsWith("bv3", System.StringComparison.CurrentCultureIgnoreCase))
            {
                info.FileVersion = FileType.BV3;
            }
            if (filename.EndsWith("bv6", System.StringComparison.CurrentCultureIgnoreCase))
            {
                info.FileVersion = FileType.BV6;
            }
            if (filename.EndsWith("bv7", System.StringComparison.CurrentCultureIgnoreCase))
            {
                info.FileVersion = FileType.BV7;
            }

            return info;
        }

    }
}
