using System;
using System.Data;
using System.Web.UI;
using mojoPortal.Business;
using Resources;

namespace mojoPortal.Web.ForumUI;

public partial class UserThreadListAlt : UserControl
{
	private int userId = -1;
	private int pageNumber = 1;
	private int pageSize = 20;
	private int totalPages = 1;
	protected double timeOffset = 0;
	private SiteUser forumUser = null;
	private SiteSettings siteSettings = null;

	public string SiteRoot { get; set; } = string.Empty;

	public string ImageSiteRoot { get; set; } = string.Empty;

	public SiteSettings SiteSettings
	{
		set { siteSettings = value; }
	}

	public SiteUser ForumUser
	{
		set { forumUser = value; }
	}

	public int PageNumber
	{
		set { pageNumber = value; }
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Visible) { return; }

		LoadSettings();
		PopulateLabels();
		PopulateControls();
	}

	private void PopulateControls()
	{
		if (forumUser == null) return;


		using IDataReader reader = ForumThread.GetPageByUser(
			userId,
			forumUser.SiteId,
			pageNumber,
			pageSize,
			out totalPages);

		string pageUrl = $"Forums/UserThreads.aspx".ToLinkBuilder().AddParam("userid", userId).PageNumber("{0}").ToString();

		pgrTop.PageURLFormat = pageUrl;
		pgrTop.ShowFirstLast = true;
		pgrTop.CurrentIndex = pageNumber;
		pgrTop.PageSize = pageSize;
		pgrTop.PageCount = totalPages;
		pgrTop.Visible = pgrTop.PageCount > 1;

		pgrBottom.PageURLFormat = pageUrl;
		pgrBottom.ShowFirstLast = true;
		pgrBottom.CurrentIndex = pageNumber;
		pgrBottom.PageSize = pageSize;
		pgrBottom.PageCount = totalPages;
		pgrBottom.Visible = pgrBottom.PageCount > 1;

		rptForums.DataSource = reader;
		rptForums.DataBind();
	}

	protected string FormatThreadUrl(int threadId, int moduleId, int itemId, int pageId)
	{
		if (ForumConfiguration.CombineUrlParams)
		{
			return "Forums/Thread.aspx".ToLinkBuilder().PageId(pageId).AddParam("t", ThreadParameterParser.FormatCombinedParam(threadId, pageNumber)).ToString();
		}

		return "Forums/Thread.aspx".ToLinkBuilder().PageId(pageId).ModuleId(moduleId).ItemId(itemId).AddParam("thread", threadId).ToString();
	}

	private void PopulateLabels()
	{
		pgrTop.NavigateToPageText = ForumResources.CutePagerNavigateToPageText;
		pgrTop.BackToFirstClause = ForumResources.CutePagerBackToFirstClause;
		pgrTop.GoToLastClause = ForumResources.CutePagerGoToLastClause;
		pgrTop.BackToPageClause = ForumResources.CutePagerBackToPageClause;
		pgrTop.NextToPageClause = ForumResources.CutePagerNextToPageClause;
		pgrTop.PageClause = ForumResources.CutePagerPageClause;
		pgrTop.OfClause = ForumResources.CutePagerOfClause;

		pgrBottom.NavigateToPageText = ForumResources.CutePagerNavigateToPageText;
		pgrBottom.BackToFirstClause = ForumResources.CutePagerBackToFirstClause;
		pgrBottom.GoToLastClause = ForumResources.CutePagerGoToLastClause;
		pgrBottom.BackToPageClause = ForumResources.CutePagerBackToPageClause;
		pgrBottom.NextToPageClause = ForumResources.CutePagerNextToPageClause;
		pgrBottom.PageClause = ForumResources.CutePagerPageClause;
		pgrBottom.OfClause = ForumResources.CutePagerOfClause;
	}

	private void LoadSettings()
	{
		timeOffset = SiteUtils.GetUserTimeOffset();
		if (forumUser != null)
		{
			userId = forumUser.UserId;
		}

	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}
}