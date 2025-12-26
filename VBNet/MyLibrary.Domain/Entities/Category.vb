Imports MyLibrary.Domain.MyApp.Domain.Common

Partial Public Class Category
    Inherits BaseEntity

    Public Property CategoryId As Integer
    Public Property CategoryName As String

    ' Navigation
    Public Overridable Property Books As ICollection(Of Book)
End Class
