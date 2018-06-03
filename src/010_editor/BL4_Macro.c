struct MacroList; struct Macro;

typedef struct // MacroList
{
	uint lenMacroBase;
	if (lenMacroBase) Macro macroBase(3)[lenMacroBase] <optimize = true>;
	uint lenMacro;
	if (lenMacro) Macro macro(1)[lenMacro] <optimize = true>;
	uint lenMacroInit;
	if (lenMacroInit) Macro macroInit(1)[lenMacroInit] <optimize = true>;
	uint lenMacroActive;
	if (lenMacroActive) Macro macroActive(1)[lenMacroActive] <optimize = true>;
	uint lenMacroDesactive;
	if (lenMacroDesactive) Macro macroDesactive(1)[lenMacroDesactive] <optimize = true>;
	uint lenMacroRemplace;
	if (lenMacroRemplace) Macro macroRemplace(2)[lenMacroRemplace] <optimize = true>;
	uint lenMacroEchange;
	if (lenMacroEchange) Macro macroEchange(2)[lenMacroEchange] <optimize = true>;
} MacroList;

typedef struct(int numValue) // Macro
{
	int values[numValue];
} Macro;
