Public Interface IAuthService
    Function Login(email As String, password As String) As LoginResponseDto

    Sub Register(dto As RegisterRequestDto)

    Function EmailExists(email As String) As Boolean

    ' Xác thực tài khoản sau khi đăng ký
    Sub VerifyAccount(email As String, code As String)
    ' Gửi OTP quên mật khẩu
    Sub ForgotPassword(email As String)
    ' Đổi mật khẩu mới bằng OTP
    Sub CompletePasswordReset(email As String, otp As String, newPassword As String)
End Interface
