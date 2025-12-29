Imports MyLibrary.Domain

Public Class BookRepository
    Inherits GenericRepository(Of Book)
    Implements IBookRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Function GetBooksFullInfo() As List(Of Book) Implements IBookRepository.GetBooksFullInfo
        Return _dbSet.Include("Author") _
                     .Include("Category") _
                     .Include("Publisher") _
                     .Where(Function(b) Not b.IsDeleted) _
                     .ToList()
    End Function
End Class