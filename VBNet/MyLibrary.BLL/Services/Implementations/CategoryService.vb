Imports System.Data.Entity
Imports AutoMapper
Imports MyLibrary.DAL
Imports MyLibrary.Domain
Imports NLog

Public Class CategoryService
    Implements ICategoryService

    Private ReadOnly _uow As IUnitOfWork
    Private ReadOnly _mapper As IMapper
    Private Shared ReadOnly logger As Logger = LogManager.GetCurrentClassLogger()

    Public Sub New(uow As IUnitOfWork, mapper As IMapper)
        _uow = uow
        _mapper = mapper
    End Sub

#Region "Get Paged Categories"

    Public Async Function GetPagedAsync(keyword As String,
                             pageIndex As Integer,
                             pageSize As Integer) As Task(Of PagedResult(Of Category)) _
        Implements ICategoryService.GetPagedAsync

        Try
            logger.Info("GetPaged Categories: Keyword={0}, PageIndex={1}, PageSize={2}",
                        keyword, pageIndex, pageSize)

            Dim query = _uow.Categories.GetAll()

            If Not String.IsNullOrWhiteSpace(keyword) Then
                query = query.Where(Function(a) a.CategoryName.Contains(keyword))
            End If

            Dim totalCount = Await query.CountAsync()
            Dim totalPages = CInt(Math.Ceiling(totalCount / pageSize))

            Dim items = Await query _
                .OrderBy(Function(a) a.CategoryName) _
                .Skip((pageIndex - 1) * pageSize) _
                .Take(pageSize) _
                .ToListAsync()

            Return New PagedResult(Of Category) With {
                .Items = items,
                .TotalCount = totalCount,
                .TotalPages = totalPages
            }

        Catch ex As Exception
            logger.Error(ex, "GetPaged Categories FAILED")
            Throw
        End Try
    End Function

#End Region

#Region "Add Category"
    'Create new Category
    Public Async Function AddAsync(dto As CategoryDto) As Task _
        Implements ICategoryService.AddAsync
        Try
            logger.Info("Add Category START: Name={0}", dto.CategoryName)

            If String.IsNullOrWhiteSpace(dto.CategoryName) Then
                logger.Warn("Tên danh mục không được rỗng")
                Throw New Exception(CategoryMessages.CategoryNameNotNull)
            End If

            If Await _uow.Categories.ExistsByNameAsync(dto.CategoryName) Then
                logger.Warn("Tên danh mục đã tồn tại")
                Throw New Exception(CategoryMessages.CategoryNameExist)
            End If

            Dim category = _mapper.Map(Of Category)(dto)
            category.CreatedAt = DateTime.Now
            category.UpdatedAt = DateTime.Now
            category.IsDeleted = False

            _uow.Categories.Add(category)
            Await _uow.SaveAsync()

            logger.Info("Add Category SUCCESS: Name={0}", dto.CategoryName)

        Catch ex As Exception
            logger.Error(ex, "Add Category FAILED: Name={0}", dto.CategoryName)
            Throw
        End Try
    End Function

#End Region

#Region "Update Category"
    'Update category
    Public Async Function UpdateAsync(id As Integer, dto As CategoryDto) As Task _
        Implements ICategoryService.UpdateAsync
        Try
            logger.Info("Update Category START: Id={0}, Name={1}", id, dto.CategoryName)

            Dim category = Await _uow.Categories.GetByIdAsync(id)
            If category Is Nothing OrElse category.IsDeleted Then
                logger.Warn("Không tìm thấy danh mục")
                Throw New Exception(CategoryMessages.CategoryNameNotFound)
            End If

            If Await _uow.Categories.HasBorrowedBooksAsync(id) Then
                logger.Warn("Không thể chỉnh sửa vì đang có sách cho mượn")
                Throw New Exception(CategoryMessages.BookHasBorrowed)
            End If

            If Await _uow.Categories.ExistsByNameAsync(dto.CategoryName, id) Then
                logger.Warn("Tên danh mục tồn tại")
                Throw New Exception(CategoryMessages.CategoryNameExist)
            End If

            _mapper.Map(dto, category)
            category.UpdatedAt = DateTime.Now

            Await _uow.SaveAsync()

            logger.Info("Update Category SUCCESS: Id={0}", id)

        Catch ex As Exception
            logger.Error(ex, "Update Category FAILED: Id={0}", id)
            Throw
        End Try
    End Function

#End Region

#Region "Delete Category"
    'Delete Category by soft delete
    Public Async Function DeleteAsync(id As Integer) As Task _
        Implements ICategoryService.DeleteAsync
        Try
            logger.Warn("Delete Category REQUEST: Id={0}", id)

            Dim category = Await _uow.Categories.GetByIdAsync(id)
            If category Is Nothing OrElse category.IsDeleted Then
                logger.Warn("Không tìm thấy danh mục")
                Throw New Exception(CategoryMessages.CategoryNameNotFound)
            End If

            If Await _uow.Categories.HasBorrowedBooksAsync(id) Then
                logger.Warn("Không thể chỉnh sửa vì đang có sách cho mượn")
                Throw New Exception(CategoryMessages.BookHasBorrowed)
            End If

            _uow.Categories.SoftDelete(category)
            Await _uow.SaveAsync()

            logger.Warn("Delete Category SUCCESS: Id={0}", id)

        Catch ex As Exception
            logger.Error(ex, "Delete Category FAILED: Id={0}", id)
            Throw
        End Try
    End Function

#End Region

#Region "Get Category Detail (Books in Category)"

    Public Async Function GetDetailAsync(categoryId As Integer,
                              bookKeyword As String,
                              pageIndex As Integer,
                              pageSize As Integer,
                              publisherId As Integer?) As Task(Of CategoryDetailDto) _
        Implements ICategoryService.GetDetailAsync

        Try
            logger.Info("GetDetail CategoryId={0}, Keyword={1}, Page={2}, Size={3}, PublisherId={4}",
                        categoryId, bookKeyword, pageIndex, pageSize, publisherId)

            Dim category = Await _uow.Categories.GetByIdAsync(categoryId)

            If category Is Nothing OrElse category.IsDeleted Then
                logger.Warn("Không tìm thấy danh mục")
                Throw New Exception(CategoryMessages.CategoryNameNotFound)
            End If

            Dim query = _uow.Books.GetAll().
                        Where(Function(b) b.CategoryId = categoryId)

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

            Dim bookItems = Await query _
                .OrderByDescending(Function(b) b.Id) _
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
                }).ToListAsync()

            Return New CategoryDetailDto With {
                .CategoryId = category.Id,
                .CategoryName = category.CategoryName,
                .Books = New PagedResult(Of CategoryBookDto) With {
                    .Items = bookItems,
                    .TotalCount = totalCount,
                    .TotalPages = totalPages
                }
            }

        Catch ex As Exception
            logger.Error(ex, "GetDetail FAILED: CategoryId={0}", categoryId)
            Throw
        End Try
    End Function

#End Region

#Region "Get Category By Id"

    Public Async Function GetByIdAsync(id As Integer) As Task(Of CategoryDto) _
        Implements ICategoryService.GetByIdAsync

        Try
            logger.Info("GetCategoryById: Id={0}", id)

            Dim entity = Await _uow.Categories.GetByIdAsync(id)
            If entity Is Nothing OrElse entity.IsDeleted Then Return Nothing

            Return _mapper.Map(Of CategoryDto)(entity)

        Catch ex As Exception
            logger.Error(ex, "GetCategoryById FAILED: Id={0}", id)
            Throw
        End Try
    End Function

#End Region

End Class
