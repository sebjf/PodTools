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
    public class Converter
    {
        Dictionary<string, PortableMesh> meshes = new Dictionary<string, PortableMesh>();
        Dictionary<uint, PortableMaterial> flatMaterials = new Dictionary<uint, PortableMaterial>();
        Dictionary<uint, PortableMaterial> textureMaterials = new Dictionary<uint, PortableMaterial>();
        List<PortableTexture> textures = new List<PortableTexture>();

        private const string DefaultMesh = "default";
        private string CurrentMesh = DefaultMesh;

        private string[] wheelMeshNames = new string[]
            {
                "WheelFrontR",
                "WheelRearR",
                "WheelFrontL",
                "WheelRearL"
            };

        AssetsInfo.CircuitInfo circuitProperties;
        AssetsInfo.CarInfo carProperties;

        public Converter()
        {
            meshes[CurrentMesh] = new PortableMesh();
        }

        public void Save(string file, params string[] meshnames)
        {
            var directory = Path.GetDirectoryName(file);
            var filename = Path.GetFileNameWithoutExtension(file);

            Directory.CreateDirectory(directory);

            foreach (var t in textures)
            {
                t.image.Save(directory + Path.DirectorySeparatorChar + t.filename, ImageFormat.Png);
            }

            if (circuitProperties != null)
            {
                circuitProperties.Save(directory + Path.DirectorySeparatorChar + filename + ".xml");
                SaveObj(directory, filename, DefaultMesh);
            }

            if(carProperties != null)
            {
                carProperties.Save(directory + Path.DirectorySeparatorChar + filename + ".xml");
                SaveObj(directory, filename, DefaultMesh);
                foreach (var wheel in wheelMeshNames)
                {
                    SaveObj(directory, wheel, wheel);
                }
            }
        }

        private void SaveObj(string directory, string filename, params string[] meshnames)
        {
            var obj = new ObjFile();

            var referenced_materials = new List<PortableMaterial>();

            foreach (var name in meshnames)
            {
                var pmesh = meshes[name];

                for (int i = 0; i < pmesh.triangles.Count; i++)
                {
                    var triangle = pmesh.triangles[i];
                    var face = new ObjFace();

                    face.ObjectName = name;

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

                    referenced_materials.Add(triangle.material);
                }
            }

            var mtl = new ObjMaterialFile();

            var materials = referenced_materials.Distinct();
            foreach (var m in materials)
            {
                var material = new ObjMaterial(m.name);
                material.DiffuseColor = new ObjMaterialColor(m.colour.x, m.colour.y, m.colour.z);
                if (m.texture != null)
                {
                    material.DiffuseMap = new ObjMaterialMap(m.texture.filename);
                }
                material.IsAntiAliasingEnabled = false;

                mtl.Materials.Add(material);
            }

            mtl.WriteTo(directory + Path.DirectorySeparatorChar + filename + ".mtl");
            obj.MaterialLibraries.Add(filename + ".mtl");

            obj.WriteTo(directory + Path.DirectorySeparatorChar + filename + ".obj");
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

            circuitProperties = new AssetsInfo.CircuitInfo();

            // get the start positions

            circuitProperties.grid.pole    = CoordinateSystems.Unity(circuit.DesignationForward.Starts[0].Data);
            circuitProperties.grid.second  = CoordinateSystems.Unity(circuit.DesignationForward.Starts[3].Data);
            circuitProperties.grid.third   = CoordinateSystems.Unity(circuit.DesignationForward.Starts[6].Data);
            circuitProperties.grid.forward = CoordinateSystems.Unity(circuit.DesignationForward.Starts[1].Data);
        }

        public void Add(Car car)
        {
            ResetMaterials();

            foreach (var texture in car.Material.Textures)
            {
                Add(texture, "diffuse");
            }

            Add(car.Geometry.Good);
            Add(car.Geometry.Wheels);

            carProperties = new AssetsInfo.CarInfo();
            carProperties.WheelRearL  = CoordinateSystems.UnityWheelPosition(car.Data.DataObjects[3].Position);
            carProperties.WheelFrontL = CoordinateSystems.UnityWheelPosition(car.Data.DataObjects[2].Position);
            carProperties.WheelRearR  = CoordinateSystems.UnityWheelPosition(car.Data.DataObjects[1].Position);
            carProperties.WheelFrontR = CoordinateSystems.UnityWheelPosition(car.Data.DataObjects[0].Position);
            carProperties.Chassis = CoordinateSystems.Unity(car.Data.DataObjects[4].Position);

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
            CurrentMesh = "WheelFrontL"; Add(wheel.FrontL);
            CurrentMesh = "WheelFrontR"; Add(wheel.FrontR);
            CurrentMesh = "WheelRearL"; Add(wheel.RearL);
            CurrentMesh = "WheelRearR"; Add(wheel.RearR);
            CurrentMesh = DefaultMesh;
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
            if(!meshes.ContainsKey(CurrentMesh))
            {
                meshes.Add(CurrentMesh, new PortableMesh());
            }
            meshes[CurrentMesh].triangles.Add(triangle);
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
            vertex.position = CoordinateSystems.Unity(mesh.Positions[face.Indices[i]]);
            vertex.normal = CoordinateSystems.Unity(mesh.Normals[face.Indices[i]]);
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
