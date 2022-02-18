#pragma once

#include <string>
#include <vector>

struct MW;

class Reader
{

public:

	static std::vector<MW> OpenFile();
	static void ReplaceCharacters(std::string& param, const bool& is_file_name = false);

private:

	static MW ProcessNode(const std::string& chars);

};

