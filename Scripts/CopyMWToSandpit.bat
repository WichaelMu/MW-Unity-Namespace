

@ECHO off

REM Location of 'Sandpit' - A Unity Environment for testing MW.
@SET DESTINATION=..\MSandpit\MSandpit\Assets\MW

IF NOT EXIST %DESTINATION% (
	ECHO No Sandpit Unity Environment Found!
	EXIT \b 1
)

>NUL COPY ..\MW\Output\Binaries\Release\netstandard2.0\MW.dll %DESTINATION%
>NUL COPY ..\MW\Output\Binaries\Release\netstandard2.0\MW.xml %DESTINATION%
>NUL COPY ..\MWEditor\Output\Binaries\Release\netstandard2.0\MWEditor.dll %DESTINATION%

@ECHO .
@ECHO MW.dll, MW.xml, and MWEditor.dll were exported to %DESTINATION%