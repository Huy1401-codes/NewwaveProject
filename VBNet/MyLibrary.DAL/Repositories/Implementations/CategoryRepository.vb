Imports MyLibrary.Domain

Public Class CategoryRepository
    Inherits GenericRepository(Of Category)
    Implements ICategoryRepository
    Public Sub New(context As AppDbContext)
        MyBase.New(context)
    End Sub
    Public Function GetByName(name As String) As Category _
        Implements ICategoryRepository.GetByName

        Return _dbSet.FirstOrDefault(Function(c) _
            c.CategoryName = name AndAlso c.IsDeleted = False)
    End Function
End Class
