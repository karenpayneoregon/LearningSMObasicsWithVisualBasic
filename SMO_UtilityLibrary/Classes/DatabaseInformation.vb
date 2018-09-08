Imports System.IO
Imports Microsoft.SqlServer.Management.Smo

Namespace Classes
    Public Class DatabaseInformation
        Inherits BaseExceptionsHandler

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

            mHasException = False

            Dim srv = New Server
            Dim db As Database

            Try
                db = srv.Databases(pOriginalDatabase)
                Dim dbCopy As Database
                dbCopy = New Database(srv, pNewDatabase)
                dbCopy.Create()

                Dim trans As Transfer
                trans = New Transfer(db)
                trans.CopyAllTables = True
                trans.Options.WithDependencies = True
                trans.Options.ContinueScriptingOnError = True
                trans.DestinationDatabase = pNewDatabase
                trans.DestinationServer = srv.Name
                trans.DestinationLoginSecure = True
                trans.Options.DriAllKeys = True
                trans.CopySchema = True

                trans.TransferData()

                Return True

            Catch ex As Exception
                mHasException = True
                mLastException = ex
                Return False
            End Try

        End Function
        ''' <summary>
        ''' Using the named server copy one database to a new database with tables,
        ''' data, primary keys
        ''' </summary>
        ''' <param name="pServer">Existing SQL-Server instance name</param>
        ''' <param name="pOriginalDatabase">Existing database</param>
        ''' <param name="pNewDatabase">Non-existing database</param>
        ''' <returns>True on successully completing the copy, false on failure</returns>
        Public Function CopyDatabase(pServer As String, pOriginalDatabase As String, pNewDatabase As String) As Boolean

            mHasException = False

            Dim srv = New Server(pServer)
            Dim db As Database

            Try
                db = srv.Databases(pOriginalDatabase)
                Dim dbCopy As Database
                dbCopy = New Database(srv, pNewDatabase)
                dbCopy.Create()

                Dim trans As Transfer
                trans = New Transfer(db)
                trans.CopyAllTables = True
                trans.Options.WithDependencies = True
                trans.Options.ContinueScriptingOnError = True
                trans.DestinationDatabase = pNewDatabase
                trans.DestinationServer = srv.Name
                trans.DestinationLoginSecure = True
                trans.Options.DriAllKeys = True
                trans.CopySchema = True

                trans.TransferData()

                Return True

            Catch ex As Exception
                mHasException = True
                mLastException = ex
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

            'Console.WriteLine(srv.InstallDataDirectory)
            'Console.WriteLine(srv.ConnectionContext.DatabaseEngineType.ToString())

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
        ''' If database exists on default server drop the database, for this demo
        ''' we are using a hard-coded database on the default server.
        ''' </summary>
        ''' <returns></returns>
        Public Function WhenDatabaseExistsDrop(pDatabaseName As String) As DatabaseExistsResult
            Try
                Dim srv = New Server()
                Dim result As New DatabaseDetails With {.Exists = False}
                Dim db = srv.Databases(pDatabaseName)

                If db IsNot Nothing Then
                    result.Exists = True
                    result.Name = pDatabaseName
                    result.Database = db
                    srv.Databases(pDatabaseName).Drop()

                    Return DatabaseExistsResult.Dropped
                Else
                    Return DatabaseExistsResult.DropNotRequired
                End If

            Catch ex As Exception
                '
                ' Common reason to land here is the database is "in use"
                ' which means it can not be dropped. Now it's possible to
                ' force remove a database via 
                '
                '    srv.KillDatabase(pDatabaseName)
                '
                ' yet that in many cases would be rude if someone is using the
                ' database and this code dropped it.
                '
                ' See the following http://www.blackwasp.co.uk/SQLRestrictedUser.aspx
                '
                mHasException = True
                mLastException = ex
                Return DatabaseExistsResult.ExceptionThrown
            End Try


        End Function
        ''' <summary>
        ''' This code sample shows how to create and drop a table with events.
        ''' If the database exists it is dropped, no prompting.
        ''' 
        ''' On table create an event is raised indicating a table was created
        ''' while a message is shown when the table is dropped
        ''' </summary>
        ''' <remarks>
        ''' * Alternate is to use TSQL script to do the same work, here the advantage
        '''   for some is being able to inspect/alter properties when creating or after
        '''   creating.
        ''' * What can go wrong: There is a live connection on the database which would
        '''   cause the drop method to fail, in this case raise an exception which here
        '''   is remembered and sent back to the calling method within the form.
        ''' </remarks>
        Public Function CreateAndDropTableWithEvents(pDatabaseName As String) As Boolean

            mHasException = False

            Dim dropResults = WhenDatabaseExistsDrop(pDatabaseName)
            Dim srv = New Server()

            If dropResults = DatabaseExistsResult.Dropped OrElse dropResults = DatabaseExistsResult.DropNotRequired Then

                Dim db As New Database(srv, pDatabaseName)
                'Define a Schema object variable by supplying the parent database and name arguments in the constructor.
                'this is used in DemoTable below.
                Dim schema As Schema
                schema = New Schema(db, "kp")
                schema.Owner = "dbo"

                db.Create()

                'Create the schema on the instance of SQL Server.
                schema.Create()

            Else
                Return False
            End If

            Dim database = srv.Databases(pDatabaseName)

            Dim databaseCreateEventSet As New DatabaseEventSet
            databaseCreateEventSet.CreateTable = True
            databaseCreateEventSet.DropTable = True

            Dim serverCreateEventHandler As ServerEventHandler
            serverCreateEventHandler = New ServerEventHandler(AddressOf CreateDropTableEventHandler)

            'Subscribe to the first server event handler when a CreateTable event occurs.
            database.Events.SubscribeToEvents(databaseCreateEventSet, serverCreateEventHandler)
            database.Events.StartEvents()

            'Create a table on the database.
            'Create three most populate field types, primary key; integer, string field, date field
            Dim tb As Table
            tb = New Table(database, "DemoTable")

            Dim primaryIdentifierColumn As New Column(tb, "ID", DataType.Int)
            primaryIdentifierColumn.Identity = True
            primaryIdentifierColumn.IdentitySeed = 1
            primaryIdentifierColumn.Nullable = False
            tb.Columns.Add(primaryIdentifierColumn)

            Dim nameColumn As Column
            nameColumn = New Column(tb, "Name", DataType.NChar(50))
            nameColumn.Collation = "Latin1_General_CI_AS"
            nameColumn.Nullable = True
            tb.Columns.Add(nameColumn)

            Dim joinDateColumn As New Column(tb, "JoinedDate", DataType.DateTime)
            joinDateColumn.AddDefaultConstraint() ' you can specify constraint name here as well
            joinDateColumn.DefaultConstraint.Text = "GETDATE()"
            tb.Columns.Add(joinDateColumn)

            ' Add primary key index to the table
            Dim primaryKeyIndex As New Index(tb, "PK_TestTableIdentifier")
            primaryKeyIndex.IndexKeyType = IndexKeyType.DriPrimaryKey
            primaryKeyIndex.IndexedColumns.Add(New IndexedColumn(primaryKeyIndex, "ID"))
            tb.Indexes.Add(primaryKeyIndex)

            tb.Schema = "kp"
            tb.Create()

            '
            ' Read script to insert serveral record from disk followed by performing the inserts/
            '
            Try
                database.ExecuteNonQuery(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "DemoTableRecord.txt")))
            Catch ex As Exception
                mHasException = True
                mLastException = ex
                database.Events.StopEvents()
            End Try

            Try
                'Remove the table. 
                tb.Drop()
                ' drop database
                database.Drop()

                'Wait until the events have occured.
                Dim dummy As Integer
                For outer = 1 To 1000000000
                    dummy = outer * 2
                Next

            Catch ex As Exception
                mHasException = True
                mLastException = ex
            Finally
                'Stop event handling listening
                database.Events.StopEvents()
            End Try

            Return True

        End Function
        ''' <summary>
        ''' Given an existing database with tables (best with data)
        ''' will generate a text file for each table that has insert
        ''' statements with data.
        ''' 
        ''' Requirements
        '''     Database exists (replace NorthWindAzure with your database)
        ''' </summary>
        Public Function ScriptDatabaseTables() As List(Of String)
            Dim fileNames As New List(Of String) From {"Tables for NorthWindAzure", ""}
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
                fileNames.Add(Path.GetFileName(scrp.Options.FileName))
                scrp.EnumScript(New SqlSmoObject() {table})
            Next

            Return fileNames

        End Function
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