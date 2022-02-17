#include <iostream>
#include <fstream>
#include <map>

#include "Writer.h"

#define HTML_HEADER "<!DOCTYPE html><html lang="\
"en"\
"><head>"\
"<title>Home | MICHAEL WU</title><link rel=""stylesheet"" href="\
"MWUnityNamespace.css"\
"><meta charset="\
"UTF-8"\
"></head><body><div class=""header"" id=""top"">"\
"MW UNITY NAMESPACE</div>"\

#define HTML_FOOTER "</body></html>"

#define HTML_OPEN_NAVBAR "<div class=""ListLeft""><nav>"
#define HTML_NAV_FIRST "<div class=""navLinks""><a href="""
#define HTML_NAV_MID ".html>"
#define HTML_NAV_LAST "</div></a></div>"
#define HTML_CLOSE_NAVBAR "</nav>"

#define HTML_NAV_ENTRY(entry) HTML_OPEN_NAVBAR << HTML_NAV_FIRST << entry << HTML_NAV_MID << entry << HTML_NAV_LAST << HTML_CLOSE_NAVBAR

#define HTML_WRITE_TITLE(title, summary) "<div><h1 class=""basicHead"">" << title << "</h1><p class=""simplePara C"">" << summary << "</p>"

void Writer::Write(const std::vector<MW>& all_mw)
{
	std::map<std::string, std::string> namespace_to_html;

	const std::string HTML_PATH = "../../HTML/";

	for (auto& n : all_mw)
	{
		if (namespace_to_html.find(n.mw_namespace) == namespace_to_html.end())
		{
			std::string new_file = HTML_PATH + n.mw_namespace + ".html";
			namespace_to_html.insert({ n.mw_namespace, new_file });

			std::ofstream html_file(new_file);

			html_file << HTML_HEADER;

			html_file.close();
		}
	}
	
	for (auto& ns : namespace_to_html)
	{
		for (auto& nth : namespace_to_html)
		{
			std::ofstream html(nth.second, std::ios_base::app);

			html << HTML_NAV_ENTRY(ns.first);

			html.close();
		}
	}

	for (auto& mw : all_mw)
	{
		std::ofstream html(namespace_to_html[mw.mw_namespace], std::ios_base::app);

		html << HTML_WRITE_TITLE(mw.mw_name, mw.summary);

		html.close();
	}

	// Write HTML footer
	for (auto& nth : namespace_to_html)
	{
		std::ofstream html(nth.second, std::ios_base::app);

		html << HTML_FOOTER;
	}
}
