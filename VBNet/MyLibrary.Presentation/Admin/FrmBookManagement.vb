Imports System.IO
Imports AutoMapper
Imports MyLibrary.BLL
Imports MyLibrary.BLL.BusinessAccessLayer.Services
Imports MyLibrary.BusinessAccessLayer.Services
Imports MyLibrary.DAL
Imports MyLibrary.Domain

Public Class FrmBookManagement

    Private _bookService As BookService
    Private _cloudinaryService As ICloudinaryService

    Private _currentPage As Integer = 1
    Private _pageSize As Integer = 15
    Private _totalPages As Integer = 0

    Public Sub New()
        InitializeComponent()

        Dim config As New MapperConfiguration(Sub(cfg)
                                                  cfg.AddProfile(New BookMappingProfile())
                                              End Sub)
        Dim mapper As IMapper = config.CreateMapper()
        Dim uow As New UnitOfWork()

        _bookService = New BookService(uow, mapper)
        _cloudinaryService = New CloudinaryService()
    End Sub

    Private Sub FrmBookManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigGrid()
        LoadFilters()

        LoadData()
    End Sub

    Private Sub ConfigGrid()
        dgvBooks.AutoGenerateColumns = False
        dgvBooks.Columns.Clear()

        AddColumn("Mã sách", "BookCode", 100)
        AddColumn("Tên sách", "Title", 250)
        AddColumn("Tác giả", "AuthorName", 150)
        AddColumn("Thể loại", "CategoryName", 120)
        AddColumn("NXB", "PublisherName", 150)
        AddColumn("Năm", "PublishYear", 80)
        AddColumn("Giá", "Price", 100, True, "N0")
        AddColumn("SL", "Quantity", 60)
        AddColumn("Id", "Id", 0, False)

        Dim btnDetail As New DataGridViewButtonColumn()
        btnDetail.HeaderText = "Chi tiết"
        btnDetail.Name = "btnDetail"
        btnDetail.Text = "Xem"
        btnDetail.UseColumnTextForButtonValue = True
        btnDetail.Width = 60
        dgvBooks.Columns.Add(btnDetail)
    End Sub

    Private Sub AddColumn(header As String, propName As String, width As Integer,
                         Optional visible As Boolean = True, Optional format As String = "")
        Dim col As New DataGridViewTextBoxColumn()
        col.HeaderText = header
        col.DataPropertyName = propName
        col.Width = width
        col.Visible = visible
        col.Name = propName
        If Not String.IsNullOrEmpty(format) Then
            col.DefaultCellStyle.Format = format
        End If
        dgvBooks.Columns.Add(col)
    End Sub

    Private Sub LoadFilters()
        Dim publishers = _bookService.GetPublishers()
        publishers.Insert(0, New Publisher With {.Id = 0, .PublisherName = "Tất cả NXB"})

        cboFilterPublisher.DataSource = publishers
        cboFilterPublisher.DisplayMember = "PublisherName"
        cboFilterPublisher.ValueMember = "Id"
        cboFilterPublisher.SelectedIndex = 0

        cboFilterYear.Items.Clear()
        cboFilterYear.Items.Add("Tất cả Năm")
        Dim currentYear = DateTime.Now.Year
        For i As Integer = currentYear To currentYear - 50 Step -1
            cboFilterYear.Items.Add(i)
        Next
        cboFilterYear.SelectedIndex = 0
    End Sub

    Private Sub LoadData()
        Dim keyword As String = txtSearch.Text.Trim()

        Dim pubId As Integer? = Nothing
        If cboFilterPublisher.SelectedValue IsNot Nothing Then
            Dim val As Integer
            If Integer.TryParse(cboFilterPublisher.SelectedValue.ToString(), val) AndAlso val > 0 Then
                pubId = val
            End If
        End If

        Dim year As Integer? = Nothing
        If cboFilterYear.SelectedItem IsNot Nothing AndAlso IsNumeric(cboFilterYear.SelectedItem) Then
            year = Convert.ToInt32(cboFilterYear.SelectedItem)
        End If

        Dim result = _bookService.GetBooksCombined(keyword, pubId, year, _currentPage, _pageSize)

        dgvBooks.DataSource = result.Items
        _totalPages = result.TotalPages

        lblPageInfo.Text = $"Trang {_currentPage} / {If(_totalPages = 0, 1, _totalPages)} (Tổng: {result.TotalCount} sách)"
        btnPrev.Enabled = (_currentPage > 1)
        btnNext.Enabled = (_currentPage < _totalPages)
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        _currentPage = 1
        LoadData()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        _currentPage = 1
        LoadData()
    End Sub

    Private Sub cboFilterPublisher_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cboFilterPublisher.SelectionChangeCommitted
        _currentPage = 1
        LoadData()
    End Sub

    Private Sub cboFilterYear_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cboFilterYear.SelectionChangeCommitted
        _currentPage = 1
        LoadData()
    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        If _currentPage > 1 Then
            _currentPage -= 1
            LoadData()
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If _currentPage < _totalPages Then
            _currentPage += 1
            LoadData()
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Using frm As New FrmBookEditor(_bookService, _cloudinaryService, 0)
            If frm.ShowDialog() = DialogResult.OK Then
                LoadData()
            End If
        End Using
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If dgvBooks.SelectedRows.Count = 0 Then Return
        Dim selectedId As Integer = Convert.ToInt32(dgvBooks.SelectedRows(0).Cells("Id").Value)

        Using frm As New FrmBookEditor(_bookService, _cloudinaryService, selectedId)
            If frm.ShowDialog() = DialogResult.OK Then
                LoadData()
            End If
        End Using
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgvBooks.SelectedRows.Count = 0 Then Return
        Dim selectedId As Integer = Convert.ToInt32(dgvBooks.SelectedRows(0).Cells("Id").Value)

        If MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Try
                _bookService.DeleteBook(selectedId)
                LoadData()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub

    Private Sub dgvBooks_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBooks.CellDoubleClick
        If e.RowIndex >= 0 Then btnEdit.PerformClick()
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        Dim books As List(Of BookDto) = _bookService.GetAllBooksForView()
        Dim fileBytes = _bookService.ExportBooksToExcel(books)

        Dim saveFile As New SaveFileDialog() With {
            .Filter = "Excel Files|*.xlsx",
            .FileName = "Books.xlsx"
        }

        If saveFile.ShowDialog() = DialogResult.OK Then
            File.WriteAllBytes(saveFile.FileName, fileBytes)
            MessageBox.Show("Export thành công !")
        End If
    End Sub

    Private Sub dgvBooks_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBooks.CellContentClick

        If e.RowIndex >= 0 AndAlso dgvBooks.Columns(e.ColumnIndex).Name = "btnDetail" Then

            Dim bookId As Integer = Convert.ToInt32(dgvBooks.Rows(e.RowIndex).Cells("Id").Value)

            Using frm As New FrmBookEditor(_bookService, _cloudinaryService, bookId, True)
                frm.ShowDialog()
            End Using
        End If
    End Sub
End Class