Imports MyLibrary.Domain

Public Interface IPublisherRepository
    Inherits IGenericRepository(Of Publisher)

    Function GetByNameAsync(name As String) As Task(Of Publisher)
    Function HasBorrowedBooksAsync(publisherId As Integer) As Task(Of Boolean)

    Function ExistsByNameAsync(name As String, Optional excludeId As Integer? = Nothing) As Task(Of Boolean)
End Interface
