Imports MyLibrary.Domain

Public Class UnitOfWork
    Implements IUnitOfWork

    Private ReadOnly _context As AppDbContext

    Public Sub New()
        _context = New AppDbContext()
    End Sub

    Public ReadOnly Property Context As AppDbContext _
        Implements IUnitOfWork.Context
        Get
            Return _context
        End Get
    End Property

    Private _authors As IAuthorRepository
    Public ReadOnly Property Authors As IAuthorRepository _
        Implements IUnitOfWork.Authors
        Get
            If _authors Is Nothing Then
                _authors = New AuthorRepository(_context)
            End If
            Return _authors
        End Get
    End Property

    Private _books As IBookRepository
    Public ReadOnly Property Books As IBookRepository _
        Implements IUnitOfWork.Books
        Get
            If _books Is Nothing Then
                _books = New BookRepository(_context)
            End If
            Return _books
        End Get
    End Property

    Private _categories As ICategoryRepository
    Public ReadOnly Property Categories As ICategoryRepository _
        Implements IUnitOfWork.Categories
        Get
            If _categories Is Nothing Then
                _categories = New CategoryRepository(_context)
            End If
            Return _categories
        End Get
    End Property

    Private _publishers As IPublisherRepository
    Public ReadOnly Property Publishers As IPublisherRepository _
        Implements IUnitOfWork.Publishers
        Get
            If _publishers Is Nothing Then
                _publishers = New PublisherRepository(_context)
            End If
            Return _publishers
        End Get
    End Property

    Private _users As IUserRepository
    Public ReadOnly Property Users As IUserRepository _
        Implements IUnitOfWork.Users
        Get
            If _users Is Nothing Then
                _users = New UserRepository(_context)
            End If
            Return _users
        End Get
    End Property

    Private _roles As IRoleRepository
    Public ReadOnly Property Roles As IRoleRepository _
        Implements IUnitOfWork.Roles
        Get
            If _roles Is Nothing Then
                _roles = New RoleRepository(_context)
            End If
            Return _roles
        End Get
    End Property

    Private _userRoles As IUserRoleRepository
    Public ReadOnly Property UserRoles As IUserRoleRepository _
        Implements IUnitOfWork.UserRoles
        Get
            If _userRoles Is Nothing Then
                _userRoles = New UserRoleRepository(_context)
            End If
            Return _userRoles
        End Get
    End Property

    Private _borrowTickets As IBorrowTicketRepository
    Public ReadOnly Property BorrowTickets As IBorrowTicketRepository _
        Implements IUnitOfWork.BorrowTickets
        Get
            If _borrowTickets Is Nothing Then
                _borrowTickets = New BorrowTicketRepository(_context)
            End If
            Return _borrowTickets
        End Get
    End Property

    Private _borrowDetails As IGenericRepository(Of BorrowDetail)
    Public ReadOnly Property BorrowDetails As IGenericRepository(Of BorrowDetail) _
        Implements IUnitOfWork.BorrowDetails
        Get
            If _borrowDetails Is Nothing Then
                _borrowDetails = New GenericRepository(Of BorrowDetail)(_context)
            End If
            Return _borrowDetails
        End Get
    End Property

    Private _deposits As IGenericRepository(Of Deposit)
    Public ReadOnly Property Deposits As IGenericRepository(Of Deposit) _
        Implements IUnitOfWork.Deposits
        Get
            If _deposits Is Nothing Then
                _deposits = New GenericRepository(Of Deposit)(_context)
            End If
            Return _deposits
        End Get
    End Property

    Private _payments As IGenericRepository(Of Payment)
    Public ReadOnly Property Payments As IGenericRepository(Of Payment) _
        Implements IUnitOfWork.Payments
        Get
            If _payments Is Nothing Then
                _payments = New GenericRepository(Of Payment)(_context)
            End If
            Return _payments
        End Get
    End Property

    Public Sub Save() Implements IUnitOfWork.Save
        _context.SaveChanges()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        _context.Dispose()
    End Sub

End Class
