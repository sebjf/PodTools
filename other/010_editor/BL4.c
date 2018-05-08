//------------------------------------------------
//--- 010 Editor v8.0 Binary Template
//
//      File: BL4.bt
//   Authors: Syroot
//   Version: 0.1.0
//   Purpose: Parse decrypted Pod BL4 files.
//  Category: Pod
// File Mask: *.dec.bin
//  ID Bytes: 
//   History: 
//  0.1.0   2017-10-02  Initial version.

#include "Math.c"
#include "Pod.c"

struct Circuit;
struct EventList; struct Event;
struct MacroList; struct Macro;
struct TextureList; struct Texture; struct TextureFrame; struct TextureImage;
struct SectorList; struct Sector; struct SectorFace;
struct VisibleList; struct Visible;
struct EnvironmentList; struct Environment; struct EnvironmentObject; struct EnvironmentInstance; struct EnvironmentSectorInstanceList; struct EnvironmentObjectContacts;
struct Lights; struct LightList; struct Light;
struct Animations1;

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
	TextureList textures(256, 256, 2); // RGB565
	SectorList sectors <bgcolor = 0xCDCDFF>;
	VisibleList visibles <bgcolor = 0xBDBDEB>;
	EnvironmentList environmentList <bgcolor = 0xEFCDFF>;

	PbdfString lightsName;
	if (lightsName.decData != "NEANT")
		Lights lights;

	PbdfString animations1Name;
	if (animations1Name.decData != "NEANT")
		Animations1 animations;

} Circuit <bgcolor = 0xCDFFFF>;

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
	ubyte paramData[sizParam * numParam];
} Event <read = EventRead>;
string EventRead(Event& value)
{
	return PbdfStringRead(value.name);
}

// ---- Macros ----

typedef struct // MacroList
{
	uint lenMacroBase;
	Macro macroBase(3)[lenMacroBase] <optimize = true>;
	uint lenMacro;
	Macro macro(1)[lenMacro] <optimize = true>;
	uint lenMacroInit;
	Macro macroInit(1)[lenMacroInit] <optimize = true>;
	uint lenMacroActive;
	Macro macroActive(1)[lenMacroActive] <optimize = true>;
	uint lenMacroDesactive;
	Macro macroDesactive(1)[lenMacroDesactive] <optimize = true>;
	uint lenMacroRemplace;
	Macro macroRemplace(2)[lenMacroRemplace] <optimize = true>;
	uint lenMacroEchange;
	Macro macroEchange(2)[lenMacroEchange] <optimize = true>;
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
	uint numFrame;
	TextureFrame frames[numFrame];
} Texture;

typedef struct // TextureFrame
{
	char name[32];
	uint left;
	uint top;
	uint right;
	uint bottom;
	uint idx;
} TextureFrame <read = TextureFrameRead>;
string TextureFrameRead(TextureFrame& value)
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
	Sector sectors(hasNamedFaces, false)[num] <optimize = false>;
} SectorList;

typedef struct(uint hasNamedFaces, bool isEnvironmentSector) // Sector
{
	uint numVertex;
	Vector3F16x16 positions[numVertex];
	uint numFace;
	uint numTri;
	uint numQuad;
	SectorFace faces(hasNamedFaces, isEnvironmentSector)[numFace] <optimize = false>;
	Vector3F16x16 normals[numVertex] <optimize = true>;
	uint unknown; // Color?
	if (!isEnvironmentSector)
	{
		ubyte vertexLight[numVertex];
		Vector3F16x16 boundingBoxMin; // z -= 2
		Vector3F16x16 boundingBoxMax; // z += 10
	}
} Sector;

typedef struct(uint hasNamedFaces, bool isEnvironmentSector) // SectorFace
{
	if (hasNamedFaces)
		PbdfString faceName;
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
		if (!isEnvironmentSector)
			uint unknown; // byte
		uint properties; // byte[3]
	}
	else
	{
		uint reserved;
	}
} SectorFace <read = SectorFaceRead>;
string SectorFaceRead(SectorFace& value)
{
	return PbdfStringRead(value.faceName);
}

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
		Macro macros(3)[numMacro] <optimize = true>;
		uint numEnvironment;
		Environment environments[numEnvironment] <optimize = false>;
		uint numInstance;
		EnvironmentInstance instances[numInstance] <optimize = true>;
		EnvironmentSectorInstanceList sectorInstances[circuit.sectors.num] <optimize = false>;
	}
} EnvironmentList;

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
	Sector sector(hasNamedFaces, true);
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
	Vector2U vectors[numVectors];
	Vector3F16x16 position;
	Matrix3x3F16x16 rotation;
} EnvironmentInstance;

typedef struct // EnvironmentSectorInstanceList
{
	uint num;
	EnvironmentInstance instances[num] <optimize = true>;
} EnvironmentSectorInstanceList;

typedef struct // EnvironmentObjectContacts
{
	ubyte contacts[64];
} EnvironmentObjectContacts;

// ---- Lights ----

typedef struct // Lights
{
	uint numSector;
	uint value1;
	uint value2[3];
	uint value3;
	uint value4;
	uint value5;
	uint value6;
	uint value7;
	uint value8;
	uint value9;
	if (numSector >= 0)
		LightList globalLightList;
	LightList sectorLightLists[numSector] <optimize = false>;
} Lights <bgcolor = 0xFFCDEF>;

typedef struct // LightList
{
	uint count;
	Light lights[count] <optimize = true>;
} LightList;

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

typedef struct // Animations1
{
	uint numMacro;
	Macro macros(3)[numMacro] <optimize = true>;
	uint animationCount;
} Animations1 <bgcolor = 0xFFCDCD>;

// ==== Contents =======================================================================================================

LittleEndian();
PbdfHeader header;
Circuit circuit <open = true>;
