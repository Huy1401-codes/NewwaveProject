Imports MyLibrary.Domain

Public Interface ICategoryService
    Function GetPagedAsync(keyword As String,
                    pageIndex As Integer,
                    pageSize As Integer) As Task(Of PagedResult(Of Category))
    Function GetByIdAsync(id As Integer) As Task(Of CategoryDto)

    Function AddAsync(dto As CategoryDto) As Task

    Function UpdateAsync(id As Integer, dto As CategoryDto) As Task
    Function DeleteAsync(id As Integer) As Task
    Function GetDetailAsync(categoryId As Integer,
                            bookKeyword As String,
                            pageIndex As Integer,
                            pageSize As Integer,
                            publisherId As Integer?) As Task(Of CategoryDetailDto)

End Interface
