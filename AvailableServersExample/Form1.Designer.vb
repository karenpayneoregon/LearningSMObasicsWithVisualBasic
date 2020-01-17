<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cmdGetAvailableServers = New System.Windows.Forms.Button()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.BackupButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'cmdGetAvailableServers
        '
        Me.cmdGetAvailableServers.Location = New System.Drawing.Point(204, 32)
        Me.cmdGetAvailableServers.Name = "cmdGetAvailableServers"
        Me.cmdGetAvailableServers.Size = New System.Drawing.Size(139, 23)
        Me.cmdGetAvailableServers.TabIndex = 0
        Me.cmdGetAvailableServers.Text = "AvailableServers"
        Me.cmdGetAvailableServers.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(21, 32)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(177, 108)
        Me.ListBox1.TabIndex = 2
        '
        'BackupButton
        '
        Me.BackupButton.Location = New System.Drawing.Point(204, 153)
        Me.BackupButton.Name = "BackupButton"
        Me.BackupButton.Size = New System.Drawing.Size(139, 23)
        Me.BackupButton.TabIndex = 3
        Me.BackupButton.Text = "Perform backup"
        Me.BackupButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(351, 188)
        Me.Controls.Add(Me.BackupButton)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.cmdGetAvailableServers)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Form1"
        Me.Text = "Available servers"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cmdGetAvailableServers As Button
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents BackupButton As Button
End Class
