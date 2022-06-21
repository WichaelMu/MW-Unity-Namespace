#pragma once

#include <iostream>
#include <vector>

#include "MW.h"
#include "MMacros.h"

class Writer
{

public:

	static void Write(const std::vector<MW>& all_mw);

private:

	static std::string GetDecorations(const VT(std::string)& decorations);
};

