' Class Vote
' Documentation:

Option Strict On
Option Explicit On

Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports OWEnts = ISL.OneWeb4.Entities

Namespace Entities

#Region "Vote Class"

    ''' -----------------------------------------------------------------------------
    ''' Project	 : VotingApplication
    ''' Class	 : OneWeb4.Applications.VoteCalendar.Entities.Vote
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' An Vote in the calendar
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Travis Musika]	2006-03-01	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    <Serializable()> _
    Public Class Vote
        Inherits OWEnts.BaseEntity

#Region "Private Members"

        Private _voteId As Integer = 0
        Private _siteId As Integer = 0
        Private _site As OWEnts.Site = Nothing
        Private _culture As String = String.Empty
        Private _title As String = String.Empty
        Private _lure As String = String.Empty
        Private _question As String = String.Empty
        Private _answer1 As String = String.Empty
        Private _answer2 As String = String.Empty
        Private _answer1Count As Integer = 0
        Private _answer2Count As Integer = 0
        Private _displayDate As DateTime = DateTime.MinValue
        Private _expiryDate As DateTime = DateTime.MaxValue
        Private _lastModifiedBy As String = String.Empty
        Private _lastModifiedDate As Date = Date.MaxValue
        Private _status As Status = Status.Active
        Private _isDeleted As Boolean = False

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
        ''' Gets or sets the id of the Vote.
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
        ''' Gets or sets the ID of the Site.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property SiteId() As Integer
            Get
                Return Me._siteId
            End Get
            Set(ByVal Value As Integer)
                If Not _siteId = Value Then
                    _siteId = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the Site the application is on.
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore()> _
        Public ReadOnly Property Site() As OWEnts.Site
            Get
                If _siteId = 0 Then Return Nothing
                If _site Is Nothing Then
                    _site = New OWEnts.Site
                    _site.SiteId = Me._siteId
                    _site.Culture = Me._culture
                End If
                Return _site
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the culture of the inquiry.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property Culture() As String
            Get
                Return Me._culture
            End Get
            Set(ByVal Value As String)
                If Not _culture = Value Then
                    _culture = Value
                    _site = Nothing
                    Me.IsDirty = True
                    Me.IsLoaded = False
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the status of the Vote.
        ''' If the vote hasn't started yet then it's Inactive
        ''' If the vote has started and hasn't expired it's Active
        ''' If the votes expiry date has passed, it's Closed
        ''' </summary>
        ''' <returns>Status</returns>
        Public ReadOnly Property VoteStatus As Status
            Get

                If _expiryDate < DateTime.Now Then
                    _status = Status.Expired
                ElseIf _displayDate > DateTime.Now Then
                    _status = Status.Pending
                ElseIf _displayDate <= DateTime.Now And _expiryDate > DateTime.Now Then
                    _status = Status.Active
                Else
                    _status = Status.Unknown
                End If
                Return _status
            End Get
        End Property


        ' ''' <summary>
        ' ''' Gets or sets start date of the Vote.
        ' ''' </summary>
        ' ''' <returns></returns>
        '<XmlIgnore()> _
        'Public Property Duration() As TimeSpan
        '    Get
        '        Return _endTime - _startTime
        '    End Get
        '    Set(ByVal Value As TimeSpan)
        '        If Not (_endTime - _startTime) = Value Then
        '            _endTime = _startTime + Value
        '            Me.IsDirty = True
        '        End If
        '    End Set
        'End Property

        '' XmlSerializer does not support TimeSpan, so use this property for serialization instead.
        '<ComponentModel.Browsable(False), ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
        '  <XmlAttribute(DataType:="duration", AttributeName:="Duration")> _
        'Public Property DurationString As String
        '    Get
        '        Return System.Xml.XmlConvert.ToString(Duration)
        '    End Get
        '    Set(value As String)
        '        Duration = If(String.IsNullOrEmpty(value), TimeSpan.Zero, System.Xml.XmlConvert.ToTimeSpan(value))

        '    End Set
        'End Property

        ''' <summary>
        ''' Gets or sets the title of the Vote.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property Title() As String
            Get
                Return Me._title
            End Get
            Set(ByVal Value As String)
                If Not _title = Value Then
                    _title = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the lure text of the Vote.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property Lure() As String
            Get
                Return Me._lure
            End Get
            Set(ByVal Value As String)
                If Not CBool(String.Compare(_lure, Value) = 0) Then
                    _lure = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the question to Vote on.
        ''' </summary>
        ''' <returns></returns>
        <XmlElement()> _
        Public Property Question() As String
            Get
                Return Me._question
            End Get
            Set(ByVal Value As String)
                If Not _question = Value Then
                    _question = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the first answer for the Vote.
        ''' </summary>
        ''' <returns></returns>
        <XmlElement()> _
        Public Property Answer1() As String
            Get
                Return Me._answer1
            End Get
            Set(ByVal Value As String)
                If Not _answer1 = Value Then
                    _answer1 = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the second answer for the Vote.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property Answer2() As String
            Get
                Return Me._answer2
            End Get
            Set(ByVal Value As String)
                If Not _answer2 = Value Then
                    _answer2 = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of times the first answer was chosen for the Vote.
        ''' </summary>
        ''' <returns></returns>
        <XmlElement()> _
        Public Property Answer1Count() As Integer
            Get
                Return Me._answer1Count
            End Get
            Set(ByVal Value As Integer)
                If Not _answer1Count = Value Then
                    _answer1Count = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of times the second answer was chosen for the Vote.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property Answer2Count() As Integer
            Get
                Return Me._answer2Count
            End Get
            Set(ByVal Value As Integer)
                If Not _answer2Count = Value Then
                    _answer2Count = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property
        ''' <summary>
        ''' Gets or sets display date of the Vote.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property DisplayDate() As DateTime
            Get
                Return Me._displayDate
            End Get
            Set(ByVal Value As DateTime)
                If Not _displayDate = Value Then
                    _displayDate = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets expiry date of the Vote.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property ExpiryDate() As DateTime
            Get
                Return Me._expiryDate
            End Get
            Set(ByVal Value As DateTime)
                If Not _expiryDate = Value Then
                    _expiryDate = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the person who last modified the Vote.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property LastModifiedBy() As String
            Get
                Return Me._lastModifiedBy
            End Get
            Set(ByVal Value As String)
                If Not _lastModifiedBy = Value Then
                    _lastModifiedBy = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets last modified date of the Vote.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property LastModifiedDate() As Date
            Get
                Return Me._lastModifiedDate
            End Get
            Set(ByVal Value As Date)
                If Not _lastModifiedDate = Value Then
                    _lastModifiedDate = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whetjher the Vote has been deleted (soft deleted in the db).
        ''' </summary>
        ''' <returns></returns>
        <XmlAttributeAttribute()> _
        Public Property IsDeleted() As Boolean
            Get
                Return Me._isDeleted
            End Get
            Set(ByVal Value As Boolean)
                If Not _isDeleted = Value Then
                    _isDeleted = Value
                    Me.IsDirty = True
                End If
            End Set
        End Property
        ' ''' <summary>
        ' ''' Gets or sets the default time zone id for the Site.
        ' ''' </summary>
        ' ''' <returns>Integer</returns>
        '<XmlAttributeAttribute()> _
        'Public Property TimeZoneId() As String
        '    Get
        '        Return Me._timeZoneId
        '    End Get
        '    Set(ByVal Value As String)
        '        If Value <> _timeZoneId Then
        '            Me._timeZoneId = Value
        '            Me._timeZone = Nothing
        '            Me.IsDirty = True
        '        End If
        '    End Set
        'End Property

        ' ''' <summary>
        ' ''' Gets a TimeZoneInfo object representing the timezone setting for the site.
        ' ''' </summary>
        ' ''' <returns></returns>
        '<XmlIgnore()> _
        'Public ReadOnly Property TimeZone() As TimeZoneInfo
        '    Get
        '        ' if the site does not have a time zone set, use the local (server) time zone
        '        If _timeZoneId = String.Empty Then Return TimeZoneInfo.Local
        '        If _timeZone Is Nothing Then
        '            Try
        '                _timeZone = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId)
        '            Catch ex As Exception
        '                _timeZone = TimeZoneInfo.Local
        '            End Try
        '        End If
        '        Return _timeZone
        '    End Get
        'End Property


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
                Return Me.Title
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
                Case "id" : Me.VoteId = CInt(value)
                Case "siteid" : Me.SiteId = CInt(value)
                Case "culture" : Me.Culture = CStr(value)
                Case "title" : Me.Title = CStr(value)
                Case "lure" : Me.Lure = CStr(value)
                Case "question" : Me.Question = CStr(value)
                Case "answer1" : Me.Answer1 = CStr(value)
                Case "answer2" : Me.Answer2 = CStr(value)
                Case "answer1count" : Me.Answer1Count = CInt(value)
                Case "answer2count" : Me.Answer2Count = CInt(value)
                Case "displaydate" : Me.DisplayDate = CDate(value)
                Case "expirydate" : Me.ExpiryDate = CDate(value)
                Case "lastmodifiedby" : Me.LastModifiedBy = CStr(value)
                Case "lastmodifieddate" : Me.LastModifiedDate = CDate(value)
                Case "isdeleted" : Me.IsDeleted = CBool(value)

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
                With CType(base, [Vote])
                    Me.VoteId = .VoteId
                    Me.SiteId = .SiteId
                    Me.Culture = .Culture
                    Me.Title = .Title
                    Me.Lure = .Lure
                    Me.Question = .Question
                    Me.Answer1 = .Answer1
                    Me.Answer2 = .Answer2
                    Me.Answer1Count = .Answer1Count
                    Me.Answer2Count = .Answer2Count
                    Me.DisplayDate = .DisplayDate
                    Me.ExpiryDate = .ExpiryDate
                    Me.LastModifiedBy = .LastModifiedBy
                    Me.LastModifiedDate = .LastModifiedDate
                    Me.IsDeleted = .IsDeleted

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

#Region "VoteCollection Class"

    ''' <summary>
    ''' A strongly-typed Collection of Votes
    ''' </summary>
    <Serializable(), XmlRoot("Votes")> _
    Public Class VoteCollection
        Inherits OWEnts.BaseEntityCollection
        Implements ICloneable

        ''' <summary>
        ''' Gets the Personnel specified by the index given.
        ''' </summary>
        ''' <param name="idx"></param>
        ''' <returns></returns>
        Default Public Property Item(ByVal idx As Integer) As Vote
            Get
                Return CType(MyBase.List(idx), Vote)
            End Get
            Set(ByVal Value As Vote)
                MyBase.List(idx) = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets the Personnel specified by the Id value given.
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public ReadOnly Property ItemById(ByVal id As Integer) As Vote
            Get
                For Each p As Vote In Me.InnerList
                    If p.VoteId = id Then Return p
                Next
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Adds a new Personnel entity to the collection.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns>Integer index of the newly added Personnel</returns>
        Public Function Add(ByVal value As Vote) As Integer
            If value Is Nothing Then Throw New System.ArgumentNullException("value", "A null value was passed to the Add method")
            Return MyBase.List.Add(value)
        End Function

        ''' <summary>
        ''' Adds a collection of Personnel entities to the collection.
        ''' </summary>
        ''' <param name="lc">PersonnelCollection containing entities to be added.</param>
        Public Overridable Sub AddRange(ByVal pc As VoteCollection)
            For Each p As Vote In pc
                MyBase.List.Add(p)
            Next
        End Sub

        ''' <summary>
        ''' Creates a shallow copy of the PersonnelCollection.
        ''' </summary>
        Public Overridable Function Clone() As Object Implements ICloneable.Clone
            Dim pc As New VoteCollection
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
        ''' Loads the properties of the Personnel from the rows of a DataReader
        ''' </summary>
        ''' <param name="dr">DataReader to load properties with.</param>
        ''' <param name="clear">Boolean value indicating whether to clear the collection first.</param>
        ''' <returns>Boolean success value</returns>
        Public Overloads Function Load(ByVal dr As System.Data.SqlClient.SqlDataReader, ByVal clear As Boolean) As Boolean

            ' clear out current items
            If clear Then MyBase.Clear()

            Try
                Do While dr.Read()
                    Dim p As New Vote

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

