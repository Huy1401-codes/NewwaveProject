Imports MyLibrary.Domain.MyApp.Domain.Common

Partial Public Class Role
    Inherits BaseEntity

    Public Property RoleName As String

    ' Navigation
    Public Overridable Property UserRoles As ICollection(Of UserRole)
End Class
