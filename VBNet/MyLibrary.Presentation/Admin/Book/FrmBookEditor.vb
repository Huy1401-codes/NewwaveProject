Imports System.IO
Imports MyLibrary.BLL
Imports MyLibrary.Domain

Public Class FrmBookEditor

    Private _bookService As BookService
    Private _cloudinaryService As ICloudinaryService
    Private _bookId As Integer = 0
    Private _currentLocalPath As String = ""
    Private _currentUrl As String = ""
    Private _isImageChanged As Boolean = False
    Private _isViewOnly As Boolean = False
    Public Sub New(service As BookService, cloudService As ICloudinaryService,
                   Optional id As Integer = 0, Optional isViewOnly As Boolean = False)
        InitializeComponent()
        _bookService = service
        _cloudinaryService = cloudService
        _bookId = id
        _isViewOnly = isViewOnly
    End Sub

    Private Sub FrmBookEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadComboBoxes()

        If _bookId > 0 Then
            Me.Text = "CẬP NHẬT SÁCH"
            LoadDataToEdit()
        Else
            Me.Text = "THÊM SÁCH MỚI"
            ResetForm()
        End If

        If _isViewOnly Then
            lblTitle.Text = "CHI TIẾT SÁCH"
            btnSave.Visible = False
            btnBrowse.Visible = False


            txtCode.ReadOnly = True
            txtTitle.ReadOnly = True
            txtPrice.ReadOnly = True
            txtQuantity.ReadOnly = True
            txtYear.ReadOnly = True
            txtAvailableQty.ReadOnly = True
            cboAuthor.Enabled = False
            cboCategory.Enabled = False
            cboPublisher.Enabled = False
        End If
    End Sub

    Private Sub LoadComboBoxes()
        cboAuthor.DataSource = _bookService.GetAuthors()
        cboAuthor.DisplayMember = "AuthorName"
        cboAuthor.ValueMember = "Id"

        cboCategory.DataSource = _bookService.GetCategories()
        cboCategory.DisplayMember = "CategoryName"
        cboCategory.ValueMember = "Id"

        cboPublisher.DataSource = _bookService.GetPublishers()
        cboPublisher.DisplayMember = "PublisherName"
        cboPublisher.ValueMember = "Id"
    End Sub

    Private Sub ResetForm()
        txtCode.Clear()
        txtTitle.Clear()
        txtPrice.Text = "0"
        txtQuantity.Text = "0"
        txtAvailableQty.Text = "0"
        txtYear.Clear()
        cboAuthor.SelectedIndex = -1
        cboCategory.SelectedIndex = -1
        cboPublisher.SelectedIndex = -1
        picCover.Image = Nothing
        _currentLocalPath = ""
        _currentUrl = ""
        _isImageChanged = False
    End Sub

    Private Sub LoadDataToEdit()
        Dim book = _bookService.GetBookById(_bookId)
        If book IsNot Nothing Then
            txtCode.Text = book.BookCode
            txtCode.Enabled = False
            txtTitle.Text = book.Title
            txtPrice.Text = If(book.Price.HasValue, book.Price.Value.ToString("0"), "0")
            txtQuantity.Text = book.Quantity.ToString()
            txtYear.Text = If(book.PublishYear.HasValue, book.PublishYear.Value.ToString(), "")
            txtAvailableQty.Text = book.AvailableQuantity.ToString()
            cboAuthor.SelectedValue = If(book.AuthorId, -1)
            cboCategory.SelectedValue = If(book.CategoryId, -1)
            cboPublisher.SelectedValue = If(book.PublisherId, -1)

            _currentUrl = book.ImagePath
            If Not String.IsNullOrEmpty(_currentUrl) Then
                Try
                    picCover.LoadAsync(_currentUrl)
                Catch ex As Exception
                    picCover.Image = Nothing
                End Try
            Else
                picCover.Image = Nothing
            End If
        End If
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        If ofdImage.ShowDialog() = DialogResult.OK Then
            _currentLocalPath = ofdImage.FileName

            Using fs As New FileStream(_currentLocalPath, FileMode.Open, FileAccess.Read)
                picCover.Image = Image.FromStream(fs)
            End Using

            _isImageChanged = True
        End If
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If String.IsNullOrWhiteSpace(txtCode.Text) Then
                MessageBox.Show("Vui lòng nhập Mã sách")
                Return
            End If

            If String.IsNullOrWhiteSpace(txtTitle.Text) Then
                MessageBox.Show("Vui lòng nhập Tên sách")
                Return
            End If

            Dim price As Decimal
            If Not Decimal.TryParse(txtPrice.Text.Trim(), price) OrElse price < 0 Then
                MessageBox.Show("Giá tiền phải là số không âm")
                Return
            End If

            Dim quantity As Integer
            If Not Integer.TryParse(txtQuantity.Text.Trim(), quantity) OrElse quantity < 0 Then
                MessageBox.Show("Số lượng phải là số nguyên không âm")
                Return
            End If

            Dim availableQty As Integer
            If Not Integer.TryParse(txtAvailableQty.Text.Trim(), availableQty) OrElse availableQty < 0 Then
                MessageBox.Show("Số lượng còn phải là số nguyên không âm")
                Return
            End If

            If availableQty > quantity Then
                MessageBox.Show(
                "Số lượng còn không được lớn hơn tổng số lượng",
                "Không hợp lệ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
                Return
            End If

            If _bookId > 0 Then
                Dim oldBook = _bookService.GetBookById(_bookId)
                If oldBook Is Nothing Then
                    MessageBox.Show("Không tìm thấy sách")
                    Return
                End If

                Dim borrowed = oldBook.Quantity - oldBook.AvailableQuantity
                If quantity < borrowed Then
                    MessageBox.Show(
                    $"Số lượng mới ({quantity}) không được nhỏ hơn số đang cho mượn ({borrowed})",
                    "Không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)
                    Return
                End If
            End If

            Dim year As Integer? = Nothing
            If Not String.IsNullOrWhiteSpace(txtYear.Text) Then
                Dim tempYear As Integer
                If Not Integer.TryParse(txtYear.Text.Trim(), tempYear) Then
                    MessageBox.Show("Năm xuất bản phải là số nguyên")
                    Return
                End If
                year = tempYear
            End If

            btnSave.Enabled = False
            btnSave.Text = "Đang xử lý..."
            Me.Cursor = Cursors.WaitCursor

            Dim finalImageUrl As String = _currentUrl
            If _isImageChanged AndAlso Not String.IsNullOrEmpty(_currentLocalPath) Then
                Dim uploadedUrl = Await _cloudinaryService.UploadImageAsync(_currentLocalPath, "book-covers")
                If Not String.IsNullOrEmpty(uploadedUrl) Then
                    finalImageUrl = uploadedUrl
                End If
            End If

            Dim book As New BookDto With {
            .Id = _bookId,
            .BookCode = txtCode.Text.Trim(),
            .Title = txtTitle.Text.Trim(),
            .Price = price,
            .Quantity = quantity,
            .AvailableQuantity = availableQty,
            .PublishYear = year,
            .AuthorId = CInt(cboAuthor.SelectedValue),
            .CategoryId = CInt(cboCategory.SelectedValue),
            .PublisherId = CInt(cboPublisher.SelectedValue),
            .ImagePath = finalImageUrl
        }

            If _bookId = 0 Then
                _bookService.AddBook(book)
                MessageBox.Show("Thêm mới thành công!")
            Else
                _bookService.UpdateBook(book)
                MessageBox.Show("Cập nhật thành công!")
            End If

            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Lỗi: " & ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnSave.Enabled = True
            btnSave.Text = "LƯU"
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub


End Class