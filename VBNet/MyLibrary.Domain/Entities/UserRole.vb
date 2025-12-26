Partial Public Class UserRole
    Public Property UserId As Integer
    Public Property RoleId As Integer

    ' Navigation
    Public Overridable Property User As User
    Public Overridable Property Role As Role
End Class
