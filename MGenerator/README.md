# MGenerator

Welcome to source code that automatically executes to generate the documentation for the MW Unity Namespace!

The result of MGenerator can be found [here](https://wichaelmu.github.io/MW-Unity-Namespace/)!

The process of automatically generating documentation begins in the Post-Build Event after MW is built. The Script in `../Scripts/PostBuildEvent.bat` commences a sequence of instructions that may cause the main MW build to fail if any errors occur.

MGenerator uses [RapidXml](https://rapidxml.sourceforge.net/).

MGenerator begins execution in `int main()` in `Generator.cpp`, then the `Reader` parses the MW.xml documentation file. The result of the parse is then handed over to `Writer` which generates `.html` files.
```cpp
int main()
{
	Reader::OpenFile();
	Writer::Write();
}
```
Note that some source code has been omitted for clarity.

For MGenerator to function properly, MGenerator needs to be built first, before MW. This is so that the MGenerator binaries exist. Afterwards, MW can be built can create the `MW.xml` file for MGenerator to parse and convert.