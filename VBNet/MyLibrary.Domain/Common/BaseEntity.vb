Namespace MyApp.Domain.Common
    Public MustInherit Class BaseEntity
        Public Property Id As Integer
        Public Property CreatedAt As DateTime
        Public Property UpdatedAt As Nullable(Of DateTime)
        Public Property IsDeleted As Boolean
    End Class
End Namespace
