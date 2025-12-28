Imports MyLibrary.DAL

Public Interface IBookService
    Function GetAllBooksForView() As List(Of BookDto)
    Function SearchBooks(keyword As String) As List(Of BookDto)

    Function GetAuthors() As List(Of Author)
    Function GetCategories() As List(Of Category)
    Function GetPublishers() As List(Of Publisher)

    Sub AddBook(book As Book)
    Sub UpdateBook(book As Book)
    Sub DeleteBook(id As Integer)
    Function GetBookById(id As Integer) As Book
End Interface