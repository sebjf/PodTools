using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace CircuitInfo
{
    /*
    This library is for Unity, which needs .net 3.5.
    VS2017: Tools > Get Tools and Features,
    select "Individual Components" tab
    select ".NET Framework 3.5 development tools"
    */



    public class CircuitInfo
    {
        public struct Vector3
        {
            public float x;
            public float y;
            public float z;
        }

        public struct Grid
        {
            public Vector3 pole;
            public Vector3 second;
            public Vector3 third;
            public Vector3 forward;
        }

        public Grid grid;

        public void Save(string filename)
        {
            XmlSerializer serialiser = new XmlSerializer(typeof(CircuitInfo));
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                serialiser.Serialize(stream, this);
            }
        }

        public static CircuitInfo Load(byte[] bytes)
        {
            XmlSerializer serialiser = new XmlSerializer(typeof(CircuitInfo));
            return serialiser.Deserialize(new MemoryStream(bytes)) as CircuitInfo;
        }
    }
}
