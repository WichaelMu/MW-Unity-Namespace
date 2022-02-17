#pragma once

#include <string>
#include <vector>

#define GENERATE_DEFAULTS this->mw_type = mw_type;\
this->mw_namespace = mw_namespace;\
this->mw_class = mw_class;\
this->mw_name = mw_name;\
this->summary = summary;\

struct MW
{
	std::string mw_type, mw_namespace, mw_class, mw_name;

	std::string summary;

	std::vector<std::string> function_parameters_type;
	std::vector<std::string> function_parameters_name;
	std::vector<std::string> function_parameters_desc;

	MW() {}

	MW(const std::string mw_type, std::string mw_namespace, std::string mw_class, std::string mw_name, std::string summary)
	{
		GENERATE_DEFAULTS
	}

	void Print()
	{

		std::cout << mw_namespace << '.' << mw_class << "::" << mw_name << '\n';
		std::cout << summary << '\n' << '\n';
		//return;
		for (int i = 0; i < function_parameters_name.size(); ++i)
		{
			std::cout << function_parameters_type[i] << ' ' << function_parameters_name[i] << '\n';
			std::cout << function_parameters_desc[i] << '\n';
		}

		std::cout << '\n' << '\n';
	}
};

