@ECHO off

REM Check for required binaries to compile MW.
>NUL WHERE msbuild
IF %ERRORLEVEL% NEQ 0 (
	ECHO The command 'msbuild' is unrecognised. Install and/or add 'msbuild' to your system environment.
	PAUSE
	EXIT \b 1
)

>NUL WHERE dotnet
if %ERRORLEVEL% NEQ 0 (
	ECHO The command 'dotnet' is unrecognised. Install and/or add 'dotnet' to your system environment.
	PAUSE
	EXIT \b 1
)

@SET VS_BUILDCONFIGURATION=%1
@SET BUILD_CONFIGURATION=/p:Configuration=%VS_BUILDCONFIGURATION%

IF %VS_BUILDCONFIGURATION%X==X (
	@SET VS_BUILDCONFIGURATION=Release
)

IF %VS_BUILDCONFIGURATION%==Standalone (
	call :BuildMW
	PAUSE
	EXIT \b 0
)

IF %VS_BUILDCONFIGURATION% NEQ Release (
	ECHO Unrecognised Build Configuration. Valid values are 'Release' and 'Standalone'.
	PAUSE
	EXIT \b 1
)

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



REM Build MGenerator and Remove Unnecessary Residue
@ECHO Building MGenerator
>NUL msbuild ../MGenerator/Generator.sln %BUILD_CONFIGURATION% /p:DebugSymbols=false
>NUL DEL ..\MGenerator\Output\MGenerator.pdb

REM Build MTest
@ECHO Building MTest
>NUL dotnet build ../MTest/MTest.csproj %BUILD_CONFIGURATION%

REM Build MWEditor
@ECHO Building MWEditor
>NUL dotnet build ../MWEditor/MWEditor.csproj %BUILD_CONFIGURATION%

@ECHO MW Project Initialised! Building MW...

:BuildMW
@dotnet build ../MW.csproj %BUILD_CONFIGURATION%

@ECHO -- Initialised MW --
