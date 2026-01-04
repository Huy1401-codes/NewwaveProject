Imports MyLibrary.Domain.MyApp.Domain.Enums

Public Interface IBorrowService
    ' 1. User gửi yêu cầu mượn
    Sub RequestBorrow(userId As Integer, bookIds As List(Of Integer), dueDate As DateTime)

    ' 2. Admin duyệt hoặc từ chối
    Sub ApproveBorrow(ticketId As Integer, isApproved As Boolean)

    ' 3. Trả sách & Thanh toán phạt (nếu có)
    Sub ReturnBook(ticketId As Integer, paymentMethod As PaymentMethod)

    ' 5. User xem lịch sử bản thân
    Function GetMyHistory(userId As Integer) As List(Of BorrowTicketDto)

    ' 6. Tính toán tiền phạt trước khi trả (để hiển thị lên màn hình)
    Function CalculateFine(ticketId As Integer) As Decimal

    ' Thêm dòng này
    Function GetPendingListPaged(sortOrder As String, page As Integer, pageSize As Integer) As PagedResult(Of BorrowTicketDto)
    Function GetMyCurrentBorrows(userId As Integer) As List(Of BorrowTicketDto)
End Interface