Public Class FrmLibraryManager
    Private Sub btnBooks_Click(sender As Object, e As EventArgs) Handles btnBooks.Click
        Dim frm As New FrmBookManagement()
        Me.Hide()
        frm.ShowDialog()
        Me.Show()
    End Sub

    Private Sub btnAuthors_Click(sender As Object, e As EventArgs) Handles btnAuthors.Click
        Dim frm As New FrmAuthorList()
        Me.Hide()
        frm.ShowDialog()
        Me.Show()
    End Sub
End Class