
@SET BUILDCONFIGURATION=%1

if %BUILDCONFIGURATION% == Standalone (
	@ECHO -- STANDALONE BUILD --
	EXIT 0
)

@ECHO .
@ECHO ----------------------------
@ECHO      POST BUILD EVENTS
@ECHO ----------------------------
@ECHO .

@CD ..
@CD MGenerator
@CALL GenerateDocs

@CD ..
@CD MTest
@CALL RunMTest

@CD ..
@CD Scripts
@CALL CopyMWToSandpit