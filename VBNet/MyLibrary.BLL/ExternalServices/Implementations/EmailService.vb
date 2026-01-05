Imports System.Net
Imports System.Net.Mail

Public Class EmailService
    Implements IEmailService

    Private ReadOnly _fromEmail As String
    Private ReadOnly _appPassword As String

    Public Sub New(fromEmail As String, appPassword As String)
        _fromEmail = fromEmail
        _appPassword = appPassword
    End Sub

    Public Sub SendEmail(toEmail As String, subject As String, body As String) _
      Implements IEmailService.SendEmail

        If String.IsNullOrWhiteSpace(toEmail) Then
            Throw New Exception("Email người nhận rỗng")
        End If

        Dim smtp As New SmtpClient() With {
            .Host = "smtp.gmail.com",
            .Port = 587,
            .EnableSsl = True,
            .UseDefaultCredentials = False,
            .Credentials = New NetworkCredential(_fromEmail, _appPassword),
            .DeliveryMethod = SmtpDeliveryMethod.Network
        }

        Dim mail As New MailMessage()
        mail.From = New MailAddress(_fromEmail, "My App")
        mail.To.Add(toEmail.Trim())
        mail.Subject = subject
        mail.Body = body
        mail.IsBodyHtml = True

        smtp.Send(mail)
    End Sub

End Class
