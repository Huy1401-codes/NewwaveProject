Imports System.Data.Entity
Imports MyLibrary.Domain

Public Class BookRepository
    Inherits GenericRepository(Of Book)
    Implements IBookRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Async Function GetBooksFullInfoAsync() As Task(Of List(Of Book)) Implements IBookRepository.GetBooksFullInfoAsync
        Return Await _dbSet.Include("Author") _
                     .Include("Category") _
                     .Include("Publisher") _
                     .Where(Function(b) Not b.IsDeleted) _
                     .ToListAsync()
    End Function

    Public Async Function ExistsByCodeAsync(bookCode As String) As Task(Of Boolean) _
       Implements IBookRepository.ExistsByCodeAsync

        Return Await _dbSet.AnyAsync(Function(b) _
            b.BookCode = bookCode AndAlso b.IsDeleted = False)
    End Function
End Class