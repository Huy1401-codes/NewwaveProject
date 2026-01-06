Imports System.Data.Entity
Imports MyLibrary.Domain

Public Class PublisherRepository
    Inherits GenericRepository(Of Publisher)
    Implements IPublisherRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Async Function GetByNameAsync(name As String) As Task(Of Publisher) _
        Implements IPublisherRepository.GetByNameAsync

        Return Await _dbSet.FirstOrDefaultAsync(Function(p) _
            p.PublisherName = name AndAlso p.IsDeleted = False)
    End Function

    Public Async Function HasBorrowedBooksAsync(publisherId As Integer) As Task(Of Boolean) _
    Implements IPublisherRepository.HasBorrowedBooksAsync

        Return Await _context.Books.AnyAsync(Function(b) _
            b.PublisherId = publisherId AndAlso
            b.IsDeleted = False AndAlso
            b.Quantity <> b.AvailableQuantity)
    End Function

    Public Async Function ExistsByNameAsync(name As String, Optional excludeId As Integer? = Nothing) As Task(Of Boolean) _
        Implements IPublisherRepository.ExistsByNameAsync
        Dim normalized = name.Trim().ToLower()

        Dim query = _context.Publishers.Where(Function(c) _
        Not c.IsDeleted AndAlso c.PublisherName.ToLower() = normalized)

        If excludeId.HasValue Then
            query = query.Where(Function(c) c.Id <> excludeId.Value)
        End If

        Return Await query.AnyAsync()
    End Function
End Class
