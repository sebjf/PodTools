using Syroot.Maths;

namespace SJF.Pod.Converter
{
    public static class CoordinateSystems
    {
        public static PortableVector3 Unity(Vector3F v1)
        {
            var v2 = new PortableVector3();
            v2.z = v1.X;
            v2.y = v1.Z;
            v2.x = v1.Y;
            return v2;
        }

        public static PortableVector3 UnityWheelPosition(Vector3F v1)
        {
            var v2 = new PortableVector3();
            v2.z = v1.X;
            v2.y = v1.Z;
            v2.x = -v1.Y;
            return v2;
        }
    }
}
