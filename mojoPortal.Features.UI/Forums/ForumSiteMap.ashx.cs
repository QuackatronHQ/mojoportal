﻿using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.ForumUI;

/// <summary>
/// https://www.google.com/webmasters/tools/docs/en/protocol.html
/// Site Map of forum threads formatted in google site map protocol
/// You can submit your site maps to google or bing and other search indexes that support the protocol
/// The Site Map url to submit is yoursiteroot/Forums/ForumSiteMap.ashx
/// </summary>
public class ForumSiteMap : IHttpHandler
{

	public void ProcessRequest(HttpContext context)
	{
		GenerateSiteMap(context);
	}

	private void GenerateSiteMap(HttpContext context)
	{
		context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(ForumConfiguration.SiteMapCacheMinutes));
		context.Response.Cache.SetCacheability(HttpCacheability.Public);

		context.Response.ContentType = "application/xml";
		Encoding encoding = new UTF8Encoding();
		context.Response.ContentEncoding = encoding;

		using (XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding))
		{
			xmlTextWriter.Formatting = Formatting.Indented;

			xmlTextWriter.WriteStartDocument();

			xmlTextWriter.WriteStartElement("urlset");
			xmlTextWriter.WriteStartAttribute("xmlns");
			xmlTextWriter.WriteValue("http://www.sitemaps.org/schemas/sitemap/0.9");
			xmlTextWriter.WriteEndAttribute();

			// add forum thread urls
			if (ForumConfiguration.EnableSiteMap && ForumConfiguration.CombineUrlParams)
			{
				AddForumThreadUrls(context, xmlTextWriter);
			}


			xmlTextWriter.WriteEndElement(); //urlset

			//end of document
			xmlTextWriter.WriteEndDocument();

		}



	}

	private void AddForumThreadUrls(HttpContext context, XmlTextWriter xmlTextWriter)
	{


		SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (siteSettings == null) { return; }

		if (siteSettings.SiteGuid == Guid.Empty) { return; }

		string baseUrl = SiteUtils.GetNavigationSiteRoot()
			+ "/Forums/Thread.aspx?pageid=";

		if ((siteSettings.UseSslOnAllPages) && (SiteUtils.SslIsAvailable()))
		{
			baseUrl = baseUrl.Replace("http:", "https:");
		}
		else
		{
			baseUrl = baseUrl.Replace("https:", "http:");
		}


		using (IDataReader reader = ForumThread.GetThreadsForSiteMap(siteSettings.SiteId))
		{
			while (reader.Read())
			{
				string pageViewRoles = reader["AuthorizedRoles"].ToString();
				string moduleViewRoles = reader["ViewRoles"].ToString();
				bool include = (
					(pageViewRoles.Contains("All Users"))
					&& ((moduleViewRoles.Length == 0) || (moduleViewRoles.Contains("All Users")))
					);

				if (!include) { continue; }

				xmlTextWriter.WriteStartElement("url");
				xmlTextWriter.WriteElementString(
					"loc",
					baseUrl // /Forums/Thread.aspx?pageid=
					+ reader["PageID"].ToString()
					+ "&t=" + reader["ThreadID"].ToString()
					+ "~-1" // the full thread view without paging
					);
				xmlTextWriter.WriteElementString(
						"lastmod",
						Convert.ToDateTime(reader["MostRecentPostDate"]).ToString("u", CultureInfo.InvariantCulture).Replace(" ", "T"));


				//xmlTextWriter.WriteElementString("changefreq", "monthly");

				xmlTextWriter.WriteElementString("priority", "0.5");

				xmlTextWriter.WriteEndElement(); //url
			}


		}


	}

	public bool IsReusable
	{
		get
		{
			return false;
		}
	}
}