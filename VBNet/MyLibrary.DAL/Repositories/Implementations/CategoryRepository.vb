Imports System.Data.Entity
Imports MyLibrary.Domain

Public Class CategoryRepository
    Inherits GenericRepository(Of Category)
    Implements ICategoryRepository
    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Async Function GetByNameAsync(name As String) As Task(Of Category) _
        Implements ICategoryRepository.GetByNameAsync

        Return Await _dbSet.FirstOrDefaultAsync(Function(c) _
            c.CategoryName = name AndAlso c.IsDeleted = False)
    End Function

    Public Async Function HasBorrowedBooksAsync(categoryId As Integer) As Task(Of Boolean) _
     Implements ICategoryRepository.HasBorrowedBooksAsync

        Return Await _context.Books.AnyAsync(Function(b) _
            b.CategoryId = categoryId AndAlso
            b.IsDeleted = False AndAlso
            b.Quantity <> b.AvailableQuantity)
    End Function

    Public Async Function ExistsByNameAsync(name As String, Optional excludeId As Integer? = Nothing) As Task(Of Boolean) _
        Implements ICategoryRepository.ExistsByNameAsync
        Dim normalized = name.Trim().ToLower() 

        Dim query = _context.Categories.Where(Function(c) _
        Not c.IsDeleted AndAlso c.CategoryName.ToLower() = normalized)

        If excludeId.HasValue Then
            query = query.Where(Function(c) c.Id <> excludeId.Value)
        End If

        Return Await query.AnyAsync()
    End Function

End Class
