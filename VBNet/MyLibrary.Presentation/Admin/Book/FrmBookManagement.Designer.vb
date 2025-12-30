<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmBookManagement
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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.lblTitleHeader = New System.Windows.Forms.Label()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.cboFilterYear = New System.Windows.Forms.ComboBox()
        Me.lblFilterYear = New System.Windows.Forms.Label()
        Me.cboFilterPublisher = New System.Windows.Forms.ComboBox()
        Me.lblFilterPub = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.pnlTools = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.lblPageInfo = New System.Windows.Forms.Label()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.dgvBooks = New System.Windows.Forms.DataGridView()
        Me.pnlTop.SuspendLayout()
        Me.pnlTools.SuspendLayout()
        Me.pnlBottom.SuspendLayout()
        CType(Me.dgvBooks, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlTop.Controls.Add(Me.btnImport)
        Me.pnlTop.Controls.Add(Me.btnExcel)
        Me.pnlTop.Controls.Add(Me.lblTitleHeader)
        Me.pnlTop.Controls.Add(Me.btnAdd)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(1122, 70)
        Me.pnlTop.TabIndex = 0
        '
        'btnImport
        '
        Me.btnImport.BackColor = System.Drawing.Color.BurlyWood
        Me.btnImport.ForeColor = System.Drawing.Color.White
        Me.btnImport.Location = New System.Drawing.Point(772, 17)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(100, 30)
        Me.btnImport.TabIndex = 2
        Me.btnImport.Text = "Import Excel"
        Me.btnImport.UseVisualStyleBackColor = False
        '
        'btnExcel
        '
        Me.btnExcel.BackColor = System.Drawing.Color.BlueViolet
        Me.btnExcel.ForeColor = System.Drawing.Color.White
        Me.btnExcel.Location = New System.Drawing.Point(894, 17)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(100, 30)
        Me.btnExcel.TabIndex = 1
        Me.btnExcel.Text = "Xuất Excel"
        Me.btnExcel.UseVisualStyleBackColor = False
        '
        'lblTitleHeader
        '
        Me.lblTitleHeader.AutoSize = True
        Me.lblTitleHeader.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitleHeader.ForeColor = System.Drawing.Color.DimGray
        Me.lblTitleHeader.Location = New System.Drawing.Point(33, 17)
        Me.lblTitleHeader.Name = "lblTitleHeader"
        Me.lblTitleHeader.Size = New System.Drawing.Size(150, 25)
        Me.lblTitleHeader.TabIndex = 0
        Me.lblTitleHeader.Text = "QUẢN LÝ SÁCH"
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.SeaGreen
        Me.btnAdd.ForeColor = System.Drawing.Color.White
        Me.btnAdd.Location = New System.Drawing.Point(1010, 17)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(100, 30)
        Me.btnAdd.TabIndex = 0
        Me.btnAdd.Text = "Thêm mới"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'cboFilterYear
        '
        Me.cboFilterYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboFilterYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilterYear.FormattingEnabled = True
        Me.cboFilterYear.Location = New System.Drawing.Point(589, 14)
        Me.cboFilterYear.Name = "cboFilterYear"
        Me.cboFilterYear.Size = New System.Drawing.Size(100, 25)
        Me.cboFilterYear.TabIndex = 6
        '
        'lblFilterYear
        '
        Me.lblFilterYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFilterYear.AutoSize = True
        Me.lblFilterYear.Location = New System.Drawing.Point(540, 17)
        Me.lblFilterYear.Name = "lblFilterYear"
        Me.lblFilterYear.Size = New System.Drawing.Size(41, 19)
        Me.lblFilterYear.TabIndex = 5
        Me.lblFilterYear.Text = "Năm:"
        '
        'cboFilterPublisher
        '
        Me.cboFilterPublisher.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboFilterPublisher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilterPublisher.FormattingEnabled = True
        Me.cboFilterPublisher.Location = New System.Drawing.Point(417, 14)
        Me.cboFilterPublisher.Name = "cboFilterPublisher"
        Me.cboFilterPublisher.Size = New System.Drawing.Size(115, 25)
        Me.cboFilterPublisher.TabIndex = 4
        '
        'lblFilterPub
        '
        Me.lblFilterPub.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFilterPub.AutoSize = True
        Me.lblFilterPub.Location = New System.Drawing.Point(363, 17)
        Me.lblFilterPub.Name = "lblFilterPub"
        Me.lblFilterPub.Size = New System.Drawing.Size(38, 19)
        Me.lblFilterPub.TabIndex = 3
        Me.lblFilterPub.Text = "NXB:"
        '
        'btnSearch
        '
        Me.btnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSearch.Location = New System.Drawing.Point(721, 11)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(90, 30)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "Tìm kiếm"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Location = New System.Drawing.Point(170, 15)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(177, 25)
        Me.txtSearch.TabIndex = 1
        '
        'pnlTools
        '
        Me.pnlTools.Controls.Add(Me.Label1)
        Me.pnlTools.Controls.Add(Me.btnSearch)
        Me.pnlTools.Controls.Add(Me.lblFilterPub)
        Me.pnlTools.Controls.Add(Me.lblFilterYear)
        Me.pnlTools.Controls.Add(Me.cboFilterYear)
        Me.pnlTools.Controls.Add(Me.btnDelete)
        Me.pnlTools.Controls.Add(Me.btnEdit)
        Me.pnlTools.Controls.Add(Me.cboFilterPublisher)
        Me.pnlTools.Controls.Add(Me.txtSearch)
        Me.pnlTools.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTools.Location = New System.Drawing.Point(0, 70)
        Me.pnlTools.Name = "pnlTools"
        Me.pnlTools.Size = New System.Drawing.Size(1122, 50)
        Me.pnlTools.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(156, 19)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Tìm theo tên hoặc code:"
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.IndianRed
        Me.btnDelete.ForeColor = System.Drawing.Color.White
        Me.btnDelete.Location = New System.Drawing.Point(1010, 10)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "Xóa sách"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.Color.Orange
        Me.btnEdit.ForeColor = System.Drawing.Color.White
        Me.btnEdit.Location = New System.Drawing.Point(894, 12)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(96, 27)
        Me.btnEdit.TabIndex = 1
        Me.btnEdit.Text = "Cập nhật"
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'pnlBottom
        '
        Me.pnlBottom.Controls.Add(Me.btnNext)
        Me.pnlBottom.Controls.Add(Me.lblPageInfo)
        Me.pnlBottom.Controls.Add(Me.btnPrev)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 621)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(1122, 40)
        Me.pnlBottom.TabIndex = 3
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(1030, 6)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(80, 30)
        Me.btnNext.TabIndex = 2
        Me.btnNext.Text = "Sau >"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'lblPageInfo
        '
        Me.lblPageInfo.AutoSize = True
        Me.lblPageInfo.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblPageInfo.Location = New System.Drawing.Point(459, 11)
        Me.lblPageInfo.Name = "lblPageInfo"
        Me.lblPageInfo.Size = New System.Drawing.Size(73, 19)
        Me.lblPageInfo.TabIndex = 1
        Me.lblPageInfo.Text = "Trang 0/0"
        '
        'btnPrev
        '
        Me.btnPrev.Location = New System.Drawing.Point(12, 6)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(80, 30)
        Me.btnPrev.TabIndex = 0
        Me.btnPrev.Text = "< Trước"
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'dgvBooks
        '
        Me.dgvBooks.AllowUserToAddRows = False
        Me.dgvBooks.AllowUserToDeleteRows = False
        Me.dgvBooks.BackgroundColor = System.Drawing.Color.White
        Me.dgvBooks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBooks.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvBooks.Location = New System.Drawing.Point(0, 120)
        Me.dgvBooks.MultiSelect = False
        Me.dgvBooks.Name = "dgvBooks"
        Me.dgvBooks.ReadOnly = True
        Me.dgvBooks.RowTemplate.Height = 25
        Me.dgvBooks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvBooks.Size = New System.Drawing.Size(1122, 501)
        Me.dgvBooks.TabIndex = 2
        '
        'FrmBookManagement
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1122, 661)
        Me.Controls.Add(Me.dgvBooks)
        Me.Controls.Add(Me.pnlBottom)
        Me.Controls.Add(Me.pnlTools)
        Me.Controls.Add(Me.pnlTop)
        Me.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Name = "FrmBookManagement"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Hệ thống quản lý thư viện"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.pnlTools.ResumeLayout(False)
        Me.pnlTools.PerformLayout()
        Me.pnlBottom.ResumeLayout(False)
        Me.pnlBottom.PerformLayout()
        CType(Me.dgvBooks, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlTop As Panel
    Friend WithEvents lblTitleHeader As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents pnlTools As Panel
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents dgvBooks As DataGridView
    Friend WithEvents cboFilterYear As ComboBox
    Friend WithEvents lblFilterYear As Label
    Friend WithEvents cboFilterPublisher As ComboBox
    Friend WithEvents lblFilterPub As Label
    Friend WithEvents pnlBottom As Panel
    Friend WithEvents btnNext As Button
    Friend WithEvents lblPageInfo As Label
    Friend WithEvents btnPrev As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents btnExcel As Button
    Friend WithEvents btnImport As Button
End Class