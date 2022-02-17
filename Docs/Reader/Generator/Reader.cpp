#include <iostream>
#include <fstream>
#include <string>

#include "Reader.h"

#include "XML/rapidxml.hpp"
#include "XML/rapidxml_print.hpp"
#include "XML/rapidxml_utils.hpp"

using namespace rapidxml;

void Reader::OpenFile()
{
	file<> file("../../../bin/Debug/netstandard2.0/MW.xml");
	xml_document<>* doc = new xml_document<>();
	doc->parse<0>(file.data());

	xml_node<char>* members = doc->first_node()->first_node()->next_sibling();

	for (xml_node<>* member = members->first_node(); member; member = member->next_sibling())
	{
		xml_attribute<>* member_name_attribute = member->first_attribute("name");

		MW m = ProcessNode(member_name_attribute->value());

		m.summary = member->first_node()->value();

		if (m.function_parameters_type.size() > 0)
		{
			for (xml_node<>* summary_params_etc = member->first_node(); summary_params_etc; summary_params_etc = summary_params_etc->next_sibling())
			{
				const std::string param = "param";
				if (summary_params_etc->name() != param)
					continue;

				std::string cref_value;

				if (summary_params_etc->first_attribute())
				{
					m.function_parameters_name.push_back(summary_params_etc->first_attribute()->value());
					cref_value = summary_params_etc->first_attribute()->value();
					cref_value += " ";
				}
				else
				{
					m.function_parameters_name.push_back("");
				}

				cref_value += summary_params_etc->value();

				m.function_parameters_desc.push_back(summary_params_etc->value());
			}
		}

		m.Print();
	}

	delete doc;
}

MW Reader::ProcessNode(const std::string& chars)
{
	MW mw;

	switch (chars[0])
	{
	case 'T':
		mw.mw_type = "TYPE: ";
		break;
	case 'P':
		mw.mw_type = "PROPERTY: ";
		break;
	case 'F':
		mw.mw_type = "FIELD: ";
		break;
	case 'M':
		mw.mw_type = "MEMBER: ";
		break;
	}

	for (int i = 5, iteration = 0; i < chars.length(); ++i)
	{
		if (chars[i] != '.')
		{
			switch (iteration)
			{
			case 0:
				mw.mw_namespace += chars[i];
				break;
			case 1:
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
					for (int k = i + 1; k < chars.length(); ++k)
					{
						if (chars[k] != ')')
							param += chars[k];

						if (chars[k] == '.')
							param = "";

						if (chars[k] == ',' || chars[k] == ')')
							mw.function_parameters_type.push_back(param);

					}

					i = chars.length() + 1;
				}
				break;
			}
		}
		else
		{
			++iteration;
		}
	}

	return mw;
}
