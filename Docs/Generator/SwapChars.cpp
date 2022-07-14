#include "SwapChars.h"


void SwapChars::Replace(std::string& param, const bool& is_file_name)
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
	// Reference/Out Types.
	else if (param == "Single@")
	{
		param = "float&";
	}
	else if (param == "Boolean@")
	{
		param = "bool&";
	}
	else if (param == "Int32@")
	{
		param = "int&";
	}
	else if (param == "Int64@")
	{
		param = "long&";
	}
	else if (param == "UInt32@")
	{
		param = "uint&";
	}
	else
	{
		if (!is_file_name)
		{
			// This is an overloaded operator.
			if (param.length() > 3 && param[2] == '_')
			{
				if (param == "op_Addition")
					param = "operator+";
				else if (param == "op_Subtraction")
					param = "operator-";
				else if (param == "op_UnaryNegation")
					param = "operator-";
				else if (param == "op_Multiply")
					param = "operator*";
				else if (param == "op_Division")
					param = "operator/";
				else if (param == "op_ExclusiveOr")
					param = "operator^";
				else if (param == "op_BitwiseOr")
					param = "operator|";
				else if (param == "op_GreaterThan")
					param = "operator&gt;";
				else if (param == "op_LessThan")
					param = "operator&lt;";
				else if (param == "op_RightShift")
					param = "operator&gt;&gt;";
				else if (param == "op_LeftShift")
					param = "operator&lt;&lt";
				else if (param == "op_Equality")
					param = "operator=";
				else if (param == "op_Inequality")
					param = "operator!=";
			}
			else
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

		if (param[param.length() - 1] == '.')
		{
			param.erase(param.length() - 1, 1);
		}
	}

	param.erase(remove(param.begin(), param.end(), '`'), param.end());
}