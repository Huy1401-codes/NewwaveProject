Imports AutoMapper
Imports MyLibrary.DAL
Imports MyLibrary.Domain
Imports NLog

Public Class PublisherService
    Implements IPublisherService
    Private ReadOnly _uow As IUnitOfWork
    Private ReadOnly _mapper As IMapper
    Private Shared ReadOnly logger As Logger = LogManager.GetCurrentClassLogger()

    Public Sub New(uow As IUnitOfWork, mapper As IMapper)
        _uow = uow
        _mapper = mapper
    End Sub

#Region "Get Paged Publishers"

    Public Function GetPaged(keyword As String,
                             pageIndex As Integer,
                             pageSize As Integer) As PagedResult(Of Publisher) _
        Implements IPublisherService.GetPaged

        Try
            logger.Info("GetPaged Publishers: Keyword={0}, PageIndex={1}, PageSize={2}",
                        keyword, pageIndex, pageSize)

            Dim query = _uow.Publishers.GetAll()

            If Not String.IsNullOrWhiteSpace(keyword) Then
                query = query.Where(Function(a) a.PublisherName.Contains(keyword))
            End If

            Dim totalCount = query.Count()
            Dim totalPages = CInt(Math.Ceiling(totalCount / pageSize))

            Dim items = query _
                .OrderBy(Function(a) a.PublisherName) _
                .Skip((pageIndex - 1) * pageSize) _
                .Take(pageSize) _
                .ToList()

            Return New PagedResult(Of Publisher) With {
                .Items = items,
                .TotalCount = totalCount,
                .TotalPages = totalPages
            }

        Catch ex As Exception
            logger.Error(ex, "GetPaged Publishers FAILED")
            Throw
        End Try
    End Function

#End Region

#Region "Add Publisher"
    'Create new Publisher
    Public Sub Add(dto As PublisherDto) Implements IPublisherService.Add
        Try
            logger.Info("Add Publisher START: Name={0}", dto.PublisherName)

            If String.IsNullOrWhiteSpace(dto.PublisherName) Then
                logger.Warn("Không được để trống NXB.")
                Throw New Exception(PublisherMessages.PublisherNameNotNull)
            End If

            If _uow.Publishers.ExistsByName(dto.PublisherName) Then
                logger.Warn("Tên NXB đã tồn tại.")
                Throw New Exception(PublisherMessages.PublisherNameExist)
            End If

            Dim Publisher = _mapper.Map(Of Publisher)(dto)
            Publisher.CreatedAt = DateTime.Now
            Publisher.UpdatedAt = DateTime.Now
            Publisher.IsDeleted = False

            _uow.Publishers.Add(Publisher)
            _uow.Save()

            logger.Info("Add Publisher SUCCESS: Name={0}", dto.PublisherName)

        Catch ex As Exception
            logger.Error(ex, "Add Publisher FAILED: Name={0}", dto.PublisherName)
            Throw
        End Try
    End Sub

#End Region

#Region "Update Publisher"
    'Update Publisher
    Public Sub Update(id As Integer, dto As PublisherDto) Implements IPublisherService.Update
        Try
            logger.Info("Update Publisher START: Id={0}, Name={1}", id, dto.PublisherName)

            Dim Publisher = _uow.Publishers.GetById(id)
            If Publisher Is Nothing OrElse Publisher.IsDeleted Then
                logger.Warn("Không tìm thấy NXB")
                Throw New Exception(PublisherMessages.PublisherNameNotFound)
            End If

            If _uow.Publishers.HasBorrowedBooks(id) Then
                logger.Warn("Không thể chỉnh sửa vì đang có sách cho mượn")
                Throw New Exception(PublisherMessages.BookHasBorrowed)
            End If

            If _uow.Publishers.ExistsByName(dto.PublisherName, id) Then
                logger.Warn("Tên NXB tồn tại")
                Throw New Exception(PublisherMessages.PublisherNameExist)
            End If

            _mapper.Map(dto, Publisher)
            Publisher.UpdatedAt = DateTime.Now

            _uow.Save()

            logger.Info("Update Publisher SUCCESS: Id={0}", id)

        Catch ex As Exception
            logger.Error(ex, "Update Publisher FAILED: Id={0}", id)
            Throw
        End Try
    End Sub

#End Region

#Region "Delete Publisher"
    'Delete Publisher by soft delete
    Public Sub Delete(id As Integer) Implements IPublisherService.Delete
        Try
            logger.Warn("Delete Publisher REQUEST: Id={0}", id)

            Dim Publisher = _uow.Publishers.GetById(id)
            If Publisher Is Nothing OrElse Publisher.IsDeleted Then
                logger.Warn("Không tìm thấy NXB")
                Throw New Exception(PublisherMessages.PublisherNameNotFound)
            End If

            If _uow.Publishers.HasBorrowedBooks(id) Then
                logger.Warn("Không thể chỉnh sửa vì đang có sách cho mượn")
                Throw New Exception(PublisherMessages.BookHasBorrowed)
            End If

            _uow.Publishers.SoftDelete(Publisher)
            _uow.Save()

            logger.Warn("Delete Publisher SUCCESS: Id={0}", id)

        Catch ex As Exception
            logger.Error(ex, "Delete Publisher FAILED: Id={0}", id)
            Throw
        End Try
    End Sub

#End Region

#Region "Get Publisher Detail (Books in Publisher)"

    Public Function GetDetail(publisherId As Integer,
                              bookKeyword As String,
                              pageIndex As Integer,
                              pageSize As Integer,
                              categoryId As Integer?) As PublisherDetailDto _
        Implements IPublisherService.GetDetail

        Try
            logger.Info("GetDetail PublisherId={0}, Keyword={1}, Page={2}, Size={3}, CategoryId={4}",
                        publisherId, bookKeyword, pageIndex, pageSize, categoryId)

            Dim Publisher = _uow.Publishers.GetById(publisherId)

            If Publisher Is Nothing OrElse Publisher.IsDeleted Then
                logger.Warn("Không tìm thấy NXB")
                Throw New Exception(PublisherMessages.PublisherNameNotFound)
            End If

            Dim query = _uow.Books.GetAll().
                        Where(Function(b) b.PublisherId = publisherId)

            If Not String.IsNullOrWhiteSpace(bookKeyword) Then
                Dim kw = bookKeyword.Trim().ToLower()
                query = query.Where(Function(b) b.Title.ToLower().Contains(kw) OrElse
                                                b.BookCode.ToLower().Contains(kw))
            End If

            If categoryId.HasValue AndAlso categoryId.Value > 0 Then
                query = query.Where(Function(b) b.PublisherId = categoryId.Value)
            End If

            Dim totalCount = query.Count()
            Dim totalPages = CInt(Math.Ceiling(totalCount / pageSize))

            Dim bookItems = query _
                .OrderByDescending(Function(b) b.Id) _
                .Skip((pageIndex - 1) * pageSize) _
                .Take(pageSize) _
                .Select(Function(b) New PublisherBookDto With {
                    .BookId = b.Id,
                    .BookCode = b.BookCode,
                    .Title = b.Title,
                    .ImagePath = b.ImagePath,
                    .Price = b.Price,
                    .PublishYear = b.PublishYear,
                    .AuthorName = If(b.Author IsNot Nothing, b.Author.AuthorName, "N/A"),
                    .CategoryName = If(b.Category IsNot Nothing, b.Category.CategoryName, "N/A")
                }).ToList()

            Return New PublisherDetailDto With {
                .PublisherId = Publisher.Id,
                .PublisherName = Publisher.PublisherName,
                .Books = New PagedResult(Of PublisherBookDto) With {
                    .Items = bookItems,
                    .TotalCount = totalCount,
                    .TotalPages = totalPages
                }
            }

        Catch ex As Exception
            logger.Error(ex, "GetDetail FAILED: PublisherId={0}", publisherId)
            Throw
        End Try
    End Function

#End Region

#Region "Get Publisher By Id"

    Public Function GetById(id As Integer) As PublisherDto _
        Implements IPublisherService.GetById

        Try
            logger.Info("GetPublisherById: Id={0}", id)

            Dim entity = _uow.Publishers.GetById(id)
            If entity Is Nothing OrElse entity.IsDeleted Then Return Nothing

            Return _mapper.Map(Of PublisherDto)(entity)

        Catch ex As Exception
            logger.Error(ex, "GetPublisherById FAILED: Id={0}", id)
            Throw
        End Try
    End Function

#End Region

End Class
