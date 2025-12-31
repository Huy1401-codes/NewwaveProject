Imports MyLibrary.Domain

Public Interface IPublisherRepository
    Inherits IGenericRepository(Of Publisher)

    Function GetByName(name As String) As Publisher
    Function HasBorrowedBooks(publisherId As Integer) As Boolean

    Function ExistsByName(name As String, Optional excludeId As Integer? = Nothing) As Boolean
End Interface
