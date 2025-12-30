Imports MyLibrary.Domain

Public Interface IPublisherRepository
    Inherits IGenericRepository(Of Publisher)

    Function GetByName(name As String) As Publisher
End Interface
