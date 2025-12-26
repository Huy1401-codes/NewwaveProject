Imports MyLibrary.Domain.MyApp.Domain.Common

Partial Public Class BorrowDetail
    Inherits BaseEntity

    Public Property BorrowDetailId As Integer
    Public Property BorrowTicketId As Integer
    Public Property BookId As Integer
    Public Property Quantity As Integer

    ' Navigation
    Public Overridable Property BorrowTicket As BorrowTicket
    Public Overridable Property Book As Book
End Class
