
@echo .
@echo ----------------------------
@echo      POST_BUILD_EVENTS
@echo ----------------------------
@echo.

@cd ..
@cd MGenerator
@call GenerateDocs

@cd..
@cd MTest
@call BuildMTest
@call RunMTest
