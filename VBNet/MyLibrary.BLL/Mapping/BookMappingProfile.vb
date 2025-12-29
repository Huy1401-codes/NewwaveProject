Imports AutoMapper
Imports MyLibrary.Domain

Public Class BookMappingProfile
    Inherits Profile

    Public Sub New()
        CreateMap(Of Book, BookDto)() _
            .ForMember(Function(dest) dest.AuthorName, Sub(opt) opt.MapFrom(Function(src) If(src.Author IsNot Nothing, src.Author.AuthorName, "N/A"))) _
            .ForMember(Function(dest) dest.CategoryName, Sub(opt) opt.MapFrom(Function(src) If(src.Category IsNot Nothing, src.Category.CategoryName, "N/A"))) _
            .ForMember(Function(dest) dest.PublisherName, Sub(opt) opt.MapFrom(Function(src) If(src.Publisher IsNot Nothing, src.Publisher.PublisherName, "N/A")))

        CreateMap(Of BookDto, Book)() _
            .ForMember(Function(dest) dest.Id, Sub(opt) opt.MapFrom(Function(src) src.Id)) _
            .ForMember(Function(dest) dest.AuthorId, Sub(opt) opt.MapFrom(Function(src) src.AuthorId)) _
            .ForMember(Function(dest) dest.CategoryId, Sub(opt) opt.MapFrom(Function(src) src.CategoryId)) _
            .ForMember(Function(dest) dest.PublisherId, Sub(opt) opt.MapFrom(Function(src) src.PublisherId)) _
            .ForMember(Function(dest) dest.ImagePath, Sub(opt) opt.MapFrom(Function(src) src.ImagePath)) _
            .ForMember(Function(dest) dest.CreatedAt, Sub(opt) opt.Ignore())
    End Sub
End Class