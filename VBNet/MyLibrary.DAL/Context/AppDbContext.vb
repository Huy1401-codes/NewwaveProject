Imports System.Data.Entity
Imports MyLibrary.Domain

Public Class AppDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=LibraryManagementDB")
    End Sub

    Public Property Authors As DbSet(Of Author)
    Public Property Books As DbSet(Of Book)
    Public Property Categories As DbSet(Of Category)
    Public Property Publishers As DbSet(Of Publisher)

    Public Property Users As DbSet(Of User)
    Public Property Roles As DbSet(Of Role)
    Public Property UserRoles As DbSet(Of UserRole)

    Public Property BorrowTickets As DbSet(Of BorrowTicket)
    Public Property BorrowDetails As DbSet(Of BorrowDetail)

    Public Property Deposits As DbSet(Of Deposit)
    Public Property Payments As DbSet(Of Payment)

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)

        ' Composite key
        modelBuilder.Entity(Of UserRole)().
            HasKey(Function(x) New With {x.UserId, x.RoleId})

        MyBase.OnModelCreating(modelBuilder)
    End Sub

End Class
