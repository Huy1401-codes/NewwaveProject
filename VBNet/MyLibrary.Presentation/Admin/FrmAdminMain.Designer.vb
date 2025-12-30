Imports MyLibrary.BLL


<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAdminMain
    Inherits Form

    Private components As System.ComponentModel.IContainer

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub InitializeComponent()
        Me.lblWelcome = New Label()
        Me.btnLogout = New Button()

        Me.btnManageBooks = New Button()
        Me.btnManageBorrow = New Button()
        Me.btnManageUsers = New Button()
        Me.btnStatistics = New Button()
        Me.grpDashboard = New GroupBox()

        Me.SuspendLayout()


        Me.lblWelcome.Location = New Point(20, 20)
        Me.lblWelcome.Size = New Size(350, 25)
        Me.lblWelcome.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        Me.lblWelcome.Text = "Welcome Admin"


        Me.btnLogout.Location = New Point(450, 15)
        Me.btnLogout.Size = New Size(100, 35)
        Me.btnLogout.Text = "Đăng xuất"
        Me.btnLogout.BackColor = Color.IndianRed
        Me.btnLogout.ForeColor = Color.White


        Me.grpDashboard.Location = New Point(20, 70)
        Me.grpDashboard.Size = New Size(530, 260)
        Me.grpDashboard.Text = "Chức năng quản lý"

        Me.btnManageBooks.Location = New Point(30, 40)
        Me.btnManageBooks.Size = New Size(220, 80)
        Me.btnManageBooks.Text = "QUẢN LÝ THƯ VIỆN"
        Me.btnManageBooks.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Me.btnManageBooks.BackColor = Color.LightBlue

        Me.btnManageBorrow.Location = New Point(280, 40)
        Me.btnManageBorrow.Size = New Size(220, 80)
        Me.btnManageBorrow.Text = "QUẢN LÝ MƯỢN TRẢ"
        Me.btnManageBorrow.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Me.btnManageBorrow.BackColor = Color.LightGreen

        Me.btnManageUsers.Location = New Point(30, 150)
        Me.btnManageUsers.Size = New Size(220, 80)
        Me.btnManageUsers.Text = "QUẢN LÝ TÀI KHOẢN"
        Me.btnManageUsers.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Me.btnManageUsers.BackColor = Color.LightSalmon

        Me.btnStatistics.Location = New Point(280, 150)
        Me.btnStatistics.Size = New Size(220, 80)
        Me.btnStatistics.Text = "THỐNG KÊ DOANH THU"
        Me.btnStatistics.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Me.btnStatistics.BackColor = Color.LightGoldenrodYellow

        Me.grpDashboard.Controls.Add(Me.btnManageBooks)
        Me.grpDashboard.Controls.Add(Me.btnManageBorrow)
        Me.grpDashboard.Controls.Add(Me.btnManageUsers)
        Me.grpDashboard.Controls.Add(Me.btnStatistics)

        Me.ClientSize = New Size(580, 360)
        Me.Controls.Add(Me.lblWelcome)
        Me.Controls.Add(Me.btnLogout)
        Me.Controls.Add(Me.grpDashboard)
        Me.Text = "Admin Dashboard - Hệ Thống Quản Lý Thư Viện"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False

        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents lblWelcome As Label
    Friend WithEvents btnLogout As Button
    Friend WithEvents grpDashboard As GroupBox
    Friend WithEvents btnManageBooks As Button
    Friend WithEvents btnManageBorrow As Button
    Friend WithEvents btnManageUsers As Button
    Friend WithEvents btnStatistics As Button
End Class
