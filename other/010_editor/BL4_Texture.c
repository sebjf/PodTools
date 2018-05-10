struct TextureList; struct Texture; struct TextureRegion; struct TextureImage;

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
