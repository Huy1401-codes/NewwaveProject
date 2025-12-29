Imports MyLibrary.Domain

Public Interface IBookRepository
    Inherits IGenericRepository(Of Book)
    Function GetBooksFullInfo() As List(Of Book)
End Interface