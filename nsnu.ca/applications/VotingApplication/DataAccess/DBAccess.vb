' Class DBAccess
' Documentation:

Option Strict On
Option Explicit On

Imports AspWeb = System.Web
Imports System.Data.SqlClient
Imports MSData = Microsoft.ApplicationBlocks.Data
Imports OWEnts = ISL.OneWeb4.Entities
Imports OWSys = ISL.OneWeb4.Sys
Imports OWWeb = ISL.OneWeb4.UI
Imports OWHelpers = ISL.OneWeb4.UI.Components.Helpers
Imports VoteEnts = ISL.OneWeb.ClientApplications.NSNU.VotingApplication.Entities

Namespace DataAccess

    ''' <summary>
    ''' A DataAcess class to handle all the database calls.
    ''' </summary>
    Public Class DBAccess

        Private Shared _dbConnection As String = OWWeb.Applications.BaseApplication.ConnectionStrings.Application

        Shared Sub New()
        End Sub

        ''' <summary>
        ''' Checks a database-returned value for NULL, and returns an empty variable value instead.
        ''' </summary>
        ''' <param name="value">Database field value</param>
        ''' <param name="fieldType">Database field type</param>
        ''' <returns>Object returned</returns>
        Public Shared Function CheckForNull(ByVal value As Object, ByVal fieldType As Type) As Object
            If TypeOf value Is DBNull Then
                Select Case fieldType.Name
                    Case "DateTime"
                        Return System.DateTime.MinValue
                    Case "String"
                        Return String.Empty
                    Case "Guid"
                        Return System.Guid.Empty
                    Case Else
                        Return 0
                End Select
            Else
                If fieldType.IsEnum Then
                    Return [Enum].ToObject(fieldType, value) ' enums have a problem with conversion from object types
                ElseIf fieldType Is GetType(Boolean) Then
                    Return CBool(value) ' booleans are Int32's in the record, so a narrowing conversion is needed
                Else
                    Return value
                End If
            End If
        End Function

        ''' <summary>
        ''' Checks a saved value in case certain empty values should be stored as null on the database.
        ''' </summary>
        ''' <param name="value">Database field value</param>
        ''' <param name="fieldType">Database field type</param>
        ''' <returns>Object returned</returns>
        Protected Shared Function CheckForNullable(ByVal value As Object, ByVal fieldType As SqlDbType) As Object
            Select Case fieldType
                Case SqlDbType.DateTime, SqlDbType.DateTime2, SqlDbType.Date, SqlDbType.Time, SqlDbType.SmallDateTime
                    ' datetime type
                    Return If(CDate(value) = System.DateTime.MinValue Or CDate(value).Date = System.DateTime.MaxValue.Date, DBNull.Value, value)
                Case SqlDbType.NVarChar, SqlDbType.NChar, SqlDbType.NText, _
                 SqlDbType.VarChar, SqlDbType.Char, SqlDbType.Text
                    ' string type
                    Return If(String.IsNullOrEmpty(CStr(value)), DBNull.Value, value)
                Case SqlDbType.UniqueIdentifier
                    ' globally unique id
                    Return If(DirectCast(value, System.Guid) = System.Guid.Empty, DBNull.Value, value)
                Case SqlDbType.Bit
                    ' boolean type
                    Return If(CBool(value) = False, SqlTypes.SqlBoolean.Null, value)
                Case SqlDbType.Int, SqlDbType.BigInt, SqlDbType.TinyInt, SqlDbType.SmallInt
                    ' integer types
                    Return If(CLng(value) = 0, DBNull.Value, value)
                Case SqlDbType.Float, SqlDbType.Real, SqlDbType.Decimal, SqlDbType.Money, SqlDbType.SmallMoney
                    ' floating-point types
                    Return If(CDec(value) = 0.0, DBNull.Value, value)

                Case SqlDbType.Xml
                    Return If(String.IsNullOrEmpty(CStr(value)), DBNull.Value, value)

                Case SqlDbType.Binary
                    Return If(value, DBNull.Value)

                Case SqlDbType.Variant
                    Return If(value, DBNull.Value)

                Case Else
                    Return If(value, DBNull.Value)
            End Select
        End Function

#Region "Reporting Methods"

        Public Shared Function GetDataTable(ByVal spname As String, voteID As Integer) As DataTable

            Dim conn As SqlConnection = New SqlConnection(_dbConnection)
            Dim comm As SqlCommand

            comm = New SqlCommand(spname, conn)
            comm.CommandType = CommandType.StoredProcedure
            comm.Parameters.Add(New SqlParameter("@voteID", voteID))

            Dim dt As New DataTable()
            Dim DataAdapter As SqlDataAdapter
            DataAdapter = New SqlDataAdapter
            DataAdapter.SelectCommand = comm
            DataAdapter.Fill(dt)

            Return dt


        End Function


#End Region


#Region "Read Methods"

        ''' <summary>
        ''' Creates a new Vote within the current Site
        ''' </summary>
        ''' <returns>ISL.OneWeb.ClientApplications.NSNU.VotingApplication.Entities.Vote</returns>
        Public Shared Function CreateVote() As VoteEnts.Vote

            ' create the page and set some default values
            Dim vote As New VoteEnts.Vote
            vote.Title = "New Vote"
            vote.IsNew = True
            Return vote

        End Function

        ''' <summary>
        ''' Creates a new Vote within the current Site
        ''' </summary>
        ''' <returns>ISL.OneWeb.ClientApplications.NSNU.VotingApplication.Entities.Vote</returns>
        Public Shared Function CreateVote(ByVal voteId As Integer) As VoteEnts.Vote

            ' create the page and set some default values
            Dim vote As New VoteEnts.Vote(voteId)
            vote.Title = "New Vote"
            vote.IsNew = True
            Return vote

        End Function


        ''' <summary>
        ''' Creates a new UserVote within the current Site
        ''' </summary>
        ''' <returns>ISL.OneWeb.ClientApplications.NSNU.VotingApplication.Entities.userVote</returns>
        Public Shared Function CreateUserVote() As VoteEnts.UserVote

            ' create the page and set some default values
            Dim uservote As New VoteEnts.UserVote
            uservote.Username = "Voting Member"
            uservote.IsNew = True
            Return uservote

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the Votes for a Site
        ''' </summary>
        ''' <param name="site">Site entity to load with the Votes</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Simon Anderson]	2005-12-08	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function GetVotes(ByVal site As OWEnts.Site) As VoteEnts.VoteCollection
            ' parameter positions
            'Const pRC As Integer = 0
            Const pSiteID As Integer = 0

            ' Initialize the stored proc name
            ' These will form the key used to store and retrieve the parameters
            Dim spName As String = "vaGetVotes"
            Dim dr As SqlDataReader
            Dim votes As New VoteEnts.VoteCollection
            Dim params() As SqlParameter

            Try
                'Retrieve the parameters
                params = MSData.SqlHelperParameterCache.GetSpParameterSet(_dbConnection, spName, False)

                params(pSiteID).Value = site.SiteId

                ' call SP 
                dr = MSData.SqlHelper.ExecuteReader(_dbConnection, spName, params)

                If dr.HasRows Then
                    ' load the record into the entity
                    votes.Load(dr)
                End If
                Return votes


            Catch ex As Data.SqlClient.SqlException
                Throw New OWSys.DataException("Exception occurred in GetVotes.", ex)
            Finally
                ' close the datareader if not yet closed
                If dr IsNot Nothing AndAlso Not dr.IsClosed Then dr.Close()
            End Try
            Return Nothing

        End Function


        Public Shared Function GetVotesByDate() As DataSet


            'SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
            '    mySqlCommand.CommandText = "IDCategory";
            '    mySqlCommand.CommandType = CommandType.StoredProcedure;
            '    mySqlCommand.Parameters.Add("@IDCategory", SqlDbType.Int).Value = 5;

            '    SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
            '    mySqlDataAdapter.SelectCommand = mySqlCommand;
            '    DataSet myDataSet = new DataSet();
            '    mySqlConnection.Open();
            '    mySqlDataAdapter.Fill(myDataSet);

        End Function

        ''' <summary>
        ''' Gets Vote information from the database.
        ''' </summary>
        ''' <param name="vote">Vote entity to load</param>
        ''' <returns></returns>
        Public Shared Function LoadVote(ByVal vote As VoteEnts.Vote) As Boolean
            'These will form the key used to store and retrieve the parameters
            Dim spName As String = "vaGetVote"
            Dim dr As SqlDataReader

            ' parameter positions
            'Const pRC As Integer = 0
            Const pVoteID As Integer = 0

            Dim params() As SqlParameter

            Try

                'Retrieve the parameters
                params = MSData.SqlHelperParameterCache.GetSpParameterSet(_dbConnection, spName, False)
                params(pVoteID).Value = vote.VoteId

                ' call SP 
                dr = MSData.SqlHelper.ExecuteReader(_dbConnection, spName, params)

                If dr.HasRows AndAlso dr.Read() Then
                    ' load the record into the entity
                    If vote.Load(dr) Then Return True
                End If
                Return False

            Catch ex As Data.SqlClient.SqlException
                Throw New OWSys.DataException("Exception occurred in LoadVote.", ex)
            Finally
                If Not dr Is Nothing AndAlso Not dr.IsClosed Then dr.Close()
            End Try

        End Function

        ''' <summary>
        ''' Gets UserVote information from the database.
        ''' </summary>
        ''' <param name="uservote">Vote entity to load</param>
        ''' <returns></returns>
        Public Shared Function LoadMemberVote(ByVal uservote As VoteEnts.UserVote) As Boolean
            'These will form the key used to store and retrieve the parameters
            Dim spName As String = "vaGetUserVote"
            Dim dr As SqlDataReader

            ' parameter positions
            'Const pRC As Integer = 0
            Const pVoteID As Integer = 0
            Const pUsername As Integer = 1

            Dim params() As SqlParameter

            Try

                'Retrieve the parameters
                params = MSData.SqlHelperParameterCache.GetSpParameterSet(_dbConnection, spName, False)
                params(pVoteID).Value = uservote.VoteId
                params(pUsername).Value = uservote.Username


                ' call SP 
                dr = MSData.SqlHelper.ExecuteReader(_dbConnection, spName, params)

                If dr.HasRows AndAlso dr.Read() Then
                    ' load the record into the entity
                    If uservote.Load(dr) Then Return True
                End If
                Return False

            Catch ex As Data.SqlClient.SqlException
                Throw New OWSys.DataException("Exception occurred in LoadUserVote.", ex)
            Finally
                If Not dr Is Nothing AndAlso Not dr.IsClosed Then dr.Close()
            End Try

        End Function
#End Region

#Region "Write Methods"

        ''' <summary>
        ''' Adds a new Vote entity to the database.
        ''' </summary>
        ''' <param name="vote">The Vote entity to add.</param>
        ''' <returns>Boolean success value.</returns>
        Public Shared Function AddVote(ByVal vote As VoteEnts.Vote) As Boolean
            ' parameter positions
            Const pRC As Integer = 0
            Const pSiteID As Integer = 1
            Const pTitle As Integer = 2
            Const pLure As Integer = 3
            Const pQuestion As Integer = 4
            Const pAnswer1 As Integer = 5
            Const pAnswer2 As Integer = 6
            Const pDisplayDate As Integer = 7
            Const pExpiryDate As Integer = 8
            Const pLastModifiedBy As Integer = 9
            Const pLastModifiedDate As Integer = 10
            Const pVoteID As Integer = 11

            ' check if the item is dirty
            If Not vote.IsDirty Then Exit Function

            'Initialize the stored proc name
            'These will form the key used to store and retrieve the parameters
            Dim spName As String = "vaAddVote"

            'Retrieve the parameters
            Dim params() As SqlParameter
            Try
                params = MSData.SqlHelperParameterCache.GetSpParameterSet(_dbConnection, spName, True)

                'Cache the parameters
                params(pSiteID).Value = vote.SiteId
                params(pTitle).Value = vote.Title
                params(pLure).Value = vote.Lure
                params(pQuestion).Value = vote.Question
                params(pAnswer1).Value = vote.Answer1
                params(pAnswer2).Value = vote.Answer2
                params(pDisplayDate).Value = CheckForNullable(vote.DisplayDate, params(pDisplayDate).SqlDbType)
                params(pExpiryDate).Value = CheckForNullable(vote.ExpiryDate, params(pExpiryDate).SqlDbType)
                params(pLastModifiedBy).Value = vote.LastModifiedBy
                params(pLastModifiedDate).Value = CheckForNullable(vote.LastModifiedDate, params(pLastModifiedDate).SqlDbType)

                'Use the parameters in a command
                MSData.SqlHelper.ExecuteNonQuery(_dbConnection, CommandType.StoredProcedure, spName, params)

            Catch ex As Data.SqlClient.SqlException
                Throw New OWSys.DataException("Exception occurred in AddVote.", ex)
            End Try

            ' get the return value
            Dim rc As Integer
            rc = CInt(params(pRC).Value)

            ' check return code
            If rc = 0 Then
                ' unable to update the data
                Throw New OWSys.DataException("Exception occurred in AddVote.", Nothing)
                Return False
            ElseIf rc = 2 Then
                ' vote date overlaps another vote date
                Throw New VotingApplication.Exceptions.OverlapDateException("Please ensure that the vote's active date range does not overlap with another vote's.")
                Return False
            ElseIf rc = 1 Then
                ' success - set the new values
                vote.VoteId = CInt(CheckForNull(params(pVoteID).Value, vote.VoteId.GetType))
                vote.IsLoaded = True
                vote.IsDirty = False

                ' return true
                Return True
            Else
                ' unknown problem
                Return False
            End If

        End Function

        ''' <summary>
        ''' Saves the Vote entity to the database.
        ''' </summary>
        ''' <param name="vote">The Vote entity to save.</param>
        ''' <returns>Boolean success level.</returns>
        Public Shared Function SaveVote(ByVal vote As VoteEnts.Vote) As Boolean
            ' parameter positions
            Const pRC As Integer = 0
            Const pVoteID As Integer = 1
            Const pSiteID As Integer = 2
            Const pTitle As Integer = 3
            Const pLure As Integer = 4
            Const pQuestion As Integer = 5
            Const pAnswer1 As Integer = 6
            Const pAnswer2 As Integer = 7
            Const pDisplayDate As Integer = 8
            Const pExpiryDate As Integer = 9
            Const pLastModifiedBy As Integer = 10
            Const pLastModifiedDate As Integer = 11

            ' check if the item is dirty
            If Not vote.IsDirty Then Return True

            'Initialize the stored proc name
            'These will form the key used to store and retrieve the parameters
            Dim spName As String = "vaSaveVote"

            'Retrieve the parameters
            Dim params() As SqlParameter
            Try
                'Cache the parameters
                params = MSData.SqlHelperParameterCache.GetSpParameterSet(_dbConnection, spName, True)

                ' set the parameters
                params(pVoteID).Value = vote.VoteId
                params(pSiteID).Value = vote.SiteId
                params(pTitle).Value = vote.Title
                params(pLure).Value = vote.Lure
                params(pQuestion).Value = vote.Question
                params(pAnswer1).Value = vote.Answer1
                params(pAnswer2).Value = vote.Answer2
                params(pDisplayDate).Value = CheckForNullable(vote.DisplayDate, params(pDisplayDate).SqlDbType)
                params(pExpiryDate).Value = CheckForNullable(vote.ExpiryDate, params(pExpiryDate).SqlDbType)
                params(pLastModifiedBy).Value = vote.LastModifiedBy
                params(pLastModifiedDate).Value = CheckForNullable(vote.LastModifiedDate, params(pLastModifiedDate).SqlDbType)

                'Use the parameters in a command
                MSData.SqlHelper.ExecuteNonQuery(_dbConnection, CommandType.StoredProcedure, spName, params)

            Catch ex As Data.SqlClient.SqlException
                Throw New OWSys.DataException("Exception occurred in SaveVote.", ex)
            End Try

            ' get the return value
            Dim rc As Integer
            rc = CInt(params(pRC).Value)

            ' check return code
            If rc = 0 Then
                ' unable to update the data
                Throw New OWSys.DataException("Exception occurred in SaveVote.", Nothing)
                Return False
            ElseIf rc = 2 Then
                ' vote date overlaps another vote date
                Throw New VotingApplication.Exceptions.OverlapDateException("Please ensure that the vote's active date range does not overlap with another vote's.")
                Return False
            ElseIf rc = 1 Then
                ' success - set the new values
                vote.IsLoaded = True
                vote.IsDirty = False

                ' return true
                Return True
            Else
                ' unknown problem
                Return False
            End If

        End Function

        ''' <summary>
        ''' Deletes the Vote.
        ''' </summary>
        ''' <param name="vote">ISL.OneWeb.ClientApplications.NSNU.VotingApplication.Entities.Vote entity to delete.</param>
        ''' <returns>Boolean succees</returns>
        Public Shared Function DeleteVote(ByVal vote As VoteEnts.Vote) As Boolean
            ' parameter positions
            Const pRC As Integer = 0
            Const pVoteID As Integer = 1

            'Initialize the stored proc name
            'These will form the key used to store and retrieve the parameters
            Dim spName As String = "vaDeleteVote"

            'Retrieve the parameters
            Dim params() As SqlParameter
            Try
                params = MSData.SqlHelperParameterCache.GetSpParameterSet(_dbConnection, spName, True)

                'Cache the parameters
                params(pVoteID).Value = vote.VoteId

                'Use the parameters in a command
                MSData.SqlHelper.ExecuteNonQuery(_dbConnection, CommandType.StoredProcedure, spName, params)

            Catch ex As Data.SqlClient.SqlException
                Throw New OWSys.DataException("Exception occurred in DeleteVote.", ex)
            End Try

            ' get the return value
            Dim rc As Integer
            rc = CInt(params(pRC).Value)

            ' check return code
            If rc = 0 Then
                ' unable to move the page
                Throw New OWSys.DataException("Exception occurred in DeleteVote.", Nothing)
                Return False
            ElseIf rc = 1 Then
                ' success - return the parent page

                ' return true
                Return True
            Else
                ' unknown problem
                Return False
            End If
        End Function


        Public Shared Function AddUserVote(ByVal userVote As VoteEnts.UserVote) As Boolean

            ' parameter positions
            Const pRC As Integer = 0
            Const pVoteID As Integer = 1
            Const pUserID As Integer = 2
            Const pVoteDate As Integer = 3

            ' check if the item is dirty
            If Not userVote.IsDirty Then Exit Function

            'Initialize the stored proc name
            'These will form the key used to store and retrieve the parameters
            Dim spName As String = "vaAddUserVote"

            'Retrieve the parameters
            Dim params() As SqlParameter
            Try
                params = MSData.SqlHelperParameterCache.GetSpParameterSet(_dbConnection, spName, True)

                'Cache the parameters
                params(pVoteID).Value = userVote.VoteId
                params(pUserID).Value = userVote.Username
                params(pVoteDate).Value = CheckForNullable(userVote.VoteDate, params(pVoteDate).SqlDbType)

                'Use the parameters in a command
                MSData.SqlHelper.ExecuteNonQuery(_dbConnection, CommandType.StoredProcedure, spName, params)

            Catch ex As Data.SqlClient.SqlException
                Throw New OWSys.DataException("Exception occurred in AddUserVote.", ex)
            End Try

            ' get the return value
            Dim rc As Integer
            rc = CInt(params(pRC).Value)

            ' check return code
            If rc = 0 Then
                ' unable to update the data
                Throw New OWSys.DataException("Exception occurred in AddUserVote.", Nothing)
                Return False

            ElseIf rc = 1 Then
                ' unable to update the data, voter was not a member of the Nurses system
                Throw New OWSys.DataException("Exception occurred in AddUserVote - member not in system", Nothing)
                Return False

            ElseIf rc = 2 Then
                ' return true
                Return True
            Else
                ' unknown problem
                Return False
            End If

        End Function

        Public Shared Function UpdateVoteCounts(ByVal vote As VoteEnts.Vote) As Boolean

            ' parameter positions
            Const pRC As Integer = 0
            Const pVoteID As Integer = 1
            Const pAnswer1Count As Integer = 2
            Const pAnswer2Count As Integer = 3

            ' check if the item is dirty
            If Not vote.IsDirty Then Return True

            'Initialize the stored proc name
            'These will form the key used to store and retrieve the parameters
            Dim spName As String = "vaUpdateVoteCounts"

            'Retrieve the parameters
            Dim params() As SqlParameter
            Try
                'Cache the parameters
                params = MSData.SqlHelperParameterCache.GetSpParameterSet(_dbConnection, spName, True)

                ' set the parameters
                params(pVoteID).Value = vote.VoteId
                params(pAnswer1Count).Value = vote.Answer1Count
                params(pAnswer2Count).Value = vote.Answer2Count

                'Use the parameters in a command
                MSData.SqlHelper.ExecuteNonQuery(_dbConnection, CommandType.StoredProcedure, spName, params)

            Catch ex As Data.SqlClient.SqlException
                Throw New OWSys.DataException("Exception occurred in UpdateVoteCounts.", ex)
            End Try

            ' get the return value
            Dim rc As Integer
            rc = CInt(params(pRC).Value)

            ' check return code
            If rc = 0 Then
                ' unable to update the data
                Throw New OWSys.DataException("Exception occurred in UpdateVoteCounts.", Nothing)
                Return False
            ElseIf rc = 1 Then
                ' success - set the new values
                vote.IsLoaded = True
                vote.IsDirty = False

                Return True
            Else
                ' unknown problem
                Return False
            End If

        End Function
#End Region





    End Class

End Namespace
