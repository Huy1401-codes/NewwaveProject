<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAuthorDetail
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
        Me.pnlInfo = New System.Windows.Forms.Panel()
        Me.lblAuthorName = New System.Windows.Forms.Label()
        Me.picAvatar = New System.Windows.Forms.PictureBox()
        Me.pnlSearch = New System.Windows.Forms.Panel()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.cboPublisher = New System.Windows.Forms.ComboBox()
        Me.lblPublisher = New System.Windows.Forms.Label()
        Me.txtSearchBook = New System.Windows.Forms.TextBox()
        Me.lblSearchBook = New System.Windows.Forms.Label()
        Me.dgvBooks = New System.Windows.Forms.DataGridView()
        Me.pnlPaging = New System.Windows.Forms.Panel()
        Me.lblPageInfo = New System.Windows.Forms.Label()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.pnlInfo.SuspendLayout()
        CType(Me.picAvatar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSearch.SuspendLayout()
        CType(Me.dgvBooks, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPaging.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlInfo
        '
        Me.pnlInfo.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlInfo.Controls.Add(Me.lblAuthorName)
        Me.pnlInfo.Controls.Add(Me.picAvatar)
        Me.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlInfo.Location = New System.Drawing.Point(0, 0)
        Me.pnlInfo.Name = "pnlInfo"
        Me.pnlInfo.Size = New System.Drawing.Size(769, 87)
        Me.pnlInfo.TabIndex = 0
        '
        'lblAuthorName
        '
        Me.lblAuthorName.AutoSize = True
        Me.lblAuthorName.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblAuthorName.ForeColor = System.Drawing.Color.Navy
        Me.lblAuthorName.Location = New System.Drawing.Point(94, 17)
        Me.lblAuthorName.Name = "lblAuthorName"
        Me.lblAuthorName.Size = New System.Drawing.Size(126, 25)
        Me.lblAuthorName.TabIndex = 1
        Me.lblAuthorName.Text = "TÊN TÁC GIẢ"
        '
        'picAvatar
        '
        Me.picAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picAvatar.Location = New System.Drawing.Point(10, 10)
        Me.picAvatar.Name = "picAvatar"
        Me.picAvatar.Size = New System.Drawing.Size(69, 70)
        Me.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picAvatar.TabIndex = 0
        Me.picAvatar.TabStop = False
        '
        'pnlSearch
        '
        Me.pnlSearch.Controls.Add(Me.btnSearch)
        Me.pnlSearch.Controls.Add(Me.cboPublisher)
        Me.pnlSearch.Controls.Add(Me.lblPublisher)
        Me.pnlSearch.Controls.Add(Me.txtSearchBook)
        Me.pnlSearch.Controls.Add(Me.lblSearchBook)
        Me.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlSearch.Location = New System.Drawing.Point(0, 87)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(769, 43)
        Me.pnlSearch.TabIndex = 1
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(450, 11)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(69, 22)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "Tìm kiếm"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cboPublisher
        '
        Me.cboPublisher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPublisher.FormattingEnabled = True
        Me.cboPublisher.Location = New System.Drawing.Point(282, 12)
        Me.cboPublisher.Name = "cboPublisher"
        Me.cboPublisher.Size = New System.Drawing.Size(155, 21)
        Me.cboPublisher.TabIndex = 3
        '
        'lblPublisher
        '
        Me.lblPublisher.AutoSize = True
        Me.lblPublisher.Location = New System.Drawing.Point(249, 15)
        Me.lblPublisher.Name = "lblPublisher"
        Me.lblPublisher.Size = New System.Drawing.Size(32, 13)
        Me.lblPublisher.TabIndex = 2
        Me.lblPublisher.Text = "NXB:"
        '
        'txtSearchBook
        '
        Me.txtSearchBook.Location = New System.Drawing.Point(63, 12)
        Me.txtSearchBook.Name = "txtSearchBook"
        Me.txtSearchBook.Size = New System.Drawing.Size(172, 20)
        Me.txtSearchBook.TabIndex = 1
        '
        'lblSearchBook
        '
        Me.lblSearchBook.AutoSize = True
        Me.lblSearchBook.Location = New System.Drawing.Point(10, 15)
        Me.lblSearchBook.Name = "lblSearchBook"
        Me.lblSearchBook.Size = New System.Drawing.Size(55, 13)
        Me.lblSearchBook.TabIndex = 0
        Me.lblSearchBook.Text = "Tên sách:"
        '
        'dgvBooks
        '
        Me.dgvBooks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBooks.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvBooks.Location = New System.Drawing.Point(0, 130)
        Me.dgvBooks.Name = "dgvBooks"
        Me.dgvBooks.RowTemplate.Height = 150
        Me.dgvBooks.Size = New System.Drawing.Size(769, 350)
        Me.dgvBooks.TabIndex = 2
        '
        'pnlPaging
        '
        Me.pnlPaging.Controls.Add(Me.lblPageInfo)
        Me.pnlPaging.Controls.Add(Me.btnNext)
        Me.pnlPaging.Controls.Add(Me.btnPrev)
        Me.pnlPaging.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlPaging.Location = New System.Drawing.Point(0, 480)
        Me.pnlPaging.Name = "pnlPaging"
        Me.pnlPaging.Size = New System.Drawing.Size(769, 43)
        Me.pnlPaging.TabIndex = 3
        '
        'lblPageInfo
        '
        Me.lblPageInfo.AutoSize = True
        Me.lblPageInfo.Location = New System.Drawing.Point(86, 16)
        Me.lblPageInfo.Name = "lblPageInfo"
        Me.lblPageInfo.Size = New System.Drawing.Size(61, 13)
        Me.lblPageInfo.TabIndex = 2
        Me.lblPageInfo.Text = "Trang 1 / 1"
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(171, 12)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(64, 20)
        Me.btnNext.TabIndex = 1
        Me.btnNext.Text = "Sau >"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPrev
        '
        Me.btnPrev.Location = New System.Drawing.Point(10, 12)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(64, 20)
        Me.btnPrev.TabIndex = 0
        Me.btnPrev.Text = "< Trước"
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'FrmAuthorDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(769, 523)
        Me.Controls.Add(Me.dgvBooks)
        Me.Controls.Add(Me.pnlPaging)
        Me.Controls.Add(Me.pnlSearch)
        Me.Controls.Add(Me.pnlInfo)
        Me.Name = "FrmAuthorDetail"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Chi Tiết Tác Giả & Sách"
        Me.pnlInfo.ResumeLayout(False)
        Me.pnlInfo.PerformLayout()
        CType(Me.picAvatar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        CType(Me.dgvBooks, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPaging.ResumeLayout(False)
        Me.pnlPaging.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlInfo As System.Windows.Forms.Panel
    Friend WithEvents lblAuthorName As System.Windows.Forms.Label
    Friend WithEvents picAvatar As System.Windows.Forms.PictureBox
    Friend WithEvents pnlSearch As System.Windows.Forms.Panel
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents cboPublisher As System.Windows.Forms.ComboBox
    Friend WithEvents lblPublisher As System.Windows.Forms.Label
    Friend WithEvents txtSearchBook As System.Windows.Forms.TextBox
    Friend WithEvents lblSearchBook As System.Windows.Forms.Label
    Friend WithEvents dgvBooks As System.Windows.Forms.DataGridView
    Friend WithEvents pnlPaging As System.Windows.Forms.Panel
    Friend WithEvents lblPageInfo As System.Windows.Forms.Label
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnPrev As System.Windows.Forms.Button
End Class