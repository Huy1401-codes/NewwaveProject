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

        Dim frm As FrmAdminMain = Nothing
        For Each f As Form In Application.OpenForms
            If TypeOf f Is FrmAdminMain Then
                frm = CType(f, FrmAdminMain)
                Exit For
            End If
        Next

        If frm IsNot Nothing Then
            Me.Close()
            frm.BringToFront()
        Else
            frm = New FrmAdminMain()
            Me.Close()
            frm.Show()
        End If
    End Sub


    Private Sub btnReaders_Click(sender As Object, e As EventArgs) Handles btnReaders.Click
        If Not Me.DesignMode Then
            Dim frm As New FrmCategoryList()
            Me.Hide()
            frm.ShowDialog()
            Me.Show()
        End If
    End Sub
End Class