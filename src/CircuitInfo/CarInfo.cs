using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AssetsInfo
{
    public class CarInfo
    {
        public Vector3 Chassis;
        public Vector3 WheelFrontL;
        public Vector3 WheelFrontR;
        public Vector3 WheelRearL;
        public Vector3 WheelRearR;

        public void Save(string filename)
        {
            XmlSerializer serialiser = new XmlSerializer(typeof(CarInfo));
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                serialiser.Serialize(stream, this);
            }
        }

        public static CarInfo Load(byte[] bytes)
        {
            XmlSerializer serialiser = new XmlSerializer(typeof(CarInfo));
            return serialiser.Deserialize(new MemoryStream(bytes)) as CarInfo;
        }
    }
}
