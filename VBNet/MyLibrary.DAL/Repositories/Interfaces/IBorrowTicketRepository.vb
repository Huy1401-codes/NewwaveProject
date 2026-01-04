Imports MyLibrary.Domain

Public Interface IBorrowTicketRepository
    Inherits IGenericRepository(Of BorrowTicket)

    ' Lấy lịch sử mượn của User
    Function GetHistoryByUserId(userId As Integer) As IEnumerable(Of BorrowTicket)

    ' Lấy danh sách chờ duyệt (cho Admin)
    Function GetPendingRequests() As IEnumerable(Of BorrowTicket)

    ' [MỚI] Lấy chi tiết phiếu mượn (kèm thông tin sách và user) để xử lý trả sách
    Function GetByIdWithDetails(ticketId As Integer) As BorrowTicket

    ' Thêm dòng này vào Interface
    Function GetPendingRequestsPaged(isNewest As Boolean, pageIndex As Integer, pageSize As Integer) As (List(Of BorrowTicket), Integer)

    Function GetCurrentBorrows(userId As Integer) As List(Of BorrowTicket)
End Interface