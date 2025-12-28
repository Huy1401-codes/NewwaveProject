Imports System.Security.Cryptography
Imports System.Text
Imports MyLibrary.DAL
Imports MyLibrary.Domain

Public Class AuthService
    Implements IAuthService

    Private ReadOnly _uow As IUnitOfWork
    Private ReadOnly _emailService As IEmailService

    Public Sub New(uow As IUnitOfWork, emailService As IEmailService)
        _uow = uow
        _emailService = emailService
    End Sub


    Public Function Login(email As String, password As String) As LoginResponseDto _
        Implements IAuthService.Login

        If String.IsNullOrWhiteSpace(email) OrElse String.IsNullOrWhiteSpace(password) Then
            Throw New Exception("Email và mật khẩu không được để trống")
        End If

        Dim user = _uow.Users.GetByEmail(email)
        If user Is Nothing Then
            Throw New Exception("Sai email hoặc mật khẩu")
        End If

        Dim hash = HashPassword(password)
        If user.PasswordHash <> hash Then
            Throw New Exception("Sai email hoặc mật khẩu")
        End If

        If Not user.IsActive Then
            Throw New Exception("Tài khoản chưa được kích hoạt. Vui lòng kiểm tra email để lấy mã xác thực.")
        End If

        Dim roleName = _uow.UserRoles.GetRoleNameByUserId(user.UserId)
        If String.IsNullOrEmpty(roleName) Then
            Throw New Exception("Tài khoản chưa được phân quyền")
        End If

        Return New LoginResponseDto With {
            .UserId = user.UserId,
            .Email = user.Email,
            .RoleName = roleName
        }
    End Function

    Public Sub Register(dto As RegisterRequestDto) _
    Implements IAuthService.Register

        If dto Is Nothing Then Throw New Exception("Dữ liệu không hợp lệ")
        If String.IsNullOrWhiteSpace(dto.FullName) Then Throw New Exception("Họ tên không được để trống")
        If String.IsNullOrWhiteSpace(dto.Email) Then Throw New Exception("Email không được để trống")
        If String.IsNullOrWhiteSpace(dto.Password) Then Throw New Exception("Mật khẩu không được để trống")

        Dim email As String = dto.Email.Trim().ToLower()

        If _uow.Users.ExistsByEmail(email) Then
            Throw New Exception("Email đã tồn tại")
        End If


        Dim otpCode As String = GenerateOTP()

        Dim user As New DAL.User With {
            .Email = email,
            .PasswordHash = HashPassword(dto.Password),
            .FullName = dto.FullName.Trim(),
            .IsActive = False,
            .VerificationCode = otpCode,
            .CodeExpiration = DateTime.Now.AddMinutes(1),
            .CreatedAt = DateTime.Now
        }

        _uow.Users.Add(user)
        _uow.Save()

        Dim defaultRole = _uow.Roles.GetByName("User")
        If defaultRole Is Nothing Then Throw New Exception("Role mặc định không tồn tại")

        _uow.UserRoles.Add(New UserRole With {
            .UserId = user.UserId,
            .RoleId = defaultRole.RoleId
        })
        _uow.Save()

        _emailService.SendEmail(
            user.Email,
            "Xác thực đăng ký tài khoản",
            $"<h3>Xin chào {user.FullName}</h3>
              <p>Cảm ơn bạn đã đăng ký. Mã xác thực của bạn là:</p>
              <h2 style='color:red;'>{otpCode}</h2>
              <p>Mã này có hiệu lực trong 15 phút.</p>"
        )
    End Sub

    Public Sub VerifyAccount(email As String, code As String) _
    Implements IAuthService.VerifyAccount

        Dim user = _uow.Users.GetByEmail(email.Trim().ToLower())

        If user Is Nothing Then
            Throw New Exception("Email không tồn tại")
        End If

        If user.IsActive Then
            Throw New Exception("Tài khoản này đã được kích hoạt rồi.")
        End If

        If user.VerificationCode <> code Then
            Throw New Exception("Mã xác thực không đúng.")
        End If

        If user.CodeExpiration.HasValue AndAlso user.CodeExpiration.Value < DateTime.Now Then
            Throw New Exception("Mã xác thực đã hết hạn. Vui lòng đăng ký lại.")
        End If

        user.IsActive = True
        user.VerificationCode = Nothing
        user.CodeExpiration = Nothing

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

        _uow.Users.Update(user)
        _uow.Save()

        _emailService.SendEmail(
            user.Email,
            "Mã xác thực Quên mật khẩu",
            $"<h3>Yêu cầu cấp lại mật khẩu</h3>
              <p>Mã xác thực (OTP) của bạn là:</p>
              <h2 style='color:red;'>{otpCode}</h2>
              <p>Vui lòng nhập mã này vào phần mềm để đặt lại mật khẩu mới.</p>"
        )
    End Sub

    Public Sub CompletePasswordReset(email As String, otp As String, newPassword As String) _
    Implements IAuthService.CompletePasswordReset

        Dim user = _uow.Users.GetByEmail(email.Trim().ToLower())
        If user Is Nothing Then Throw New Exception("User không tồn tại")


        If String.IsNullOrEmpty(user.VerificationCode) OrElse user.VerificationCode <> otp Then
            Throw New Exception("Mã xác thực sai hoặc không hợp lệ")
        End If

        If user.CodeExpiration.HasValue AndAlso user.CodeExpiration.Value < DateTime.Now Then
            Throw New Exception("Mã xác thực đã hết hạn")
        End If

        user.PasswordHash = HashPassword(newPassword)

        user.VerificationCode = Nothing
        user.CodeExpiration = Nothing

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