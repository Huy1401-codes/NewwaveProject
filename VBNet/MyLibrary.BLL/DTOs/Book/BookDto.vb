Public Class BookDto
    Public Property BookId As Integer
    Public Property BookCode As String
    Public Property Title As String
    Public Property AuthorId As Integer?
    Public Property CategoryId As Integer?
    Public Property PublisherId As Integer?
    Public Property PublishYear As Integer?
    Public Property Price As Decimal?
    Public Property Quantity As Integer
    Public Property AvailableQuantity As Integer
    Public Property ImagePath As String
    Public Property Description As String
    Public Property CreatedAt As DateTime
    Public Property UpdatedAt As DateTime?
    Public Property IsDeleted As Boolean
End Class
