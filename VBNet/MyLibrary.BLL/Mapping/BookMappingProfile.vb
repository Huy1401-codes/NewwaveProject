Imports AutoMapper
Imports MyLibrary.Domain

Public Class BookMappingProfile
    Inherits Profile

    Public Sub New()
        CreateMap(Of Book, BookDto)() _
            .ForMember(Function(dest) dest.AuthorName, Sub(opt) opt.MapFrom(Function(src) If(src.Author IsNot Nothing, src.Author.AuthorName, "N/A"))) _
            .ForMember(Function(dest) dest.CategoryName, Sub(opt) opt.MapFrom(Function(src) If(src.Category IsNot Nothing, src.Category.CategoryName, "N/A"))) _
            .ForMember(Function(dest) dest.PublisherName, Sub(opt) opt.MapFrom(Function(src) If(src.Publisher IsNot Nothing, src.Publisher.PublisherName, "N/A")))

    End Sub
End Class