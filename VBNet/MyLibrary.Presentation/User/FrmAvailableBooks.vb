Imports AutoMapper
Imports MyLibrary.BLL
Imports MyLibrary.DAL
Imports MyLibrary.Domain
Imports NLog

Public Class FrmAvailableBooks
    Private _bookService As BookService
    Private _uow As UnitOfWork

    Private _allBooks As List(Of BookDetailDto)
    Private _currentPage As Integer = 1
    Private _pageSize As Integer = 10

    Public Sub New()
        InitializeComponent()

        Dim config As New MapperConfiguration(Sub(cfg)
                                                  cfg.AddProfile(New BookMappingProfile())
                                              End Sub)
        Dim mapper As IMapper = config.CreateMapper()
        _uow = New UnitOfWork()
        _bookService = New BookService(_uow, mapper)
    End Sub

    Private Sub FrmAvailableBooks_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadFilters()
        LoadData("", Nothing, Nothing)
    End Sub

    Private Sub LoadFilters()
        Dim publishers = _bookService.GetPublishers()
        publishers.Insert(0, New Publisher With {.Id = 0, .PublisherName = "Tất cả NXB"})
        cboPublisher.DataSource = publishers
        cboPublisher.DisplayMember = "PublisherName"
        cboPublisher.ValueMember = "Id"
        cboPublisher.SelectedIndex = 0

        Dim categories = _bookService.GetCategories()
        categories.Insert(0, New Category With {.Id = 0, .CategoryName = "Tất cả Danh Mục"})
        cboCategory.DataSource = categories
        cboCategory.DisplayMember = "CategoryName"
        cboCategory.ValueMember = "Id"
        cboCategory.SelectedIndex = 0
    End Sub

    Private Sub LoadData(keyword As String, Optional publisherId As Integer? = Nothing, Optional categoryId As Integer? = Nothing)
        Try
            _allBooks = _bookService.GetAvailableBooks(keyword, publisherId, categoryId)
            _currentPage = 1
            RenderGrid()
        Catch ex As Exception
            MessageBox.Show("Lỗi tải dữ liệu: " & ex.Message)
        End Try
    End Sub

    Private Sub RenderGrid()
        If _allBooks Is Nothing OrElse _allBooks.Count = 0 Then
            dgvBooks.DataSource = Nothing
            lblPageInfo.Text = "Không tìm thấy sách nào."
            Return
        End If

        Dim totalRecords = _allBooks.Count
        Dim totalPages = CInt(Math.Ceiling(totalRecords / _pageSize))
        Dim pagedData = _allBooks.Skip((_currentPage - 1) * _pageSize).Take(_pageSize).ToList()
        dgvBooks.DataSource = pagedData

        If dgvBooks.Columns("Id") IsNot Nothing Then dgvBooks.Columns("Id").Visible = False
        If dgvBooks.Columns("Description") IsNot Nothing Then dgvBooks.Columns("Description").Visible = False
        If dgvBooks.Columns("ImagePath") IsNot Nothing Then dgvBooks.Columns("ImagePath").Visible = False
        If dgvBooks.Columns("Title") IsNot Nothing Then dgvBooks.Columns("Title").HeaderText = "Tên Sách"
        If dgvBooks.Columns("AuthorName") IsNot Nothing Then dgvBooks.Columns("AuthorName").HeaderText = "Tác Giả"
        If dgvBooks.Columns("AvailableQuantity") IsNot Nothing Then dgvBooks.Columns("AvailableQuantity").HeaderText = "Còn Lại"

        lblPageInfo.Text = $"Trang {_currentPage}/{totalPages}"
        btnPrev.Enabled = _currentPage > 1
        btnNext.Enabled = _currentPage < totalPages
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs)
        ApplyFilters()
    End Sub

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        ApplyFilters()
    End Sub

    Private Sub ApplyFilters()
        Dim keyword As String = txtSearch.Text.Trim()

        Dim publisherId As Integer? = If(CInt(cboPublisher.SelectedValue) > 0, CInt(cboPublisher.SelectedValue), Nothing)
        Dim categoryId As Integer? = If(CInt(cboCategory.SelectedValue) > 0, CInt(cboCategory.SelectedValue), Nothing)

        LoadData(keyword, publisherId, categoryId)
    End Sub
    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        If _currentPage > 1 Then
            _currentPage -= 1
            RenderGrid()
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim totalPages = CInt(Math.Ceiling(_allBooks.Count / _pageSize))
        If _currentPage < totalPages Then
            _currentPage += 1
            RenderGrid()
        End If
    End Sub

    Private Sub dgvBooks_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBooks.CellContentClick
        If e.RowIndex >= 0 AndAlso dgvBooks.Columns(e.ColumnIndex).Name = "colView" Then
            Dim selectedBook = DirectCast(dgvBooks.Rows(e.RowIndex).DataBoundItem, BookDetailDto)
            Dim frmDetail As New FrmBookDetail(selectedBook.Id)
            frmDetail.ShowDialog()
            ApplyFilters()
        End If
    End Sub
End Class
