<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Lure.ascx.vb" Inherits="ISL.OneWeb.ClientApplications.NSNU.VotingApplication.UI.App.Lure" %>

<%--
•	If there are no active votes for which the current user has not voted then the lure is hidden.
•	The lure text (ex. Tentative Agreement Reached) and the expirydate are dynamically generated from the active vote.
•	Time format is 24 hour clock--%>

<asp:PlaceHolder runat="server" ID="plcLure">
    <div class="announcement">
        <aside class="inner">
            <div class="mod_vote_lure_title">
            <asp:Literal runat="server" ID="litLure" />
            </div>

            <div class="mod_vote_lure_title">
            <asp:HyperLink runat="server" ID="lnkVote">VOTE NOW &gt;&gt;</asp:HyperLink> 

            <div class="mod_vote_lure_date">Voting closes @ <asp:Literal runat="server" ID="litExpiryDate" /></div>
            </div>
        </aside>
    </div>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="plcAdminLoggedOn">
    <p>No extranet memebr is logged on. This control needs to be placed on a member-protected page. This message is visible to a logged on OneWeb user.</p>
</asp:PlaceHolder>


