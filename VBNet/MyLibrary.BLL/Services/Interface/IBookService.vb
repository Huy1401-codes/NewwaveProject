Imports MyLibrary.Domain

Public Interface IBookService
    Function GetAllBooksForView() As List(Of BookDto)
    Function GetBooksCombined(keyword As String, publisherId As Integer?, year As Integer?, pageIndex As Integer, pageSize As Integer) As PagedResult(Of BookDto)
    Function GetAuthors() As List(Of Author)
    Function GetCategories() As List(Of Category)
    Function GetPublishers() As List(Of Publisher)

    Sub AddBook(bookDto As BookDto)
    Sub UpdateBook(bookDto As BookDto)
    Sub DeleteBook(id As Integer)
    Function GetBookById(id As Integer) As Book
    Function ExportBooksToExcel(books As List(Of BookDto)) As Byte()
    Sub ImportBooksFromExcelToDb(fileBytes As Byte())

End Interface