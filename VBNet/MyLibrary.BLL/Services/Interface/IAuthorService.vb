Imports MyLibrary.Domain

Public Interface IAuthorService

    Function GetPagedAsync(keyword As String,
                           pageIndex As Integer,
                           pageSize As Integer) As Task(Of PagedResult(Of Author))

    Function GetByIdAsync(id As Integer) As Task(Of AuthorDto)

    Function AddAsync(dto As AuthorDto) As Task

    Function UpdateAsync(id As Integer, dto As AuthorDto) As Task

    Function DeleteAsync(id As Integer) As Task

    Function GetDetailAsync(authorId As Integer,
                            bookKeyword As String,
                            pageIndex As Integer,
                            pageSize As Integer,
                            publisherId As Integer?) As Task(Of AuthorDetailDto)
End Interface