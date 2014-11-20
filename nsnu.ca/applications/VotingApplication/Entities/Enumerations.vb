' Enumerations
' Documentation:

Option Strict On
Option Explicit On

Namespace Entities


    ''' <summary>
    ''' An enumeration for the modes of the application.
    ''' </summary>
    Public Enum Mode
        [Default] = 0
        Lure = 1
        Vote = 2
        'Lure = 4
        'Calendar = 1
        'Search = 5
        '<Obsolete()> _
        'Results = 6
        'AddEvent = 7
    End Enum

    ''' <summary>
    ''' An enumeration of the types of objects in the Events application.
    ''' </summary>
    <Serializable()> _
    Public Enum ApplicationObjectType
        Vote
        '[Event]
        '[Option]
        'Questions
        'Registrant
    End Enum



    ''' <summary>
    ''' An enumeration that indicates whether an event was created by an administrator (internal) or was submitted from the site by a visitor (external).
    ''' </summary>
    <Serializable()> _
    Public Enum Status
        Unknown = 0
        Active = 1      ' the current vote
        Pending = 2    ' hasn't started yet
        Expired = 3     ' voting has closed
    End Enum

End Namespace

' Copyright 2006 Internet Solutions Ltd.
