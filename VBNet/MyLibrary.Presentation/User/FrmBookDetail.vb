Imports AutoMapper
Imports MyLibrary.BLL
Imports MyLibrary.DAL

Public Class FrmBookDetail
    Private _bookId As Integer
    Private _bookService As BookService
    Private _borrowService As BorrowService
    Private _uow As UnitOfWork
    Public Sub New(bookId As Integer)
        InitializeComponent()
        _bookId = bookId


        Dim config As New MapperConfiguration(Sub(cfg)
                                                  cfg.AddProfile(New BookMappingProfile())
                                              End Sub)
        Dim mapper As IMapper = config.CreateMapper()
        _uow = New UnitOfWork()
        _bookService = New BookService(_uow, mapper)
        _borrowService = New BorrowService(_uow)
    End Sub

    Private Sub FrmBookDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBookInfo()

        dtpDueDate.Value = DateTime.Now.AddDays(7)
        dtpDueDate.MinDate = DateTime.Now.AddDays(1)
        dtpDueDate.MaxDate = DateTime.Now.AddYears(1)
    End Sub

    Private Sub LoadBookInfo()
        Dim book = _bookService.GetBookDetail(_bookId)
        If book Is Nothing Then
            MessageBox.Show("Sách không tồn tại hoặc đã bị xóa.")
            Me.Close()
            Return
        End If

        lblTitle.Text = book.Title
        lblAuthor.Text = "Tác giả: " & book.AuthorName
        lblCategory.Text = "Thể loại: " & book.CategoryName
        txtDescription.Text = book.Description

        lblQuantity.Text = $"Còn lại: {book.AvailableQuantity} / {book.TotalQuantity}"

        If book.AvailableQuantity <= 0 Then
            btnBorrow.Enabled = False
            btnBorrow.Text = "Hết Hàng"
            btnBorrow.BackColor = Color.Gray
        End If

        Try
            Dim wc As New System.Net.WebClient()
            Dim bytes() As Byte = wc.DownloadData(book.ImagePath)
            Using ms As New System.IO.MemoryStream(bytes)
                picBook.Image = Image.FromStream(ms)
            End Using
        Catch ex As Exception
            picBook.Image = Nothing
        End Try

    End Sub

    Private Sub btnBorrow_Click(sender As Object, e As EventArgs) Handles btnBorrow.Click
        Try
            If dtpDueDate.Value <= DateTime.Now Then
                MessageBox.Show("Ngày trả phải lớn hơn ngày hiện tại.")
                Return
            End If

            If SessionManager.CurrentUser Is Nothing Then
                MessageBox.Show("Phiên đăng nhập đã hết hạn hoặc bạn chưa đăng nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            ' 2. Xác nhận lại người dùng
            Dim result = MessageBox.Show($"Bạn có chắc muốn mượn sách '{lblTitle.Text}'?" & vbCrLf &
                                         $"Ngày trả dự kiến: {dtpDueDate.Value:dd/MM/yyyy}",
                                         "Xác nhận mượn", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then

                Dim borrowItems As New List(Of BorrowItemDto) From {
                    New BorrowItemDto With {
                        .BookId = _bookId,
                        .Quantity = 1
                    }
                }

                _borrowService.RequestBorrow(
                    SessionManager.CurrentUser.UserId,
                    borrowItems,
                    dtpDueDate.Value
                )

                MessageBox.Show(
                    "Gửi yêu cầu mượn thành công! Vui lòng chờ Admin duyệt.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                )

                Me.Close()
            End If


        Catch ex As Exception
            MessageBox.Show("Lỗi: " & ex.Message, "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class