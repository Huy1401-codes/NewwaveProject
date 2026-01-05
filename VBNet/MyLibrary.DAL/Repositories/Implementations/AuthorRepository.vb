Imports System.Data.Entity
Imports MyLibrary.Domain

Public Class AuthorRepository
    Inherits GenericRepository(Of Author)
    Implements IAuthorRepository
    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub
    Public Async Function GetByNameAsync(name As String) As Task(Of Author) _
        Implements IAuthorRepository.GetByNameAsync

        Return Await _dbSet.FirstOrDefaultAsync(Function(a) _
            a.AuthorName = name AndAlso a.IsDeleted = False)
    End Function

    Public Async Function HasBorrowedBooksAsync(authorId As Integer) As Task(Of Boolean) _
        Implements IAuthorRepository.HasBorrowedBooksAsync

        Return Await _context.Books.AnyAsync(Function(b) _
            b.AuthorId = authorId AndAlso
            b.IsDeleted = False AndAlso
            b.Quantity <> b.AvailableQuantity)
    End Function

End Class
