Option Strict On
Option Explicit On

Imports System.Text
Imports System.Text.RegularExpressions
Imports AspWeb = System.Web
Imports AspWebControls = System.Web.UI.WebControls
Imports AspHtmlControls = System.Web.UI.HtmlControls
Imports AspWebCache = System.Web.Caching
Imports OWEnts = ISL.OneWeb4.Entities
Imports OWSys = ISL.OneWeb4.Sys
Imports OWWeb = ISL.OneWeb4.UI
Imports OWUtil = ISL.OneWeb4.UI.Components.Utility
Imports OWHelpers = ISL.OneWeb4.UI.Components.Helpers
Imports VAEnts = ISL.OneWeb.ClientApplications.NSNU.VotingApplication.Entities
Imports VADA = ISL.OneWeb.ClientApplications.NSNU.VotingApplication.DataAccess
Namespace UI.App


    Partial Public Class Application
        Inherits OWWeb.Applications.BaseApplication


#Region "Private Members"

        Private _isDefault As Boolean
        Private _app As AspWeb.UI.UserControl
        Private _queryString As System.Collections.Specialized.NameValueCollection

        Public Const QueryStringKey As String = "vt"

#End Region

#Region "Constructors"

#End Region


#Region "UserControl Events"

        Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Init

            ' parse the event calendar part of the querystring
            Try
                ParseQueryString()

                ' get the mode parameter
                Try
                    If Not String.IsNullOrEmpty(Me.QueryString("mode")) Then
                        Me.Mode = CType(System.Enum.Parse(GetType(VAEnts.Mode), Me.QueryString("mode").Trim, True), VAEnts.Mode)
                    End If
                Catch ex As System.Exception
                    ' leave mode as set by base application

                End Try

                '' get the mode parameter
                '            Dim qsmode As VAEnts.Mode = VAEnts.Mode.List
                'Try
                '	If Not Me.QueryString("mode") Is Nothing Then qsmode = CType(System.Enum.Parse(GetType(VAEnts.Mode), Me.QueryString("mode").Trim, True), VAEnts.Mode)
                'Catch ex As System.Exception
                '	' leave mode as set by base application
                '	_isDefault = True
                'End Try
                '            If Me.Mode = VAEnts.Mode.List Then
                '                ' always allow another mode to be set when in the default mode, but make it like it's the default
                '                Me.Mode = qsmode
                '                _isDefault = True
                '            Else
                '                ' determine if the set mode can changed intelligently
                '                ' essentially, this groups together modes that can appear in the same content block
                '                Select Case Me.Mode
                '                    Case VAEnts.Mode.List, VAEnts.Mode.Details, Entities.Mode.Apply
                '                        ' list/result/details modes (main content area)
                '                        Select Case qsmode
                '                            Case VAEnts.Mode.List, _
                '                             VAEnts.Mode.Details, _
                '                             VAEnts.Mode.Apply
                '                                _isDefault = CBool(Me.Mode = qsmode)
                '                                Me.Mode = qsmode
                '                            Case Else
                '                                ' don't change
                '                                _isDefault = True
                '                        End Select

                '                        'Case Else
                '                        '    Me.Mode = VAEnts.Mode.Default
                '                End Select
                '            End If
                'Dim id As String = "emop" & System.Enum.GetName(GetType(VAEnts.Mode), Me.Mode)
                'If Me.Parent.FindControl(id) Is Nothing Then
                '    Me.ID = id
                'Else
                '    id = id & "_{0}"
                '    Dim i As Integer = 1
                '    Do While Not Me.Parent.FindControl(String.Format(id, i)) Is Nothing
                '        i += 1
                '    Loop
                '    Me.ID = String.Format(id, i)
                'End If

                ' select the correct Application user control
                Select Case Me.Mode

                    Case VAEnts.Mode.Lure
                        _app = CType(Me.LoadControl("Lure.ascx"), AspWeb.UI.UserControl)

                    Case VAEnts.Mode.Vote
                        _app = CType(Me.LoadControl("Vote.ascx"), AspWeb.UI.UserControl)

                    Case Else
                        _app = Nothing

                End Select


            Catch ex As System.FormatException
                _app = Nothing
            End Try

            If Not _app Is Nothing Then Me.Controls.Add(_app)
        End Sub


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

#End Region



#Region "Public Methods"

        ''' <summary>
        ''' Gets or sets the application mode of the control.
        ''' </summary>
        ''' <returns></returns>
        <OWWeb.Applications.ApplicationMode("applicationModeToolTip", "applicationMode")> _
        Public Property Mode() As VAEnts.Mode
            Get
                If ViewState("Mode") Is Nothing Then ViewState("Mode") = VAEnts.Mode.Lure
                Return CType(ViewState("Mode"), VAEnts.Mode)
            End Get
            Set(ByVal Value As VAEnts.Mode)
                ViewState("Mode") = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the results page for the click event.
        ''' </summary>
        ''' <returns></returns>
        <OWWeb.Applications.LinkedPage("resultsPageToolTip", "resultsPage")> _
        Public Property ResultsPage() As String
            Get
                If Me.ViewState("ResultsPage") Is Nothing Then Return String.Empty
                Return CStr(ViewState("ResultsPage"))
            End Get
            Set(ByVal Value As String)
                If Value.StartsWith("~~") Then
                    Dim site As OWEnts.Site = CType(Context.User, OWSys.Security.OneWebPrincipal).CurrentSite
                    Value = Value.Replace("~~", String.Concat(site.RootAddress, "/", site.Culture))
                End If
                ViewState("ResultsPage") = Value
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



        Friend Function LocalizedDate(ByVal d As Date) As String
            If d = Date.MinValue Then
                Return String.Empty
            Else
                Dim principal As OWSys.Security.OneWebPrincipal = CType(Context.User, OWSys.Security.OneWebPrincipal)
                Dim format As String
                'get the time portion to see if it is midnight
                Dim timeOfDay As String = d.TimeOfDay.ToString()

                If timeOfDay = "00:00:00" Then
                    format = VAEnts.Constants.DISPLAY_DATE_FORMAT
                    Return String.Format("{0}, {1}", "Midnight", d.ToString(format, principal.CurrentSiteCulture.GetSpecificCultureInfo()))
                Else
                    format = VAEnts.Constants.DISPLAY_DATE_AND_TIME_FORMAT
                    Return d.ToString(format, principal.CurrentSiteCulture.GetSpecificCultureInfo())
                End If

            End If
        End Function

        ''' <summary>
        ''' Gets if an admistrative user is logged on to OneWeb.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend ReadOnly Property IsAdminLoggedOn() As Boolean
            Get
                Return Me.IsLoggedOn()
            End Get
        End Property
        ''' <summary>
        ''' Gets if a extranet member is logged on.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend ReadOnly Property IsUserLoggedOn() As Boolean
            Get
                Return Me.IsMemberLoggedOn()
            End Get
        End Property

        ''' <summary>
        ''' Gets logged on member.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend ReadOnly Property GetCurrentMember() As OWEnts.Member
            Get
                Return Me.CurrentMember()
            End Get
        End Property

        ''' <summary>
        ''' Toggle the validators on and off within a control.
        ''' </summary>
        ''' <param name="parent"></param>
        ''' <param name="enabled"></param>
        Friend Sub ToggleValidators(ByVal parent As AspWeb.UI.Control, ByVal enabled As Boolean)

            For Each ctl As AspWeb.UI.Control In parent.Controls
                ' enable/disable validator
                If TypeOf ctl Is AspWebControls.BaseValidator Then CType(ctl, AspWebControls.BaseValidator).Enabled = enabled
                ' recurse
                If ctl.Controls.Count > 0 Then ToggleValidators(ctl, enabled)
            Next

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a collection of the querystring parameters for this Application.
        ''' </summary>
        ''' <value></value>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2005-10-14	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Friend ReadOnly Property QueryString() As System.Collections.Specialized.NameValueCollection
            Get
                If _queryString Is Nothing Then _queryString = New System.Collections.Specialized.NameValueCollection
                Return _queryString
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Returns the current querystring as an encoded String
        ''' </summary>
        ''' <value></value>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2005-10-14	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Friend ReadOnly Property Query() As String
            Get
                Return CreateQueryString()
            End Get
        End Property

        Friend Function HasTextParam(ByVal str As String) As Boolean
            Return Not String.IsNullOrEmpty(Me.QueryString(str))
        End Function

        Friend Function SelectParam(ByVal str As String, ByVal sel As AspWebControls.DropDownList) As Boolean
            Return Not Me.QueryString(str) Is Nothing AndAlso Len(Me.QueryString(str).Trim) > 0 AndAlso Not sel.Items.FindByValue(Me.QueryString(str).Trim) Is Nothing
        End Function


#End Region


#Region "Private Methods"

        Private Sub ParseQueryString()
            If Not Request.QueryString(QueryStringKey) Is Nothing Then
                ' catch errors in the query string perhaps by a malicious addition to the query string
                Try
                    _queryString = OWHelpers.NameValueParser.ConvertSafeBase64ToNameValue(Request.QueryString(QueryStringKey))
                Catch ex As System.FormatException
                    Throw
                End Try
            Else
                _queryString = New System.Collections.Specialized.NameValueCollection(Request.QueryString)

            End If
        End Sub

        Private Function CreateQueryString() As String
            If Not _queryString Is Nothing Then
                Return QueryStringKey & "=" & OWHelpers.NameValueParser.ConvertNameValueToSafeBase64(_queryString)
            Else
                Return QueryStringKey & "="
            End If
        End Function


        ''' <summary>
        ''' Removes diacritics (accents and modifiers) from characters using Unicode normalization methods.
        ''' </summary>
        ''' <param name="stIn"></param>
        ''' <returns></returns>
        Private Function RemoveDiacritics(ByVal stIn As String) As String
            Dim stFormD As String = stIn.Normalize(NormalizationForm.FormD)
            Dim l As Integer = stFormD.Length

            Dim sb As New StringBuilder

            For i As Integer = 0 To l - 1
                Dim uc As System.Globalization.UnicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD(i))
                If uc <> System.Globalization.UnicodeCategory.OpenPunctuation Then
                    sb.Append(stFormD(i))
                End If
            Next

            Return (sb.ToString().Normalize(NormalizationForm.FormC))

        End Function

#End Region



    End Class


End Namespace

