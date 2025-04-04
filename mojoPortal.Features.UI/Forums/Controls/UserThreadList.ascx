<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="UserThreadList.ascx.cs" Inherits="mojoPortal.Web.ForumUI.UserThreadList" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>

<forum:ForumDisplaySettings ID="displaySettings" runat="server" />

<div class="modulepager">
	<portal:mojoCutePager ID="pgrTop" runat="server" />
</div>
<table summary='<%# Resources.ForumResources.ForumViewTableSummary %>' class='<%= displaySettings.ThreadListCssClass %>' <% if (displaySettings.UseOldTableAttributes)
	{%> cellpadding="3" cellspacing="1" border="0" width="100%" <% } %>>
	<thead>
		<tr class="moduletitle">
			<th id='<%# Resources.ForumResources.ForumViewSubjectLabel %>' class="ftitle">
				<mp:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="ForumViewSubjectLabel" ResourceFile="ForumResources" UseLabelTag="false" />
			</th>
			<th id='<%# Resources.ForumResources.ForumLabel %>' class="fforumtitle">
				<mp:SiteLabel ID="ForumLabel1" runat="server" ConfigKey="ForumLabel" ResourceFile="ForumResources" UseLabelTag="false" />
			</th>
			<th id='<%# Resources.ForumResources.ForumViewStartedByLabel %>' class="fstartedby">
				<mp:SiteLabel ID="lblForumStartedBy" runat="server" ConfigKey="ForumViewStartedByLabel" ResourceFile="ForumResources" UseLabelTag="false" />
			</th>
			<th id='<%# Resources.ForumResources.ForumViewViewCountLabel %>' class="fpostviews">
				<mp:SiteLabel ID="lblTotalViewsCountLabel" runat="server" ConfigKey="ForumViewViewCountLabel" ResourceFile="ForumResources" UseLabelTag="false" />
			</th>
			<th id='<%# Resources.ForumResources.ForumViewReplyCountLabel %>' class="fpostreplies">
				<mp:SiteLabel ID="lblTotalRepliesCountLabel" runat="server" ConfigKey="ForumViewReplyCountLabel" ResourceFile="ForumResources" UseLabelTag="false" />
			</th>
			<th id='<%# Resources.ForumResources.ForumViewPostLastPostLabel %>' class="fpostdate">
				<mp:SiteLabel ID="lblLastPostLabel" runat="server" ConfigKey="ForumViewPostLastPostLabel" ResourceFile="ForumResources" UseLabelTag="false" />
			</th>
		</tr>
	</thead>
	<asp:Repeater ID="rptForums" runat="server">
		<HeaderTemplate>
			<tbody>
		</HeaderTemplate>
		<ItemTemplate>
			<tr class="modulerow">
				<td headers='<%# Resources.ForumResources.ForumViewSubjectLabel %>' class="ftitle">
					<img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/folder.png"  %>' />
					<a href='<%# FormatThreadUrl(Convert.ToInt32(Eval("ThreadID")),Convert.ToInt32(Eval("ModuleID")),Convert.ToInt32(Eval("ForumID")),Convert.ToInt32(Eval("PageID"))) %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ThreadSubject").ToString())%></a>
				</td>
				<td headers='<%# Resources.ForumResources.ForumLabel %>' class="fforumtitle">
					<a href='<%# FormatForumUrl(Convert.ToInt32(Eval("ForumID")),Convert.ToInt32(Eval("ModuleID")), Convert.ToInt32(Eval("PageID"))) %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Forum").ToString())%></a>
				</td>
				<td headers='<%# Resources.ForumResources.ForumViewStartedByLabel %>' class="fstartedby">
					<%# DataBinder.Eval(Container.DataItem, "StartedBy")%>
				</td>
				<td headers='<%# Resources.ForumResources.ForumViewViewCountLabel %>' class="fpostviews">
					<%# DataBinder.Eval(Container.DataItem, "TotalViews")%>
				</td>
				<td headers='<%# Resources.ForumResources.ForumViewReplyCountLabel %>' class="fpostreplies">
					<%# DataBinder.Eval(Container.DataItem, "TotalReplies")%>
				</td>
				<td headers='<%# Resources.ForumResources.ForumViewPostLastPostLabel %>' class="fpostdate">
					<%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem), "MostRecentPostDate", timeOffset)%>
					<br />
					<%# DataBinder.Eval(Container.DataItem, "MostRecentPostUser")%>
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="modulealtrow">
				<td headers='<%# Resources.ForumResources.ForumViewSubjectLabel %>' class="ftitle">
					<img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/folder.png"  %>' />
					<a href='<%# FormatThreadUrl(Convert.ToInt32(Eval("ThreadID")),Convert.ToInt32(Eval("ModuleID")),Convert.ToInt32(Eval("ForumID")),Convert.ToInt32(Eval("PageID"))) %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ThreadSubject").ToString())%></a>
				</td>
				<td headers='<%# Resources.ForumResources.ForumLabel %>' class="fforumtitle">
					<a href='<%# FormatForumUrl(Convert.ToInt32(Eval("ForumID")),Convert.ToInt32(Eval("ModuleID")), Convert.ToInt32(Eval("PageID"))) %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Forum").ToString())%></a>
				</td>
				<td headers='<%# Resources.ForumResources.ForumViewStartedByLabel %>' class="fstartedby">
					<%# DataBinder.Eval(Container.DataItem, "StartedBy")%>
				</td>
				<td headers='<%# Resources.ForumResources.ForumViewViewCountLabel %>' class="fpostviews">
					<%# DataBinder.Eval(Container.DataItem, "TotalViews")%>
				</td>
				<td headers='<%# Resources.ForumResources.ForumViewReplyCountLabel %>' class="fpostreplies">
					<%# DataBinder.Eval(Container.DataItem, "TotalReplies")%>
				</td>
				<td headers='<%# Resources.ForumResources.ForumViewPostLastPostLabel %>' class="fpostdate">
					<%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem), "MostRecentPostDate", timeOffset)%>
					<br />
					<%# DataBinder.Eval(Container.DataItem, "MostRecentPostUser")%>
				</td>
			</tr>
		</AlternatingItemTemplate>
		<FooterTemplate></tbody></FooterTemplate>
	</asp:Repeater>
</table>
<div class="modulepager">
	<portal:mojoCutePager ID="pgrBottom" runat="server" />
</div>
