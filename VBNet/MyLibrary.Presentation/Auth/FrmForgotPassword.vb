Imports System.Configuration
Imports System.Text.RegularExpressions
Imports MyLibrary.BLL
Imports MyLibrary.DAL

Public Class FrmForgotPassword

    Private ReadOnly _auth As IAuthService
    Private _currentEmail As String = ""

    Public Sub New()
        InitializeComponent()

        Dim emailFrom = ConfigurationManager.AppSettings("EmailFrom")
        Dim emailPass = ConfigurationManager.AppSettings("EmailAppPassword")
        If String.IsNullOrEmpty(emailPass) Then
            MessageBox.Show("LỖI: Không đọc được mật khẩu từ Config! Biến emailPass đang rỗng.")
        Else
            MessageBox.Show("Đã đọc mật khẩu từ Config: " & emailPass.Substring(0, 3) & "***")
        End If
        _auth = New AuthService(
        New UnitOfWork(),
        New EmailService(emailFrom, emailPass)
    )
    End Sub

    Private Sub btnGetCode_Click(sender As Object, e As EventArgs) Handles btnGetCode.Click
        lblError.Text = ""
        Dim email As String = txtEmail.Text.Trim()

        If String.IsNullOrEmpty(email) Then
            lblError.Text = "Vui lòng nhập email."
            Return
        End If

        If Not IsValidEmail(email) Then
            lblError.Text = "Định dạng email không hợp lệ."
            Return
        End If

        Try

            If Not _auth.EmailExists(email) Then
                lblError.Text = "Email này chưa đăng ký tài khoản."
                Return
            End If

            Cursor = Cursors.WaitCursor
            _auth.ForgotPassword(email)
            Cursor = Cursors.Default

            _currentEmail = email
            MessageBox.Show("Mã xác nhận (OTP) đã được gửi đến email của bạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)

            pnlStep1.Visible = False
            pnlStep2.Visible = True

        Catch ex As Exception
            Cursor = Cursors.Default
            lblError.Text = "Lỗi: " & ex.Message
        End Try
    End Sub

    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        lblErrorStep2.Text = ""

        Dim otp As String = txtOTP.Text.Trim()
        Dim newPass As String = txtNewPass.Text
        Dim confirmPass As String = txtConfirmPass.Text

        ' Validate Input
        If String.IsNullOrEmpty(otp) Then
            lblErrorStep2.Text = "Vui lòng nhập mã xác nhận."
            Return
        End If

        If String.IsNullOrEmpty(newPass) OrElse newPass.Length < 6 Then
            lblErrorStep2.Text = "Mật khẩu mới phải từ 6 ký tự trở lên."
            Return
        End If

        If newPass <> confirmPass Then
            lblErrorStep2.Text = "Mật khẩu xác nhận không khớp."
            Return
        End If

        Try
            Cursor = Cursors.WaitCursor

            _auth.CompletePasswordReset(_currentEmail, otp, newPass)

            Cursor = Cursors.Default

            MessageBox.Show("Đổi mật khẩu thành công! Vui lòng đăng nhập lại.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()

        Catch ex As Exception
            Cursor = Cursors.Default
            lblErrorStep2.Text = "Lỗi: " & ex.Message
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        pnlStep2.Visible = False
        pnlStep1.Visible = True
        lblError.Text = ""
        txtOTP.Clear()
        txtNewPass.Clear()
        txtConfirmPass.Clear()
    End Sub

    Private Function IsValidEmail(email As String) As Boolean
        Dim pattern As String = "^[^@\s]+@[^@\s]+\.[^@\s]+$"
        Return Regex.IsMatch(email, pattern)
    End Function

End Class