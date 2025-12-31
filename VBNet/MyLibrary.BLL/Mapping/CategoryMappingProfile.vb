Imports AutoMapper
Imports MyLibrary.Domain

Public Class CategoryMappingProfile
    Inherits Profile

    Public Sub New()
        CreateMap(Of Category, CategoryDto)().ReverseMap()
    End Sub
End Class
