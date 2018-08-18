Public Class ResultsForm
    Private _text As String
    Public Sub New(pText As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _text = pText
    End Sub
    Private Sub ResultsForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        TextBox1.Text = _text
    End Sub
End Class