' Class OneWebMembershipProvider
' Documentation:

Option Strict On
Option Explicit On

Imports System.Web
Imports Crypto = System.Security.Cryptography
Imports AspSecurity = System.Web.Security
Imports NetMail = System.Net.Mail
Imports OWSys = ISL.OneWeb4.Sys
Imports OWEnts = ISL.OneWeb4.Entities
Imports OWDA = ISL.OneWeb4.DataAccess
Imports OWMember = ISL.OneWeb4.Sys.Membership
Imports OWHelpers = ISL.OneWeb4.UI.Components.Helpers

Imports NursesMembership = Nsnu.MembershipServices
Imports NursesDA = Nsnu.DataAccess


Namespace Components.Membership

    Public Class NSNUMembershipProvider
        Implements OWSys.Membership.IMembershipProvider


#Region "Private Members"

        Private _member As OWEnts.Member
        Private _principal As OWSys.Security.OneWebMembershipPrincipal

        ' configuration settings
        Private _logonTimeout As Integer = 0
        Private _preventLogonSpoofing As Boolean = True
        Private _maxLogonAttempts As Integer = 0
        Private _logonAttemptTimeout As Integer = 0
        Private _persistentLogon As Boolean = False
        Private _passwordStrengthRegex As String = "^[a-zA-Z0-9]{6,20}$" '"^((\w){6,})|(\*{8})$"
        Private _features As OWMember.MembershipProviderFeatures = CType(Features.Default, OWMember.MembershipProviderFeatures)

        ' logon domain memory
        Private _loggedOnDomains As String = String.Empty   ' a comma-delimited list of the logged-on domains that the cookie token is set for (need to log-off of these domains after)
        Private _isLoggingOut As Boolean = False
        Private _logoutPingsSent As Boolean = False
        Private _ticketExpiration As Date = DateTime.MinValue

        Private Const PersistentLogonTimeout As Integer = 90    ' 90 days before a persistent logon expires
        Private Const TokenName As String = ".NSNUMBR"
        Private Const ProviderName As String = "NSNU Membership"

#End Region

#Region "Constructors and Initializers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes the OneWebMembershipProvider.
        ''' </summary>
        ''' <param name="configSettings"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub Initialize(ByVal configSettings As System.Collections.Specialized.NameValueCollection) Implements OWSys.Membership.IMembershipProvider.Initialize
            Dim value As String
            Dim allowRegistration As Integer = 1
            Dim allowRemoval As Integer = 1

            For Each key As String In configSettings
                value = configSettings(key)
                Select Case key.ToLower
                    Case "logontimeout"
                        If IsNumeric(value) Then _logonTimeout = CInt(value)
                    Case "preventlogonspoofing"
                        Try
                            _preventLogonSpoofing = Boolean.Parse(value)
                        Catch ex As Exception
                            ' invalid boolean value
                            _preventLogonSpoofing = True
                        End Try
                    Case "maxlogonattempts"
                        If IsNumeric(value) Then _maxLogonAttempts = CInt(value)
                    Case "logonattempttimeout"
                        If IsNumeric(value) Then _logonAttemptTimeout = CInt(value)
                    Case "persistentlogon"
                        Try
                            _persistentLogon = Boolean.Parse(value)
                        Catch ex As Exception
                            ' invalid boolean value
                            _persistentLogon = False
                        End Try
                    Case "passwordstrengthregex"
                        _passwordStrengthRegex = value
                    Case "allowregistration"
                        Try
                            allowRegistration = CInt(Boolean.Parse(value))
                        Catch ex As Exception
                            ' invalid boolean value
                            allowRegistration = 0
                        End Try
                    Case "allowremoval"
                        Try
                            allowRemoval = CInt(Boolean.Parse(value))
                        Catch ex As Exception
                            ' invalid boolean value
                            allowRemoval = 0
                        End Try
                    Case "enabledfeatures", "features"
                        Try
                            _features = CType(System.Enum.Parse(GetType(Features), value), OWMember.MembershipProviderFeatures)
                        Catch ex As Exception
                            ' invalid boolean value
                            _features = CType(Features.Default, OWMember.MembershipProviderFeatures)
                        End Try
                End Select
            Next

            ' allow individual configuration settings to override the main features attribute
            If allowRegistration = 0 Then _features = _features And Not OWMember.MembershipProviderFeatures.SelfRegistration
            If allowRegistration = -1 Then _features = _features Or OWMember.MembershipProviderFeatures.SelfRegistration
            If allowRemoval = 0 Then _features = _features And Not OWMember.MembershipProviderFeatures.SelfRemoval
            If allowRemoval = -1 Then _features = _features Or OWMember.MembershipProviderFeatures.SelfRemoval

        End Sub

#End Region

#Region "Public Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Returns the name of the OneWeb Membership Provider.
        ''' </summary>
        ''' <value></value>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property Name() As String Implements OWSys.Membership.IMembershipProvider.Name
            Get
                Return ProviderName
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Returns an enumerated value with the supported features of this MembershipProvider.
        ''' </summary>
        ''' <value></value>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property SupportedFeatures() As OWSys.Membership.MembershipProviderFeatures Implements OWSys.Membership.IMembershipProvider.SupportedFeatures
            Get
                Return _features
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Returns the currently logged-in OneWeb Member.
        ''' </summary>
        ''' <value></value>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property CurrentMember() As OWEnts.Member Implements OWSys.Membership.IMembershipProvider.CurrentMember
            Get
                Return _member
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Returns the current OneWebMembershipPrincipal object.
        ''' </summary>
        ''' <value></value>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property CurrentPrincipal() As System.Security.Principal.IPrincipal Implements OWSys.Membership.IMembershipProvider.CurrentPrincipal
            Get
                Return _principal
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Returns if the current Member is authenticated.
        ''' </summary>
        ''' <value></value>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property IsAuthenticated() As Boolean Implements OWSys.Membership.IMembershipProvider.IsAuthenticated
            Get
                Return Not CBool(_principal Is Nothing) AndAlso Not CBool(_member Is Nothing) AndAlso Not _isLoggingOut
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Returns if the current Member is logging out.
        ''' </summary>
        ''' <value></value>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property IsLoggingOut() As Boolean
            Get
                Return _isLoggingOut
            End Get
        End Property

#End Region

#Region "Public Methods"

#Region "Authentication Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Checks the online web cookie to determine if a Member is logged in.
        ''' </summary>
        ''' <returns>Boolean</returns>
        ''' <remarks>
        ''' This method checks the http cookies collection looking for a membership token cookie, and
        ''' determines if the Member is valid or not.
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function CheckMemberToken() As Boolean Implements OWSys.Membership.IMembershipProvider.CheckMemberToken

            ' determine if the request has a membership logon token
            Dim ctx As HttpContext = HttpContext.Current

            If ctx.Request.QueryString(TokenName) IsNot Nothing Then
                ' preferentially get the querystring value
                Dim token As String = ctx.Request.QueryString(TokenName)
                Me.ParseAuthString(token)

                ' check to see if we should remove the auth token in the querystring
                If ctx.Request.RequestType = "GET" AndAlso ctx.Request.QueryString("n") Is Nothing Then
                    Dim address As String = ctx.Request.Path & "?" & OWHelpers.NameValueParser.RemoveNameValue(ctx.Request.QueryString.ToString(), TokenName, token)
                    If address.EndsWith("?"c) Then address = address.TrimEnd("?"c)
                    ctx.Response.Redirect(address, False)
                End If

            ElseIf ctx.Request.Cookies.Item(TokenName) IsNot Nothing Then
                ' get the cookie and validate the value
                Dim token As String = ctx.Request.Cookies(TokenName).Value
                Me.ParseAuthString(token)

                ' if we don't authentic - remove the cookie
                If Not Me.IsAuthenticated Then
                    Dim authCookie As HttpCookie = ctx.Request.Cookies(TokenName)
                    authCookie.Expires = System.DateTime.Now.AddDays(-1)
                    ctx.Response.Cookies.Add(authCookie)
                End If

                ' check if we're logging out and should add a ping request
                If Me.IsLoggingOut Then AddPingRequests()

            Else
                ' neither a cookie nor a querystring value
                Return False
            End If

            Return Me.IsAuthenticated Or Me.IsLoggingOut

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Sets the OneWeb membership token as an http cookie into the outgoing response.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub SetMemberToken() Implements OWSys.Membership.IMembershipProvider.SetMemberToken

            ' determine if we have a logged-on Member, and set/clear the token appropriately

            ' Extract the membership cookie
            Dim ctx As HttpContext = HttpContext.Current
            Dim req As HttpRequest = ctx.Request
            Dim rep As HttpResponse = ctx.Response
            Dim cookieName As String = TokenName
            Dim cookieExists As Boolean = CBool(ctx.Request.Cookies(cookieName) IsNot Nothing)
            Dim qsExists As Boolean
            Dim authCookie As HttpCookie

            Try
                ' add the current user cookie (if they're an authenticated OneWeb member, or they are logging out and also being redirected (to maintain the logout state))
                If Me.IsAuthenticated OrElse (Me.IsLoggingOut AndAlso rep.IsRequestBeingRedirected) Then
                    Dim authToken As String = Me.CreateAuthString()

                    ' if we're redirecting to a different domain, need to pass the cookie as part of the querystring
                    If rep.IsRequestBeingRedirected Then
                        Dim location As String = rep.RedirectLocation
                        If location.StartsWith(Uri.UriSchemeHttp & Uri.SchemeDelimiter) Or location.StartsWith(Uri.UriSchemeHttps & Uri.SchemeDelimiter) Then
                            Dim url As New System.Uri(location)
                            qsExists = CBool(url.Query.IndexOf(cookieName & "=") > -1)
                            If url.Host <> ctx.Request.Url.Host AndAlso Not qsExists Then
                                ' create the authentication string
                                rep.RedirectLocation &= If(url.Query.Length > 1, "&", "?") & cookieName & "=" & authToken
                                '								Return
                            End If
                        End If
                    End If

                    ' create a new cookie from the current data
                    authCookie = New HttpCookie(cookieName, authToken)
                    If authCookie IsNot Nothing Then
                        If _persistentLogon Then
                            authCookie.Expires = System.DateTime.Now.AddDays(PersistentLogonTimeout)
                        End If
                    ElseIf cookieExists Then
                        ' couldn't get a new cookie - expire the old one that exists
                        authCookie = req.Cookies(cookieName)
                        authCookie.Value = authToken
                        authCookie.Expires = System.DateTime.Now.AddDays(-1)
                    End If
                    If rep.Cookies(cookieName) IsNot Nothing Then rep.Cookies.Remove(cookieName)
                    rep.Cookies.Add(authCookie)
                Else

                    ' check if we're logging out but not being redirected
                    If Me.IsLoggingOut AndAlso Not Me._logoutPingsSent Then
                        Me.AddPingRequests()
                        'Dim authToken As String = Me.CreateAuthString()
                        'If _loggedOnDomains.Length > 0 Then
                        '	' still logged on on another site; set the auth cookie to not expire so the ping requests will logoff on the next load

                        '	' create a new cookie from the current data
                        '	authCookie = New HttpCookie(cookieName, authToken)
                        '	If rep.Cookies(cookieName) IsNot Nothing Then rep.Cookies.Remove(cookieName)
                        '	rep.Cookies.Add(authCookie)
                        '	Return
                        'End If
                    End If

                    ' check if the cookie exists even though we're not authenticated
                    If cookieExists Then
                        ' expire the old one that exists
                        authCookie = req.Cookies(cookieName)
                        authCookie.Expires = System.DateTime.Now.AddDays(-1)
                        If rep.Cookies(cookieName) IsNot Nothing Then rep.Cookies.Remove(cookieName)
                        rep.Cookies.Add(authCookie)
                    End If

                End If

            Catch ex As Exception
                Return
            End Try

        End Sub


#End Region

#Region "Membership Methods"


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Logs the current member on via a logon/password.
        ''' </summary>
        ''' <param name="logonName"></param>
        ''' <param name="password"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Logon(ByVal logonName As String, ByVal password As String) As OWSys.Result Implements OWSys.Membership.IMembershipProvider.Logon
            Dim member As OWEnts.Member

            If logonName Is Nothing OrElse logonName.Length = 0 Then
                Throw New ArgumentNullException("logonName")
            End If

            ' determine if the user is using a logon name or email
            If logonName.IndexOf("@"c) > 0 Then
                member = OWDA.MemberDA.CreateMember()
                member.Email = logonName
            Else
                member = New OWEnts.Member(logonName)
            End If

            Dim nmbs As NursesMembership.MembershipBs = New NursesMembership.MembershipBs
            Dim profile As NursesDA.UserProfile = nmbs.GetValidNurseProfile(member.MemberName, password)

            If profile IsNot Nothing Then

                ' valid in Nurses datastore, no point setting this, loading the OW member will not preserve the password on the memebr object
                ' since it's not stored in the OW Member table
                'member.Password = password

                'user is authenticated against the web service try to get the member
                If OWDA.MemberDA.LoadMember(member) Then
                    ' check that the member is not locked up
                    If member.Locked Then Return OWSys.Result.Failed

                    ' check that they haven't exceeded their logon attempts
                    If member.LogonAttempts > Me._maxLogonAttempts AndAlso _
                     System.DateTime.Now.Subtract(member.LastAttemptTimestamp).Minutes < Me._logonAttemptTimeout Then
                        Return OWSys.Result.Failed
                    End If

                    'always resync email address on successful login in case admin changes members email address in Nurses system
                    member.Email = profile.email


                    _member = member
                    OWDA.MemberDA.SaveMember(_member)


                    Dim id As New OWSys.Security.OneWebMembershipIdentity(_member)
                    _principal = New OWSys.Security.OneWebMembershipPrincipal(id)

                    Return OWSys.Result.Succeeded


                Else


                    'unable to load member due to first time logon.
                    'Only Logon the member this time. OW will create the member.
                    member.MemberName = logonName 'logonName (logon parameter to this method)
                    member.LogonName = logonName 'profile.id.ToString 'logonName
                    member.FirstName = profile.Nurse.firstName 'logonName
                    member.LastName = profile.Nurse.lastName 'logonName
                    member.AuthenticationProvider = ProviderName
                    member.Locked = False
                    member.Email = profile.email 'syncs the email adress

                    _member = member


                    'write a record to the table to join the profile id of the memeber in the management system with the OW member username
                    DataAccess.DBAccess.AddMemberProfile(profile.id, member.MemberName)

                    Dim id As New OWSys.Security.OneWebMembershipIdentity(_member)
                    _principal = New OWSys.Security.OneWebMembershipPrincipal(id)

                    Return OWSys.Result.Succeeded("FIRST_TIME_LOGON")
                    'Return OWSys.Result.ReturnCode(99)

                End If

            Else

                Return OWSys.Result.Failed
            End If


        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Logs the current member off.
        ''' </summary>
        ''' <returns>Boolean</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Logoff() As OWSys.Result Implements OWSys.Membership.IMembershipProvider.Logoff
            _member = Nothing
            _principal = Nothing
            _isLoggingOut = True

            Return OWSys.Result.Succeeded
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Allows a Member to change their password.
        ''' </summary>
        ''' <param name="logonName">The logon name for the Member.</param>
        ''' <param name="originalPassword">The original password for the Member (allowing confirmation of the Member).</param>
        ''' <param name="newPassword">The new password for the Member.</param>
        ''' <returns>Boolean</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function ChangePassword(ByVal logonName As String, ByVal originalPassword As String, ByVal newPassword As String) As OWSys.Result Implements OWSys.Membership.IMembershipProvider.ChangePassword

            ' get the current culture
            Dim culture As String = DirectCast(HttpContext.Current.User, OWSys.Security.OneWebPrincipal).CurrentSite.Culture
            Dim member As OWEnts.Member

            ' ensure we're authenticated first (only if we're not setting through the simple forgottent password routine)
            If Not Me.IsAuthenticated AndAlso Not (Me.SupportedFeatures And OWSys.Membership.MembershipProviderFeatures.SimpleForgottenPassword) > 0 Then
                Return OWSys.Result.Failed
            ElseIf Not Me.IsAuthenticated Then
                ' load the member in the simple forgotten password case where the member hasn't logged in yet, but is verified through a email exchange
                member = New OWEnts.Member(logonName)
                OWDA.MemberDA.LoadMember(member)

                ' check that the member exists and is not locked up
                If Not member.IsLoaded OrElse member.Locked Then Return OWSys.Result.Failed

            Else
                member = _member
            End If

            ' change password not allowed
            If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.ChangePassword) = 0 Then Return OWSys.Result.Failed

            Dim nmbs As NursesMembership.MembershipBs = New NursesMembership.MembershipBs
            Dim aes As NursesMembership.SimpleAES = New NursesMembership.SimpleAES
            Dim profile As NursesDA.UserProfile = nmbs.GetUserProfile(member.MemberName)


            Dim rslt As OWSys.Result
            If (member.LogonName = logonName Or member.MemberName = logonName) AndAlso profile IsNot Nothing AndAlso _
             ((originalPassword Is Nothing AndAlso (Me.SupportedFeatures And OWSys.Membership.MembershipProviderFeatures.SimpleForgottenPassword) > 0) OrElse String.Compare(originalPassword, aes.Decrypt(profile.password)) = 0) Then
                ' check password strength
                If System.Text.RegularExpressions.Regex.IsMatch(newPassword, _passwordStrengthRegex) Then

                    Dim nmp As NursesMembership.NsnuMembershipProvider = New NursesMembership.NsnuMembershipProvider
                    rslt = New OWSys.Result(nmp.ChangePassword(logonName, String.Empty, newPassword))

                    If rslt.Value Then

                        ' log the member in now
                        OWDA.MemberDA.ResetLogonAttempts(member)

                        ' set and create the authenticated objects
                        _member = member
                        Dim id As New OWSys.Security.OneWebMembershipIdentity(_member)
                        _principal = New OWSys.Security.OneWebMembershipPrincipal(id)

                    End If
                Else
                    rslt = New OWSys.Result(False, OWHelpers.ResourceAccessor.GetText("invalidPassword", Me.GetType, culture))
                End If
            Else
                rslt = OWSys.Result.Failed
            End If

            Return rslt
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Implements the forgotten password functionality using the provided logon name.
        ''' </summary>
        ''' <param name="logonName">The logon name of the Member</param>
        ''' <param name="emailAddress">The confirmation email address.</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function ForgotPassword(ByVal logonName As String, ByVal emailAddress As String) As OWSys.Result Implements OWSys.Membership.IMembershipProvider.ForgotPassword

            ' new change password implementation manages this feature now
            Return OWSys.Result.Failed

            '' implement the forgotten password functionality

            '' forgotten name/email not allowed
            'If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.ForgottenPassword) = 0 Then Return OWSys.Result.Failed

            '' determine if the user is using a logon name or email
            'Dim member As OWEnts.Member = OWDA.MemberDA.CreateMember()
            'member.MemberName = logonName
            'If System.Text.RegularExpressions.Regex.IsMatch(logonName, "^([a-zA-Z0-9_\.\-\+])+@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,6})$") Then
            '    member.Email = logonName
            '    member.MemberName = String.Empty
            'Else
            '    member.MemberName = logonName
            '    member.Email = String.Empty
            'End If

            '' get the current culture
            'Dim culture As String = DirectCast(HttpContext.Current.User, OWSys.Security.OneWebPrincipal).CurrentSite.Culture

            'If OWDA.MemberDA.LoadMember(member) AndAlso member.Email = emailAddress Then

            '    If Not String.IsNullOrWhiteSpace(member.AuthenticationProvider) AndAlso member.AuthenticationProvider <> ProviderName Then
            '        ' this authentication is not managed by OneWeb
            '        Return OWSys.Result.Failed
            '    End If

            '    'get new pwd
            '    Dim newPwd As String = OWSys.Security.Security.GenerateNewPassword()

            '    'set up the email message
            '    Dim template As New ISL.OneWeb4.UI.Components.Utility.EmailTemplate(OWHelpers.ResourceAccessor.GetText("forgotPwdEmailTemplate", Me.GetType, culture, True))
            '    Dim params As New Collections.Specialized.ListDictionary() From {{"FirstName", member.FirstName}, {"Password", newPwd}}
            '    Dim sent As Boolean = False

            '    'Dim email As New NetMail.MailMessage	' updated to use email template instead
            '    'With email
            '    '	.To.Add(New NetMail.MailAddress(member.Email, member.FullName))
            '    '	.Subject = OWHelpers.ResourceAccessor.GetText("forgotPwdEmailSubject", Me.GetType, culture)
            '    '	.IsBodyHtml = False
            '    '	.Body = String.Format(OWHelpers.ResourceAccessor.GetText("forgotPwdEmailBody", Me.GetType, culture), _
            '    '	  member.FirstName, newPwd, vbCrLf)
            '    'End With

            '    'update member profile with hashed version of the password
            '    member.Password = OWSys.Security.Security.HashPassword(newPwd)
            '    If OWDA.MemberDA.ChangePassword(member) Then

            '        Try
            '            'send the email
            '            sent = OWHelpers.SendEmail.SendFromTemplate(template, member.FullNameForEmail, Nothing, params)
            '            'OWHelpers.SendEmail.SendMessage(email)
            '        Catch ex As Exception
            '            ' sending of email failed - notify sysadmin through exception publishing
            '            OWHelpers.ExceptionPublisher.Publish(New OWSys.ConfigurationException("SMTP is not configured correctly for OneWeb Membership Features", ex))
            '            Return OWSys.Result.Failed(OWHelpers.ResourceAccessor.GetText("forgotPwdFailed", Me.GetType, culture))
            '        End Try

            '        Return OWSys.Result.Succeeded(OWHelpers.ResourceAccessor.GetText("forgotPwdEmailSent", Me.GetType, culture))
            '    End If

            'End If
            'Return OWSys.Result.Failed(OWHelpers.ResourceAccessor.GetText("forgotPwdFailed", Me.GetType, culture))

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Provides functionality for a member that has lost the email or member name values.
        ''' </summary>
        ''' <param name="nameOrEmail">Name or email address to utilize.</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-09-15	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function ForgotNameOrEmail(ByVal nameOrEmail As String) As OWSys.Result Implements OWSys.Membership.IMembershipProvider.ForgotNameOrEmail

            '' implement the forgotten name/email functionality

            '' forgotten name/email not allowed
            'If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.ForgottenNameOrEmail) = 0 Then Return OWSys.Result.Failed

            '' determine if the user is using a logon name or email
            'Dim member As OWEnts.Member = OWDA.MemberDA.CreateMember()
            'If System.Text.RegularExpressions.Regex.IsMatch(nameOrEmail, "^([a-zA-Z0-9_\.\-\+])+@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,6})$") Then
            '    member.Email = nameOrEmail
            '    member.MemberName = String.Empty
            'Else
            '    member.MemberName = nameOrEmail
            '    member.Email = String.Empty
            'End If

            '' get the current culture
            'Dim culture As String = DirectCast(HttpContext.Current.User, OWSys.Security.OneWebPrincipal).CurrentSite.Culture

            'If OWDA.MemberDA.LoadMember(member) Then

            '    If member.Email.Length = 0 Then Return OWSys.Result.Failed(OWHelpers.ResourceAccessor.GetText("forgotLogonFailed", Me.GetType, culture))

            '    'set up the email message
            '    Dim template As New ISL.OneWeb4.UI.Components.Utility.EmailTemplate(OWHelpers.ResourceAccessor.GetText("forgotLogonEmailTemplate", Me.GetType, culture, True))
            '    Dim params As New Collections.Specialized.ListDictionary() From {{"FirstName", member.FirstName}, {"UserName", member.LogonName}}
            '    Dim sent As Boolean = False

            '    ''set up the email message	' updated to use email template instead
            '    'Dim email As New NetMail.MailMessage
            '    'With email
            '    '	.To.Add(New NetMail.MailAddress(member.Email, member.FullName))
            '    '	.Subject = OWHelpers.ResourceAccessor.GetText("forgotPwdEmailSubject", Me.GetType, culture)
            '    '	.IsBodyHtml = False
            '    '	.Body = String.Format(OWHelpers.ResourceAccessor.GetText("forgotLogonEmailBody", Me.GetType, culture), _
            '    '	 member.FirstName, member.LogonName, vbCrLf)
            '    'End With

            '    Try
            '        'send the email
            '        sent = OWHelpers.SendEmail.SendFromTemplate(template, member.FullNameForEmail, Nothing, params)
            '        'OWHelpers.SendEmail.SendMessage(email)
            '    Catch ex As Exception
            '        ' sending of email failed - notify sysadmin through exception publishing
            '        OWHelpers.ExceptionPublisher.Publish(New OWSys.ConfigurationException("SMTP is not configured correctly for OneWeb Membership Features", ex))
            '        Return OWSys.Result.Failed(OWHelpers.ResourceAccessor.GetText("forgotLogonFailed", Me.GetType, culture))
            '    End Try

            '    Return OWSys.Result.Succeeded(OWHelpers.ResourceAccessor.GetText("forgotLogonEmailSent", Me.GetType, culture))

            'End If
            'Return OWSys.Result.Failed(OWHelpers.ResourceAccessor.GetText("forgotLogonFailed", Me.GetType, culture))
            Return OWSys.Result.Failed("Not Supported by the " & Me.Name)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Allows a Member to self-register.
        ''' </summary>
        ''' <param name="memberName">The unique member name</param>
        ''' <param name="password">The member's password</param>
        ''' <param name="firstName">The member's first name</param>
        ''' <param name="lastName">The member's last name</param>
        ''' <param name="title">The member's title</param>
        ''' <param name="email">The member's email address</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Register(ByVal memberName As String, ByVal password As String, ByVal firstName As String, ByVal lastName As String, ByVal title As String, ByVal email As String) As OWSys.Result Implements OWSys.Membership.IMembershipProvider.Register

            '' get the current culture
            'Dim culture As String = DirectCast(HttpContext.Current.User, OWSys.Security.OneWebPrincipal).CurrentSite.Culture

            '' self-registration not allowed
            'If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.SelfRegistration) = 0 Then Return OWSys.Result.Failed

            'Try
            '    Dim member As OWEnts.Member = OWDA.MemberDA.CreateMember()
            '    member.MemberName = memberName
            '    member.LogonName = memberName
            '    member.Password = OWSys.Security.Security.HashPassword(password)
            '    If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.UpdateFirstName) > 0 Then member.FirstName = firstName
            '    If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.UpdateLastName) > 0 Then member.LastName = lastName
            '    If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.UpdateTitle) > 0 Then member.Title = title
            '    If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.UpdateEmail) > 0 Then member.Email = email
            '    member.AuthenticationProvider = Me.Name

            '    Try

            '        If OWDA.MemberDA.AddMember(member) Then
            '            _member = member
            '            Dim id As New OWSys.Security.OneWebMembershipIdentity(_member)
            '            _principal = New OWSys.Security.OneWebMembershipPrincipal(id)

            '            ' include the password into the database
            '            OWDA.MemberDA.ChangePassword(member)

            '            ' return success
            '            Return OWSys.Result.Succeeded

            '        End If
            '    Catch ex As OWDA.Exceptions.DataUpdateException
            '        ' member name already exists
            '        Return New OWSys.Result(False, OWHelpers.ResourceAccessor.GetText("conflictingMemberName", Me.GetType, culture))
            '    End Try
            '    Return OWSys.Result.Failed

            'Catch ex As Exception
            '    ' an exception occurred
            '    OWHelpers.ExceptionPublisher.Publish(ex)
            '    Return OWSys.Result.Failed
            'End Try
            Return OWSys.Result.Failed("Not Supported by the " & Me.Name)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Allows a member to self-remove.
        ''' </summary>
        ''' <param name="memberName">The unique name of the Member.</param>
        ''' <returns>Boolean success value</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Remove(ByVal memberName As String, ByVal password As String) As OWSys.Result Implements OWSys.Membership.IMembershipProvider.Remove

            '' get the current culture
            'Dim culture As String = DirectCast(HttpContext.Current.User, OWSys.Security.OneWebPrincipal).CurrentSite.Culture

            '' self-removal not allowed
            'If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.SelfRemoval) = 0 Then Return OWSys.Result.Failed

            '' ensure we're authenticated first
            'If Not Me.IsAuthenticated Then Return OWSys.Result.Failed

            'If _member.MemberName = memberName OrElse _member.LogonName = memberName Then
            '    If OWSys.Security.Security.ComparePasswordToHash(password, _member.Password) Then
            '        Try
            '            If OWDA.MemberDA.DeleteMember(_member) Then

            '                ' remove this member from the current context
            '                _member = Nothing
            '                _principal = Nothing

            '                Return OWSys.Result.Succeeded
            '            End If
            '        Catch ex As Exception
            '            ' delete did not work
            '            OWHelpers.ExceptionPublisher.Publish(ex)
            '            Return OWSys.Result.Failed
            '        End Try
            '    Else
            '        Return New OWSys.Result(False, OWHelpers.ResourceAccessor.GetText("invalidPassword", Me.GetType, culture))
            '    End If
            'End If

            '' wrong member
            'Return OWSys.Result.Failed

            Return OWSys.Result.Failed("Not Supported by the " & Me.Name)

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Allows a Member to update their details.
        ''' </summary>
        ''' <param name="firstName"></param>
        ''' <param name="lastName"></param>
        ''' <param name="title"></param>
        ''' <param name="email"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function UpdateDetails(ByVal firstName As String, ByVal lastName As String, ByVal title As String, ByVal email As String) As OWSys.Result Implements OWSys.Membership.IMembershipProvider.UpdateDetails

            ' ensure we're authenticated first
            If Not Me.IsAuthenticated Then Return OWSys.Result.Failed

            Try
                If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.UpdateFirstName) > 0 Then _member.FirstName = firstName
                If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.UpdateLastName) > 0 Then _member.LastName = lastName
                If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.UpdateTitle) > 0 Then _member.Title = title
                If (Me.SupportedFeatures And OWMember.MembershipProviderFeatures.UpdateEmail) > 0 Then
                    _member.Email = email

                    ' sync the email address with Nurse membership system here
                    Dim nmbs As NursesMembership.MembershipBs = New NursesMembership.MembershipBs

                    Dim profile As NursesDA.UserProfile = nmbs.GetUserProfile(_member.MemberName)

                    If profile IsNot Nothing Then
                        profile.email = email
                        nmbs.ProcessNurseProfile(profile)
                    End If

                End If

                If OWDA.MemberDA.SaveMember(_member) Then
                    Return OWSys.Result.Succeeded
                End If
            Catch ex As Exception
                ' update did not work
                OWHelpers.ExceptionPublisher.Publish(ex)
                Return OWSys.Result.Failed
            End Try

            ' update did not work
            Return OWSys.Result.Failed

        End Function

#End Region

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Creates an authentication string from the current logged-on member principal.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function CreateAuthString() As String

            ' get required info from the principal

            ' check that the current domain is in the logged-on domains
            Dim domain As String = HttpContext.Current.Request.Url.Host
            Dim rx As New System.Text.RegularExpressions.Regex("(^|,)" & domain & "(,|$)", System.Text.RegularExpressions.RegexOptions.CultureInvariant Or System.Text.RegularExpressions.RegexOptions.IgnoreCase Or System.Text.RegularExpressions.RegexOptions.Compiled)
            If Not Me.IsLoggingOut AndAlso Not rx.IsMatch(_loggedOnDomains) Then
                _loggedOnDomains = If(String.IsNullOrEmpty(_loggedOnDomains), domain, domain & "," & _loggedOnDomains)
            ElseIf Me.IsLoggingOut AndAlso rx.IsMatch(_loggedOnDomains) Then
                _loggedOnDomains = rx.Replace(_loggedOnDomains, ",").Trim(","c)
            End If

            ' create the userData string for the authentication ticket
            Dim userData As String
            If _member IsNot Nothing Then
                userData = String.Format("{0}|{1}|{2}|{3}", _member.MemberId, HttpContext.Current.Request.UserHostAddress, _loggedOnDomains, CInt(_isLoggingOut))
            Else
                userData = String.Format("{0}|{1}|{2}|{3}", 0, HttpContext.Current.Request.UserHostAddress, _loggedOnDomains, CInt(_isLoggingOut))
            End If

            Dim expireDate As Date
            If Me.IsLoggingOut Then
                ' maintain the same expiration if the user is logging out (prevent the sliding window)
                expireDate = System.DateTime.Now.AddDays(-1)
            ElseIf _logonTimeout > 0 Then
                ' slide the logon window forward based on the current time
                expireDate = System.DateTime.Now.AddMinutes(_logonTimeout)
            Else
                ' set the expirate to default 1 year
                expireDate = System.DateTime.Now.AddYears(1)
            End If

            ' create the authentication ticket
            Dim ticket As New AspSecurity.FormsAuthenticationTicket( _
             1, _
             If(_principal IsNot Nothing, _principal.Identity.Name, String.Empty), _
             System.DateTime.Now, _
             expireDate, _
             _persistentLogon, _
             userData, _
             AspSecurity.FormsAuthentication.FormsCookiePath)

            ' Encrypt the ticket and store in a cookie
            Dim authstring As String = AspSecurity.FormsAuthentication.Encrypt(ticket)
            Try
                ' try to decode the ticket as a hex string, to re-encode it in a format that is a bit more efficient
                Dim bytes As Byte() = OWSys.Encode.FromHexString(authstring)
                If bytes.Length > 0 Then
                    authstring = OWSys.Encode.ToBase64String(bytes, OWSys.Encode.Base64FormattingOptions.None, OWSys.Encode.Base64CharacterSet.Safe)
                End If
            Catch ex As Exception
                ' continue to use auth string
            End Try

            Return authstring
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Parses the input string to determine if the token is valid.
        ''' </summary>
        ''' <param name="token"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-08-28	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub ParseAuthString(ByVal token As String)
            Dim ticket As AspSecurity.FormsAuthenticationTicket = Nothing

            Try
                ' convert from the more-efficient Ascii85 format
                Dim bytes As Byte() = OWSys.Encode.FromBase64String(token, OWSys.Encode.Base64CharacterSet.Safe)
                token = OWSys.Encode.ToHexString(bytes)
            Catch ex As Exception
                ' attempt to use the ticket as-is
            End Try

            Try
                ' get the decrypted cookie
                ticket = AspSecurity.FormsAuthentication.Decrypt(token)
                If ticket Is Nothing Then Return
            Catch ex As ArgumentException
                ' catch an invalid ticket value - user not logged in
                Return
            End Try

            ' break up the info
            Dim userData() As String = ticket.UserData.Split("|"c)

            ' perform the token security checks
            _ticketExpiration = ticket.Expiration
            If Me._logonTimeout > 0 AndAlso ticket.Expired Then
                Return
            End If

            If Me._preventLogonSpoofing AndAlso userData(1).CompareTo(HttpContext.Current.Request.UserHostAddress) <> 0 Then
                Return
            End If

            ' get the currently logged on domains
            _loggedOnDomains = If(userData.Length > 2, userData(2), String.Empty)

            ' check if we're logging out
            If userData.Length > 3 Then
                _isLoggingOut = CBool(userData(3))
                If _isLoggingOut Then Return
            End If

            ' get the member id
            Dim id As System.Security.Principal.IIdentity
            Dim memberId As Integer = CInt(userData(0))
            If memberId = 0 Then
                Return
            Else
                ' OneWeb Member
                Dim member As New OWEnts.Member(memberId)

                If OWDA.MemberDA.LoadMember(member) Then
                    ' set the internal Member value
                    _member = member
                Else
                    ' member not existent
                    Return
                End If

                ' create the identity object
                id = New OWSys.Security.OneWebMembershipIdentity(_member)
            End If

            ' create the membership principal object
            _principal = New OWSys.Security.OneWebMembershipPrincipal(id)

        End Sub

        ''' <summary>
        ''' Adds any logout ping requests to the current response.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub AddPingRequests()
            Dim ctx As HttpContext = HttpContext.Current
            If Me.IsLoggingOut AndAlso TypeOf ctx.ApplicationInstance Is ISL.OneWeb4.UI.BaseHttpApplication Then
                Dim authToken As String = Me.CreateAuthString()
                Dim currDomain As String = HttpContext.Current.Request.Url.Host
                For Each domain As String In _loggedOnDomains.Split(","c)
                    If String.Compare(domain, currDomain, StringComparison.InvariantCultureIgnoreCase) <> 0 AndAlso Not String.IsNullOrEmpty(domain) Then
                        DirectCast(ctx.ApplicationInstance, ISL.OneWeb4.UI.BaseHttpApplication).AddPingRequest(String.Empty, domain, TokenName & "=" & authToken & "&n=")
                    End If
                Next
                _logoutPingsSent = True
            End If
        End Sub

        ''' <summary>
        ''' Logs the authorization cookie for debugging purposes
        ''' </summary>
        Protected Sub LogCookie()

            Dim ctx As HttpContext = HttpContext.Current

            ' Extract the forms authentication cookie
            Dim cookieName As String = TokenName
            Dim authCookie As HttpCookie = ctx.Request.Cookies(cookieName)
            Dim authQS As String = ctx.Request.QueryString(cookieName)

            Dim logCookie As HttpCookie = ctx.Request.Cookies("LMC")
            If Not logCookie Is Nothing Or OWHelpers.NameValueParser.CheckForNameValue(ctx.Request.QueryString.ToString, Nothing, "logmbrcookies") Then
                If logCookie Is Nothing Then logCookie = New HttpCookie("LMC", CStr(True))
                Dim logger As System.IO.StreamWriter
                Try
                    logger = New System.IO.StreamWriter(ctx.Server.MapPath("~/mbrcookie.log"), True)
                    If Not authQS Is Nothing Then
                        logger.WriteLine(String.Format("{1}{0}{2:s}{0}{3}{0}{4}", vbTab, ctx.Request.RawUrl, Date.Now, ctx.Request.UrlReferrer.ToString, System.Web.Security.FormsAuthentication.Decrypt(authQS).UserData))
                    ElseIf Not authCookie Is Nothing Then
                        logger.WriteLine(String.Format("{1}{0}{2:s}{0}{3}{0}{4}", vbTab, ctx.Request.RawUrl, Date.Now, ctx.Request.UrlReferrer.ToString, System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value).UserData))
                    Else
                        logger.WriteLine(String.Format("{1}{0}{2:s}{0}{3}{0}{4}", vbTab, ctx.Request.RawUrl, Date.Now, ctx.Request.UrlReferrer.ToString, String.Empty))
                    End If

                Catch ex As Exception
                Finally
                    If Not logger Is Nothing Then logger.Close()
                End Try
                If ctx.Request.Cookies("LC") Is Nothing Then ctx.Response.Cookies.Add(logCookie)
            End If

        End Sub

#End Region

#Region "Internal Features Enum"
        Public Enum Features
            ' Individual features
            SelfRegistration = 1
            SelfRemoval = 2
            ForgottenPassword = 4
            ForgottenNameOrEmail = 8
            ChangePassword = 16
            UpdateFirstName = 32
            UpdateLastName = 64
            UpdateTitle = 128
            UpdateEmail = 256
            SimpleForgottenPassword = 512
            ' combination features
            ''' <summary>
            ''' Manage self registration features
            ''' </summary>
            ''' <remarks></remarks>
            ManageRegistration = SelfRegistration Or SelfRemoval
            ''' <summary>
            ''' Manage password features
            ''' </summary>
            ''' <remarks></remarks>
            ManagePassword = ForgottenPassword Or ChangePassword Or SimpleForgottenPassword
            ''' <summary>
            ''' A combination of all the update properties values
            ''' </summary>
            ''' <remarks></remarks>
            UpdateProperties = UpdateFirstName Or UpdateLastName Or UpdateTitle Or UpdateEmail
            ''' <summary>
            ''' Default features
            ''' </summary>
            ''' <remarks></remarks>
            [Default] = ManagePassword Or ForgottenNameOrEmail Or UpdateProperties
            ''' <summary>
            ''' Everything
            ''' </summary>
            ''' <remarks></remarks>
            All = ManageRegistration Or ManagePassword Or ForgottenNameOrEmail Or UpdateProperties

        End Enum
#End Region

    End Class

End Namespace

' Copyright 2006-2013 Internet Solutions Ltd.