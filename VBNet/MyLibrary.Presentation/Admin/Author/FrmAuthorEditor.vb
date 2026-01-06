Imports MyLibrary.BLL
Imports MyLibrary.BLL.BusinessAccessLayer.Services
Imports MyLibrary.BLL.DTOs

Public Class FrmAuthorEditor

    Private ReadOnly _authorId As Integer
    Private ReadOnly _service As IAuthorService
    Private ReadOnly _cloudinaryService As CloudinaryService


    Private _currentUrl As String = ""
    Private _localImagePath As String = ""
    Private _isImageChanged As Boolean = False

    Public Sub New(id As Integer, service As IAuthorService, cloudService As CloudinaryService)
        InitializeComponent()
        _authorId = id
        _service = service
        _cloudinaryService = cloudService
    End Sub

    Private Async Sub FrmAuthorEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _authorId = 0 Then
            Me.Text = "Thêm Tác Giả Mới"
            btnSave.Text = "Thêm mới"
            dtpBirthDate.Value = DateTime.Now
            picAvatar.Image = Nothing
        Else
            Me.Text = "Cập Nhật Tác Giả"
            btnSave.Text = "Lưu thay đổi"
            Await LoadAuthorData()
        End If
    End Sub

    Private Async Function LoadAuthorData() As Task
        Try
            Dim author = Await _service.GetByIdAsync(_authorId)
            If author IsNot Nothing Then
                txtName.Text = author.AuthorName
                txtBio.Text = author.Biography
                txtNation.Text = author.Nationality
                If author.BirthDate.HasValue Then dtpBirthDate.Value = author.BirthDate.Value

                _currentUrl = author.Avatar
                If Not String.IsNullOrEmpty(_currentUrl) Then
                    picAvatar.ImageLocation = _currentUrl
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Lỗi tải dữ liệu: " & ex.Message)
            Me.Close()
        End Try
    End Function

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Using dialog As New OpenFileDialog()
            dialog.Title = "Chọn ảnh đại diện"
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif"
            If dialog.ShowDialog() = DialogResult.OK Then
                picAvatar.ImageLocation = dialog.FileName ' Hiển thị preview
                _localImagePath = dialog.FileName
                _isImageChanged = True
            End If
        End Using
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If String.IsNullOrWhiteSpace(txtName.Text) Then
                MessageBox.Show("Vui lòng nhập tên tác giả.")
                Return
            End If

            btnSave.Enabled = False
            btnSave.Text = "Đang xử lý..."
            Cursor = Cursors.WaitCursor

            Dim finalAvatarUrl As String = _currentUrl

            If _isImageChanged AndAlso Not String.IsNullOrEmpty(_localImagePath) Then
                Dim uploadedUrl = Await _cloudinaryService.UploadImageAsync(_localImagePath, "authors")
                If Not String.IsNullOrEmpty(uploadedUrl) Then
                    finalAvatarUrl = uploadedUrl
                End If
            End If


            Dim dto As New AuthorDto With {
                .AuthorName = txtName.Text.Trim(),
                .Biography = txtBio.Text.Trim(),
                .Nationality = txtNation.Text.Trim(),
                .BirthDate = dtpBirthDate.Value,
                .Avatar = finalAvatarUrl
            }


            If _authorId = 0 Then
                Await _service.AddAsync(dto)
                MessageBox.Show("Thêm mới thành công!")
            Else
                Await _service.UpdateAsync(_authorId, dto)
                MessageBox.Show("Cập nhật thành công!")
            End If


            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Có lỗi xảy ra: " & ex.Message)
        Finally
            btnSave.Enabled = True
            btnSave.Text = If(_authorId = 0, "Thêm mới", "Lưu thay đổi")
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class