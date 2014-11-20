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

    Partial Public Class Vote
        Inherits AspWeb.UI.UserControl

#Region "Private Members"

        Private _site As OWEnts.Site
        Private _culture As OWEnts.Culture
        Private _main As Application

        Private _votes As VAEnts.VoteCollection
        Private _member As OWEnts.Member
        Private _userVote As VAEnts.UserVote

#End Region

#Region "Page events"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' set the parent property
            If TypeOf Me.Parent Is Application Then _main = CType(Me.Parent, Application)
            Dim principal As OWSys.Security.OneWebPrincipal = CType(Context.User, OWSys.Security.OneWebPrincipal)

            With principal
                Me._site = .CurrentSite
                Me._culture = .CurrentSiteCulture
            End With

            ' get logged on extranet member
            If _main.IsUserLoggedOn Then
                _member = _main.GetCurrentMember

            Else
                SetVisiblePlaceholder(plcAdminLoggedOn)
                Exit Sub
            End If

        End Sub

        ''' <summary>
        ''' Page pre-render event.  Occurs after page_load and after control click events.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            'if response code is 301, 302 etc. return
            If Response.IsRequestBeingRedirected Then Return

            ' see if the logged on member is authenticated against the Nurses management system
            If _member IsNot Nothing AndAlso _member.AuthenticationProvider <> VAEnts.Constants.AuthenticationProviderName Then
                SetVisiblePlaceholder(Nothing)
                Exit Sub

            Else
                LoadVotes()

                If _votes IsNot Nothing Then

                    ' we only need the active vote, and there can only be one
                    _votes.Filter("IsDeleted", False)
                    _votes.Filter("VoteStatus", VAEnts.Status.Active, OneWeb4.Entities.FilterOrder.EqualTo)

                    If _votes.Count = 1 Then
                        Me.VoteID = _votes(0).VoteId
                        litQuestion.Text = _votes(0).Question
                        litExpires.Text = String.Format(litExpires.Text, _main.LocalizedDate(_votes(0).ExpiryDate))
                        litYes.Text = _votes(0).Answer1
                        litNo.Text = _votes(0).Answer2

                        ' see if the logged on member has voted on this yet
                        _userVote = VADA.DBAccess.CreateUserVote()
                        _userVote.VoteId = _votes(0).VoteId
                        _userVote.Username = _member.MemberName
                        LoadMemberVote()

                        If _userVote.IsLoaded Then
                            SetVisiblePlaceholder(plcAlreadyVoted)
                            litAlreadyVoted.Text = String.Format(litAlreadyVoted.Text, _main.LocalizedDate(_userVote.VoteDate))
                        Else
                            SetVisiblePlaceholder(plcVote)
                        End If
                    Else
                        ' no active vote - message member
                        SetVisiblePlaceholder(plcNoActiveVotes)
                    End If
                Else
                    ' no active vote - message member
                    SetVisiblePlaceholder(plcNoActiveVotes)
                End If
            End If



        End Sub

#End Region

#Region "Control events"

        ''' <summary>
        ''' Prepares and executes Ad add.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ow_btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click

            Page.Validate()
            If Page.IsValid Then

                Dim vote As New VAEnts.Vote
                Dim uv As New VAEnts.UserVote
                Dim voteID As Integer = Me.VoteID

                vote.VoteId = Me.VoteID
                uv.VoteId = Me.VoteID

                VADA.DBAccess.LoadVote(vote)

                uv.Username = _member.MemberName
                uv.VoteDate = DateTime.Now

                Try
                    If VADA.DBAccess.AddUserVote(uv) Then
                        If Me.rbYes.Checked Then vote.Answer1Count += 1
                        If Me.rbNo.Checked Then vote.Answer2Count += 1
                        VADA.DBAccess.UpdateVoteCounts(vote)
                    Else
                        SetVisiblePlaceholder(plcVoteFailed)
                    End If
                Catch ex As Exception
                    OWHelpers.ExceptionPublisher.Publish(ex)
                    If _main.IsAdminLoggedOn Then
                        SetVisiblePlaceholder(plcException)
                        litException.Text = ex.StackTrace
                    End If
                End Try
            End If
        End Sub

#End Region


#Region "Private Methods"

        ''' <summary>
        ''' Gets or sets the administration mode of the control.
        ''' </summary>
        ''' <returns></returns>
        Private Property VoteID() As Integer
            Get
                If ViewState("VoteId") Is Nothing Then ViewState("VoteId") = 0
                Return CInt(ViewState("VoteId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("VoteId") = Value
            End Set
        End Property

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

        Private Sub SetVisiblePlaceholder(ByVal plc As AspWebControls.PlaceHolder)
            ' no forms available
            plcVote.Visible = CBool(plc Is plcVote)
            plcAlreadyVoted.Visible = CBool(plc Is plcAlreadyVoted)
            plcAdminLoggedOn.Visible = CBool(plc Is plcAdminLoggedOn)
            plcException.Visible = CBool(plc Is plcException)
            plcVoteFailed.Visible = CBool(plc Is plcVoteFailed)
            plcNoActiveVotes.Visible = CBool(plc Is plcNoActiveVotes)

            plcQuestion.Visible = plcVote.Visible Or plcAlreadyVoted.Visible
        End Sub

#End Region


    End Class

End Namespace
