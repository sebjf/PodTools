struct EnvironmentList;
struct Decoration; struct DecorationContact; struct DecorationInstance; struct DecorationSectorInstanceList;

typedef struct // EnvironmentList
{
	PbdfString fileName;
	if (!PbdfStringCompare(fileName, "NEANT"))
	{
		uint numMacro;
		if (numMacro) Macro macros(3)[numMacro] <optimize = true>;
		uint numDecorations;
		Decoration decorations[numDecorations] <optimize = false>;
		uint numInstance;
		DecorationInstance instances[numInstance] <optimize = true>;
		DecorationSectorInstanceList sectorInstances[circuit.sectorList.num] <optimize = false>;
	}
} EnvironmentList <read = EnvironmentListRead>;
string EnvironmentListRead(EnvironmentList& value)
{
	return PbdfStringRead(value.fileName);
}

typedef struct // Decoration
{
	PbdfString decorationName;
	PbdfString objectName;
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
	DecorationContact contacts[numContacts];
} Decoration;

typedef struct // DecorationContact
{
	ubyte data[64];
} DecorationContact;

typedef struct // DecorationInstance
{
	uint idx;
	uint numVectors;
	if (numVectors) Vector2U vectors[numVectors];
	Vector3F16x16 position;
	Matrix3x3F16x16 rotation;
} DecorationInstance;

typedef struct // DecorationSectorInstanceList
{
	uint num;
	if (num) DecorationInstance instances[num] <optimize = true>;
} DecorationSectorInstanceList;
