<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmPublisherList
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
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
        Me.dgvCategories = New System.Windows.Forms.DataGridView()
        Me.pnlTop.SuspendLayout()
        Me.pnlBottom.SuspendLayout()
        Me.pnlActions.SuspendLayout()
        CType(Me.dgvCategories, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.Button1)
        Me.pnlTop.Controls.Add(Me.Label2)
        Me.pnlTop.Controls.Add(Me.Label1)
        Me.pnlTop.Controls.Add(Me.btnSearch)
        Me.pnlTop.Controls.Add(Me.txtSearch)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(741, 90)
        Me.pnlTop.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(15, 20)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Quay Lại"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(299, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(241, 30)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Quản Lý Nhà xuất bản"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(158, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Tìm kiếm theo tên NXB:"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(449, 62)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(64, 22)
        Me.btnSearch.TabIndex = 1
        Me.btnSearch.Text = "Tìm kiếm"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(176, 62)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(258, 20)
        Me.txtSearch.TabIndex = 0
        '
        'pnlBottom
        '
        Me.pnlBottom.Controls.Add(Me.lblPageInfo)
        Me.pnlBottom.Controls.Add(Me.btnNext)
        Me.pnlBottom.Controls.Add(Me.btnPrev)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 507)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(741, 43)
        Me.pnlBottom.TabIndex = 1
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
        Me.btnNext.Location = New System.Drawing.Point(171, 13)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(64, 20)
        Me.btnNext.TabIndex = 1
        Me.btnNext.Text = "Sau >"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPrev
        '
        Me.btnPrev.Location = New System.Drawing.Point(10, 13)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(64, 20)
        Me.btnPrev.TabIndex = 0
        Me.btnPrev.Text = "< Trước"
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'pnlActions
        '
        Me.pnlActions.Controls.Add(Me.btnDetail)
        Me.pnlActions.Controls.Add(Me.btnDelete)
        Me.pnlActions.Controls.Add(Me.btnEdit)
        Me.pnlActions.Controls.Add(Me.btnAdd)
        Me.pnlActions.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlActions.Location = New System.Drawing.Point(612, 90)
        Me.pnlActions.Name = "pnlActions"
        Me.pnlActions.Size = New System.Drawing.Size(129, 417)
        Me.pnlActions.TabIndex = 2
        '
        'btnDetail
        '
        Me.btnDetail.Location = New System.Drawing.Point(13, 139)
        Me.btnDetail.Name = "btnDetail"
        Me.btnDetail.Size = New System.Drawing.Size(103, 30)
        Me.btnDetail.TabIndex = 3
        Me.btnDetail.Text = "Xem chi tiết"
        Me.btnDetail.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(13, 88)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(103, 30)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "Xóa"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(13, 53)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(103, 30)
        Me.btnEdit.TabIndex = 1
        Me.btnEdit.Text = "Sửa thông tin"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(13, 17)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(103, 30)
        Me.btnAdd.TabIndex = 0
        Me.btnAdd.Text = "Thêm mới"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'dgvCategories
        '
        Me.dgvCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCategories.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvCategories.Location = New System.Drawing.Point(0, 90)
        Me.dgvCategories.Name = "dgvCategories"
        Me.dgvCategories.RowTemplate.Height = 25
        Me.dgvCategories.Size = New System.Drawing.Size(612, 417)
        Me.dgvCategories.TabIndex = 3
        '
        'FrmPublisherList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(741, 550)
        Me.Controls.Add(Me.dgvCategories)
        Me.Controls.Add(Me.pnlActions)
        Me.Controls.Add(Me.pnlBottom)
        Me.Controls.Add(Me.pnlTop)
        Me.Name = "FrmPublisherList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Quản Lý Nhà xuất bản"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.pnlBottom.ResumeLayout(False)
        Me.pnlBottom.PerformLayout()
        Me.pnlActions.ResumeLayout(False)
        CType(Me.dgvCategories, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents dgvCategories As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Label2 As Label
End Class