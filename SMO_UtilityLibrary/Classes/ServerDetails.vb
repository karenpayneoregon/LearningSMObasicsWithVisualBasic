Imports Microsoft.SqlServer.Management.Smo
Imports SMO_UtilityLibrary.Interfaces

Namespace Classes
    Public Class ServerDetails
        Implements IDetails
        Public Property Name As String Implements IDetails.Name
        Public Property Server() As Server
        Public Property Databases() As DatabaseCollection
        Public Property Exists As Boolean Implements IDetails.Exists
        Public Property Exception() As Exception
    End Class
End Namespace