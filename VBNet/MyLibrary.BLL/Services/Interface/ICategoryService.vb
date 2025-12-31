Imports MyLibrary.Domain

Public Interface ICategoryService
    Function GetPaged(keyword As String,
                    pageIndex As Integer,
                    pageSize As Integer) As PagedResult(Of Category)
    Function GetById(id As Integer) As CategoryDto
    Sub Add(dto As CategoryDto)
    Sub Update(id As Integer, dto As CategoryDto)
    Sub Delete(id As Integer)
    Function GetDetail(categoryId As Integer,
                              bookKeyword As String,
                              pageIndex As Integer,
                              pageSize As Integer, publisherId As Integer?) As CategoryDetailDto
End Interface
