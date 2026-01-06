Imports MyLibrary.BLL
Imports MyLibrary.DAL
Imports MyLibrary.Domain.MyApp.Domain.Enums

Public Class FrmMyHistory
    Inherits Form

    Private _uow As UnitOfWork
    Private _borrowService As BorrowService
    Public Sub New()
        InitializeComponent()
        _uow = New UnitOfWork()
        _borrowService = New BorrowService(_uow)
    End Sub

    Private Sub FrmMyHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadData()
    End Sub

    Private Async Sub LoadData()
        Try
            If SessionManager.CurrentUser Is Nothing Then
                MessageBox.Show("Bạn chưa đăng nhập!")
                Me.Close()
                Return
            End If

            Dim userId = SessionManager.CurrentUser.UserId
            Dim listTickets = Await _borrowService.GetMyHistoryAsync(userId)

            dgvHistory.DataSource = listTickets
            FormatGrid()

        Catch ex As Exception
            MessageBox.Show("Lỗi tải dữ liệu: " & ex.Message)
        End Try
    End Sub

    Private Sub FormatGrid()
        Dim hiddenCols = {"Id", "UserName", "Status", "FineAmount"}
        For Each col In hiddenCols
            If dgvHistory.Columns(col) IsNot Nothing Then dgvHistory.Columns(col).Visible = False
        Next

        If dgvHistory.Columns("BookList") IsNot Nothing Then
            dgvHistory.Columns("BookList").HeaderText = "Sách đã mượn"
            dgvHistory.Columns("BookList").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If

        If dgvHistory.Columns("BorrowDate") IsNot Nothing Then
            dgvHistory.Columns("BorrowDate").HeaderText = "Ngày mượn"
            dgvHistory.Columns("BorrowDate").Width = 100
            dgvHistory.Columns("BorrowDate").DefaultCellStyle.Format = "dd/MM/yyyy"
        End If

        If dgvHistory.Columns("DueDate") IsNot Nothing Then
            dgvHistory.Columns("DueDate").HeaderText = "Hạn trả"
            dgvHistory.Columns("DueDate").Width = 100
            dgvHistory.Columns("DueDate").DefaultCellStyle.Format = "dd/MM/yyyy"
        End If

        If dgvHistory.Columns("StatusDisplay") IsNot Nothing Then
            dgvHistory.Columns("StatusDisplay").HeaderText = "Trạng thái"
            dgvHistory.Columns("StatusDisplay").Width = 150
        End If
    End Sub

    Private Async Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        If dgvHistory.SelectedRows.Count = 0 Then
            MessageBox.Show("Vui lòng chọn phiếu mượn muốn trả.")
            Return
        End If

        Dim row = dgvHistory.SelectedRows(0)
        Dim statusDisplay = ""
        If row.Cells("StatusDisplay").Value IsNot Nothing Then
            statusDisplay = row.Cells("StatusDisplay").Value.ToString()
        End If

        If statusDisplay.Contains("Đã trả") OrElse statusDisplay.Contains("từ chối") OrElse statusDisplay.Contains("Chờ duyệt") Then
            MessageBox.Show("Bạn chỉ có thể trả sách khi trạng thái là 'Đang mượn' (hoặc Quá hạn).")
            Return
        End If

        Dim ticketId = CInt(row.Cells("Id").Value)

        Try


            Dim fineAmount As Decimal = Await _borrowService.CalculateFineAsync(ticketId)
            Dim msg As String = "Bạn xác nhận trả các sách trong phiếu này?"

            If fineAmount > 0 Then
                msg &= vbCrLf & $"⚠ LƯU Ý: Bạn đã quá hạn trả sách!" & vbCrLf &
                       $"Số tiền phạt cần thanh toán: {fineAmount:N0} VNĐ." & vbCrLf &
                       $"Chọn YES để xác nhận thanh toán (Tiền mặt) và trả sách."
            End If

            If MessageBox.Show(msg, "Xác nhận trả sách", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                Await _borrowService.ReturnBookAsync(ticketId, PaymentMethod.Cash)

                MessageBox.Show("Trả sách thành công! Cảm ơn bạn.")
                LoadData()
            End If

        Catch ex As Exception
            MessageBox.Show("Lỗi trả sách: " & ex.Message)
        End Try
    End Sub

End Class