Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading.Tasks ' <-- Import Task
Imports MyLibrary.DAL
Imports MyLibrary.Domain
Imports NLog

Public Class AuthService
    Implements IAuthService

    Private ReadOnly _uow As IUnitOfWork
    Private ReadOnly _emailService As IEmailService
    Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

    Public Sub New(uow As IUnitOfWork, emailService As IEmailService)
        _uow = uow
        _emailService = emailService
    End Sub

    ' --- 1. LOGIN ASYNC ---
    Public Async Function LoginAsync(email As String, password As String) As Task(Of LoginResponseDto) _
        Implements IAuthService.LoginAsync

        If String.IsNullOrWhiteSpace(email) OrElse String.IsNullOrWhiteSpace(password) Then
            Throw New Exception(AuthMessages.EmailOrPasswordEmpty)
        End If

        ' Gọi Async
        Dim user = Await _uow.Users.GetByEmailAsync(email)

        If user Is Nothing Then
            logger.Warn("Sai email hoặc mật khẩu")
            Throw New Exception(AuthMessages.EmailOrPasswordErorr)
        End If

        If user.IsDeleted Then
            logger.Warn($"Login thất bại: Email {email} đã bị xóa.")
            Throw New Exception(AuthMessages.EmailOrPasswordErorr)
        End If

        Dim hash = HashPassword(password)
        If user.PasswordHash <> hash Then
            logger.Warn("Sai email hoặc mật khẩu")
            Throw New Exception(AuthMessages.EmailOrPasswordErorr)
        End If

        If Not user.IsActive Then
            logger.Warn("Tài khoản chưa được kích hoạt.")
            Throw New Exception(AuthMessages.AccountNotActive)
        End If

        ' Gọi Async lấy tên Role
        Dim roleName = Await _uow.UserRoles.GetRoleNameByUserIdAsync(user.Id)

        If String.IsNullOrEmpty(roleName) Then
            logger.Warn("Tài khoản chưa được phân quyền.")
            Throw New Exception(AuthMessages.AccountNotRole)
        End If

        logger.Info("Login thành công")

        Return New LoginResponseDto With {
            .UserId = user.Id,
            .Email = user.Email,
            .RoleName = roleName
        }
    End Function

    Public Async Function RegisterAsync(dto As RegisterRequestDto) As Task _
        Implements IAuthService.RegisterAsync

        If dto Is Nothing Then Throw New Exception(AuthMessages.InforError)
        If String.IsNullOrWhiteSpace(dto.FullName) Then Throw New Exception(AuthMessages.FullNameNotNull)
        If String.IsNullOrWhiteSpace(dto.Email) Then Throw New Exception(AuthMessages.EmailOrPasswordErorr)
        If String.IsNullOrWhiteSpace(dto.Password) Then Throw New Exception(AuthMessages.PasswordNotNull)

        Dim email As String = dto.Email.Trim().ToLower()


        Dim defaultRole = Await _uow.Roles.GetByNameAsync("Customer")

        If defaultRole Is Nothing Then
            defaultRole = New Role With {
                .RoleName = "Customer",
                .IsDeleted = False
            }
            logger.Info("Tạo role mới thành công")
            _uow.Roles.Add(defaultRole)
            Await _uow.SaveAsync()
        End If

        Dim existingUser = Await _uow.Users.GetByEmailAsync(email)
        If existingUser IsNot Nothing Then
            logger.Warn("Email tồn tại")
            Throw New Exception(AuthMessages.EmailExist)
        End If

        Dim otpCode As String = GenerateOTP()

        Dim user As New User With {
            .Email = email,
            .PasswordHash = HashPassword(dto.Password),
            .FullName = dto.FullName.Trim(),
            .IsActive = False,
            .VerificationCode = otpCode,
            .CodeExpiration = DateTime.Now.AddMinutes(3),
            .CreatedAt = DateTime.Now,
            .IsDeleted = False,
            .UpdatedAt = DateTime.Now
        }

        _uow.Users.Add(user)

        _uow.UserRoles.Add(New UserRole With {
            .User = user,
            .RoleId = defaultRole.Id
        })

        Try
            logger.Info("Tạo account mới thành công")
            Await _uow.SaveAsync()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then
                msg &= vbCrLf & "Inner: " & ex.InnerException.Message
            End If
            Throw New Exception("Lỗi lưu Database: " & msg)
        End Try

        Try

            Await Task.Run(Sub()
                               _emailService.SendEmail(
                                   user.Email,
                                   "Xác thực đăng ký tài khoản",
                                   $"<h3>Xin chào {user.FullName}</h3><p>Mã OTP: {otpCode}</p>"
                               )
                           End Sub)
            logger.Info("Gửi email thành công")
        Catch ex As Exception
            logger.Error(ex, "Lỗi gửi email")
        End Try
    End Function

    Public Async Function VerifyAccountAsync(email As String, code As String) As Task _
        Implements IAuthService.VerifyAccountAsync

        Dim user = Await _uow.Users.GetByEmailAsync(email.Trim().ToLower())

        If user Is Nothing Then
            logger.Warn("User không tồn tại")
            Throw New Exception(AuthMessages.AccountNotExist)
        End If

        If user.IsActive Then
            logger.Warn("Account đã được kích hoạt")
            Throw New Exception(AuthMessages.AccountIsActive)
        End If

        If user.VerificationCode <> code Then
            logger.Info("Code không đúng")
            Throw New Exception(AuthMessages.CodeError)
        End If

        If user.CodeExpiration.HasValue AndAlso user.CodeExpiration.Value < DateTime.Now Then
            logger.Info("Code hết hạn")
            Throw New Exception(AuthMessages.CodeExpired)
        End If

        user.IsActive = True
        user.VerificationCode = Nothing
        user.CodeExpiration = Nothing

        logger.Info("Kích hoạt thành công")

        _uow.Users.Update(user)
        Await _uow.SaveAsync()
    End Function

    Public Async Function ForgotPasswordAsync(email As String) As Task _
        Implements IAuthService.ForgotPasswordAsync

        If String.IsNullOrWhiteSpace(email) Then Return

        Dim emailTrim As String = email.Trim().ToLower()
        Dim user = Await _uow.Users.GetByEmailAsync(emailTrim)

        If user Is Nothing OrElse Not user.IsActive Then
            Return
        End If

        Dim otpCode As String = GenerateOTP()

        user.VerificationCode = otpCode
        user.CodeExpiration = DateTime.Now.AddMinutes(15)

        logger.Info("Tạo OTP quên mật khẩu thành công")
        _uow.Users.Update(user)
        Await _uow.SaveAsync()

        Try
            Await Task.Run(Sub()
                               _emailService.SendEmail(
                                  user.Email,
                                  "Mã xác thực Quên mật khẩu",
                                  $"<h3>Yêu cầu cấp lại mật khẩu</h3><p>OTP: {otpCode}</p>"
                              )
                           End Sub)
        Catch ex As Exception
            logger.Error(ex, "Lỗi gửi email ForgotPassword")
            Throw New Exception("Không gửi được email. Vui lòng thử lại.")
        End Try
    End Function

    Public Async Function CompletePasswordResetAsync(email As String, otp As String, newPassword As String) As Task _
        Implements IAuthService.CompletePasswordResetAsync

        Dim user = Await _uow.Users.GetByEmailAsync(email.Trim().ToLower())

        If user Is Nothing Then Throw New Exception(AuthMessages.AccountNotExist)

        If String.IsNullOrEmpty(user.VerificationCode) OrElse user.VerificationCode <> otp Then
            logger.Warn("Code lỗi")
            Throw New Exception(AuthMessages.CodeError)
        End If

        If user.CodeExpiration.HasValue AndAlso user.CodeExpiration.Value < DateTime.Now Then
            logger.Warn("Code hết hạn")
            Throw New Exception(AuthMessages.CodeExpired)
        End If

        user.PasswordHash = HashPassword(newPassword)
        user.VerificationCode = Nothing
        user.CodeExpiration = Nothing

        logger.Warn("Reset password thành công")
        _uow.Users.Update(user)
        Await _uow.SaveAsync()
    End Function

    Public Async Function EmailExistsAsync(email As String) As Task(Of Boolean) _
        Implements IAuthService.EmailExistsAsync

        If String.IsNullOrWhiteSpace(email) Then Return False

        Dim user = Await _uow.Users.GetByEmailAsync(email)
        Return (user IsNot Nothing)
    End Function

    Private Function HashPassword(password As String) As String
        Using sha As SHA256 = SHA256.Create()
            Dim bytes = Encoding.UTF8.GetBytes(password)
            Dim hashBytes = sha.ComputeHash(bytes)
            Return Convert.ToBase64String(hashBytes)
        End Using
    End Function

    Private Function GenerateOTP() As String
        Dim random As New Random()
        Return random.Next(100000, 999999).ToString()
    End Function

End Class