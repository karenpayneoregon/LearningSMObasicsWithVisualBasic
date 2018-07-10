Imports Microsoft.SqlServer.Management.Common
Imports Microsoft.SqlServer.Management.Sdk.Sfc
Imports Microsoft.SqlServer.Management.Smo

Namespace Classes
    Public Class TableInformation
        ''' <summary>
        ''' Determine if a table exists by name in a database using the default instance of SQL-Server
        ''' </summary>
        ''' <param name="pDatabaseName">Existing database</param>
        ''' <param name="pTableName">Table to deterime if it exists or not</param>
        ''' <returns>An instance of TableDetails with .Exists property set</returns>
        Public Function TableExists(pDatabaseName As String, pTableName As String) As TableDetails
            Dim result As New TableDetails With {.Exists = False}

            Dim srv = New Server
            Dim database = srv.Databases.OfType(Of Database)().FirstOrDefault(Function(tbl) tbl.Name = pDatabaseName)

            If database IsNot Nothing Then

                result.Exists = (database.Tables.OfType(Of Table)().
                                     Where(Function(tbl) (Not tbl.IsSystemObject)).
                                     FirstOrDefault(Function(tbl) tbl.Name = pTableName) IsNot Nothing)
            End If

            Return result

        End Function
        ''' <summary>
        ''' Determine if a table exists by name in a database using a specific server name
        ''' </summary>
        ''' <param name="pServer">Specific SQL-Server name</param>
        ''' <param name="pDatabaseName">Existing database</param>
        ''' <param name="pTableName">Table to deterime if it exists or not</param>
        ''' <returns>An instance of TableDetails with .Exists property set</returns>
        Public Function TableExists(pServer As String, pDatabaseName As String, pTableName As String) As TableDetails
            Dim result As New TableDetails With {.Exists = False}

            Dim srv = New Server(pServer)

            Dim database = srv.Databases.OfType(Of Database)().FirstOrDefault(Function(tbl) tbl.Name = pDatabaseName)

            If database IsNot Nothing Then

                result.Exists = (database.Tables.OfType(Of Table)().
                                     Where(Function(tbl) (Not tbl.IsSystemObject)).
                                     FirstOrDefault(Function(tbl) tbl.Name = pTableName) IsNot Nothing)
            End If

            Return result

        End Function
        Public Function ColumnExists(pDatabaseName As String, pTableName As String, pColumnName As String) As Boolean
            Dim srv = New Server
            Dim exists As Boolean = False

            Dim database = srv.Databases.OfType(Of Database)().FirstOrDefault(Function(db) db.Name = pDatabaseName)

            If database IsNot Nothing Then
                Dim table = database.Tables.OfType(Of Table)().FirstOrDefault(Function(tbl) tbl.Name = pTableName)
                If table IsNot Nothing Then
                    exists = (table.Columns.OfType(Of Column)().FirstOrDefault(Function(col) col.Name = pColumnName) IsNot Nothing)
                End If
            End If

            Return exists

        End Function
        ''' <summary>
        ''' Obtain column details for an existing table specifying the default instance of SQL-SDerver
        ''' </summary>
        ''' <param name="pDatabaseName">Existing database</param>
        ''' <param name="pTableName">Existing table</param>
        ''' <returns></returns>
        Public Function GetColumnDetails(pDatabaseName As String, pTableName As String) As List(Of ColumnDetails)
            Dim srv = New Server
            Dim columnDetails = New List(Of ColumnDetails)()

            Dim database = srv.Databases.OfType(Of Database)().
                    FirstOrDefault(Function(db) db.Name = pDatabaseName)

            If database IsNot Nothing Then

                Dim table = database.Tables.OfType(Of Table)().
                        FirstOrDefault(Function(tbl) tbl.Name = pTableName)

                If table IsNot Nothing Then

                    columnDetails = table.Columns.OfType(Of Column)().
                        Select(Function(col) New ColumnDetails() With
                                  {
                                      .Identity = col.Identity,
                                      .DataType = col.DataType,
                                      .Name = col.Name,
                                      .InPrimaryKey = col.InPrimaryKey,
                                      .Nullable = col.Nullable
                                  }
                        ).ToList()

                End If
            End If

            Return columnDetails

        End Function
        ''' <summary>
        ''' Obtain column details for an existing table specifying the SQL-Server instance name
        ''' </summary>
        ''' <param name="pServer">Available server</param>
        ''' <param name="pDatabaseName">Existing database</param>
        ''' <param name="pTableName">Existing table</param>
        ''' <returns></returns>
        Public Function GetColumnDetails(pServer As String, pDatabaseName As String, pTableName As String) As List(Of ColumnDetails)
            Dim srv = New Server(pServer)
            Dim columnDetails = New List(Of ColumnDetails)()

            Dim database = srv.Databases.OfType(Of Database)().FirstOrDefault(Function(db) db.Name = pDatabaseName)

            If database IsNot Nothing Then
                Dim table = database.Tables.OfType(Of Table)().FirstOrDefault(Function(tbl) tbl.Name = pTableName)

                If table IsNot Nothing Then

                    columnDetails = table.Columns.OfType(Of Column)().
                        Select(Function(col) New ColumnDetails() With
                                  {
                                      .Identity = col.Identity,
                                      .DataType = col.DataType,
                                      .Name = col.Name,
                                      .InPrimaryKey = col.InPrimaryKey,
                                      .Nullable = col.Nullable
                                  }
                        ).ToList()

                End If
            End If

            Return columnDetails

        End Function
        Public Function TableKeys(pDatabaseName As String, pTableName As String) As List(Of ForeignKeysDetails)
            Dim srv = New Server()
            Dim keyList = New List(Of ForeignKeysDetails)()
            Dim database = srv.Databases.OfType(Of Database)().FirstOrDefault(Function(db) db.Name = pDatabaseName)
            If database IsNot Nothing Then
                Dim table = database.Tables.OfType(Of Table)().FirstOrDefault(Function(tbl) tbl.Name = pTableName)
                If table IsNot Nothing Then

                    ' ReSharper disable once LoopCanBeConvertedToQuery
                    For Each item As Column In table.Columns.OfType(Of Column)()


                        Dim fkds As List(Of ForeignKeysDetails) = item.EnumForeignKeys().
                                AsEnumerable().
                                Select(Function(row) New ForeignKeysDetails With
                                          {
                                              .TableSchema = row.Field(Of String)("Table_Schema"),
                                              .TableName = row.Field(Of String)("Table_Name"),
                                              .SchemaName = row.Field(Of String)("Name")
                                          }
                                ).ToList()

                        For Each ts As ForeignKeysDetails In fkds
                            keyList.Add(ts)
                        Next

                    Next
                End If
            End If

            Return keyList

        End Function
        ''' <summary>
        ''' Get a Table by server, database name, table name
        ''' </summary>
        ''' <param name="pServer">SQL-Server</param>
        ''' <param name="pDatabaseName">Database name in pServer</param>
        ''' <param name="pTableName">Table name in pDatabaseName</param>
        ''' <returns></returns>
        Public Function GetTableByExactName(pServer As Server, pDatabaseName As String, pTableName As String) As Table
            Dim tblResult As Table = Nothing
            Dim database = pServer.Databases.OfType(Of Database)().
                    FirstOrDefault(Function(tbl) tbl.Name = pDatabaseName)

            If database IsNot Nothing Then

                tblResult = database.Tables.OfType(Of Table)().
                    Where(Function(tbl) (Not tbl.IsSystemObject)).
                    Select(Function(tbl) tbl).
                    FirstOrDefault(Function(x) x.Name = pTableName)

            End If

            Return tblResult

        End Function
        Public Function GetTableByContainingToken(pServer As Server, pDatabaseName As String, pPartialTableName As String) As Table
            Dim tblResult As Table = Nothing

            Dim database = pServer.Databases.OfType(Of Database)().
                    FirstOrDefault(Function(tbl) tbl.Name = pDatabaseName)

            If database IsNot Nothing Then

                tblResult = database.Tables.OfType(Of Table)().
                    Where(Function(tbl) (Not tbl.IsSystemObject)).
                    Select(Function(tbl) tbl).
                    FirstOrDefault(Function(x) x.Name.Contains(pPartialTableName))

            End If

            Return tblResult

        End Function

    End Class
End Namespace