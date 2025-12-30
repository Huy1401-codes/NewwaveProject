Imports MyLibrary.Domain

Public Class PublisherRepository
    Inherits GenericRepository(Of Publisher)
    Implements IPublisherRepository

    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub

    Public Function GetByName(name As String) As Publisher _
        Implements IPublisherRepository.GetByName

        Return _dbSet.FirstOrDefault(Function(p) _
            p.PublisherName = name AndAlso p.IsDeleted = False)
    End Function
End Class
