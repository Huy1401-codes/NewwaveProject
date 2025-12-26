Imports MyLibrary.BLL
Imports MyLibrary.DAL

Public Class FrmRegister

    Private ReadOnly _auth As IAuthService

    Public Sub New()
        InitializeComponent()

        _auth = New AuthService(
            New UnitOfWork(),
            New EmailService("huydo272@gmail.com", "seky quuk rnev lyzc")
        )
    End Sub

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Try
            lblError.Text = ""

            If txtPassword.Text <> txtConfirm.Text Then
                Throw New Exception("Password không khớp")
            End If

            _auth.Register(New RegisterRequestDto With {
                .FullName = txtFullName.Text.Trim(),
                .Email = txtEmail.Text.Trim(),
                .Password = txtPassword.Text
            })

            MessageBox.Show("Đăng ký thành công", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

End Class
