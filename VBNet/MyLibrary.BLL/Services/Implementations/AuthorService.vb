Imports AutoMapper
Imports MyLibrary.DAL
Imports MyLibrary.Domain

Public Class AuthorService
    Implements IAuthorService

    Private ReadOnly _uow As IUnitOfWork
    Private ReadOnly _mapper As IMapper
    Public Sub New(uow As IUnitOfWork, mapper As IMapper)
        _uow = uow
        _mapper = mapper
    End Sub

    Public Function GetPaged(keyword As String,
                         pageIndex As Integer,
                         pageSize As Integer) As PagedResult(Of Author) _
    Implements IAuthorService.GetPaged

        Dim query = _uow.Authors.GetAll()

        If Not String.IsNullOrWhiteSpace(keyword) Then
            query = query.Where(Function(a) a.AuthorName.Contains(keyword))
        End If

        Dim totalCount = query.Count()
        Dim totalPages = Math.Ceiling(totalCount / pageSize)

        Dim items = query _
        .OrderBy(Function(a) a.AuthorName) _
        .Skip((pageIndex - 1) * pageSize) _
        .Take(pageSize) _
        .ToList()

        Return New PagedResult(Of Author) With {
        .Items = items,
        .TotalCount = totalCount,
        .TotalPages = totalPages
    }
    End Function


    Public Sub Add(dto As AuthorDto) Implements IAuthorService.Add
        If String.IsNullOrWhiteSpace(dto.AuthorName) Then
            Throw New Exception("Tên tác giả không được rỗng")
        End If

        Dim author = _mapper.Map(Of Author)(dto)

        author.CreatedAt = DateTime.Now
        author.UpdatedAt = DateTime.Now
        author.IsDeleted = False

        _uow.Authors.Add(author)
        _uow.Save()
    End Sub
    Public Sub Update(id As Integer, dto As AuthorDto) Implements IAuthorService.Update
        Dim author = _uow.Authors.GetById(id)
        If author Is Nothing OrElse author.IsDeleted Then
            Throw New Exception("Không tìm thấy tác giả")
        End If

        If _uow.Authors.HasBorrowedBooks(id) Then
            Throw New Exception("Không thể chỉnh sửa vì đang có sách cho mượn")
        End If

        _mapper.Map(dto, author)

        author.UpdatedAt = DateTime.Now
        author.Id = id

        _uow.Save()
    End Sub

    Public Sub Delete(id As Integer) _
        Implements IAuthorService.Delete

        Dim author = _uow.Authors.GetById(id)
        If author Is Nothing OrElse author.IsDeleted Then
            Throw New Exception("Không tìm thấy tác giả")
        End If

        If _uow.Authors.HasBorrowedBooks(id) Then
            Throw New Exception("Không thể xóa vì đang có sách cho mượn")
        End If

        _uow.Authors.SoftDelete(author)
        _uow.Save()
    End Sub

    Public Function GetDetail(authorId As Integer,
                              bookKeyword As String,
                              pageIndex As Integer,
                              pageSize As Integer,
                              publisherId As Integer?) As AuthorDetailDto _
                              Implements IAuthorService.GetDetail

        Dim author = _uow.Authors.GetById(authorId)
        If author Is Nothing OrElse author.IsDeleted Then
            Throw New Exception("Không tìm thấy tác giả")
        End If

        Dim query = _uow.Books.GetAll().Where(Function(b) b.AuthorId = authorId)

        If Not String.IsNullOrWhiteSpace(bookKeyword) Then
            Dim kw = bookKeyword.Trim().ToLower()
            query = query.Where(Function(b) b.Title.ToLower().Contains(kw) OrElse
                                            b.BookCode.ToLower().Contains(kw))
        End If

        If publisherId.HasValue AndAlso publisherId.Value > 0 Then
            query = query.Where(Function(b) b.PublisherId = publisherId.Value)
        End If


        Dim totalCount = query.Count()
        Dim totalPages = CInt(Math.Ceiling(totalCount / pageSize))

        Dim bookItems = query.OrderByDescending(Function(b) b.Id) _
                             .Skip((pageIndex - 1) * pageSize) _
                             .Take(pageSize) _
                             .Select(Function(b) New AuthorBookDto With {
                                 .BookId = b.Id,
                                 .BookCode = b.BookCode,
                                 .Title = b.Title,
                                 .ImagePath = b.ImagePath,
                                 .Price = b.Price,
                                 .PublishYear = b.PublishYear,
                                 .CategoryName = If(b.Category IsNot Nothing, b.Category.CategoryName, "N/A"),
                                 .PublisherName = If(b.Publisher IsNot Nothing, b.Publisher.PublisherName, "N/A")
                             }).ToList()

        Return New AuthorDetailDto With {
            .AuthorId = author.Id,
            .AuthorName = author.AuthorName,
            .Books = New PagedResult(Of AuthorBookDto) With {
                .Items = bookItems,
                .TotalCount = totalCount,
                .TotalPages = totalPages
            }
        }
    End Function

    Public Function GetById(id As Integer) As AuthorDto Implements IAuthorService.GetById
        Dim entity = _uow.Authors.GetById(id)
        If entity Is Nothing OrElse entity.IsDeleted Then Return Nothing

        Return _mapper.Map(Of AuthorDto)(entity)
    End Function
End Class
