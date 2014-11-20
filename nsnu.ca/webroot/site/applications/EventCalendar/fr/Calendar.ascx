<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.Modules.EventCalendar.UI.App.Calendar" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%-- EventCalendar styles --%>
<OWWeb:Stylesheet Runat="server" Media="screen" Mode="Link" Url="~/site/styles/mod_ec.css ~/core/styles/mod_ec.css" />
<%-- The following server-side script is in the template file because it affects the display of the calendar control, and to allow easy changes through the template file --%>
<script runat="server" language="vb">

    Sub DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs)

        If e.Day.IsOtherMonth Then
            e.Cell.Text = "&nbsp;"
        End If

    End Sub

</script>
<asp:UpdatePanel ID="updCalendar" runat="server">
<ContentTemplate>
<div id="ec_calendar">
    <div class="ec_title"><asp:LinkButton CausesValidation="false" runat="server" id="lnkMonth"></asp:LinkButton></div>

    <asp:PlaceHolder ID="plcCalendar" Runat="server">

	<div class="ec_calendartable">
        <asp:Calendar id="calEvents" runat="server" SelectionMode="Day" ShowTitle="False" ShowNextPrevMonth="False"
		    PrevMonthText="&lt; Previous Month" NextMonthText="&gt; Next Month" OnDayRender="DayRender"
		    CellPadding="0" CellSpacing="0"
		    BorderStyle="None" DayHeaderStyle-CssClass="ec_calendar_dayheader" DayStyle-CssClass="ec_calendar_day" SelectableDayCssClass="ec_calendar_selectday" TodayDayStyle-CssClass="ec_calendar_currentday" DayNameFormat="FirstLetter">
	    </asp:Calendar>
    </div>

	<div class="ec_calendar_nav clearfix">
	    <div class="ec_calendarprev"><asp:LinkButton CausesValidation="false" id="lnkPrevious" runat="server">&laquo; précédent</asp:LinkButton></div>
	    <div class="ec_calendarnext"><asp:LinkButton CausesValidation="false" id="lnkNext" runat="server">prochain &raquo;</asp:LinkButton></div>
	</div>

    </asp:PlaceHolder>
</div>
</ContentTemplate>
</asp:UpdatePanel>

<%-- Calendar.ascx - 7.0.4752 --%> 
