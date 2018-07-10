Imports System.Collections.Specialized
Imports System.IO
Imports System.Text
Imports Microsoft.SqlServer.Management.Sdk.Sfc
Imports Microsoft.SqlServer.Management.Smo
Imports Microsoft.SqlServer.SmoExtended

Namespace Classes
    Public Class DatabaseInformation

        ''' <summary>
        ''' Get database on default server
        ''' </summary>
        ''' <param name="pDatabaseName">Database name</param>
        ''' <returns></returns>
        Public Function GetDatabase(pDatabaseName As String) As DatabaseDetails
            Dim result As New DatabaseDetails With {.Exists = False}
            Dim srv = New Server
            Dim db = srv.Databases(pDatabaseName)

            If db IsNot Nothing Then
                result.Exists = True
                result.Name = pDatabaseName
                result.Database = db
            End If

            Return result

        End Function
        ''' <summary>
        ''' Using the default server copy one database to a new database with tables,
        ''' data, primary keys
        ''' </summary>
        ''' <param name="pOriginalDatabase">Existing database</param>
        ''' <param name="pNewDatabase">Non-existing database</param>
        ''' <returns>True on successully completing the copy, false on failure</returns>
        Public Function CopyDatabase(pOriginalDatabase As String, pNewDatabase As String) As Boolean
            Dim srv As Server = New Server
            Dim db As Database

            Try
                db = srv.Databases(pOriginalDatabase)
                Dim dbCopy As Database
                dbCopy = New Database(srv, pNewDatabase)
                dbCopy.Create()

                Dim xfr As Transfer
                xfr = New Transfer(db)
                xfr.CopyAllTables = True
                xfr.Options.WithDependencies = True
                xfr.Options.ContinueScriptingOnError = True
                xfr.DestinationDatabase = pNewDatabase
                xfr.DestinationServer = srv.Name
                xfr.DestinationLoginSecure = True
                xfr.Options.DriAllKeys = True
                xfr.CopySchema = True

                xfr.TransferData()
                Return True
            Catch ex As Exception
                Return False
            End Try

        End Function
        ''' <summary>
        ''' Get database on specified server
        ''' </summary>
        ''' <param name="pServer">Server name</param>
        ''' <param name="pDatabaseName">Database name</param>
        ''' <returns></returns>
        Public Function GetDatabase(pServer As String, pDatabaseName As String) As DatabaseDetails

            Dim result As New DatabaseDetails With {.Exists = False}
            Dim srv = New Server(pServer)

            Console.WriteLine(srv.InstallDataDirectory)
            Console.WriteLine(srv.ConnectionContext.DatabaseEngineType.ToString())
            Dim db = srv.Databases(pDatabaseName)

            If db IsNot Nothing Then
                result.Exists = True
                result.Name = pDatabaseName
                result.Database = db
            End If

            Return result

        End Function
        ''' <summary>
        ''' Get table names using the default server and database name
        ''' </summary>
        ''' <param name="pDatabaseName">Existing database in pServer</param>
        ''' <returns>TableDetails object indicating if there the database exists and has tables</returns>
        Public Function TableNames(pDatabaseName As String) As TableDetails

            Dim result As New TableDetails With {.Exists = False, .DatabaseName = pDatabaseName}
            Dim srv = New Server()
            Dim database = srv.Databases(pDatabaseName)

            If database IsNot Nothing Then

                result.ServerName = srv.Name
                result.Exists = True

                result.NameList = database.Tables.OfType(Of Table)().
                    Where(Function(tbl) (Not tbl.IsSystemObject)).
                    Select(Function(tbl) tbl.Name).ToList()

            End If

            Return result

        End Function
        ''' <summary>
        ''' This code sample shows how to create and drop a table with events.
        ''' Requirements
        ''' 1. The database exists.
        ''' 2. The table does not exists.
        ''' 
        ''' On table create an event is raised indicating a table was created
        ''' while a message is shown when the table is dropped
        ''' </summary>
        Public Sub CreateAndDropTableWithEvents()
            Dim srv = New Server()
            Dim database = srv.Databases("CreatedInVisualStudio")

            Dim databaseCreateEventSet As New DatabaseEventSet
            databaseCreateEventSet.CreateTable = True
            databaseCreateEventSet.DropTable = True

            Dim serverCreateEventHandler As ServerEventHandler
            serverCreateEventHandler = New ServerEventHandler(AddressOf CreateDropTableEventHandler)

            'Subscribe to the first server event handler when a CreateTable event occurs.
            database.Events.SubscribeToEvents(databaseCreateEventSet, serverCreateEventHandler)
            database.Events.StartEvents()

            'Create a table on the database.
            Dim tb As Table
            tb = New Table(database, "Test_Table")
            Dim mycol1 As Column
            mycol1 = New Column(tb, "Name", DataType.NChar(50))
            mycol1.Collation = "Latin1_General_CI_AS"
            mycol1.Nullable = True
            tb.Columns.Add(mycol1)
            tb.Create()

            'Remove the table.
            tb.Drop()

            'Wait until the events have occured.
            Dim outer As Integer
            Dim inner As Integer
            For outer = 1 To 1000000000
                inner = outer * 2
            Next

            'Stop event handling listening
            database.Events.StopEvents()

        End Sub
        ''' <summary>
        ''' Given an existing database with tables (best with data)
        ''' will generate a text file for each table that has insert
        ''' statements with data.
        ''' 
        ''' Requirements
        '''     Database exists (replace NorthWindAzure with your database)
        ''' </summary>
        Public Sub ScriptDatabaseTables()
            ' uses default server
            Dim srv = New Server()

            Dim scrp As New Scripter With {
                .Server = srv
            }

            scrp.Options.ScriptData = True
            scrp.Options.ScriptSchema = False
            scrp.Options.ToFileOnly = True

            Dim database = srv.Databases("NorthWindAzure")

            Dim tables = database.Tables.OfType(Of Table)().
                    Where(Function(tbl) (Not tbl.IsSystemObject))

            For Each table As Table In tables
                scrp.Options.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, table.Name & ".txt")
                scrp.EnumScript(New SqlSmoObject() {table})
            Next

        End Sub
        ''' <summary>
        ''' Used in CreateAndDropTableWithEvents to handling pushing results
        ''' of create and drop operations by EventType
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CreateDropTableEventHandler(sender As Object, e As ServerEventArgs)
            Dim args As New SmoTableCreateDropArgs
            Dim message As String = $"In database: {e.Properties("DatabaseName").Value} a {e.Properties("ObjectType").Value} " &
                                    $"named {e.Properties("ObjectName").Value} was "

            If e.EventType = EventType.CreateTable Then
                args.Message = message & "created"
                OnSmoTableCreate(args)
            ElseIf e.EventType = EventType.DropTable Then
                args.Message = message & "dropped"
                OnSmoTableCreate(args)
            End If

        End Sub
        Public Event SmoTableCreate As EventHandler(Of SmoTableCreateDropArgs)
        Public Sub OnSmoTableCreate(e As SmoTableCreateDropArgs)
            RaiseEvent SmoTableCreate(Me, e)
        End Sub
        ''' <summary>
        ''' Get table names using a named server and database name
        ''' </summary>
        ''' <param name="pServer">Available SQL-Server name</param>
        ''' <param name="pDatabaseName">Existing database in pServer</param>
        ''' <returns>TableDetails object indicating if there the database exists and has tables</returns>
        Public Function TableNames(pServer As String, pDatabaseName As String) As TableDetails

            Dim result As New TableDetails With {.Exists = False, .ServerName = pServer, .DatabaseName = pDatabaseName}
            Dim srv = New Server(pServer)
            Dim database = srv.Databases(pDatabaseName)

            If database IsNot Nothing Then

                result.Exists = True

                result.NameList = database.Tables.OfType(Of Table)().
                    Where(Function(tbl) (Not tbl.IsSystemObject)).
                    Select(Function(tbl) tbl.Name).ToList()


            End If

            Return result

        End Function
    End Class
End Namespace