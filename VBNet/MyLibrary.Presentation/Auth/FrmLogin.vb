Imports System.Data.SqlClient
Imports MyLibrary.BLL
Imports MyLibrary.DAL

Public Class FrmLogin

    Private ReadOnly _authService As IAuthService

    Public Sub New()
        InitializeComponent()

        ' Khởi tạo service
        Dim uow As New UnitOfWork()
        Dim emailService As New EmailService("your@gmail.com", "app-password")
        _authService = New AuthService(uow, emailService)
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Try
            lblError.Text = ""

            Dim email = txtEmail.Text.Trim()
            Dim password = txtPassword.Text

            Dim result = _authService.Login(email, password)

            ' Lưu session
            SessionManager.CurrentUser = result

            ' Phân quyền
            If result.RoleName = "Admin" Then
                Dim f As New FrmAdminMain()
                f.Show()
            Else
                Dim f As New FrmUserMain()
                f.Show()
            End If

            Me.Hide()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub lnkForgotPassword_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
        Handles lnkForgotPassword.LinkClicked

        Dim email = InputBox("Nhập email của bạn:", "Quên mật khẩu")
        If String.IsNullOrWhiteSpace(email) Then Return

        Try
            _authService.ResetPassword(email)
            MessageBox.Show("Mật khẩu mới đã được gửi qua email", "Thông báo")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub lnkRegister_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
        Handles lnkRegister.LinkClicked

        Dim f As New FrmRegister()
        f.ShowDialog()
    End Sub

End Class
