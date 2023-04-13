#pragma once

#include <string>
#include <vector>

struct MW;

class Reader
{

public:

	static std::vector<MW> OpenFile();

private:

	static MW ProcessNode(const std::string& chars);
	static void ProcessPredefinedGenericType(std::string& param);

	static bool FileExists(const char* file_name);

};

