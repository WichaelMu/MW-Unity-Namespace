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

};

