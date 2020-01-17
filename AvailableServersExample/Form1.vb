Imports SMO_UtilityLibrary.Classes
Public Class Form1
    Private _serverNames As List(Of String)

    Private Async Function GetAvailableServerNames() As Task
        Dim serverOperations = New ServerInformation
        _serverNames = Await serverOperations.GetServersAsync()
    End Function

    Private Async Sub cmdGetAvailableServers_Click(sender As Object, e As EventArgs) _
        Handles cmdGetAvailableServers.Click

        Await GetAvailableServerNames()

        ListBox1.DataSource = _serverNames
        ListBox1.DisplayMember = "Name"

        If ListBox1.Items.Count = 0 Then
            MessageBox.Show("Found no servers present.")
        Else
            MessageBox.Show("Known servers have been loaded")
        End If
    End Sub
    ''' <summary>
    ''' TODO Move this to another form
    ''' This is here to suggest an option to backup a database
    ''' from a forum post
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BackupButton_Click(sender As Object, e As EventArgs) Handles BackupButton.Click
        Dim serverOperations = New DatabaseInformation
        serverOperations.BackupDatabase()
    End Sub
End Class
