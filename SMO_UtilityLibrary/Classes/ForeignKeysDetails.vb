Namespace Classes
    Public Class ForeignKeysDetails
        Public Property TableSchema() As String
        Public Property TableName() As String
        Public Property SchemaName() As String
        Public Overrides Function ToString() As String
            Return SchemaName
        End Function
    End Class
End Namespace