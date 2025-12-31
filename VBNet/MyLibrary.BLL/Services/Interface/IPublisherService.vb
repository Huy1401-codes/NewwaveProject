Imports MyLibrary.Domain

Public Interface IPublisherService
    Function GetPaged(keyword As String,
                   pageIndex As Integer,
                   pageSize As Integer) As PagedResult(Of Publisher)
    Function GetById(id As Integer) As PublisherDto
    Sub Add(dto As PublisherDto)
    Sub Update(id As Integer, dto As PublisherDto)
    Sub Delete(id As Integer)
    Function GetDetail(publisherId As Integer,
                              bookKeyword As String,
                              pageIndex As Integer,
                              pageSize As Integer, categoryId As Integer?) As PublisherDetailDto
End Interface
