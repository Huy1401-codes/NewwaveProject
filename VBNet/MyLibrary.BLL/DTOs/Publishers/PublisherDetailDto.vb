Public Class PublisherDetailDto
    Public Property PublisherId As Integer
    Public Property PublisherName As String
    Public Property Books As PagedResult(Of PublisherBookDto)
End Class
