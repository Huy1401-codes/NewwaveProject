Imports MyLibrary.Domain

Public Class BorrowTicketRepository
    Inherits GenericRepository(Of BorrowTicket)
    Implements IBorrowTicketRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Function GetHistoryByUserId(userId As Integer) As IEnumerable(Of BorrowTicket) _
        Implements IBorrowTicketRepository.GetHistoryByUserId

        Return _dbSet.Include("BorrowDetails.Book") _
                     .Where(Function(t) t.UserId = userId) _
                     .OrderByDescending(Function(t) t.CreatedAt) _
                     .ToList()
    End Function


    Public Function GetPendingRequests() As IEnumerable(Of BorrowTicket) _
        Implements IBorrowTicketRepository.GetPendingRequests

        Return _dbSet.Include("User") _
                     .Include("BorrowDetails.Book") _
                     .Where(Function(t) t.Status = BorrowStatus.Pending) _
                     .OrderBy(Function(t) t.CreatedAt) _
                     .ToList()
    End Function


    Public Function GetByIdWithDetails(ticketId As Integer) As BorrowTicket _
        Implements IBorrowTicketRepository.GetByIdWithDetails

        Return _dbSet.Include("User") _
                     .Include("BorrowDetails.Book") _
                     .FirstOrDefault(Function(t) t.Id = ticketId)
    End Function

    Public Function GetPendingRequestsPaged(isNewest As Boolean, pageIndex As Integer, pageSize As Integer) As (List(Of BorrowTicket), Integer) _
        Implements IBorrowTicketRepository.GetPendingRequestsPaged

        Dim query = _dbSet.Include("User") _
                          .Include("BorrowDetails.Book") _
                          .Where(Function(t) t.Status = BorrowStatus.Pending)
        If isNewest Then
            query = query.OrderByDescending(Function(t) t.CreatedAt)
        Else
            query = query.OrderBy(Function(t) t.CreatedAt)
        End If
        ' 3. Đếm tổng số lượng (để tính số trang)
        Dim totalCount = query.Count()

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