Imports MyLibrary.Domain

Public Interface IUserRepository
    Inherits IGenericRepository(Of User)

    Function GetByEmail(email As String) As User
    Function ExistsByEmail(email As String) As Boolean
End Interface
