Imports System.Data.Entity
Imports MyLibrary.DAL
Imports MyLibrary.Domain.MyApp.Domain.Enums

Public Class UserService
    Implements IUserService

    Private ReadOnly _uow As UnitOfWork

    Public Sub New(uow As UnitOfWork)
        _uow = uow
    End Sub

    Public Async Function GetPagedAsync(keyword As String, status As Boolean?, pageIndex As Integer,
                             pageSize As Integer) As Task(Of PagedResult(Of UserDto)) Implements IUserService.GetPagedAsync


        Dim query = _uow.Users.GetAllIncludedDeleted().Include("UserRoles.Role")

        query = query.Where(Function(u) Not u.UserRoles.Any(Function(ur) ur.Role.RoleName = "Admin"))

        If Not String.IsNullOrWhiteSpace(keyword) Then
            Dim kw = keyword.Trim().ToLower()
            query = query.Where(Function(u) u.FullName.ToLower().Contains(kw) OrElse
                                            u.Email.ToLower().Contains(kw) OrElse
                                            u.Phone.Contains(kw))
        End If


        If status.HasValue Then
            query = query.Where(Function(u) u.IsActive = status.Value)
        End If

        Dim totalRow = Await query.CountAsync()

        Dim items = query.OrderByDescending(Function(u) u.Id) _
                         .Skip((pageIndex - 1) * pageSize) _
                         .Take(pageSize) _
                         .ToList() _
                         .Select(Function(u) New UserDto With {
                             .Id = u.Id,
                             .FullName = u.FullName,
                             .Email = u.Email,
                             .Phone = u.Phone,
                             .IsActive = u.IsActive,
                             .RoleNames = String.Join(", ", u.UserRoles.Select(Function(ur) ur.Role.RoleName))
                         }).ToList()

        Return New PagedResult(Of UserDto) With {
            .Items = items,
            .TotalCount = totalRow,
            .TotalPages = Math.Ceiling(totalRow / pageSize)
        }
    End Function

    Public Async Function ToggleStatusAsync(userId As Integer) As Task Implements IUserService.ToggleStatusAsync
        Dim user = Await _uow.Users.GetByIdAsync(userId)
        If user Is Nothing Then Throw New Exception("Người dùng không tồn tại")

        user.IsActive = Not user.IsActive

        _uow.Users.Update(user)
        Await _uow.SaveAsync()
    End Function
End Class