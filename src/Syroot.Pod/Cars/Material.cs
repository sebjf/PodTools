using System.Collections.Generic;
using System.IO;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Pod.IO;
using Syroot.Pod.Circuits;

namespace Syroot.Pod.Cars
{
    public class Material : IData<Car>
    {
        public string Name { get; set; }
        public TextureList Textures { get; set; }

        public void Load(DataLoader<Car> loader, object parameter = null)
        {
            Name = loader.ReadPodString();

            if(Name != "GOURAUD")
            {
                Textures = loader.Load<TextureList>(parameter);
            }
        }

        public void Save(DataSaver<Car> saver, object parameter = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
