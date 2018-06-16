struct Anim2List;
struct Anim2;
struct Anim2Texture; struct Anim2TextureKey; struct Anim2TextureFrame;
struct Anim2Object; struct Anim2ObjectKey; struct Anim2ObjectFrame;

typedef struct // Anim2List
{
	uint num;
	if (num) Anim2 Anim2s[(num + 1) / 2] <optimize = false>;
} Anim2List;

typedef struct // Anim2
{
	PbdfString name;
	if (name.decData == "ANIME SECTEUR")
	{
		Error("Not implemented");
	}
	else
	{
		uint numTexture;
		Anim2Texture textures[numTexture] <optimize = false>;
		uint unknown1;
		uint numObject;
		Anim2Object objects[numObject] <optimize = false>;
	}
} Anim2 <read = Anim2Read>;
string Anim2Read(Anim2& value)
{
	return PbdfStringRead(value.name);
}

typedef struct // Anim2Texture
{
	PbdfString name;
	uint numKeys;
	Anim2TextureKey keys[numKeys] <optimize = true>;
	fixed16x16 totalTime;
	uint numFrame;
	Anim2TextureFrame frames[numFrame] <optimize = true>;
} Anim2Texture <read = Anim2TextureRead>;
string Anim2TextureRead(Anim2Texture& value)
{
	return PbdfStringRead(value.name);
}

typedef struct // Anim2TextureFrame
{
	uint time;
	uint idxKey;
} Anim2TextureFrame;

typedef struct // Anim2TextureKey
{
	uint idxTexture; // ushort
	Vector2U uv[4]; // ubytes
} Anim2TextureKey;

typedef struct // Anim2Object
{
	uint idxAnimation;
	uint numFrame;
	Anim2ObjectFrame frames[numFrame] <optimize = false>;
} Anim2Object;

typedef struct // Anim2ObjectFrame
{
	uint idxSector;
	uint numKey;
	Anim2ObjectKey values[numKey] <optimize = true>;
} Anim2ObjectFrame;

typedef struct // Anim2ObjectKey
{
	uint isLooping;
	uint faceType; // 3 or 4 vertices
	uint idxFace;
} Anim2ObjectKey;
