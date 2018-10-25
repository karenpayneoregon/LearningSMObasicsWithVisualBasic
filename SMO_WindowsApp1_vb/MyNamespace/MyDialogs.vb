
Namespace My
    ''' <summary>
    ''' Contains wrappers for common dialog operations where the intent is to keep
    ''' method based and easy to maintain.
    ''' </summary>
    ''' <remarks></remarks>
    <ComponentModel.EditorBrowsable(Global.System.ComponentModel.EditorBrowsableState.Never)>
    Partial Friend Class _Dialogs

        ''' <summary>
        ''' Ask question with NO as the default button
        ''' </summary>
        ''' <param name="pText"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Question(pText As String) As Boolean
            Return (MessageBox.Show(pText, My.Application.Info.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = MsgBoxResult.Yes)
        End Function
        Public Function Question(pText As String, pTitle As String) As Boolean
            Return (MessageBox.Show(pText, pTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = MsgBoxResult.Yes)
        End Function
        ''' <summary>
        ''' Ask question
        ''' </summary>
        ''' <param name="pText">Question to ask</param>
        ''' <param name="pTitle">Text for dialog caption</param>
        ''' <param name="pDefaultButton">Which button is the default</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Question(pText As String, pTitle As String, pDefaultButton As MsgBoxResult) As Boolean
            Dim db As MessageBoxDefaultButton
            If pDefaultButton = MsgBoxResult.No Then
                db = MessageBoxDefaultButton.Button2
            End If
            Return (MessageBox.Show(pText, pTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, db) = MsgBoxResult.Yes)
        End Function
        ''' <summary>
        ''' Shows text in dialog with information icon
        ''' </summary>
        ''' <param name="pText">Message to display</param>
        ''' <remarks></remarks>
        Public Sub InformationDialog(pText As String)
            MessageBox.Show(pText, My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub
        ''' <summary>
        ''' Shows text in dialog with information icon
        ''' </summary>
        ''' <param name="pText">Message to display</param>
        ''' <param name="pTitle"></param>
        ''' <remarks></remarks>
        Public Sub InformationDialog(pText As String, pTitle As String)
            MessageBox.Show(pText, pTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub
        Public Sub WarningDialog(ByVal Text As String)
            MessageBox.Show(Text, My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Sub
        Public Sub WarningDialog(pText As String, pTitle As String)
            MessageBox.Show(pText, pTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Sub
        ''' <summary>
        ''' For displaying error/exception text with Error icon
        ''' </summary>
        ''' <param name="pText"></param>
        ''' <remarks></remarks>
        Public Sub ExceptionDialog(pText As String)
            MessageBox.Show(pText, My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub
        ''' <summary>
        ''' For displaying error/exception text with Error icon
        ''' </summary>
        ''' <param name="pText"></param>
        ''' <param name="pTitle"></param>
        ''' <remarks></remarks>
        Public Sub ExceptionDialog(pText As String, pTitle As String)
            MessageBox.Show(pText, pTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub
        Public Sub ExceptionDialog(pText As String, pTitle As String, ex As Exception)
            Dim Message As String = String.Concat(pText, Environment.NewLine, ex.Message)
            MessageBox.Show(Message, pTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ex"></param>
        ''' <param name="pText"></param>
        ''' <param name="pTitle"></param>
        ''' <remarks>
        ''' </remarks>
        Public Sub ExceptionDialog(ex As Exception, pText As String, pTitle As String)
            MessageBox.Show($"{pText}{Environment.NewLine}{ex.Message}", pTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub
    End Class

    <HideModuleName()>
    Friend Module Karens_Dialogs
        Private instance As New ThreadSafeObjectProvider(Of _Dialogs)
        ReadOnly Property Dialogs() As _Dialogs
            Get
                Return instance.GetInstance()
            End Get
        End Property
    End Module
End Namespace
