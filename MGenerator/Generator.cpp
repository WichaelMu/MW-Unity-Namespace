
#include "MMacros.h"
#if WITH_TIMER
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
#if WITH_TIMER
	PerformanceTimer t;
	t.StartTime();
#endif

	std::vector<MW> all_mw = Reader::OpenFile();
	Writer::Write(all_mw);

#if WITH_TIMER
	t.PrintTime("\nFiles Generated in:");
	std::cin.get();
#endif

	return 0;
}
