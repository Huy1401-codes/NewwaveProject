Imports System.Data.Entity
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

    Public Async Function GetPagedAsync(keyword As String,
                                        pageIndex As Integer,
                                        pageSize As Integer) As Task(Of PagedResult(Of Author)) _
        Implements IAuthorService.GetPagedAsync

        Dim query = _uow.Authors.GetAll()

        If Not String.IsNullOrWhiteSpace(keyword) Then
            query = query.Where(Function(a) a.AuthorName.Contains(keyword))
        End If

        Dim totalCount = Await query.CountAsync()

        Dim totalPages = Math.Ceiling(totalCount / pageSize)

        Dim items = Await query.OrderBy(Function(a) a.AuthorName) _
                               .Skip((pageIndex - 1) * pageSize) _
                               .Take(pageSize) _
                               .ToListAsync()

        Return New PagedResult(Of Author) With {
            .Items = items,
            .TotalCount = totalCount,
            .TotalPages = totalPages
        }
    End Function

    Public Async Function AddAsync(dto As AuthorDto) As Task _
        Implements IAuthorService.AddAsync

        If String.IsNullOrWhiteSpace(dto.AuthorName) Then
            Throw New Exception("Tên tác giả không được rỗng")
        End If

        Dim author = _mapper.Map(Of Author)(dto)

        author.CreatedAt = DateTime.Now
        author.UpdatedAt = DateTime.Now
        author.IsDeleted = False

        _uow.Authors.Add(author)

        Await _uow.SaveAsync()
    End Function

    Public Async Function UpdateAsync(id As Integer, dto As AuthorDto) As Task _
        Implements IAuthorService.UpdateAsync

        ' GetByIdAsync
        Dim author = Await _uow.Authors.GetByIdAsync(id)

        If author Is Nothing OrElse author.IsDeleted Then
            Throw New Exception("Không tìm thấy tác giả")
        End If


        If Await _uow.Authors.HasBorrowedBooksAsync(id) Then
            Throw New Exception("Không thể chỉnh sửa vì đang có sách cho mượn")
        End If

        _mapper.Map(dto, author)

        author.UpdatedAt = DateTime.Now
        author.Id = id

        Await _uow.SaveAsync()
    End Function

    Public Async Function DeleteAsync(id As Integer) As Task _
        Implements IAuthorService.DeleteAsync

        Dim author = Await _uow.Authors.GetByIdAsync(id)

        If author Is Nothing OrElse author.IsDeleted Then
            Throw New Exception("Không tìm thấy tác giả")
        End If

        If Await _uow.Authors.HasBorrowedBooksAsync(id) Then
            Throw New Exception("Không thể xóa vì đang có sách cho mượn")
        End If

        _uow.Authors.SoftDelete(author)

        Await _uow.SaveAsync()
    End Function

    Public Async Function GetDetailAsync(authorId As Integer,
                                         bookKeyword As String,
                                         pageIndex As Integer,
                                         pageSize As Integer,
                                         publisherId As Integer?) As Task(Of AuthorDetailDto) _
        Implements IAuthorService.GetDetailAsync

        Dim author = Await _uow.Authors.GetByIdAsync(authorId)

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

        Dim totalCount = Await query.CountAsync()
        Dim totalPages = CInt(Math.Ceiling(totalCount / pageSize))

        Dim bookItems = Await query.OrderByDescending(Function(b) b.Id) _
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
                                   }).ToListAsync()

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

    Public Async Function GetByIdAsync(id As Integer) As Task(Of AuthorDto) _
        Implements IAuthorService.GetByIdAsync

        Dim entity = Await _uow.Authors.GetByIdAsync(id)

        If entity Is Nothing OrElse entity.IsDeleted Then Return Nothing

        Return _mapper.Map(Of AuthorDto)(entity)
    End Function

End Class