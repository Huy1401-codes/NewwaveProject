<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAdminApproval
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.lblSort = New System.Windows.Forms.Label()
        Me.cboSort = New System.Windows.Forms.ComboBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.lblPageInfo = New System.Windows.Forms.Label()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.btnReject = New System.Windows.Forms.Button()
        Me.btnApprove = New System.Windows.Forms.Button()
        Me.dgvRequests = New System.Windows.Forms.DataGridView()
        Me.pnlTop.SuspendLayout()
        Me.pnlBottom.SuspendLayout()
        CType(Me.dgvRequests, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlTop.Controls.Add(Me.lblSort)
        Me.pnlTop.Controls.Add(Me.cboSort)
        Me.pnlTop.Controls.Add(Me.lblTitle)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(984, 70)
        Me.pnlTop.TabIndex = 0
        '
        'lblSort
        '
        Me.lblSort.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSort.AutoSize = True
        Me.lblSort.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSort.Location = New System.Drawing.Point(750, 28)
        Me.lblSort.Name = "lblSort"
        Me.lblSort.Size = New System.Drawing.Size(64, 16)
        Me.lblSort.TabIndex = 2
        Me.lblSort.Text = "Sắp xếp:"
        '
        'cboSort
        '
        Me.cboSort.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSort.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSort.FormattingEnabled = True
        Me.cboSort.Items.AddRange(New Object() {"Mới nhất", "Cũ nhất"})
        Me.cboSort.Location = New System.Drawing.Point(820, 25)
        Me.cboSort.Name = "cboSort"
        Me.cboSort.Size = New System.Drawing.Size(150, 24)
        Me.cboSort.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.Navy
        Me.lblTitle.Location = New System.Drawing.Point(15, 22)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(262, 24)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "DUYỆT YÊU CẦU MƯỢN"
        '
        'pnlBottom
        '
        Me.pnlBottom.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlBottom.Controls.Add(Me.lblPageInfo)
        Me.pnlBottom.Controls.Add(Me.btnNext)
        Me.pnlBottom.Controls.Add(Me.btnPrev)
        Me.pnlBottom.Controls.Add(Me.btnReject)
        Me.pnlBottom.Controls.Add(Me.btnApprove)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 491)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(984, 70)
        Me.pnlBottom.TabIndex = 2
        '
        'lblPageInfo
        '
        Me.lblPageInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPageInfo.AutoSize = True
        Me.lblPageInfo.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblPageInfo.Location = New System.Drawing.Point(815, 27)
        Me.lblPageInfo.Name = "lblPageInfo"
        Me.lblPageInfo.Size = New System.Drawing.Size(37, 16)
        Me.lblPageInfo.TabIndex = 4
        Me.lblPageInfo.Text = "1 / 1"
        Me.lblPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNext.Location = New System.Drawing.Point(880, 18)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(90, 35)
        Me.btnNext.TabIndex = 3
        Me.btnNext.Text = "Sau >"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPrev
        '
        Me.btnPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrev.Location = New System.Drawing.Point(700, 18)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(90, 35)
        Me.btnPrev.TabIndex = 2
        Me.btnPrev.Text = "< Trước"
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'btnReject
        '
        Me.btnReject.BackColor = System.Drawing.Color.Firebrick
        Me.btnReject.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReject.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReject.ForeColor = System.Drawing.Color.White
        Me.btnReject.Location = New System.Drawing.Point(150, 18)
        Me.btnReject.Name = "btnReject"
        Me.btnReject.Size = New System.Drawing.Size(120, 35)
        Me.btnReject.TabIndex = 1
        Me.btnReject.Text = "TỪ CHỐI"
        Me.btnReject.UseVisualStyleBackColor = False
        '
        'btnApprove
        '
        Me.btnApprove.BackColor = System.Drawing.Color.ForestGreen
        Me.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnApprove.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApprove.ForeColor = System.Drawing.Color.White
        Me.btnApprove.Location = New System.Drawing.Point(19, 18)
        Me.btnApprove.Name = "btnApprove"
        Me.btnApprove.Size = New System.Drawing.Size(120, 35)
        Me.btnApprove.TabIndex = 0
        Me.btnApprove.Text = "ĐỒNG Ý"
        Me.btnApprove.UseVisualStyleBackColor = False
        '
        'dgvRequests
        '
        Me.dgvRequests.AllowUserToAddRows = False
        Me.dgvRequests.AllowUserToDeleteRows = False
        Me.dgvRequests.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvRequests.BackgroundColor = System.Drawing.Color.White
        Me.dgvRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvRequests.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvRequests.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvRequests.Location = New System.Drawing.Point(0, 70)
        Me.dgvRequests.MultiSelect = False
        Me.dgvRequests.Name = "dgvRequests"
        Me.dgvRequests.ReadOnly = True
        Me.dgvRequests.RowHeadersVisible = False
        Me.dgvRequests.RowTemplate.Height = 30
        Me.dgvRequests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvRequests.Size = New System.Drawing.Size(984, 421)
        Me.dgvRequests.TabIndex = 1
        '
        'FrmAdminApproval
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 561)
        Me.Controls.Add(Me.dgvRequests)
        Me.Controls.Add(Me.pnlBottom)
        Me.Controls.Add(Me.pnlTop)
        Me.Name = "FrmAdminApproval"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Quản lý Duyệt Mượn"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.pnlBottom.ResumeLayout(False)
        Me.pnlBottom.PerformLayout()
        CType(Me.dgvRequests, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents cboSort As System.Windows.Forms.ComboBox
    Friend WithEvents lblSort As System.Windows.Forms.Label
    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Friend WithEvents btnReject As System.Windows.Forms.Button
    Friend WithEvents btnApprove As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnPrev As System.Windows.Forms.Button
    Friend WithEvents lblPageInfo As System.Windows.Forms.Label
    Friend WithEvents dgvRequests As System.Windows.Forms.DataGridView
End Class