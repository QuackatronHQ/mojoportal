using System;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Net;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ForumUI;

public partial class UnsubscribeForum : mojoBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(UnsubscribeForum));

	private Guid subGuid = Guid.Empty;

	#region OnInit
	override protected void OnInit(EventArgs e)
	{
		this.Load += new System.EventHandler(this.Page_Load);
		base.OnInit(e);
	}
	#endregion

	private void Page_Load(object sender, System.EventArgs e)
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, ForumResources.UnSubscribeLink);

		AddClassToBody("forumunsubscribe");

		subGuid = WebUtils.ParseGuidFromQueryString("fs", subGuid);

		if (subGuid != Guid.Empty)
		{
			Forum.Unsubscribe(subGuid);
			lblUnsubscribe.Text = ForumResources.ForumUnsubscribeCompleted;
			return;
		}

		int forumID = WebUtils.ParseInt32FromQueryString("itemid", -1);

		if (forumID > -1)
		{
			UnsubscribeUser(forumID);
			return;
		}


		if (WebUser.IsAdmin && (Request.Params.Get("ue") != null))
		{
			UnsubscribeUserFromAll(Request.Params.Get("ue"));
		}
	}

	private void UnsubscribeUser(int forumId)
	{
		if (SiteUtils.GetCurrentSiteUser() is not SiteUser siteUser)
		{
			return;
		}

		var forum = new Forum(forumId);
		if (!forum.Unsubscribe(siteUser.UserId))
		{
			log.ErrorFormat("Forum.UnSubscribe({0}, {1}, ) failed", forumId, siteUser.UserId);
			lblUnsubscribe.Text = ForumResources.ForumUnsubscribeFailed;
			return;
		}

		lblUnsubscribe.Text = ForumResources.ForumUnsubscribeCompleted;
	}

	private void UnsubscribeUserFromAll(string userEmail)
	{
		if (string.IsNullOrEmpty(userEmail))
		{
			return;
		}

		if (!Email.IsValidEmailAddressSyntax(userEmail))
		{
			return;
		}

		if (SiteUser.GetByEmail(siteSettings, userEmail) is not SiteUser user)
		{
			return;
		}

		if (user.UserGuid == Guid.Empty)
		{
			return;
		}

		ForumThread.UnsubscribeAll(user.UserId);
		Forum.UnsubscribeAll(user.UserId);

		lblUnsubscribe.Text = ForumResources.AdminUnsubscribeUserComplete;
	}
}
