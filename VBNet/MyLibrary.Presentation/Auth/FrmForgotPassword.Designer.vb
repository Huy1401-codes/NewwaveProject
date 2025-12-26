<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmForgotPassword
    Inherits Form

    Private Sub InitializeComponent()
        Me.txtEmail = New TextBox()
        Me.btnSend = New Button()

        Me.txtEmail.Location = New Point(40, 40)
        Me.txtEmail.Size = New Size(280, 23)

        Me.btnSend.Location = New Point(40, 80)
        Me.btnSend.Size = New Size(280, 35)
        Me.btnSend.Text = "Send new password"

        Me.ClientSize = New Size(360, 150)
        Me.Controls.AddRange({txtEmail, btnSend})
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Forgot Password"
    End Sub

    Friend WithEvents txtEmail As TextBox
    Friend WithEvents btnSend As Button
End Class
