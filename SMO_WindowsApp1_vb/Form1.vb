Imports System.IO
Imports System.Text
Imports Microsoft.SqlServer.Management.Smo
Imports SMO_UtilityLibrary
Imports SMO_UtilityLibrary.Classes
Imports Tulpep.NotificationWindow

Public Class Form1
    Private _serverNames As List(Of String)
    'Private myAnimator As New FormAnimator(Me, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up, 400)
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
        Else
            Dim popupNotifier1 = New PopupNotifier
            popupNotifier1.ContentText = "Server(s) loaded"
            popupNotifier1.BodyColor = Color.FloralWhite
            popupNotifier1.ContentPadding = New Padding(12)
            popupNotifier1.Delay = 2000
            popupNotifier1.Popup()
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
            Dim popupNotifier1 = New PopupNotifier
            popupNotifier1.ContentText = "Databases have been loaded!"
            popupNotifier1.BodyColor = Color.FloralWhite
            popupNotifier1.ContentPadding = New Padding(12)
            popupNotifier1.Delay = 2000
            popupNotifier1.Popup()
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
        If My.Dialogs.Question("NorthWindAzure must exists and Customers must exists otherwise stop execution. Continue?") Then
            '
            ' NorthWindAzure must exists and Customers must exists
            '
            Dim ops As New TableInformation
            If ops.TableExists("NorthWindAzure", "Customers").Exists Then
                MessageBox.Show("Found")
            Else
                MessageBox.Show("Not found")
            End If
        End If
    End Sub

    Private Sub cmdTableExistNotFound_Click(sender As Object, e As EventArgs) Handles cmdTableExistNotFound.Click

        If My.Dialogs.Question("NorthWindAzure must not exists and Customers must not exists otherwise stop execution. Continue?") Then
            '
            ' NorthWindAzure must exists and Customers must not exists
            '
            Dim ops As New TableInformation
            If ops.TableExists("NorthWindAzure", "ABC_123").Exists Then
                MessageBox.Show("Found")
            Else
                MessageBox.Show("Not found")
            End If
        End If

    End Sub

    Private Sub cmdColumnExistsFound_Click(sender As Object, e As EventArgs) Handles cmdColumnExistsFound.Click
        If My.Dialogs.Question("NorthWindAzure must exists, Customers must exists, ContactName must exist. Continue?") Then
            '
            ' NorthWindAzure must exists, Customers must exists, ContactName must exist
            '
            Dim ops As New TableInformation
            If ops.ColumnExists("NorthWindAzure", "Customers", "ContactName") Then
                MessageBox.Show("Found")
            Else
                MessageBox.Show("Not found")
            End If
        End If
    End Sub

    Private Sub cmdColumnExistNotFound_Click(sender As Object, e As EventArgs) Handles cmdColumnExistNotFound.Click
        If My.Dialogs.Question("NorthWindAzure must exists, Customers must exists, ContactGuitar must not exist. Continue?") Then
            '
            ' NorthWindAzure must exists, Customers must exists, ContactGuitar must not exist
            '
            Dim ops As New TableInformation
            If ops.ColumnExists("NorthWindAzure", "Customers", "ContactGuitar") Then
                MessageBox.Show("Found")
            Else
                MessageBox.Show("Not found")
            End If
        End If
    End Sub

    Private Sub cmdColumnDetails_Click(sender As Object, e As EventArgs) Handles cmdColumnDetails.Click
        If My.Dialogs.Question("NorthWindAzure must exists, Customers must exists with columns. Continue?") Then
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
        End If
    End Sub
    Private Sub cmdTableKeys_Click(sender As Object, e As EventArgs) Handles cmdTableKeys.Click
        If My.Dialogs.Question("NorthWindAzure must exists, Customers must exists with columns. Continue?") Then
            Dim ops As New TableInformation
            '
            ' NorthWindAzure must exists, Customers must exists with columns
            '
            Dim result = ops.TableKeys("NorthWindAzure", "Customers")
            If result.Count > 0 Then
                Dim firstKey = result.FirstOrDefault()
                MessageBox.Show($"Shema name: {firstKey.SchemaName} = {firstKey.TableSchema}.{firstKey.TableName}")
            End If
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
    ''' <summary>
    ''' If author's machine populate one server
    ''' Remove all text files which may have been left over from scripting example.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        '
        ' Pre-load as it exists on the author's box
        '
        If Environment.UserName = "Karens" Then
            ListBox1.DataSource = New List(Of String) From {"KARENS-PC"}
        End If

        ' delete any files which were created with scripting demo
        Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory), "*.txt").
            ToList().
            ForEach(Sub(f) File.Delete(f))

    End Sub
    ''' <summary>
    ''' Demo to create a new table in an database which if exists is dropped then immediately drop the
    ''' newly created table to demostrate working with events.
    ''' 
    ''' HOW TO RUN:
    ''' 1. Please a breakpoint on CreateAndDropTableWithEvents
    ''' 2. Step through the code, when you hit tb.Drop() pause.
    ''' 3. Connect to SQL-Server via Server Explorer in Visual Studio or via SSMS.
    ''' 4. Look for the database CreatedForCodeSample, if not there do a refresh on the database.
    ''' 5. Examine the table and indices.
    ''' 6. Pop out of the database leaving nothing open.
    ''' 7. Continue debugging.
    ''' 8. table is dropped followed by database being dropped.
    ''' 8. Re-examine the database, it will be gone.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdCreateDropTableWithEvents_Click(sender As Object, e As EventArgs) Handles Button1.Click
        txtWithEvents.Text = ""
        Dim ops As New DatabaseInformation
        AddHandler ops.SmoTableCreate, AddressOf CreateTableEvent

        If Not ops.CreateAndDropTableWithEvents("CreatedForCodeSample") Then
            MessageBox.Show(ops.LastExceptionMessage)
        End If

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
        Dim fileNames = ops.ScriptDatabaseTables()
        Dim f As New ResultsForm(fileNames)
        Try
            f.ShowDialog()
        Finally
            f.Dispose()
        End Try
    End Sub
End Class
