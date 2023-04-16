@ECHO off

REM Check Prerequisite binaries
@SET UNITYENGINE=../Extensions/UnityEngine.dll
@SET UNITYEDITOR=../Extensions/UnityEditor.dll
@SET TMPRO=../Extensions/Unity.TextMeshPro.dll

REM MW cannot be built without Unity binaries.
IF NOT EXIST %UNITYENGINE% (
	ECHO UnityEngine.dll not found in Extensions! Please add UnityEngine.dll from your Unity install into Extensions/
	PAUSE
	EXIT \b 1
)

IF NOT EXIST %UNITYEDITOR% (
	ECHO UnityEditor.dll not found in Extensions! Please add UnityEditor.dll from your Unity install into Extensions/
	PAUSE
	EXIT \b 1
)

IF NOT EXIST %TMPRO% (
	ECHO Unity.TextMeshPro.dll not found in Extensions! Please add Unity.TextMeshPro.dll from your Unity install into Extensions/
	PAUSE
	EXIT \b 1
)

@SET BUILD_CONFIGURATION=/p:Configuration=Release

REM Build MGenerator and Remove Unnecessary Residue
@ECHO Building MGenerator
>NUL msbuild ../MGenerator/Generator.sln %BUILD_CONFIGURATION% /p:DebugSymbols=false
>NUL DEL ..\MGenerator\Output\MGenerator.pdb

REM Build MTest
@ECHO Building MTest
>NUL dotnet msbuild ../MTest/MTest.csproj %BUILD_CONFIGURATION%

REM Build MWEditor
@ECHO Building MWEditor
>NUL dotnet msbuild ../MWEditor/MWEditor.csproj %BUILD_CONFIGURATION%

@ECHO MW Project Initialised! Building MW...

dotnet msbuild ../MW.sln %BUILD_CONFIGURATION%

@ECHO -- Initialised MW --