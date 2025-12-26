Public Class RoleRepository
    Inherits GenericRepository(Of Role)
    Implements IRoleRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Function GetByName(roleName As String) As Role _
        Implements IRoleRepository.GetByName

        Return _dbSet.FirstOrDefault(Function(r) r.RoleName = roleName)
    End Function

End Class
