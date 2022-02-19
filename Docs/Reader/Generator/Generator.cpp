
#if !_DEBUG
#include <iostream>
#include "Timer.h"
#endif

#include "Reader.h"
#include "Writer.h"

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
}
