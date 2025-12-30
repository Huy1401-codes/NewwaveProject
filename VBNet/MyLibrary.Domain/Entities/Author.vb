Imports MyLibrary.Domain.MyApp.Domain.Common

Partial Public Class Author
    Inherits BaseEntity

    Public Property AuthorName As String
    Public Property Biography As String

    Public Property BirthDate As DateTime?

    Public Property Nationality As String
    Public Property Avatar As String

    ' Navigation
    Public Overridable Property Books As ICollection(Of Book)
End Class
