Imports MyLibrary.Domain
Imports System.Data.Entity

Public Class UserRepository
    Inherits GenericRepository(Of User)
    Implements IUserRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Function GetByEmail(email As String) As User _
        Implements IUserRepository.GetByEmail

        Return _dbSet.
            Include("UserRoles.Role").
            FirstOrDefault(Function(u) u.Email = email AndAlso u.IsDeleted = False)
    End Function

    Public Function ExistsByEmail(email As String) As Boolean _
        Implements IUserRepository.ExistsByEmail

        Return _dbSet.Any(Function(u) u.Email = email AndAlso u.IsDeleted = False)
    End Function

End Class
