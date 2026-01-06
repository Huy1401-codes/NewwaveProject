Imports System.Data.Entity
Imports MyLibrary.Domain
Public Class UserRoleRepository
    Inherits GenericRepository(Of UserRole)
    Implements IUserRoleRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Async Function GetRoleNameByUserIdAsync(userId As Integer) As Task(Of String) _
        Implements IUserRoleRepository.GetRoleNameByUserIdAsync

        Return Await _dbSet _
            .Include("Role") _
            .Where(Function(ur) ur.UserId = userId) _
            .Select(Function(ur) ur.Role.RoleName) _
            .FirstOrDefaultAsync()
    End Function

End Class
