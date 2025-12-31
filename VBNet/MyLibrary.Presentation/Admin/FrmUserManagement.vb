Imports MyLibrary.BLL
Imports MyLibrary.DAL

Public Class FrmUserManagement

    Private ReadOnly _userService As UserService

    Private _currentPage As Integer = 1
    Private _pageSize As Integer = 20
    Private _totalPages As Integer = 1
    Public Sub New()
        InitializeComponent()
        Dim uow As New UnitOfWork()

        _userService = New UserService(uow)
    End Sub

    Private Sub FrmUserManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupGrid()
        LoadCombos()
        LoadData()
    End Sub

    Private Sub SetupGrid()
        dgvUsers.AutoGenerateColumns = False
        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvUsers.MultiSelect = False
        dgvUsers.ReadOnly = True
        dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvUsers.RowTemplate.Height = 35

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Id",
            .HeaderText = "ID",
            .Width = 50
        })

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "FullName",
            .HeaderText = "Họ và tên"
        })

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Email",
            .HeaderText = "Email"
        })

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Phone",
            .HeaderText = "SĐT",
            .Width = 120
        })

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "RoleNames",
            .HeaderText = "Quyền hạn",
            .Width = 150
        })

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "IsActive",
            .Name = "colIsActive",
            .Visible = False
        })

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "colStatusText",
            .HeaderText = "Trạng thái",
            .Width = 120
        })
    End Sub

    Private Sub LoadCombos()
        Dim statuses = New List(Of Object) From {
            New With {.Value = -1, .Text = "-- Tất cả --"},
            New With {.Value = 1, .Text = "Đang hoạt động"},
            New With {.Value = 0, .Text = "Đã khóa"}
        }

        cboStatus.DataSource = statuses
        cboStatus.DisplayMember = "Text"
        cboStatus.ValueMember = "Value"
        cboStatus.SelectedIndex = 0
    End Sub

    Private Sub LoadData()
        Try
            ' Lấy giá trị lọc Status
            Dim statusFilter As Boolean? = Nothing
            Dim selectedStatus = CInt(cboStatus.SelectedValue)

            If selectedStatus <> -1 Then
                statusFilter = (selectedStatus = 1)
            End If

            Dim result = _userService.GetPaged(txtSearch.Text.Trim(), statusFilter, _currentPage, _pageSize)

            dgvUsers.DataSource = result.Items

            _totalPages = CInt(result.TotalPages)
            lblPageInfo.Text = $"Trang {_currentPage} / {_totalPages}"

            btnPrev.Enabled = (_currentPage > 1)
            btnNext.Enabled = (_currentPage < _totalPages)

        Catch ex As Exception
            MessageBox.Show("Lỗi tải dữ liệu: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvUsers_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvUsers.CellFormatting
        ' Kiểm tra nếu đang render cột "Trạng thái" (cột cuối cùng mình thêm vào)
        If dgvUsers.Columns(e.ColumnIndex).Name = "colStatusText" Then
            Dim row = dgvUsers.Rows(e.RowIndex)
            Dim userDto = TryCast(row.DataBoundItem, UserDto)

            If userDto IsNot Nothing Then
                If userDto.IsActive Then
                    e.Value = "Đang hoạt động"
                    e.CellStyle.ForeColor = Color.Green
                Else
                    e.Value = "Đã khóa"
                    e.CellStyle.ForeColor = Color.Red
                    e.CellStyle.Font = New Font(e.CellStyle.Font, FontStyle.Italic)

                    row.DefaultCellStyle.BackColor = Color.WhiteSmoke
                End If
            End If
        End If
    End Sub

    Private Sub btnToggleStatus_Click(sender As Object, e As EventArgs) Handles btnToggleStatus.Click
        If dgvUsers.SelectedRows.Count = 0 Then
            MessageBox.Show("Vui lòng chọn một tài khoản để thao tác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedRow = dgvUsers.SelectedRows(0)
        Dim user = TryCast(selectedRow.DataBoundItem, UserDto)

        If user Is Nothing Then Return

        ' Xác định hành động sắp làm
        Dim actionName As String = If(user.IsActive, "KHÓA (Vô hiệu hóa)", "MỞ KHÓA (Khôi phục)")
        Dim msg As String = $"Bạn có chắc chắn muốn {actionName} tài khoản [{user.Email}] không?"

        If MessageBox.Show(msg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                _userService.ToggleStatus(user.Id)
                MessageBox.Show("Thao tác thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Tải lại dữ liệu để cập nhật trạng thái mới
                LoadData()
            Catch ex As Exception
                MessageBox.Show("Lỗi: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' --- 6. CÁC SỰ KIỆN TÌM KIẾM & PHÂN TRANG ---
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        _currentPage = 1 ' Reset về trang 1 khi tìm kiếm mới
        LoadData()
    End Sub

    ' Cho phép nhấn Enter ở ô tìm kiếm
    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnSearch.PerformClick()
        End If
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

End Class