Imports System.Text.RegularExpressions
Imports MyLibrary.BLL
Imports MyLibrary.DAL
Imports MyLibrary.Domain

Public Class FrmRegister

    Private ReadOnly _auth As IAuthService
    Private _registeredEmail As String = ""

    Public Sub New()
        InitializeComponent()

        ' Khởi tạo Service
        _auth = New AuthService(
            New UnitOfWork(),
            New EmailService("huydo272@gmail.com", "seky quuk rnev lyzc")
        )
    End Sub

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        lblError.Text = ""
        Dim email = txtEmail.Text.Trim()
        Dim pass = txtPassword.Text
        Dim confirm = txtConfirm.Text
        Dim fullName = txtFullName.Text.Trim()

        If pass <> confirm Then
            lblError.Text = "Mật khẩu xác nhận không khớp."
            Return
        End If

        Try
            Cursor = Cursors.WaitCursor

            Dim dto As New RegisterRequestDto With {
                .FullName = fullName,
                .Email = email,
                .Password = pass
            }
            _auth.Register(dto)

            Cursor = Cursors.Default


            _registeredEmail = email
            MessageBox.Show("Đăng ký thành công! Mã xác thực đã được gửi đến email của bạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)


            pnlRegisterInfo.Visible = False
            pnlVerify.Visible = True

        Catch ex As Exception
            Cursor = Cursors.Default
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub btnConfirmOTP_Click(sender As Object, e As EventArgs) Handles btnConfirmOTP.Click
        lblErrorVerify.Text = ""
        Dim otp = txtOTP.Text.Trim()

        If String.IsNullOrEmpty(otp) Then
            lblErrorVerify.Text = "Vui lòng nhập mã xác thực."
            Return
        End If

        Try
            Cursor = Cursors.WaitCursor

            _auth.VerifyAccount(_registeredEmail, otp)

            Cursor = Cursors.Default

            MessageBox.Show("Kích hoạt tài khoản thành công! Bạn có thể đăng nhập ngay bây giờ.", "Chúc mừng", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            Cursor = Cursors.Default
            lblErrorVerify.Text = ex.Message
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        pnlVerify.Visible = False
        pnlRegisterInfo.Visible = True
        lblError.Text = ""
    End Sub

End Class