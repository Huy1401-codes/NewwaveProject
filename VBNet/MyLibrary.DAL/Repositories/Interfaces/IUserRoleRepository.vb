Imports MyLibrary.Domain

Public Interface IUserRoleRepository
    Inherits IGenericRepository(Of UserRole)

    Function GetRoleNameByUserId(userId As Integer) As String
End Interface
