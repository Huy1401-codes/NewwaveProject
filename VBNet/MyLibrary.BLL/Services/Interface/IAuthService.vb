Public Interface IAuthService
    Function LoginAsync(email As String, password As String) As Task(Of LoginResponseDto)

    Function RegisterAsync(dto As RegisterRequestDto) As Task

    Function EmailExistsAsync(email As String) As Task(Of Boolean)

    Function VerifyAccountAsync(email As String, code As String) As Task

    Function ForgotPasswordAsync(email As String) As Task

    Function CompletePasswordResetAsync(email As String, otp As String, newPassword As String) As Task
End Interface