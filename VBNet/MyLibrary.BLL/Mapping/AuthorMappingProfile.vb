Imports AutoMapper
Imports MyLibrary.Domain

Public Class AuthorMappingProfile
    Inherits Profile

    Public Sub New()
        CreateMap(Of Author, AuthorDto)().ReverseMap()
    End Sub
End Class
