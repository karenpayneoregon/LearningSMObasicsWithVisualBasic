Imports SMO_UtilityLibrary
Imports SMO_UtilityLibrary.Interfaces

Namespace Classes
    Public Class TableDetails
        Implements IDetails

        Public Property ServerName() As String
        Public Property DatabaseName() As String
        ''' <summary>
        ''' Indicates if the object is valid
        ''' </summary>
        ''' <returns></returns>
        Public Property Exists As Boolean Implements IDetails.Exists
        ''' <summary>
        ''' Table name
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String Implements IDetails.Name
        ''' <summary>
        ''' Table names
        ''' </summary>
        ''' <returns></returns>
        Public Property NameList() As List(Of String)
        ''' <summary>
        ''' Check to see if there are tables in the database
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property HasTables() As Boolean
            Get
                Return NameList.Count > 0
            End Get
        End Property
    End Class
End Namespace