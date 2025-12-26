Imports MyLibrary.Domain.MyApp.Domain.Common

Partial Public Class Author
    Inherits BaseEntity

    Public Property AuthorId As Integer
    Public Property AuthorName As String

    ' Navigation
    Public Overridable Property Books As ICollection(Of Book)
End Class
