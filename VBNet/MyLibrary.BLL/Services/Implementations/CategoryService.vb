Imports AutoMapper
Imports MyLibrary.DAL
Imports MyLibrary.Domain

Public Class CategoryService
    Implements ICategoryService

    Private ReadOnly _uow As IUnitOfWork
    Private ReadOnly _mapper As IMapper
    Public Sub New(uow As IUnitOfWork, mapper As IMapper)
        _uow = uow
        _mapper = mapper
    End Sub

    Public Function GetPaged(keyword As String,
                         pageIndex As Integer,
                         pageSize As Integer) As PagedResult(Of Category) _
    Implements ICategoryService.GetPaged

        Dim query = _uow.Categories.GetAll()

        If Not String.IsNullOrWhiteSpace(keyword) Then
            query = query.Where(Function(a) a.categoryName.Contains(keyword))
        End If

        Dim totalCount = query.Count()
        Dim totalPages = Math.Ceiling(totalCount / pageSize)

        Dim items = query _
        .OrderBy(Function(a) a.categoryName) _
        .Skip((pageIndex - 1) * pageSize) _
        .Take(pageSize) _
        .ToList()

        Return New PagedResult(Of Category) With {
        .Items = items,
        .TotalCount = totalCount,
        .TotalPages = totalPages
    }
    End Function

    Public Sub Add(dto As CategoryDto) Implements ICategoryService.Add
        If String.IsNullOrWhiteSpace(dto.CategoryName) Then
            Throw New Exception("Tên danh mục không được rỗng")
        End If

        If _uow.Categories.ExistsByName(dto.CategoryName) Then
            Throw New Exception("Tên danh mục đã tồn tại")
        End If

        Dim category = _mapper.Map(Of Category)(dto)

        category.CreatedAt = DateTime.Now
        category.UpdatedAt = DateTime.Now
        category.IsDeleted = False

        _uow.Categories.Add(category)
        _uow.Save()
    End Sub

    Public Sub Update(id As Integer, dto As CategoryDto) Implements ICategoryService.Update
        Dim category = _uow.Categories.GetById(id)
        If category Is Nothing OrElse category.IsDeleted Then
            Throw New Exception("Không tìm thấy danh mục")
        End If

        If _uow.Categories.HasBorrowedBooks(id) Then
            Throw New Exception("Không thể chỉnh sửa vì đang có sách cho mượn")
        End If

        If _uow.Categories.ExistsByName(dto.CategoryName, id) Then
            Throw New Exception("Tên danh mục đã tồn tại")
        End If

        _mapper.Map(dto, category)

        category.UpdatedAt = DateTime.Now
        category.Id = id

        _uow.Save()
    End Sub

    Public Sub Delete(id As Integer) _
        Implements ICategoryService.Delete

        Dim category = _uow.Categories.GetById(id)
        If category Is Nothing OrElse category.IsDeleted Then
            Throw New Exception("Không tìm thấy danh mục")
        End If

        If _uow.Categories.HasBorrowedBooks(id) Then
            Throw New Exception("Không thể xóa vì đang có sách cho mượn")
        End If

        _uow.Categories.SoftDelete(category)
        _uow.Save()
    End Sub

    Public Function GetDetail(categoryId As Integer,
                              bookKeyword As String,
                              pageIndex As Integer,
                              pageSize As Integer,
                              publisherId As Integer?) As CategoryDetailDto _
                              Implements ICategoryService.GetDetail

        Dim category = _uow.Categories.GetById(categoryId)
        If category Is Nothing OrElse category.IsDeleted Then
            Throw New Exception("Không tìm thấy danh mục")
        End If

        Dim query = _uow.Books.GetAll().Where(Function(b) b.CategoryId = categoryId)

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
                             .Select(Function(b) New CategoryBookDto With {
                                 .BookId = b.Id,
                                 .BookCode = b.BookCode,
                                 .Title = b.Title,
                                 .ImagePath = b.ImagePath,
                                 .Price = b.Price,
                                 .PublishYear = b.PublishYear,
                                 .AuthorName = If(b.Author IsNot Nothing, b.Author.AuthorName, "N/A"),
                                 .PublisherName = If(b.Publisher IsNot Nothing, b.Publisher.PublisherName, "N/A")
                             }).ToList()

        Return New CategoryDetailDto With {
            .CategoryId = category.Id,
            .CategoryName = category.CategoryName,
            .Books = New PagedResult(Of CategoryBookDto) With {
                .Items = bookItems,
                .TotalCount = totalCount,
                .TotalPages = totalPages
            }
        }
    End Function

    Public Function GetById(id As Integer) As CategoryDto Implements ICategoryService.GetById
        Dim entity = _uow.Categories.GetById(id)
        If entity Is Nothing OrElse entity.IsDeleted Then Return Nothing

        Return _mapper.Map(Of CategoryDto)(entity)
    End Function
End Class
