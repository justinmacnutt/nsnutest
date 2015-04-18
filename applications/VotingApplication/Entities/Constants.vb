' Constants
' Documentation:

Option Strict On
Option Explicit On

Namespace Entities

	Public Class Constants
		Public Const DateSerializationFormat As String = "yyyyMMdd"
        Public Const CacheControlKey As String = "vtE-All"
        Public Const VoteCacheListKeyFormat As String = "vtE-{0}-{1}-{2:" & DateSerializationFormat & "}-{3:" & DateSerializationFormat & "}-{4}-{5}-{6}-{7}"
		Public Const SerializedCacheKey As String = "ec.{0}-{1}-{2}-{3:d}-{4}-{5}-{6}"

        'Public Const KEYWORD As String = "key"
        ' Public Const CATERGORYID As String = "cat"
        Public Const SDISPLAYDATE As String = "st"
        Public Const EXPIRYDATE As String = "ed"
        Public Const VISIBLEDATE As String = "dt"
        Public Const PAGENUMBER As String = "pg"
        Public Const PAGESIZE As String = "ps"

        Public Const QueryStringKey As String = "vt"
        Public Const MODE As String = "mode"
        Public Const INDEX As String = "idx"

        Public Const DISPLAY_DATE_FORMAT = "MMM d, yyyy" ' military time 24 hour clock
        Public Const DISPLAY_DATE_AND_TIME_FORMAT = "HH:mm, MMM d, yyyy" ' military time 24 hour clock


        Public Const AuthenticationProviderName As String = "NSNU Membership"
	End Class

End Namespace