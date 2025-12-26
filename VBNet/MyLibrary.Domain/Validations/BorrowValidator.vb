Public Class BorrowValidator
    Public Shared Sub Validate(ticket As BorrowTicket)
        If ticket.DueDate <= ticket.BorrowDate Then
            Throw New Exception("DueDate phải lớn hơn BorrowDate")
        End If
    End Sub
End Class
