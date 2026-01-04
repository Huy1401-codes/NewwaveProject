<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmUserMenu
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
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.btnReturn = New System.Windows.Forms.Button()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pnlMenu = New System.Windows.Forms.Panel()
        Me.btnPayReturn = New System.Windows.Forms.Button()
        Me.btnMyHistory = New System.Windows.Forms.Button()
        Me.btnBooks = New System.Windows.Forms.Button()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.pnlHeader.SuspendLayout()
        Me.pnlMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.pnlHeader.Controls.Add(Me.btnReturn)
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(799, 68)
        Me.pnlHeader.TabIndex = 0
        '
        'btnReturn
        '
        Me.btnReturn.BackColor = System.Drawing.Color.Red
        Me.btnReturn.Location = New System.Drawing.Point(680, 18)
        Me.btnReturn.Margin = New System.Windows.Forms.Padding(4)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(96, 28)
        Me.btnReturn.TabIndex = 1
        Me.btnReturn.Text = "Đăng xuất"
        Me.btnReturn.UseVisualStyleBackColor = False
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTitle.Location = New System.Drawing.Point(308, 18)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(165, 28)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Quản lý cá nhân"
        '
        'pnlMenu
        '
        Me.pnlMenu.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.pnlMenu.Controls.Add(Me.btnPayReturn)
        Me.pnlMenu.Controls.Add(Me.btnMyHistory)
        Me.pnlMenu.Controls.Add(Me.btnBooks)
        Me.pnlMenu.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlMenu.Location = New System.Drawing.Point(0, 68)
        Me.pnlMenu.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlMenu.Name = "pnlMenu"
        Me.pnlMenu.Size = New System.Drawing.Size(799, 68)
        Me.pnlMenu.TabIndex = 1
        '
        'btnPayReturn
        '
        Me.btnPayReturn.BackColor = System.Drawing.Color.FromArgb(CType(CType(61, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(188, Byte), Integer))
        Me.btnPayReturn.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnPayReturn.FlatAppearance.BorderSize = 0
        Me.btnPayReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPayReturn.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnPayReturn.ForeColor = System.Drawing.Color.White
        Me.btnPayReturn.Location = New System.Drawing.Point(523, 0)
        Me.btnPayReturn.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPayReturn.Name = "btnPayReturn"
        Me.btnPayReturn.Size = New System.Drawing.Size(276, 68)
        Me.btnPayReturn.TabIndex = 3
        Me.btnPayReturn.Text = "⚙️  Thanh toán tiền phạt"
        Me.btnPayReturn.UseVisualStyleBackColor = False
        '
        'btnMyHistory
        '
        Me.btnMyHistory.BackColor = System.Drawing.Color.FromArgb(CType(CType(61, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(188, Byte), Integer))
        Me.btnMyHistory.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnMyHistory.FlatAppearance.BorderSize = 0
        Me.btnMyHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMyHistory.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnMyHistory.ForeColor = System.Drawing.Color.White
        Me.btnMyHistory.Location = New System.Drawing.Point(248, 0)
        Me.btnMyHistory.Margin = New System.Windows.Forms.Padding(4)
        Me.btnMyHistory.Name = "btnMyHistory"
        Me.btnMyHistory.Size = New System.Drawing.Size(275, 68)
        Me.btnMyHistory.TabIndex = 1
        Me.btnMyHistory.Text = "✍️  Lịch sử mượn sách"
        Me.btnMyHistory.UseVisualStyleBackColor = False
        '
        'btnBooks
        '
        Me.btnBooks.BackColor = System.Drawing.Color.FromArgb(CType(CType(61, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(188, Byte), Integer))
        Me.btnBooks.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnBooks.FlatAppearance.BorderSize = 0
        Me.btnBooks.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBooks.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnBooks.ForeColor = System.Drawing.Color.White
        Me.btnBooks.Location = New System.Drawing.Point(0, 0)
        Me.btnBooks.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBooks.Name = "btnBooks"
        Me.btnBooks.Size = New System.Drawing.Size(248, 68)
        Me.btnBooks.TabIndex = 0
        Me.btnBooks.Text = "📚  Đăng kí mượn sách"
        Me.btnBooks.UseVisualStyleBackColor = False
        '
        'pnlContent
        '
        Me.pnlContent.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(0, 136)
        Me.pnlContent.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(799, 121)
        Me.pnlContent.TabIndex = 2
        '
        'FrmUserMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(799, 257)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.pnlMenu)
        Me.Controls.Add(Me.pnlHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "FrmUserMenu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Library Manager"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub


    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents pnlMenu As Panel
    Friend WithEvents btnBooks As Button
    Friend WithEvents btnMyHistory As Button
    Friend WithEvents btnPayReturn As Button
    Friend WithEvents pnlContent As Panel
    Friend WithEvents btnReturn As Button
End Class
