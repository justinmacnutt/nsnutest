Option Strict On
Option Explicit On

Imports System.Text.RegularExpressions
Imports AspWeb = System.Web
Imports AspWebControls = System.Web.UI.WebControls
Imports AspWebCache = System.Web.Caching
Imports OWEnts = ISL.OneWeb4.Entities
Imports OWHelpers = ISL.OneWeb4.UI.Components.Helpers
Imports OWSys = ISL.OneWeb4.Sys
Imports OWWeb = ISL.OneWeb4.UI
Imports VAEnts = ISL.OneWeb.ClientApplications.NSNU.VotingApplication.Entities
Imports VADA = ISL.OneWeb.ClientApplications.NSNU.VotingApplication.DataAccess
Imports System.Collections.Generic


Namespace UI.App

    Partial Public Class Lure
        Inherits AspWeb.UI.UserControl


#Region "Private Members"

        Private _site As OWEnts.Site
        Private _culture As OWEnts.Culture
        Private _main As Application

        Private _votes As VAEnts.VoteCollection
        Private _userVote As VAEnts.UserVote


#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' set the parent property
            If TypeOf Me.Parent Is Application Then _main = CType(Me.Parent, Application)
            Dim principal As OWSys.Security.OneWebPrincipal = CType(Context.User, OWSys.Security.OneWebPrincipal)

            With principal
                Me._site = .CurrentSite
                Me._culture = .CurrentSiteCulture
            End With

        End Sub

        ''' <summary>
        ''' Page pre-render event.  Occurs after page_load and after control click events.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            'if response code is 301, 302 etc. return
            If Response.IsRequestBeingRedirected Then Return

            ' get logged on extranet member
            If _main.IsUserLoggedOn Then

                ' see if the logged on member is authenticated against the Nurses management system
                Dim member As OWEnts.Member = _main.GetCurrentMember

                If member.AuthenticationProvider = VAEnts.Constants.AuthenticationProviderName Then

                    LoadVotes()

                    If _votes IsNot Nothing Then

                        ' we only need the active vote, and there can only be one
                        _votes.Filter("IsDeleted", False)
                        _votes.Filter("VoteStatus", VAEnts.Status.Active, OneWeb4.Entities.FilterOrder.EqualTo)

                        If _votes.Count = 1 Then


                            _userVote = VADA.DBAccess.CreateUserVote()
                            _userVote.VoteId = _votes(0).VoteId
                            _userVote.Username = member.MemberName
                            LoadMemberVote()

                            If _userVote.IsLoaded Then
                                'member has already voted - hide the control
                                SetVisiblePlaceholder(Nothing)
                            Else
                                ' good to go
                                SetVisiblePlaceholder(plcLure)
                                Me.litLure.Text = _votes(0).Lure
                                Me.litExpiryDate.Text = _main.LocalizedDate(_votes(0).ExpiryDate)
                                Me.lnkVote.NavigateUrl = Me.GetVoteAddress(_votes(0).VoteId)
                            End If

                        Else
                            ' more than one active vote - hide the control
                            SetVisiblePlaceholder(Nothing)
                        End If
                    Else
                        ' no votes - hide the control
                        SetVisiblePlaceholder(Nothing)
                    End If


                Else
                    ' member logged on but not authenticated against Nurses memebership
                    SetVisiblePlaceholder(Nothing)
                End If

            ElseIf _main.IsAdminLoggedOn Then
                ' no member, but there is a logged on OW administrator - show message
                SetVisiblePlaceholder(plcAdminLoggedOn)
            Else
                ' no member, no logged on OW administrator - hide the control
                SetVisiblePlaceholder(Nothing)
            End If

        End Sub

#Region "Private Methods"

        ''' <summary>
        ''' Load the Votes for the current site from the database.
        ''' </summary>
        Private Sub LoadVotes()

            _votes = VADA.DBAccess.GetVotes(_site)

        End Sub

        ''' <summary>
        ''' Load the vote for the current member from the database.
        ''' </summary>
        Private Sub LoadMemberVote()

            VADA.DBAccess.LoadMemberVote(_userVote)

        End Sub

        Private Function GetVoteAddress(ByVal voteId As Integer) As String


            '  Dim sb As New System.Text.StringBuilder
            ' Dim pathInfo As String = String.Format("/{0}/{1}", _main.GetCanonicalEventName(job), CStr(job.JobId))

            ' append the proper query string parameters
            _main.QueryString.Clear()
            _main.QueryString.Add("mode", CStr(VAEnts.Mode.Vote))

            If Not _main.ResultsPage = String.Empty AndAlso Not ResolveUrl(_main.ResultsPage) = Request.Url.AbsolutePath Then
                Return String.Concat(Me.ResolveUrl(_main.ResultsPage), "?", _main.Query)
            Else
                Return String.Concat(Me.Request.Url.AbsolutePath, "?", _main.Query)
            End If

        End Function


        Private Sub SetVisiblePlaceholder(ByVal plc As AspWebControls.PlaceHolder)
            ' no forms available
            plcLure.Visible = CBool(plc Is plcLure)
            plcAdminLoggedOn.Visible = CBool(plc Is plcAdminLoggedOn)

        End Sub

#End Region


    End Class

End Namespace


