Public Class FrmAdminMain

    Private Sub FrmAdminMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblWelcome.Text = $"Welcome Admin: {SessionManager.CurrentUser.Email}"
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        SessionManager.CurrentUser = Nothing

        Dim login As New FrmLogin()
        login.Show()

        Me.Close()
    End Sub

End Class
