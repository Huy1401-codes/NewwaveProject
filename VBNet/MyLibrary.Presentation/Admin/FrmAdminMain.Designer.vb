Partial Class FrmAdminMain
    Inherits Form

    Private Sub InitializeComponent()
        Me.lblWelcome = New Label()
        Me.btnLogout = New Button()

        Me.lblWelcome.Location = New Point(20, 20)
        Me.lblWelcome.Size = New Size(300, 30)

        Me.btnLogout.Location = New Point(20, 60)
        Me.btnLogout.Text = "Logout"

        Me.ClientSize = New Size(400, 200)
        Me.Controls.AddRange({lblWelcome, btnLogout})
        Me.Text = "Admin Dashboard"
    End Sub

    Friend WithEvents lblWelcome As Label
    Friend WithEvents btnLogout As Button
End Class
