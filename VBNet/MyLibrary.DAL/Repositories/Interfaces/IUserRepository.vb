Imports MyLibrary.Domain

Public Interface IUserRepository
    Inherits IGenericRepository(Of User)

    Function GetByEmailAsync(email As String) As Task(Of User)
    Function ExistsByEmailAsync(email As String) As Task(Of Boolean)

    Function GetAllIncludedDeleted() As IQueryable(Of User)
End Interface
