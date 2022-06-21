#include <fstream>
#include <map>

#include "Writer.h"
#include "MW.h"

#if BUILD
#include "Timer.h"
#endif

#define HTML_HEADER "<!DOCTYPE html><html lang="\
			"en"\
			"><head>"\
			"<title>Home | MICHAEL WU</title><link rel=""stylesheet"" href="\
			"MWUnityNamespace.css"\
			"><meta charset="\
			"UTF-8"\
			"></head><body><div class=""header"" id=""top"">"\
			"MW UNITY NAMESPACE</div>"\

#define HTML_END "</div></div></div></body></html>"

/*
* CSS styles to apply to html.
*/

/*
* The div marking the beginning of the two column table.
* The left column is the nav links with the complete MW
  namespace and links to their XML tags.
* The right column describes the fields/properties/functions
  constructors and summaries of the respective namespace
  function.
*/
constexpr const char* CSS_HOLDING_DIV = "\"width: 100%; display: table;\"";
/*
* The div marking the first and only row in the file.
*/
constexpr const char* CSS_INNER_DIV = "\"display: table-row\"";
/*
* The div defining the styles of the left column.
*/
constexpr const char* CSS_LEFT_COL_DIV = "\"width: 200px; display: table-cell;\"";
/*
* The div defining the styles of the right column.
*/
constexpr const char* CSS_RIGHT_COL_DIV = "\"display: table-cell;\"";
/*
* The CSS selector used for every link in the navbar.
*/
constexpr const char* CSS_NAV_LINKS = "\"navLinks\"";
/*
* The CSS selector used to define the style for namespace classes or
  functions, depending on whether the namespace class is a part of
  the root MW namespace.
*/
constexpr const char* CSS_HEADER_STYLE = "\"basicHead\"";
/*
* The CSS selector/s used to define the style for namespace classes.
*/
constexpr const char* CSS_CLASS_STYLE = "\"classHead C\"";
/*
* The CSS selector/s used to style the namespace class summary.
*/
constexpr const char* CSS_CLASS_SUMMARY_STYLE = "\"simplePara C\"";
/*
* The CSS selector used to style function summaries and parameter
  definitions.
*/
constexpr const char* CSS_PARAGRAPH = "\"simplePara\"";
/*
* The CSS selector used to style keywords for function summaries.
*/
constexpr const char* CSS_KEYWORD = "\"keyword\"";

#define INTER_INJECT_TEXT(text) << text
#define INTER_INJECT_CLASS_DECORATIONS(decorations) "<pre class=\"C\" style=\"padding-right:25%;color:rgb(40,255,120);\">" INTER_INJECT_TEXT(decorations) << "</pre>"
#define INTER_INJECT_FUNCTION_DECORATIONS(decorations) "<br><pre style=\"padding-right:25%;color:rgb(78,201,176);font-weight:549\">" INTER_INJECT_TEXT(decorations) << "</pre>"

#define WRITE_DEBUG_LINES 0

#if WRITE_DEBUG_LINES
#define DEBUG_WRITELINE << "<p style=\"color:white\">" INTER_INJECT_TEXT(__LINE__) << "</p>"
#else
#define DEBUG_WRITELINE
#endif

#define HTML_HOLDING_DIV "<div style=" << CSS_HOLDING_DIV << "><div style=" << CSS_INNER_DIV << "><div style=" << CSS_LEFT_COL_DIV << ">" DEBUG_WRITELINE
#define HTML_NAV_ENTRY(entry) "<div class=" << CSS_NAV_LINKS << "><a href=" << entry << ".html>" << entry << "</a></div><br>" DEBUG_WRITELINE
#define HTML_SUMMARY_START "</div><br><br><div style=" << CSS_RIGHT_COL_DIV << ">" DEBUG_WRITELINE
#define HTML_CLASS_START(entry, summary, decorations) INTER_INJECT_CLASS_DECORATIONS(decorations) << "</pre><h1 class=" << CSS_CLASS_STYLE << ">" INTER_INJECT_TEXT(entry) << "</h1><br><p class=" << CSS_CLASS_SUMMARY_STYLE << ">" INTER_INJECT_TEXT(summary) << "</p>" DEBUG_WRITELINE
#define HTML_SUMMARY_TITLE(title, decorations) INTER_INJECT_FUNCTION_DECORATIONS(decorations) << "<p class=" << CSS_HEADER_STYLE << ">" INTER_INJECT_TEXT(title) << "</p>" DEBUG_WRITELINE
#define HTML_DECLARE_FUNCTION_PARAMS(title, params, decorations) INTER_INJECT_FUNCTION_DECORATIONS(decorations) << "<h1 class=" << CSS_HEADER_STYLE << ">" INTER_INJECT_TEXT(title) << " (" INTER_INJECT_TEXT(params) << ")</h1>" DEBUG_WRITELINE
#define HTML_DECLARE_OPERATOR_OVERLOAD(overload, params, decorations) INTER_INJECT_FUNCTION_DECORATIONS(decorations) << "<br><h1 class" << CSS_HEADER_STYLE << ">" INTER_INJECT_TEXT(overload) INTER_INJECT_TEXT(params) << "</h1>" DEBUG_WRITELINE
#define HTML_SUMMARY_ENTRY(entry) "<p class=" << CSS_PARAGRAPH << ">" INTER_INJECT_TEXT(entry) << "</p>" DEBUG_WRITELINE
#define HTML_KEYWORD(keyword) "<p class=" << CSS_KEYWORD << ">" INTER_INJECT_TEXT(keyword) << "</p>" DEBUG_WRITELINE

void Writer::Write(const std::vector<MW>& all_mw)
{
	std::map<std::string, std::string> namespace_to_html;

#if _DEBUG && WITH_VS
	const std::string HTML_PATH = "../HTML/";
#else
	const std::string HTML_PATH = "../../HTML/";
#endif

	// Write/Create basic HTML file.
	for (auto& n : all_mw)
	{
		if (namespace_to_html.find(n.mw_namespace) == namespace_to_html.end())
		{
			std::string new_file = HTML_PATH + n.mw_namespace + ".html";
			namespace_to_html.insert({ n.mw_namespace, new_file });

			std::ofstream html_file(new_file);

			html_file << HTML_HEADER << HTML_HOLDING_DIV;

#if BUILD || _DEBUG
			if (html_file.fail())
			{
				std::cout << "Failed to create HTML file at " << HTML_PATH << ". Maybe permissions?\n";
				std::cout << "Writing to HTML file/s has been stopped!\n";
				return;
			}
			else
			{
				std::cout << n.mw_namespace << ".html" << " created.\n";
			}
#endif

			html_file.close();
		}
	}

	// Write all namespace links.
	for (auto& ns : namespace_to_html)
	{
		for (auto& nth : namespace_to_html)
		{
			std::ofstream html(nth.second, std::ios_base::app);

			html << HTML_NAV_ENTRY(ns.first);

			html.close();
		}
	}

	// Prepare the right column.
	for (auto& namespace_file : namespace_to_html)
	{
		std::ofstream html(namespace_file.second, std::ios_base::app);

		html << HTML_SUMMARY_START;

		html.close();
	}

	for (auto& mw : all_mw)
	{
		std::ofstream html(namespace_to_html[mw.mw_namespace], std::ios_base::app);
		if (mw.mw_name.length() == 0)
		{
			html << HTML_CLASS_START(
				(mw.mw_class.length() != 0
					? mw.mw_class
					: mw.mw_namespace)
				, mw.summary, GetDecorations(mw.decorations));
		}
		else
		{
			if (mw.function_parameters_type.size() == 0)
			{
				if (mw.mw_type == "MEMBER")
				{
					if (mw.implicit.length() == 0)
					{
						// A function.
						// Because this function_parameters_type.size == 0, this has no parameters.
						// Write the name of the function with empty brackets.
						html << HTML_DECLARE_FUNCTION_PARAMS(mw.mw_name, "", GetDecorations(mw.decorations));
					}
					else
					{
						// An implicit operator.
						html << HTML_DECLARE_FUNCTION_PARAMS(mw.implicit, "", GetDecorations(mw.decorations));
					}

					// If there is a summary, write it here.
					if (mw.summary.length() != 0)
						html << HTML_KEYWORD("Summary:") << HTML_SUMMARY_ENTRY(mw.summary);

					// If there are remarks, write it here.
					if (mw.remarks.length() != 0)
						html << HTML_KEYWORD("Remarks:") << HTML_SUMMARY_ENTRY(mw.remarks);

					// If there is a return value, write it here.
					if (mw.returns.length() != 0)
						html << HTML_KEYWORD("Returns:") << HTML_SUMMARY_ENTRY(mw.returns);
				}
				else
				{
					// Not a function.
					// Write whatever this is normally.
					if ((mw.mw_class.length() == 0) ^ mw.mw_type == "FIELD" ^ mw.mw_type == "PROPERTY")
					{
						html << HTML_SUMMARY_TITLE(mw.mw_name, GetDecorations(mw.decorations)) << HTML_SUMMARY_ENTRY(mw.summary);

						if (mw.remarks.length() != 0)
							html << HTML_SUMMARY_ENTRY(mw.remarks);
					}
					else
					{
						html << HTML_CLASS_START(mw.mw_name, mw.summary + "<br>" + mw.remarks, GetDecorations(mw.decorations));
					}
				}
			}
			else
			{
				std::string params;

				auto size_of_name = mw.function_parameters_name.size();

				// Writing function parameter types.
				std::string generics = "TYUMNKR";
				for (int i = 0, generic_count = 0; i < size_of_name; ++i)
				{
					// If the type is just a standalone 'T', then we know it's a generic.
					// Replace the genric 'T' with the std::string generics using generic_count.
					if (mw.function_parameters_type[i].length() == 1 && mw.function_parameters_type[i][0] == 'T')
					{
						params += generics[generic_count++];
					}
					else
					{
						// For some reason, there may be a generic parameter marked by two T's
						// (TT), where in reality, they reference only T.
						// If this is the case, only add one T, the first T, to the params.
						if (mw.function_parameters_type[i] == "TT")
						{
							params += mw.function_parameters_type[i][0];
						}
						else
						{
							// Otherwise, add the type as normal.
							params += mw.function_parameters_type[i];
						}
					}

					params += " " + mw.function_parameters_name[i];

					if (i != size_of_name - 1)
						params += ", ";
				}

				// Write the name of the function.
				html << HTML_DECLARE_FUNCTION_PARAMS(mw.mw_name, params, GetDecorations(mw.decorations));

				// If there is a summary, write it here.
				if (mw.summary.length() != 0)
					html << HTML_KEYWORD("Summary:") << HTML_SUMMARY_ENTRY(mw.summary);

				// If there are remarks, write it here.
				if (mw.remarks.length() != 0)
					html << HTML_KEYWORD("Remarks:") << HTML_SUMMARY_ENTRY(mw.remarks);

				// Write the summaries for the parameters (if any).
				for (int i = 0; i < size_of_name; ++i)
				{
					if (mw.function_parameters_desc[i].length() != 0)
					{
						if (i == 0)
							html << HTML_KEYWORD("Params:");

						html << HTML_SUMMARY_ENTRY(mw.function_parameters_name[i] + ": " + mw.function_parameters_desc[i]);
					}
				}

				// If there is a return value, write it here.
				if (mw.returns.length() != 0)
					html << HTML_KEYWORD("Returns:") << HTML_SUMMARY_ENTRY(mw.returns);
			}
		}

		html.close();
	}

	// End basic HTML file.
	for (auto& nth : namespace_to_html)
	{
		std::ofstream html(nth.second, std::ios_base::app);

		html << HTML_END;

		html.close();
	}
}


std::string Writer::GetDecorations(const VT(std::string)& decorations)
{
	if (decorations.empty())
		return std::string();

	std::string decor = "<br>";

	for (size_t i = 0; i < decorations.size(); ++i)
	{
		decor += decorations[i] + " ";
	}

	return decor;
}
