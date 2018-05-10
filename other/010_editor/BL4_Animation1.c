struct Animation1List; struct Animation1;
struct Animation1ObjectAnim; struct Animation1ObjectFrame; struct Animation1ObjectKey;
struct Animation1TextureAnim; struct Animation1TextureConfig; struct Animation1TextureFrame; struct Animation1TextureFrameData;
struct Animation1SectorList; struct Animation1Sector;

typedef struct // Animation1List
{
	PbdfString fileName;
	if (fileName.decData != "NEANT")
	{
		uint numMacro;
		Macro macros(3)[numMacro] <optimize = true>;
		uint numAnimation1;
		Animation1 animation1s[numAnimation1] <optimize = false>;
	}
} Animation1List <read = Animation1ListRead>;
string Animation1ListRead(Animation1List& value)
{
	return PbdfStringRead(value.fileName);
}

typedef struct // Animation1
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
	Animation1ObjectAnim objectAnims[numObjectAnim] <optimize = false>;
	if (numTextureAnim) Animation1TextureAnim textureAnims[numTextureAnim] <optimize = false>;
	uint numSector;
	if (circuit.sectorList.num)
		Animation1SectorList sectorDefaultsList;
	Animation1SectorList sectorList[circuit.sectorList.num] <optimize = false>;
} Animation1;

typedef struct // Animation1Object
{
	uint startFrame;
	uint numFrame;
	uint hasNamedFaces;
	uint numMesh;
	PbdfString name;
	Mesh mesh(hasNamedFaces, false)[numMesh] <optimize = false>;
	Animation1ObjectFrame frames(numMesh)[numFrame] <optimize = false>;
} Animation1ObjectAnim;

typedef struct(uint numMesh) // Animation1ObjectFrame
{
	Animation1ObjectKey keys[numMesh] <optimize = false>;
} Animation1ObjectFrame;

typedef struct // Animation1ObjectKey
{
	uint value;
	if (value)
	{
		Matrix3x3F16x16 rotation;
		Vector3F16x16 position;
	}
} Animation1ObjectKey;

typedef struct // Animation1TextureAnim
{
	uint startFrame;
	uint numFrame;
	uint value1;
	uint value2;
	PbdfString name; // texture with the same name must be loaded previously, or game crashes
	TextureList textures(256, 256, 2)[numTexture] <optimize = false>; // RGB565
	Animation1TextureConfig configs[value1] <optimize = true>;
	Animation1TextureFrame frames[numFrame] <optimize = false>;
} Animation1TextureAnim;

typedef struct // Animation1TextureConfig
{
	uint value1;
	uint value2;
	uint value3;
	uint value4;
	uint value5;
} Animation1TextureConfig;

typedef struct // Animation1TextureFrame
{
	Vector3U value1;
	uint numData;
	Animation1TextureFrameData data[numData] <optimize = true>;
} Animation1TextureFrame;

typedef struct // Animation1TextureFrameData
{
	uint valueA;
	Vector2U valueB;
	Vector2U valueC;
} Animation1TextureFrameData;

typedef struct // Animation1SectorList
{
	uint numSector;
	uint value2;
	if (numSector) Animation1Sector sectors[numSector] <optimize = false>;
} Animation1SectorList;

typedef struct // Animation1Sector
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
} Animation1Sector;
