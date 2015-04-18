<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Vote.ascx.vb" Inherits="ISL.OneWeb.ClientApplications.NSNU.VotingApplication.UI.App.Vote" %>

<%--If the current user has previously responded to the active vote, then the yes/no radio inputs and submit button are hidden 
and replaced with a message ‘You responded to this question Jan 13, 2013 at 17:00.--%>



<div class="mod_voting">
    <asp:PlaceHolder runat="server" ID="plcQuestion">
    <p class="mod_voting_question">
    <asp:Literal runat="server" ID="litQuestion"/>
    </p>
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="plcVote">
    <div class="mod_voting_answers">
        <p class="mod_voting_input">
        <input type="radio" id="rbYes" name="rbVote" runat="server" checked /> <asp:Literal runat="server" ID="litYes" />
        <br />
        <input type="radio" id="rbNo" name="rbVote" runat="server" /> <asp:Literal runat="server" ID="litNo" />
        </p>
    </div>
    <p class="mod_voting_expires">
    <asp:Literal runat="server" ID="litExpires" Text="&lt;strong&gt;Expires:&lt;/strong&gt; {0}" />
    </p>
    <p class="mod_voting_submit">
    <asp:Button runat="server" CSSClass="btn_red" ID="btnOK" Text="Submit" CausesValidation="false" />
    </p>

    </asp:PlaceHolder>


    <asp:PlaceHolder runat="server" ID="plcAlreadyVoted">
        <p class="mod_voting_responded"><asp:Literal runat="server" ID="litAlreadyVoted" Text="You responded to this question {0}." /></p>
    </asp:PlaceHolder>


    <asp:PlaceHolder runat="server" ID="plcNoActiveVotes">
    There are currently no active votes.
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="plcVoteFailed">
    <p class="mod_voting_failed">Your vote could not be recorded.</p>
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="plcException">
    <p class="mod_voting_failed">An exception occurred trying to add a vote:

    <asp:Literal runat="server" ID="litException" /></p>
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="plcAdminLoggedOn">
    <p class="mod_voting_failed">No extranet memebr is logged on. This control needs to be placed on a member-protected page. This message is visible to a logged on OneWeb user.</p>
    </asp:PlaceHolder>


</div>