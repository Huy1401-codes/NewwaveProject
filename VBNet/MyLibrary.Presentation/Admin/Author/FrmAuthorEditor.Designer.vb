<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAuthorEditor
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
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblNation = New System.Windows.Forms.Label()
        Me.txtNation = New System.Windows.Forms.TextBox()
        Me.lblBirth = New System.Windows.Forms.Label()
        Me.dtpBirthDate = New System.Windows.Forms.DateTimePicker()
        Me.lblBio = New System.Windows.Forms.Label()
        Me.txtBio = New System.Windows.Forms.TextBox()
        Me.picAvatar = New System.Windows.Forms.PictureBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        CType(Me.picAvatar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(20, 20)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(61, 15)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Họ tên (*):"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(20, 40)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(250, 23)
        Me.txtName.TabIndex = 1
        '
        'lblNation
        '
        Me.lblNation.AutoSize = True
        Me.lblNation.Location = New System.Drawing.Point(20, 80)
        Me.lblNation.Name = "lblNation"
        Me.lblNation.Size = New System.Drawing.Size(62, 15)
        Me.lblNation.TabIndex = 2
        Me.lblNation.Text = "Quốc tịch:"
        '
        'txtNation
        '
        Me.txtNation.Location = New System.Drawing.Point(20, 100)
        Me.txtNation.Name = "txtNation"
        Me.txtNation.Size = New System.Drawing.Size(250, 23)
        Me.txtNation.TabIndex = 3
        '
        'lblBirth
        '
        Me.lblBirth.AutoSize = True
        Me.lblBirth.Location = New System.Drawing.Point(20, 140)
        Me.lblBirth.Name = "lblBirth"
        Me.lblBirth.Size = New System.Drawing.Size(63, 15)
        Me.lblBirth.TabIndex = 4
        Me.lblBirth.Text = "Ngày sinh:"
        '
        'dtpBirthDate
        '
        Me.dtpBirthDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpBirthDate.Location = New System.Drawing.Point(20, 160)
        Me.dtpBirthDate.Name = "dtpBirthDate"
        Me.dtpBirthDate.Size = New System.Drawing.Size(250, 23)
        Me.dtpBirthDate.TabIndex = 5
        '
        'lblBio
        '
        Me.lblBio.AutoSize = True
        Me.lblBio.Location = New System.Drawing.Point(20, 200)
        Me.lblBio.Name = "lblBio"
        Me.lblBio.Size = New System.Drawing.Size(47, 15)
        Me.lblBio.TabIndex = 6
        Me.lblBio.Text = "Tiểu sử:"
        '
        'txtBio
        '
        Me.txtBio.Location = New System.Drawing.Point(20, 220)
        Me.txtBio.Multiline = True
        Me.txtBio.Name = "txtBio"
        Me.txtBio.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBio.Size = New System.Drawing.Size(250, 100)
        Me.txtBio.TabIndex = 7
        '
        'picAvatar
        '
        Me.picAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picAvatar.Location = New System.Drawing.Point(300, 40)
        Me.picAvatar.Name = "picAvatar"
        Me.picAvatar.Size = New System.Drawing.Size(150, 150)
        Me.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picAvatar.TabIndex = 8
        Me.picAvatar.TabStop = False
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(300, 200)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(150, 30)
        Me.btnBrowse.TabIndex = 9
        Me.btnBrowse.Text = "Chọn ảnh..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.LightGreen
        Me.btnSave.Location = New System.Drawing.Point(200, 350)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(120, 40)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "Lưu"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(330, 350)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(120, 40)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "Hủy bỏ"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'FrmAuthorEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(480, 410)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.picAvatar)
        Me.Controls.Add(Me.txtBio)
        Me.Controls.Add(Me.lblBio)
        Me.Controls.Add(Me.dtpBirthDate)
        Me.Controls.Add(Me.lblBirth)
        Me.Controls.Add(Me.txtNation)
        Me.Controls.Add(Me.lblNation)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmAuthorEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Thông Tin Tác Giả"
        CType(Me.picAvatar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lblNation As System.Windows.Forms.Label
    Friend WithEvents txtNation As System.Windows.Forms.TextBox
    Friend WithEvents lblBirth As System.Windows.Forms.Label
    Friend WithEvents dtpBirthDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblBio As System.Windows.Forms.Label
    Friend WithEvents txtBio As System.Windows.Forms.TextBox
    Friend WithEvents picAvatar As System.Windows.Forms.PictureBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class