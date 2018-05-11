struct EnvironmentList; struct Environment;
struct EnvironmentObject; struct EnvironmentInstance; struct EnvironmentSectorInstanceList; struct EnvironmentObjectContacts;

typedef struct // EnvironmentList
{
	PbdfString fileName;
	if (!PbdfStringCompare(fileName, "NEANT"))
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
