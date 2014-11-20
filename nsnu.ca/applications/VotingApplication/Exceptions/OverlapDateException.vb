Imports OWSys = ISL.OneWeb4.Sys


Namespace VotingApplication.Exceptions

    Public Class OverlapDateException
        Inherits Microsoft.ApplicationBlocks.ExceptionManagement.BaseApplicationException

#Region "Constructors"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the OverlapDateException class.
        ''' </summary>
        ''' <param name="message">String.</param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Simon Anderson]	2010-04-30	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the OverlapDateException class.
        ''' </summary>
        ''' <param name="message">String.</param>
        ''' <param name="ex">System.Exception</param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Simon Anderson]	2010-04-30	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New(ByVal message As String, ByVal ex As System.Exception)
            MyBase.New(message, ex)
        End Sub

#End Region

    End Class



End Namespace
