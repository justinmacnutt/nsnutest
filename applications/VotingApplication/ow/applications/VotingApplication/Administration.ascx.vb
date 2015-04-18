
Option Strict On
Option Explicit On

Imports AspWeb = System.Web
Imports AspWebControls = System.Web.UI.WebControls
Imports AspHtmlControls = System.Web.UI.HtmlControls
Imports OWEnts = ISL.OneWeb4.Entities
Imports OWSys = ISL.OneWeb4.Sys
Imports OWWeb = ISL.OneWeb4.UI
Imports OWWebControls = ISL.OneWeb4.UI.WebControls

Namespace UI.Admin

    Partial Public Class Administration
        Inherits OWWeb.Applications.BaseApplication

#Region "Private Members"

#End Region

#Region "Constructors"

#End Region

#Region "UserControl Events"


        Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Init
            ''Put user code to initialize the page here
            'Dim user As OWEnts.User

            '' get the current users' application access
            'If TypeOf context.User.Identity Is OWSys.Security.OneWebIdentity Then
            '    Dim principal As OWSys.Security.OneWebPrincipal = CType(context.User, OWSys.Security.OneWebPrincipal)
            '    user = CType(principal.Identity, OWSys.Security.OneWebIdentity).User
            '    Me.LoadUserInternal(user)
            '    Me.LoadUserAccessInternal(user)

            'End If

            ' select the correct admin user control
            Dim admctl As AspWeb.UI.Control
            If Me.IsAdministering Then
                ' get the mode parameter
                If Not Request.Params("mode") Is Nothing Then
                    AdminMode = Request.Params("mode").Trim
                End If
                Select Case Me.AdminMode
                    Case "listvotes"
                        admctl = CType(Me.LoadControl("ListVotes.ascx"), AspWeb.UI.UserControl)
                    Case "managevote"
                        admctl = CType(Me.LoadControl("ManageVote.ascx"), AspWeb.UI.UserControl)
                    Case "addvote"
                        admctl = CType(Me.LoadControl("AddVote.ascx"), AspWeb.UI.UserControl)
                    Case Else
                        ' event list is the default control
                        admctl = CType(Me.LoadControl("ListVotes.ascx"), AspWeb.UI.UserControl)
                End Select

                If Not admctl Is Nothing Then

                    ' add the admin user control
                    admctl.ID = "app"
                    Me.Controls.Add(admctl)
                End If
            Else
                '     ow_mnuEventCalendar.Highlighted = False             ' Added By JDA, 01/19/2012 - not in admin page, do de-hilight the main menu item
            End If

            ' get the current users' application access
            If TypeOf Context.User.Identity Is OWSys.Security.OneWebIdentity Then
                Me.LoadUserAccessInternal(CType(Context.User.Identity, OWSys.Security.OneWebIdentity).User)
            End If

        End Sub

        ''' <summary>
        ''' Set up the UI values.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            If Me.IsRedirected Then Return
        End Sub


#End Region

#Region "Control Events"

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Gets or sets the administration mode of the control.
        ''' </summary>
        ''' <returns></returns>
        Public Property AdminMode() As String
            Get
                If ViewState("AdminMode") Is Nothing Then ViewState("AdminMode") = ""
                Return CStr(ViewState("AdminMode"))
            End Get
            Set(ByVal Value As String)
                ViewState("AdminMode") = Value
            End Set
        End Property

        ''' <summary>
        ''' Recursively loops through the control collections looking for a specifically-named control.
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="container"></param>
        ''' <returns></returns>
        Public Overloads Function FindControl(ByVal id As String, ByVal container As AspWeb.UI.Control) As AspWeb.UI.Control

            For Each ctl As AspWeb.UI.Control In container.Controls
                If ctl.ID = id Then Return ctl
                If ctl.Controls.Count > 0 Then
                    Dim ctl2 As AspWeb.UI.Control = FindControl(id, ctl)
                    If Not ctl2 Is Nothing Then Return ctl2
                End If
            Next
            Return Nothing

        End Function



#End Region

#Region "Friend Methods"

        Friend ReadOnly Property ApplicationEntity() As OWEnts.Application
            Get
                Return MyBase.ApplicationEntityInternal
            End Get
        End Property


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Loads the site object.
        ''' </summary>
        ''' <param name="site"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2005-07-26	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Friend Sub LoadSite(ByVal site As OWEnts.Site)
            Me.LoadSiteInternal(site, True)
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Loads the page object.
        ''' </summary>
        ''' <param name="page"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2005-07-26	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Friend Sub LoadPage(ByVal page As OWEnts.Page)
            Me.LoadPageInternal(page)
        End Sub

#Region "User Management Methods"

        ''' <summary>
        ''' Loads the specified user information.
        ''' </summary>
        ''' <param name="user"></param>
        Friend Sub LoadUser(ByVal user As OWEnts.User)
            Me.LoadUserInternal(user)
        End Sub

#End Region

#End Region

#Region "Protected Methods"

#End Region

#Region "Private Methods"


#End Region

    End Class

End Namespace
