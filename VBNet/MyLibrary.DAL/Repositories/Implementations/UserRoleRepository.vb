Imports MyLibrary.Domain

Public Class UserRoleRepository
    Inherits GenericRepository(Of UserRole)
    Implements IUserRoleRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Function GetRoleNameByUserId(userId As Integer) As String _
        Implements IUserRoleRepository.GetRoleNameByUserId

        Return _dbSet _
            .Include("Role") _
            .Where(Function(ur) ur.UserId = userId) _
            .Select(Function(ur) ur.Role.RoleName) _
            .FirstOrDefault()
    End Function

End Class
