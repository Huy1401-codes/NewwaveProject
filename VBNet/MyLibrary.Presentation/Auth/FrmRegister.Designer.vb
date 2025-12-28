<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmRegister
    Inherits Form

    Private components As System.ComponentModel.IContainer

    Friend WithEvents pnlRegisterInfo As Panel
    Friend WithEvents lblFullName As Label
    Friend WithEvents txtFullName As TextBox
    Friend WithEvents lblEmail As Label
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents lblPassword As Label
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents lblConfirm As Label
    Friend WithEvents txtConfirm As TextBox
    Friend WithEvents btnRegister As Button
    Friend WithEvents lblError As Label

    Friend WithEvents pnlVerify As Panel
    Friend WithEvents lblVerifyTitle As Label
    Friend WithEvents lblVerifyMsg As Label
    Friend WithEvents txtOTP As TextBox
    Friend WithEvents btnConfirmOTP As Button
    Friend WithEvents btnBack As Button
    Friend WithEvents lblErrorVerify As Label


    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub InitializeComponent()
        Me.pnlRegisterInfo = New Panel()
        Me.lblFullName = New Label()
        Me.txtFullName = New TextBox()
        Me.lblEmail = New Label()
        Me.txtEmail = New TextBox()
        Me.lblPassword = New Label()
        Me.txtPassword = New TextBox()
        Me.lblConfirm = New Label()
        Me.txtConfirm = New TextBox()
        Me.btnRegister = New Button()
        Me.lblError = New Label()

        Me.pnlVerify = New Panel()
        Me.lblVerifyTitle = New Label()
        Me.lblVerifyMsg = New Label()
        Me.txtOTP = New TextBox()
        Me.btnConfirmOTP = New Button()
        Me.btnBack = New Button()
        Me.lblErrorVerify = New Label()

        Me.SuspendLayout()

        Me.ClientSize = New Size(360, 380)
        Me.Text = "Đăng Ký Tài Khoản"
        Me.StartPosition = FormStartPosition.CenterScreen


        Me.pnlRegisterInfo.Location = New Point(10, 10)
        Me.pnlRegisterInfo.Size = New Size(340, 360)
        Me.pnlRegisterInfo.Visible = True


        Me.lblFullName.Text = "Họ và tên:"
        Me.lblFullName.Location = New Point(20, 10)
        Me.lblFullName.AutoSize = True
        Me.pnlRegisterInfo.Controls.Add(Me.lblFullName)

        Me.txtFullName.Location = New Point(20, 30)
        Me.txtFullName.Size = New Size(300, 23)
        Me.pnlRegisterInfo.Controls.Add(Me.txtFullName)

        Me.lblEmail.Text = "Email:"
        Me.lblEmail.Location = New Point(20, 60)
        Me.lblEmail.AutoSize = True
        Me.pnlRegisterInfo.Controls.Add(Me.lblEmail)

        Me.txtEmail.Location = New Point(20, 80)
        Me.txtEmail.Size = New Size(300, 23)
        Me.pnlRegisterInfo.Controls.Add(Me.txtEmail)

        Me.lblPassword.Text = "Mật khẩu:"
        Me.lblPassword.Location = New Point(20, 110)
        Me.lblPassword.AutoSize = True
        Me.pnlRegisterInfo.Controls.Add(Me.lblPassword)

        Me.txtPassword.Location = New Point(20, 130)
        Me.txtPassword.Size = New Size(300, 23)
        Me.txtPassword.PasswordChar = "*"c
        Me.pnlRegisterInfo.Controls.Add(Me.txtPassword)

        Me.lblConfirm.Text = "Nhập lại mật khẩu:"
        Me.lblConfirm.Location = New Point(20, 160)
        Me.lblConfirm.AutoSize = True
        Me.pnlRegisterInfo.Controls.Add(Me.lblConfirm)

        Me.txtConfirm.Location = New Point(20, 180)
        Me.txtConfirm.Size = New Size(300, 23)
        Me.txtConfirm.PasswordChar = "*"c
        Me.pnlRegisterInfo.Controls.Add(Me.txtConfirm)

        Me.btnRegister.Text = "Đăng Ký"
        Me.btnRegister.Location = New Point(20, 230)
        Me.btnRegister.Size = New Size(300, 40)
        Me.btnRegister.BackColor = Color.LightBlue
        Me.pnlRegisterInfo.Controls.Add(Me.btnRegister)


        Me.lblError.ForeColor = Color.Red
        Me.lblError.Location = New Point(20, 280)
        Me.lblError.Size = New Size(300, 40)
        Me.pnlRegisterInfo.Controls.Add(Me.lblError)

        Me.Controls.Add(Me.pnlRegisterInfo)

        Me.pnlVerify.Location = New Point(10, 10)
        Me.pnlVerify.Size = New Size(340, 360)
        Me.pnlVerify.Visible = False

        Me.lblVerifyTitle.Text = "XÁC THỰC TÀI KHOẢN"
        Me.lblVerifyTitle.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        Me.lblVerifyTitle.Location = New Point(20, 20)
        Me.lblVerifyTitle.AutoSize = True
        Me.pnlVerify.Controls.Add(Me.lblVerifyTitle)

        Me.lblVerifyMsg.Text = "Mã xác thực (OTP) đã được gửi đến email của bạn. Vui lòng nhập mã để kích hoạt:"
        Me.lblVerifyMsg.Location = New Point(20, 60)
        Me.lblVerifyMsg.Size = New Size(300, 40)
        Me.pnlVerify.Controls.Add(Me.lblVerifyMsg)

        Me.txtOTP.Location = New Point(20, 110)
        Me.txtOTP.Size = New Size(300, 30)
        Me.txtOTP.Font = New Font("Segoe UI", 14)
        Me.txtOTP.TextAlign = HorizontalAlignment.Center
        Me.pnlVerify.Controls.Add(Me.txtOTP)

        Me.btnConfirmOTP.Text = "Kích Hoạt Tài Khoản"
        Me.btnConfirmOTP.Location = New Point(20, 160)
        Me.btnConfirmOTP.Size = New Size(300, 40)
        Me.btnConfirmOTP.BackColor = Color.LightGreen
        Me.pnlVerify.Controls.Add(Me.btnConfirmOTP)

        Me.btnBack.Text = "Quay lại (Nhập sai email)"
        Me.btnBack.Location = New Point(20, 210)
        Me.btnBack.Size = New Size(300, 30)
        Me.pnlVerify.Controls.Add(Me.btnBack)

        Me.lblErrorVerify.ForeColor = Color.Red
        Me.lblErrorVerify.Location = New Point(20, 250)
        Me.lblErrorVerify.Size = New Size(300, 40)
        Me.pnlVerify.Controls.Add(Me.lblErrorVerify)

        Me.Controls.Add(Me.pnlVerify)

        Me.ResumeLayout(False)
    End Sub

End Class