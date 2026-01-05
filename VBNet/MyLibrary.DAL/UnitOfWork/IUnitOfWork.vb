Imports MyLibrary.Domain

Public Interface IUnitOfWork
    Inherits IDisposable

    ReadOnly Property Context As AppDbContext
    ReadOnly Property Authors As IAuthorRepository
    ReadOnly Property Books As IBookRepository
    ReadOnly Property Categories As ICategoryRepository
    ReadOnly Property Publishers As IPublisherRepository

    ReadOnly Property Users As IUserRepository
    ReadOnly Property Roles As IRoleRepository
    ReadOnly Property UserRoles As IUserRoleRepository

    ReadOnly Property BorrowTickets As IBorrowTicketRepository
    ReadOnly Property BorrowDetails As IGenericRepository(Of BorrowDetail)

    ReadOnly Property Deposits As IGenericRepository(Of Deposit)
    ReadOnly Property Payments As IGenericRepository(Of Payment)

    Function SaveAsync() As Task
End Interface
