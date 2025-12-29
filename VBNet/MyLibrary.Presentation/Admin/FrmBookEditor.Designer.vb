<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmBookEditor
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
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.picCover = New System.Windows.Forms.PictureBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.ofdImage = New System.Windows.Forms.OpenFileDialog()
        CType(Me.picCover, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.Location = New System.Drawing.Point(20, 25)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(85, 19)
        Me.lblCode.TabIndex = 0
        Me.lblCode.Text = "Mã sách (*):"
        '
        ' txtCode
        '
        Me.txtCode.Location = New System.Drawing.Point(110, 22)
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(180, 25)
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
        Me.txtTitle.Location = New System.Drawing.Point(110, 62)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(300, 25)
        Me.txtTitle.TabIndex = 3
        '
        ' lblPrice
        '
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Location = New System.Drawing.Point(20, 105)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(60, 19)
        Me.lblPrice.TabIndex = 4
        Me.lblPrice.Text = "Giá tiền:"
        '
        ' txtPrice
        '
        Me.txtPrice.Location = New System.Drawing.Point(110, 102)
        Me.txtPrice.Name = "txtPrice"
        Me.txtPrice.Size = New System.Drawing.Size(180, 25)
        Me.txtPrice.TabIndex = 5
        Me.txtPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        ' lblQuantity
        '
        Me.lblQuantity.AutoSize = True
        Me.lblQuantity.Location = New System.Drawing.Point(20, 145)
        Me.lblQuantity.Name = "lblQuantity"
        Me.lblQuantity.Size = New System.Drawing.Size(66, 19)
        Me.lblQuantity.TabIndex = 6
        Me.lblQuantity.Text = "Số lượng:"
        '
        ' txtQuantity
        '
        Me.txtQuantity.Location = New System.Drawing.Point(110, 142)
        Me.txtQuantity.Name = "txtQuantity"
        Me.txtQuantity.Size = New System.Drawing.Size(100, 25)
        Me.txtQuantity.TabIndex = 7
        Me.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        ' lblAuthor
        '
        Me.lblAuthor.AutoSize = True
        Me.lblAuthor.Location = New System.Drawing.Point(20, 185)
        Me.lblAuthor.Name = "lblAuthor"
        Me.lblAuthor.Size = New System.Drawing.Size(56, 19)
        Me.lblAuthor.TabIndex = 8
        Me.lblAuthor.Text = "Tác giả:"
        '
        ' cboAuthor
        '
        Me.cboAuthor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAuthor.FormattingEnabled = True
        Me.cboAuthor.Location = New System.Drawing.Point(110, 182)
        Me.cboAuthor.Name = "cboAuthor"
        Me.cboAuthor.Size = New System.Drawing.Size(300, 25)
        Me.cboAuthor.TabIndex = 9
        '
        ' lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Location = New System.Drawing.Point(20, 225)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(60, 19)
        Me.lblCategory.TabIndex = 10
        Me.lblCategory.Text = "Thể loại:"
        '
        ' cboCategory
        '
        Me.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCategory.FormattingEnabled = True
        Me.cboCategory.Location = New System.Drawing.Point(110, 222)
        Me.cboCategory.Name = "cboCategory"
        Me.cboCategory.Size = New System.Drawing.Size(300, 25)
        Me.cboCategory.TabIndex = 11
        '
        ' lblPublisher
        '
        Me.lblPublisher.AutoSize = True
        Me.lblPublisher.Location = New System.Drawing.Point(20, 265)
        Me.lblPublisher.Name = "lblPublisher"
        Me.lblPublisher.Size = New System.Drawing.Size(41, 19)
        Me.lblPublisher.TabIndex = 12
        Me.lblPublisher.Text = "NXB:"
        '
        ' cboPublisher
        '
        Me.cboPublisher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPublisher.FormattingEnabled = True
        Me.cboPublisher.Location = New System.Drawing.Point(110, 262)
        Me.cboPublisher.Name = "cboPublisher"
        Me.cboPublisher.Size = New System.Drawing.Size(300, 25)
        Me.cboPublisher.TabIndex = 13
        '
        ' lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.Location = New System.Drawing.Point(220, 145)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(64, 19)
        Me.lblYear.TabIndex = 14
        Me.lblYear.Text = "Năm XB:"
        '
        ' txtYear
        '
        Me.txtYear.Location = New System.Drawing.Point(290, 142)
        Me.txtYear.Name = "txtYear"
        Me.txtYear.Size = New System.Drawing.Size(120, 25)
        Me.txtYear.TabIndex = 15
        '
        ' btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.DodgerBlue
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.Location = New System.Drawing.Point(440, 310)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 40)
        Me.btnSave.TabIndex = 16
        Me.btnSave.Text = "LƯU"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        ' btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnCancel.Location = New System.Drawing.Point(560, 310)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 40)
        Me.btnCancel.TabIndex = 17
        Me.btnCancel.Text = "Hủy bỏ"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        ' picCover
        '
        Me.picCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picCover.Location = New System.Drawing.Point(440, 22)
        Me.picCover.Name = "picCover"
        Me.picCover.Size = New System.Drawing.Size(220, 230)
        Me.picCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picCover.TabIndex = 18
        Me.picCover.TabStop = False
        '
        ' btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(440, 260)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(220, 30)
        Me.btnBrowse.TabIndex = 19
        Me.btnBrowse.Text = "Chọn ảnh bìa..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        ' ofdImage
        '
        Me.ofdImage.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
        Me.ofdImage.Title = "Chọn ảnh bìa sách"
        '
        ' FrmBookEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(694, 371)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.picCover)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtYear)
        Me.Controls.Add(Me.lblYear)
        Me.Controls.Add(Me.cboPublisher)
        Me.Controls.Add(Me.lblPublisher)
        Me.Controls.Add(Me.cboCategory)
        Me.Controls.Add(Me.lblCategory)
        Me.Controls.Add(Me.cboAuthor)
        Me.Controls.Add(Me.lblAuthor)
        Me.Controls.Add(Me.txtQuantity)
        Me.Controls.Add(Me.lblQuantity)
        Me.Controls.Add(Me.txtPrice)
        Me.Controls.Add(Me.lblPrice)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.txtCode)
        Me.Controls.Add(Me.lblCode)
        Me.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmBookEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Thông tin sách"
        CType(Me.picCover, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents lblCode As Label
    Friend WithEvents txtCode As TextBox
    Friend WithEvents lblTitle As Label
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents lblPrice As Label
    Friend WithEvents txtPrice As TextBox
    Friend WithEvents lblQuantity As Label
    Friend WithEvents txtQuantity As TextBox
    Friend WithEvents lblAuthor As Label
    Friend WithEvents cboAuthor As ComboBox
    Friend WithEvents lblCategory As Label
    Friend WithEvents cboCategory As ComboBox
    Friend WithEvents lblPublisher As Label
    Friend WithEvents cboPublisher As ComboBox
    Friend WithEvents lblYear As Label
    Friend WithEvents txtYear As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents picCover As PictureBox
    Friend WithEvents btnBrowse As Button
    Friend WithEvents ofdImage As OpenFileDialog
End Class