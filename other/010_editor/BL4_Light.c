struct LightSectorList; struct LightSector; struct Light;

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
