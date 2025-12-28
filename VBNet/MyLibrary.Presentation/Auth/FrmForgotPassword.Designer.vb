<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmForgotPassword
    Inherits Form

    ' Form Components
    Private components As System.ComponentModel.IContainer

    ' Step 1 Controls
    Friend WithEvents pnlStep1 As Panel
    Friend WithEvents lblEmail As Label
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents btnGetCode As Button
    Friend WithEvents lblError As Label

    ' Step 2 Controls
    Friend WithEvents pnlStep2 As Panel
    Friend WithEvents lblInstructions As Label
    Friend WithEvents lblOTP As Label
    Friend WithEvents txtOTP As TextBox
    Friend WithEvents lblNewPass As Label
    Friend WithEvents txtNewPass As TextBox
    Friend WithEvents lblConfirmPass As Label
    Friend WithEvents txtConfirmPass As TextBox
    Friend WithEvents btnConfirm As Button
    Friend WithEvents btnBack As Button
    Friend WithEvents lblErrorStep2 As Label

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub InitializeComponent()
        ' Khởi tạo các Control
        Me.pnlStep1 = New Panel()
        Me.lblEmail = New Label()
        Me.txtEmail = New TextBox()
        Me.btnGetCode = New Button()
        Me.lblError = New Label()

        Me.pnlStep2 = New Panel()
        Me.lblInstructions = New Label()
        Me.lblOTP = New Label()
        Me.txtOTP = New TextBox()
        Me.lblNewPass = New Label()
        Me.txtNewPass = New TextBox()
        Me.lblConfirmPass = New Label()
        Me.txtConfirmPass = New TextBox()
        Me.btnConfirm = New Button()
        Me.btnBack = New Button()
        Me.lblErrorStep2 = New Label()

        Me.SuspendLayout()

        ' --- CẤU HÌNH FORM ---
        Me.ClientSize = New Size(350, 350)
        Me.Text = "Quên Mật Khẩu"
        Me.StartPosition = FormStartPosition.CenterScreen

        ' --- STEP 1 PANEL (Nhập Email) ---
        Me.pnlStep1.Location = New Point(10, 10)
        Me.pnlStep1.Size = New Size(330, 330)
        Me.pnlStep1.Visible = True ' Hiện đầu tiên

        ' Label Email
        Me.lblEmail.Text = "Nhập Email đăng ký:"
        Me.lblEmail.Location = New Point(10, 50)
        Me.lblEmail.AutoSize = True
        Me.pnlStep1.Controls.Add(Me.lblEmail)

        ' Textbox Email
        Me.txtEmail.Location = New Point(10, 75)
        Me.txtEmail.Size = New Size(300, 25)
        Me.pnlStep1.Controls.Add(Me.txtEmail)

        ' Button Get Code
        Me.btnGetCode.Text = "Lấy mã xác nhận"
        Me.btnGetCode.Location = New Point(10, 115)
        Me.btnGetCode.Size = New Size(300, 40)
        Me.pnlStep1.Controls.Add(Me.btnGetCode)

        ' Label Error Step 1
        Me.lblError.ForeColor = Color.Red
        Me.lblError.Location = New Point(10, 160)
        Me.lblError.Size = New Size(300, 40)
        Me.pnlStep1.Controls.Add(Me.lblError)

        Me.Controls.Add(Me.pnlStep1)

        ' --- STEP 2 PANEL (Nhập OTP & Pass Mới) ---
        Me.pnlStep2.Location = New Point(10, 10)
        Me.pnlStep2.Size = New Size(330, 330)
        Me.pnlStep2.Visible = False ' Mặc định ẩn

        ' Instruction
        Me.lblInstructions.Text = "Vui lòng kiểm tra email và nhập mã."
        Me.lblInstructions.Location = New Point(10, 10)
        Me.lblInstructions.AutoSize = True
        Me.lblInstructions.ForeColor = Color.Blue
        Me.pnlStep2.Controls.Add(Me.lblInstructions)

        ' OTP
        Me.lblOTP.Text = "Mã OTP (6 số):"
        Me.lblOTP.Location = New Point(10, 40)
        Me.lblOTP.AutoSize = True
        Me.pnlStep2.Controls.Add(Me.lblOTP)

        Me.txtOTP.Location = New Point(10, 60)
        Me.txtOTP.Size = New Size(150, 25)
        Me.pnlStep2.Controls.Add(Me.txtOTP)

        ' New Password
        Me.lblNewPass.Text = "Mật khẩu mới:"
        Me.lblNewPass.Location = New Point(10, 100)
        Me.lblNewPass.AutoSize = True
        Me.pnlStep2.Controls.Add(Me.lblNewPass)

        Me.txtNewPass.Location = New Point(10, 120)
        Me.txtNewPass.Size = New Size(300, 25)
        Me.txtNewPass.PasswordChar = "*"c
        Me.pnlStep2.Controls.Add(Me.txtNewPass)

        ' Confirm Password
        Me.lblConfirmPass.Text = "Nhập lại mật khẩu:"
        Me.lblConfirmPass.Location = New Point(10, 160)
        Me.lblConfirmPass.AutoSize = True
        Me.pnlStep2.Controls.Add(Me.lblConfirmPass)

        Me.txtConfirmPass.Location = New Point(10, 180)
        Me.txtConfirmPass.Size = New Size(300, 25)
        Me.txtConfirmPass.PasswordChar = "*"c
        Me.pnlStep2.Controls.Add(Me.txtConfirmPass)

        ' Button Confirm
        Me.btnConfirm.Text = "Đổi Mật Khẩu"
        Me.btnConfirm.Location = New Point(10, 220)
        Me.btnConfirm.Size = New Size(300, 40)
        Me.pnlStep2.Controls.Add(Me.btnConfirm)

        ' Button Back
        Me.btnBack.Text = "Quay lại"
        Me.btnBack.Location = New Point(10, 270)
        Me.btnBack.Size = New Size(300, 30)
        Me.pnlStep2.Controls.Add(Me.btnBack)

        ' Label Error Step 2
        Me.lblErrorStep2.ForeColor = Color.Red
        Me.lblErrorStep2.Location = New Point(10, 310)
        Me.lblErrorStep2.AutoSize = True
        Me.pnlStep2.Controls.Add(Me.lblErrorStep2)

        Me.Controls.Add(Me.pnlStep2)

        Me.ResumeLayout(False)
    End Sub

End Class