Imports System.Linq.Expressions

Public Interface IGenericRepository(Of T As Class)

    Function GetByIdAsync(id As Integer) As Task(Of T)
    Function GetAll() As IQueryable(Of T)
    Function FindAsync(predicate As Expression(Of Func(Of T, Boolean))) As Task(Of IEnumerable(Of T))
    Sub Add(entity As T)
    Sub Update(entity As T)
    Sub SoftDelete(entity As T)

End Interface
