using System.Collections.Generic;
using System.Linq;
using Syroot.BinaryData;
using Syroot.Maths;
using Syroot.Pod.IO;

namespace Syroot.Pod.Circuits
{
    public class LightSection : ISectionData
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public string Name { get; set; }
        
        public uint Value1 { get; set; }

        public Vector3U Value2 { get; set; }

        public uint Value3 { get; set; }

        public uint Value4 { get; set; }

        public uint Value5 { get; set; }

        public uint Value6 { get; set; }

        public uint Value7 { get; set; }

        public uint Value8 { get; set; }

        public uint Value9 { get; set; }

        public IList<Light> GlobalLights { get; set; }

        public IList<IList<Light>> SectorLights { get; set; }
        
        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IData<Circuit>.Load(DataLoader<Circuit> loader, object parameter)
        {
            int lights = loader.ReadInt32();
            Value1 = loader.ReadUInt32();
            Value2 = loader.ReadVector3U();
            Value3 = loader.ReadUInt32();
            Value4 = loader.ReadUInt32();
            Value5 = loader.ReadUInt32();
            Value6 = loader.ReadUInt32();
            Value7 = loader.ReadUInt32();
            Value8 = loader.ReadUInt32();
            Value9 = loader.ReadUInt32();
            if (lights > 0)
                GlobalLights = loader.LoadMany<Light>(loader.ReadInt32()).ToList();
            SectorLights = new List<IList<Light>>();
            while (lights-- > 0)
                SectorLights.Add(loader.LoadMany<Light>(loader.ReadInt32()).ToList());
        }

        void IData<Circuit>.Save(DataSaver<Circuit> saver, object parameter)
        {
            saver.WriteInt32(SectorLights.Count);
            saver.WriteUInt32(Value1);
            saver.WriteVector3U(Value2);
            saver.WriteUInt32(Value3);
            saver.WriteUInt32(Value4);
            saver.WriteUInt32(Value5);
            saver.WriteUInt32(Value6);
            saver.WriteUInt32(Value7);
            saver.WriteUInt32(Value8);
            saver.WriteUInt32(Value9);
            if (SectorLights.Count > 0)
            {
                saver.WriteInt32(GlobalLights.Count);
                saver.SaveMany(GlobalLights);
            }
            foreach (IList<Light> lights in SectorLights)
            {
                saver.WriteInt32(lights.Count);
                saver.SaveMany(lights);
            }
        }
    }
}
