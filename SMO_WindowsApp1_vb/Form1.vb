﻿Imports System.IO
Imports System.Text
Imports Microsoft.SqlServer.Management.Smo
Imports SMO_UtilityLibrary
Imports SMO_UtilityLibrary.Classes

Public Class Form1
    Private _serverNames As List(Of String)
    ''' <summary>
    ''' Get all available servers
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub cmdGetAvailableServers_Click(sender As Object, e As EventArgs) Handles cmdGetAvailableServers.Click

        MessageBox.Show("Press OK, will get servers asynchronous and keep this example UI responsive.")

        Await GetAvailableServerNames()

        ListBox1.DataSource = _serverNames
        ListBox1.DisplayMember = "Name"

        If ListBox1.Items.Count = 0 Then
            MessageBox.Show("Found no servers present.")
        End If
    End Sub
    Private Async Function GetAvailableServerNames() As Task
        Dim serverOperations = New ServerInformation
        _serverNames = Await serverOperations.GetServersAsync()
    End Function
    ''' <summary>
    ''' Get root folder where SQL-Server has been installed for the default server,
    ''' use the overloaded version of SqlServerInstallPath for a specific server 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdSqlServerInstalledPath_Click(sender As Object, e As EventArgs) Handles cmdSqlServerInstalledPath.Click
        Dim serverOperations = New ServerInformation
        MessageBox.Show($"Server path is{Environment.NewLine}{serverOperations.SqlServerInstallPath()}")
    End Sub
    Private Sub cmdGetAllDatabases_Click(sender As Object, e As EventArgs) Handles cmdGetAllDatabases.Click

        If ListBox1.DataSource IsNot Nothing AndAlso ListBox1.Items.Count > 0 Then
            Dim serverOperations = New ServerInformation
            Dim databaseList = serverOperations.GetDatabases(ListBox1.Text)
            lstDatabases.DataSource = Nothing
            MessageBox.Show($"Getting all databases for server {ListBox1.Text}")
            lstDatabases.DataSource = databaseList.Select(Function(db) db.Name).ToList()
        Else
            MessageBox.Show("Please select the button 'Get available servers', allows servers to load, select a server and try again.")
            ActiveControl = cmdGetAvailableServers
        End If

    End Sub
    ''' <summary>
    ''' Get all databases and tables for each database
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdGetAllDatabasesAndTables_Click(sender As Object, e As EventArgs) Handles cmdGetAllDatabasesAndTables.Click
        If ListBox1.DataSource IsNot Nothing AndAlso ListBox1.Items.Count > 0 Then
            Dim serverOperations = New ServerInformation
            Dim dbsTables As List(Of DatabaseAndTables) = serverOperations.GetAllDatabasesAndTables(ListBox1.Text)

            If dbsTables.Count > 0 Then
                Dim sb As New StringBuilder
                For Each o As DatabaseAndTables In dbsTables
                    sb.AppendLine(o.DatabaseName)
                    For Each tableName As String In o.TableNameList
                        sb.AppendLine($"  {tableName}")
                    Next
                Next

                Dim f As New ResultsForm(sb.ToString())
                Try
                    f.ShowDialog()
                Finally
                    f.Dispose()
                End Try
            Else
                MessageBox.Show("No databases thus no tables")
            End If
        Else
            MessageBox.Show("Please select the button 'Get available servers', allows servers to load, select a server and try again.")
            ActiveControl = cmdGetAvailableServers
        End If

    End Sub
    ''' <summary>
    ''' Get an existing database, in this case the variable databaseName needs to point to an existing database
    ''' and the server is the default server so it's not selected via ListBox1 like other methods use.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdGetDatabaseThatExistsDefaultServer_Click(sender As Object, e As EventArgs) Handles cmdGetDatabaseThatExistsDefaultServer.Click

        '
        ' Change to an existing database
        '
        Dim databaseName = "NorthWindAzure"

        MessageBox.Show($"This method will look for '{databaseName}' on the default server.{Environment.NewLine}If '{databaseName}' does not exist this demo is invalid.{Environment.NewLine}Stop now and change the database to an existing database on the default server.")

        Dim databaseOperations = New DatabaseInformation

        Dim results = databaseOperations.GetDatabase(databaseName)
        If results.Exists Then
            MessageBox.Show($"{databaseName}' created: {results.CreationDateTime()}{Environment.NewLine}Last backed up: {results.LastBackupDate}")
        Else
            MessageBox.Show($"{databaseName}' Not found")
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

        MessageBox.Show($"If {serverName}.{databaseName} does not exists stop and modify the code for vaild server and database.")
        Dim databaseOperations = New DatabaseInformation

        Dim results = databaseOperations.GetDatabase(serverName, databaseName)
        If results.Exists Then
            MessageBox.Show($"{databaseName}' created: {results.CreationDateTime()}{Environment.NewLine}Last backed up: {results.LastBackupDate}")
        Else
            MessageBox.Show($"{databaseName}' Not found")
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
            MessageBox.Show($"Created: {results.CreationDateTime()} Last backed up: {results.LastBackupDate}")
        Else
            MessageBox.Show($"'{databaseName}' Not found")
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
            MessageBox.Show("Yes")
        Else
            MessageBox.Show("No")
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
            MessageBox.Show("Yes")
        Else
            MessageBox.Show("No")
        End If
    End Sub
    Private Sub cmdTableExistFound_Click(sender As Object, e As EventArgs) Handles cmdTableExistFound.Click
        MessageBox.Show("NorthWindAzure must exists and Customers must exists otherwise stop execution.")
        '
        ' NorthWindAzure must exists and Customers must exists
        '
        Dim ops As New TableInformation
        If ops.TableExists("NorthWindAzure", "Customers").Exists Then
            MessageBox.Show("Found")
        Else
            MessageBox.Show("Not found")
        End If
    End Sub

    Private Sub cmdTableExistNotFound_Click(sender As Object, e As EventArgs) Handles cmdTableExistNotFound.Click
        MessageBox.Show("NorthWindAzure must not exists and Customers must not exists otherwise stop execution.")
        '
        ' NorthWindAzure must exists and Customers must not exists
        '
        Dim ops As New TableInformation
        If ops.TableExists("NorthWindAzure", "ABC_123").Exists Then
            MessageBox.Show("Found")
        Else
            MessageBox.Show("Not found")
        End If
    End Sub

    Private Sub cmdColumnExistsFound_Click(sender As Object, e As EventArgs) Handles cmdColumnExistsFound.Click
        MessageBox.Show("NorthWindAzure must exists, Customers must exists, ContactName must exist.")
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
        MessageBox.Show("NorthWindAzure must exists, Customers must exists, ContactGuitar must not exist.")
        '
        ' NorthWindAzure must exists, Customers must exists, ContactGuitar must not exist
        '
        Dim ops As New TableInformation
        If ops.ColumnExists("NorthWindAzure", "Customers", "ContactGuitar") Then
            MessageBox.Show("Found")
        Else
            MessageBox.Show("Not found")
        End If
    End Sub

    Private Sub cmdColumnDetails_Click(sender As Object, e As EventArgs) Handles cmdColumnDetails.Click
        MessageBox.Show("NorthWindAzure must exists, Customers must exists with columns")
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
            MessageBox.Show(identityColumn.Name)
        End If

    End Sub
    Private Sub cmdTableKeys_Click(sender As Object, e As EventArgs) Handles cmdTableKeys.Click
        MessageBox.Show("NorthWindAzure must exists, Customers must exists with columns")

        Dim ops As New TableInformation
        '
        ' NorthWindAzure must exists, Customers must exists with columns
        '
        Dim result = ops.TableKeys("NorthWindAzure", "Customers")
        If result.Count > 0 Then
            Dim firstKey = result.FirstOrDefault()
            MessageBox.Show($"Shema name: {firstKey.SchemaName} = {firstKey.TableSchema}.{firstKey.TableName}")
        End If
    End Sub
    ''' <summary>
    ''' Provides a default name for a copy operation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lstDatabases_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDatabases.SelectedIndexChanged
        txtDatabaseCopyName.Text = $"{lstDatabases.Text}_Copy"
    End Sub
    ''' <summary>
    ''' Make a complete copy of a database on the default server
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdCopyDatabase_Click(sender As Object, e As EventArgs) Handles cmdCopyDatabase.Click
        If lstDatabases.Items.Count > 0 Then
            Dim ops As New DatabaseInformation
            ops.CopyDatabase(lstDatabases.Text, txtDatabaseCopyName.Text)
        Else
            MessageBox.Show("Press Get all server then get all databases")
        End If
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        ' delete any files which were created with scripting demo
        Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory), "*.txt").
            ToList().
            ForEach(Sub(f) File.Delete(f))

    End Sub
    ''' <summary>
    ''' Demo to create a new table in an existing database then immediately drop the
    ''' newly created table to demostrate working with events.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdCreateDropTableWithEvents_Click(sender As Object, e As EventArgs) Handles Button1.Click
        txtWithEvents.Text = ""
        Dim ops As New DatabaseInformation
        AddHandler ops.SmoTableCreate, AddressOf CreateTableEvent

        ops.CreateAndDropTableWithEvents()

    End Sub
    ''' <summary>
    ''' To properly avoid cross-threading
    ''' </summary>
    ''' <param name="pText"></param>
    Private Delegate Sub StringArgReturningVoidDelegate(pText As String)
    Public Sub CreateTableEvent(sender As Object, e As SmoTableCreateDropArgs)
        SetText(e.Message)
    End Sub
    Public Sub SetText(pText As String)
        If txtWithEvents.InvokeRequired Then
            Dim sarvd As New StringArgReturningVoidDelegate(AddressOf SetText)
            Invoke(sarvd, New Object() {pText})
        Else
            txtWithEvents.AppendText($"{pText}{Environment.NewLine}")
        End If
    End Sub
    Private Sub cmdScriptTables_Click(sender As Object, e As EventArgs) Handles cmdScriptTables.Click
        Dim ops As New DatabaseInformation
        ops.ScriptDatabaseTables()
    End Sub
End Class
