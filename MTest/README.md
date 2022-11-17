# MTest Automatic Testing

Welcome to the source code that automatically executes to test the accuracy and correctness of the core features of the MW Unity Namespace!

Automated testing begins execution in the Post-Build Event after MW is built. The Script in `../Scripts/PostBuildEvent.bat` commences a sequence of instructions that may cause the main MW build to fail if any errors occur.

MTest uses MSTest.