Imports MyLibrary.Domain.MyApp.Domain.Enums

Public Interface IBorrowService
    Function RequestBorrowAsync(userId As Integer, bookIds As List(Of BorrowItemDto), dueDate As DateTime) As Task

    Function ApproveBorrowAsync(ticketId As Integer, isApproved As Boolean) As Task

    Function ReturnBookAsync(ticketId As Integer, paymentMethod As PaymentMethod) As Task

    Function GetMyHistoryAsync(userId As Integer) As Task(Of List(Of BorrowTicketDto))

    Function CalculateFineAsync(ticketId As Integer) As Task(Of Decimal)

    Function GetPendingListPagedAsync(sortOrder As String, page As Integer, pageSize As Integer) As Task(Of PagedResult(Of BorrowTicketDto))
End Interface