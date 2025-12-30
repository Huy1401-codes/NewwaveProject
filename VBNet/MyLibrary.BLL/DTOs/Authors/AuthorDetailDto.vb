Public Class AuthorDetailDto
    Public Property AuthorId As Integer
    Public Property AuthorName As String
    Public Property Avatar As String
    Public Property Books As PagedResult(Of AuthorBookDto)
End Class