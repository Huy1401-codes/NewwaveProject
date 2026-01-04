Imports MyLibrary.BLL
Imports MyLibrary.Domain

Public Class FrmAuthorDetail
    Private ReadOnly _authorId As Integer
    Private ReadOnly _service As IAuthorService
    Private ReadOnly _serviceBook As IBookService
    Private ReadOnly _imageCache As New Dictionary(Of String, Image)

    Private _currentPage As Integer = 1
    Private _pageSize As Integer = 5
    Private _totalPages As Integer = 1
    Private _currentUrl As String = ""

    Private _currentKeyword As String = ""
    Private _currentPublisherId As Integer? = Nothing
    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(id As Integer, service As IAuthorService, serviceBook As IBookService)
        Me.New()
        _authorId = id
        _service = service
        _serviceBook = serviceBook
    End Sub

    Private Sub FrmAuthorDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupGrid()
        LoadPublishers()
        LoadData()
    End Sub

    Private Sub SetupGrid()
        dgvBooks.AutoGenerateColumns = False
        dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvBooks.ReadOnly = True
        dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgvBooks.Columns.Add("BookCode", "Mã sách")
        dgvBooks.Columns("BookCode").DataPropertyName = "BookCode"

        Dim imgCol As New DataGridViewImageColumn()
        imgCol.Name = "Image"
        imgCol.HeaderText = "Hình ảnh"
        imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom
        imgCol.Width = 90
        dgvBooks.Columns.Add(imgCol)

        dgvBooks.Columns.Add("Title", "Tên sách")
        dgvBooks.Columns("Title").DataPropertyName = "Title"

        dgvBooks.Columns.Add("CategoryName", "Thể loại")
        dgvBooks.Columns("CategoryName").DataPropertyName = "CategoryName"

        dgvBooks.Columns.Add("PublisherName", "Nhà xuất bản")
        dgvBooks.Columns("PublisherName").DataPropertyName = "PublisherName"

        dgvBooks.Columns.Add("PublishYear", "Năm XB")
        dgvBooks.Columns("PublishYear").DataPropertyName = "PublishYear"

        dgvBooks.Columns.Add("Price", "Giá")
        dgvBooks.Columns("Price").DataPropertyName = "Price"
        dgvBooks.Columns("Price").DefaultCellStyle.Format = "N0"
    End Sub

    Private Sub LoadPublishers()

        Dim publishers = _serviceBook.GetPublishers()
        publishers.Insert(0, New Publisher With {.Id = 0, .PublisherName = "Tất cả NXB"})

        cboPublisher.DataSource = publishers
        cboPublisher.DisplayMember = "PublisherName"
        cboPublisher.ValueMember = "Id"
        cboPublisher.SelectedIndex = 0
    End Sub

    Private Sub LoadData()
        Try
            Dim author = _service.GetById(_authorId)

            Dim detail = _service.GetDetail(_authorId,
                                            _currentKeyword,
                                            _currentPage,
                                            _pageSize,
                                            _currentPublisherId)

            lblAuthorName.Text = detail.AuthorName.ToUpper()
            Me.Text = "Chi tiết: " & detail.AuthorName

            _currentUrl = author.Avatar
            If Not String.IsNullOrEmpty(_currentUrl) Then
                picAvatar.ImageLocation = _currentUrl
            End If

            dgvBooks.DataSource = detail.Books.Items

            _totalPages = detail.Books.TotalPages
            lblPageInfo.Text = $"Trang {_currentPage} / {_totalPages}"

            btnPrev.Enabled = (_currentPage > 1)
            btnNext.Enabled = (_currentPage < _totalPages)

        Catch ex As Exception
            MessageBox.Show("Lỗi: " & ex.Message)
        End Try
    End Sub


    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        _currentKeyword = txtSearchBook.Text.Trim()

        Dim pubId = Convert.ToInt32(cboPublisher.SelectedValue)
        If pubId > 0 Then
            _currentPublisherId = pubId
        Else
            _currentPublisherId = Nothing
        End If


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
    Private Sub dgvBooks_CellFormatting(
    sender As Object,
    e As DataGridViewCellFormattingEventArgs) Handles dgvBooks.CellFormatting

        If dgvBooks.Columns(e.ColumnIndex).Name = "Image" AndAlso e.RowIndex >= 0 Then

            Dim book = TryCast(dgvBooks.Rows(e.RowIndex).DataBoundItem, AuthorBookDto)
            If book Is Nothing Then Return

            If String.IsNullOrWhiteSpace(book.ImagePath) Then
                e.Value = Nothing
                Return
            End If

            If _imageCache.ContainsKey(book.ImagePath) Then
                e.Value = _imageCache(book.ImagePath)
                Return
            End If

            Try
                Using wc As New Net.WebClient()
                    Dim bytes = wc.DownloadData(book.ImagePath)
                    Using ms As New IO.MemoryStream(bytes)
                        Dim img = Image.FromStream(ms)
                        _imageCache(book.ImagePath) = img
                        e.Value = img
                    End Using
                End Using
            Catch
                e.Value = Nothing
            End Try
        End If
    End Sub

End Class