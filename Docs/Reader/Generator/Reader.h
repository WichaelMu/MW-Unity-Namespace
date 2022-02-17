#pragma once

#include "MW.h"

class Reader
{

public:

	static void OpenFile();

private:

	static MW ProcessNode(const std::string& chars);

};

