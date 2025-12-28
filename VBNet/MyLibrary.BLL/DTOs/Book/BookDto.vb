Public Class BookDto
    Public Property BookId As Integer
    Public Property BookCode As String
    Public Property Title As String
    Public Property AuthorName As String     ' Thay vì AuthorId
    Public Property CategoryName As String   ' Thay vì CategoryId
    Public Property PublisherName As String  ' Thay vì PublisherId
    Public Property PublishYear As Integer?
    Public Property Price As Decimal?
    Public Property Quantity As Integer
    Public Property AvailableQuantity As Integer
End Class