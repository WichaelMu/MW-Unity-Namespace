#pragma once

#include <string>

class SwapChars
{

public:

	static void Replace(std::string& param, const bool& is_file_name = false);
	static void ReplaceAngleBrackets(std::string& param, const bool& continuous = false);
};

