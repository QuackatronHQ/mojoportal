using System;
using System.Data;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.ForumUI;

public partial class SubscriberDialog : mojoDialogBasePage
{
	private int pageId = -1;
	private int moduleId = -1;
	private int itemId = -1;
	private int totalPages = 1;
	private int pageNumber = 1;
	private int pageSize = 12;
	protected bool isAdmin = false;
	//private SiteSettings siteSettings = null;

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();

		if (!UserCanEditModule(moduleId, Forum.FeatureGuid))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		if (!IsPostBack) { BindGrid(); }

	}

	private void BindGrid()
	{
		using (IDataReader reader = Forum.GetSubscriberPage(itemId, pageNumber, pageSize, out totalPages))
		{
			string pageUrl = SiteRoot
			+ "/Forums/SubscriberDialog.aspx?ItemID="
			+ itemId.ToInvariantString()
			+ "&amp;mid="
			+ moduleId.ToInvariantString()
			+ "&amp;pageid=" + pageId.ToInvariantString()
			+ "&amp;pagenumber={0}";

			pgr.PageURLFormat = pageUrl;
			pgr.ShowFirstLast = true;
			pgr.CurrentIndex = pageNumber;
			pgr.PageSize = pageSize;
			pgr.PageCount = totalPages;
			pgr.Visible = (totalPages > 1);

			rptUsers.DataSource = reader;
			rptUsers.DataBind();

		}

	}

	void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		if (e.CommandName == "unsubscribe")
		{
			int subscriptionId = Convert.ToInt32(e.CommandArgument);
			Forum.DeleteSubscription(subscriptionId);

		}

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}

	private void LoadSettings()
	{
		//siteSettings = CacheHelper.GetCurrentSiteSettings();
		pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
		moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
		itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);
		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
		isAdmin = WebUser.IsAdmin;


	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(Page_Load);
		rptUsers.ItemCommand += new RepeaterCommandEventHandler(rptUsers_ItemCommand);

	}


}
