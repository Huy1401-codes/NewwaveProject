Imports MyLibrary.Domain.MyApp.Domain.Common

Partial Public Class BorrowTicket
    Inherits BaseEntity

    Public Property BorrowTicketId As Integer
    Public Property UserId As Integer
    Public Property BorrowDate As DateTime
    Public Property DueDate As DateTime
    Public Property Status As String

    ' Navigation
    Public Overridable Property User As User
    Public Overridable Property BorrowDetails As ICollection(Of BorrowDetail)
    Public Overridable Property Deposits As ICollection(Of Deposit)
End Class
