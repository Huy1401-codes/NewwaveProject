Imports MyLibrary.Domain.MyApp.Domain.Common

Partial Public Class Payment
    Inherits BaseEntity

    Public Property PaymentId As Integer
    Public Property DepositId As Integer
    Public Property PaymentMethod As String
    Public Property TransactionCode As String
    Public Property PaymentDate As DateTime

    ' Navigation
    Public Overridable Property Deposit As Deposit
End Class
