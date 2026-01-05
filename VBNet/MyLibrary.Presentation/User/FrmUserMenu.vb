Public Class FrmUserMenu
    Private Sub btnBooks_Click(sender As Object, e As EventArgs) Handles btnBooks.Click
        If Not Me.DesignMode Then
            Dim frm As New FrmAvailableBooks()
            Me.Hide()
            frm.ShowDialog()
            Me.Show()
        End If
    End Sub

    Private Sub btnMyHistory_Click(sender As Object, e As EventArgs) Handles btnMyHistory.Click
        If Not Me.DesignMode Then
            Dim frm As New FrmMyHistory()
            Me.Hide()
            frm.ShowDialog()
            Me.Show()
        End If
    End Sub

    'Private Sub btnPayReturn_Click(sender As Object, e As EventArgs) Handles btnPayReturn.Click
    '    If Not Me.DesignMode Then
    '        Dim frm As New FrmUserReturnBook()
    '        Me.Hide()
    '        frm.ShowDialog()
    '        Me.Show()
    '    End If

    'End Sub

    Private Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        Dim result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            SessionManager.CurrentUser = Nothing
            Dim login As New FrmLogin()
            login.Show()
            Me.Close()
        End If
    End Sub
End Class