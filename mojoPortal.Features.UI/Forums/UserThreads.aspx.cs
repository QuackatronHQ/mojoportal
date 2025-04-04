using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ForumUI;

public partial class ForumUserThreadsPage : mojoBasePage
{
	private int userId = -1;
	private int pageNumber = 1;
	private SiteUser forumUser = null;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!WebConfigSettings.AllowUserThreadBrowsing)
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		LoadSettings();
		AddCanonicalUrl();
		//this page has no content other than nav
		SiteUtils.AddNoIndexFollowMeta(Page);
		PopulateControls();

		AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsForumSection", "forums");
	}

	private void PopulateControls()
	{
		if (forumUser == null)
		{
			return;
		}

		heading.Text = string.Format(CultureInfo.InvariantCulture,
			ForumResources.ForumUserThreadHeading,
			Server.HtmlEncode(forumUser.Name));

		Title = SiteUtils.FormatPageTitle(siteSettings, string.Format(CultureInfo.InvariantCulture,
			ForumResources.UserThreadTitleFormat, Server.HtmlEncode(forumUser.Name)));

		MetaDescription = string.Format(CultureInfo.InvariantCulture,
			ForumResources.UserThreadMetaFormat, Server.HtmlEncode(forumUser.Name));
	}

	private void AddCanonicalUrl()
	{
		if (Page.Header == null)
		{
			return;
		}

		var userThreadUrl = "Forums/UserThreads.aspx".ToLinkBuilder().AddParam("userid", userId).PageNumber(pageNumber);
		var link = new Literal
		{
			ID = "threadurl",
			Text = $"\n<link rel=\"canonical\" href=\"{userThreadUrl}\" />"
		};

		Page.Header.Controls.Add(link);
	}

	private void LoadSettings()
	{
		userId = WebUtils.ParseInt32FromQueryString("userId", -1);
		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);

		forumUser = new SiteUser(siteSettings, userId);
		if (forumUser.UserId == -1)
		{
			forumUser = null;
		}

		threadList.SiteSettings = siteSettings;
		threadList.ForumUser = forumUser;
		threadList.PageNumber = pageNumber;
		threadList.SiteRoot = SiteRoot;
		threadList.ImageSiteRoot = ImageSiteRoot;

		threadListAlt.SiteSettings = siteSettings;
		threadListAlt.ForumUser = forumUser;
		threadListAlt.PageNumber = pageNumber;
		threadListAlt.SiteRoot = SiteRoot;
		threadListAlt.ImageSiteRoot = ImageSiteRoot;

		if (displaySettings.UseAltUserThreadList)
		{
			threadList.Visible = false;
			threadListAlt.Visible = true;
		}

		AddClassToBody("forumuserthreads");
	}


	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);

		SuppressMenuSelection();
		SuppressPageMenu();
	}

	#endregion
}
