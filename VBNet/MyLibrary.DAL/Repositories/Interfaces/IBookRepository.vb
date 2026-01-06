Imports MyLibrary.Domain

Public Interface IBookRepository
    Inherits IGenericRepository(Of Book)
    Function GetBooksFullInfoAsync() As Task(Of List(Of Book))
    Function ExistsByCodeAsync(bookCode As String) As Task(Of Boolean)
End Interface