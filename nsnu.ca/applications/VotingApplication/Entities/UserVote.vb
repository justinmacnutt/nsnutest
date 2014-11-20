' Class UserVote
' Documentation:

Option Strict On
Option Explicit On

Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports OWEnts = ISL.OneWeb4.Entities

Namespace Entities

#Region "User Vote Class"

    ''' -----------------------------------------------------------------------------
    ''' Project	 : VotingApplication
    ''' Class	 : OneWeb4.Applications.VoteCalendar.Entities.UserVote
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Travis Musika]	2006-03-01	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    <Serializable()> _
    Public Class UserVote
        Inherits OWEnts.BaseEntity

#Region "Private Members"

        Private _voteId As Integer = 0
        Private _username As String = String.Empty
        Private _voteDate As Date = Date.MinValue

#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal VoteId As Integer)
            MyBase.New()
            _voteId = VoteId
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Gets or sets the vote id of the UserVote.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property VoteId() As Integer
            Get
                Return Me._voteId
            End Get
            Set(ByVal Value As Integer)
                If Not _voteId = Value Then
                    _voteId = Value
                    Me.IsDirty = True
                    Me.IsLoaded = False
                End If
            End Set
        End Property



        ''' <summary>
        ''' Gets or sets the username of the person who voted.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property Username() As String
            Get
                Return Me._username
            End Get
            Set(ByVal Value As String)
                If Not _username = Value Then
                    _username = Value
                    Me.IsDirty = True
                    Me.IsLoaded = False
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the  date the Vote was submitted.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property VoteDate() As Date
            Get
                Return Me._voteDate
            End Get
            Set(ByVal Value As Date)
                If Not _voteDate = Value Then
                    _voteDate = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property


#End Region

#Region "BaseEntity Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the id of this entity, as an integer
        ''' </summary>
        ''' <value></value>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Travis Musika]	2006-03-21	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides ReadOnly Property EntityId() As Integer
            Get
                Return Me.VoteId
            End Get
        End Property

        ''' <summary>
        ''' Gets the name of this entity
        ''' </summary>
        ''' <returns>String</returns>
        Public Overrides ReadOnly Property EntityName() As String
            Get
                Return Me.Username
            End Get
        End Property

        ''' <summary>
        ''' Gets the type of object this entity represents, as a value in the ObjectType Enum
        ''' </summary>
        ''' <returns>ObjectType</returns>
        Public Overrides ReadOnly Property EntityType() As OWEnts.ObjectType
            Get
                Return OWEnts.ObjectType.ApplicationDefined
            End Get
        End Property

#End Region

#Region "Load Methods"

        ''' <summary>
        ''' Sets the property of the entity from a named value.
        ''' </summary>
        ''' <param name="name">Property name.</param>
        ''' <param name="value">Property value</param>
        Public Overrides Sub LoadProperty(ByVal name As String, ByVal value As Object)
            Dim found As Boolean = True

            Dim propname As String = name.ToLower
            Select Case propname
                Case "voteid" : Me.VoteId = CInt(value)
                Case "userid" : Me.Username = CStr(value)
                Case "votedate" : Me.VoteDate = CDate(value)
                Case Else
                    found = False
            End Select


            'End Select

            ' check if we didn't match the property, and call the base class method
            If Not found Then MyBase.LoadProperty(name, value)

        End Sub

        ''' <summary>
        ''' Loads the properties of the entity from the first row of a DataReader using reflection.
        ''' </summary>
        ''' <param name="dr">DataReader to load properties with.</param>
        ''' <returns>Boolean success value</returns>
        Public Overloads Function Load(ByVal dr As SqlClient.SqlDataReader) As Boolean
            Dim schemaTable As DataTable = dr.GetSchemaTable()

            Dim row As DataRow
            Dim nameColumn As DataColumn = schemaTable.Columns("ColumnName")
            Dim typeColumn As DataColumn = schemaTable.Columns("DataType")
            Dim ordinalColumn As DataColumn = schemaTable.Columns("ColumnOrdinal")

            Try
                Me.SuppressNotification = True
                ' loop through all the rows to set the properties
                Dim colName As String, colIdx As Integer
                For Each row In schemaTable.Rows
                    colName = CStr(row.Item(nameColumn))
                    colIdx = CInt(row.Item(ordinalColumn))
                    LoadProperty(colName, CheckForNull(dr.Item(colName), dr.GetFieldType(colIdx)))
                Next

            Catch ex As System.Exception
                Return False
            Finally
                Me.SuppressNotification = False
            End Try

            Me.IsLoaded = True
            Return True

        End Function

        ''' <summary>
        ''' Loads the properties of the entity from a DataRow using reflection.
        ''' </summary>
        ''' <param name="dr">DataRow to load properties with.</param>
        ''' <returns>Boolean success value</returns>
        Public Overloads Function Load(ByVal dr As System.Data.DataRow) As Boolean
            Dim table As DataTable = dr.Table

            Try
                Me.SuppressNotification = True
                ' loop through all the columns to set the properties
                For Each column As System.Data.DataColumn In table.Columns
                    LoadProperty(column.ColumnName, CheckForNull(dr.Item(column), column.DataType))
                Next
            Catch ex As System.Exception
                Return False
            Finally
                Me.SuppressNotification = False
            End Try

            Me.IsLoaded = True
            Return True

        End Function

        ''' <summary>
        ''' Loads the shallow properties of the entity from an XmlElement.
        ''' </summary>
        ''' <param name="element">XmlElement to load properties with.</param>
        ''' <returns>Boolean success value</returns>
        Public Overloads Function Load(ByVal element As System.Xml.XmlElement) As Boolean

            Try
                Me.SuppressNotification = True
                ' loop through all the columns to set the properties
                For Each attribute As System.Xml.XmlAttribute In element.Attributes
                    LoadProperty(attribute.LocalName, attribute.Value)
                Next

            Catch ex As System.Exception
                Return False
            Finally
                Me.SuppressNotification = False
            End Try

            Me.IsLoaded = True
            Return True

        End Function

        ''' <summary>
        ''' Loads the shallow properties of the entity from another like entity using direct copying.
        ''' </summary>
        ''' <param name="base">BaseEntity of the same base type to load properties from.</param>
        ''' <returns>Boolean success value</returns>
        Public Overloads Function Load(ByVal base As OWEnts.BaseEntity) As Boolean
            'Dim props As System.Reflection.PropertyInfo()
            'Dim propinfo As System.Reflection.PropertyInfo

            ' sanity check
            If base Is Nothing OrElse Not base.GetType.IsAssignableFrom(Me.GetType) Then Return False

            Try
                Me.SuppressNotification = True
                With CType(base, UserVote)
                    Me.VoteId = .VoteId
                    Me.Username = .Username
                    Me.VoteDate = .VoteDate

                End With

            Catch ex As System.Exception
                Return False
            Finally
                Me.SuppressNotification = False
            End Try

            Me.IsLoaded = base.IsLoaded
            Me.IsDirty = base.IsDirty
            Me.IsNew = base.IsNew

            Return True
        End Function

#End Region

    End Class

#End Region

#Region "UserVoteCollection Class"

    ''' <summary>
    ''' A strongly-typed Collection of Votes
    ''' </summary>
    <Serializable(), XmlRoot("UserVotes")> _
    Public Class UserVoteCollection
        Inherits OWEnts.BaseEntityCollection
        Implements ICloneable

        ''' <summary>
        ''' Gets the UserVote specified by the index given.
        ''' </summary>
        ''' <param name="idx"></param>
        ''' <returns></returns>
        Default Public Property Item(ByVal idx As Integer) As UserVote
            Get
                Return CType(MyBase.List(idx), UserVote)
            End Get
            Set(ByVal Value As UserVote)
                MyBase.List(idx) = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets the UserVote specified by the Id value given.
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public ReadOnly Property ItemById(ByVal id As Integer) As UserVote
            Get
                For Each p As UserVote In Me.InnerList
                    If p.VoteId = id Then Return p
                Next
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Adds a new UserVote entity to the collection.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns>Integer index of the newly added UserVote</returns>
        Public Function Add(ByVal value As UserVote) As Integer
            If value Is Nothing Then Throw New System.ArgumentNullException("value", "A null value was passed to the Add method")
            Return MyBase.List.Add(value)
        End Function

        ''' <summary>
        ''' Adds a collection of UserVote entities to the collection.
        ''' </summary>
        ''' <param name="uv">UserVoteCollection containing entities to be added.</param>
        Public Overridable Sub AddRange(ByVal uv As UserVoteCollection)
            For Each p As UserVote In uv
                MyBase.List.Add(p)
            Next
        End Sub

        ''' <summary>
        ''' Creates a shallow copy of the PersonnelCollection.
        ''' </summary>
        Public Overridable Function Clone() As Object Implements ICloneable.Clone
            Dim pc As New UserVoteCollection
            pc.AddRange(Me)
            Return pc
        End Function

        ''' <summary>
        ''' Loads the properties of the Personnel from the rows of a DataReader.
        ''' </summary>
        ''' <param name="dr">DataReader to load properties with.</param>
        ''' <returns>Boolean success value</returns>
        Public Overloads Function Load(ByVal dr As System.Data.SqlClient.SqlDataReader) As Boolean
            Return Load(dr, True)
        End Function


        ''' <summary>
        ''' Loads the properties of the UserVote from the rows of a DataReader
        ''' </summary>
        ''' <param name="dr">DataReader to load properties with.</param>
        ''' <param name="clear">Boolean value indicating whether to clear the collection first.</param>
        ''' <returns>Boolean success value</returns>
        Public Overloads Function Load(ByVal dr As System.Data.SqlClient.SqlDataReader, ByVal clear As Boolean) As Boolean

            ' clear out current items
            If clear Then MyBase.Clear()

            Try
                Do While dr.Read()
                    Dim p As New UserVote

                    Try
                        p.Load(dr)
                        ' add a new personnel to the collection
                        Me.Add(p)

                    Catch ex As System.Exception
                        ' ignore
                    End Try
                Loop

            Catch ex As System.Exception
                Return False
            Finally
                ' close the reader
                dr.Close()
            End Try

            Return True

        End Function
    End Class

#End Region

End Namespace

