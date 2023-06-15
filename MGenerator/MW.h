#pragma once

#include <string>
#include <vector>

#include "MMacros.h"

#define GENERATE_DEFAULTS() this->mw_type = mw_type;\
this->mw_namespace = mw_namespace;\
this->mw_class = mw_class;\
this->mw_name = mw_name;\
this->summary = summary;\
this->implicit = "";

struct MW
{
	std::string mw_type, mw_namespace, mw_class, mw_name;

	std::string summary;
	std::string returns;
	std::string remarks;

	// Parameter information.
	VT(std::string) function_parameters_type;
	VT(std::string) function_parameters_name;
	VT(std::string) function_parameters_desc;

	std::string implicit;

	VT(std::string) decorations;

	MW() {}

	MW(const std::string mw_type, std::string mw_namespace, std::string mw_class, std::string mw_name, std::string summary)
	{
		GENERATE_DEFAULTS()
	}

	__forceinline bool IsOverloadedOperator() const
	{
		if (mw_name.length() > 8)
		{
			std::string first_eight = mw_name.substr(0, 8);
			
			return first_eight == "operator";
		}

		return false;
	}

#if PRINT_DEBUG_MSGS
#define VECTOR_SIZE(v) v.size()

	void Print()
	{
		std::cout << VECTOR_SIZE(function_parameters_type) << " " << VECTOR_SIZE(function_parameters_name) << " " << VECTOR_SIZE(function_parameters_desc) << " " << mw_namespace << " " << mw_class << " " << mw_name << '\n';
		std::cout << mw_namespace << '.' << mw_class << "::" << mw_name << '\n';
		std::cout << summary << '\n' << '\n';

		for (int i = 0; i < function_parameters_name.size(); ++i)
		{
			std::cout << function_parameters_type[i] << ' ' << function_parameters_name[i] << '\n';
			std::cout << function_parameters_desc[i] << '\n';
		}

		std::cout << '\n' << '\n';
	}
#else
	void Print() {  }
#endif
};

