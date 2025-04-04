using System;
using log4net;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ForumUI;

public partial class UnsubscribeForumThread : mojoBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(UnsubscribeForumThread));

	#region OnInit
	override protected void OnInit(EventArgs e)
	{
		this.Load += new System.EventHandler(this.Page_Load);
		base.OnInit(e);
	}
	#endregion


	private Guid threadSubGuid = Guid.Empty;

	private void Page_Load(object sender, System.EventArgs e)
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, ForumResources.UnSubscribeLink);

		AddClassToBody("forumthreadunsubscribe");

		threadSubGuid = WebUtils.ParseGuidFromQueryString("ts", threadSubGuid);

		if (threadSubGuid != Guid.Empty)
		{
			ForumThread.Unsubscribe(threadSubGuid);
			lblUnsubscribe.Text = ForumResources.ForumThreadUnsubscribeCompleted;
			return;
		}

		var threadID = WebUtils.ParseInt32FromQueryString("threadid", -1);

		if (threadID > -1)
		{
			UnsubscribeUser(threadID);
		}
	}

	private void UnsubscribeUser(int threadId)
	{
		if (SiteUtils.GetCurrentSiteUser() is not SiteUser siteUser)
		{
			return;
		}

		if (!ForumThread.Unsubscribe(threadId, siteUser.UserId))
		{
			log.ErrorFormat("ForumThread.UnSubscribe({0}, {1}) failed", threadId, siteUser.UserId);
			lblUnsubscribe.Text = ForumResources.ForumThreadUnsubscribeFailed;
			return;
		}

		lblUnsubscribe.Text = ForumResources.ForumThreadUnsubscribeCompleted;
	}
}
