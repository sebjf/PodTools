struct Mesh; struct MeshFace;

typedef struct(uint hasNamedFaces, bool hasUnkNormalProperty) // Mesh
{
	uint numVertex;
	Vector3F16x16 positions[numVertex];
	uint numFace;
	uint numTri;
	uint numQuad;
	MeshFace faces(hasNamedFaces, hasUnkNormalProperty)[numFace] <optimize = false>;
	Vector3F16x16 normals[numVertex] <optimize = true>;
	uint unknown; // Color?
} Mesh;

typedef struct(uint hasName, bool hasUnkNormalProperty) // MeshFace
{
	if (hasName)
		PbdfString name;
	if (key == 0x00005CA8)
	{
		uint idxVertexD;
		uint idxVertexA;
		uint numFaceVertex;
		uint idxVertexC;
		uint idxVertexB;
	}
	else
	{
		uint numFaceVertex;
		uint idxVertices[4];
	}
	Vector3F16x16 normal;
	PbdfString materialName; // FLAT, GOURAUD, TEXTURE, TEXGOU
	if (materialName.decData == "FLAT" || materialName.decData == "GOURAUD")
		uint faceColor;
	else
		uint idxTexture;
	Vector2U textureUV[4];
	uint reserved1; // Color?
	if (numFaceVertex == 4)
		Vector3F16x16 quadReserved;
	if (normal.x || normal.y || normal.z)
	{
		if (hasUnkNormalProperty)
			uint unknown; // byte
		uint properties; // byte[3]
	}
	else
	{
		uint reserved2;
	}
} MeshFace <read = MeshFaceRead>;
string MeshFaceRead(MeshFace& value)
{
	return PbdfStringRead(value.name);
}
