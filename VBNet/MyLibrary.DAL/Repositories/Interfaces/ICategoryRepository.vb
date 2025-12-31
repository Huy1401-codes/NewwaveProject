Imports MyLibrary.Domain

Public Interface ICategoryRepository
    Inherits IGenericRepository(Of Category)

    Function GetByName(name As String) As Category

    Function HasBorrowedBooks(categoryId As Integer) As Boolean

    Function ExistsByName(name As String, Optional excludeId As Integer? = Nothing) As Boolean
End Interface
