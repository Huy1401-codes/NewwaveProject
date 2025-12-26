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

    ' =========================
    ' LOGIN
    ' =========================
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
            Throw New Exception("Tài khoản đã bị khóa")
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

    ' =========================
    ' REGISTER
    ' =========================
    Public Sub Register(dto As RegisterRequestDto) _
        Implements IAuthService.Register

        If _uow.Users.ExistsByEmail(dto.Email) Then
            Throw New Exception("Email đã tồn tại")
        End If

        Dim user As New DAL.User With {
            .Email = dto.Email,
            .PasswordHash = HashPassword(dto.Password),
            .FullName = dto.FullName,
            .IsActive = True,
            .CreatedAt = DateTime.Now
        }

        _uow.Users.Add(user)
        _uow.Save()

        Dim defaultRole = _uow.Roles.GetByName("User")
        If defaultRole Is Nothing Then
            Throw New Exception("Role mặc định không tồn tại")
        End If

        _uow.UserRoles.Add(New UserRole With {
            .UserId = user.UserId,
            .RoleId = defaultRole.RoleId
        })

        _uow.Save()

        _emailService.SendEmail(
            user.Email,
            "Đăng ký thành công",
            $"<h3>Xin chào {user.FullName}</h3>
              <p>Tài khoản của bạn đã được tạo thành công.</p>"
        )
    End Sub

    Public Sub ResetPassword(email As String) _
        Implements IAuthService.ResetPassword

        Dim user = _uow.Users.GetByEmail(email)
        If user Is Nothing Then
            Throw New Exception("Email không tồn tại")
        End If

        Dim newPassword = GeneratePassword()

        user.PasswordHash = HashPassword(newPassword)
        _uow.Users.Update(user)
        _uow.Save()

        _emailService.SendEmail(
            user.Email,
            "Cấp lại mật khẩu",
            $"<p>Mật khẩu mới của bạn là: <b>{newPassword}</b></p>"
        )
    End Sub

    Private Function HashPassword(password As String) As String
        Using sha As SHA256 = SHA256.Create()
            Dim bytes = Encoding.UTF8.GetBytes(password)
            Dim hashBytes = sha.ComputeHash(bytes)
            Return Convert.ToBase64String(hashBytes)
        End Using
    End Function


    Private Function GeneratePassword() As String
        Return Guid.NewGuid().ToString("N").Substring(0, 8)
    End Function

End Class
