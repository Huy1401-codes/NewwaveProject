Imports MyLibrary.Domain

Public Class RoleRepository
    Inherits GenericRepository(Of Role)
    Implements IRoleRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Function GetByName(roleName As String) As Role _
    Implements IRoleRepository.GetByName

        Dim allData = _dbSet.ToList()
        Dim count = allData.Count

        If String.IsNullOrWhiteSpace(roleName) Then Return Nothing
        Dim nameToFind As String = roleName.Trim().ToLower()

        Return _dbSet.FirstOrDefault(Function(r) _
        r.IsDeleted = False AndAlso
        r.RoleName.Trim().ToLower() = nameToFind)
    End Function

End Class
