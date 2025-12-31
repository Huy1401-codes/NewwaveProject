Imports AutoMapper
Imports MyLibrary.BLL
Imports MyLibrary.BLL.BusinessAccessLayer.Services
Imports MyLibrary.DAL

Public Class FrmCategoryList
    Private ReadOnly _categoryService As CategoryService
    Private ReadOnly _bookService As BookService
    Private ReadOnly _cloudinaryService As CloudinaryService

    Private _currentPage As Integer = 1
    Private _pageSize As Integer = 10
    Private _totalPages As Integer = 1

    Public Sub New()
        InitializeComponent()
        Dim config As New MapperConfiguration(Sub(cfg)
                                                  cfg.AddProfile(New CategoryMappingProfile())
                                              End Sub)
        Dim mapper As IMapper = config.CreateMapper()
        Dim uow As New UnitOfWork()
        _categoryService = New CategoryService(uow, mapper)
        _cloudinaryService = New CloudinaryService
        _bookService = New BookService(uow, mapper)
    End Sub

    Private Sub FrmCategoryList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupGrid()
        LoadData()
    End Sub

    Private Sub SetupGrid()
        dgvCategories.AutoGenerateColumns = False
        dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvCategories.MultiSelect = False
        dgvCategories.ReadOnly = True
        dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill


        dgvCategories.Columns.Add("Id", "ID")
        dgvCategories.Columns("Id").DataPropertyName = "Id"
        dgvCategories.Columns("Id").Visible = False

        dgvCategories.Columns.Add("CategoryName", "Tên danh mục")
        dgvCategories.Columns("CategoryName").DataPropertyName = "CategoryName"

        dgvCategories.Columns.Add("CreatedAt", "Ngày tạo")
        dgvCategories.Columns("CreatedAt").DataPropertyName = "CreatedAt"

    End Sub

    Private Sub LoadData()
        Try
            Dim result = _categoryService.GetPaged(txtSearch.Text.Trim(), _currentPage, _pageSize)

            dgvCategories.DataSource = result.Items
            _totalPages = result.TotalPages
            lblPageInfo.Text = $"Trang {_currentPage} / {_totalPages}"

            btnPrev.Enabled = (_currentPage > 1)
            btnNext.Enabled = (_currentPage < _totalPages)

            Me.BackColor = Color.WhiteSmoke
            Dim lbl As New Label()
            lbl.Text = "QUẢN LÝ DANH MỤC"
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
        Using frm As New FrmCategoryEditor(0, _categoryService, _cloudinaryService)
            If frm.ShowDialog() = DialogResult.OK Then
                LoadData()
            End If
        End Using
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim id = GetSelectedId()
        If id = 0 Then Return

        Using frm As New FrmCategoryEditor(id, _categoryService, _cloudinaryService)
            If frm.ShowDialog() = DialogResult.OK Then
                LoadData()
            End If
        End Using
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim id = GetSelectedId()
        If id = 0 Then Return

        If MessageBox.Show("Bạn có chắc chắn muốn xóa danh mục này?", "Cảnh báo",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                _categoryService.Delete(id)
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

        Using frm As New FrmCategoryDetail(id, _categoryService, _bookService)
            frm.ShowDialog()
        End Using
    End Sub

    Private Function GetSelectedId() As Integer
        If dgvCategories.SelectedRows.Count = 0 Then
            MessageBox.Show("Vui lòng chọn một danh mục trong danh sách.")
            Return 0
        End If
        Return Convert.ToInt32(dgvCategories.SelectedRows(0).Cells("Id").Value)
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FrmLibraryManager.Show()
        Me.Close()
    End Sub
End Class