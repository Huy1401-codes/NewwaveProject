Imports MyLibrary.Domain
Imports System.Data.Entity

Public Class UserRepository
    Inherits GenericRepository(Of User)
    Implements IUserRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Async Function GetByEmailAsync(email As String) As Task(Of User) _
        Implements IUserRepository.GetByEmailAsync

        Return Await _dbSet.
            Include("UserRoles.Role").
            FirstOrDefaultAsync(Function(u) u.Email = email AndAlso u.IsDeleted = False)
    End Function

    Public Async Function ExistsByEmailAsync(email As String) As Task(Of Boolean) _
        Implements IUserRepository.ExistsByEmailAsync

        Return Await _dbSet.AnyAsync(Function(u) u.Email = email AndAlso u.IsDeleted = False)
    End Function

    Public Function GetAllIncludedDeleted() As IQueryable(Of User) Implements IUserRepository.GetAllIncludedDeleted
        Return _context.Users
    End Function

End Class
