Imports Microsoft.SqlServer.Management.Smo
Imports SMO_UtilityLibrary.Interfaces

Namespace Classes

    Public Class DatabaseDetails
        Implements IDetails
        Public Property ServerName() As String
        Public Property Name As String Implements IDetails.Name
        Public Property Exists As Boolean Implements IDetails.Exists
        Public Property Database() As Database
        Public ReadOnly Property CreationDateTime() As Date
            Get
                Return If(Exists, Database.CreateDate, New Date())
            End Get
        End Property
        Public ReadOnly Property LastBackupDate() As Date
            Get
                Return Database.LastBackupDate
            End Get
        End Property
    End Class
End Namespace