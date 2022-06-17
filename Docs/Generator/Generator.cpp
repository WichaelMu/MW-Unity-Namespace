
#if !_DEBUG
#include <iostream>
#include "Timer.h"
#endif

#include "Reader.h"
#include "Writer.h"

/* 
* Do not run in Visual Studio with the 'Release' Configuration.
* 
* Building MW should automatically call Generator.
*/

int main()
{
#if !_DEBUG
	PerformanceTimer t;
	t.StartTime();
#endif

	std::vector<MW> all_mw = Reader::OpenFile();
	Writer::Write(all_mw);

#if !_DEBUG
	t.PrintTime("\n\nFull Suite");
	std::cin.get();
#endif

	return 0;
}
