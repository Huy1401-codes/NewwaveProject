Imports MyLibrary.Domain.MyApp.Domain.Common

Partial Public Class Publisher
    Inherits BaseEntity

    Public Property PublisherId As Integer
    Public Property PublisherName As String

    ' Navigation
    Public Overridable Property Books As ICollection(Of Book)
End Class
