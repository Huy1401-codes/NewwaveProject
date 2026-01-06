Imports AutoMapper
Imports MyLibrary.BLL
Imports MyLibrary.BLL.BusinessAccessLayer.Services
Imports MyLibrary.DAL

Public Class FrmAuthorList
    Private ReadOnly _authorService As AuthorService
    Private ReadOnly _bookService As BookService
    Private ReadOnly _cloudinaryService As CloudinaryService

    Private _currentPage As Integer = 1
    Private _pageSize As Integer = 10
    Private _totalPages As Integer = 1

    Public Sub New()
        InitializeComponent()
        Dim config As New MapperConfiguration(Sub(cfg)
                                                  cfg.AddProfile(New AuthorMappingProfile())
                                              End Sub)
        Dim mapper As IMapper = config.CreateMapper()
        Dim uow As New UnitOfWork()
        _authorService = New AuthorService(uow, mapper)
        _cloudinaryService = New CloudinaryService
        _bookService = New BookService(uow, mapper)
    End Sub

    Private Sub FrmAuthorList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupGrid()
        LoadData()
    End Sub

    Private Sub SetupGrid()
        dgvAuthors.AutoGenerateColumns = False
        dgvAuthors.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAuthors.MultiSelect = False
        dgvAuthors.ReadOnly = True
        dgvAuthors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill


        dgvAuthors.Columns.Add("Id", "ID")
        dgvAuthors.Columns("Id").DataPropertyName = "Id"
        dgvAuthors.Columns("Id").Visible = False

        dgvAuthors.Columns.Add("AuthorName", "Tên tác giả")
        dgvAuthors.Columns("AuthorName").DataPropertyName = "AuthorName"

        dgvAuthors.Columns.Add("Nationality", "Quốc tịch")
        dgvAuthors.Columns("Nationality").DataPropertyName = "Nationality"

        dgvAuthors.Columns.Add("BirthDate", "Ngày sinh")
        dgvAuthors.Columns("BirthDate").DataPropertyName = "BirthDate"
    End Sub

    Private Async Sub LoadData()
        Try
            Dim result = Await _authorService.GetPagedAsync(txtSearch.Text.Trim(), _currentPage, _pageSize)

            dgvAuthors.DataSource = result.Items
            _totalPages = result.TotalPages
            lblPageInfo.Text = $"Trang {_currentPage} / {_totalPages}"

            btnPrev.Enabled = (_currentPage > 1)
            btnNext.Enabled = (_currentPage < _totalPages)

            Me.BackColor = Color.WhiteSmoke
            Dim lbl As New Label()
            lbl.Text = "QUẢN LÝ TÁC GIẢ"
            lbl.Font = New Font("Segoe UI", 14, FontStyle.Bold)
            lbl.AutoSize = True
            lbl.Location = New Point(20, 20)
            Me.Controls.Add(lbl)
        Catch ex As Exception
            MessageBox.Show("Lỗi tải dữ liệu: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
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
        Using frm As New FrmAuthorEditor(0, _authorService, _cloudinaryService)
            If frm.ShowDialog() = DialogResult.OK Then
                LoadData()
            End If
        End Using
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim id = GetSelectedId()
        If id = 0 Then Return

        Using frm As New FrmAuthorEditor(id, _authorService, _cloudinaryService)
            If frm.ShowDialog() = DialogResult.OK Then
                LoadData()
            End If
        End Using
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim id = GetSelectedId()
        If id = 0 Then Return

        If MessageBox.Show("Bạn có chắc chắn muốn xóa tác giả này?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Await _authorService.DeleteAsync(id)
                MessageBox.Show("Xóa thành công!")
                LoadData()
            Catch ex As Exception
                MessageBox.Show("Lỗi xóa: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnDetail_Click(sender As Object, e As EventArgs) Handles btnDetail.Click
        Dim id = GetSelectedId()
        If id = 0 Then Return

        Using frm As New FrmAuthorDetail(id, _authorService, _bookService)
            frm.ShowDialog()
        End Using
    End Sub

    Private Function GetSelectedId() As Integer
        If dgvAuthors.SelectedRows.Count = 0 Then
            MessageBox.Show("Vui lòng chọn một tác giả trong danh sách.")
            Return 0
        End If
        Return Convert.ToInt32(dgvAuthors.SelectedRows(0).Cells("Id").Value)
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FrmLibraryManager.Show()
        Me.Close()
    End Sub
End Class