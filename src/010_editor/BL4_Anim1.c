struct Anim1Section; struct Anim1SectorList; struct Anim1Sector;
struct Anim1;
struct Anim1ObjectAnim; struct Anim1ObjectFrame; struct Anim1ObjectKey;
struct Anim1TextureAnim; struct Anim1TextureConfig; struct Anim1TextureFrame; struct Anim1TextureFrameData;

typedef struct // Anim1Section
{
	PbdfString fileName;
	if (!PbdfStringCompare(fileName, "NEANT"))
	{
		uint numMacro;
		Macro macros(3)[numMacro] <optimize = true>;
		uint numAnim1;
		Anim1 Anim1s[numAnim1] <optimize = false>;
		uint numSector;
		if (circuit.sectorList.num)
			Anim1SectorList sectorDefaultsList;
		Anim1SectorList sectorList[circuit.sectorList.num] <optimize = false>;
	}
} Anim1Section <read = Anim1SectionRead>;
string Anim1SectionRead(Anim1Section& value)
{
	return PbdfStringRead(value.fileName);
}

typedef struct // Anim1SectorList
{
	uint numSector;
	uint value2;
	if (numSector) Anim1Sector sectors[numSector] <optimize = false>;
} Anim1SectorList;

typedef struct // Anim1Sector
{
	uint idx;
	uint value4; // ushort
	uint value5; // ushort
	uint value6; // ushort
	uint value7; // ushort
	uint numValue9A;
	Vector2U value9A[numValue9A];
	Vector3F16x16 position;
	Matrix3x3F16x16 rotation;
} Anim1Sector;

typedef struct // Anim1
{
	PbdfString name;
	if (name.decData == "wrongway.ani")
	{
		uint wrongWayValue1; // ushort
		uint wrongWayValue2; // ushort
	}
	uint numTextureAnim; // ushort
	uint numObjectAnim; // ushort
	uint value3; // ushort
	Anim1ObjectAnim objectAnims[numObjectAnim] <optimize = false>;
	if (numTextureAnim) Anim1TextureAnim textureAnims[numTextureAnim] <optimize = false>;
} Anim1;

typedef struct // Anim1ObjectAnim
{
	uint startFrame;
	uint numFrame;
	uint hasNamedFaces;
	uint numMesh;
	PbdfString name;
	Mesh mesh(hasNamedFaces, false)[numMesh] <optimize = false>;
	Anim1ObjectFrame frames(numMesh)[numFrame] <optimize = false>;
} Anim1ObjectAnim;

typedef struct(uint numMesh) // Anim1ObjectFrame
{
	Anim1ObjectKey keys[numMesh] <optimize = false>;
} Anim1ObjectFrame;

typedef struct // Anim1ObjectKey
{
	uint value;
	if (value)
	{
		Matrix3x3F16x16 rotation;
		Vector3F16x16 position;
	}
} Anim1ObjectKey;

typedef struct // Anim1TextureAnim
{
	uint startFrame;
	uint numFrame;
	uint numConfig;
	uint value2;
	PbdfString name; // texture with the same name must be loaded previously, or game crashes
	TextureList textures(256, 256, 2) <optimize = false>; // RGB565
	Anim1TextureConfig configs[numConfig] <optimize = true>;
	Anim1TextureFrame frames[numFrame] <optimize = false>;
} Anim1TextureAnim;

typedef struct // Anim1TextureConfig
{
	uint value1;
	uint value2;
	uint value3;
	uint value4;
	uint value5;
} Anim1TextureConfig;

typedef struct // Anim1TextureFrame
{
	Vector3U value1;
	uint numData;
	Anim1TextureFrameData data[numData] <optimize = true>;
} Anim1TextureFrame;

typedef struct // Anim1TextureFrameData
{
	uint valueA;
	Vector2U valueB;
	Vector2U valueC;
} Anim1TextureFrameData;
