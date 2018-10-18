Imports Microsoft.Win32
Imports System.Text
Imports System.IO

Public Class WebHelpForm
    Public url As String
    Private Sub WebHelpForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
        WebBrowser.Navigate(appPath & "\" & url)
    End Sub

End Class
