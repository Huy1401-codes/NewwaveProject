Imports MyLibrary.Domain

Public Class BorrowTicketRepository
    Inherits GenericRepository(Of BorrowTicket)
    Implements IBorrowTicketRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    ' Hàm 1: Lấy lịch sử user
    Public Function GetHistoryByUserId(userId As Integer) As IEnumerable(Of BorrowTicket) _
        Implements IBorrowTicketRepository.GetHistoryByUserId

        Return _dbSet.Include("BorrowDetails.Book") _
                     .Where(Function(t) t.UserId = userId) _
                     .OrderByDescending(Function(t) t.CreatedAt) _
                     .ToList()
    End Function

    ' Hàm 2: Lấy danh sách chờ duyệt
    Public Function GetPendingRequests() As IEnumerable(Of BorrowTicket) _
        Implements IBorrowTicketRepository.GetPendingRequests

        Return _dbSet.Include("User") _
                     .Include("BorrowDetails.Book") _
                     .Where(Function(t) t.Status = BorrowStatus.Pending) _
                     .OrderBy(Function(t) t.CreatedAt) _
                     .ToList()
    End Function

    ' Hàm 3 [MỚI]: Lấy 1 phiếu cụ thể kèm full thông tin (Dùng cho chức năng Trả sách/Duyệt)
    Public Function GetByIdWithDetails(ticketId As Integer) As BorrowTicket _
        Implements IBorrowTicketRepository.GetByIdWithDetails

        Return _dbSet.Include("User") _
                     .Include("BorrowDetails.Book") _
                     .FirstOrDefault(Function(t) t.Id = ticketId)
    End Function

    ' Thêm hàm này vào Class (Implement Interface)
    Public Function GetPendingRequestsPaged(isNewest As Boolean, pageIndex As Integer, pageSize As Integer) As (List(Of BorrowTicket), Integer) _
        Implements IBorrowTicketRepository.GetPendingRequestsPaged

        ' 1. Tạo câu truy vấn (chưa execute)
        Dim query = _dbSet.Include("User") _
                          .Include("BorrowDetails.Book") _
                          .Where(Function(t) t.Status = BorrowStatus.Pending)

        ' 2. Xử lý Lọc (Mới nhất / Cũ nhất)
        If isNewest Then
            query = query.OrderByDescending(Function(t) t.CreatedAt)
        Else
            query = query.OrderBy(Function(t) t.CreatedAt)
        End If

        ' 3. Đếm tổng số lượng (để tính số trang)
        Dim totalCount = query.Count()

        ' 4. Phân trang (Chỉ lấy đúng số lượng cần thiết)
        Dim items = query.Skip((pageIndex - 1) * pageSize) _
                         .Take(pageSize) _
                         .ToList()

        Return (items, totalCount)
    End Function

    Public Function GetCurrentBorrows(userId As Integer) As List(Of BorrowTicket) _
        Implements IBorrowTicketRepository.GetCurrentBorrows
        Return _dbSet.Include("BorrowDetails.Book") _
                     .Where(Function(t) t.UserId = userId And t.Status = BorrowStatus.Approved) _
                     .ToList()
    End Function
End Class