Imports Microsoft.SqlServer.Management.Smo

Namespace Classes
    Public Class ServerInformation
        ''' <summary>
        ''' Get installation path of SQL-Server for the default instance
        ''' </summary>
        ''' <returns></returns>
        Public Function SqlServerInstallPath() As String
            Dim srv = New Server
            Return srv.RootDirectory
        End Function
        ''' <summary>
        ''' Get installation path of SQL-Server for a specific server
        ''' </summary>
        ''' <param name="pServerName"></param>
        ''' <returns></returns>
        Public Function SqlServerInstallPath(pServerName As String) As String
            Dim srv = New Server(pServerName)
            Return srv.RootDirectory
        End Function
        ''' <summary>
        ''' Return available servers. As this can take time this method
        ''' is asnc to keep the caller responsive
        ''' </summary>
        ''' <returns></returns>
        Public Async Function GetServersAsync() As Task(Of List(Of String))
            Dim serverNames As New List(Of String)

            Await Task.Run(
                Sub()
                    serverNames = SmoApplication.
                                EnumAvailableSqlServers(True).
                                AsEnumerable().
                                Select(Function(row) row.Field(Of String)("Name")).
                                ToList()
                End Sub)

            Return serverNames

        End Function
        ''' <summary>
        ''' Get an instance of a SQL-Server
        ''' </summary>
        ''' <param name="pServerName">Server name</param>
        ''' <param name="pLoadDatabases">True to populate Databases</param>
        ''' <returns>ServerDetails - on Success .Exists is true, failure .Exists = false</returns>
        ''' <remarks>
        ''' This method assumes the server exists and does a check on Database.Count, if the server is
        ''' not available this will throw an exception. The other way to handle this is to call GetServers
        ''' and determine if the server name is listed. The downside is calling GetServers takes time to
        ''' execute.
        ''' 
        ''' Setting pLoadDatabases to true (default) will load the property Databases which may not always be
        ''' why this method is called
        ''' </remarks>
        Public Function GetServer(pServerName As String, Optional pLoadDatabases As Boolean = True) As ServerDetails

            Dim serverDetails As New ServerDetails
            Dim srv = New Server(pServerName)

            Try
                serverDetails.Name = pServerName
                serverDetails.Server = srv
                serverDetails.Exists = True

                If pLoadDatabases Then
                    serverDetails.Databases = srv.Databases
                End If

            Catch ex As Exception

                If ex.Message.Contains("Failed to connect to server") Then
                    serverDetails.Exists = False
                    serverDetails.Exception = ex
                End If

            End Try

            Return serverDetails

        End Function
        ''' <summary>
        ''' Get all databases for the default server
        ''' </summary>
        ''' <param name="pServerName"></param>
        ''' <returns></returns>
        Public Function GetDatabases(pServerName As String) As List(Of Database)

            Dim srvDetails = GetServer(pServerName, True)
            Dim excludeDatabaseNames As String() = {"master", "model", "msdb"}

            Dim result As New List(Of Database)


            If srvDetails.Exists Then

                If srvDetails.Databases IsNot Nothing Then
                    For Each db As Database In srvDetails.Databases
                        If Not excludeDatabaseNames.Contains(db.Name) Then
                            result.Add(db)
                        End If
                    Next
                End If

            End If

            Return result

        End Function
        ''' <summary>
        ''' Helper function to get database names for the default server
        ''' </summary>
        ''' <param name="pServerName"></param>
        ''' <returns></returns>
        Public Function GetDatabaseNames(pServerName As String) As List(Of String)
            Return GetDatabases(pServerName).Select(Function(db) db.Name).ToList()
        End Function
        Public Function GetAllDatabasesAndTables(pServerName As String, Optional pLoadDatabases As Boolean = True) As List(Of DatabaseAndTables)

            Dim srvDetails = GetServer(pServerName, pLoadDatabases)
            Dim excludeDatabaseNames As String() = {"master", "model", "msdb"}

            Dim result = New List(Of DatabaseAndTables)

            If srvDetails.Exists Then
                If srvDetails.Databases IsNot Nothing Then

                    For Each db As Database In srvDetails.Databases

                        If Not excludeDatabaseNames.Contains(db.Name) Then

                            Dim item As New DatabaseAndTables With {.DatabaseName = db.Name, .TableNameList = New List(Of String)}
                            For Each tbl As Table In db.Tables
                                item.TableNameList.Add(tbl.Name)
                            Next

                            result.Add(item)

                        End If

                    Next

                End If
            End If

            Return result

        End Function
    End Class

End Namespace