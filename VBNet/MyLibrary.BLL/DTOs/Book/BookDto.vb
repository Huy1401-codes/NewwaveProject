Public Class BookDto
    Public Property Id As Integer
    Public Property BookCode As String
    Public Property Title As String
    Public Property AuthorName As String
    Public Property CategoryName As String
    Public Property PublisherName As String
    Public Property PublishYear As Integer?
    Public Property Price As Decimal?
    Public Property Quantity As Integer
    Public Property AvailableQuantity As Integer

    Public Property AuthorId As Integer
    Public Property CategoryId As Integer
    Public Property PublisherId As Integer
    Public Property ImagePath As String
End Class