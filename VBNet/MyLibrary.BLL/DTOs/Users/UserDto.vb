Public Class UserDto
    Public Property Id As Integer
    Public Property Phone As String
    Public Property FullName As String
    Public Property Email As String
    Public Property PhoneNumber As String
    Public Property RoleNames As String
    Public Property IsActive As Boolean

    Public ReadOnly Property StatusText As String
        Get
            Return If(IsActive, "Đang hoạt động", "Đã khóa")
        End Get
    End Property
End Class