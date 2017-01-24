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

Namespace DataAccess

    ''' <summary>
    ''' A DataAcess class to handle all the database calls.
    ''' </summary>
    Public Class DBAccess

        Private Shared _dbConnection As String = OWWeb.Applications.BaseApplication.ConnectionStrings.Application

        Shared Sub New()
        End Sub

        ' ''' <summary>
        ' ''' Checks a database-returned value for NULL, and returns an empty variable value instead.
        ' ''' </summary>
        ' ''' <param name="value">Database field value</param>
        ' ''' <param name="fieldType">Database field type</param>
        ' ''' <returns>Object returned</returns>
        'Public Shared Function CheckForNull(ByVal value As Object, ByVal fieldType As Type) As Object
        '    If TypeOf value Is DBNull Then
        '        Select Case fieldType.Name
        '            Case "DateTime"
        '                Return System.DateTime.MinValue
        '            Case "String"
        '                Return String.Empty
        '            Case "Guid"
        '                Return System.Guid.Empty
        '            Case Else
        '                Return 0
        '        End Select
        '    Else
        '        If fieldType.IsEnum Then
        '            Return [Enum].ToObject(fieldType, value) ' enums have a problem with conversion from object types
        '        ElseIf fieldType Is GetType(Boolean) Then
        '            Return CBool(value) ' booleans are Int32's in the record, so a narrowing conversion is needed
        '        Else
        '            Return value
        '        End If
        '    End If
        'End Function

        ' ''' <summary>
        ' ''' Checks a saved value in case certain empty values should be stored as null on the database.
        ' ''' </summary>
        ' ''' <param name="value">Database field value</param>
        ' ''' <param name="fieldType">Database field type</param>
        ' ''' <returns>Object returned</returns>
        'Protected Shared Function CheckForNullable(ByVal value As Object, ByVal fieldType As SqlDbType) As Object
        '    Select Case fieldType
        '        Case SqlDbType.DateTime, SqlDbType.DateTime2, SqlDbType.Date, SqlDbType.Time, SqlDbType.SmallDateTime
        '            ' datetime type
        '            Return If(CDate(value) = System.DateTime.MinValue Or CDate(value).Date = System.DateTime.MaxValue.Date, DBNull.Value, value)
        '        Case SqlDbType.NVarChar, SqlDbType.NChar, SqlDbType.NText, _
        '         SqlDbType.VarChar, SqlDbType.Char, SqlDbType.Text
        '            ' string type
        '            Return If(String.IsNullOrEmpty(CStr(value)), DBNull.Value, value)
        '        Case SqlDbType.UniqueIdentifier
        '            ' globally unique id
        '            Return If(DirectCast(value, System.Guid) = System.Guid.Empty, DBNull.Value, value)
        '        Case SqlDbType.Bit
        '            ' boolean type
        '            Return If(CBool(value) = False, SqlTypes.SqlBoolean.Null, value)
        '        Case SqlDbType.Int, SqlDbType.BigInt, SqlDbType.TinyInt, SqlDbType.SmallInt
        '            ' integer types
        '            Return If(CLng(value) = 0, DBNull.Value, value)
        '        Case SqlDbType.Float, SqlDbType.Real, SqlDbType.Decimal, SqlDbType.Money, SqlDbType.SmallMoney
        '            ' floating-point types
        '            Return If(CDec(value) = 0.0, DBNull.Value, value)

        '        Case SqlDbType.Xml
        '            Return If(String.IsNullOrEmpty(CStr(value)), DBNull.Value, value)

        '        Case SqlDbType.Binary
        '            Return If(value, DBNull.Value)

        '        Case SqlDbType.Variant
        '            Return If(value, DBNull.Value)

        '        Case Else
        '            Return If(value, DBNull.Value)
        '    End Select
        'End Function



#Region "Write Methods"

        ''' <summary>
        ''' Adds a new Vote entity to the database.
        ''' </summary>
        ''' <returns>Boolean success value.</returns>
        Public Shared Function AddMemberProfile(ByVal profileId As Integer, ByVal membername As String) As Boolean
            ' parameter positions
            Const pRC As Integer = 0
            Const pProfileID As Integer = 1
            Const pMemberName As Integer = 2


            'Initialize the stored proc name
            'These will form the key used to store and retrieve the parameters
            Dim spName As String = "vaAddMemberProfile"

            'Retrieve the parameters
            Dim params() As SqlParameter
            Try
                params = MSData.SqlHelperParameterCache.GetSpParameterSet(_dbConnection, spName, True)

                'Cache the parameters
                params(pProfileID).Value = profileId
                params(pMemberName).Value = membername

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
                ' unable to save the data
                Throw New OWSys.DataException("Exception occurred in AddMemberProfile.", Nothing)
                Return False
            ElseIf rc = 1 Then
                ' return true
                Return True
            Else
                ' unknown problem
                Return False
            End If

        End Function

#End Region





    End Class

End Namespace
