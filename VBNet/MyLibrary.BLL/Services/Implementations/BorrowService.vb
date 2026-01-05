Imports MyLibrary.DAL
Imports MyLibrary.Domain
Imports MyLibrary.Domain.MyApp.Domain.Enums
Imports NLog

Public Class BorrowService
    Implements IBorrowService

    Private ReadOnly _uow As IUnitOfWork
    Private Shared ReadOnly logger As Logger = LogManager.GetCurrentClassLogger()

    Public Sub New(uow As IUnitOfWork)
        _uow = uow
    End Sub

    Public Sub RequestBorrow(userId As Integer,
                          borrowItems As List(Of BorrowItemDto),
                          dueDate As DateTime) _
    Implements IBorrowService.RequestBorrow

        logger.Info("RequestBorrow START | UserId={0}, Books={1}, DueDate={2}",
                userId,
                String.Join(",",
                    borrowItems.Select(Function(x) $"{x.BookId}({x.Quantity})")),
                dueDate)

        If borrowItems Is Nothing OrElse borrowItems.Count = 0 Then
            Throw New Exception("Chưa chọn sách nào.")
        End If

        If dueDate <= DateTime.Now Then
            Throw New Exception("Ngày trả không hợp lệ.")
        End If

        If dueDate > DateTime.Now.AddYears(1) Then
            Throw New Exception("Chỉ được mượn tối đa 1 năm.")
        End If

        Using transaction = _uow.Context.Database.BeginTransaction()
            Try
                Dim ticket As New BorrowTicket With {
                .UserId = userId,
                .BorrowDate = DateTime.Now,
                .DueDate = dueDate,
                .Status = BorrowStatus.Pending,
                .CreatedAt = DateTime.Now,
                .IsDeleted = False
            }

                _uow.BorrowTickets.Add(ticket)
                _uow.Save()
                For Each item In borrowItems
                    Dim book = _uow.Books.GetById(item.BookId)

                    If book Is Nothing Then
                        Throw New Exception($"Sách ID {item.BookId} không tồn tại.")
                    End If

                    If item.Quantity <= 0 Then
                        Throw New Exception($"Số lượng mượn của sách '{book.Title}' không hợp lệ.")
                    End If

                    If book.AvailableQuantity < item.Quantity Then
                        Throw New Exception($"Sách '{book.Title}' không đủ số lượng.")
                    End If

                    Dim detail As New BorrowDetail With {
                    .BorrowTicketId = ticket.Id,
                    .BookId = item.BookId,
                    .Quantity = item.Quantity,
                    .CreatedAt = DateTime.Now,
                    .IsDeleted = False
                }

                    _uow.BorrowDetails.Add(detail)

                    book.AvailableQuantity -= item.Quantity
                    _uow.Books.Update(book)
                Next

                _uow.Save()
                transaction.Commit()

                logger.Info("RequestBorrow SUCCESS | UserId={0}, TicketId={1}",
                        userId, ticket.Id)

            Catch ex As Exception
                transaction.Rollback()
                logger.Error(ex, "RequestBorrow ERROR | UserId={0}", userId)
                Throw
            End Try
        End Using
    End Sub


    Public Sub ApproveBorrow(ticketId As Integer, isApproved As Boolean) _
        Implements IBorrowService.ApproveBorrow
        logger.Info("ApproveBorrow START | TicketId={0}, Approved={1}", ticketId, isApproved)

        Dim ticket = _uow.BorrowTickets.GetByIdWithDetails(ticketId)
        If ticket Is Nothing Then Throw New Exception("Phiếu mượn không tồn tại.")

        If ticket.Status <> BorrowStatus.Pending Then
            Throw New Exception("Chỉ được xử lý phiếu đang chờ duyệt (Pending).")
        End If

        Using trans = _uow.Context.Database.BeginTransaction()
            Try
                If isApproved Then

                    ticket.Status = BorrowStatus.Approved
                Else
                    ticket.Status = BorrowStatus.Rejected

                    If ticket.BorrowDetails IsNot Nothing Then
                        For Each d In ticket.BorrowDetails
                            Dim book = _uow.Books.GetById(d.BookId)
                            If book IsNot Nothing Then
                                book.AvailableQuantity += d.Quantity
                                _uow.Books.Update(book)
                            End If
                        Next
                    End If
                End If

                ticket.UpdatedAt = DateTime.Now
                _uow.BorrowTickets.Update(ticket)
                _uow.Save()
                trans.Commit()

                logger.Info("ApproveBorrow SUCCESS | TicketId={0}, Status={1}",
                            ticketId, ticket.Status)
            Catch ex As Exception
                logger.Error(ex, "ApproveBorrow ERROR | TicketId={0}", ticketId)
                trans.Rollback()
                Throw ex
            End Try
        End Using
    End Sub


    Public Sub ReturnBook(ticketId As Integer, paymentMethod As PaymentMethod) _
        Implements IBorrowService.ReturnBook

        Dim ticket = _uow.BorrowTickets.GetByIdWithDetails(ticketId)
        If ticket Is Nothing Then Throw New Exception("Phiếu không tồn tại.")

        If ticket.Status <> BorrowStatus.Approved Then Throw New Exception("Phiếu này không trong trạng thái đang mượn.")

        Using trans = _uow.Context.Database.BeginTransaction()
            Try
                Dim fineAmount As Decimal = 0
                If DateTime.Now > ticket.DueDate Then
                    Dim overdueDays = (DateTime.Now - ticket.DueDate).Days
                    fineAmount = overdueDays * 5000
                End If

                If fineAmount > 0 Then
                    Dim deposit As New Deposit With {
                        .BorrowTicketId = ticketId,
                        .DepositAmount = fineAmount,
                        .IsRefunded = False,
                        .CreatedAt = DateTime.Now
                    }
                    _uow.Deposits.Add(deposit)
                    _uow.Save()

                    Dim payment As New Payment With {
                        .DepositId = deposit.Id,
                        .PaymentMethod = paymentMethod.ToString(),
                        .PaymentDate = DateTime.Now,
                        .TransactionCode = "PAY-" & Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                        .CreatedAt = DateTime.Now
                    }
                    _uow.Payments.Add(payment)
                End If

                ' Cập nhật trạng thái
                ticket.Status = BorrowStatus.Returned
                ticket.UpdatedAt = DateTime.Now
                _uow.BorrowTickets.Update(ticket)

                ' Trả sách  
                If ticket.BorrowDetails IsNot Nothing Then
                    For Each d In ticket.BorrowDetails
                        Dim book = _uow.Books.GetById(d.BookId)
                        If book IsNot Nothing Then
                            book.AvailableQuantity += d.Quantity
                            _uow.Books.Update(book)
                        End If
                    Next
                End If

                _uow.Save()
                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                Throw ex
            End Try
        End Using
    End Sub

    Public Function GetPendingListPaged(sortOrder As String, page As Integer, pageSize As Integer) As PagedResult(Of BorrowTicketDto) _
    Implements IBorrowService.GetPendingListPaged

        Dim isNewest = (sortOrder = "Mới nhất")

        Dim result = _uow.BorrowTickets.GetPendingRequestsPaged(isNewest, page, pageSize)

        Dim dtos = result.Item1.Select(Function(t) MapToDto(t)).ToList()
        Dim totalRecords = result.Item2

        Dim totalPages As Integer = 0
        If totalRecords > 0 Then
            totalPages = CInt(Math.Ceiling(totalRecords / pageSize))
        End If

        Return New PagedResult(Of BorrowTicketDto) With {
        .Items = dtos,
        .TotalCount = totalRecords,
        .TotalPages = totalPages
    }
    End Function

    Public Function GetMyHistory(userId As Integer) As List(Of BorrowTicketDto) _
        Implements IBorrowService.GetMyHistory

        Dim tickets = _uow.BorrowTickets.GetHistoryByUserId(userId)
        logger.Info("GetMyHistory | UserId={0}", userId)
        Return tickets.Select(Function(t) MapToDto(t)).ToList()
    End Function

    Public Function CalculateFine(ticketId As Integer) As Decimal _
        Implements IBorrowService.CalculateFine

        Dim ticket = _uow.BorrowTickets.GetById(ticketId)
        If ticket Is Nothing Then Return 0

        If ticket.Status = BorrowStatus.Approved AndAlso DateTime.Now > ticket.DueDate Then
            Dim overdueDays = (DateTime.Now - ticket.DueDate).Days
            Return overdueDays * 5000
        End If
        Return 0
    End Function


    Private Function MapToDto(t As BorrowTicket) As BorrowTicketDto
        Dim dto As New BorrowTicketDto With {
            .Id = t.Id,
            .UserName = If(t.User IsNot Nothing, t.User.FullName, "Unknown"),
            .BorrowDate = t.BorrowDate,
            .DueDate = t.DueDate,
            .Status = t.Status,
            .FineAmount = 0
        }

        If t.BorrowDetails IsNot Nothing AndAlso t.BorrowDetails.Any() Then
            dto.BookList = String.Join(", ", t.BorrowDetails.Select(Function(d) d.Book.Title))
        Else
            dto.BookList = "N/A"
        End If

        Select Case t.Status
            Case BorrowStatus.Pending : dto.StatusDisplay = "Chờ duyệt"
            Case BorrowStatus.Rejected : dto.StatusDisplay = "Đã từ chối"
            Case BorrowStatus.Returned : dto.StatusDisplay = "Đã trả xong"
            Case BorrowStatus.Approved
                If DateTime.Now > t.DueDate Then
                    Dim days = (DateTime.Now - t.DueDate).Days
                    dto.FineAmount = days * 5000
                    dto.StatusDisplay = $"Quá hạn {days} ngày (Phạt {dto.FineAmount:N0}đ)"
                Else
                    dto.StatusDisplay = "Đang mượn"
                End If
            Case Else : dto.StatusDisplay = t.Status
        End Select
        Return dto
    End Function

End Class