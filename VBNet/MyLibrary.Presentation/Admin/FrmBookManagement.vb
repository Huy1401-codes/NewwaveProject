Imports MyLibrary.BLL
Imports MyLibrary.DAL
Imports AutoMapper
Public Class FrmBookManagement

    ' Dependency & State
    Private _bookService As BookService
    Private _currentBookId As Integer = 0

    ' Constructor
    Public Sub New()
        ' Gọi hàm từ file Designer để vẽ giao diện
        InitializeComponent()

        ' Khởi tạo Dependency Injection (Composition Root)
        Dim config As New MapperConfiguration(Sub(cfg)
                                                  cfg.AddProfile(New BookMappingProfile())
                                              End Sub)
        Dim mapper As IMapper = config.CreateMapper()
        Dim uow As New UnitOfWork()

        ' Inject Service
        _bookService = New BookService(uow, mapper)
    End Sub

    ' --- LOAD FORM ---
    Private Sub FrmBookManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadComboBoxes()
        LoadData()
        ResetForm()
    End Sub

    ' --- HELPERS ---
    Private Sub LoadComboBoxes()
        ' Load Authors
        cboAuthor.DataSource = _bookService.GetAuthors()
        cboAuthor.DisplayMember = "AuthorName"
        cboAuthor.ValueMember = "AuthorId"
        cboAuthor.SelectedIndex = -1

        ' Load Categories
        cboCategory.DataSource = _bookService.GetCategories()
        cboCategory.DisplayMember = "CategoryName"
        cboCategory.ValueMember = "CategoryId"
        cboCategory.SelectedIndex = -1

        ' Load Publishers
        cboPublisher.DataSource = _bookService.GetPublishers()
        cboPublisher.DisplayMember = "PublisherName"
        cboPublisher.ValueMember = "PublisherId"
        cboPublisher.SelectedIndex = -1
    End Sub

    Private Sub LoadData(Optional keyword As String = "")
        Dim list As List(Of BookDto)

        If String.IsNullOrEmpty(keyword) Then
            list = _bookService.GetAllBooksForView()
        Else
            list = _bookService.SearchBooks(keyword)
        End If

        dgvBooks.DataSource = list

        ' Format Grid
        If dgvBooks.Columns("BookId") IsNot Nothing Then dgvBooks.Columns("BookId").Visible = False
        If dgvBooks.Columns("ImagePath") IsNot Nothing Then dgvBooks.Columns("ImagePath").Visible = False

        dgvBooks.Columns("Title").HeaderText = "Tên Sách"
        dgvBooks.Columns("BookCode").HeaderText = "Mã Sách"
        dgvBooks.Columns("AuthorName").HeaderText = "Tác Giả"
        dgvBooks.Columns("CategoryName").HeaderText = "Thể Loại"
        dgvBooks.Columns("PublisherName").HeaderText = "NXB"
        dgvBooks.Columns("Price").HeaderText = "Giá"
        dgvBooks.Columns("Quantity").HeaderText = "Tổng SL"
        dgvBooks.Columns("AvailableQuantity").HeaderText = "Khả dụng"

        dgvBooks.Columns("Price").DefaultCellStyle.Format = "N0"
    End Sub

    Private Sub ResetForm()
        _currentBookId = 0
        txtCode.Enabled = True
        txtCode.Clear()
        txtTitle.Clear()
        txtPrice.Text = "0"
        txtQuantity.Text = "0"
        txtYear.Clear()
        txtSearch.Clear()

        cboAuthor.SelectedIndex = -1
        cboCategory.SelectedIndex = -1
        cboPublisher.SelectedIndex = -1

        btnAdd.Enabled = True
        btnEdit.Enabled = False
        btnDelete.Enabled = False
    End Sub

    Private Function ValidateInput() As Boolean
        If String.IsNullOrWhiteSpace(txtCode.Text) Then
            MessageBox.Show("Vui lòng nhập Mã sách")
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtTitle.Text) Then
            MessageBox.Show("Vui lòng nhập Tên sách")
            Return False
        End If

        Dim p As Decimal
        If Not Decimal.TryParse(txtPrice.Text, p) OrElse p < 0 Then
            MessageBox.Show("Giá tiền phải là số dương")
            Return False
        End If

        Dim q As Integer
        If Not Integer.TryParse(txtQuantity.Text, q) OrElse q < 0 Then
            MessageBox.Show("Số lượng phải là số nguyên dương")
            Return False
        End If

        If cboAuthor.SelectedIndex = -1 Then
            MessageBox.Show("Vui lòng chọn Tác giả")
            Return False
        End If

        Return True
    End Function

    ' --- EVENTS ---

    ' 1. Click Grid -> Fill Data
    Private Sub dgvBooks_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBooks.CellClick
        If e.RowIndex < 0 Then Return

        Dim row = dgvBooks.Rows(e.RowIndex)
        _currentBookId = Convert.ToInt32(row.Cells("BookId").Value)

        Dim book = _bookService.GetBookById(_currentBookId)
        If book IsNot Nothing Then
            txtCode.Text = book.BookCode
            txtCode.Enabled = False
            txtTitle.Text = book.Title
            txtPrice.Text = If(book.Price.HasValue, book.Price.Value.ToString("0"), "0")
            txtQuantity.Text = book.Quantity.ToString()
            txtYear.Text = If(book.PublishYear.HasValue, book.PublishYear.Value.ToString(), "")

            cboAuthor.SelectedValue = If(book.AuthorId, -1)
            cboCategory.SelectedValue = If(book.CategoryId, -1)
            cboPublisher.SelectedValue = If(book.PublisherId, -1)

            btnAdd.Enabled = False
            btnEdit.Enabled = True
            btnDelete.Enabled = True
        End If
    End Sub

    ' 2. Button Thêm
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            If Not ValidateInput() Then Return

            Dim book As New Book With {
                .BookCode = txtCode.Text.Trim(),
                .Title = txtTitle.Text.Trim(),
                .Price = Decimal.Parse(txtPrice.Text),
                .Quantity = Integer.Parse(txtQuantity.Text),
                .PublishYear = If(String.IsNullOrWhiteSpace(txtYear.Text), Nothing, Integer.Parse(txtYear.Text)),
                .AuthorId = Convert.ToInt32(cboAuthor.SelectedValue),
                .CategoryId = Convert.ToInt32(cboCategory.SelectedValue),
                .PublisherId = Convert.ToInt32(cboPublisher.SelectedValue),
                .ImagePath = ""
            }

            _bookService.AddBook(book)
            MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ResetForm()
            LoadData()
        Catch ex As Exception
            MessageBox.Show("Lỗi: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' 3. Button Sửa
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If _currentBookId = 0 Then Return
        Try
            If Not ValidateInput() Then Return

            Dim book = _bookService.GetBookById(_currentBookId)

            book.Title = txtTitle.Text.Trim()
            book.Price = Decimal.Parse(txtPrice.Text)
            book.Quantity = Integer.Parse(txtQuantity.Text)
            book.PublishYear = If(String.IsNullOrWhiteSpace(txtYear.Text), Nothing, Integer.Parse(txtYear.Text))
            book.AuthorId = Convert.ToInt32(cboAuthor.SelectedValue)
            book.CategoryId = Convert.ToInt32(cboCategory.SelectedValue)
            book.PublisherId = Convert.ToInt32(cboPublisher.SelectedValue)

            _bookService.UpdateBook(book)
            MessageBox.Show("Cập nhật sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ResetForm()
            LoadData()
        Catch ex As Exception
            MessageBox.Show("Lỗi: " & ex.Message)
        End Try
    End Sub

    ' 4. Button Xóa
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If _currentBookId = 0 Then Return

        Dim result = MessageBox.Show($"Bạn có chắc muốn xóa sách: {txtTitle.Text}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.Yes Then
            Try
                _bookService.DeleteBook(_currentBookId)
                MessageBox.Show("Đã xóa sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ResetForm()
                LoadData()
            Catch ex As Exception
                MessageBox.Show("Lỗi khi xóa: " & ex.Message)
            End Try
        End If
    End Sub

    ' 5. Button Tìm kiếm
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadData(txtSearch.Text.Trim())
    End Sub

    ' 6. Button Làm mới
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ResetForm()
        LoadData()
    End Sub

End Class