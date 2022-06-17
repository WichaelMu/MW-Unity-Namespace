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
	std::vector<MW> all_mw;
#if _DEBUG && WITH_VS

	file<> file("../../Output/Release/netstandard2.0/MW.xml");

	if (!file.data())
	{
		std::cout << "No XML file!\n";
		return all_mw;
	}
#else
	const char* xml_path = "../../../../Output/Release/netstandard2.0/MW.xml";
	file<> file(xml_path);

	if (!file.data())
	{
		std::cout << "The MW.xml file at: " << xml_path << " cannot be found, or opened!\n";
		std::cout << "HTML Generator will now terminate!\n";
		std::exit(-1);
	}
#endif
	xml_document<>* doc = new xml_document<>();
	doc->parse<0>(file.data());

	xml_node<char>* members = doc->first_node()->first_node()->next_sibling();

	for (xml_node<>* member = members->first_node(); member; member = member->next_sibling())
	{
		xml_attribute<>* member_name_attribute = member->first_attribute("name");

		MW m = ProcessNode(member_name_attribute->value());

		// Everything that appears in the docs has a summary, write it here.
		m.summary = member->first_node()->value();

		const std::string docs = "docs";
		const std::string param = "param";
		const std::string returns_default = "returns";
		const std::string returns_custom = "ret";
		const std::string remarks = "remarks";

		for (xml_node<>* summary_params_etc = member->first_node(); summary_params_etc; summary_params_etc = summary_params_etc->next_sibling())
		{

			const std::string this_name = summary_params_etc->name();

			if (this_name == docs)
			{
				// Over-write the summary if a <docs> tag appears.
				// This overrides the <summary> tag.
				m.summary = summary_params_etc->value();
			}
			else if (this_name == param)
			{
				// <param name="name_of_parameter">description</param>

				// Add the name of the parameters.
				m.function_parameters_name.push_back(summary_params_etc->first_attribute()->value());

				// Add the description of the parameters.
				m.function_parameters_desc.push_back(summary_params_etc->value());
			}
			else if (this_name == returns_custom)
			{
				// <returns>value</returns>
				m.returns = summary_params_etc->value();
			}
			else if (this_name == returns_default)
			{
				if (m.returns.length() == 0)
				{
					m.returns = summary_params_etc->value();
				}
			}
			else if (this_name == remarks)
			{
				// <remarks>remarks</remarks>

				m.remarks = summary_params_etc->value();
			}
		}

		m.Print();

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

	// Count the number of periods in chars.
	// This is <member name="..."></member> Where ... is chars.
	// If there are exactly two periods, we know that this member is part of the base MW namespace.
	// 
	// Also, do not go past a '(', this will indicate a function and the counting will be inaccurate,
	//	if included.
	uint8_t periods = 0;
	for (size_t i = 0; i < chars.length() && chars[i] != '('; ++i)
	{
		if (chars[i] == '.')
			periods++;
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
				// If the number of periods are NOT exactly two, continue as normal.
				if (periods != 2)
				{
					++iteration;
				}
				else
				{
					// If periods is exactly two, the namespace IS the class and the 
					// namespace is the base MW.

					// Skip to writing the name of a property, or a function and it's 
					// parameters, if any.
					iteration = 2;
				}
			}
		}
	}

	ReplaceCharacters(mw.mw_namespace, true);
	ReplaceCharacters(mw.mw_class);
	ReplaceCharacters(mw.mw_name);

	if (mw.mw_class.length() == 0)
		mw.mw_class = mw.mw_namespace;

	return mw;
}

void Reader::ReplaceCharacters(std::string& param, const bool& is_file_name)
{
	param.erase(remove(param.begin(), param.end(), ','), param.end());
	param.erase(remove(param.begin(), param.end(), '1'), param.end());

	// Hard-coded replacements.
	if (param == "Single")
	{
		param = "float";
	}
	else if (param == "Boolean")
	{
		param = "bool";
	}
	else if (param == "Int32")
	{
		param = "int";
	}
	else if (param == "Int64")
	{
		param = "long";
	}
	else if (param == "UInt32")
	{
		param = "uint";
	}
	else
	{
		if (!is_file_name)
		{
			// The @ character signifies a ref or out keyword in C#.
			// In C++, this is the '&' character.
			// Both 'out' and 'ref' are used throughout the MW namespace.
			// It is impossible to determine which one the '@' represents,
			// so just use & to signify an 'out' OR 'ref' parameter.
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
						new_param += "T";
						param.erase(remove(param.begin(), param.end(), '1'), param.end());
						break;
					case '2':
						new_param = "T";
						param.erase(remove(param.begin(), param.end(), '2'), param.end());
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

			// Remove the trailing curly bracket that exists, for some reason.
			// Also, can't use the remove method, like above, for some reason.
			pos = new_param.find('}');
			if (pos != std::string::npos)
				new_param.replace(pos, 1, "");

			param = new_param;
		}
	}

	param.erase(remove(param.begin(), param.end(), '`'), param.end());
}