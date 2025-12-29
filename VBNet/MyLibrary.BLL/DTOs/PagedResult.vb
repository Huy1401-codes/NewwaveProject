Public Class PagedResult(Of T)
    Public Property Items As List(Of T)
    Public Property TotalCount As Integer
    Public Property TotalPages As Integer
End Class