Imports MyLibrary.Domain

Public Interface IUnitOfWork
    Inherits IDisposable

    ReadOnly Property Authors As IGenericRepository(Of Author)
    ReadOnly Property Books As IBookRepository
    ReadOnly Property Categories As IGenericRepository(Of Category)
    ReadOnly Property Publishers As IGenericRepository(Of Publisher)

    ReadOnly Property Users As IUserRepository
    ReadOnly Property Roles As IRoleRepository
    ReadOnly Property UserRoles As IUserRoleRepository

    ReadOnly Property BorrowTickets As IGenericRepository(Of BorrowTicket)
    ReadOnly Property BorrowDetails As IGenericRepository(Of BorrowDetail)

    ReadOnly Property Deposits As IGenericRepository(Of Deposit)
    ReadOnly Property Payments As IGenericRepository(Of Payment)

    Sub Save()
End Interface
