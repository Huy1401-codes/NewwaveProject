Imports MyLibrary.Domain.MyApp.Domain.Common

Partial Public Class Book
    Inherits BaseEntity
    Public Property BookCode As String
    Public Property Title As String
    Public Property AuthorId As Nullable(Of Integer)
    Public Property CategoryId As Nullable(Of Integer)
    Public Property PublisherId As Nullable(Of Integer)
    Public Property PublishYear As Nullable(Of Integer)
    Public Property Price As Nullable(Of Decimal)
    Public Property Quantity As Integer
    Public Property AvailableQuantity As Integer
    Public Property ImagePath As String
    Public Property Description As String

    ' Navigation
    Public Overridable Property Author As Author
    Public Overridable Property Category As Category
    Public Overridable Property Publisher As Publisher
    Public Overridable Property BorrowDetails As ICollection(Of BorrowDetail)
End Class
