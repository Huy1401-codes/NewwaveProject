Imports MyLibrary.Domain

Public Interface IBookRepository
    Inherits IGenericRepository(Of Book)
    Function GetBooksFullInfo() As List(Of Book)

    Function ExistsByCode(bookCode As String) As Boolean
End Interface