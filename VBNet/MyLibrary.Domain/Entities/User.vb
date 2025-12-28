Imports MyLibrary.Domain.MyApp.Domain.Common

Partial Public Class User
    Inherits BaseEntity

    Public Property UserId As Integer
    Public Property FullName As String
    Public Property Email As String
    Public Property PasswordHash As String
    Public Property Phone As String
    Public Property IsActive As Boolean

    ' Lưu mã OTP (6 số). 
    Public Property VerificationCode As String

    ' Lưu thời hạn của mã
    Public Property CodeExpiration As DateTime?


    ' Navigation
    Public Overridable Property BorrowTickets As ICollection(Of BorrowTicket)
    Public Overridable Property UserRoles As ICollection(Of UserRole)
End Class
