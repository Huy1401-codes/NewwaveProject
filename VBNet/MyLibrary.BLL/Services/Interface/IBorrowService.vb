Imports MyLibrary.Domain.MyApp.Domain.Enums

Public Interface IBorrowService
    '  User gửi yêu cầu mượn
    Sub RequestBorrow(userId As Integer, bookIds As List(Of Integer), dueDate As DateTime)

    ' Admin duyệt hoặc từ chối
    Sub ApproveBorrow(ticketId As Integer, isApproved As Boolean)

    ' Trả sách 
    Sub ReturnBook(ticketId As Integer, paymentMethod As PaymentMethod)

    ' User xem lịch sử bản thân
    Function GetMyHistory(userId As Integer) As List(Of BorrowTicketDto)

    ' Tính toán tiền phạt trước khi trả (để hiển thị lên màn hình)
    Function CalculateFine(ticketId As Integer) As Decimal

    ' Hiển thị danh sách book
    Function GetPendingListPaged(sortOrder As String, page As Integer, pageSize As Integer) As PagedResult(Of BorrowTicketDto)
End Interface