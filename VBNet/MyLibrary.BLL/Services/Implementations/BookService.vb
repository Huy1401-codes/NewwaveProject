Imports AutoMapper
Imports MyLibrary.DAL
Imports MyLibrary.Domain.MyApp.Domain.Common

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

    Public Function SearchBooks(keyword As String) As List(Of BookDto) Implements IBookService.SearchBooks
        Dim allBooks = GetAllBooksForView()
        If String.IsNullOrWhiteSpace(keyword) Then Return allBooks

        keyword = keyword.ToLower()
        Return allBooks.Where(Function(b) b.Title.ToLower().Contains(keyword) OrElse
                                          b.BookCode.ToLower().Contains(keyword)).ToList()
    End Function

    Public Sub AddBook(book As Book) Implements IBookService.AddBook
        book.AvailableQuantity = book.Quantity
        _uow.Books.Add(book)
        _uow.Save()
    End Sub

    Public Sub UpdateBook(book As Book) Implements IBookService.UpdateBook
        _uow.Books.Update(book)
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

End Class