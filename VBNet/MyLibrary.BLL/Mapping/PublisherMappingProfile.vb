Imports AutoMapper
Imports MyLibrary.Domain

Public Class PublisherMappingProfile
    Inherits Profile
    Public Sub New()
        CreateMap(Of Publisher, PublisherDto)().ReverseMap()
    End Sub
End Class
