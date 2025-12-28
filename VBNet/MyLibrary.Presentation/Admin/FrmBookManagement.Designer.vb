<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmBookManagement
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.grpInfo = New System.Windows.Forms.GroupBox()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.lblPrice = New System.Windows.Forms.Label()
        Me.txtPrice = New System.Windows.Forms.TextBox()
        Me.lblQuantity = New System.Windows.Forms.Label()
        Me.txtQuantity = New System.Windows.Forms.TextBox()
        Me.lblAuthor = New System.Windows.Forms.Label()
        Me.cboAuthor = New System.Windows.Forms.ComboBox()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.cboCategory = New System.Windows.Forms.ComboBox()
        Me.lblPublisher = New System.Windows.Forms.Label()
        Me.cboPublisher = New System.Windows.Forms.ComboBox()
        Me.lblYear = New System.Windows.Forms.Label()
        Me.txtYear = New System.Windows.Forms.TextBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.grpList = New System.Windows.Forms.GroupBox()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dgvBooks = New System.Windows.Forms.DataGridView()
        Me.grpInfo.SuspendLayout()
        Me.grpList.SuspendLayout()
        CType(Me.dgvBooks, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' grpInfo
        '
        Me.grpInfo.Controls.Add(Me.btnClear)
        Me.grpInfo.Controls.Add(Me.btnDelete)
        Me.grpInfo.Controls.Add(Me.btnEdit)
        Me.grpInfo.Controls.Add(Me.btnAdd)
        Me.grpInfo.Controls.Add(Me.txtYear)
        Me.grpInfo.Controls.Add(Me.lblYear)
        Me.grpInfo.Controls.Add(Me.cboPublisher)
        Me.grpInfo.Controls.Add(Me.lblPublisher)
        Me.grpInfo.Controls.Add(Me.cboCategory)
        Me.grpInfo.Controls.Add(Me.lblCategory)
        Me.grpInfo.Controls.Add(Me.cboAuthor)
        Me.grpInfo.Controls.Add(Me.lblAuthor)
        Me.grpInfo.Controls.Add(Me.txtQuantity)
        Me.grpInfo.Controls.Add(Me.lblQuantity)
        Me.grpInfo.Controls.Add(Me.txtPrice)
        Me.grpInfo.Controls.Add(Me.lblPrice)
        Me.grpInfo.Controls.Add(Me.txtTitle)
        Me.grpInfo.Controls.Add(Me.lblTitle)
        Me.grpInfo.Controls.Add(Me.txtCode)
        Me.grpInfo.Controls.Add(Me.lblCode)
        Me.grpInfo.Location = New System.Drawing.Point(12, 12)
        Me.grpInfo.Name = "grpInfo"
        Me.grpInfo.Size = New System.Drawing.Size(880, 220)
        Me.grpInfo.TabIndex = 0
        Me.grpInfo.TabStop = False
        Me.grpInfo.Text = "Thông tin chi tiết"
        '
        ' lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.Location = New System.Drawing.Point(20, 30)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(85, 19)
        Me.lblCode.TabIndex = 0
        Me.lblCode.Text = "Mã sách (*):"
        '
        ' txtCode
        '
        Me.txtCode.Location = New System.Drawing.Point(120, 27)
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(150, 25)
        Me.txtCode.TabIndex = 1
        '
        ' lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(20, 65)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(86, 19)
        Me.lblTitle.TabIndex = 2
        Me.lblTitle.Text = "Tên sách (*):"
        '
        ' txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(120, 62)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(300, 25)
        Me.txtTitle.TabIndex = 3
        '
        ' lblPrice
        '
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Location = New System.Drawing.Point(20, 100)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(60, 19)
        Me.lblPrice.TabIndex = 4
        Me.lblPrice.Text = "Giá tiền:"
        '
        ' txtPrice
        '
        Me.txtPrice.Location = New System.Drawing.Point(120, 97)
        Me.txtPrice.Name = "txtPrice"
        Me.txtPrice.Size = New System.Drawing.Size(150, 25)
        Me.txtPrice.TabIndex = 5
        Me.txtPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        ' lblQuantity
        '
        Me.lblQuantity.AutoSize = True
        Me.lblQuantity.Location = New System.Drawing.Point(20, 135)
        Me.lblQuantity.Name = "lblQuantity"
        Me.lblQuantity.Size = New System.Drawing.Size(66, 19)
        Me.lblQuantity.TabIndex = 6
        Me.lblQuantity.Text = "Số lượng:"
        '
        ' txtQuantity
        '
        Me.txtQuantity.Location = New System.Drawing.Point(120, 132)
        Me.txtQuantity.Name = "txtQuantity"
        Me.txtQuantity.Size = New System.Drawing.Size(100, 25)
        Me.txtQuantity.TabIndex = 7
        Me.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        ' lblAuthor
        '
        Me.lblAuthor.AutoSize = True
        Me.lblAuthor.Location = New System.Drawing.Point(450, 30)
        Me.lblAuthor.Name = "lblAuthor"
        Me.lblAuthor.Size = New System.Drawing.Size(56, 19)
        Me.lblAuthor.TabIndex = 8
        Me.lblAuthor.Text = "Tác giả:"
        '
        ' cboAuthor
        '
        Me.cboAuthor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAuthor.FormattingEnabled = True
        Me.cboAuthor.Location = New System.Drawing.Point(530, 27)
        Me.cboAuthor.Name = "cboAuthor"
        Me.cboAuthor.Size = New System.Drawing.Size(250, 25)
        Me.cboAuthor.TabIndex = 9
        '
        ' lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Location = New System.Drawing.Point(450, 65)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(60, 19)
        Me.lblCategory.TabIndex = 10
        Me.lblCategory.Text = "Thể loại:"
        '
        ' cboCategory
        '
        Me.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCategory.FormattingEnabled = True
        Me.cboCategory.Location = New System.Drawing.Point(530, 62)
        Me.cboCategory.Name = "cboCategory"
        Me.cboCategory.Size = New System.Drawing.Size(250, 25)
        Me.cboCategory.TabIndex = 11
        '
        ' lblPublisher
        '
        Me.lblPublisher.AutoSize = True
        Me.lblPublisher.Location = New System.Drawing.Point(450, 100)
        Me.lblPublisher.Name = "lblPublisher"
        Me.lblPublisher.Size = New System.Drawing.Size(41, 19)
        Me.lblPublisher.TabIndex = 12
        Me.lblPublisher.Text = "NXB:"
        '
        ' cboPublisher
        '
        Me.cboPublisher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPublisher.FormattingEnabled = True
        Me.cboPublisher.Location = New System.Drawing.Point(530, 97)
        Me.cboPublisher.Name = "cboPublisher"
        Me.cboPublisher.Size = New System.Drawing.Size(250, 25)
        Me.cboPublisher.TabIndex = 13
        '
        ' lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.Location = New System.Drawing.Point(450, 135)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(64, 19)
        Me.lblYear.TabIndex = 14
        Me.lblYear.Text = "Năm XB:"
        '
        ' txtYear
        '
        Me.txtYear.Location = New System.Drawing.Point(530, 132)
        Me.txtYear.Name = "txtYear"
        Me.txtYear.Size = New System.Drawing.Size(100, 25)
        Me.txtYear.TabIndex = 15
        '
        ' btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.LightGreen
        Me.btnAdd.Location = New System.Drawing.Point(120, 175)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(100, 35)
        Me.btnAdd.TabIndex = 16
        Me.btnAdd.Text = "Thêm mới"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        ' btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.Color.LightYellow
        Me.btnEdit.Location = New System.Drawing.Point(230, 175)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(100, 35)
        Me.btnEdit.TabIndex = 17
        Me.btnEdit.Text = "Cập nhật"
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        ' btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.LightPink
        Me.btnDelete.Location = New System.Drawing.Point(340, 175)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 35)
        Me.btnDelete.TabIndex = 18
        Me.btnDelete.Text = "Xóa"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        ' btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(450, 175)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(100, 35)
        Me.btnClear.TabIndex = 19
        Me.btnClear.Text = "Làm mới"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        ' grpList
        '
        Me.grpList.Controls.Add(Me.dgvBooks)
        Me.grpList.Controls.Add(Me.btnSearch)
        Me.grpList.Controls.Add(Me.txtSearch)
        Me.grpList.Location = New System.Drawing.Point(12, 240)
        Me.grpList.Name = "grpList"
        Me.grpList.Size = New System.Drawing.Size(880, 350)
        Me.grpList.TabIndex = 1
        Me.grpList.TabStop = False
        Me.grpList.Text = "Danh sách sách"
        '
        ' txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(20, 30)
        Me.txtSearch.Name = "txtSearch"
        'Me.txtSearch.PlaceholderText = "Nhập tên sách hoặc mã sách..."
        Me.txtSearch.Size = New System.Drawing.Size(300, 25)
        Me.txtSearch.TabIndex = 0
        '
        ' btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(330, 29)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 27)
        Me.btnSearch.TabIndex = 1
        Me.btnSearch.Text = "Tìm kiếm"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        ' dgvBooks
        '
        Me.dgvBooks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvBooks.BackgroundColor = System.Drawing.Color.White
        Me.dgvBooks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBooks.Location = New System.Drawing.Point(20, 70)
        Me.dgvBooks.MultiSelect = False
        Me.dgvBooks.Name = "dgvBooks"
        Me.dgvBooks.ReadOnly = True
        Me.dgvBooks.RowTemplate.Height = 25
        Me.dgvBooks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvBooks.Size = New System.Drawing.Size(840, 260)
        Me.dgvBooks.TabIndex = 2
        '
        ' FrmBookManagement
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 600)
        Me.Controls.Add(Me.grpList)
        Me.Controls.Add(Me.grpInfo)
        Me.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Name = "FrmBookManagement"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Quản Lý Sách - Thư Viện"
        Me.grpInfo.ResumeLayout(False)
        Me.grpInfo.PerformLayout()
        Me.grpList.ResumeLayout(False)
        Me.grpList.PerformLayout()
        CType(Me.dgvBooks, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblCode As System.Windows.Forms.Label
    Friend WithEvents txtCode As System.Windows.Forms.TextBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents lblPrice As System.Windows.Forms.Label
    Friend WithEvents txtPrice As System.Windows.Forms.TextBox
    Friend WithEvents lblQuantity As System.Windows.Forms.Label
    Friend WithEvents txtQuantity As System.Windows.Forms.TextBox
    Friend WithEvents lblAuthor As System.Windows.Forms.Label
    Friend WithEvents cboAuthor As System.Windows.Forms.ComboBox
    Friend WithEvents lblCategory As System.Windows.Forms.Label
    Friend WithEvents cboCategory As System.Windows.Forms.ComboBox
    Friend WithEvents lblPublisher As System.Windows.Forms.Label
    Friend WithEvents cboPublisher As System.Windows.Forms.ComboBox
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents txtYear As System.Windows.Forms.TextBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents grpList As System.Windows.Forms.GroupBox
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents dgvBooks As System.Windows.Forms.DataGridView
End Class