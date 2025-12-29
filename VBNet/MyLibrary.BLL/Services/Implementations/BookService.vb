Imports System.ComponentModel
Imports AutoMapper
Imports MyLibrary.DAL
Imports MyLibrary.Domain
Imports MyLibrary.Domain.MyApp.Domain.Common
Imports OfficeOpenXml

Public Class BookService
    Implements IBookService

    Private ReadOnly _uow As IUnitOfWork
    Private ReadOnly _mapper As IMapper

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
        Dim newBook = _mapper.Map(Of Book)(bookDto)
        newBook.CreatedAt = DateTime.Now
        newBook.UpdatedAt = DateTime.Now

        Dim existingCode = _uow.Books.GetAll().FirstOrDefault(Function(b) b.BookCode = bookDto.BookCode)

        If existingCode IsNot Nothing Then
            Throw New Exception("Mã code đã tồn tại ! ")
        End If

        If newBook.Price < 0 Or newBook.Quantity < 0 Then
            Throw New Exception("Không nhận giá trị âm ! ")
        End If

        If newBook.CreatedAt < New DateTime(1753, 1, 1) Then
            newBook.CreatedAt = DateTime.Now
        End If
        newBook.AvailableQuantity = newBook.Quantity

        newBook.Id = 0
        _uow.Books.Add(newBook)

        _uow.Save()
    End Sub

    Public Sub UpdateBook(bookDto As BookDto) Implements IBookService.UpdateBook

        Dim existingBook = _uow.Books.GetById(bookDto.Id)

        If existingBook Is Nothing Then
            Throw New Exception("Không tìm thấy sách cần sửa!")
        End If

        _mapper.Map(bookDto, existingBook)


        If existingBook.Price < 0 Or existingBook.Quantity < 0 Then
            Throw New Exception("Không nhận giá trị âm ! ")
        End If

        If existingBook.CreatedAt < New DateTime(1753, 1, 1) Then
            existingBook.CreatedAt = DateTime.Now
        End If

        existingBook.UpdatedAt = DateTime.Now
        _uow.Save()
    End Sub

    Public Sub DeleteBook(id As Integer) Implements IBookService.DeleteBook
        Dim book = _uow.Books.GetById(id)
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
End Class