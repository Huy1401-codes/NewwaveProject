Imports System.Data.SqlClient
Imports MyLibrary.BLL
Imports MyLibrary.DAL

Public Class FrmLogin

    Private ReadOnly _authService As IAuthService

    Public Sub New()
        InitializeComponent()
        Dim uow As New UnitOfWork()
        Dim emailService As New EmailService("huydo272@gmail.com", "app-password")
        _authService = New AuthService(uow, emailService)
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Try
            lblError.Text = ""

            Dim email = txtEmail.Text.Trim()
            Dim password = txtPassword.Text

            Dim result = _authService.Login(email, password)

            SessionManager.CurrentUser = result

            If result.RoleName = "Admin" Then
                Dim f As New FrmAdminMain()
                f.Show()
            Else
                Dim f As New FrmUserMenu()
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
            _authService.ForgotPassword(email)
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Try
        '    Using context As New AppDbContext()

        '        Dim count = context.Users.Count()

        '        MessageBox.Show($"Kết nối THÀNH CÔNG! Hiện có {count} user trong DB.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    End Using
        'Catch ex As Exception
        '    MessageBox.Show("Lỗi kết nối: " & ex.Message & vbCrLf & "Inner: " & If(ex.InnerException IsNot Nothing, ex.InnerException.Message, ""), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try

        Dim emailService As New EmailService(
     "huydo272@gmail.com",
     "vxsfydbwjyrytxne"
 )

        emailService.SendEmail(
            "huydqhe173522@fpt.edu.vn",
            "SMTP OK",
            "Test thành công"
        )

        MessageBox.Show("Gửi mail OK")
    End Sub
End Class
