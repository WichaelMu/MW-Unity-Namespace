#include <fstream>
#include <map>

#include "Writer.h"

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

#define HTML_HOLDING_DIV "<div style=" << CSS_HOLDING_DIV << "><div style=" << CSS_INNER_DIV << "><div style=" << CSS_LEFT_COL_DIV << ">"
#define HTML_NAV_ENTRY(entry) "<div class=" << CSS_NAV_LINKS << "><a href=" << entry << ".html>" << entry << "</a></div><br>"
#define HTML_SUMMARY_START "</div><br><br><div style=" << CSS_RIGHT_COL_DIV << ">"
#define HTML_CLASS_START(entry, summary) "<br><br><h1 class=" << CSS_CLASS_STYLE << ">" << entry << "</h1><br><p class=" << CSS_CLASS_SUMMARY_STYLE << ">" << summary << "</p>"
#define HTML_SUMMARY_TITLE(title) "<p class=" << CSS_HEADER_STYLE << ">" << title << "</p><br>"
#define HTML_DECLARE_FUNCTION_PARAMS(title, params) "<h1 class=" << CSS_HEADER_STYLE << ">" << title << " (" << params << ")</h1><br>"
#define HTML_SUMMARY_ENTRY(entry) "<p class=" << CSS_PARAGRAPH << ">" << entry << "</p>"

void Writer::Write(const std::vector<MW>& all_mw)
{
#if BUILD
	PerformanceTimer t;
	t.StartTime();
#endif
	std::map<std::string, std::string> namespace_to_html;

#if _DEBUG
	const std::string HTML_PATH = "../../HTML/";
#else
	const std::string HTML_PATH = "C:/Users/table/Documents/Machine Code/MW/MW/Docs/HTML/";
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

#if BUILD
			if (html_file.fail())
			{
				std::cout << "Failed to create HTML file at " << HTML_PATH << ". Maybe permissions?\n";
				std::cout << "Writing to HTML file/s has been stopped!\n";
				return;
			}
			else
			{
				std::cout << new_file << " created.\n";
			}
#endif

			html_file.close();
		}
	}

#if BUILD
	t.PrintTime("Writing/Creating basic HTML file");
	t.StartTime();
#endif

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

#if BUILD
	t.PrintTime("Writing all namespace links");
	t.StartTime();
#endif

	// Prepare the right column.
	for (auto& namespace_file : namespace_to_html)
	{
		std::ofstream html(namespace_file.second, std::ios_base::app);

		html << HTML_SUMMARY_START;

		html.close();
	}

#if BUILD
	t.PrintTime("Preparing the right column");
	t.StartTime();
#endif

	for (auto& mw : all_mw)
	{
		std::ofstream html(namespace_to_html[mw.mw_namespace], std::ios_base::app);
		if (mw.mw_name.length() == 0)
		{
			html << HTML_CLASS_START(
				(mw.mw_class.length() != 0
					? mw.mw_class
					: mw.mw_namespace)
				, mw.summary);
		}
		else
		{
			if (mw.function_parameters_type.size() == 0)
			{
				if (mw.mw_type == "MEMBER")
				{
					// A function.
					// Becaues this function_parameters_type.size == 0, this has no parameters.
					// Write the name of the function with empty brackets.
					html << HTML_DECLARE_FUNCTION_PARAMS(mw.mw_name, "") << HTML_SUMMARY_ENTRY(mw.summary);
				}
				else
				{
					// Not a function.
					// Write whatever this is normally.
					html << HTML_SUMMARY_TITLE(mw.mw_name) << HTML_SUMMARY_ENTRY(mw.summary);
				}
			}
			else
			{
				std::string params;

				auto size_of_name = mw.function_parameters_name.size();

				std::string generics = "TYUMNKR";
				for (int i = 0, generic_count = 0; i < size_of_name; ++i)
				{
					// If the type is just a standalone 'T', then we know it's a generic.
					// Replace the genric 'T' with the std::string generics using generic_count.
					if (mw.function_parameters_type[i].length() == 1 && mw.function_parameters_type[i][0] == 'T')
						params += generics[generic_count++];
					else
						params += mw.function_parameters_type[i];
						
					params += " " + mw.function_parameters_name[i];

					if (i != size_of_name - 1)
						params += ", ";
				}

				html << HTML_DECLARE_FUNCTION_PARAMS(mw.mw_name, params) << HTML_SUMMARY_ENTRY(mw.summary);

				for (int i = 0; i < size_of_name; ++i)
				{
					if (mw.function_parameters_desc[i].length() != 0)
						html << HTML_SUMMARY_ENTRY(mw.function_parameters_name[i] + ": " + mw.function_parameters_desc[i]);
				}
			}
		}

		html.close();
	}

#if BUILD
	t.PrintTime("Writing summaries");
	t.StartTime();
#endif

	// End basic HTML file.
	for (auto& nth : namespace_to_html)
	{
		std::ofstream html(nth.second, std::ios_base::app);

		html << HTML_END;

		html.close();
	}


#if BUILD
	t.PrintTime("Ending HTML file");
	t.StartTime();
#endif
}
