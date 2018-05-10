struct PbdfHeader; struct PbdfString;

typedef struct // PbdfHeader
{
	uint fileSize;
	uint numOffsets;
	uint offsets[numOffsets];
} PbdfHeader <fgcolor=0x0000FF>;

typedef struct // PbdfString
{
	ubyte length;
	if (length) ubyte data[length];

	local char decData[length];
	local int i <hidden=true>;
	for (i = 0; i < length; i++)
		decData[i] = data[i] ^ ~i;
} PbdfString <fgcolor=0x000088, read=PbdfStringRead>;
string PbdfStringRead(PbdfString &value)
{
	return value.decData;
}
