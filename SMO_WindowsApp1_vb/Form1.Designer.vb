<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.cmdGetDatabaseThatExistsDefaultServer = New System.Windows.Forms.Button()
        Me.cmdGetDatabaseThatDoesNotExists = New System.Windows.Forms.Button()
        Me.cmdGetTablesForExistingDatabaseHasTables = New System.Windows.Forms.Button()
        Me.cmdGetDatabaseThatExistsNamedServer = New System.Windows.Forms.Button()
        Me.cmdGetTablesForExistingDatabaseHasNoTables = New System.Windows.Forms.Button()
        Me.cmdGetAvailableServers = New System.Windows.Forms.Button()
        Me.cmdGetAllDatabasesAndTables = New System.Windows.Forms.Button()
        Me.cmdTableExistFound = New System.Windows.Forms.Button()
        Me.cmdTableExistNotFound = New System.Windows.Forms.Button()
        Me.cmdSqlServerInstalledPath = New System.Windows.Forms.Button()
        Me.cmdColumnExistsFound = New System.Windows.Forms.Button()
        Me.cmdColumnExistNotFound = New System.Windows.Forms.Button()
        Me.cmdColumnDetails = New System.Windows.Forms.Button()
        Me.cmdTableKeys = New System.Windows.Forms.Button()
        Me.cmdGetAllDatabases = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmdCopyDatabase = New System.Windows.Forms.Button()
        Me.txtDatabaseCopyName = New System.Windows.Forms.TextBox()
        Me.lstDatabases = New System.Windows.Forms.ListBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cmdScriptTables = New System.Windows.Forms.Button()
        Me.txtWithEvents = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(12, 9)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(177, 446)
        Me.ListBox1.TabIndex = 1
        '
        'cmdGetDatabaseThatExistsDefaultServer
        '
        Me.cmdGetDatabaseThatExistsDefaultServer.Location = New System.Drawing.Point(726, 129)
        Me.cmdGetDatabaseThatExistsDefaultServer.Name = "cmdGetDatabaseThatExistsDefaultServer"
        Me.cmdGetDatabaseThatExistsDefaultServer.Size = New System.Drawing.Size(266, 23)
        Me.cmdGetDatabaseThatExistsDefaultServer.TabIndex = 7
        Me.cmdGetDatabaseThatExistsDefaultServer.Text = "Get Database that exists (default server)"
        Me.cmdGetDatabaseThatExistsDefaultServer.UseVisualStyleBackColor = True
        '
        'cmdGetDatabaseThatDoesNotExists
        '
        Me.cmdGetDatabaseThatDoesNotExists.Location = New System.Drawing.Point(726, 187)
        Me.cmdGetDatabaseThatDoesNotExists.Name = "cmdGetDatabaseThatDoesNotExists"
        Me.cmdGetDatabaseThatDoesNotExists.Size = New System.Drawing.Size(266, 23)
        Me.cmdGetDatabaseThatDoesNotExists.TabIndex = 8
        Me.cmdGetDatabaseThatDoesNotExists.Text = "Get Database that does not exists (default server)"
        Me.cmdGetDatabaseThatDoesNotExists.UseVisualStyleBackColor = True
        '
        'cmdGetTablesForExistingDatabaseHasTables
        '
        Me.cmdGetTablesForExistingDatabaseHasTables.Location = New System.Drawing.Point(726, 216)
        Me.cmdGetTablesForExistingDatabaseHasTables.Name = "cmdGetTablesForExistingDatabaseHasTables"
        Me.cmdGetTablesForExistingDatabaseHasTables.Size = New System.Drawing.Size(266, 23)
        Me.cmdGetTablesForExistingDatabaseHasTables.TabIndex = 9
        Me.cmdGetTablesForExistingDatabaseHasTables.Text = "Get tables for existing database (has tables)"
        Me.cmdGetTablesForExistingDatabaseHasTables.UseVisualStyleBackColor = True
        '
        'cmdGetDatabaseThatExistsNamedServer
        '
        Me.cmdGetDatabaseThatExistsNamedServer.Location = New System.Drawing.Point(726, 158)
        Me.cmdGetDatabaseThatExistsNamedServer.Name = "cmdGetDatabaseThatExistsNamedServer"
        Me.cmdGetDatabaseThatExistsNamedServer.Size = New System.Drawing.Size(266, 23)
        Me.cmdGetDatabaseThatExistsNamedServer.TabIndex = 10
        Me.cmdGetDatabaseThatExistsNamedServer.Text = "Get Database that exists (named server)"
        Me.cmdGetDatabaseThatExistsNamedServer.UseVisualStyleBackColor = True
        '
        'cmdGetTablesForExistingDatabaseHasNoTables
        '
        Me.cmdGetTablesForExistingDatabaseHasNoTables.Location = New System.Drawing.Point(726, 245)
        Me.cmdGetTablesForExistingDatabaseHasNoTables.Name = "cmdGetTablesForExistingDatabaseHasNoTables"
        Me.cmdGetTablesForExistingDatabaseHasNoTables.Size = New System.Drawing.Size(266, 23)
        Me.cmdGetTablesForExistingDatabaseHasNoTables.TabIndex = 11
        Me.cmdGetTablesForExistingDatabaseHasNoTables.Text = "Get tables for existing database (has not tables)"
        Me.cmdGetTablesForExistingDatabaseHasNoTables.UseVisualStyleBackColor = True
        '
        'cmdGetAvailableServers
        '
        Me.cmdGetAvailableServers.Location = New System.Drawing.Point(726, 9)
        Me.cmdGetAvailableServers.Name = "cmdGetAvailableServers"
        Me.cmdGetAvailableServers.Size = New System.Drawing.Size(266, 23)
        Me.cmdGetAvailableServers.TabIndex = 12
        Me.cmdGetAvailableServers.Text = "Get available servers"
        Me.cmdGetAvailableServers.UseVisualStyleBackColor = True
        '
        'cmdGetAllDatabasesAndTables
        '
        Me.cmdGetAllDatabasesAndTables.Location = New System.Drawing.Point(726, 97)
        Me.cmdGetAllDatabasesAndTables.Name = "cmdGetAllDatabasesAndTables"
        Me.cmdGetAllDatabasesAndTables.Size = New System.Drawing.Size(266, 23)
        Me.cmdGetAllDatabasesAndTables.TabIndex = 13
        Me.cmdGetAllDatabasesAndTables.Text = "Get all databases and tables"
        Me.cmdGetAllDatabasesAndTables.UseVisualStyleBackColor = True
        '
        'cmdTableExistFound
        '
        Me.cmdTableExistFound.Location = New System.Drawing.Point(726, 274)
        Me.cmdTableExistFound.Name = "cmdTableExistFound"
        Me.cmdTableExistFound.Size = New System.Drawing.Size(266, 23)
        Me.cmdTableExistFound.TabIndex = 14
        Me.cmdTableExistFound.Text = "Table exists (found)"
        Me.cmdTableExistFound.UseVisualStyleBackColor = True
        '
        'cmdTableExistNotFound
        '
        Me.cmdTableExistNotFound.Location = New System.Drawing.Point(726, 303)
        Me.cmdTableExistNotFound.Name = "cmdTableExistNotFound"
        Me.cmdTableExistNotFound.Size = New System.Drawing.Size(266, 23)
        Me.cmdTableExistNotFound.TabIndex = 15
        Me.cmdTableExistNotFound.Text = "Table exists (not found)"
        Me.cmdTableExistNotFound.UseVisualStyleBackColor = True
        '
        'cmdSqlServerInstalledPath
        '
        Me.cmdSqlServerInstalledPath.Location = New System.Drawing.Point(726, 38)
        Me.cmdSqlServerInstalledPath.Name = "cmdSqlServerInstalledPath"
        Me.cmdSqlServerInstalledPath.Size = New System.Drawing.Size(266, 23)
        Me.cmdSqlServerInstalledPath.TabIndex = 16
        Me.cmdSqlServerInstalledPath.Text = "SQL-Server install path"
        Me.cmdSqlServerInstalledPath.UseVisualStyleBackColor = True
        '
        'cmdColumnExistsFound
        '
        Me.cmdColumnExistsFound.Location = New System.Drawing.Point(726, 332)
        Me.cmdColumnExistsFound.Name = "cmdColumnExistsFound"
        Me.cmdColumnExistsFound.Size = New System.Drawing.Size(266, 23)
        Me.cmdColumnExistsFound.TabIndex = 17
        Me.cmdColumnExistsFound.Text = "Column exists (found)"
        Me.cmdColumnExistsFound.UseVisualStyleBackColor = True
        '
        'cmdColumnExistNotFound
        '
        Me.cmdColumnExistNotFound.Location = New System.Drawing.Point(726, 361)
        Me.cmdColumnExistNotFound.Name = "cmdColumnExistNotFound"
        Me.cmdColumnExistNotFound.Size = New System.Drawing.Size(266, 23)
        Me.cmdColumnExistNotFound.TabIndex = 18
        Me.cmdColumnExistNotFound.Text = "Column exists (not found)"
        Me.cmdColumnExistNotFound.UseVisualStyleBackColor = True
        '
        'cmdColumnDetails
        '
        Me.cmdColumnDetails.Location = New System.Drawing.Point(726, 390)
        Me.cmdColumnDetails.Name = "cmdColumnDetails"
        Me.cmdColumnDetails.Size = New System.Drawing.Size(266, 23)
        Me.cmdColumnDetails.TabIndex = 19
        Me.cmdColumnDetails.Text = "Column details"
        Me.cmdColumnDetails.UseVisualStyleBackColor = True
        '
        'cmdTableKeys
        '
        Me.cmdTableKeys.Location = New System.Drawing.Point(726, 419)
        Me.cmdTableKeys.Name = "cmdTableKeys"
        Me.cmdTableKeys.Size = New System.Drawing.Size(266, 23)
        Me.cmdTableKeys.TabIndex = 20
        Me.cmdTableKeys.Text = "Table keys"
        Me.cmdTableKeys.UseVisualStyleBackColor = True
        '
        'cmdGetAllDatabases
        '
        Me.cmdGetAllDatabases.Location = New System.Drawing.Point(726, 68)
        Me.cmdGetAllDatabases.Name = "cmdGetAllDatabases"
        Me.cmdGetAllDatabases.Size = New System.Drawing.Size(266, 23)
        Me.cmdGetAllDatabases.TabIndex = 21
        Me.cmdGetAllDatabases.Text = "Get all databases"
        Me.cmdGetAllDatabases.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.cmdCopyDatabase)
        Me.GroupBox1.Controls.Add(Me.txtDatabaseCopyName)
        Me.GroupBox1.Controls.Add(Me.lstDatabases)
        Me.GroupBox1.Location = New System.Drawing.Point(195, 164)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(523, 307)
        Me.GroupBox1.TabIndex = 22
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Transfer schema/data from one db to another"
        '
        'cmdCopyDatabase
        '
        Me.cmdCopyDatabase.Location = New System.Drawing.Point(232, 61)
        Me.cmdCopyDatabase.Name = "cmdCopyDatabase"
        Me.cmdCopyDatabase.Size = New System.Drawing.Size(266, 23)
        Me.cmdCopyDatabase.TabIndex = 23
        Me.cmdCopyDatabase.Text = "Copy database"
        Me.cmdCopyDatabase.UseVisualStyleBackColor = True
        '
        'txtDatabaseCopyName
        '
        Me.txtDatabaseCopyName.Location = New System.Drawing.Point(220, 35)
        Me.txtDatabaseCopyName.Name = "txtDatabaseCopyName"
        Me.txtDatabaseCopyName.Size = New System.Drawing.Size(289, 20)
        Me.txtDatabaseCopyName.TabIndex = 1
        '
        'lstDatabases
        '
        Me.lstDatabases.FormattingEnabled = True
        Me.lstDatabases.Location = New System.Drawing.Point(10, 21)
        Me.lstDatabases.Name = "lstDatabases"
        Me.lstDatabases.Size = New System.Drawing.Size(204, 264)
        Me.lstDatabases.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(157, 95)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(266, 23)
        Me.Button1.TabIndex = 23
        Me.Button1.Text = "Create/Drop table with events"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cmdScriptTables
        '
        Me.cmdScriptTables.Location = New System.Drawing.Point(726, 448)
        Me.cmdScriptTables.Name = "cmdScriptTables"
        Me.cmdScriptTables.Size = New System.Drawing.Size(266, 23)
        Me.cmdScriptTables.TabIndex = 24
        Me.cmdScriptTables.Text = "Script tables example"
        Me.cmdScriptTables.UseVisualStyleBackColor = True
        '
        'txtWithEvents
        '
        Me.txtWithEvents.Location = New System.Drawing.Point(14, 23)
        Me.txtWithEvents.Multiline = True
        Me.txtWithEvents.Name = "txtWithEvents"
        Me.txtWithEvents.Size = New System.Drawing.Size(495, 63)
        Me.txtWithEvents.TabIndex = 25
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtWithEvents)
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Location = New System.Drawing.Point(195, 15)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(523, 143)
        Me.GroupBox2.TabIndex = 26
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Events"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1001, 486)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.cmdScriptTables)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmdGetAllDatabases)
        Me.Controls.Add(Me.cmdTableKeys)
        Me.Controls.Add(Me.cmdColumnDetails)
        Me.Controls.Add(Me.cmdColumnExistNotFound)
        Me.Controls.Add(Me.cmdColumnExistsFound)
        Me.Controls.Add(Me.cmdSqlServerInstalledPath)
        Me.Controls.Add(Me.cmdTableExistNotFound)
        Me.Controls.Add(Me.cmdTableExistFound)
        Me.Controls.Add(Me.cmdGetAllDatabasesAndTables)
        Me.Controls.Add(Me.cmdGetAvailableServers)
        Me.Controls.Add(Me.cmdGetTablesForExistingDatabaseHasNoTables)
        Me.Controls.Add(Me.cmdGetDatabaseThatExistsNamedServer)
        Me.Controls.Add(Me.cmdGetTablesForExistingDatabaseHasTables)
        Me.Controls.Add(Me.cmdGetDatabaseThatDoesNotExists)
        Me.Controls.Add(Me.cmdGetDatabaseThatExistsDefaultServer)
        Me.Controls.Add(Me.ListBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TechNet SMO Examples"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents cmdGetDatabaseThatExistsDefaultServer As Button
    Friend WithEvents cmdGetDatabaseThatDoesNotExists As Button
    Friend WithEvents cmdGetTablesForExistingDatabaseHasTables As Button
    Friend WithEvents cmdGetDatabaseThatExistsNamedServer As Button
    Friend WithEvents cmdGetTablesForExistingDatabaseHasNoTables As Button
    Friend WithEvents cmdGetAvailableServers As Button
    Friend WithEvents cmdGetAllDatabasesAndTables As Button
    Friend WithEvents cmdTableExistFound As Button
    Friend WithEvents cmdTableExistNotFound As Button
    Friend WithEvents cmdSqlServerInstalledPath As Button
    Friend WithEvents cmdColumnExistsFound As Button
    Friend WithEvents cmdColumnExistNotFound As Button
    Friend WithEvents cmdColumnDetails As Button
    Friend WithEvents cmdTableKeys As Button
    Friend WithEvents cmdGetAllDatabases As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lstDatabases As ListBox
    Friend WithEvents txtDatabaseCopyName As TextBox
    Friend WithEvents cmdCopyDatabase As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents cmdScriptTables As Button
    Friend WithEvents txtWithEvents As TextBox
    Friend WithEvents GroupBox2 As GroupBox
End Class
