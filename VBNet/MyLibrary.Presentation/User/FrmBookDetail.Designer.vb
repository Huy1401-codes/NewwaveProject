<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmBookDetail
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.picBook = New System.Windows.Forms.PictureBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblAuthor = New System.Windows.Forms.Label()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.lblQuantity = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.lblDueDateTitle = New System.Windows.Forms.Label()
        Me.dtpDueDate = New System.Windows.Forms.DateTimePicker()
        Me.btnBorrow = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        CType(Me.picBook, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picBook
        '
        Me.picBook.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picBook.Location = New System.Drawing.Point(27, 25)
        Me.picBook.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.picBook.Name = "picBook"
        Me.picBook.Size = New System.Drawing.Size(239, 307)
        Me.picBook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picBook.TabIndex = 0
        Me.picBook.TabStop = False
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Arial", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.Navy
        Me.lblTitle.Location = New System.Drawing.Point(293, 25)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitle.MaximumSize = New System.Drawing.Size(600, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(137, 32)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Tên Sách"
        '
        'lblAuthor
        '
        Me.lblAuthor.AutoSize = True
        Me.lblAuthor.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAuthor.Location = New System.Drawing.Point(296, 74)
        Me.lblAuthor.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAuthor.Name = "lblAuthor"
        Me.lblAuthor.Size = New System.Drawing.Size(72, 22)
        Me.lblAuthor.TabIndex = 2
        Me.lblAuthor.Text = "Tác giả"
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCategory.Location = New System.Drawing.Point(296, 111)
        Me.lblCategory.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(77, 22)
        Me.lblCategory.TabIndex = 3
        Me.lblCategory.Text = "Thể loại"
        '
        'lblQuantity
        '
        Me.lblQuantity.AutoSize = True
        Me.lblQuantity.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQuantity.ForeColor = System.Drawing.Color.Red
        Me.lblQuantity.Location = New System.Drawing.Point(296, 148)
        Me.lblQuantity.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblQuantity.Name = "lblQuantity"
        Me.lblQuantity.Size = New System.Drawing.Size(97, 22)
        Me.lblQuantity.TabIndex = 4
        Me.lblQuantity.Text = "Số lượng"
        '
        'txtDescription
        '
        Me.txtDescription.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.txtDescription.Location = New System.Drawing.Point(300, 185)
        Me.txtDescription.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(565, 147)
        Me.txtDescription.TabIndex = 5
        '
        'lblDueDateTitle
        '
        Me.lblDueDateTitle.AutoSize = True
        Me.lblDueDateTitle.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblDueDateTitle.Location = New System.Drawing.Point(27, 369)
        Me.lblDueDateTitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDueDateTitle.Name = "lblDueDateTitle"
        Me.lblDueDateTitle.Size = New System.Drawing.Size(146, 19)
        Me.lblDueDateTitle.TabIndex = 6
        Me.lblDueDateTitle.Text = "Ngày trả dự kiến:"
        '
        'dtpDueDate
        '
        Me.dtpDueDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpDueDate.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.dtpDueDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDueDate.Location = New System.Drawing.Point(213, 366)
        Me.dtpDueDate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dtpDueDate.Name = "dtpDueDate"
        Me.dtpDueDate.Size = New System.Drawing.Size(185, 27)
        Me.dtpDueDate.TabIndex = 7
        '
        'btnBorrow
        '
        Me.btnBorrow.BackColor = System.Drawing.Color.DodgerBlue
        Me.btnBorrow.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnBorrow.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBorrow.ForeColor = System.Drawing.Color.White
        Me.btnBorrow.Location = New System.Drawing.Point(432, 366)
        Me.btnBorrow.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnBorrow.Name = "btnBorrow"
        Me.btnBorrow.Size = New System.Drawing.Size(192, 30)
        Me.btnBorrow.TabIndex = 8
        Me.btnBorrow.Text = "MƯỢN SÁCH"
        Me.btnBorrow.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(688, 369)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(113, 27)
        Me.btnClose.TabIndex = 9
        Me.btnClose.Text = "Đóng"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'FrmBookDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(933, 443)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnBorrow)
        Me.Controls.Add(Me.dtpDueDate)
        Me.Controls.Add(Me.lblDueDateTitle)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.lblQuantity)
        Me.Controls.Add(Me.lblCategory)
        Me.Controls.Add(Me.lblAuthor)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.picBook)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmBookDetail"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Chi Tiết Sách"
        CType(Me.picBook, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    ' Các biến này sẽ được file code chính
    Friend WithEvents picBook As System.Windows.Forms.PictureBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblAuthor As System.Windows.Forms.Label
    Friend WithEvents lblCategory As System.Windows.Forms.Label
    Friend WithEvents lblQuantity As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents lblDueDateTitle As System.Windows.Forms.Label
    Friend WithEvents dtpDueDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnBorrow As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class