
IF NOT EXIST Output (
	ECHO MGenerator has no Output folder! Build MGenerator, or run Initialise.
	EXIT \b 1
)

@echo.
@echo -- Running Documentation Generator --
@echo.

IF NOT EXIST Output\MGenerator.exe (
	ECHO MGenerator Binary not found! Build MGenerator, or run initialise.
	EXIT \b 1
)

@cd Output
@MGenerator.exe
@cd ..
