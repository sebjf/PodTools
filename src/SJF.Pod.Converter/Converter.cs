using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using Syroot.Maths;
using Syroot.Pod;
using Syroot.Pod.IO;
using Syroot.Pod.Circuits;
using Syroot.Pod.Cars;
using JeremyAnsel.Media.WavefrontObj;

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

    public static class PodExtensions
    {

    }

    public class Converter
    {
        PortableMesh pmesh;

        Dictionary<uint, PortableMaterial> flatMaterials = new Dictionary<uint, PortableMaterial>();
        Dictionary<uint, PortableMaterial> textureMaterials = new Dictionary<uint, PortableMaterial>();
        List<PortableTexture> textures = new List<PortableTexture>();

        AssetsInfo.CircuitInfo info;

        public Converter()
        {
            pmesh = new PortableMesh();
        }

        public void Save(string file)
        {
            var directory = Path.GetDirectoryName(file);
            var filename = Path.GetFileNameWithoutExtension(file);

            var obj = new ObjFile();

            for (int i = 0; i < pmesh.triangles.Count; i++)
            {
                var triangle = pmesh.triangles[i];
                var face = new ObjFace();

                foreach (var vertex in triangle.vertices)
                {
                    obj.Vertices.Add(vertex.position);
                    obj.VertexNormals.Add(vertex.normal);
                    obj.TextureVertices.Add(vertex.uv);

                    var index = obj.Vertices.Count;
                    face.Vertices.Add(new ObjTriplet(index, index, index));
                }

                face.MaterialName = triangle.material.name;
                obj.Faces.Add(face);
            }

            foreach (var t in textures)
            {
                t.image.Save(directory + Path.DirectorySeparatorChar + t.filename, ImageFormat.Png);
            }

            var mtl = new ObjMaterialFile();

            var materials = pmesh.triangles.Select(t => t.material).Distinct();
            foreach (var m in materials)
            {
                var material = new ObjMaterial(m.name);
                material.DiffuseColor = new ObjMaterialColor(m.colour.x, m.colour.y, m.colour.z);
                if(m.texture != null)
                {
                    material.DiffuseMap = new ObjMaterialMap(m.texture.filename);
                }
                material.IsAntiAliasingEnabled = false;

                mtl.Materials.Add(material);
            }

            mtl.WriteTo(directory + Path.DirectorySeparatorChar + filename + ".mtl");
            obj.MaterialLibraries.Add(filename + ".mtl");

            obj.WriteTo(file);

            if(info != null)
            {
                info.Save(directory + Path.DirectorySeparatorChar + filename + ".xml");
            }
        }

        public void Add(Texture texture, string prefix)
        {
            var size = texture._Size;

            var image = new Bitmap(size, size, PixelFormat.Format32bppRgb);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    var pixel = texture.Data[y * size + x];
                    image.SetPixel(x, y,
                        System.Drawing.Color.FromArgb(
                            (int)(255f * ((pixel >> 11 & 0b11111) / (float)(0b11111))),
                            (int)(255f * ((pixel >> 5 & 0b111111) / (float)(0b111111))),
                            (int)(255f * ((pixel >> 0 & 0b11111) / (float)(0b11111)))
                            ));
                }
            }

            PortableTexture tex = new PortableTexture();
            tex.name = string.Format("{1}_{0}", prefix, textures.Count);
            tex.filename = string.Format("{1}_{0}.png", prefix, textures.Count);
            tex.image = image;

            textures.Add(tex);
        }

        public void Add(Circuit circuit)
        {
            ResetMaterials();

            foreach(var texture in circuit.Textures)
            {
                Add(texture, "diffuse");
            }

            foreach (var sector in circuit.Sectors)
            {
                Add(sector);
            }

            foreach (var background in circuit.Background.Textures)
            {
                Add(background, "background");
            }

            try
            {
                foreach (var sky in circuit.Sky.Textures)
                {
                    Add(sky, "sky");
                }
            }
            catch
            {
            }

            info = new AssetsInfo.CircuitInfo();

            // get the start positions
            // (remember to swap z & y)

            info.grid.pole.x = circuit.DesignationForward.Starts[0].Data.X;
            info.grid.pole.y = circuit.DesignationForward.Starts[0].Data.Z;
            info.grid.pole.z = -circuit.DesignationForward.Starts[0].Data.Y;
            info.grid.second.x = circuit.DesignationForward.Starts[3].Data.X;
            info.grid.second.y = circuit.DesignationForward.Starts[3].Data.Z;
            info.grid.second.z = -circuit.DesignationForward.Starts[3].Data.Y;
            info.grid.third.x = circuit.DesignationForward.Starts[6].Data.X;
            info.grid.third.y = circuit.DesignationForward.Starts[6].Data.Z;
            info.grid.third.z = -circuit.DesignationForward.Starts[6].Data.Y;
            info.grid.forward.x = circuit.DesignationForward.Starts[1].Data.X;
            info.grid.forward.y = circuit.DesignationForward.Starts[1].Data.Z;
            info.grid.forward.z = -circuit.DesignationForward.Starts[1].Data.Y;



        }

        public void Add(Car car)
        {
            ResetMaterials();

            foreach (var texture in car.Material.Textures)
            {
                Add(texture, "diffuse");
            }

            Add(car.Geometry.Good);
            //Add(car.Geometry.Wheels);
        }

        void Add(Body body)
        {
            Add(body.FrontL);
            Add(body.FrontR);
            Add(body.SideL);
            Add(body.SideR);
            Add(body.RearL);
            Add(body.RearR);
        }

        void Add(Wheels wheel)
        {
            Add(wheel.FrontL);
            Add(wheel.FrontR);
            Add(wheel.RearL);
            Add(wheel.RearR);
        }

        void Add(Sector sector)
        {
            Add(sector.Mesh);
        }

        void Add(Mesh mesh)
        {
            foreach (var face in mesh.Faces)
            {
                Add(MakeTriangle(mesh, face, 0, 1, 2));

                if (face.FaceVertexCount == 4)
                {
                    Add(MakeTriangle(mesh, face, 0, 2, 3));
                }
            }                      
        }

        void Add(PortableTriangle triangle)
        {
            pmesh.triangles.Add(triangle);
        }

        PortableMaterial GetFlatMaterial(uint colour)
        {
            if(!flatMaterials.ContainsKey(colour))
            {
                var r = colour >> 16 & 0xFF;
                var b = colour >> 8 & 0xFF;
                var g = colour & 0xFF;

                PortableMaterial material = new PortableMaterial();
                material.name = string.Format("colour_{0}_{1}_{2}", r, g, b);
                material.colour = new PortableVector3(r / 255f, b / 255f, g / 255f);

                flatMaterials.Add(colour, material);
            }

            return flatMaterials[colour];
        }

        PortableMaterial GetTextureMaterial(uint texture)
        {
            if(!textureMaterials.ContainsKey(texture))
            {
                PortableMaterial material = new PortableMaterial();
                material.name = string.Format("texture_{0}", texture);
                material.colour = new PortableVector3(1f, 1f, 1f);
                material.texture = textures[(int)texture];

                textureMaterials.Add(texture, material);
            }

            return textureMaterials[texture];
        }

        void ResetMaterials()
        {
            flatMaterials.Clear();
            textureMaterials.Clear();
            textures.Clear();
        }

        PortableVertex MakeVertex(Mesh mesh, MeshFace face, int i)
        {
            PortableVertex vertex = new PortableVertex();
            vertex.position.x = mesh.Positions[face.Indices[i]].X;
            vertex.position.y = mesh.Positions[face.Indices[i]].Z;
            vertex.position.z = -mesh.Positions[face.Indices[i]].Y;
            vertex.normal.x = mesh.Normals[face.Indices[i]].X;
            vertex.normal.y = mesh.Normals[face.Indices[i]].Z;
            vertex.normal.z = -mesh.Normals[face.Indices[i]].Y;
            vertex.uv.x = face.TexCoords[i].X / 255f;
            vertex.uv.y = 1f - (face.TexCoords[i].Y / 255f);

            return vertex;
        }

        PortableTriangle MakeTriangle(Mesh mesh, MeshFace face, int a, int b, int c)
        {
            PortableTriangle triangle = new PortableTriangle();
            triangle.vertices = new PortableVertex[]
            {
                MakeVertex(mesh, face, a),
                MakeVertex(mesh, face, b),
                MakeVertex(mesh, face, c)
            };
            switch (face.MaterialType)
            {
                case "FLAT":
                case "GOURAUD":
                    triangle.material = GetFlatMaterial(face.ColorOrTexIndex);
                    break;
                case "TEXGOU":
                case "TEX":
                    triangle.material = GetTextureMaterial(face.ColorOrTexIndex);
                    break;
            }
            return triangle;
        }
    }
}
