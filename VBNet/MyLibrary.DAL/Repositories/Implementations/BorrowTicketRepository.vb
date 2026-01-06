Imports MyLibrary.Domain
Imports System.Data.Entity ' <-- QUAN TRỌNG: Để dùng ToListAsync, CountAsync...
Imports System.Threading.Tasks

Public Class BorrowTicketRepository
    Inherits GenericRepository(Of BorrowTicket)
    Implements IBorrowTicketRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    'Lấy lịch sử mượn sách của 1 user
    Public Async Function GetHistoryByUserIdAsync(userId As Integer) As Task(Of IEnumerable(Of BorrowTicket)) _
        Implements IBorrowTicketRepository.GetHistoryByUserIdAsync

        Return Await _dbSet.Include("BorrowDetails.Book") _
                           .Where(Function(t) t.UserId = userId) _
                           .OrderByDescending(Function(t) t.CreatedAt) _
                           .ToListAsync()
    End Function

    ' Lấy danh sách yêu cầu mượn giành cho admin
    Public Async Function GetPendingRequestsAsync() As Task(Of IEnumerable(Of BorrowTicket)) _
        Implements IBorrowTicketRepository.GetPendingRequestsAsync

        Return Await _dbSet.Include("User") _
                           .Include("BorrowDetails.Book") _
                           .Where(Function(t) t.Status = BorrowStatus.Pending) _
                           .OrderBy(Function(t) t.CreatedAt) _
                           .ToListAsync()
    End Function

    'Xem thông tin chi tiết của sách mượn của 1 user
    Public Async Function GetByIdWithDetailsAsync(ticketId As Integer) As Task(Of BorrowTicket) _
        Implements IBorrowTicketRepository.GetByIdWithDetailsAsync

        Return Await _dbSet.Include("User") _
                           .Include("BorrowDetails.Book") _
                           .FirstOrDefaultAsync(Function(t) t.Id = ticketId)
    End Function

    ' Lấy danh sách yêu cầu mượn giành cho admin
    Public Async Function GetPendingRequestsPagedAsync(isNewest As Boolean, pageIndex As Integer, pageSize As Integer) _
        As Task(Of (List(Of BorrowTicket), Integer)) _
        Implements IBorrowTicketRepository.GetPendingRequestsPagedAsync

        Dim query = _dbSet.Include("User") _
                          .Include("BorrowDetails.Book") _
                          .Where(Function(t) t.Status = BorrowStatus.Pending)

        If isNewest Then
            query = query.OrderByDescending(Function(t) t.CreatedAt)
        Else
            query = query.OrderBy(Function(t) t.CreatedAt)
        End If

        Dim totalCount = Await query.CountAsync()

        Dim items = Await query.Skip((pageIndex - 1) * pageSize) _
                               .Take(pageSize) _
                               .ToListAsync()

        Return (items, totalCount)
    End Function

    ' Danh sách đang được mượn hiện tại
    Public Async Function GetCurrentBorrowsAsync(userId As Integer) As Task(Of List(Of BorrowTicket)) _
        Implements IBorrowTicketRepository.GetCurrentBorrowsAsync

        Return Await _dbSet.Include("BorrowDetails.Book") _
                           .Where(Function(t) t.UserId = userId And t.Status = BorrowStatus.Approved) _
                           .ToListAsync()
    End Function

End Class