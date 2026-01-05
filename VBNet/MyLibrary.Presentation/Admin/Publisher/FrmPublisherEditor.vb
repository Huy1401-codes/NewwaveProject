Imports MyLibrary.BLL
Imports MyLibrary.BLL.BusinessAccessLayer.Services

Public Class FrmPublisherEditor

    Private ReadOnly _publisherId As Integer
    Private ReadOnly _service As IPublisherService
    Private ReadOnly _cloudinaryService As CloudinaryService

    Private _currentUrl As String = ""
    Private _localImagePath As String = ""
    Private _isImageChanged As Boolean = False

    Public Sub New(id As Integer, service As IPublisherService, cloudService As CloudinaryService)
        InitializeComponent()
        _publisherId = id
        _service = service
        _cloudinaryService = cloudService
    End Sub

    Private Sub FrmPublisherEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _publisherId = 0 Then
            Me.Text = "Thêm Nhà Xuất Bản Mới"
            btnSave.Text = "Thêm mới"
        Else
            Me.Text = "Cập Nhật Nhà Xuất Bản"
            btnSave.Text = "Lưu thay đổi"
            LoadCategoryData()
        End If
    End Sub

    Private Async Sub LoadCategoryData()
        Try
            Dim category = Await _service.GetByIdAsync(_publisherId)
            If category IsNot Nothing Then
                txtName.Text = category.PublisherName
            End If
        Catch ex As Exception
            MessageBox.Show("Lỗi tải dữ liệu: " & ex.Message)
            Me.Close()
        End Try
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If String.IsNullOrWhiteSpace(txtName.Text) Then
                MessageBox.Show("Vui lòng nhập tên Nhà Xuất Bản.")
                Return
            End If

            btnSave.Enabled = False
            btnSave.Text = "Đang xử lý..."
            Cursor = Cursors.WaitCursor


            If _isImageChanged AndAlso Not String.IsNullOrEmpty(_localImagePath) Then
                Dim uploadedUrl = Await _cloudinaryService.UploadImageAsync(_localImagePath, "categorys")
            End If

            Dim dto As New PublisherDto With {
                .PublisherName = txtName.Text.Trim()
            }


            If _publisherId = 0 Then
                Await _service.AddAsync(dto)
                MessageBox.Show("Thêm mới thành công!")
            Else
                Await _service.UpdateAsync(_publisherId, dto)
                MessageBox.Show("Cập nhật thành công!")
            End If


            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Có lỗi xảy ra: " & ex.Message)
        Finally
            btnSave.Enabled = True
            btnSave.Text = If(_publisherId = 0, "Thêm mới", "Lưu thay đổi")
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class