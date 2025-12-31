Imports MyLibrary.Domain

Public Interface IAuthorService
    Function GetPaged(keyword As String,
                      pageIndex As Integer,
                      pageSize As Integer) As PagedResult(Of Author)
    Function GetById(id As Integer) As AuthorDto
    Sub Add(dto As AuthorDto)
    Sub Update(id As Integer, dto As AuthorDto)
    Sub Delete(id As Integer)
    Function GetDetail(authorId As Integer,
                              bookKeyword As String,
                              pageIndex As Integer,
                              pageSize As Integer, publisherId As Integer?) As AuthorDetailDto
End Interface