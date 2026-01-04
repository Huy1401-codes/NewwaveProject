Imports MyLibrary.Domain.MyApp.Domain.Enums

Public Interface IBorrowService

    Sub RequestBorrow(userId As Integer, bookIds As List(Of Integer), dueDate As DateTime)

    Sub ApproveBorrow(ticketId As Integer, isApproved As Boolean)

    Sub ReturnBook(ticketId As Integer, paymentMethod As PaymentMethod)

    Function GetMyHistory(userId As Integer) As List(Of BorrowTicketDto)

    Function CalculateFine(ticketId As Integer) As Decimal

    Function GetPendingListPaged(sortOrder As String, page As Integer, pageSize As Integer) As PagedResult(Of BorrowTicketDto)
End Interface