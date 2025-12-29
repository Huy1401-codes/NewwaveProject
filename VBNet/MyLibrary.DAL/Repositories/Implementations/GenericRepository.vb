Imports System.Data.Entity
Imports System.Linq.Expressions
Imports System.Reflection

Public Class GenericRepository(Of T As Class)
    Implements IGenericRepository(Of T)

    Protected ReadOnly _context As AppDbContext
    Protected ReadOnly _dbSet As DbSet(Of T)

    Public Sub New(context As AppDbContext)
        _context = context
        _dbSet = context.Set(Of T)()
    End Sub

    Public Function GetById(id As Integer) As T _
        Implements IGenericRepository(Of T).GetById

        Return _dbSet.Find(id)
    End Function

    Public Function GetAll() As IQueryable(Of T) Implements IGenericRepository(Of T).GetAll
        Dim propInfo = GetType(T).GetProperty("IsDeleted")

        If propInfo IsNot Nothing AndAlso propInfo.PropertyType Is GetType(Boolean) Then

            Dim parameter = Expression.Parameter(GetType(T), "x")
            Dim propertyAccess = Expression.MakeMemberAccess(parameter, propInfo)

            Dim constant = Expression.Constant(False)
            Dim equality = Expression.Equal(propertyAccess, constant)
            Dim lambda = Expression.Lambda(Of Func(Of T, Boolean))(equality, parameter)
            Return _dbSet.Where(lambda)
        End If

        Return _dbSet
    End Function

    Public Function Find(predicate As Expression(Of Func(Of T, Boolean))) _
        As IEnumerable(Of T) Implements IGenericRepository(Of T).Find

        Return _dbSet.Where(predicate).ToList()
    End Function

    Public Sub Add(entity As T) _
        Implements IGenericRepository(Of T).Add

        _dbSet.Add(entity)
    End Sub

    Public Sub Update(entity As T) _
        Implements IGenericRepository(Of T).Update

        _context.Entry(entity).State = EntityState.Modified
    End Sub

    Public Sub SoftDelete(entity As T) _
        Implements IGenericRepository(Of T).SoftDelete

        Dim prop = GetType(T).GetProperty("IsDeleted", BindingFlags.Public Or BindingFlags.Instance)

        If prop Is Nothing Then
            Throw New Exception("Entity không có IsDeleted")
        End If

        prop.SetValue(entity, True)
        _context.Entry(entity).State = EntityState.Modified
    End Sub

End Class
