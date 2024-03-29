#include <iostream>
#include <sys/stat.h>

#include "Reader.h"
#include "SwapChars.h"

#include "XML/rapidxml.hpp"
#include "XML/rapidxml_print.hpp"
#include "XML/rapidxml_utils.hpp"

using namespace rapidxml;

#include "MW.h"
#include "MMacros.h"

#if BUILD
#include "Timer.h"
#endif

std::vector<MW> Reader::OpenFile()
{
	std::vector<MW> all_mw;

#if EXEC_FROM_VS
	const char* xml_path = "../MW/Output/Binaries/Release/netstandard2.0/MW.xml";
#else
	const char* xml_path = "../../MW/Output/Binaries/Release/netstandard2.0/MW.xml";
#endif

	if (!FileExists(xml_path))
	{
		std::cout << "The MW.xml file at: " << xml_path << " cannot be found, or opened!\n";
		std::cout << "HTML Generator will now terminate!\n";
#if !EXEC_FROM_VS
		std::cout << std::endl;
#endif
		std::exit(-1);
	}

	file<> file(xml_path);

	xml_document<>* doc = new xml_document<>();
	doc->parse<0>(file.data());

	SwapChars::BuildTranslator();

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
		const std::string returns_custom = "docreturns";
		const std::string remarks = "remarks";
		const std::string doc_remarks = "docremarks";
		const std::string decorations = "decorations";

		/*
		* When using tags that override the normal XML tags, ensure the custom
		  tag is checked before the normal tag. Before writing values with the
		  normal tag, check if the value's length is not zero.

		  See returns_custom and returns_default and doc_remarks and remarks
		  for an example.
		*/

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
				// <docreturns>custom return value</docreturns>
				m.returns = summary_params_etc->value();
			}
			else if (this_name == returns_default)
			{
				// <returns>return value</returns>

				if (m.returns.length() == 0)
				{
					m.returns = summary_params_etc->value();
				}
			}
			else if (this_name == doc_remarks)
			{
				// <docremarks>doc remarks</docremarks>
				m.remarks = summary_params_etc->value();
			}
			else if (this_name == remarks)
			{
				// <remarks>remarks</remarks>
				m.remarks = summary_params_etc->value();
			}
			else if (this_name == decorations)
			{
				// <decorations name="value"...></decorations>

				std::string ReplacedAngleBrackets = summary_params_etc->first_attribute()->value();
				SwapChars::ReplaceAngleBrackets(ReplacedAngleBrackets, true);
				m.decorations.push_back(ReplacedAngleBrackets);
			}
		}

#if WRITE_NO_DECORATIONS
		if (!m.decorations.size() && m.mw_type == MEMBER && m.mw_name != "CONSTRUCTOR")
		{
			std::cout << m.mw_namespace << '.' << m.mw_class << '.' << m.mw_name << " has no decorations!\n";
		}
#endif // WRITE_NO_DECORATIONS

		m.Print();

		all_mw.push_back(m);
	}

#if WRITE_NO_DECORATIONS
	std::cout << "Decoration checks complete!\n\n";
#endif // WRITE_NO_DECORATIONS

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
		mw.mw_type = TYPE;
		break;
	case 'P':
		mw.mw_type = PROPERTY;
		break;
	case 'F':
		mw.mw_type = FIELD;
		break;
	case 'M':
		mw.mw_type = MEMBER;
		break;
	}

	// Count the number of periods in chars.
	// This is <member name="..."></member> Where ... is chars.
	// If there are exactly two periods, this Node is part of the base MW namespace.
	// 
	// Also, do not go past a '(', this will indicate a function and the counting will be inaccurate,
	//	if included.
	uint8_t periods = 0, period_count = 0;
	for (uint8_t i = 0; i < chars.length() && chars[i] != '('; ++i)
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
				// Classes in the global MW namespace will be referenced in mw_namespace.
				mw.mw_namespace += chars[i];
				break;
			case 1:
				// Classes in the global MW namespace will not have the name of the
				// class stored in here. It will be in mw_namespace.
				// Only classes in an extension of MW.* will have their classes stored here.
				mw.mw_class += chars[i];
				break;
			case 2: // Function and its parameters.
				if (chars[i] != '(')
				{
					mw.mw_name += chars[i];
				}
				else
				{
					std::string param = "";
					bool is_predefined_generic_type = false; // E.g., MW.MArray<MW.Kinetic.ProjectileArcCollision> Collisions.
					for (size_t k = i + 1; k < chars.length(); ++k)
					{
						if (chars[k] != ')')
							param += chars[k];

						if (chars[k] == '{' && chars[k + 1] != '`')
							is_predefined_generic_type = true;

						if (chars[k] == '.' && !is_predefined_generic_type)
							param = "";

						// Checks if a parameter has been terminated. (1, 2). Adds only 1 and 2.
						if (chars[k] == ',' || chars[k] == ')')
						{
							// If we're not a custom generic type, continue as normal...
							if (!is_predefined_generic_type)
							{
								SwapChars::Replace(param);

								// Add the type of the parameter after replacing illegals.
								mw.function_parameters_type.push_back(param);
							}
							// Otherwise, process the generic type *first*, then replace and continue as normal...
							else
							{
								ProcessPredefinedGenericType(param);
								SwapChars::Replace(param);

								mw.function_parameters_type.push_back(param);
								is_predefined_generic_type = false;
							}

							// Some parameters don't have the 'System.' prefix. (System.Int32, System.String, System.Single).
							// Some parameters are generic '``0' and are represented only with ('``0') (no System.).
							// This system relies on checking for '.' periods for function parameters.
							// Without this assignment, it will fail.
							param = "";
						}
					}

					i = chars.length() + 1;
				}
				break;
			case 3: // Implicit operator.
				std::string implicit_from_to;

				if (chars[i] == '(')
				{
					for (size_t k = i + 1; k < chars.length(); ++k)
					{
						if (chars[k] != ')')
							implicit_from_to += chars[k];

						if (chars[k] == '.')
							implicit_from_to = "";

						if (chars[k] == ')' || k == chars.length() - 1)
						{
							mw.implicit += implicit_from_to;
						}

						if (chars[k] == '~')
						{
							implicit_from_to = "";
							mw.implicit += " -> ";
						}
					}
				}

				SwapChars::Replace(mw.implicit);

				break;
			}
		}
		else
		{
			// Constructor check.
			if (chars[i] == '#')
			{
				mw.mw_name = "CONSTRUCTOR";

				// Constructors are marked by '#CTOR'.
				// If this character is a '#', it denotes the start of a Constructor declaration.
				// Set mw_name to CONSTRUCTOR and skip 4 characters, the 'CTOR'.
				i += 4;
			}
			// Everything else.
			else
			{
				++period_count;

				if (mw.mw_type == TYPE)
				{
					if (period_count == periods - 1)
					{
						++iteration;
					}
					else
					{
						mw.mw_namespace += '.';
					}
				}
				else if (period_count != periods - 1)
				{
					/*
					* Continue as normal IF:
					*	- We are not in the global MW namespace (periods != 2).
					*	- We are the second-last period. (period_count == periods - 2).
					*		- Consider: <member name="M:MW.Math.Magic.Fast.InverseSqrt(...)">
					*		- The namespace continues until we are the second-last period;
					*			* Namespace = MW.Math.Magic (Math.Magic in Docs).
					*			* Continue onwards where Class = Fast...
					*/
					if (periods != 2 && period_count == periods - 2)
					{
						++iteration;
					}
					/*
					* Consider: <member name="M:MW.Math.Magic.Fast.InverseSqrt(...)">
					*
					* If we have reached the first period (the period in Math.Magic), do not increment
					* out iteration and instead add a period in the Namespace for Docs.
					*
					* This *should* support namespaces branching through multiple levels.
					*/
					else
					{
						if (period_count != periods - 1)
							mw.mw_namespace += '.';
					}

					// The iteration can only go to 3 if it meets the conditions below.
					if (iteration == 3)
						break;
				}
				else
				{
					// Checks if i + 5 is within the string.
					// An substring of length 5 will present 'op_Im' which means that this is an op_Implicit (implicit operator).
					// These 5 characters are all we need to identify an implicit operator.
					// TODO: An implicit operator always has a '_' at position 3, a 'p' in 6 and 'y' in 11. We can make this faster.
					if (i + 6 < chars.length() && chars[i + 3] == '_' && chars[i + 6] == 'p')
					{
						// This is just here so that Writer doesn't consider this Implicit Operator as a Class.
						mw.mw_name = "Implicit Operator: ";

						// This is an implicit operator.
						i += 11; // Skip op_Implicit (11 characters, go straight to the parameter [beginning with a '('] ).
						iteration = 3;
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
	}

	mw.mw_namespace.erase(remove(mw.mw_namespace.begin(), mw.mw_namespace.end(), '1'), mw.mw_namespace.end());
	mw.mw_namespace.erase(remove(mw.mw_namespace.begin(), mw.mw_namespace.end(), '2'), mw.mw_namespace.end());
	mw.mw_namespace.erase(remove(mw.mw_namespace.begin(), mw.mw_namespace.end(), '3'), mw.mw_namespace.end());
	SwapChars::Replace(mw.mw_namespace, true);
	SwapChars::Replace(mw.mw_class);
	SwapChars::Replace(mw.mw_name, false, true);

	if (mw.mw_class.length() == 0)
		mw.mw_class = mw.mw_namespace;

	return mw;
}

void Reader::ProcessPredefinedGenericType(std::string& param)
{
	size_t index_of_angle_bracket = std::string::npos;
	size_t index_of_dot = std::string::npos;

	for (size_t i = 0; i < param.length(); ++i)
	{
		if (param[i] == '{')
			index_of_angle_bracket = i;
		else if (param[i] == '.')
			index_of_dot = i;
	}

	if (index_of_angle_bracket != std::string::npos && index_of_dot != std::string::npos)
		param.erase(param.begin() + index_of_angle_bracket + 1, param.begin() + index_of_dot + 1);
}

bool Reader::FileExists(const char* file_name)
{
	struct stat buffer;
	return (stat(file_name, &buffer) == 0);
}
