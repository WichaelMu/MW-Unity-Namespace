#pragma once

#include <string>
#include <map>

class SwapChars
{

public:

	static void Replace(std::string& param, const bool is_file_name = false, const bool treat_as_template = false);
	static void ReplaceAngleBrackets(std::string& param, const bool continuous = false);
	static void BuildTranslator();

private:

	static std::map<std::string, std::string> translator;

};

