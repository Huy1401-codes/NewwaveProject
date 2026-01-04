Public Class FrmAdminMain

    Private Sub FrmAdminMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If SessionManager.CurrentUser IsNot Nothing Then
            lblWelcome.Text = $"Xin chào, Admin: {SessionManager.CurrentUser.FullName}"
        End If
    End Sub

    Private Sub btnManageBooks_Click(sender As Object, e As EventArgs) Handles btnManageBooks.Click
        Dim frm As New FrmLibraryManager()
        Me.Hide()
        frm.ShowDialog()
        Me.Show()
    End Sub

    Private Sub btnManageBorrow_Click(sender As Object, e As EventArgs) Handles btnManageBorrow.Click
        Dim frm As New FrmAdminApproval()
        Me.Hide()
        frm.ShowDialog()
        Me.Show()
    End Sub

    Private Sub btnManageUsers_Click(sender As Object, e As EventArgs) Handles btnManageUsers.Click
        Dim frm As New FrmUserManagement()
        Me.Hide()
        frm.ShowDialog()
        Me.Show()
    End Sub

    Private Sub btnStatistics_Click(sender As Object, e As EventArgs) Handles btnStatistics.Click
        Dim frm As New FrmStatistics()
        Me.Hide()
        frm.ShowDialog()
        Me.Show()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            SessionManager.CurrentUser = Nothing
            Dim login As New FrmLogin()
            login.Show()
            Me.Close()
        End If
    End Sub

End Class