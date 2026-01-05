Public Interface IUserService
    Function GetPagedAsync(keyword As String, status As Boolean?,
                      pageIndex As Integer, pageSize As Integer) As Task(Of PagedResult(Of UserDto))

    Function ToggleStatusAsync(userId As Integer) As Task
End Interface