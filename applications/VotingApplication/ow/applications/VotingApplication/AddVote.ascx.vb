Option Strict On
Option Explicit On 

Imports AspWeb = System.Web
Imports AspHtmlControls = System.Web.UI.HtmlControls
Imports AspWebControls = system.Web.UI.WebControls
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
    ''' This page allows the user to add a new Vote to the VoteCalendar application.
    ''' </summary>
    Public Class AddVote
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


        Protected ow_tsVote As Global.Telerik.Web.UI.RadTabStrip
        Protected ow_tbOne As Global.Telerik.Web.UI.RadTab
        Protected ow_tbTwo As Global.Telerik.Web.UI.RadTab

        Protected ow_mvVote As Global.Telerik.Web.UI.RadMultiPage
        Protected ow_pvAddVote As Global.Telerik.Web.UI.RadPageView
        Protected ow_pvMeta As Global.Telerik.Web.UI.RadPageView

        Protected WithEvents ow_btnOK As AspWebControls.Button
        Protected WithEvents ow_btnCancel As AspWebControls.Button

        Private _user As OWEnts.User
        Private _site As OWEnts.Site
        Private _culture As String
        Private _app As New OWEnts.Application
        Private _main As Administration

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

            _returnUrl = String.Format("{0}&mode={1}&vt={2}", _main.AdministrationUrl, "Votes", Request.QueryString("vt"))

            With principal
                Me.AccessLevel = .MaximumAccess
                Me._user = CType(.Identity, OWSys.Security.OneWebIdentity).User
                Me._site = .CurrentSite
                Me._culture = .CurrentPage.Culture
            End With

            Page.Title = OWHelpers.ResourceAccessor.GetText("addVoteTitle", _main.GetType.BaseType)


            If Not Page.IsPostBack Then
                ow_txtQuestion.Attributes.Add("maxlength", CStr(ow_txtQuestion.MaxLength))
                ow_txtAnswer1.Text = OWHelpers.ResourceAccessor.GetText("voteToAccept", _main.GetType.BaseType)
                ow_txtAnswer2.Text = OWHelpers.ResourceAccessor.GetText("voteToDecline", _main.GetType.BaseType)
            End If

            '1.	The display date must be equal to or greater than the current date
            ow_dtDisplayDateAndTime.MinDate = Today '.AddDays(1)

        End Sub

        ''' <summary>
        ''' Page pre-render Vote.  Occurs after page_load and after control click Votes.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            If Response.IsRequestBeingRedirected Then Return

            'set server-side control text from resource files
            ow_btnOK.Text = OWHelpers.ResourceAccessor.GetText("ok", _main.GetType.BaseType)
            ow_btnCancel.Text = OWHelpers.ResourceAccessor.GetText("cancel", _main.GetType.BaseType)
            litIntro.Text = String.Format(OWHelpers.ResourceAccessor.GetText("addVoteIntro", _main.GetType.BaseType))
            ow_tbOne.Text = OWHelpers.ResourceAccessor.GetText("addVote", _main.GetType.BaseType)

        End Sub

#End Region

#Region "Control Vote handlers"

        ''' <summary>
        ''' Redirects the user to the list Votes page.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ow_btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ow_btnCancel.Click
            _main.Messages.Enqueue(OWHelpers.ResourceAccessor.GetText("addVoteCancelled", _main.GetType.BaseType))
            Response.Redirect(_returnUrl, False)
        End Sub

        ''' <summary>
        ''' Prepares and executes Ad add.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ow_btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ow_btnOK.Click

            'confirm fields are filled; display errors or save new Vote and upload new file
            Page.Validate()
            If Page.IsValid Then

                ' good to go
                Dim vote As VAEnts.Vote = VADA.DBAccess.CreateVote
                vote.Culture = _culture
                vote.Title = ow_txtTitle.Text.Trim()
                vote.Lure = ow_txtLure.Text.Trim()
                vote.Answer1 = ow_txtAnswer1.Text.Trim()
                vote.Answer2 = ow_txtAnswer2.Text.Trim()
                vote.Question = ow_txtQuestion.Text.Trim().Replace(vbCrLf, "<br />")
                vote.SiteId = _site.SiteId

                If ow_dtDisplayDateAndTime.SelectedDate IsNot Nothing Then
                    vote.DisplayDate = CDate(ow_dtDisplayDateAndTime.SelectedDate)
                Else
                    vote.DisplayDate = Date.MaxValue
                End If

                If ow_dtExpiryDateAndTime.SelectedDate IsNot Nothing Then
                    vote.ExpiryDate = CDate(ow_dtExpiryDateAndTime.SelectedDate)
                Else
                    vote.ExpiryDate = vote.DisplayDate
                End If

                ' use the current user as the creator
                If Not _user.IsLoaded Then _main.LoadUser(_user)
                vote.LastModifiedBy = _user.FullName
                vote.LastModifiedDate = DateTime.Now

                Try


                    ' create the Vote
                    If VADA.DBAccess.AddVote(vote) Then
                        Dim nv As System.Collections.Specialized.NameValueCollection
                        If Not Request.QueryString("vt") Is Nothing AndAlso Request.QueryString("vt").Length > 0 Then
                            nv = OWHelpers.NameValueParser.ConvertSafeBase64ToNameValue(Request.QueryString("vt"))
                        Else
                            nv = New System.Collections.Specialized.NameValueCollection
                        End If
                        nv.Item("id") = CStr(vote.VoteId)
                        _returnUrl = String.Format("{0}&mode={1}&vt={2}", _main.AdministrationUrl, "Votes", OWHelpers.NameValueParser.ConvertNameValueToSafeBase64(nv))

                        _main.Messages.Enqueue(String.Format(OWHelpers.ResourceAccessor.GetText("addVoteSuccess", _main.GetType.BaseType), vote.Title))

                        ' insert the control key for the Vote if it's not there yet
                        Me.Cache.Insert(VAEnts.Constants.CacheControlKey, Now)

                    Else
                        _main.Messages.Enqueue(String.Format(OWHelpers.ResourceAccessor.GetText("addVoteFailure", _main.GetType.BaseType), vote.Title))
                    End If
                    Response.Redirect(_returnUrl, False)

                Catch ex As OWSys.DataException
                    ' name conflict caught
                    Dim vld As New AspWebControls.CustomValidator
                    vld.EnableViewState = False
                    vld.IsValid = False
                    vld.ErrorMessage = OWHelpers.ResourceAccessor.GetText("addVoteFailure", _main.GetType.BaseType)
                    vld.Text = String.Empty
                    vld.Display = AspWebControls.ValidatorDisplay.None
                    vld.CssClass = "ow_txt_inv"

                    Me.Controls.Add(vld)

                    ' The submitted date range must not fall within (i.e. overlap) another vote’s active date range.
                    ' means the start date must be greater than the expiry date of all votes in the table

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

#Region "Public methods"

#End Region

#Region "Private methods"

#End Region

    End Class

End Namespace

'Copyright 2005-2010 Internet Solutions Limited
