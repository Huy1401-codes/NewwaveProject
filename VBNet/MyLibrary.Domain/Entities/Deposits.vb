Imports MyLibrary.Domain.MyApp.Domain.Common

Partial Public Class Deposit
    Inherits BaseEntity

    Public Property DepositId As Integer
    Public Property BorrowTicketId As Integer
    Public Property DepositAmount As Decimal
    Public Property IsRefunded As Boolean
    Public Property RefundDate As Nullable(Of DateTime)

    ' Navigation
    Public Overridable Property BorrowTicket As BorrowTicket
    Public Overridable Property Payments As ICollection(Of Payment)
End Class
