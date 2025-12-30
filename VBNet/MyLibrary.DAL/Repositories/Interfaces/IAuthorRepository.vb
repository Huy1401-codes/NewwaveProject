Imports MyLibrary.Domain

Public Interface IAuthorRepository
    Inherits IGenericRepository(Of Author)

    Function GetByName(name As String) As Author
    Function HasBorrowedBooks(authorId As Integer) As Boolean
End Interface