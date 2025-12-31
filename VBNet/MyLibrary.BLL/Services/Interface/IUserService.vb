Public Interface IUserService
    Function GetPaged(keyword As String, status As Boolean?,
                      pageIndex As Integer, pageSize As Integer) As PagedResult(Of UserDto)

    Sub ToggleStatus(userId As Integer)
End Interface