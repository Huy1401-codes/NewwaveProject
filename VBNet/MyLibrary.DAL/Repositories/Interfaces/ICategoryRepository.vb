Imports MyLibrary.Domain

Public Interface ICategoryRepository
    Inherits IGenericRepository(Of Category)

    Function GetByName(name As String) As Category
End Interface
