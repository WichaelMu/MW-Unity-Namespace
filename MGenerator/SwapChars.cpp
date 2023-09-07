#include "SwapChars.h"


void SwapChars::Replace(std::string& param, const bool is_file_name, const bool treat_as_template)
{
	if (!treat_as_template)
	{
		param.erase(remove(param.begin(), param.end(), ','), param.end());
	}

	// Hard-coded replacements.
	if (translator.count(param))
	{
		param = translator[param];
	}
	else
	{
		if (!is_file_name)
		{
			// This is an overloaded operator.
			if (param.length() > 3 && param[2] == '_' && translator.count(param))
			{
				param = translator[param];
			}
			else
			{
				// The @ character signifies a ref or out keyword in C#.
				// In C++, this is the '&' character.
				// Both 'out' and 'ref' are used throughout the MW namespace.
				// It is impossible to determine which one the '@' represents,
				// so just use & to signify an 'out' OR 'ref' parameter.
				const std::string ref = "&";

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

				ReplaceAngleBrackets(new_param);

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

void SwapChars::ReplaceAngleBrackets(std::string& param, const bool continuous)
{
	if (continuous)
	{
		const size_t length = param.length();
		for (size_t i = 0; i < length; ++i)
		{
			size_t pos = param.find('{');
			if (pos != std::string::npos)
				param.replace(pos, 1, "&lt;");

			pos = param.find('}');
			if (pos != std::string::npos)
				param.replace(pos, 1, "&gt;");
		}
	}
	else
	{
		size_t pos = param.find('{');
		if (pos != std::string::npos)
			param.replace(pos, 1, "&lt;");

		pos = param.find('}');
		if (pos != std::string::npos)
			param.replace(pos, 1, "&gt;");

		// Remove the trailing curly bracket that exists, for some reason.
		// Also, can't use the remove method, like above, for some reason.
		pos = param.find('}');
		if (pos != std::string::npos)
			param.replace(pos, 1, "");
	}
}


std::map<std::string, std::string> SwapChars::translator;

void SwapChars::BuildTranslator()
{
	translator["Single"] = "float";
	translator["Boolean"] = "bool";
	translator["Int32"] = "int";
	translator["String"] = "string";
	translator["Int64"] = "long";
	translator["UInt64"] = "uint";
	translator["Double"] = "double";
	translator["SByte"] = "sbyte";
	translator["Int16"] = "short";

	// Reference/Out Types.
	translator["Single@"] = "float&";
	translator["Boolean@"] = "bool&";
	translator["Int32@"] = "int&";
	translator["Int64@"] = "long&";
	translator["UInt32@"] = "uint&";
	translator["Double@"] = "double&";
	translator["SByte@"] = "sbyte&";
	translator["Int16@"] = "short&";

	// Array / Params.
	translator["Single[]"] = "float[]";
	translator["Boolean[]"] = "bool[]";
	translator["Int32[]"] = "int[]";
	translator["Int64[]"] = "long[]";
	translator["UInt32[]"] = "uint[]";
	translator["Double[]"] = "double[]";
	translator["SByte[]"] = "sbyte[]";
	translator["Int16[]"] = "short[]";

	translator["op_Addition"] = "operator+";
	translator["op_Subtraction"] = "operator-";
	translator["op_UnaryNegation"] = "operator-";
	translator["op_Multiply"] = "operator*";
	translator["op_Division"] = "operator/";
	translator["op_ExclusiveOr"] = "operator^";
	translator["op_BitwiseOr"] = "operator|";
	translator["op_BitwiseAnd"] = "operator&";
	translator["op_GreaterThan"] = "operator&gt;";
	translator["op_LessThan"] = "operator&lt;";
	translator["op_RightShift"] = "operator&gt;&gt;";
	translator["op_LeftShift"] = "operator&lt;&lt;";
	translator["op_Equality"] = "operator=";
	translator["op_Inequality"] = "operator!=";
	translator["op_LogicalNot"] = "operator!";
	translator["op_OnesComplement"] = "operator~";
	translator["op_True"] = "operator true";
	translator["op_False"] = "operator false";
}
