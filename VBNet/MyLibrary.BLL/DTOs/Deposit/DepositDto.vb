Public Class DepositDto
    Public Property DepositId As Integer
    Public Property BorrowTicketId As Integer
    Public Property DepositAmount As Decimal
    Public Property IsRefunded As Boolean
    Public Property RefundDate As DateTime?
    Public Property CreatedAt As DateTime
    Public Property UpdatedAt As DateTime?
    Public Property IsDeleted As Boolean
End Class
