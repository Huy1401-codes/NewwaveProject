Public Class CategoryDetailDto
    Public Property CategoryId As Integer
    Public Property CategoryName As String
    Public Property Books As PagedResult(Of CategoryBookDto)
End Class
