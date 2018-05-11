//------------------------------------------------
//--- 010 Editor v8.0 Binary Template
//
//      File: BL4.bt
//   Authors: Syroot
//   Version: 0.1.0
//   Purpose: Parse decrypted Pod BL4 (circuit) files.
//  Category: Pod
// File Mask: *.dec.bl4
//  ID Bytes: 
//   History: 
//  0.1.0   2017-10-02  Initial version.

#include "Math.c"
#include "PBDF.c"
#include "BL4_Mesh.c"
#include "BL4_Event.c"
#include "BL4_Macro.c"
#include "BL4_Texture.c"
#include "BL4_Sector.c"
#include "BL4_Visible.c"
#include "BL4_Environment.c"
#include "BL4_Light.c"
#include "BL4_Animation1.c"
#include "BL4_Sound.c";
#include "BL4_Background.c";
#include "BL4_Animation2.c";

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
	TextureList textureList(256, 256, 2); // RGB565
	SectorList sectorList <bgcolor = 0xCDCDFF>;
	VisibleList visibleList <bgcolor = 0xBDBDEB>;
	EnvironmentList environmentList <bgcolor = 0xEFCDFF>;
	LightSectorList lightSectorList <bgcolor = 0xFFCDEF>;
	Animation1List animation1List <bgcolor = 0xFFCDCD>;
	SoundList soundList <bgcolor = 0xFFD6CD>;
	Background background <bgcolor = 0xFFEFCD>;
	Sky sky <bgcolor = 0xEBDCBD>;
	Animation2List animation2List <bgcolor = 0xEBBDBD>;
} Circuit <bgcolor = 0xCDFFFF>;

LittleEndian();
local uint key = 0x00000000;
PbdfHeader header;
Circuit circuit <open = true>;
