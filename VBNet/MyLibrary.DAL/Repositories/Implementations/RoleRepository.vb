Imports System.Data.Entity
Imports MyLibrary.Domain

Public Class RoleRepository
    Inherits GenericRepository(Of Role)
    Implements IRoleRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Async Function GetByNameAsync(roleName As String) As Task(Of Role) _
                 Implements IRoleRepository.GetByNameAsync

        If String.IsNullOrWhiteSpace(roleName) Then Return Nothing

        Return Await _dbSet.FirstOrDefaultAsync(Function(r) _
            Not r.IsDeleted AndAlso r.RoleName = roleName)
    End Function

End Class
