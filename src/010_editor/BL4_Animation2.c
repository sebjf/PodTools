struct Animation2List;
struct Animation2;
struct Animation2TextureAnim;
struct Animation2TextureAnimKey;
struct Animation2TextureAnimFrame;
struct Animation2SectorAnim;
struct Animation2SectorAnimSector;
struct Animation2SectorAnimValue;

typedef struct // Animation2List
{
	uint num;
	if (num) Animation2 animation2s[(num + 1) / 2] <optimize = false>;
} Animation2List;

typedef struct // Animation2
{
	PbdfString name;
	if (name.decData == "ANIME SECTEUR")
	{
		Error("Not implemented");
	}
	else
	{
		uint numTexAnim;
		Animation2TextureAnim texAnims[numTexAnim] <optimize = false>;
		uint value1;
		uint numSectorAnim;
		Animation2SectorAnim sectorAnims[numSectorAnim] <optimize = false>;
	}
} Animation2 <read = Animation2Read>;
string Animation2Read(Animation2& value)
{
	return PbdfStringRead(value.name);
}

typedef struct // Animation2TextureAnim
{
	PbdfString name;
	uint numKeys;
	Animation2TextureAnimKey keys[numKeys] <optimize = true>;
	fixed16x16 totalTime;
	uint numFrame;
	Animation2TextureAnimFrame frames[numFrame] <optimize = true>;
} Animation2TextureAnim <read = Animation2TextureAnimRead>;
string Animation2TextureAnimRead(Animation2TextureAnim& value)
{
	return PbdfStringRead(value.name);
}

typedef struct // Animation2TextureAnimKey
{
	uint idxTexture; // ushort
	Vector2U uv[4]; // ubytes
} Animation2TextureAnimKey;

typedef struct // Animation2TextureAnimFrame
{
	uint time;
	uint idxKey;
} Animation2TextureAnimFrame;

typedef struct // Animation2SectorAnim
{
	uint idxAnimation;
	uint numSector;
	Animation2SectorAnimSector sectors[numSector] <optimize = false>;
} Animation2SectorAnim;

typedef struct // Animation2SectorAnimSector
{
	uint idxSector;
	uint numValue;
	Animation2SectorAnimValue values[numValue] <optimize = true>;
} Animation2SectorAnimSector;

typedef struct // Animation2SectorAnimValue
{
	uint isLooping;
	uint faceType; // 3 or 4 vertices
	uint idxFace;
} Animation2SectorAnimValue;
