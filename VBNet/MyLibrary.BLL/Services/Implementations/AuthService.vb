Imports System.Security.Cryptography
Imports System.Text
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


    Public Function Login(email As String, password As String) As LoginResponseDto _
        Implements IAuthService.Login

        If String.IsNullOrWhiteSpace(email) OrElse String.IsNullOrWhiteSpace(password) Then
            Throw New Exception(AuthMessages.EmailOrPasswordEmpty)
        End If

        Dim user = _uow.Users.GetByEmail(email)
        If user Is Nothing Then
            logger.Warn("Sai email hoặc mật khẩu")
            Throw New Exception(AuthMessages.EmailOrPasswordErorr)

        End If

        If user Is Nothing OrElse user.IsDeleted Then
            logger.Warn($"Login thất bại: Email {email} không tồn tại hoặc đã bị xóa.")
            Throw New Exception(AuthMessages.EmailOrPasswordErorr)
        End If

        Dim hash = HashPassword(password)
        If user.PasswordHash <> hash Then
            logger.Warn("Sai email hoặc mật khẩu")
            Throw New Exception(AuthMessages.EmailOrPasswordErorr)
        End If

        If Not user.IsActive Then
            logger.Warn("Tài khoản chưa được kích hoạt. Vui lòng kiểm tra email để lấy mã xác thực.")
            Throw New Exception(AuthMessages.AccountNotActive)
        End If

        Dim roleName = _uow.UserRoles.GetRoleNameByUserId(user.Id)
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

    Public Sub Register(dto As RegisterRequestDto) Implements IAuthService.Register
        If dto Is Nothing Then Throw New Exception(AuthMessages.InforError)
        If String.IsNullOrWhiteSpace(dto.FullName) Then Throw New Exception(AuthMessages.FullNameNotNull)
        If String.IsNullOrWhiteSpace(dto.Email) Then Throw New Exception(AuthMessages.EmailOrPasswordErorr)
        If String.IsNullOrWhiteSpace(dto.Password) Then Throw New Exception(AuthMessages.PasswordNotNull)

        Dim email As String = dto.Email.Trim().ToLower()

        Dim defaultRole = _uow.Roles.GetByName("Customer")

        If defaultRole Is Nothing Then
            defaultRole = New Role With {
            .RoleName = "Customer",
            .IsDeleted = False
        }
            logger.Info("Tạo role mới thành công")
            _uow.Roles.Add(defaultRole)
            _uow.Save()
        End If

        If _uow.Users.ExistsByEmail(email) Then
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
            _uow.Save()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then
                msg &= vbCrLf & "Inner: " & ex.InnerException.Message
                If ex.InnerException.InnerException IsNot Nothing Then
                    msg &= vbCrLf & "SQL Error: " & ex.InnerException.InnerException.Message
                End If
            End If
            Throw New Exception("Lỗi lưu Database: " & msg)
        End Try

        Try
            _emailService.SendEmail(
            user.Email,
            "Xác thực đăng ký tài khoản",
            $"<h3>Xin chào {user.FullName}</h3><p>Mã OTP: {otpCode}</p>"
        )
            logger.Info("Gửi email thành công")
        Catch ex As Exception

        End Try

    End Sub



    Public Sub VerifyAccount(email As String, code As String) _
    Implements IAuthService.VerifyAccount

        Dim user = _uow.Users.GetByEmail(email.Trim().ToLower())

        If user Is Nothing Then
            logger.Warn("Email tồn tại")
            Throw New Exception(AuthMessages.EmailExist)
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

        logger.Info("Tạo mới thành công")
        _uow.Users.Update(user)
        _uow.Save()
    End Sub

    Public Sub ForgotPassword(email As String) _
    Implements IAuthService.ForgotPassword

        If String.IsNullOrWhiteSpace(email) Then Return

        Dim emailTrim As String = email.Trim().ToLower()
        Dim user = _uow.Users.GetByEmail(emailTrim)


        If user Is Nothing OrElse Not user.IsActive Then
            Return
        End If

        Dim otpCode As String = GenerateOTP()

        user.VerificationCode = otpCode
        user.CodeExpiration = DateTime.Now.AddMinutes(15)

        logger.Info("Thành công")
        _uow.Users.Update(user)
        _uow.Save()
        Try
            _emailService.SendEmail(
            user.Email,
            "Mã xác thực Quên mật khẩu",
            $"<h3>Yêu cầu cấp lại mật khẩu</h3>
              <p>Mã xác thực (OTP) của bạn là:</p>
              <h2 style='color:red;'>{otpCode}</h2>
              <p>Vui lòng nhập mã này vào phần mềm để đặt lại mật khẩu mới.</p>"
        )
        Catch ex As Exception
            logger.Error(ex, "Lỗi gửi email ForgotPassword")
        Throw New Exception("Không gửi được email. Vui lòng thử lại.")
        End Try
    End Sub

    Public Sub CompletePasswordReset(email As String, otp As String, newPassword As String) _
    Implements IAuthService.CompletePasswordReset

        Dim user = _uow.Users.GetByEmail(email.Trim().ToLower())
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
        _uow.Save()
    End Sub


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

    Public Function EmailExists(email As String) As Boolean _
    Implements IAuthService.EmailExists
        If String.IsNullOrWhiteSpace(email) Then Return False
        Return _uow.Users.ExistsByEmail(email)
    End Function

End Class