struct PbdfHeader;
struct PbdfString;

typedef struct // PbdfHeader
{
	uint fileSize;
	uint numOffsets;
	uint offsets[numOffsets];
} PbdfHeader <fgcolor=0x0000FF>;

typedef struct // PbdfString
{
	ubyte length;
	if (length)
	{
		ubyte data[length];
		// Decrypt the string.
		local char decData[length];
		local int i <hidden = true>;
		for (i = 0; i < length; i++)
			decData[i] = data[i] ^ ~i;
	}
} PbdfString <fgcolor=0x000088, read=PbdfStringRead>;
string PbdfStringRead(PbdfString& value)
{
	return value.length == 0 ? "<empty>" : value.decData;
}
bool PbdfStringCompare(PbdfString& a, char b[])
{
	if (!a.length || !sizeof(b))
		return true;
	if (!a.length)
		return false;
	if (!sizeof(b))
		return false;
	return !Stricmp(a.decData, b);
}

void PbdfSeekOffset(uint idxOffset)
{
	local uint headerSize = header.offsets[0];
	local uint offset = header.offsets[idxOffset];
	FSeek(offset - (offset / blockSize * sizeof(uint)));
}
