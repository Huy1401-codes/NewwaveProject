Imports MyLibrary.BLL
Imports MyLibrary.DAL

Public Class FrmAdminApproval
    Inherits Form

    Private _uow As UnitOfWork
    Private _borrowService As BorrowService

    Private _currentPage As Integer = 1
    Private _pageSize As Integer = 10
    Private _totalPages As Integer = 1

    Public Sub New()
        InitializeComponent()
        _uow = New UnitOfWork()
        _borrowService = New BorrowService(_uow)
    End Sub

    Private Sub FrmAdminApproval_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboSort.Items.Clear()
        cboSort.Items.Add("Mới nhất")
        cboSort.Items.Add("Cũ nhất")
        cboSort.SelectedIndex = 0

        LoadData()
    End Sub

    Private Async Sub LoadData()
        Try
            Dim sortOrder = cboSort.SelectedItem.ToString()

            Dim result = Await _borrowService.GetPendingListPagedAsync(sortOrder, _currentPage, _pageSize)

            dgvRequests.DataSource = result.Items
            _totalPages = result.TotalPages

            lblPageInfo.Text = $"Trang {_currentPage} / {If(_totalPages = 0, 1, _totalPages)}"

            btnPrev.Enabled = (_currentPage > 1)
            btnNext.Enabled = (_currentPage < _totalPages)

            FormatGrid()

        Catch ex As Exception
            MessageBox.Show("Lỗi tải dữ liệu: " & ex.Message)
        End Try
    End Sub

    Private Sub FormatGrid()
        Dim hiddenCols = {"Id", "UserId", "Status", "FineAmount", "UpdatedAt", "StatusDisplay"}
        For Each colName In hiddenCols
            If dgvRequests.Columns(colName) IsNot Nothing Then
                dgvRequests.Columns(colName).Visible = False
            End If
        Next

        If dgvRequests.Columns("UserName") IsNot Nothing Then
            With dgvRequests.Columns("UserName")
                .HeaderText = "Người mượn"
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
        End If

        If dgvRequests.Columns("BookList") IsNot Nothing Then
            With dgvRequests.Columns("BookList")
                .HeaderText = "Sách đăng ký mượn"
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                .MinimumWidth = 200
            End With
        End If

        If dgvRequests.Columns("CreatedAt") IsNot Nothing Then
            With dgvRequests.Columns("CreatedAt")
                .HeaderText = "Thời gian gửi"
                .Width = 140
                .DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        End If

        If dgvRequests.Columns("DueDate") IsNot Nothing Then
            With dgvRequests.Columns("DueDate")
                .HeaderText = "Hẹn trả"
                .Width = 100
                .DefaultCellStyle.Format = "dd/MM/yyyy"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        End If

        dgvRequests.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9.75!, FontStyle.Bold)
        dgvRequests.ColumnHeadersHeight = 35
        dgvRequests.EnableHeadersVisualStyles = False
        dgvRequests.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
        dgvRequests.RowTemplate.Height = 30
    End Sub

    Private Sub cboSort_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSort.SelectedIndexChanged
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

    Private Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        ProcessRequest(True)
    End Sub

    Private Sub btnReject_Click(sender As Object, e As EventArgs) Handles btnReject.Click
        ProcessRequest(False)
    End Sub

    Private Async Sub ProcessRequest(isApproved As Boolean)
        If dgvRequests.SelectedRows.Count = 0 Then
            MessageBox.Show("Vui lòng chọn phiếu cần xử lý.")
            Return
        End If

        Dim row = dgvRequests.SelectedRows(0)
        Dim ticketId = CInt(row.Cells("Id").Value)

        Dim actionText = If(isApproved, "DUYỆT", "TỪ CHỐI")

        If MessageBox.Show($"Bạn chắc chắn muốn {actionText} phiếu này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _borrowService.ApproveBorrowAsync(ticketId, isApproved)

                MessageBox.Show("Thành công!")
                LoadData()
            Catch ex As Exception
                MessageBox.Show("Lỗi: " & ex.Message)
            End Try
        End If
    End Sub
End Class