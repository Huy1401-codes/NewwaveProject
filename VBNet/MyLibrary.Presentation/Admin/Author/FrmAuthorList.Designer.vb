<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAuthorList
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


    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.lblPageInfo = New System.Windows.Forms.Label()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.pnlActions = New System.Windows.Forms.Panel()
        Me.btnDetail = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.dgvAuthors = New System.Windows.Forms.DataGridView()
        Me.pnlTop.SuspendLayout()
        Me.pnlBottom.SuspendLayout()
        Me.pnlActions.SuspendLayout()
        CType(Me.dgvAuthors, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop (Tìm kiếm)
        '
        Me.pnlTop.Controls.Add(Me.btnSearch)
        Me.pnlTop.Controls.Add(Me.txtSearch)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(800, 50)
        Me.pnlTop.TabIndex = 0
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(12, 12)
        Me.txtSearch.Name = "txtSearch"
        'Me.txtSearch.PlaceholderText = "Nhập tên tác giả để tìm..."
        Me.txtSearch.Size = New System.Drawing.Size(300, 23)
        Me.txtSearch.TabIndex = 0
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(318, 11)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 25)
        Me.btnSearch.TabIndex = 1
        Me.btnSearch.Text = "Tìm kiếm"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'pnlActions (Các nút Thêm/Sửa/Xóa bên phải)
        '
        Me.pnlActions.Controls.Add(Me.btnDetail)
        Me.pnlActions.Controls.Add(Me.btnDelete)
        Me.pnlActions.Controls.Add(Me.btnEdit)
        Me.pnlActions.Controls.Add(Me.btnAdd)
        Me.pnlActions.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlActions.Location = New System.Drawing.Point(650, 50)
        Me.pnlActions.Name = "pnlActions"
        Me.pnlActions.Size = New System.Drawing.Size(150, 350)
        Me.pnlActions.TabIndex = 2
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(15, 20)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(120, 35)
        Me.btnAdd.TabIndex = 0
        Me.btnAdd.Text = "Thêm mới"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(15, 61)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(120, 35)
        Me.btnEdit.TabIndex = 1
        Me.btnEdit.Text = "Sửa thông tin"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(15, 102)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(120, 35)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "Xóa"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnDetail
        '
        Me.btnDetail.Location = New System.Drawing.Point(15, 160)
        Me.btnDetail.Name = "btnDetail"
        Me.btnDetail.Size = New System.Drawing.Size(120, 35)
        Me.btnDetail.TabIndex = 3
        Me.btnDetail.Text = "Xem chi tiết"
        Me.btnDetail.UseVisualStyleBackColor = True
        '
        'pnlBottom (Phân trang)
        '
        Me.pnlBottom.Controls.Add(Me.lblPageInfo)
        Me.pnlBottom.Controls.Add(Me.btnNext)
        Me.pnlBottom.Controls.Add(Me.btnPrev)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 400)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(800, 50)
        Me.pnlBottom.TabIndex = 1
        '
        'btnPrev
        '
        Me.btnPrev.Location = New System.Drawing.Point(12, 15)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(75, 23)
        Me.btnPrev.TabIndex = 0
        Me.btnPrev.Text = "< Trước"
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(200, 15)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(75, 23)
        Me.btnNext.TabIndex = 1
        Me.btnNext.Text = "Sau >"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'lblPageInfo
        '
        Me.lblPageInfo.AutoSize = True
        Me.lblPageInfo.Location = New System.Drawing.Point(100, 19)
        Me.lblPageInfo.Name = "lblPageInfo"
        Me.lblPageInfo.Size = New System.Drawing.Size(65, 15)
        Me.lblPageInfo.TabIndex = 2
        Me.lblPageInfo.Text = "Trang 1 / 1"
        '
        'dgvAuthors (Lưới dữ liệu)
        '
        Me.dgvAuthors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAuthors.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAuthors.Location = New System.Drawing.Point(0, 50)
        Me.dgvAuthors.Name = "dgvAuthors"
        Me.dgvAuthors.RowTemplate.Height = 25
        Me.dgvAuthors.Size = New System.Drawing.Size(650, 350)
        Me.dgvAuthors.TabIndex = 3
        '
        'FrmAuthorList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.dgvAuthors)
        Me.Controls.Add(Me.pnlActions)
        Me.Controls.Add(Me.pnlBottom)
        Me.Controls.Add(Me.pnlTop)
        Me.Name = "FrmAuthorList"
        Me.Text = "Quản Lý Tác Giả"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.pnlBottom.ResumeLayout(False)
        Me.pnlBottom.PerformLayout()
        Me.pnlActions.ResumeLayout(False)
        CType(Me.dgvAuthors, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Friend WithEvents lblPageInfo As System.Windows.Forms.Label
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnPrev As System.Windows.Forms.Button
    Friend WithEvents pnlActions As System.Windows.Forms.Panel
    Friend WithEvents btnDetail As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents dgvAuthors As System.Windows.Forms.DataGridView
End Class