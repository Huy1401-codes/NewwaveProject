Imports MyLibrary.Domain

Public Class PublisherRepository
    Inherits GenericRepository(Of Publisher)
    Implements IPublisherRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Function GetByName(name As String) As Publisher _
        Implements IPublisherRepository.GetByName

        Return _dbSet.FirstOrDefault(Function(p) _
            p.PublisherName = name AndAlso p.IsDeleted = False)
    End Function

    Public Function HasBorrowedBooks(publisherId As Integer) As Boolean _
    Implements IPublisherRepository.HasBorrowedBooks

        Return _context.Books.Any(Function(b) _
            b.PublisherId = publisherId AndAlso
            b.IsDeleted = False AndAlso
            b.Quantity <> b.AvailableQuantity)
    End Function

    Public Function ExistsByName(name As String, Optional excludeId As Integer? = Nothing) As Boolean _
        Implements IPublisherRepository.ExistsByName
        Dim normalized = name.Trim().ToLower()

        Dim query = _context.Publishers.Where(Function(c) _
        Not c.IsDeleted AndAlso c.PublisherName.ToLower() = normalized)

        If excludeId.HasValue Then
            query = query.Where(Function(c) c.Id <> excludeId.Value)
        End If

        Return query.Any()
    End Function
End Class
