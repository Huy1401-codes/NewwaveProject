Imports System.ComponentModel
Imports System.IO
Imports AutoMapper
Imports MyLibrary.DAL
Imports MyLibrary.Domain
Imports MyLibrary.Domain.MyApp.Domain.Common
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

    Public Function GetAllBooksForView() As List(Of BookDto) Implements IBookService.GetAllBooksForView
        Dim entities = _uow.Books.GetBooksFullInfo()
        Return _mapper.Map(Of List(Of BookDto))(entities)
    End Function

    Public Function GetBooksCombined(keyword As String, publisherId As Integer?, year As Integer?, pageIndex As Integer,
                                     pageSize As Integer) As PagedResult(Of BookDto) Implements IBookService.GetBooksCombined
        Dim query = _uow.Books.GetBooksFullInfo().AsQueryable()


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

        Dim totalRecords = query.Count()
        Dim totalPages = CInt(Math.Ceiling(totalRecords / pageSize))

        Dim entities = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()

        Dim dtos = _mapper.Map(Of List(Of BookDto))(entities)

        Return New PagedResult(Of BookDto) With {
        .Items = dtos,
        .TotalCount = totalRecords,
        .TotalPages = totalPages
    }
    End Function

    Public Sub AddBook(bookDto As BookDto) Implements IBookService.AddBook

        If bookDto Is Nothing Then
            Throw New ArgumentNullException(NameOf(bookDto))
        End If

        If bookDto.Quantity < 0 Then
            Throw New Exception("Số lượng không được là số âm")
        End If

        If bookDto.AvailableQuantity > 0 AndAlso
       bookDto.AvailableQuantity > bookDto.Quantity Then

            Throw New Exception("Số lượng còn lại không được lớn hơn tổng số lượng")
        End If

        Dim exists = _uow.Books.GetAll().
        Any(Function(b) b.BookCode = bookDto.BookCode AndAlso Not b.IsDeleted)

        If exists Then
            Throw New Exception(BookMessages.BookCodeExist)
        End If

        Dim newBook = _mapper.Map(Of Book)(bookDto)

        newBook.AvailableQuantity = newBook.Quantity
        newBook.CreatedAt = DateTime.Now
        newBook.UpdatedAt = DateTime.Now
        newBook.IsDeleted = False

        _uow.Books.Add(newBook)
        _uow.Save()

    End Sub


    Public Sub UpdateBook(bookDto As BookDto) Implements IBookService.UpdateBook

        Dim existingBook = _uow.Books.GetById(bookDto.Id)

        If existingBook Is Nothing Then
            logger.Warn("Không tìm thấy sách")
            Throw New Exception(BookMessages.BookIsNull)
        End If

        If bookDto.Price < 0 OrElse bookDto.Quantity < 0 Then
            logger.Warn("Không nhận giá trị âm")
            Throw New Exception(BookMessages.NotInterger)
        End If


        Dim borrowed As Integer =
        existingBook.Quantity - existingBook.AvailableQuantity

        If bookDto.Quantity < borrowed Then
            logger.Warn("Số lượng mới nhỏ hơn số đang cho mượn")
            Throw New Exception(
            $"Số lượng mới ({bookDto.Quantity}) không được nhỏ hơn số đang cho mượn ({borrowed})")
        End If


        _mapper.Map(bookDto, existingBook)
        existingBook.AvailableQuantity =
        bookDto.Quantity - borrowed
        existingBook.UpdatedAt = DateTime.Now

        If existingBook.CreatedAt < New DateTime(1753, 1, 1) Then
            existingBook.CreatedAt = DateTime.Now
        End If

        logger.Info("Cập nhật sách thành công")
        _uow.Save()
    End Sub


    Public Sub DeleteBook(id As Integer) Implements IBookService.DeleteBook
        Dim book = _uow.Books.GetById(id)

        If book.Quantity <> book.AvailableQuantity Then
            logger.Warn("Không thể xóa sách vì đã có sách được mượn.")
            Throw New Exception(BookMessages.BookLend)
        End If

        If book IsNot Nothing Then
            _uow.Books.SoftDelete(book)
            _uow.Save()
        End If
    End Sub

    Public Function GetBookById(id As Integer) As Book Implements IBookService.GetBookById
        Return _uow.Books.GetById(id)
    End Function

    Public Function GetAuthors() As List(Of Author) Implements IBookService.GetAuthors
        Return _uow.Authors.GetAll().ToList()
    End Function

    Public Function GetCategories() As List(Of Category) Implements IBookService.GetCategories
        Return _uow.Categories.GetAll().ToList()
    End Function

    Public Function GetPublishers() As List(Of Publisher) Implements IBookService.GetPublishers
        Return _uow.Publishers.GetAll().ToList()
    End Function

    Public Function ExportBooksToExcel(books As List(Of BookDto)) As Byte() Implements IBookService.ExportBooksToExcel
        Using package As New ExcelPackage()
            Dim ws = package.Workbook.Worksheets.Add("Books")

            ws.Cells(1, 1).Value = "Id"
            ws.Cells(1, 2).Value = "Book Code"
            ws.Cells(1, 3).Value = "Title"
            ws.Cells(1, 4).Value = "Author"
            ws.Cells(1, 5).Value = "Category"
            ws.Cells(1, 6).Value = "Publisher"
            ws.Cells(1, 7).Value = "Publish Year"
            ws.Cells(1, 8).Value = "Price"
            ws.Cells(1, 9).Value = "Quantity"
            ws.Cells(1, 10).Value = "Available Quantity"

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

    Public Sub ImportBooksFromExcelToDb(fileBytes As Byte()) _
    Implements IBookService.ImportBooksFromExcelToDb

        Using stream As New MemoryStream(fileBytes)
            Using package As New ExcelPackage(stream)
                Dim ws = package.Workbook.Worksheets.FirstOrDefault()
                If ws Is Nothing Then
                    Throw New Exception("File Excel không hợp lệ")
                End If

                Dim rowCount = ws.Dimension.End.Row

                For row As Integer = 2 To rowCount

                    Dim bookCode = ws.Cells(row, 2).Text.Trim()
                    Dim title = ws.Cells(row, 3).Text.Trim()

                    If String.IsNullOrWhiteSpace(bookCode) OrElse
                   String.IsNullOrWhiteSpace(title) Then
                        Continue For
                    End If

                    If _uow.Books.ExistsByCode(bookCode) Then
                        Continue For
                    End If

                    Dim authorName = ws.Cells(row, 4).Text.Trim()
                    Dim author = _uow.Authors.GetByName(authorName)
                    If author Is Nothing Then
                        author = New Author With {
                        .AuthorName = authorName,
                        .CreatedAt = DateTime.Now,
                        .IsDeleted = False
                    }
                        _uow.Authors.Add(author)
                    End If

                    Dim categoryName = ws.Cells(row, 5).Text.Trim()
                    Dim category = _uow.Categories.GetByName(categoryName)
                    If category Is Nothing Then
                        category = New Category With {
                        .CategoryName = categoryName,
                        .CreatedAt = DateTime.Now,
                        .IsDeleted = False
                    }
                        _uow.Categories.Add(category)
                    End If

                    Dim publisherName = ws.Cells(row, 6).Text.Trim()
                    Dim publisher = _uow.Publishers.GetByName(publisherName)
                    If publisher Is Nothing Then
                        publisher = New Publisher With {
                        .PublisherName = publisherName,
                        .CreatedAt = DateTime.Now,
                        .IsDeleted = False
                    }
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
                    logger.Info("Import thành công")
                    _uow.Books.Add(book)

                Next

                _uow.Save()
            End Using
        End Using
    End Sub

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