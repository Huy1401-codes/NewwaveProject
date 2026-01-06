Imports MyLibrary.Domain

Public Interface IPublisherService
    Function GetPagedAsync(keyword As String,
                   pageIndex As Integer,
                   pageSize As Integer) As Task(Of PagedResult(Of Publisher))
    Function GetByIdAsync(id As Integer) As Task(Of PublisherDto)
    Function AddAsync(dto As PublisherDto) As Task
    Function UpdateAsync(id As Integer, dto As PublisherDto) As Task
    Function DeleteAsync(id As Integer) As Task
    Function GetDetailAsync(publisherId As Integer,
                              bookKeyword As String,
                              pageIndex As Integer,
                              pageSize As Integer, categoryId As Integer?) As Task(Of PublisherDetailDto)

End Interface
