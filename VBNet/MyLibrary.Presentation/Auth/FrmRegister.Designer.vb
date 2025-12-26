<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmRegister
    Inherits Form

    Private components As System.ComponentModel.IContainer

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then components.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Private Sub InitializeComponent()
        Me.txtFullName = New TextBox()
        Me.txtEmail = New TextBox()
        Me.txtPassword = New TextBox()
        Me.txtConfirm = New TextBox()
        Me.btnRegister = New Button()
        Me.lblError = New Label()

        Me.SuspendLayout()

        Me.txtFullName.Location = New Point(40, 40)
        Me.txtFullName.Size = New Size(280, 23)
        Me.txtFullName.Text = "Full name:"


        Me.txtEmail.Location = New Point(40, 80)
        Me.txtEmail.Size = New Size(280, 23)

        Me.txtPassword.Location = New Point(40, 120)
        Me.txtPassword.PasswordChar = "*"c
        Me.txtPassword.Size = New Size(280, 23)

        Me.txtConfirm.Location = New Point(40, 160)
        Me.txtConfirm.PasswordChar = "*"c
        Me.txtConfirm.Size = New Size(280, 23)

        Me.btnRegister.Location = New Point(40, 200)
        Me.btnRegister.Size = New Size(280, 35)
        Me.btnRegister.Text = "Register"

        Me.lblError.ForeColor = Color.Red
        Me.lblError.Location = New Point(40, 245)
        Me.lblError.Size = New Size(280, 30)

        Me.ClientSize = New Size(360, 300)
        Me.Controls.AddRange(New Control() {
            txtFullName, txtEmail, txtPassword, txtConfirm, btnRegister, lblError
        })

        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Register"

        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents txtFullName As TextBox
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents txtConfirm As TextBox
    Friend WithEvents btnRegister As Button
    Friend WithEvents lblError As Label
End Class
