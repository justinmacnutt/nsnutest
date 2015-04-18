<%@ Control Language="vb" AutoEventWireup="false" Inherits="ISL.OneWeb4.UI.UserControls.BaseControl" %>
<%@ Register TagPrefix="OWWeb" Assembly="ISL.OneWeb4.UI.Controls" Namespace="ISL.OneWeb4.UI.WebControls" %>
<%@ Register TagPrefix="OWSite" TagName="jQuery" Src="~/core/controls/IncludejQuery.ascx" %>

<script type="text/VB" runat="server">
		
	Protected ci As Globalization.CultureInfo
	Protected dayMap(5)() As Integer
	Protected timezone As String
	Public Overrides ReadOnly Property ResourceAccessType() As System.Type
		Get
			Return GetType(ISL.OneWeb4.Modules.EventCalendar.UI.App.Application)
		End Get
	End Property
    
	Public Sub Page_Load(sender As Object, args As System.EventArgs) Handles Me.Load
		ci = CType(Context.User, ISL.OneWeb4.Sys.Security.OneWebPrincipal).CurrentSiteCulture.GetSpecificCultureInfo()
		MyBase.RegisterClientCultureInfo("Site", ci)
				
		Dim site As ISL.OneWeb4.Entities.Site = CType(Context.User, ISL.OneWeb4.Sys.Security.OneWebPrincipal).CurrentSite
		timezone = site.TimeZone.StandardName & If(site.TimeZone.SupportsDaylightSavingTime, "/" & site.TimeZone.DaylightName, "")

		' create the calendar day map for the current month
		Dim currDate = New Date(DateTime.Now.Year, DateTime.Now.Month, 1)
		Dim wk As Integer = 0
		ReDim dayMap(wk)(6)
		For i As Integer = 1 To DateTime.DaysInMonth(currDate.Year, currDate.Month)
			dayMap(wk)(CInt(currDate.DayOfWeek)) = currDate.Day
			currDate = currDate.AddDays(1)
			If currDate.DayOfWeek = DayOfWeek.Sunday Then
				wk += 1
				ReDim dayMap(wk)(6)
			End If
		Next
		If wk < 5 Then ReDim dayMap(5)(6)
		rptCalDays.DataSource = dayMap
		rptCalDays.DataBind()
	End Sub

</script>

<OWSite:jQuery runat="server" />
<OWWeb:ScriptBlock runat="server" ID="scrPreview" ScriptType="text/javascript" IncludeLocation="Startup" DocumentSource="../../Application.previews.js" EnableViewState="false"/>
<OWWeb:Stylesheet runat="server" Mode="Import" Url="../../Application.previews.css" EnableViewState="false" />

<div id="ec_previewContainer">
<div id="ec_eventCalendar">

    <div class="ec_List clearfix" id="ec_List">

		<div class="ec_listItem">
			<div class="ec_listInfo">
				<h2 class="ec_listDate"><span class="ec_date" title="<%= DateTime.Now.AddDays(1).ToString("s")%>"></span> - <span class="ec_date" title="<%= DateTime.Now.AddDays(2).ToString("s")%>"></span>,
				<span class="ec_time" title="<%= DateTime.Today.AddMinutes((9 * 60) + 30).ToString("s")%>"></span> - <span class="ec_time" title="<%= DateTime.Today.AddHours(13).ToString("s")%>"></span></h2>
				<h2 class="ec_listEventName"><a href="javascript:void(01);" class="ec_listLink" title="Event name">Event name value</a></h2>
				<p class="ec_listLocation">Location value</p>
				<p class="ec_listCategory">Category value</p>
				<p class="ec_listSummary">if a summary is available, it will list here.</p>
			</div>
		</div>
		<div class="ec_hr"></div>

		<div class="ec_listItem">
			<div class="ec_listInfo">
				<h2 class="ec_listDate"><span class="ec_date" title="<%= DateTime.Now.AddDays(3).ToString("s")%>"></span>,
				<span class="ec_time" title="<%= DateTime.Today.AddMinutes((9 * 60) + 30).ToString("s")%>"></span> - <span class="ec_time" title="<%= DateTime.Today.AddHours(13).ToString("s")%>"></span></h2>
				<h2 class="ec_listEventName"><a href="javascript:void(02);" class="ec_listLink" title="Event name">Event name value</a></h2>
				<p class="ec_listLocation">Location value</p>
				<p class="ec_listCategory">Category value</p>
				<p class="ec_listSummary">if a summary is available, it will list here.</p>
			</div>
		</div>

		<div class="ec_hr"></div>

	    <%-- Pagination --%>
	    <div class="ow_pagerListWrapper">
		    <ul id="ow_pagerList">
			    <li class="ow_pagerLinks"><strong>1</strong></li>
			    <li class="ow_pagerLinks"><a id="ec_lnkPage2" href="javascript:void(2);" title="Page 2 des 1000 entrées">2</a></li>
			    <li class="ow_pagerLinks"><a id="ec_lnkPage3" href="javascript:void(3);" title="Page 3 des 1000 entrées">3</a></li>
			    <li class="ow_pagerLinks"><a id="ec_lnkPage4" href="javascript:void(4);" title="Page 4 des 1000 entrées">4</a></li>
			    <li class="ow_pagerLinks"><a id="ec_lnkPage5" href="javascript:void(5);" title="Page 5 des 1000 entrées">5</a></li>
				<li class="ow_pagerNext">
					<a id="ow_lnkNextGrp" href="javascript:void('grp');">...</a>
					<a id="ow_lnkNext" href="javascript:void('next');">Suivant &gt</a>
				</li>
		    </ul>
	    </div>
	</div>

    <div class="ec_List clearfix" id="ec_Lure">

		<div class="ec_listItem">
			<div class="ec_listInfo">
				<h2 class="ec_listDate"><span class="ec_date" title="<%= DateTime.Now.AddDays(1).ToString("s")%>"></span> - <span class="ec_date" title="<%= DateTime.Now.AddDays(2).ToString("s")%>"></span>,
				<span class="ec_time" title="<%= DateTime.Today.AddMinutes((9 * 60) + 30).ToString("s")%>"></span> - <span class="ec_time" title="<%= DateTime.Today.AddHours(13).ToString("s")%>"></span></h2>
				<h2 class="ec_listEventName"><a href="javascript:void(01);" class="ec_listLink" title="Event name">Event name value</a></h2>
				<p class="ec_listLocation">Location value</p>
				<p class="ec_listCategory">Category value</p>
			</div>
		</div>
		<div class="ec_hr"></div>

		<div class="ec_listItem">
			<div class="ec_listInfo">
				<h2 class="ec_listDate"><span class="ec_date" title="<%= DateTime.Now.AddDays(3).ToString("s")%>"></span>,
				<span class="ec_time" title="<%= DateTime.Today.AddMinutes((9 * 60) + 30).ToString("s")%>"></span> - <span class="ec_time" title="<%= DateTime.Today.AddHours(13).ToString("s")%>"></span></h2>
				<h2 class="ec_listEventName"><a href="javascript:void(02);" class="ec_listLink" title="Event name">Event name value</a></h2>
				<p class="ec_listLocation">Location value</p>
				<p class="ec_listCategory">Category value</p>
			</div>
		</div>
	    <p id="ec_moreEvents"><a href="javascript:void('more');" title="Link to more list of events">More events &raquo;</a></p>
	</div>

    <div class="ec_AddEvent clearfix" id="ec_AddEvent">
        <div class="ec_title">Submitter Details</div>
        <div class="ec_subtitle">The submitter details do not display on the event listing.</div>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
        <div class="ec_addEventFields">

            <div class="ec_addEventGroup">
	            <div class="formlabel"><label>Submitter’s name <span class="required">&bull;</span></label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" /></div>
            </div>
  
            <div class="ec_addEventGroup">
	            <div class="formlabel"><label>Submitter’s email <span class="required">&bull;</span></label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" /></div>
	            <div class="ec_tooltip">(person@isl.ca)</div>
            </div>

        </div>
 
        <hr />
        <div class="ec_title">Event Details</div>
        <div class="ec_subtitle">All of this information displays on the event listing except for the image code.</div>
 
        <div class="ec_addEventFields">    
		
            <div class="ec_addEventGroup">
	            <div class="formlabel"><label>Event name <span class="required">&bull;</span></label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" /></div>
            </div> 
      
            <div class="ec_addEventGroup" id="ec_addEventGroupCategory">
	            <div class="formlabel"><label>Category <span class="required">&bull;</span></label></div>
                <div class="forminput">
	                <select>
		                <option value="0">Select...</option>
		                <option value="1">Sports</option>
		                <option value="2">Entertainment</option>
		                <option value="3">Careers</option>
    	            </select>
                </div>
            </div>   
                   
            <div class="ec_addEventGroup">
	            <div class="formlabel"><label>Start date <span class="required">&bull;</span></label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" /><div class="ec_datePicker"></div></div>
                 
            </div>      
          
            <div class="ec_addEventGroup" style="margin-left:30px;">            
	            <div class="formlabel"><label>Start time</label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" /><div class="ec_timePicker"></div></div>
	            <div class="ec_tooltip">(<%= timezone %>)</div>

	            <div class="formlabel"><label>End time</label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" /><div class="ec_timePicker"></div></div>
            </div>
 
          
            <div class="ec_addEventGroup">            
	            <div class="formlabel"><label>End date</label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" /><div class="ec_datePicker"></div></div>
	            <div class="ec_tooltip">(Leave blank for one-day events)</div>
            </div>
 
            <div class="ec_addEventGroup">
	            <div class="formlabel"><label>Location</label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" /></div>
            </div> 
   	
            <div class="ec_addEventGroup">
        	    <div class="formlabel"><label>Summary</label></div>
	            <div class="forminput"><textarea rows="3" cols="20" class="ec_txt" style="width:350px;"></textarea></div>   		
            </div>

            <div class="ec_addEventGroup">
	            <div class="formlabel"><label>Full description <span class="required">&bull;</span></label></div>
	            <div class="forminput"><textarea rows="3" cols="20" class="ec_txt" style="width:350px;"></textarea></div> 		        
            </div>
 
            <div class="ec_addEventGroup">
	            <div class="formlabel"><label>Contact name</label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" /></div>
            </div>

            <div class="ec_addEventGroup">
	            <div class="formlabel"><label>Contact phone</label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" /></div>
            </div>

            <div class="ec_addEventGroup">
	            <div class="formlabel"><label>Contact email</label></div>
	            <div class="forminput"><input type="text" maxlength="50" class="isl_txt" /></div>
	            <div class="ec_tooltip">(person@isl.ca)</div>
            </div>
    		
            <div class="ec_addEventGroup">
	            <div class="formlabel"><label>Event website</label></div>
	            <div class="forminput"><input type="text" class="isl_txt" /></div>
	            <div class="ec_tooltip">(http://www.isl.ca)</div>
            </div>
   		
            <div class="ec_addEventGroup">
	            <div class="formlabel"><label>Registration website</label></div>
	            <div class="forminput"><input type="text" class="isl_txt" /></div>
	            <div class="ec_tooltip">(http://www.isl.ca/register)</div>
            </div>

        </div><%--end of ec_addEventFields--%>

        <div class="ec_searchbutton">
            <div class="ec_button1"><input type="button" value="Submit" class="isl_btn" onclick="javascript:void(0);" /></div>
        </div>

    
    </div> 
    <%--end of ec_AddEvent--%>

    <div class="ec_Search clearfix" id="ec_Search">
    <div class="ec_titleSearch">Find Events</div>
        <div class="ec_searchEventFields">    

            <div class="ec_searchEventGroup">
	            <div class="formlabel"><label>Keyword</label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" readonly="readonly" /></div>
            </div> 
      
            <div class="ec_searchEventGroup" id="ec_searchEventGroupCategory">
	            <div class="formlabel"><label>Category</label></div>
                <div class="forminput">
	                <select>
		                <option value="0">Select...</option>
		                <option value="1">Sports</option>
		                <option value="2">Entertainment</option>
		                <option value="3">Careers</option>
    	            </select>
                </div>
            </div>   
                   
            <div class="ec_searchEventGroup">
	            <div class="formlabel"><label>From</label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" readonly="readonly" /><div class="ec_datePicker"></div></div>
            </div>      
          
            <div class="ec_searchEventGroup">
	            <div class="formlabel"><label>To</label></div>
	            <div class="forminput"><input type="text" class="ec_txt" readonly="readonly" readonly="readonly" /><div class="ec_datePicker"></div></div>
            </div>
 
            <div class="ec_searchbutton">
                <div class="ec_button1"><input type="button" value="Find Events" class="isl_btn"  onclick="javascript:void(0);"/><input type="button" value="Reset" class="isl_btn"  onclick="javascript:void(0);" /></div>
            </div>

        </div><%--end of ec_searchEventFields--%>

    </div>

    <div class="ec_Details clearfix" id="ec_Details">
			<div class="ec_detailsPager"><table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td style="text-align:left;"><a href="javascript:void('prev');" title="Précédent event" id="lnkPrev1">&laquo; Précédent</a></td>
					<td style="text-align:right;"><a href="javascript:void('next');" title="Suivant event" id="lnkNext1">Suivant &raquo;</a></td>
				</tr>
			</table></div>

			<div class="ec_detailsInfo">
				<div class="ec_headers clearfix">
					<div class="ec_registerlink"><a id="lnkRegister" href="javascript:void('register');">Register Now</a></div>
					<div class="ec_iCallink"><a href="javascript:void('ical');" class="ec_listLink" title="Add to calendar">Add to calendar</a></div>
					<div class="ec_details_info">
						<h2 class="ec_details_date"><span class="ec_date" title="<%= DateTime.Now.AddDays(1).ToString("s")%>"></span> - <span class="ec_date" title="<%= DateTime.Now.AddDays(2).ToString("s")%>"></span>,
						<span class="ec_time" title="<%= DateTime.Today.AddMinutes((9 * 60) + 30).ToString("s")%>"></span> - <span class="ec_time" title="<%= DateTime.Today.AddHours(13).ToString("s")%>"></span></h2>
						<p class="ec_details_location">Location value</p>
						<p class="ec_details_category">Category value</p>
						<div class="ec_details_website"><a id="lnkWebsite" href="javascript:void('web');">Consulter le site Web de l'&eacute;v&eacute;nement</a></div>
						<div class="ec_details_contactName">Contact : Bob Jones</div>
						<div class="ec_details_contactPhone">T&eacute;l&eacute;phone : 904 405-3104</div>
						<div class="ec_details_contactEmail">Courriel : <a href="mailto:theevent.com">bjones@theevent.com</a></div>
					</div>
					<div class="ec_details_logo"><div class="ec_image">Image</div></div>
				</div>
				<div class="ec_body">
					<div class="ec_detailsText">
						<strong>Heading 1</strong>
						<p>This is the full description.  It should include details such as cost, venue information, time of day, and how to get tickets.</p>
						<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque pretium molestie enim. Aliquam non eleifend diam. Suspendisse potenti. Donec nec faucibus enim. Nullam adipiscing sapien sit amet metus rhoncus ac rhoncus justo lacinia. Nunc at consectetur nisi. In hac habitasse platea dictumst. Vivamus vitae est tellus.</p>
 					</div>
				</div>
			</div>

			<div class="ec_detailsPager"><table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td style="text-align:left;"><a href="javascript:void('prev');" title="Précédent event" id="lnkPrev2">&laquo; Précédent</a></td>
					<td style="text-align:right;"><a href="javascript:void('next');" title="Suivant event" id="lnkNext2">Suivant &raquo;</a></td>
				</tr>
			</table></div>
    </div>

    <div class="ec_Calendar clearfix" id="ec_Calendar">
		<div class="ec_title"><a id="ecCalendar_lnkMonth" href="javascript:void('mth')"><%= DateTime.Today.ToString("Y", ci)%></a></div>
		<div class="ec_calendartable"><table style="border-width: 1px; border-collapse: collapse;" id="ecCalendar_calEvents"
				title="Calendar" cellspacing="0" cellpadding="0"><tbody>
					<tr>
						<th class="ec_calendar_dayheader" abbr="Dimanche" scope="col" align="center">D</th>
						<th class="ec_calendar_dayheader" abbr="Lundi" scope="col" align="center">L</th>
						<th class="ec_calendar_dayheader" abbr="Mardi" scope="col" align="center">M</th>
						<th class="ec_calendar_dayheader" abbr="Mecredi" scope="col" align="center">M</th>
						<th class="ec_calendar_dayheader" abbr="Jeudi" scope="col" align="center">J</th>
						<th class="ec_calendar_dayheader" abbr="Vendredi" scope="col" align="center">V</th>
						<th class="ec_calendar_dayheader" abbr="Samedi" scope="col" align="center">S</th>
					</tr>
					<asp:Repeater ID="rptCalDays" runat="server" >
					<ItemTemplate>
						<tr>
						<asp:Repeater runat="server" DataSource=<%# Container.DataItem %>>
							<ItemTemplate>
							<td style="width: 14%;" class="<%# If(Container.DataItem <> DateTime.Today.Day, "ec_calendar_day", "ec_calendar_selectday")%>" align="center">
							<%# If(Container.DataItem = 0, "&nbsp", "<a href=""javascript:void(" & CStr(Container.DataItem) & ")"">" & CStr(Container.DataItem) & "</a>")%>
							</td></ItemTemplate>
						</asp:Repeater>
						</tr>
					</ItemTemplate>
					</asp:Repeater>
				</tbody>
			</table>
		</div>
		<div class="ec_calendar_nav clearfix">
			<div class="ec_calendarprev"><a id="ecCalendar_lnkPrevious" href="javascript:void('prev')">« précédent</a></div>&nbsp;
			<div class="ec_calendarnext"><a id="ecCalendar_lnkNext" href="javascript:void('next')">prochain »</a></div>
		</div>
    </div>

</div><%--end of ec_eventCalendar--%>
</div><%--end of ec_previewContainer--%>
<p class="ow_footnote"><asp:Literal runat="server" Text="footnote"/></p>

<%-- Application.ascx - 7.0.4752 --%> 
