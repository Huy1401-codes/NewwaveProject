Imports MyLibrary.Domain

Public Interface ICategoryRepository
    Inherits IGenericRepository(Of Category)

    Function GetByNameAsync(name As String) As Task(Of Category)

    Function HasBorrowedBooksAsync(categoryId As Integer) As Task(Of Boolean)

    Function ExistsByNameAsync(name As String, Optional excludeId As Integer? = Nothing) As Task(Of Boolean)
End Interface
