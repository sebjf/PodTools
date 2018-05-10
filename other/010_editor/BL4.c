//------------------------------------------------
//--- 010 Editor v8.0 Binary Template
//
//      File: BL4.bt
//   Authors: Syroot
//   Version: 0.1.0
//   Purpose: Parse decrypted Pod BL4 files.
//  Category: Pod
// File Mask: *.dec.bl4
//  ID Bytes: 
//   History: 
//  0.1.0   2017-10-02  Initial version.

#include "Math.c"
#include "Pod.c"

struct Circuit;
struct Mesh; struct MeshFace;
struct EventList; struct Event;
struct MacroList; struct Macro;
struct TextureList; struct Texture; struct TextureRegion; struct TextureImage;
struct SectorList; struct Sector;
struct VisibleList; struct Visible;
struct EnvironmentList; struct Environment;
	struct EnvironmentObject; struct EnvironmentInstance; struct EnvironmentSectorInstanceList; struct EnvironmentObjectContacts;
struct LightSectorList; struct LightSector; struct Light;
struct Animation1List; struct Animation1;
	struct Animation1ObjectAnim; struct Animation1ObjectFrame; struct Animation1ObjectKey;
	struct Animation1TextureAnim; struct Animation1TextureConfig; struct Animation1TextureFrame; struct Animation1TextureFrameData;
	struct Animation1SectorList; struct Animation1Sector;

// ==== Structures =====================================================================================================

local uint key = 0x00000000;

typedef struct // Circuit
{
	// Read main chunk.
	FSeek(header.offsets[0]);
	uint check; // 3
	uint unused;
	EventList events <bgcolor = 0xCDE6FF>;
	MacroList macros <bgcolor = 0xBDD4EB>;
	PbdfString trackName;
	uint textureLoD[16];
	PbdfString projectName;
	TextureList textureList(256, 256, 2); // RGB565
	SectorList sectorList <bgcolor = 0xCDCDFF>;
	VisibleList visibleList <bgcolor = 0xBDBDEB>;
	EnvironmentList environmentList <bgcolor = 0xEFCDFF>;
	LightSectorList lightSectorList <bgcolor = 0xFFCDEF>;
	Animation1List animations1List <bgcolor = 0xFFCDCD>;
} Circuit <bgcolor = 0xCDFFFF>;

// ---- Meshes ----

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
	uint reserved; // Color?
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
		uint reserved;
	}
} MeshFace <read = SectorFaceRead>;
string MeshFaceRead(MeshFace& value)
{
	return PbdfStringRead(value.name);
}

// ---- Events ----

typedef struct // EventList
{
	uint num;
	uint sizBuffer;
	Event events[num] <optimize = false>;
} EventList;

typedef struct // Event
{
	PbdfString name;
	uint sizParam;
	uint numParam;
	if (sizParam * numParam) ubyte paramData[sizParam * numParam];
} Event <read = EventRead>;
string EventRead(Event& value)
{
	return PbdfStringRead(value.name);
}

// ---- Macros ----

typedef struct // MacroList
{
	uint lenMacroBase;
	if (lenMacroBase) Macro macroBase(3)[lenMacroBase] <optimize = true>;
	uint lenMacro;
	if (lenMacro) Macro macro(1)[lenMacro] <optimize = true>;
	uint lenMacroInit;
	if (lenMacroInit) Macro macroInit(1)[lenMacroInit] <optimize = true>;
	uint lenMacroActive;
	if (lenMacroActive) Macro macroActive(1)[lenMacroActive] <optimize = true>;
	uint lenMacroDesactive;
	if (lenMacroDesactive) Macro macroDesactive(1)[lenMacroDesactive] <optimize = true>;
	uint lenMacroRemplace;
	if (lenMacroRemplace) Macro macroRemplace(2)[lenMacroRemplace] <optimize = true>;
	uint lenMacroEchange;
	if (lenMacroEchange) Macro macroEchange(2)[lenMacroEchange] <optimize = true>;
} MacroList;

typedef struct(int numValue) // Macro
{
	int values[numValue];
} Macro;

// ---- Textures ----

typedef struct(int width, int height, int pixelSize) // TextureList
{
	uint numTexture;
	uint reserved;
	Texture textures[numTexture] <optimize = false>;
	TextureImage images(width, height, pixelSize)[numTexture] <optimize = true>;
} TextureList;

typedef struct // Texture
{
	uint numRegion;
	TextureRegion regions[numRegion];
} Texture;

typedef struct // TextureRegion
{
	char name[32];
	uint left;
	uint top;
	uint right;
	uint bottom;
	uint idx;
} TextureRegion <read = TextureRegionRead>;
string TextureRegionRead(TextureRegion& value)
{
	return value.name;
}

typedef struct(int width, int height, int pixelSize) // TextureImage
{
	ubyte data[width * height * pixelSize];
} TextureImage;

// ---- Sectors ----

typedef struct // SectorList
{
	uint hasNamedFaces;
	uint num;
	Sector sectors(hasNamedFaces)[num] <optimize = false>;
} SectorList;

typedef struct(uint hasNamedFaces) // Sector
{
	Mesh mesh(hasNamedFaces, true);
	ubyte vertexLight[mesh.numVertex];
	Vector3F16x16 boundingBoxMin; // z -= 2
	Vector3F16x16 boundingBoxMax; // z += 10
} Sector;

// ---- Visible ----

typedef struct // VisibleList
{
	uint numVisibles;
	Visible visible[numVisibles] <optimize = false>;
} VisibleList;

typedef struct // Visible
{
	int numSectors; // -1 = no sectors are visible
	if (numSectors > 0) uint sectors[numSectors];
} Visible;

// ---- Environment ----

typedef struct // EnvironmentList
{
	PbdfString fileName;
	if (fileName.decData != "NEANT")
	{
		uint numMacro;
		if (numMacro) Macro macros(3)[numMacro] <optimize = true>;
		uint numEnvironment;
		Environment environments[numEnvironment] <optimize = false>;
		uint numInstance;
		EnvironmentInstance instances[numInstance] <optimize = true>;
		EnvironmentSectorInstanceList sectorInstances[circuit.sectorList.num] <optimize = false>;
	}
} EnvironmentList <read = EnvironmentListRead>;
string EnvironmentListRead(EnvironmentList& value)
{
	return PbdfStringRead(value.fileName);
}

typedef struct // Environment
{
	PbdfString name;
	EnvironmentObject object;
} Environment;

typedef struct // EnvironmentObject
{
	PbdfString name;
	TextureList textures(128, 128, 2); // RGB565
	uint hasNamedFaces;
	Mesh mesh(hasNamedFaces, false);
	Vector3F16x16 collisionPrism1;
	uint collisionPrism2;
	Vector3F16x16 collisionPrism3;
	uint unknown1;
	Vector3 unknown2;
	uint unknown3;
	uint unknown4;
	uint numContacts;
	EnvironmentObjectContacts contacts[numContacts];
} EnvironmentObject;

typedef struct // EnvironmentInstance
{
	uint idx;
	uint numVectors;
	if (numVectors) Vector2U vectors[numVectors];
	Vector3F16x16 position;
	Matrix3x3F16x16 rotation;
} EnvironmentInstance;

typedef struct // EnvironmentSectorInstanceList
{
	uint num;
	if (num) EnvironmentInstance instances[num] <optimize = true>;
} EnvironmentSectorInstanceList;

typedef struct // EnvironmentObjectContacts
{
	ubyte contacts[64];
} EnvironmentObjectContacts;

// ---- Lights ----

typedef struct // LightSectorList
{
	PbdfString fileName;
	if (fileName.decData != "NEANT")
	{
		uint num;
		uint value1;
		uint value2[3];
		uint value3;
		uint value4;
		uint value5;
		uint value6;
		uint value7;
		uint value8;
		uint value9;
		if (num >= 0)
			LightSector globalLightSector;
		LightSector lightSectors[num] <optimize = false>;
	}
} LightSectorList <read = LightSectorListRead>;
string LightSectorListRead(LightSectorList& value)
{
	return PbdfStringRead(value.fileName);
}

typedef struct // LightSector
{
	uint numLights;
	if (numLights) Light lights[numLights] <optimize = true>;
} LightSector;

typedef struct // Light
{
	uint type;
	ubyte value1[48];
	uint value2;
	uint value3;
	uint diffusion;
	uint values;
} Light;

// ---- Animations1 ----

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

// ==== Contents =======================================================================================================

LittleEndian();
PbdfHeader header;
Circuit circuit <open = true>;
