Imports MyLibrary.BLL
Imports MyLibrary.BLL.BusinessAccessLayer.Services

Public Class FrmCategoryEditor

    Private ReadOnly _categoryId As Integer
    Private ReadOnly _service As ICategoryService
    Private ReadOnly _cloudinaryService As CloudinaryService

    Private _currentUrl As String = ""
    Private _localImagePath As String = ""
    Private _isImageChanged As Boolean = False

    Public Sub New(id As Integer, service As ICategoryService, cloudService As CloudinaryService)
        InitializeComponent()
        _categoryId = id
        _service = service
        _cloudinaryService = cloudService
    End Sub

    Private Sub FrmCategoryEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _categoryId = 0 Then
            Me.Text = "Thêm Danh Mục Mới"
            btnSave.Text = "Thêm mới"
        Else
            Me.Text = "Cập Nhật Danh Mục"
            btnSave.Text = "Lưu thay đổi"
            LoadCategoryData()
        End If
    End Sub

    Private Async Sub LoadCategoryData()
        Try
            Dim category = Await _service.GetByIdAsync(_categoryId)
            If category IsNot Nothing Then
                txtName.Text = category.CategoryName
            End If
        Catch ex As Exception
            MessageBox.Show("Lỗi tải dữ liệu: " & ex.Message)
            Me.Close()
        End Try
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If String.IsNullOrWhiteSpace(txtName.Text) Then
                MessageBox.Show("Vui lòng nhập tên Danh Mục.")
                Return
            End If

            btnSave.Enabled = False
            btnSave.Text = "Đang xử lý..."
            Cursor = Cursors.WaitCursor


            If _isImageChanged AndAlso Not String.IsNullOrEmpty(_localImagePath) Then
                Dim uploadedUrl = Await _cloudinaryService.UploadImageAsync(_localImagePath, "categorys")
            End If

            Dim dto As New CategoryDto With {
                .CategoryName = txtName.Text.Trim()
            }


            If _categoryId = 0 Then
                Await _service.AddAsync(dto)
                MessageBox.Show("Thêm mới thành công!")
            Else
                Await _service.UpdateAsync(_categoryId, dto)
                MessageBox.Show("Cập nhật thành công!")
            End If


            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Có lỗi xảy ra: " & ex.Message)
        Finally
            btnSave.Enabled = True
            btnSave.Text = If(_categoryId = 0, "Thêm mới", "Lưu thay đổi")
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class