Option Strict On
Option Explicit On 

Imports System.Data.SqlClient
Imports AspWeb = System.Web
Imports AspWebControls = system.Web.UI.WebControls
Imports AspHtmlControls = System.Web.UI.HtmlControls
Imports Draw = System.Drawing
Imports Xml = System.Xml
Imports OWModules = ISL.OneWeb4.UI.Components.Modules
Imports OWHelpers = ISL.OneWeb4.UI.Components.Helpers
Imports OWUtility = ISL.OneWeb4.UI.Components.Utility
Imports OWSys = ISL.OneWeb4.Sys
Imports OWEnts = ISL.OneWeb4.Entities
Imports OWWeb = ISL.OneWeb4.UI
Imports OWWebControls = ISL.OneWeb4.UI.WebControls
Imports MSData = Microsoft.ApplicationBlocks.Data
Imports VAEnts = ISL.OneWeb.ClientApplications.NSNU.VotingApplication.Entities
Imports VADA = ISL.OneWeb.ClientApplications.NSNU.VotingApplication.DataAccess

Namespace UI.Admin

    ''' <summary>
    ''' This page allows the user to edit an existing Vote in the Vote Calendar application.
    ''' </summary>
    Public Class ManageVote
        Inherits AspWeb.UI.UserControl

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        'NOTE: The following placeholder declaration is required by the Web Form Designer.
        'Do not delete or move it.
        Private designerPlaceholderDeclaration As System.Object

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            ' ensure we can localize dates/numbers client-side
            Dim sm As AspWeb.UI.ScriptManager = AspWeb.UI.ScriptManager.GetCurrent(Me.Page)
            If sm IsNot Nothing Then sm.EnableScriptGlobalization = True

        End Sub

#End Region

#Region "Private members"

        Public AccessLevel As OWEnts.AccessLevel = OWEnts.AccessLevel.Default

        Protected litIntro As AspWebControls.Literal
        Protected ow_txtTitle As AspWebControls.TextBox
        Protected ow_txtLure As AspWebControls.TextBox
        Protected ow_txtQuestion As AspWebControls.TextBox
        Protected ow_txtAnswer1 As AspWebControls.TextBox
        Protected ow_txtAnswer2 As AspWebControls.TextBox
        Protected WithEvents ow_dtDisplayDateAndTime As Telerik.Web.UI.RadDateTimePicker
        Protected WithEvents ow_dtExpiryDateAndTime As Telerik.Web.UI.RadDateTimePicker


        Protected litOverallYesCount As AspWebControls.Literal
        Protected litOverallNoCount As AspWebControls.Literal
        Protected litOverallYesPercent As AspWebControls.Literal
        Protected litOverallNoPercent As AspWebControls.Literal
        Protected rptVotesByDate As AspWebControls.Repeater
        Protected rptVotesByDesignation As AspWebControls.Repeater
        Protected rptVotesByFacility As AspWebControls.Repeater
        Protected rptVotesBySector As AspWebControls.Repeater
        Protected rptVotesByRegion As AspWebControls.Repeater


        Protected ow_tsVote As Global.Telerik.Web.UI.RadTabStrip
        Protected ow_tbOne As Global.Telerik.Web.UI.RadTab
        Protected ow_tbTwo As Global.Telerik.Web.UI.RadTab

        Protected ow_mvVote As Global.Telerik.Web.UI.RadMultiPage
        Protected ow_pvAddVote As Global.Telerik.Web.UI.RadPageView
        Protected ow_pvResults As Global.Telerik.Web.UI.RadPageView

        Protected WithEvents ow_btnOK As AspWebControls.Button
        Protected WithEvents ow_btnCancel As AspWebControls.Button
        Protected WithEvents ow_btnDelete As AspWebControls.Button

        Private _user As OWEnts.User
        Private _site As OWEnts.Site
        Private _culture As String
        Private _app As New OWEnts.Application
        Private _main As Administration
        Private _vote As VAEnts.Vote

        ' the return address after an action is taken on this page
        Private _returnUrl As String


#End Region

#Region "Page Vote handlers"

        ''' <summary>
        ''' Page load Vote. Follows page_init Vote.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            ' set the parent property
            If TypeOf Me.Parent Is Administration Then
                _main = CType(Me.Parent, Administration)
            Else
                Throw New OWSys.BaseApplicationException(Me.GetType.Name & " is not contained in a Administration control.")
            End If
            Dim principal As OWSys.Security.OneWebPrincipal = CType(Context.User, OWSys.Security.OneWebPrincipal)

            _returnUrl = String.Format("{0}&mode={1}&vt={2}", _main.AdministrationUrl, "votes", Request.QueryString("vt"))

            If Request.QueryString("vid") Is Nothing OrElse Not IsNumeric(Request.QueryString("vid")) Then
                _main.Messages.Enqueue(OWHelpers.ResourceAccessor.GetText("voteIdInvalid", _main.GetType.BaseType))
                Response.Redirect(_returnUrl, False)
                Exit Sub
            End If

            With principal
                Me.AccessLevel = .MaximumAccess
                Me._user = CType(.Identity, OWSys.Security.OneWebIdentity).User
                Me._site = .CurrentSite
                Me._culture = .CurrentPage.Culture
            End With

            Page.Title = OWHelpers.ResourceAccessor.GetText("modifyVoteTitle", _main.GetType.BaseType)


            _vote = VADA.DBAccess.CreateVote
            _vote.VoteId = CInt(Request.QueryString("vid"))
            _vote.Culture = _culture
            LoadVote()

            If Not Page.IsPostBack AndAlso _vote.IsLoaded Then

                ow_txtTitle.Text = _vote.Title
                ow_txtLure.Text = _vote.Lure
                ow_txtQuestion.Text = _vote.Question.Replace("<br />", vbCrLf)
                ow_txtAnswer1.Text = _vote.Answer1
                ow_txtAnswer2.Text = _vote.Answer2
                ow_dtDisplayDateAndTime.SelectedDate = _vote.DisplayDate
                ow_dtExpiryDateAndTime.SelectedDate = _vote.ExpiryDate

                ow_txtQuestion.Attributes.Add("maxlength", CStr(ow_txtQuestion.MaxLength))

                GetOverallVotes()
                GetVotesByDate()
                GetVotesByDesignation()
                GetVotesByFacility()
                GetVotesBySector()
                GetVotesByRegion()
            End If

        End Sub

        ''' <summary>
        ''' Page pre-render Vote.  Occurs after page_load and after control click Votes.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            If Response.IsRequestBeingRedirected Then Return

            'set server-side control text from resource files
            ow_btnOK.Text = OWHelpers.ResourceAccessor.GetText("ok", _main.GetType())
            ow_btnCancel.Text = OWHelpers.ResourceAccessor.GetText("cancel", _main.GetType())
            ow_btnDelete.Text = OWHelpers.ResourceAccessor.GetText("delete", _main.GetType())

            ow_tbOne.Text = OWHelpers.ResourceAccessor.GetText("editVote", _main.GetType())
            ow_tbTwo.Text = OWHelpers.ResourceAccessor.GetText("results", _main.GetType())


            'the Update button is inactivated, and all fields are readonly, if the vote is expired.
            'The Vote title, lure, question, answer 1 text, answer 2 text, and display date fields 
            'are read only if the display date is less than or equal to the current date. 
            If _vote.VoteStatus = Entities.Status.Expired Then
                ow_btnOK.Enabled = False
                ow_txtTitle.Enabled = False
                ow_txtLure.Enabled = False
                ow_txtQuestion.Enabled = False
                ow_txtAnswer1.Enabled = False
                ow_txtAnswer2.Enabled = False
                ow_dtDisplayDateAndTime.Enabled = False
                ow_dtExpiryDateAndTime.Enabled = False

            ElseIf _vote.DisplayDate <= Now() Then 'If _vote.VoteStatus = Entities.Status.Active Then
                ow_txtTitle.Enabled = False
                ow_txtLure.Enabled = False
                ow_txtQuestion.Enabled = False
                ow_txtAnswer1.Enabled = False
                ow_txtAnswer2.Enabled = False
                ow_dtDisplayDateAndTime.Enabled = False

                'Only the expiry date may be edited in this circumstance, and it may only be set to a date greater than its current value (while not exceeding the 7 day active vote maxmimum).
                ow_dtExpiryDateAndTime.MinDate = _vote.ExpiryDate
            End If

        End Sub


#End Region

#Region "Control Vote handlers"

        ''' <summary>
        ''' Redirects the user to the list users page.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ow_btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ow_btnCancel.Click
            _main.Messages.Enqueue(String.Format(OWHelpers.ResourceAccessor.GetText("editVoteCancelled", _main.GetType()), _vote.Title))
            Response.Redirect(_returnUrl, False)
        End Sub

        Private Sub ow_btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ow_btnDelete.Click

            If VADA.DBAccess.DeleteVote(_vote) Then

                _main.Messages.Enqueue(String.Format(OWHelpers.ResourceAccessor.GetText("deleteVoteSuccess", _main.GetType.BaseType), _vote.Title))

                ' insert the control key for the event if it's not there yet
                Me.Cache.Insert(VAEnts.Constants.CacheControlKey, Now)

                ' create the return url without the event id as its now deleted
                _returnUrl = String.Format("{0}&mode={1}&vt={2}", _main.AdministrationUrl, "votes", Request.QueryString("vt"))
                Response.Redirect(_returnUrl, False)
            Else
                _main.Messages.Enqueue(String.Format(OWHelpers.ResourceAccessor.GetText("deleteVoteFailure", _main.GetType.BaseType), _vote.Title))
                Response.Redirect(_returnUrl, False)
            End If

        End Sub

        ''' <summary>
        ''' Saves the updated package.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ow_btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ow_btnOK.Click

            'confirm fields are filled; display errors or save Vote
            Page.Validate()
            If Page.IsValid Then

                _vote.Culture = _culture
                _vote.Title = ow_txtTitle.Text.Trim()
                _vote.Lure = ow_txtLure.Text.Trim()
                _vote.Answer1 = ow_txtAnswer1.Text.Trim()
                _vote.Answer2 = ow_txtAnswer2.Text.Trim()
                _vote.Question = ow_txtQuestion.Text.Trim().Replace(vbCrLf, "<br />")
                _vote.SiteId = _site.SiteId

                If ow_dtDisplayDateAndTime.SelectedDate IsNot Nothing Then
                    _vote.DisplayDate = CDate(ow_dtDisplayDateAndTime.SelectedDate)
                Else
                    _vote.DisplayDate = Date.MaxValue
                End If

                If ow_dtExpiryDateAndTime.SelectedDate IsNot Nothing Then
                    _vote.ExpiryDate = CDate(ow_dtExpiryDateAndTime.SelectedDate)
                Else
                    _vote.ExpiryDate = _vote.DisplayDate
                End If

                ' use the current user as the creator
                If Not _user.IsLoaded Then _main.LoadUser(_user)
                _vote.LastModifiedBy = _user.FullName
                _vote.LastModifiedDate = DateTime.Now

                Try
                    ' save the vote
                    If VADA.DBAccess.SaveVote(_vote) Then
                        _main.Messages.Enqueue(String.Format(OWHelpers.ResourceAccessor.GetText("editVoteSuccess", _main.GetType.BaseType), _vote.Title))

                        ' insert the control key for the Vote if it's not there yet
                        Me.Cache.Insert(VAEnts.Constants.CacheControlKey, Now)
                    Else
                        _main.Messages.Enqueue(String.Format(OWHelpers.ResourceAccessor.GetText("editVoteFailure", _main.GetType.BaseType), _vote.Title))
                    End If

                    Response.Redirect(_returnUrl, False)

                Catch ex As OWSys.DataException
                    ' name conflict caught
                    Dim vld As New AspWebControls.CustomValidator
                    vld.EnableViewState = False
                    vld.IsValid = False
                    vld.ErrorMessage = String.Format(OWHelpers.ResourceAccessor.GetText("editVoteFailure", _main.GetType.BaseType), _vote.Title)
                    vld.Text = String.Empty
                    vld.Display = AspWebControls.ValidatorDisplay.None
                    vld.CssClass = "ow_txt_inv"

                    Me.Controls.Add(vld)

                Catch ex As VotingApplication.Exceptions.OverlapDateException
                    ' name conflict caught
                    Dim vld As New AspWebControls.CustomValidator
                    vld.EnableViewState = False
                    vld.IsValid = False
                    vld.ErrorMessage = OWHelpers.ResourceAccessor.GetText("voteDateInvalid", _main.GetType.BaseType)
                    vld.Text = String.Empty
                    vld.Display = AspWebControls.ValidatorDisplay.None
                    vld.CssClass = "ow_txt_inv"

                    Me.Controls.Add(vld)

                End Try
            End If
        End Sub

#End Region

#Region "Server-side Validators"

        Sub DateValidation(ByVal source As Object, ByVal args As AspWebControls.ServerValidateEventArgs)
            Try
                ' Test that the start date is earlier than the end date if an end date is entered
                If ow_dtExpiryDateAndTime.SelectedDate IsNot Nothing Then
                    If ow_dtDisplayDateAndTime.SelectedDate <= ow_dtExpiryDateAndTime.SelectedDate Then
                        args.IsValid = True
                    Else
                        args.IsValid = False
                    End If
                Else
                    args.IsValid = True
                End If

            Catch ex As Exception
                args.IsValid = False
            End Try
        End Sub
#End Region


#Region "Private methods"


        Private Sub GetOverallVotes()

            Dim dt As DataTable = VADA.DBAccess.GetDataTable("vaGetVotesOverall", _vote.VoteId)

            litOverallYesCount.Text = dt.Rows(0).Item("answer1count").ToString
            litOverallYesPercent.Text = dt.Rows(0).Item("answer1percent").ToString
            litOverallNoCount.Text = dt.Rows(0).Item("answer2count").ToString
            litOverallNoPercent.Text = dt.Rows(0).Item("answer2percent").ToString

        End Sub

        Private Sub GetVotesByDate()

            Dim dt As DataTable = VADA.DBAccess.GetDataTable("vaGetVotesByDate", _vote.VoteId)

            rptVotesByDate.DataSource = dt
            rptVotesByDate.DataBind()

        End Sub


        Private Sub GetVotesByDesignation()

            Dim dt As DataTable = VADA.DBAccess.GetDataTable("vaGetVotesByDesignation", _vote.VoteId)

            rptVotesByDesignation.DataSource = dt
            rptVotesByDesignation.DataBind()

        End Sub


        Private Sub GetVotesByFacility()

            Dim dt As DataTable = VADA.DBAccess.GetDataTable("vaGetVotesByFacility", _vote.VoteId)

            rptVotesByFacility.DataSource = dt
            rptVotesByFacility.DataBind()

        End Sub

        Private Sub GetVotesByRegion()

            Dim dt As DataTable = VADA.DBAccess.GetDataTable("vaGetVotesByregion", _vote.VoteId)

            rptVotesByRegion.DataSource = dt
            rptVotesByRegion.DataBind()

        End Sub

        Private Sub GetVotesBySector()

            Dim dt As DataTable = VADA.DBAccess.GetDataTable("vaGetVotesBySector", _vote.VoteId)

            rptVotesBySector.DataSource = dt
            rptVotesBySector.DataBind()

        End Sub
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Load the Vote from the database.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2005-07-22	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub LoadVote()
            If VADA.DBAccess.LoadVote(_vote) Then
                litIntro.Text = String.Format(OWHelpers.ResourceAccessor.GetText("editVoteIntro", _main.GetType.BaseType), _vote.Title)
            Else
                _main.Messages.Enqueue(String.Format(OWHelpers.ResourceAccessor.GetText("editVoteInvalid", _main.GetType.BaseType), _vote.VoteId))
                Response.Redirect(_returnUrl, False)
            End If

        End Sub

#End Region

    End Class

End Namespace

'Copyright 2005 Internet Solutions Limited
