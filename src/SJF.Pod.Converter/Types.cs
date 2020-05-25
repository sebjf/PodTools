using System.Collections.Generic;
using System.Drawing;
using JeremyAnsel.Media.WavefrontObj;
using Syroot.Maths;

namespace SJF.Pod.Converter
{
    public class PortableMesh
    {
        public List<PortableTriangle> triangles = new List<PortableTriangle>();
    }

    public class PortableMaterial
    {
        public string name;
        public PortableTexture texture;
        public PortableVector3 colour;
    }

    public class PortableTexture
    {
        public string name;
        public string filename;
        public Bitmap image;
    }

    public struct PortableVector3
    {
        public float x;
        public float y;
        public float z;

        public PortableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator PortableVector3(Vector3F v)
        {
            PortableVector3 result = new PortableVector3();
            result.x = v.X;
            result.y = v.Y;
            result.z = v.Z;
            return result;
        }

        public static implicit operator ObjVector4(PortableVector3 v)
        {
            ObjVector4 result = new ObjVector4();
            result.X = v.x;
            result.Y = v.y;
            result.Z = v.z;
            result.W = 0;
            return result;
        }

        public static implicit operator ObjVector3(PortableVector3 v)
        {
            ObjVector3 result = new ObjVector3();
            result.X = v.x;
            result.Y = v.y;
            result.Z = v.z;
            return result;
        }

        public static implicit operator ObjVertex(PortableVector3 v)
        {
            ObjVertex result = new ObjVertex();
            result.Position = v;
            return result;
        }

        public static implicit operator AssetsInfo.Vector3(PortableVector3 v)
        {
            var result = new AssetsInfo.Vector3();
            result.x = v.x;
            result.y = v.y;
            result.z = v.z;
            return result;
        }
    }

    public struct PortableVector2
    {
        public float x;
        public float y;

        public static implicit operator PortableVector2(Vector2U v)
        {
            PortableVector2 result = new PortableVector2();
            result.x = v.X;
            result.y = v.Y;
            return result;
        }

        public static implicit operator ObjVector3(PortableVector2 v)
        {
            ObjVector3 result = new ObjVector3();
            result.X = v.x;
            result.Y = v.y;
            result.Z = 0;
            return result;
        }
    }

    public struct PortableVertex
    {
        public PortableVector3 position;
        public PortableVector3 normal;
        public PortableVector2 uv;
    }

    public struct PortableTriangle
    {
        public PortableMaterial material;
        public PortableVertex[] vertices;
    }
}
