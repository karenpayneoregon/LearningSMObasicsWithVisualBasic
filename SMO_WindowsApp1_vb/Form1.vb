Imports Microsoft.SqlServer.Management.Smo
Imports SMO_UtilityLibrary
Imports SMO_UtilityLibrary.Classes

Public Class Form1

    ''' <summary>
    ''' Get all available servers
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub cmdGetAvailableServers_Click(sender As Object, e As EventArgs) Handles cmdGetAvailableServers.Click
        Dim serverOperations = New ServerInformation

        ListBox1.DataSource = Await serverOperations.GetServersAsync()
        ListBox1.DisplayMember = "Name"

    End Sub
    ''' <summary>
    ''' Get root folder where SQL-Server has been installed for the default server,
    ''' use the overloaded version of SqlServerInstallPath for a specific server 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdSqlServerInstalledPath_Click(sender As Object, e As EventArgs) Handles cmdSqlServerInstalledPath.Click
        Dim serverOperations = New ServerInformation
        Console.WriteLine(serverOperations.SqlServerInstallPath())
    End Sub
    Private Sub cmdGetAllDatabases_Click(sender As Object, e As EventArgs) Handles cmdGetAllDatabases.Click
        Dim serverOperations = New ServerInformation
        Dim databaseList = serverOperations.GetDatabases("KARENS-PC")

        lstDatabases.DataSource = databaseList.Select(Function(db) db.Name).ToList()


    End Sub
    ''' <summary>
    ''' Get all databases and tables for each database
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdGetAllDatabasesAndTables_Click(sender As Object, e As EventArgs) Handles cmdGetAllDatabasesAndTables.Click
        Dim serverOperations = New ServerInformation

        '
        ' Change KARENS-PC to your server name
        '
        Dim dbsTables As List(Of DatabaseAndTables) = serverOperations.GetAllDatabasesAndTables("KARENS-PC")

        If dbsTables.Count > 0 Then
            For Each o As DatabaseAndTables In dbsTables
                Console.WriteLine(o.DatabaseName)
                For Each tableName As String In o.TableNameList
                    Console.WriteLine($"  {tableName}")
                Next
            Next
        Else
            Console.WriteLine("No databases thus no tables")
        End If
    End Sub
    ''' <summary>
    ''' Get an existing database, in this case the variable databaseName needs to point to an existing database
    ''' and the server is the default server
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdGetDatabaseThatExistsDefaultServer_Click(sender As Object, e As EventArgs) Handles cmdGetDatabaseThatExistsDefaultServer.Click
        '
        ' Change to an existing database
        '
        Dim databaseName = "NorthWindAzure"
        Dim databaseOperations = New DatabaseInformation

        Dim results = databaseOperations.GetDatabase(databaseName)
        If results.Exists Then
            Console.WriteLine($"Created: {results.CreationDateTime()} Last backed up: {results.LastBackupDate}")
        Else
            Console.WriteLine("Not found")
        End If
    End Sub

    ''' <summary>
    ''' Get an existing database, in this case the variable databaseName needs to point to an existing database
    ''' and the server (available) name is specified
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdGetDatabaseThatExistsNamedServer_Click(sender As Object, e As EventArgs) Handles cmdGetDatabaseThatExistsNamedServer.Click
        '
        ' Change KARENS-PC to your server name
        '
        Dim serverName = "KARENS-PC"
        '
        ' Change to an existing database
        '
        Dim databaseName = "NorthWindAzure"
        Dim databaseOperations = New DatabaseInformation

        Dim results = databaseOperations.GetDatabase(serverName, databaseName)
        If results.Exists Then
            Console.WriteLine($"Created: {results.CreationDateTime()} Last backed up: {results.LastBackupDate}")
        Else
            Console.WriteLine("Not found")
        End If
    End Sub

    ''' <summary>
    ''' Get an existing database, in this case the variable databaseName needs to point to a non-existing database
    ''' and the server is the default server
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdGetDatabaseThatDoesNotExists_Click(sender As Object, e As EventArgs) Handles cmdGetDatabaseThatDoesNotExists.Click
        '
        ' If you really have this database, wow, change it to a non-existing database
        '
        Dim databaseName = "AnimalCookieDatabase"
        Dim databaseOperations = New DatabaseInformation
        Dim results = databaseOperations.GetDatabase(databaseName)
        If results.Exists Then
            Console.WriteLine($"Created: {results.CreationDateTime()} Last backed up: {results.LastBackupDate}")
        Else
            Console.WriteLine("Not found")
        End If
    End Sub

    Private Sub cmdGetTablesForExistingDatabaseHasTables_Click(sender As Object, e As EventArgs) Handles cmdGetTablesForExistingDatabaseHasTables.Click
        '
        ' Change to an existing database
        '
        Dim databaseName = "NorthWindAzure"
        Dim databaseOperations = New DatabaseInformation

        Dim result = databaseOperations.TableNames(databaseName)
        If result.Exists AndAlso result.HasTables Then
            Console.WriteLine("Yes")
        Else
            Console.WriteLine("No")
        End If
    End Sub

    Private Sub cmdGetTablesForExistingDatabaseHasNoTables_Click(sender As Object, e As EventArgs) Handles cmdGetTablesForExistingDatabaseHasNoTables.Click
        '
        ' Change if you have a database named Test with tables to a database with no tables.
        '
        Dim databaseName = "Test"
        Dim databaseOperations = New DatabaseInformation

        Dim result = databaseOperations.TableNames(databaseName)
        If result.Exists AndAlso result.HasTables Then
            Console.WriteLine("Yes")
        Else
            Console.WriteLine("No")
        End If
    End Sub
    Private Sub cmdTableExistFound_Click(sender As Object, e As EventArgs) Handles cmdTableExistFound.Click
        '
        ' NorthWindAzure must exists and Customers must exists
        '
        Dim ops As New TableInformation
        If ops.TableExists("NorthWindAzure", "Customers").Exists Then
            Console.WriteLine("Found")
        Else
            Console.WriteLine("Not found")
        End If
    End Sub

    Private Sub cmdTableExistNotFound_Click(sender As Object, e As EventArgs) Handles cmdTableExistNotFound.Click
        '
        ' NorthWindAzure must exists and Customers must not exists
        '
        Dim ops As New TableInformation
        If ops.TableExists("NorthWindAzure", "ABC_123").Exists Then
            Console.WriteLine("Found")
        Else
            Console.WriteLine("Not found")
        End If



    End Sub

    Private Sub cmdColumnExistsFound_Click(sender As Object, e As EventArgs) Handles cmdColumnExistsFound.Click
        '
        ' NorthWindAzure must exists, Customers must exists, ContactName must exist
        '
        Dim ops As New TableInformation
        If ops.ColumnExists("NorthWindAzure", "Customers", "ContactName") Then
            Console.WriteLine("Found")
        Else
            Console.WriteLine("Not found")
        End If
    End Sub

    Private Sub cmdColumnExistNotFound_Click(sender As Object, e As EventArgs) Handles cmdColumnExistNotFound.Click
        '
        ' NorthWindAzure must exists, Customers must exists, ContactGuitar must not exist
        '
        Dim ops As New TableInformation
        If ops.ColumnExists("NorthWindAzure", "Customers", "ContactGuitar") Then
            Console.WriteLine("Found")
        Else
            Console.WriteLine("Not found")
        End If
    End Sub

    Private Sub cmdColumnDetails_Click(sender As Object, e As EventArgs) Handles cmdColumnDetails.Click
        '
        ' NorthWindAzure must exists, Customers must exists with columns
        '
        Dim ops As New TableInformation
        Dim detailResults = ops.GetColumnDetails("NorthWindAzure", "Customers")
        '
        ' See if there is an identity column, if so show the name
        '
        Dim identityColumn = detailResults.FirstOrDefault(Function(item) item.Identity)
        If identityColumn IsNot Nothing Then
            Console.WriteLine(identityColumn.Name)
        End If

    End Sub

    Private Sub cmdTableKeys_Click(sender As Object, e As EventArgs) Handles cmdTableKeys.Click
        Dim ops As New TableInformation
        '
        ' NorthWindAzure must exists, Customers must exists with columns
        '
        Dim result = ops.TableKeys("NorthWindAzure", "Customers")
        If result.Count > 0 Then
            Dim firstKey = result.FirstOrDefault()
            Console.WriteLine($"Shema name: {firstKey.SchemaName} = {firstKey.TableSchema}.{firstKey.TableName}")
        End If
    End Sub

    Private Sub lstDatabases_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDatabases.SelectedIndexChanged
        txtDatabaseCopyName.Text = $"{lstDatabases.Text}_Copy"
    End Sub

    Private Sub cmdCopyDatabase_Click(sender As Object, e As EventArgs) Handles cmdCopyDatabase.Click
        Dim ops As New DatabaseInformation
        ops.CopyDatabase(lstDatabases.Text, txtDatabaseCopyName.Text)
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim serverOperations = New ServerInformation
        lstDatabases.DataSource = serverOperations.GetDatabaseNames("KARENS-PC")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ops As New DatabaseInformation
        AddHandler ops.SmoTableCreate, AddressOf CreateTableEvent

        ops.CreateAndDropTableWithEvents()

    End Sub
    Public Sub CreateTableEvent(sender As Object, e As SmoTableCreateDropArgs)
        MessageBox.Show(e.Message)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim ops As New DatabaseInformation
        ops.ScriptDatabaseTables()
    End Sub
End Class
