' Class ListVotes
' Documentation:

Option Strict On
Option Explicit On

Imports AspWeb = System.Web
Imports AspWebControls = System.Web.UI.WebControls
Imports AspHtmlControls = System.Web.UI.htmlControls
Imports MSData = Microsoft.ApplicationBlocks.Data
Imports OWEnts = ISL.OneWeb4.Entities
Imports OWSys = ISL.OneWeb4.Sys
Imports OWWeb = ISL.OneWeb4.UI
Imports OWHelpers = ISL.OneWeb4.UI.Components.Helpers
Imports VAEnts = ISL.OneWeb.ClientApplications.NSNU.VotingApplication.Entities
Imports VADA = ISL.OneWeb.ClientApplications.NSNU.VotingApplication.DataAccess

Namespace UI.Admin

    ''' <summary>
    ''' A OneWeb 5 application class for displaying the Votes in the system.
    ''' </summary>
    Public Class ListVotes
        Inherits AspWeb.UI.UserControl

#Region "Private Members"

        Protected litIntro As AspWebControls.Literal
        Protected ow_rptVotes As AspWebControls.Repeater
        Protected ow_EntityID As AspHtmlControls.HtmlInputHidden

        Protected WithEvents ow_pagerTop As OWWeb.UserControls.PagerList
        Protected WithEvents ow_pagerBottom As OWWeb.UserControls.PagerList
        Protected WithEvents ow_pagerSize As OWWeb.UserControls.PagerSize

        Protected ow_sortHeaderTitle As AspWebControls.LinkButton
        Protected ow_sortHeaderQuestion As AspWebControls.LinkButton
        Protected ow_sortHeaderDisplayDate As AspWebControls.LinkButton
        Protected ow_sortHeaderExpiryDate As AspWebControls.LinkButton
        Protected ow_sortHeaderStatus As AspWebControls.LinkButton
        Protected ow_sortHeaderAnswer1 As AspWebControls.LinkButton
        Protected ow_sortHeaderAnswer2 As AspWebControls.LinkButton

        Protected ow_plcHeaders As AspWebControls.PlaceHolder
        Protected ow_plcNoRecords As AspWebControls.PlaceHolder
        Protected ow_noRecords As New AspWebControls.Literal

        Protected WithEvents ow_btnAdd As AspWebControls.Button
        Protected WithEvents ow_btnManage As AspWebControls.LinkButton
        Protected ow_adminActionsRecord As AspWebControls.PlaceHolder

        Private _main As Administration
        Private _site As OWEnts.Site
        Private _pds As New AspWebControls.PagedDataSource
        Private _votes As New VAEnts.VoteCollection
#End Region

#Region "Constructors"

#End Region

#Region "UserControl Votes"

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here

            ' set the parent property
            If TypeOf Me.Parent Is Administration Then
                _main = CType(Me.Parent, Administration)
            Else
                Throw New OWSys.BaseApplicationException(Me.GetType.Name & " is not contained in a Administration control.")
            End If

            Dim principal As OWSys.Security.OneWebPrincipal = CType(Context.User, OWSys.Security.OneWebPrincipal)

            With principal
                Me._site = .CurrentSite
            End With

            Page.Title = OWHelpers.ResourceAccessor.GetText("manageVotes", _main.GetType.BaseType)


            ' get the Votes
            LoadVotes()

            If Not Me.IsPostBack Then
                SetListState()
            End If

        End Sub

        ''' <summary>
        ''' Page pre-render Vote.  Occurs after page_load.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            If Response.IsRequestBeingRedirected Then Return

            'set non-Literal server-side control text values with resources
            ow_btnAdd.Text = OWHelpers.ResourceAccessor.GetText("add", _main.GetType.BaseType) & "..."
            ow_btnManage.Text = OWHelpers.ResourceAccessor.GetText("managevote", _main.GetType.BaseType) & "..."

            ApplySortAndFilters()

            'set pager props
            With ow_pagerTop
                'If Not Me.IsPostBack Then .CurrentPage = 1
                .RecordCount = _votes.Count
            End With

            'set paged data source props
            With Me._pds
                .DataSource = _votes
                .AllowPaging = True
                .PageSize = ow_pagerTop.PageSize
                .CurrentPageIndex = ow_pagerTop.CurrentPage - 1
            End With

            'set the repeater data source and bind
            With Me.ow_rptVotes
                .DataSource = Me._pds
                .DataBind()
            End With

            Me.ow_plcHeaders.Visible = (_votes.Count > 0)
            Me.ow_plcNoRecords.Visible = (_votes.Count <= 0)

            SetPagerProperties()

        End Sub

#End Region

#Region "Control Events"


        Private Sub ow_btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ow_btnAdd.Click
            Dim address As String = String.Format("{0}&mode={1}&{2}", _main.AdministrationUrl, "addvote", GetStateString)
            Response.Redirect(address, False)
        End Sub

        ''' <summary>
        ''' Handles the viewing of an Vote.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ow_btnManage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ow_btnManage.Click
            Dim address As String = String.Format("{0}&mode={1}&vid={2}&{3}", _main.AdministrationUrl, "managevote", ow_EntityID.Value, GetStateString)
            Response.Redirect(address, False)
        End Sub


        Protected Sub SortColumns(ByVal sender As Object, ByVal e As AspWebControls.CommandEventArgs)

            ' set the sort orders on the user interface
            SetSorts(e.CommandName, CType(e.CommandArgument, OWEnts.SortOrder))
            'reset to first page
            ow_pagerTop.CurrentPage = 1
            SetPagerProperties()
            Dim address As String = String.Format("{0}&mode={1}&{2}", _main.AdministrationUrl, "listvotes", GetStateString)
            Response.Redirect(address, False)
        End Sub

        ''' <summary>
        ''' Handles the paging of the data when the user selects a new page.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub ow_Pager_PageIndexChanged(ByVal sender As Object, ByVal e As OWWeb.UserControls.PagerPageChangedEventArgs) Handles ow_pagerTop.PageIndexChanged, ow_pagerBottom.PageIndexChanged
            _pds.CurrentPageIndex = e.NewPageIndex - 1
            ow_pagerTop.CurrentPage = e.NewPageIndex
            SetPagerProperties()
            Dim address As String = String.Format("{0}&mode={1}&{2}", _main.AdministrationUrl, "listvotes", GetStateString)
            Response.Redirect(address, False)
        End Sub

        ''' <summary>
        ''' Handles the paging of the data when the user selects a different page size of records.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub ow_Pager_PageSizeChanged(ByVal sender As Object, ByVal e As OWWeb.UserControls.PagerPageSizeChangedEventArgs) Handles ow_pagerSize.PageSizeChanged
            ow_pagerTop.PageSize = e.NewPageSize

            'reset to first page
            ow_pagerTop.CurrentPage = 1
            SetPagerProperties()
            Dim address As String = String.Format("{0}&mode={1}&{2}", _main.AdministrationUrl, "listvotes", GetStateString)
            Response.Redirect(address, False)
        End Sub

#End Region

#Region "Public Methods"

#End Region

#Region "Protected Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the 'Yes' or 'No' from a boolean value.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2005-07-21	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Function CYesNo(ByVal value As Boolean) As String
            Select Case value
                Case True : Return OWHelpers.ResourceAccessor.GetText("yes", _main.GetType.BaseType)
                Case False : Return OWHelpers.ResourceAccessor.GetText("no", _main.GetType.BaseType)
            End Select
        End Function

        Protected Function GetStatus(ByVal vote As VAEnts.Vote) As String
            Select Case vote.VoteStatus
                Case VAEnts.Status.Active : Return OWHelpers.ResourceAccessor.GetText("active", _main.GetType.BaseType)
                Case VAEnts.Status.Pending : Return OWHelpers.ResourceAccessor.GetText("pending", _main.GetType.BaseType)
                Case VAEnts.Status.Expired : Return OWHelpers.ResourceAccessor.GetText("expired", _main.GetType.BaseType)
            End Select
        End Function
#End Region

#Region "Private Methods"


        Private Sub ApplySortAndFilters()
            ' sort now
            If _votes.Count > 0 And Not Me.ViewState("SortColumn") Is Nothing Then
                _votes.Sort(CStr(Me.ViewState("SortColumn")), CType(Me.ViewState("SortOrder"), OWEnts.SortOrder))
            End If

        End Sub

        ''' <summary>
        ''' Load the Votes for the current site from the database, filtering out any deleted votes.
        ''' </summary>
        Private Sub LoadVotes()

            _votes = VADA.DBAccess.GetVotes(_site)
            _votes.Filter("IsDeleted", False)

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Synchronize the properties between the pager controls.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Guozhi Ma]	15/11/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub SetPagerProperties()
            ow_pagerBottom.RecordCount = ow_pagerTop.RecordCount
            ow_pagerSize.RecordCount = ow_pagerTop.RecordCount
            ow_pagerBottom.PageSize = ow_pagerTop.PageSize
            ow_pagerSize.PageSize = ow_pagerTop.PageSize
            ow_pagerBottom.CurrentPage = ow_pagerTop.CurrentPage
            ow_pagerBottom.Visible = ow_pagerTop.Visible
            ow_pagerSize.Enabled = ow_pagerTop.Visible
        End Sub

#End Region

#Region "State Methods"

        ''' <summary>
        ''' Create the string for preserving the list state.
        ''' </summary>
        Private Function GetStateString() As String

            Dim state As New System.Collections.Specialized.NameValueCollection
            state.Add("id", ow_EntityID.Value)
            state.Add("ps", CStr(ow_pagerTop.PageSize))
            state.Add("pg", CStr(ow_pagerTop.CurrentPage))
            state.Add("sc", CStr(Me.ViewState("SortColumn")))
            state.Add("so", CStr(CInt(Me.ViewState("SortOrder"))))
            Return "vt=" & OWHelpers.NameValueParser.ConvertNameValueToSafeBase64(state)

        End Function

        ''' <summary>
        ''' Retrieve the list state values from the state string.
        ''' </summary>
        ''' <param name="statestring"></param>
        Private Sub ParseStateString(ByVal statestring As String)

            Dim state As System.Collections.Specialized.NameValueCollection = OWHelpers.NameValueParser.ConvertSafeBase64ToNameValue(statestring)

            Try
                ow_EntityID.Value = state("id")
                ow_pagerTop.PageSize = CInt(state("ps"))
                ow_pagerTop.CurrentPage = CInt(state("pg"))
                SetSorts(state.Item("sc"), CType(state("so"), OWEnts.SortOrder))
                SetPagerProperties()
            Catch ex As System.Exception
                ' an exception occurred trying to re-establish the state.  allow defaults
                SetListDefaults()
            End Try

        End Sub

        ''' <summary>
        ''' Set the list defaults.
        ''' </summary>
        Private Sub SetListDefaults()

            ow_EntityID.Value = "0"
            ow_pagerTop.PageSize = 50
            ow_pagerTop.CurrentPage = 1
            SetPagerProperties()
            ' sort the page templates by EntityName as a default
            SetSorts("DisplayDate", OWEnts.SortOrder.Descending)

        End Sub

        ''' <summary>
        ''' Recover the list state from the querystring.
        ''' </summary>
        Private Sub SetListState()

            If Not String.IsNullOrEmpty(Request.QueryString("vt")) Then
                ParseStateString(Request.QueryString("vt"))
            Else
                SetListDefaults()
            End If

        End Sub

        ''' <summary>
        ''' Reset the attributes of sort column headers on a listing page.
        ''' </summary>
        Private Sub ResetSorts()

            ow_sortHeaderTitle.CommandArgument = CInt(OWEnts.SortOrder.Ascending).ToString
            ow_sortHeaderTitle.CssClass = ""
            ow_sortHeaderTitle.ToolTip = OWHelpers.ResourceAccessor.GetText("sortAsc", _main.GetType.BaseType)

            ow_sortHeaderQuestion.CommandArgument = CInt(OWEnts.SortOrder.Ascending).ToString
            ow_sortHeaderQuestion.CssClass = ""
            ow_sortHeaderQuestion.ToolTip = OWHelpers.ResourceAccessor.GetText("sortAsc", _main.GetType.BaseType)

            ow_sortHeaderDisplayDate.CommandArgument = CInt(OWEnts.SortOrder.Ascending).ToString
            ow_sortHeaderDisplayDate.CssClass = ""
            ow_sortHeaderDisplayDate.ToolTip = OWHelpers.ResourceAccessor.GetText("sortAsc", _main.GetType.BaseType)

            ow_sortHeaderExpiryDate.CommandArgument = CInt(OWEnts.SortOrder.Ascending).ToString
            ow_sortHeaderExpiryDate.CssClass = ""
            ow_sortHeaderExpiryDate.ToolTip = OWHelpers.ResourceAccessor.GetText("sortAsc", _main.GetType.BaseType)

            ow_sortHeaderStatus.CommandArgument = CInt(OWEnts.SortOrder.Ascending).ToString
            ow_sortHeaderStatus.CssClass = ""
            ow_sortHeaderStatus.ToolTip = OWHelpers.ResourceAccessor.GetText("sortAsc", _main.GetType.BaseType)

            ow_sortHeaderAnswer1.CommandArgument = CInt(OWEnts.SortOrder.Ascending).ToString
            ow_sortHeaderAnswer1.CssClass = ""
            ow_sortHeaderAnswer1.ToolTip = OWHelpers.ResourceAccessor.GetText("sortAsc", _main.GetType.BaseType)

            ow_sortHeaderAnswer2.CommandArgument = CInt(OWEnts.SortOrder.Ascending).ToString
            ow_sortHeaderAnswer2.CssClass = ""
            ow_sortHeaderAnswer2.ToolTip = OWHelpers.ResourceAccessor.GetText("sortAsc", _main.GetType.BaseType)

            Me.ViewState.Remove("SortColumn")
            Me.ViewState.Remove("SortOrder")
        End Sub

        ''' <summary>
        ''' Set the sort properties of the list user interface elements.
        ''' </summary>
        ''' <param name="column"></param>
        ''' <param name="order"></param>
        Private Sub SetSorts(ByVal column As String, ByVal order As OWEnts.SortOrder)
            ' get the current sort order
            Dim oppOrder As OWEnts.SortOrder = CType(IIf(order = OWEnts.SortOrder.Ascending, OWEnts.SortOrder.Descending, OWEnts.SortOrder.Ascending), OWEnts.SortOrder)
            Dim cssClass As String
            Dim toolTip As String
            If order = OWEnts.SortOrder.Ascending Then
                cssClass = "isl_sort_asc"
                toolTip = OWHelpers.ResourceAccessor.GetText("sortDesc", _main.GetType.BaseType)
            Else
                cssClass = "isl_sort_desc"
                toolTip = OWHelpers.ResourceAccessor.GetText("sortAsc", _main.GetType.BaseType)
            End If

            ' reset sort column headers to their defaults
            ResetSorts()

            ' determine which link was clicked then flip the sort order so if clicked again will sort in opposite direction
            Select Case column

                Case "Title"
                    ow_sortHeaderTitle.CommandArgument = CStr(CInt(oppOrder))
                    ow_sortHeaderTitle.CssClass = cssClass
                    ow_sortHeaderTitle.ToolTip = toolTip

                Case "Question"
                    ow_sortHeaderQuestion.CommandArgument = CStr(CInt(oppOrder))
                    ow_sortHeaderQuestion.CssClass = cssClass
                    ow_sortHeaderQuestion.ToolTip = toolTip

                Case "DisplayDate"
                    ow_sortHeaderDisplayDate.CommandArgument = CStr(CInt(oppOrder))
                    ow_sortHeaderDisplayDate.CssClass = cssClass
                    ow_sortHeaderDisplayDate.ToolTip = toolTip

                Case "ExpiryDate"
                    ow_sortHeaderExpiryDate.CommandArgument = CStr(CInt(oppOrder))
                    ow_sortHeaderExpiryDate.CssClass = cssClass
                    ow_sortHeaderExpiryDate.ToolTip = toolTip

                Case "Answer1"
                    ow_sortHeaderAnswer1.CommandArgument = CStr(CInt(oppOrder))
                    ow_sortHeaderAnswer1.CssClass = cssClass
                    ow_sortHeaderAnswer1.ToolTip = toolTip

                Case "Answer2"
                    ow_sortHeaderAnswer2.CommandArgument = CStr(CInt(oppOrder))
                    ow_sortHeaderAnswer2.CssClass = cssClass
                    ow_sortHeaderAnswer2.ToolTip = toolTip

                Case "VoteStatus"
                    ow_sortHeaderStatus.CommandArgument = CStr(CInt(oppOrder))
                    ow_sortHeaderStatus.CssClass = cssClass
                    ow_sortHeaderStatus.ToolTip = toolTip


                Case Else 'default
                    column = "DisplayDate"
                    ow_sortHeaderDisplayDate.CommandArgument = CStr(CInt(oppOrder))
                    ow_sortHeaderDisplayDate.CssClass = cssClass
                    ow_sortHeaderDisplayDate.ToolTip = toolTip

            End Select

            ' save the properties into the ViewState
            Me.ViewState("SortColumn") = column
            Me.ViewState("SortOrder") = CInt(order)

        End Sub


#End Region

    End Class

End Namespace

' Copyright 2007 Internet Solutions Ltd.
