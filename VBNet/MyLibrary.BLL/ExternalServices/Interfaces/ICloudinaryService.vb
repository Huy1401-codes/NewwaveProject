Public Interface ICloudinaryService
    Function UploadImageAsync(filePath As String,
                                  Optional folder As String = "uploads") _
                                  As Task(Of String)

    Function DeleteImageAsync(imageUrl As String) _
                                  As Task(Of Boolean)
End Interface