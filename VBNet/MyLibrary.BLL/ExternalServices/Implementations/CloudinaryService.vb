Imports CloudinaryDotNet
Imports CloudinaryDotNet.Actions
Imports System.Configuration

Namespace BusinessAccessLayer.Services
    Public Class CloudinaryService
        Implements ICloudinaryService

        Private ReadOnly _cloudinary As Cloudinary

        Public Sub New()
            Dim cloudName = ConfigurationManager.AppSettings("Cloudinary:CloudName")
            Dim apiKey = ConfigurationManager.AppSettings("Cloudinary:ApiKey")
            Dim apiSecret = ConfigurationManager.AppSettings("Cloudinary:ApiSecret")

            Dim account As New Account(cloudName, apiKey, apiSecret)
            _cloudinary = New Cloudinary(account)
        End Sub

        Public Async Function UploadImageAsync(
            filePath As String,
            Optional folder As String = "uploads"
        ) As Task(Of String) _
        Implements ICloudinaryService.UploadImageAsync

            If String.IsNullOrEmpty(filePath) OrElse Not IO.File.Exists(filePath) Then
                Return Nothing
            End If

            Dim uploadParams As New ImageUploadParams With {
                .File = New FileDescription(filePath),
                .Folder = folder,
                .Transformation = New Transformation().
                    Width(1200).
                    Height(630).
                    Crop("limit").
                    Quality("auto")
            }

            Dim result = Await _cloudinary.UploadAsync(uploadParams)
            Return If(result?.SecureUrl?.ToString(), Nothing)
        End Function

        Public Async Function DeleteImageAsync(
            imageUrl As String
        ) As Task(Of Boolean) _
        Implements ICloudinaryService.DeleteImageAsync

            If String.IsNullOrEmpty(imageUrl) Then Return False

            Try
                Dim uri As New Uri(imageUrl)
                Dim segments = uri.Segments
                Dim publicIdWithExt = segments(segments.Length - 2) & segments.Last()
                Dim publicId = publicIdWithExt _
                    .Replace(".jpg", "") _
                    .Replace(".png", "") _
                    .Replace(".jpeg", "") _
                    .Replace("/", "")

                Dim delParams As New DeletionParams(publicId)
                Dim result = Await _cloudinary.DestroyAsync(delParams)

                Return result.Result = "ok"
            Catch ex As Exception
                Return False
            End Try
        End Function

    End Class
End Namespace
