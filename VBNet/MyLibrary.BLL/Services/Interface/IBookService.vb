Imports MyLibrary.Domain

Public Interface IBookService
    Function GetAllBooksForViewAsync() As Task(Of List(Of BookDto))

    Function GetBooksCombinedAsync(keyword As String,
                                   publisherId As Integer?,
                                   year As Integer?,
                                   pageIndex As Integer,
                                   pageSize As Integer) As Task(Of PagedResult(Of BookDto))

    Function GetAvailableBooksAsync(keyword As String,
                                    publisherId As Integer?,
                                    categoryId As Integer?) As Task(Of List(Of BookDetailDto))

    Function GetBookDetailAsync(bookId As Integer) As Task(Of BookDetailDto)
    Function GetBookByIdAsync(id As Integer) As Task(Of Book)

    Function GetAuthorsAsync() As Task(Of List(Of Author))
    Function GetCategoriesAsync() As Task(Of List(Of Category))
    Function GetPublishersAsync() As Task(Of List(Of Publisher))

    Function AddBookAsync(bookDto As BookDto) As Task
    Function UpdateBookAsync(bookDto As BookDto) As Task
    Function DeleteBookAsync(id As Integer) As Task

    Function ExportBooksToExcel(books As List(Of BookDto)) As Byte()
    Function ImportBooksFromExcelToDbAsync(fileBytes As Byte()) As Task
End Interface