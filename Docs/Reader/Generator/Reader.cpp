#include <iostream>
#include <fstream>

#include "Reader.h"

#include "XML/rapidxml.hpp"
#include "XML/rapidxml_print.hpp"
#include "XML/rapidxml_utils.hpp"

using namespace rapidxml;

#include "MW.h"

#if BUILD
#include "Timer.h"
#endif

std::vector<MW> Reader::OpenFile()
{
#if BUILD
	PerformanceTimer t;
	t.StartTime();
#endif

	std::vector<MW> all_mw;
#if _DEBUG
	file<> file("../../../bin/Debug/netstandard2.0/MW.xml");

	if (!file.data())
	{
		std::cout << "No XML file!\n";
		return all_mw;
	}
#else
	const char* xml_path = "C:/Users/table/Documents/Machine Code/MW/MW/bin/Debug/netstandard2.0/MW.xml";
	file<> file(xml_path);
	
	if (!file.data())
	{
		std::cout << "The MW.xml file at: " << xml_path << " cannot be found, or opened!\n";
		std::cout << "HTML Generator will not terminate!\n";
		std::exit(-1);
	}
#endif
	xml_document<>* doc = new xml_document<>();
	doc->parse<0>(file.data());

#if BUILD
	t.PrintTime("Reading XML File");
#endif

	xml_node<char>* members = doc->first_node()->first_node()->next_sibling();

	for (xml_node<>* member = members->first_node(); member; member = member->next_sibling())
	{
#if BUILD
		t.StartTime();
#endif
		xml_attribute<>* member_name_attribute = member->first_attribute("name");

		MW m = ProcessNode(member_name_attribute->value());

		m.summary = member->first_node()->value();

		for (xml_node<>* summary_params_etc = member->first_node(); summary_params_etc; summary_params_etc = summary_params_etc->next_sibling())
		{
			const std::string param = "param";
			if (summary_params_etc->name() != param)
				continue;

			if (summary_params_etc->first_attribute())
			{
				// Add the name of the parameters.
				m.function_parameters_name.push_back(summary_params_etc->first_attribute()->value());
			}
			else
			{
				m.function_parameters_name.push_back("");
			}

			m.function_parameters_desc.push_back(summary_params_etc->value());
		}

		m.Print();
#if BUILD
		t.PrintTime("Processing");
#endif
		all_mw.push_back(m);
	}

	delete doc;

	return all_mw;
}

MW Reader::ProcessNode(const std::string& chars)
{
	MW mw;

	// Determine what this Node is based off the first character.
	switch (chars[0])
	{
	case 'T':
		mw.mw_type = "TYPE";
		break;
	case 'P':
		mw.mw_type = "PROPERTY";
		break;
	case 'F':
		mw.mw_type = "FIELD";
		break;
	case 'M':
		mw.mw_type = "MEMBER";
		break;
	}

	// Skip 5 characters. E.g., T:MW.MArray will skip the T:MW. and begin at 'M' in MArray.
	for (size_t i = 5, iteration = 0; i < chars.length(); ++i)
	{
		if (chars[i] != '.' && chars[i] != '#')
		{
			switch (iteration)
			{
			case 0:
				// Classes in the global MW namespace will be reference in mw_namespace.
				mw.mw_namespace += chars[i];
				break;
			case 1:
				// Classes in the global MW namespace will not have the name of the
				// class stored in here. It will be in mw_namespace.
				// Only classes in an extension of MW.* will have their classes stored here.
				mw.mw_class += chars[i];
				break;
			case 2:
				if (chars[i] != '(')
				{
					mw.mw_name += chars[i];
				}
				else
				{
					std::string param = "";
					for (size_t k = i + 1; k < chars.length(); ++k)
					{
						if (chars[k] != ')')
							param += chars[k];

						if (chars[k] == '.')
							param = "";

						// Checks if a parameter has been termined. (1, 2). Adds only 1 and 2.
						if (chars[k] == ',' || chars[k] == ')')
						{
							ReplaceCharacters(param);

							// Add the type of the parameter after replacing illegals.
							mw.function_parameters_type.push_back(param);
						}
					}

					i = chars.length() + 1;
				}

				break;
			}
		}
		else
		{
			if (chars[i] == '#')
			{
				mw.mw_name = "CONSTRUCTOR";

				// Constructors are marked by '#CTOR'.
				// If this character is a '#', it denotes the start of a Constructor declaration.
				// Set mw_name to CONSTRUCTOR and skip 4 characters, the 'CTOR'.
				i += 4;
			}
			else
			{
				++iteration;
			}
		}
	}

	ReplaceCharacters(mw.mw_namespace, true);
	ReplaceCharacters(mw.mw_class);
	ReplaceCharacters(mw.mw_name);

	return mw;
}

void Reader::ReplaceCharacters(std::string& param, const bool& is_file_name)
{
	param.erase(remove(param.begin(), param.end(), ','), param.end());
	param.erase(remove(param.begin(), param.end(), '{'), param.end());
	param.erase(remove(param.begin(), param.end(), '}'), param.end());
	param.erase(remove(param.begin(), param.end(), '1'), param.end());
	param.erase(remove(param.begin(), param.end(), '3'), param.end());

	if (param == "Single")
	{
		param = "float";
		return;
	}

	if (!is_file_name)
	{
		// The @ character signifies a ref or out keyword in C#.
		// In C++, this is the '&' character.
		// Both 'out' and 'ref' are used throughout the MW namespace.
		// It is impossible to determine which one the '@' represents,
		// so just use this to signify an 'out' OR 'ref' parameter.
		const std::string ref = "& ";

		std::string new_param = "";
		for (int i = 0; i < param.length(); ++i)
		{
			switch (param[i])
			{
			case '`':
				switch (param[i + 1])
				{
				case '0':
					new_param += "T";
					param.erase(remove(param.begin(), param.end(), '0'), param.end());
					break;
				case '1':
					new_param += "&lt;T&gt;";
					break;
				case '2':
					new_param += "&lt;T&gt;&lt;Y&gt;";
					break;
				}

				break;
			case '@':
				
				new_param += ref;
				break;
			default:
				new_param += param[i];
				break;
			}
		}

		size_t pos = new_param.find('{');
		if (pos != std::string::npos)
			new_param.replace(pos, 1, "&lt;");

		pos = new_param.find('}');
		if (pos != std::string::npos)
			new_param.replace(pos, 1, "&gt;");

		param = new_param;
	}

	param.erase(remove(param.begin(), param.end(), '`'), param.end());
}
