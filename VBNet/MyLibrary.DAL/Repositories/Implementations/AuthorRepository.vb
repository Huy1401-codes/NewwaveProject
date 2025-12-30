Imports MyLibrary.Domain

Public Class AuthorRepository
    Inherits GenericRepository(Of Author)
    Implements IAuthorRepository
    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub
    Public Function GetByName(name As String) As Author _
        Implements IAuthorRepository.GetByName

        Return _dbSet.FirstOrDefault(Function(a) _
            a.AuthorName = name AndAlso a.IsDeleted = False)
    End Function

    Public Function HasBorrowedBooks(authorId As Integer) As Boolean _
        Implements IAuthorRepository.HasBorrowedBooks

        Return _context.Books.Any(Function(b) _
            b.AuthorId = authorId AndAlso
            b.IsDeleted = False AndAlso
            b.Quantity <> b.AvailableQuantity)
    End Function


End Class
