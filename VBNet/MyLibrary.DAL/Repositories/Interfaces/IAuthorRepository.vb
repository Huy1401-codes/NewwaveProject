Imports MyLibrary.Domain

Public Interface IAuthorRepository
    Inherits IGenericRepository(Of Author)

    Function GetByNameAsync(name As String) As Task(Of Author)
    Function HasBorrowedBooksAsync(authorId As Integer) As Task(Of Boolean)
End Interface