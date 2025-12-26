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

        Dim smtp As New SmtpClient("smtp.gmail.com") With {
            .Port = 587,
            .Credentials = New NetworkCredential(_fromEmail, _appPassword),
            .EnableSsl = True
        }

        Dim mail As New MailMessage() With {
            .From = New MailAddress(_fromEmail, "My App"),
            .Subject = subject,
            .Body = body,
            .IsBodyHtml = True
        }

        mail.To.Add(toEmail)

        smtp.Send(mail)
    End Sub
End Class
