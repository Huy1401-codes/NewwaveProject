
Imports MyLibrary.Domain

Public Interface IRoleRepository
    Inherits IGenericRepository(Of Role)

    Function GetByName(roleName As String) As Role
End Interface
