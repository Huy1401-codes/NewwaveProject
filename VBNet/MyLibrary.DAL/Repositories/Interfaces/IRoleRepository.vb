
Imports MyLibrary.Domain

Public Interface IRoleRepository
    Inherits IGenericRepository(Of Role)

    Function GetByNameAsync(roleName As String) As Task(Of Role)
End Interface
