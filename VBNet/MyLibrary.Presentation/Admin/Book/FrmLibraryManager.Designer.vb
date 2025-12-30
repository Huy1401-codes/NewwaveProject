<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmLibraryManager
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
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pnlMenu = New System.Windows.Forms.Panel()
        Me.btnBooks = New System.Windows.Forms.Button()
        Me.btnAuthors = New System.Windows.Forms.Button()
        Me.btnReaders = New System.Windows.Forms.Button()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.pnlHeader.SuspendLayout()
        Me.pnlMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(245, 246, 248)
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(800, 55) ' adjust width if needed
        Me.pnlHeader.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64)
        Me.lblTitle.Location = New System.Drawing.Point(10, 15)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(137, 21)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Quản lý thư viện"
        '
        'pnlMenu
        '
        Me.pnlMenu.BackColor = System.Drawing.Color.FromArgb(52, 73, 94)
        Me.pnlMenu.Controls.Add(Me.btnSettings)
        Me.pnlMenu.Controls.Add(Me.btnReaders)
        Me.pnlMenu.Controls.Add(Me.btnAuthors)
        Me.pnlMenu.Controls.Add(Me.btnBooks)
        Me.pnlMenu.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlMenu.Location = New System.Drawing.Point(0, 55)
        Me.pnlMenu.Name = "pnlMenu"
        Me.pnlMenu.Size = New System.Drawing.Size(800, 55)
        Me.pnlMenu.TabIndex = 1
        '
        'btnBooks
        '
        Me.btnBooks.BackColor = System.Drawing.Color.FromArgb(61, 141, 188)
        Me.btnBooks.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnBooks.FlatAppearance.BorderSize = 0
        Me.btnBooks.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBooks.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnBooks.ForeColor = System.Drawing.Color.White
        Me.btnBooks.Location = New System.Drawing.Point(0, 0)
        Me.btnBooks.Name = "btnBooks"
        Me.btnBooks.Size = New System.Drawing.Size(150, 55)
        Me.btnBooks.TabIndex = 0
        Me.btnBooks.Text = "📚  Quản lý sách"
        Me.btnBooks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnBooks.UseVisualStyleBackColor = False
        '
        'btnAuthors
        '
        Me.btnAuthors.BackColor = System.Drawing.Color.FromArgb(61, 141, 188)
        Me.btnAuthors.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnAuthors.FlatAppearance.BorderSize = 0
        Me.btnAuthors.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAuthors.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnAuthors.ForeColor = System.Drawing.Color.White
        Me.btnAuthors.Location = New System.Drawing.Point(150, 0)
        Me.btnAuthors.Name = "btnAuthors"
        Me.btnAuthors.Size = New System.Drawing.Size(150, 55)
        Me.btnAuthors.TabIndex = 1
        Me.btnAuthors.Text = "✍️  Quản lý tác giả"
        Me.btnAuthors.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnAuthors.UseVisualStyleBackColor = False
        '
        'btnReaders
        '
        Me.btnReaders.BackColor = System.Drawing.Color.FromArgb(61, 141, 188)
        Me.btnReaders.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnReaders.FlatAppearance.BorderSize = 0
        Me.btnReaders.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReaders.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnReaders.ForeColor = System.Drawing.Color.White
        Me.btnReaders.Location = New System.Drawing.Point(300, 0)
        Me.btnReaders.Name = "btnReaders"
        Me.btnReaders.Size = New System.Drawing.Size(150, 55)
        Me.btnReaders.TabIndex = 2
        Me.btnReaders.Text = "📂  Danh mục sách"
        Me.btnReaders.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnReaders.UseVisualStyleBackColor = False
        '
        'btnSettings
        '
        Me.btnSettings.BackColor = System.Drawing.Color.FromArgb(61, 141, 188)
        Me.btnSettings.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnSettings.FlatAppearance.BorderSize = 0
        Me.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSettings.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnSettings.ForeColor = System.Drawing.Color.White
        Me.btnSettings.Location = New System.Drawing.Point(450, 0)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(150, 55)
        Me.btnSettings.TabIndex = 3
        Me.btnSettings.Text = "⚙️  Nhà xuất bản"
        Me.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnSettings.UseVisualStyleBackColor = False
        '
        'pnlContent
        '
        Me.pnlContent.BackColor = System.Drawing.Color.FromArgb(236, 240, 241)
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(0, 110)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(800, 340)
        Me.pnlContent.TabIndex = 2
        '
        'FrmLibraryManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.pnlMenu)
        Me.Controls.Add(Me.pnlHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmLibraryManager"
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
    Friend WithEvents btnAuthors As Button
    Friend WithEvents btnReaders As Button
    Friend WithEvents btnSettings As Button
    Friend WithEvents pnlContent As Panel
End Class
