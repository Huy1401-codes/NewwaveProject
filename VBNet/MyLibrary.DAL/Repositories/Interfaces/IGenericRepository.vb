Imports System.Linq.Expressions

Public Interface IGenericRepository(Of T As Class)

    Function GetById(id As Integer) As T
    Function GetAll() As IQueryable(Of T)
    Function Find(predicate As Expression(Of Func(Of T, Boolean))) As IEnumerable(Of T)

    Sub Add(entity As T)
    Sub Update(entity As T)
    Sub SoftDelete(entity As T)

End Interface
