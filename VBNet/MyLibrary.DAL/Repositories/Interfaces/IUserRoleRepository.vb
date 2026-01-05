Imports MyLibrary.Domain

Public Interface IUserRoleRepository
    Inherits IGenericRepository(Of UserRole)

    Function GetRoleNameByUserIdAsync(userId As Integer) As Task(Of String)
End Interface
