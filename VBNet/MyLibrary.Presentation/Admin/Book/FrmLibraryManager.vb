Public Class FrmLibraryManager
    Private Sub btnBooks_Click(sender As Object, e As EventArgs) Handles btnBooks.Click
        If Not Me.DesignMode Then
            Dim frm As New FrmBookManagement()
            Me.Hide()
            frm.ShowDialog()
            Me.Show()
        End If
    End Sub

    Private Sub btnAuthors_Click(sender As Object, e As EventArgs) Handles btnAuthors.Click
        If Not Me.DesignMode Then
            Dim frm As New FrmAuthorList()
            Me.Hide()
            frm.ShowDialog()
            Me.Show()
        End If
    End Sub

    Private Sub btnPublishers_Click(sender As Object, e As EventArgs) Handles btnPublishers.Click
        If Not Me.DesignMode Then
            Dim frm As New FrmPublisherList()
            Me.Hide()
            frm.ShowDialog()
            Me.Show()
        End If
    End Sub

    Private Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        If Not Me.DesignMode Then
            Dim frm As New FrmAdminMain()
            Me.Hide()
            frm.ShowDialog()
            Me.Show()
        End If
    End Sub
End Class