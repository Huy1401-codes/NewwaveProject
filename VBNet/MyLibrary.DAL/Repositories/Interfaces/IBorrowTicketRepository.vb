Imports MyLibrary.Domain

Public Interface IBorrowTicketRepository
    Inherits IGenericRepository(Of BorrowTicket)

    Function GetHistoryByUserIdAsync(userId As Integer) As Task(Of IEnumerable(Of BorrowTicket))

    Function GetPendingRequestsAsync() As Task(Of IEnumerable(Of BorrowTicket))

    Function GetByIdWithDetailsAsync(ticketId As Integer) As Task(Of BorrowTicket)

    Function GetPendingRequestsPagedAsync(isNewest As Boolean, pageIndex As Integer,
                                          pageSize As Integer) As Task(Of (List(Of BorrowTicket), Integer))

    Function GetCurrentBorrowsAsync(userId As Integer) As Task(Of List(Of BorrowTicket))
End Interface