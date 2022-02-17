

#include "Reader.h"
#include "Writer.h"

int main()
{
	std::vector<MW> all_mw = Reader::OpenFile();
	Writer::Write(all_mw);
}
