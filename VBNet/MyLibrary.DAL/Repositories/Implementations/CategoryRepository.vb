Imports MyLibrary.Domain

Public Class CategoryRepository
    Inherits GenericRepository(Of Category)
    Implements ICategoryRepository
    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub
    Public Function GetByName(name As String) As Category _
        Implements ICategoryRepository.GetByName

        Return _dbSet.FirstOrDefault(Function(c) _
            c.CategoryName = name AndAlso c.IsDeleted = False)
    End Function

    Public Function HasBorrowedBooks(categoryId As Integer) As Boolean _
     Implements ICategoryRepository.HasBorrowedBooks

        Return _context.Books.Any(Function(b) _
            b.CategoryId = categoryId AndAlso
            b.IsDeleted = False AndAlso
            b.Quantity <> b.AvailableQuantity)
    End Function

    Public Function ExistsByName(name As String, Optional excludeId As Integer? = Nothing) As Boolean _
        Implements ICategoryRepository.ExistsByName
        Dim normalized = name.Trim().ToLower()

        Dim query = _context.Categories.Where(Function(c) _
        Not c.IsDeleted AndAlso c.CategoryName.ToLower() = normalized)

        If excludeId.HasValue Then
            query = query.Where(Function(c) c.Id <> excludeId.Value)
        End If

        Return query.Any()
    End Function

End Class
