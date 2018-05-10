struct Background; struct Sky;

typedef struct // Background
{
	uint fogDistance;
	uint fogIntensity;
	uint backDepth;
	uint backBottom;
	uint hasTexture;
	uint color;
	PbdfString name;
	TextureList textures(256, 256, 2); // RGB565
	int yStart;
	int yEnd;
} Background;

typedef struct // Sky
{
	uint hasSky;
	uint yEffect;
	uint value5;
	int value6; // -7000 to 7000
	uint horizonGlowStrength;
	uint speed;
	PbdfString name;
	TextureList textures(128, 128, 2); // RGB565
	TextureImage lensflare(128, 128, 2); // RGB565
	uint reserved;
} Sky;
