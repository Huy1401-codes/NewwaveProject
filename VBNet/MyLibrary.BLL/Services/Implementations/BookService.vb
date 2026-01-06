Imports System.Data.Entity
Imports System.IO
Imports AutoMapper
Imports MyLibrary.DAL
Imports MyLibrary.Domain
Imports NLog
Imports OfficeOpenXml

Public Class BookService
    Implements IBookService

    Private ReadOnly _uow As IUnitOfWork
    Private ReadOnly _mapper As IMapper
    Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

    Public Sub New(uow As IUnitOfWork, mapper As IMapper)
        _uow = uow
        _mapper = mapper
    End Sub

    Public Async Function GetAllBooksForViewAsync() As Task(Of List(Of BookDto)) _
        Implements IBookService.GetAllBooksForViewAsync

        Dim entities = Await _uow.Books.GetBooksFullInfoAsync()
        Return _mapper.Map(Of List(Of BookDto))(entities)
    End Function

    Public Async Function GetBooksCombinedAsync(keyword As String, publisherId As Integer?, year As Integer?, pageIndex As Integer,
                                     pageSize As Integer) As Task(Of PagedResult(Of BookDto)) _
        Implements IBookService.GetBooksCombinedAsync

        Dim query = Await _uow.Books.GetBooksFullInfoAsync()

        If Not String.IsNullOrWhiteSpace(keyword) Then
            Dim k = keyword.ToLower().Trim()
            query = query.Where(Function(b) b.Title.ToLower().Contains(k) OrElse b.BookCode.ToLower().Contains(k))
        End If

        If publisherId.HasValue AndAlso publisherId.Value > 0 Then
            query = query.Where(Function(b) b.PublisherId = publisherId.Value)
        End If

        If year.HasValue AndAlso year.Value > 0 Then
            query = query.Where(Function(b) b.PublishYear = year.Value)
        End If

        Dim totalRecords = query.Count
        Dim totalPages = CInt(Math.Ceiling(totalRecords / pageSize))

        Dim entities = query.OrderByDescending(Function(b) b.Id) _
                                  .Skip((pageIndex - 1) * pageSize) _
                                  .Take(pageSize) _
                                  .ToList()

        Dim dtos = _mapper.Map(Of List(Of BookDto))(entities)

        Return New PagedResult(Of BookDto) With {
            .Items = dtos,
            .TotalCount = totalRecords,
            .TotalPages = totalPages
        }
    End Function

    Public Async Function AddBookAsync(bookDto As BookDto) As Task _
        Implements IBookService.AddBookAsync

        If bookDto Is Nothing Then Throw New ArgumentNullException(NameOf(bookDto))

        If bookDto.Quantity < 0 Then Throw New Exception("Số lượng không được là số âm")

        If bookDto.AvailableQuantity > 0 AndAlso bookDto.AvailableQuantity > bookDto.Quantity Then
            Throw New Exception("Số lượng còn lại không được lớn hơn tổng số lượng")
        End If

        ' Check trùng mã Async
        Dim exists = Await _uow.Books.ExistsByCodeAsync(bookDto.BookCode)

        If exists Then
            Throw New Exception(BookMessages.BookCodeExist)
        End If

        Dim newBook = _mapper.Map(Of Book)(bookDto)

        newBook.AvailableQuantity = newBook.Quantity
        newBook.CreatedAt = DateTime.Now
        newBook.UpdatedAt = DateTime.Now
        newBook.IsDeleted = False

        _uow.Books.Add(newBook)

        Await _uow.SaveAsync()
    End Function

    Public Async Function UpdateBookAsync(bookDto As BookDto) As Task _
        Implements IBookService.UpdateBookAsync

        Dim existingBook = Await _uow.Books.GetByIdAsync(bookDto.Id)

        If existingBook Is Nothing Then
            logger.Warn("Không tìm thấy sách")
            Throw New Exception(BookMessages.BookIsNull)
        End If

        If bookDto.Price < 0 OrElse bookDto.Quantity < 0 Then
            logger.Warn("Không nhận giá trị âm")
            Throw New Exception(BookMessages.NotInterger)
        End If

        Dim borrowed As Integer = existingBook.Quantity - existingBook.AvailableQuantity

        If bookDto.Quantity < borrowed Then
            logger.Warn("Số lượng mới nhỏ hơn số đang cho mượn")
            Throw New Exception($"Số lượng mới ({bookDto.Quantity}) không được nhỏ hơn số đang cho mượn ({borrowed})")
        End If

        _mapper.Map(bookDto, existingBook)

        existingBook.AvailableQuantity = bookDto.Quantity - borrowed
        existingBook.UpdatedAt = DateTime.Now

        If existingBook.CreatedAt < New DateTime(1753, 1, 1) Then
            existingBook.CreatedAt = DateTime.Now
        End If

        logger.Info("Cập nhật sách thành công")

        Await _uow.SaveAsync()
    End Function

    Public Async Function DeleteBookAsync(id As Integer) As Task _
        Implements IBookService.DeleteBookAsync

        Dim book = Await _uow.Books.GetByIdAsync(id)

        If book Is Nothing Then Return

        If book.Quantity <> book.AvailableQuantity Then
            logger.Warn("Không thể xóa sách vì đã có sách được mượn.")
            Throw New Exception(BookMessages.BookLend)
        End If

        _uow.Books.SoftDelete(book)

        Await _uow.SaveAsync()
    End Function

    Public Async Function GetBookByIdAsync(id As Integer) As Task(Of Book) _
        Implements IBookService.GetBookByIdAsync

        Return Await _uow.Books.GetByIdAsync(id)
    End Function

    Public Async Function GetBookDetailAsync(bookId As Integer) As Task(Of BookDetailDto) _
        Implements IBookService.GetBookDetailAsync

        Dim book = Await _uow.Books.GetByIdAsync(bookId)
        If book Is Nothing Then Return Nothing

        Return _mapper.Map(Of BookDetailDto)(book)
    End Function

    Public Async Function GetAuthorsAsync() As Task(Of List(Of Author)) _
        Implements IBookService.GetAuthorsAsync
        Return Await _uow.Authors.GetAll().ToListAsync()
    End Function

    Public Async Function GetCategoriesAsync() As Task(Of List(Of Category)) _
        Implements IBookService.GetCategoriesAsync
        Return Await _uow.Categories.GetAll().ToListAsync()
    End Function

    Public Async Function GetPublishersAsync() As Task(Of List(Of Publisher)) _
        Implements IBookService.GetPublishersAsync
        Return Await _uow.Publishers.GetAll().ToListAsync()
    End Function

    Public Async Function GetAvailableBooksAsync(keyword As String, publisherId As Integer?,
                                    categoryId As Integer?) As Task(Of List(Of BookDetailDto)) _
        Implements IBookService.GetAvailableBooksAsync

        Dim query = _uow.Books.GetAll()

        query = query.Where(Function(b) b.IsDeleted = False AndAlso
                                        b.Quantity > 0 AndAlso
                                        b.AvailableQuantity > 0)

        If Not String.IsNullOrEmpty(keyword) Then
            query = query.Where(Function(b) b.Title.Contains(keyword))
        End If

        If publisherId.HasValue AndAlso publisherId.Value > 0 Then
            query = query.Where(Function(b) b.PublisherId = publisherId.Value)
        End If

        If categoryId.HasValue AndAlso categoryId.Value > 0 Then
            query = query.Where(Function(b) b.CategoryId = categoryId.Value)
        End If

        Dim entities = Await query.ToListAsync()

        Return _mapper.Map(Of List(Of BookDetailDto))(entities)
    End Function

    Public Function ExportBooksToExcel(books As List(Of BookDto)) As Byte() _
        Implements IBookService.ExportBooksToExcel


        Using package As New ExcelPackage()
            Dim ws = package.Workbook.Worksheets.Add("Books")

            Dim headers = {"Id", "Book Code", "Title", "Author", "Category", "Publisher", "Publish Year", "Price", "Quantity", "Available Quantity"}
            For i = 0 To headers.Length - 1
                ws.Cells(1, i + 1).Value = headers(i)
            Next

            Dim row As Integer = 2
            For Each b In books
                ws.Cells(row, 1).Value = b.Id
                ws.Cells(row, 2).Value = b.BookCode
                ws.Cells(row, 3).Value = b.Title
                ws.Cells(row, 4).Value = b.AuthorName
                ws.Cells(row, 5).Value = b.CategoryName
                ws.Cells(row, 6).Value = b.PublisherName
                ws.Cells(row, 7).Value = b.PublishYear
                ws.Cells(row, 8).Value = b.Price
                ws.Cells(row, 9).Value = b.Quantity
                ws.Cells(row, 10).Value = b.AvailableQuantity
                row += 1
            Next

            ws.Cells.AutoFitColumns()
            Return package.GetAsByteArray()
        End Using
    End Function

    Public Async Function ImportBooksFromExcelToDbAsync(fileBytes As Byte()) As Task _
        Implements IBookService.ImportBooksFromExcelToDbAsync

        Using stream As New MemoryStream(fileBytes)
            Using package As New ExcelPackage(stream)
                Dim ws = package.Workbook.Worksheets.FirstOrDefault()
                If ws Is Nothing Then Throw New Exception("File Excel không hợp lệ")

                Dim rowCount = ws.Dimension.End.Row

                For row As Integer = 2 To rowCount

                    Dim bookCode = ws.Cells(row, 2).Text.Trim()
                    Dim title = ws.Cells(row, 3).Text.Trim()

                    If String.IsNullOrWhiteSpace(bookCode) OrElse String.IsNullOrWhiteSpace(title) Then Continue For


                    If Await _uow.Books.ExistsByCodeAsync(bookCode) Then Continue For


                    Dim authorName = ws.Cells(row, 4).Text.Trim()

                    Dim author = Await _uow.Authors.GetByNameAsync(authorName)
                    If author Is Nothing Then
                        author = New Author With {.AuthorName = authorName, .CreatedAt = DateTime.Now, .IsDeleted = False}
                        _uow.Authors.Add(author)

                    End If

                    Dim categoryName = ws.Cells(row, 5).Text.Trim()
                    Dim category = Await _uow.Categories.GetByNameAsync(categoryName)
                    If category Is Nothing Then
                        category = New Category With {.CategoryName = categoryName, .CreatedAt = DateTime.Now, .IsDeleted = False}
                        _uow.Categories.Add(category)
                    End If

                    Dim publisherName = ws.Cells(row, 6).Text.Trim()
                    Dim publisher = Await _uow.Publishers.GetByNameAsync(publisherName)
                    If publisher Is Nothing Then
                        publisher = New MyLibrary.Domain.Publisher With {.PublisherName = publisherName, .CreatedAt = DateTime.Now, .IsDeleted = False}
                        _uow.Publishers.Add(publisher)
                    End If

                    Dim book As New Book With {
                        .BookCode = bookCode,
                        .Title = title,
                        .Author = author,
                        .Category = category,
                        .Publisher = publisher,
                        .PublishYear = SafeInt(ws.Cells(row, 7).Text),
                        .Price = SafeDecimal(ws.Cells(row, 8).Text),
                        .Quantity = SafeInt(ws.Cells(row, 9).Text),
                        .CreatedAt = DateTime.Now,
                        .IsDeleted = False
                    }
                    book.AvailableQuantity = book.Quantity

                    _uow.Books.Add(book)
                Next

                logger.Info("Import dữ liệu đang lưu xuống DB...")
                Await _uow.SaveAsync()
                logger.Info("Import thành công")
            End Using
        End Using
    End Function

    Private Function SafeInt(value As String) As Integer
        Dim r As Integer
        Integer.TryParse(value, r)
        Return r
    End Function

    Private Function SafeDecimal(value As String) As Decimal
        Dim r As Decimal
        Decimal.TryParse(value, r)
        Return r
    End Function

End Class