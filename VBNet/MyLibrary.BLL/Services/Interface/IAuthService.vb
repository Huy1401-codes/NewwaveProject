Public Interface IAuthService
    Function Login(email As String, password As String) As LoginResponseDto

    Sub Register(dto As RegisterRequestDto)

    Sub ResetPassword(email As String)
End Interface
