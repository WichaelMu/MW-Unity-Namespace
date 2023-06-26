

@ECHO off

REM Location of 'Sandbox' - A Unity Environment for testing MW.
@SET DESTINATION=..\MSandbox\Assets\MW

IF NOT EXIST %DESTINATION% (
	ECHO No Sandbox Unity Environment Found!
	EXIT \b 1
)

@SET MWDLL=..\MW\Output\Binaries\Release\netstandard2.0\MW.dll
@SET MWXML=..\MW\Output\Binaries\Release\netstandard2.0\MW.xml
@SET MWEDITOR=..\MWEditor\Output\Binaries\Release\netstandard2.0\MWEditor.dll

IF NOT EXIST %MWDLL% (
	ECHO MW.dll not found! Ensure MW and other projects are properly built. Run Initialise.
	EXIT \b 1
)

IF NOT EXIST %MWXML% (
	ECHO MW.xml not found! Ensure MW and other projects are properly built and that MW exports an XML documentation file in MW Propertoes -> Build -> Output -> Documentation File. Or run Initialise.
	EXIT \b 1
)

IF NOT EXIST %MWEDITOR% (
	ECHO MWEditor.dll not found! Ensure MWEditor is built. Run Initialise.
	EXIT \b 1
)

>NUL COPY %MWDLL% %DESTINATION%
>NUL COPY %MWXML% %DESTINATION%
>NUL COPY %MWEDITOR% %DESTINATION%

@ECHO .
@ECHO MW.dll, MW.xml, and MWEditor.dll were exported to %DESTINATION%
