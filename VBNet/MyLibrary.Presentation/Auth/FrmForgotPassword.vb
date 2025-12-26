Imports MyLibrary.BLL
Imports MyLibrary.DAL

Public Class FrmForgotPassword

    Private ReadOnly _auth As IAuthService

    Public Sub New()
        InitializeComponent()
        _auth = New AuthService(New UnitOfWork(),
                                New EmailService("huydo272@gmail.com", "app-password"))
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try
            _auth.ResetPassword(txtEmail.Text)
            MessageBox.Show("Mật khẩu mới đã gửi qua email")
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class
