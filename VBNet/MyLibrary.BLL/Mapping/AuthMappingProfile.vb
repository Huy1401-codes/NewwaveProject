Imports AutoMapper
Imports MyLibrary.Domain

Public Class AuthMappingProfile
    Inherits Profile

    Public Sub New()

        CreateMap(Of User, LoginResponseDto)()

        CreateMap(Of User, RegisterResponseDto)()

    End Sub
End Class
