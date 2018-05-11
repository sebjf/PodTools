//
// PodX3dfx.exe, v2.2.9.0 (BL4 only)
//
//  NOTE: Layout is complete, but analysis/documentation is work in progress.
//

struct Header
{
	uint32_t FileSize;
	uint32_t OffsetCount;               // 0x0000000A
	uint32_t OffsetTable[OffsetCount];  // Fileoffset
}
uint32_t Reserved1;  // must be 0x00000003
uint32_t Reserved2;  // (not used)
struct Events
{
	uint32_t NameCount;
	uint32_t BufferSize;
	for (NameCount)  // Sorted by Name
	{
		PbdfString Name;
		uint32_t ParamSize;
		sint32_t ParamCount;
		uint8_t  ParamData[ParamSize][ParamCount];
		/**********************************
		* Name                 ParamSize *
		* "...Sinon..."        00000000  *
		* "? Concurrent"       00000000  *
		* "? Joueur"           00000000  *
		* "? Proba Unique"     00000004  *
		* "? Tour"             00000004  *
		* "Active Macro"       00000004  *
		* "Aff Env OFF"        00000000  *
		* "Aff Env ON"         00000000  *
		* "All Macros OFF"     00000000  *
		* "All Macros ON"      00000000  *
		* "Arret Anim"         0000002C  *
		* "Bloque Anim"        00000000  *
		* "Camera Libre"       00000000  *
		* "Camera Plafond"     00000004  *
		* "Contrainte"         00000000  *
		* "DEBUT-COND"         00000000  *
		* "Decision PIT"       00000000  *
		* "Degats Off"         00000000  *
		* "Degats On"          00000000  *
		* "Deplace Anim"       0000000C  *
		* "Desactive Macro"    00000004  *
		* "Echange Macros"     00000008  *
		* "FIN-COND"           00000000  *
		* "Fige Anim"          0000002C  *
		* "Fin Course"         00000000  *
		* "Fin Dep L"          00000000  *
		* "Init Macro"         00000004  *
		* "Lance Anim"         0000002C  *
		* "Macro"              00000008  *
		* "N-Chrono Inter"     00000000  *
		* "N-Chrono Tour"      00000000  *
		* "P-Chrono Inter"     00000000  *
		* "P-Chrono Tour"      00000000  *
		* "Pistes cachees"     00000000  *
		* "Reinit Anim"        00000000  *
		* "Relance Anim"       00000000  *
		* "Remplace Macro"     00000008  *
		* "Reset Reverb Voit"  00000004  *
		* "Reverb Voiture"     00000004  *
		* "Son LocalisT"       00000004  *
		* "Son Source"         00000004  *
		* "Son Statique"       00000004  *
		* "Stop Anim"          00000000  *
		* "Troncon"            00000000  *
		* "Tue Sons LocalisTs" 00000004  *
		* "Tue Sons Source"    00000000  *
		* "Tue Sons Statiques" 00000000  *
		* "Tunnel Entree"      00000000  *
		* "Tunnel Sortie"      00000000  *
		* "Wrong-Way OFF"      00000000  *
		* "Wrong-Way ON"       00000000  *
		* "}}} (Rupture)"      00000000  *
		**********************************/
	}
	struct Macros
	{
		{
			uint32_t Count1;
			for (Count1)
			{
				uint32_t Value1;
				uint32_t Value2;
				uint32_t Value3;
			}
		}
		uint32_t Count2;  // "Macro"
		for (Count2)
		{
			uint32_t Value1;
		}
		uint32_t Count3;  // "Init Macro"
		for (Count3)
		{
			uint32_t Value1;
		}
		uint32_t Count4;  // "Active Macro"
		for (Count4)
		{
			uint32_t Value1;
		}
		uint32_t Count5;  // "Desactive Macro"
		for (Count5)
		{
			uint32_t Value1;
		}
		uint32_t Count6;  // "Remplace Macro"
		for (Count6)
		{
			uint32_t Value1;
			uint32_t Value2;
		}
		uint32_t Count7;  // "Echange Macros"
		for (Count7)
		{
			uint32_t Value1;
			uint32_t Value2;
		}
	}
}
PbdfString TrackName;
struct LevelOfDetail
{
	uint32_t TextureLOD[16];
}
PbdfString ProjectName;
struct Textures
{
	uint32_t Count;
	uint32_t Reserved;
	for (Count)
	{
		uint32_t Count;
		for (Count)
		{
			uint8_t  Name[32];
			uint32_t Left;
			uint32_t Top;
			uint32_t Right;
			uint32_t Bottom;
			uint32_t Index;
		}
	}
	for (Count)
	{
		uint8_t Pixel[256][256][2];  // RGB565
	}
}
struct Sectors
{
	uint32_t NamedFaces;  // uint8_t
	uint32_t SectorCount;
	for (SectorCount)
	{
		uint32_t VertexCount;
		uint32_t VertexArray[VertexCount][3];
		uint32_t FaceCount ;
		uint32_t TriangleCount;
		uint32_t QuadrangleCount;
		for (FaceCount)
		{
			if (NamedFaces)
			{
				PbdfString FaceName;
			}
			if (__KEY__ == 0x00005CA8)
			{
				uint32_t VertexIndexD;
				uint32_t VertexIndexA;
				uint32_t FaceVertexCount;
				uint32_t VertexIndexC;
				uint32_t VertexIndexB;
			}
			else
			{
				uint32_t FaceVertexCount;
				uint32_t VertexIndices[4];
			}
			uint32_t FaceNormal[3];
			PbdfString MaterialType;
					/*************
					* "FLAT"    *
					* "GOURAUD" *
					* "TEXTURE" *
					* "TEXGOU"  *
					*************/
			{
				union
				{
					uint32_t FaceColor;     // "FLAT", "GOURAUD"
					uint32_t TextureIndex;  // "TEXTURE", "TEXGOU"
				}
				uint32_t TextureUV[4][2];
				uint32_t Reserved;  // Color?
			}
			if (FaceVertexCount == 4)
			{
				uint32_t QuadrangleReserved[3];
			}
			if (FaceNormal[0] || FaceNormal[1] || FaceNormal[2])
			{
				uint32_t Unknown;         // uint8_t
				uint32_t FaceProperties;  // uint8_t[3]
			}
			else // Invalid
			{
				uint32_t Reserved;
			}
		}
		uint32_t NormalArray[VertexCount][3];
		uint32_t Unknown;  // Color?

		// NICO: Moved from Sector group to Sector itself.
		uint8_t VertexLight[VertexCount];
		struct BoundingBox
		{
			uint32_t Min[3];  // z -= 2.0
			uint32_t Max[3];  // z += 10.0
		}
	}
}
struct Visibility
{
	uint32_t Count;
	for (Count)
	{
		int32_t VisibleCount;  // -1 = all
		if (VisibleCount > 0)
		{
			for (VisibleCount)
			{
				uint32_t SectorIndex;
			}
		}
	}
}
struct Environment
{
	// NICO: Optional, missing in some files (Beltane.bl4)	
	PbdfString Name;
	if (Name != "NEANT")
	{
		struct Macros
		{
			uint32_t Count;
			for (Count)
			{
				uint32_t Value1;
				uint32_t Value2;
				uint32_t Value3;
			}
		}
		uint32_t EnvironmentCount;
		for (EnvironmentCount)
		{
			PbdfString EnvironmentName;
			struct EnvironmentObject
			{
				PbdfString Name;
				struct Textures
				{
					uint32_t Count;
					uint32_t Reserved;
					for (Count)
					{
						uint32_t Count;
						for (Count)
						{
							uint8_t  Name[32];
							uint32_t Left;
							uint32_t Top;
							uint32_t Right;
							uint32_t Bottom;
							uint32_t Index;
						}
					}
					for (Count)
					{
						uint8_t Pixel[128][128][2];  // RGB565
					}
				}
				struct Object
				{
					uint32_t NamedFaces;  // uint8_t
					struct Faces
					{
						uint32_t VertexCount;
						uint32_t VertexArray[VertexCount][3];
						uint32_t FaceCount;
						uint32_t TriangleCount;
						uint32_t QuadrangleCount;
						for (FaceCount)
						{
							if (NamedFaces)
							{
								PbdfString FaceName;
							}
							if (__KEY__ == 0x00005CA8)
							{
								uint32_t VertexIndexD;
								uint32_t VertexIndexA;
								uint32_t FaceVertexCount;
								uint32_t VertexIndexC;
								uint32_t VertexIndexB;
							}
							else
							{
								uint32_t FaceVertexCount;
								uint32_t VertexIndices[4];
							}
							uint32_t FaceNormal[3];
							PbdfString MaterialType;
									/*************
									* "FLAT"    *
									* "GOURAUD" *
									* "TEXTURE" *
									* "TEXGOU"  *
									*************/
							{
								union
								{
									uint32_t FaceColor;     // "FLAT", "GOURAUD"
									uint32_t TextureIndex;  // "TEXTURE", "TEXGOU"
								}
								uint32_t TextureUV[4][2];
								uint32_t Reserved;  // Color?
							}
							if (FaceVertexCount == 4)
							{
								uint32_t QuadrangleReserved[3];
							}
							if (FaceNormal[0] || FaceNormal[1] || FaceNormal[2])
							{
								uint32_t FaceProperties;  // uint8_t[3]
							}
							else // Invalid
							{
								uint32_t Reserved;
							}
						}
						uint32_t NormalArray[VertexCount][3];
						uint32_t Unknown;
					}
					struct CollisionPrism
					{
						uint32_t Value1[3];
						uint32_t Value2;
						uint32_t Value3[3];
					}
					uint32_t Unknown1;
					uint32_t Unknown2[3];
					uint32_t Unknown3;
					uint32_t Unknown4;
					uint32_t ContactCount;
					uint8_t  ContactArray[ContactCount][64];
				}
			}
		}
		uint32_t InstanceCount;
		for (InstanceCount)
		{
			uint32_t Index;
			{
				uint32_t Count;
				for (Count)
				{
					uint32_t Value1;
					uint32_t Value2;
				}
			}
			uint32_t Position[3];
			uint32_t Rotation[3][3];
		}
		for (SectorCount)
		{
			uint32_t Count;
			for (Count)
			{
				uint32_t Index;
				{
					uint32_t Count;
					for (Count)
					{
						uint32_t Value1;
						uint32_t Value2;
					}
				}
				uint32_t Position[3];
				uint32_t Rotation[3][3];
			}
		}
	}
}
struct Light  // *.lum
{
	PbdfString Name;
	if (Name != "NEANT")
	{
		uint32_t SectorCount;
		{
			uint32_t Value1;
			uint32_t Value2[3];
			uint32_t Value3;
			uint32_t Value4;
			uint32_t Value5;
			uint32_t Value6;
			uint32_t Value7;
			uint32_t Value8;
			uint32_t Value9;
		}
		if (SectorCount >= 0)
		{
			uint32_t Count;
			for (Count)
			{
				uint32_t Type;       // Cylinder, Cone, Sphere
				uint8_t  Value1[48];
				uint32_t Value2;
				uint32_t Value3;
				uint32_t Diffusion;  // None, Linear, Square
				uint32_t Value5;
			}
		}
		for (SectorCount)
		{
			uint32_t Count;
			for (Count)
			{
				uint32_t Type;       // Cylinder, Cone, Sphere
				uint8_t  Value1[48];
				uint32_t Value2;
				uint32_t Value3;
				uint32_t Diffusion;  // None, Linear, Square
				uint32_t Value5;
			}
		}
	}
}
struct Animations1  // *.anc
{
	PbdfString Name;
	if (Name != "NEANT")
	{
		struct Macros
		{
			uint32_t Count;
			for (Count)
			{
				uint32_t Value1;
				uint32_t Value2;
				uint32_t Value3;
			}
		}
		uint32_t AnimationCount;
		for (AnimationCount)
		{
			// Animation1
			PbdfString Name;
			if (Name == "wrongway.ani")
			{
				uint32_t Value1;  // uint16_t
				uint32_t Value2;  // uint16_t
			}
			{
				uint32_t AnimatedTextureCount;  // uint16_t
				uint32_t AnimatedObjectsCount;  // uint16_t
				uint32_t Value3;                // uint16_t
				for (AnimatedObjectsCount)
				{
					// Animation1ObjectAnim
					uint32_t StartFrame;
					uint32_t FrameCount;
					uint32_t NamedFaces;  // uint8_t
					uint32_t ObjectCount;
					{
						PbdfString Name;
						for (ObjectCount)
						{
							uint32_t VertexCount;
							uint32_t VertexArray[VertexCount][3];
							uint32_t FaceCount :
							uint32_t TriangleCount;
							uint32_t QuadrangleCount;
							for (FaceCount)
							{
								if (NamedFaces)
								{
									PbdfString FaceName;
								}
								if (__KEY__ == 0x00005CA8)
								{
									uint32_t VertexIndexD;
									uint32_t VertexIndexA;
									uint32_t FaceVertexCount;
									uint32_t VertexIndexC;
									uint32_t VertexIndexB;
								}
								else
								{
									uint32_t FaceVertexCount;
									uint32_t VertexIndices[4];
								}
								uint32_t FaceNormal[3];
								PbdfString MaterialType;
										/*************
										* "FLAT"    *
										* "GOURAUD" *
										* "TEXTURE" *
										* "TEXGOU"  *
										*************/
								{
									union
									{
										uint32_t FaceColor;     // "FLAT", "GOURAUD"
										uint32_t TextureIndex;  // "TEXTURE", "TEXGOU"
									}
									uint32_t TextureUV[4][2];
									uint32_t Reserved;  // Color?
								}
								if (FaceVertexCount == 4)
								{
									uint32_t QuadrangleReserved[3];
								}
								if (FaceNormal[0] || FaceNormal[1] || FaceNormal[2])
								{
									uint32_t FaceProperties;  // uint8_t[3]
								}
								else // Invalid
								{
									uint32_t Reserved;
								}
							}
							uint32_t NormalArray[VertexCount][3];
							uint32_t Unknown;
						}
					}
					for (FrameCount)
					{
						// Animation1ObjectFrame
						for (ObjectCount)
						{
							// Animation1ObjectKey
							uint32_t Value1;
							if (Value1)
							{
								uint32_t Value1[3][3];
								uint32_t Value2[3];
							}
						}
					}
				}
				for (AnimatedTextureCount)
				{
					uint32_t StartFrame;
					uint32_t FrameCount;
					{

						uint32_t Value1;
						uint32_t Value2;
						PbdfString Name;
						{	//NOTE: v2.2.9.0 fails to load the textures here.
							// (a texture block with Name.Buffer must already
							//  be loaded, or it will try to load the texture
							//  block with an unsupported format ID (3) here)
							uint32_t Count;
							uint32_t Reserved;
							for (Count)
							{
								uint32_t Count;
								for (Count)
								{
									uint8_t  Name[32];
									uint32_t Left;
									uint32_t Top;
									uint32_t Right;
									uint32_t Bottom;
									uint32_t Index;
								}
							}
							for (Count)
							{
								uint8_t Pixel[256][256][2];  // RGB565
							}
						}
						for (Value1)
						{
							uint32_t Value3;
							uint32_t Value4;
							uint32_t Value5;
							uint32_t Value6;
							uint32_t Value7;
						}
						for (FrameCount)
						{
							uint32_t Value8[3];
							uint32_t Value9;
							for (Value9)
							{
								uint32_t ValueA;
								uint32_t ValueB[2];
								uint32_t ValueC[2];
							}
						}
					}
				}
			}
		}
		uint32_t SectorAnimationCount;
		if (SectorCount >= 0)
		{
			uint32_t Value1;
			uint32_t Value2;
			if (Value1)
			{
				for (Value1)
				{
					uint32_t Value3;  // AnimationIndex
					{
						uint32_t Value4;  // uint16_t
						uint32_t Value5;  // uint16_t
						uint32_t Value6;  // uint16_t
						uint32_t Value7;  // uint16_t
					}
					uint32_t Value8;
					for (Value8)
					{
						uint32_t Value9;
						uint32_t ValueA;
					}
					uint32_t ValueB[3];  // Local Translation
					uint32_t ValueC[3][3];  // Local Rotation
				}
			}
		}
		for (SectorCount)
		{
			uint32_t Value1;
			uint32_t Value2;
			if (Value1)
			{
				for (Value1)
				{
					uint32_t Value3;  // AnimationIndex
					{
						uint32_t Value4;  // uint16_t
						uint32_t Value5;  // uint16_t
						uint32_t Value6;  // uint16_t
						uint32_t Value7;  // uint16_t
					}
					uint32_t Value8;
					for (Value8)
					{
						uint32_t Value9;
						uint32_t ValueA;
					}
					uint32_t ValueB[3];  // Local Translation
					uint32_t ValueC[3][3];  // Local Rotation
				}
			}
		}
	}
}
struct Sounds  // *.hp
{
	PbdfString Name;
	if (Name != "NEANT")
	{
		uint32_t Count;
		for (Count)
		{
			uint8_t Unknown[56];
		}
	}
}
struct Background
{
	uint32_t Value1;  // (not used?) FogDistance?
	uint32_t Value2;  // (not used?) FogIntensity?
	uint32_t Value3;  // (not used?) BackDepth?
	uint32_t Value4;  // (not used?) BackBottom?
	uint32_t hasTexture;  // uint8_t (use background texture)
	uint32_t Value6;  // uint8_t (bit0..bit2 -> RGB=0xFF)
	PbdfString Name;       // F*
	struct Texture
	{
		uint32_t Count;
		uint32_t Reserved;
		for (Count)
		{
			uint32_t Count;
			for (Count)
			{
				uint8_t  Name[32];
				uint32_t Left;
				uint32_t Top;
				uint32_t Right;
				uint32_t Bottom;
				uint32_t Index;
			}
		}
		for (Count)
		{
			uint8_t Pixel[256][256][2];  // RGB565
		}
	}
	uint32_t yStart;
	uint32_t yEnd;
}
struct Sky
{
	uint32_t hasSky;
	uint32_t yEffect;
	uint32_t Value5;
	int32_t  Value6;  // -7000..7000
	uint32_t horizonGlowStrength;
	uint32_t speed;
	PbdfString Name;  // C*
	struct Texture
	{
		uint32_t Count;
		uint32_t Reserved;
		for (Count)
		{
			uint32_t Count;
			for (Count)
			{
				uint8_t  Name[32];
				uint32_t Left;
				uint32_t Top;
				uint32_t Right;
				uint32_t Bottom;
				uint32_t Index;
			}
		}
		for (Count)
		{
			uint8_t Pixel[128][128][2];  // RGB565
		}
	}
}
struct Lensflare
{
	uint8_t Pixel[128][128][2];  // RGB565
}
uint32_t Reserved;
struct Animations2  // *.cta, *.ita, "ANIME SECTEUR"
{
	uint32_t Count;
	for ((Count + 1) / 2)
	{
		PbdfString Name;
		if (Name == "ANIME SECTEUR")
		{
			//TODO: ... (not used in my tracks)
		}
		else
		{
			// Animation2
			uint32_t AnimationCount;
			for (AnimationCount)
			{
				// Animation2TextureAnim
				PbdfString Name;
				uint32_t Count;
				for (Count)
				{
					// Animation2TextureAnimKey
					uint32_t TextureIndex;  // uint16_t
					uint32_t TexUV[8];      // uint8_t
				}
				uint32_t TotalTime;  // 16:16fp * 1000.0
				uint32_t FrameCount;
				for (FrameCount)
				{
					// Animation2TextureAnimFrame
					uint32_t AbsTime;
					uint32_t Index;
				}
			}
			uint32_t Value1;
			uint32_t SectorAnimationCount;
			for (SectorAnimationCount)
			{
				// Animation2SectorAnim
				uint32_t AnimationIndex;
				uint32_t SectorCount;
				for (SectorCount)
				{
					// Animation2SectorAnimSector
					uint32_t SectorIndex;
					uint32_t Count;
					for (Count)
					{
						// Animation2SectorAnimValue
						uint32_t Looping;    // bool
						uint32_t FaceType;   // 3 or 4 vertices
						uint32_t FaceIndex;  // into Tri- or Quad-list
					}
				}
			}
		}
	}
}
struct RepairZones
{
	PbdfString Name;  // *.lev
	if (Name != "NEANT")
	{
		uint32_t Count;
		for (Count)
		{
			fp1616_t Position1[3];
			fp1616_t Position2[3];
			fp1616_t Position3[3];
			fp1616_t Position4[3];
			fp1616_t PositionCenter[3];
			fp1616_t Value1;  // height?
			fp1616_t Value2;  // speed?
		}
		fp1616_t Value3;  // time?
	}
}
struct DesignationNormal
{
	// Designation
	{
		uint32_t Value1;  // bool
		uint32_t Reserved2;
		uint32_t Value3;
		uint32_t Value4;  // bool
		uint32_t Reserved5;
		uint32_t Reserved6;
		uint32_t Reserved7;
		uint32_t Reserved8;
		uint32_t Reserved9;
		uint32_t ValueA[5];
	}
	PbdfString Name;  // *.bal
	if (Name != "NEANT")
	{
		{
			struct Macros
			{
				uint32_t Count;
				for (Count)
				{
					// Macro(3)
					uint32_t Value1;
					uint32_t Value2;
					uint32_t Value3;
				}
			}
			uint32_t Value1;
			for (Value1)
			{
				// DesignationMacro
				uint8_t  Value2[60];
				uint32_t Value3;  // MacroIndex
				uint32_t Value4;  // MacroIndex
			}
			uint32_t Value5;
			uint32_t Value6[Value5][8];
		}
		struct GamePhases
		{
			struct Macros
			{
				uint32_t Count;
				for (Count)
				{
					uint32_t Value1;
					uint32_t Value2;
					uint32_t Value3;
				}
			}
			uint32_t Count;
			for (Count)
			{
				PbdfString Name;  // "Arrivee", "Charge", "Depart", "FinCourse"
				uint32_t MacroIndex;
			}
		}
	}
}
struct SeverityNormal
{
	uint32_t StartPointCount;
	for (StartPointCount)
	{
		uint8_t StartPoint[36];  //TODO: Analyse block
	}
}
struct SeverityNormalEasy
{
	PbdfString SeverityName;
	{
		PbdfString LevName;  // *E.lev
		if (Name != "NEANT")
		{
			uint32_t Value1;  // "Enr_Positions"
			for (Value1)
			{
				uint32_t Position[3];
			}
			uint32_t Value2;  // "Troncons"
			for (Value2)
			{
				uint32_t Value3;  // "Pistes"
				for (Count3)
				{
					uint32_t Value4;  // uint16_t
					uint32_t Value5;  // uint16_t
					uint32_t Value6;  // PositionIndex
					uint32_t Value7;
				}
			}
			PbdfString Name;
			if (Name == "PLANS CONTRAINTES")
			{
				uint32_t Value8;
				for (Value8)
				{
					uint32_t Value9;  // DesignationIndex
					uint32_t ValueA;
					uint32_t ValueB;  // Constraint/Attack
				}
			}
		}
	}
	{
		PbdfString LevName;  // *E.lev
		if (Name != "NEANT")
		{
			uint32_t Value1;
			uint32_t Value2;
			uint32_t Value3;
			uint32_t Value4;
			uint32_t Value5;
			for (Value2)
			{
				uint32_t Value6[3];
				uint32_t Value7;
				uint32_t Value8;  // Value4Index or -1
				uint32_t Value9;
				for (Value9)
				{
					uint32_t ValueA;
				}
				uint32_t ValueB;
				for (ValueB)
				{
					uint32_t ValueC;
				}
			}
			for (Value3)
			{
				uint32_t ValueD;  // Value2Index+1
				uint32_t ValueE;  // Value2Index+1
				uint32_t ValueF;
				for (ValueF)
				{
					uint32_t ValueG;
				}
				uint32_t ValueH;
				uint32_t ValueI;
				for (ValueI)
				{
					uint32_t ValueJ;
				}
			}
			for (Value4)
			{
				uint32_t ValueK;  // Value2Index+1
				uint32_t ValueL;  // Value2Index+1 or -1
				uint32_t ValueM;  // Value4Index+1 or -1
			}
		}
	}
}
/*******************************************************************************
* OffsetTable[1]
******************************************************************************/
struct SeverityNormalMedium
{
	//... see struct SeverityNormalEasy
}
/*******************************************************************************
* OffsetTable[2]
******************************************************************************/
struct SeverityNormalHard
{
	//... see struct SeverityNormalEasy
}
/*******************************************************************************
* OffsetTable[3]
******************************************************************************/
struct DesignationMirror
{
	//... see struct DesignationNormal
}
struct SeverityMirror
{
	//... see struct SeverityNormal
}
struct SeverityMirrorEasy
{
	//... see struct SeverityNormalEasy
}
/*******************************************************************************
* OffsetTable[4]
******************************************************************************/
struct SeverityMirrorMedium
{
	//... see struct SeverityNormalEasy
}
/*******************************************************************************
* OffsetTable[5]
******************************************************************************/
struct SeverityMirrorHard
{
	//... see struct SeverityNormalEasy
}
/*******************************************************************************
* OffsetTable[6]
******************************************************************************/
struct CompetitorsEasy
{
	PbdfString SeverityName;
	{
		PbdfString LevName;  // *E.lev
		if (Name != "NEANT")
		{
			uint32_t Count;
			for (Count)
			{
				PbdfString Name;
				uint32_t Value1;
				uint32_t Value2;
				PbdfString Number;
				uint32_t Value3;
				for (Value3)
				{
					uint32_t Value4[3];
				}
			}
		}
	}
}
/*******************************************************************************
* OffsetTable[7]
******************************************************************************/
struct CompetitorsMedium
{
	//... see struct CompetitorsEasy
}
/*******************************************************************************
* OffsetTable[8]
******************************************************************************/
struct CompetitorsHard
{
	//... see struct CompetitorsEasy
}