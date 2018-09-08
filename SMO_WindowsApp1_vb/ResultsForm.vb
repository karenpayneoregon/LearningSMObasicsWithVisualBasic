Public Class ResultsForm
    Private _text As String
    Private _fileNames As List(Of String)

    Public Sub New(pText As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _text = pText
    End Sub
    Public Sub New(pFileNames As List(Of String))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _fileNames = pFileNames
    End Sub
    Private Sub ResultsForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If Not String.IsNullOrWhiteSpace(_text) Then
            TextBox1.Text = _text
        Else
            TextBox1.Lines = _fileNames.ToArray()
        End If

    End Sub
End Class