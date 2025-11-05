<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        btnDLLLoad = New Button()
        btnDeviceConn = New Button()
        btnDeviceSearchConn = New Button()
        btnGetDeviceInfo = New Button()
        txtIP = New TextBox()
        txtPort = New TextBox()
        lstDevices = New ListBox()
        btnConnectSelected = New Button()
        txtDeviceInfo = New TextBox()
        SuspendLayout()
        ' 
        ' btnDLLLoad
        ' 
        btnDLLLoad.Location = New Point(25, 24)
        btnDLLLoad.Name = "btnDLLLoad"
        btnDLLLoad.Size = New Size(130, 44)
        btnDLLLoad.TabIndex = 0
        btnDLLLoad.Text = "1. SDK 초기화"
        btnDLLLoad.UseVisualStyleBackColor = True
        ' 
        ' btnDeviceConn
        ' 
        btnDeviceConn.Location = New Point(185, 24)
        btnDeviceConn.Name = "btnDeviceConn"
        btnDeviceConn.Size = New Size(169, 44)
        btnDeviceConn.TabIndex = 0
        btnDeviceConn.Text = "2. IP주소와 포트로 장치연결"
        btnDeviceConn.UseVisualStyleBackColor = True
        ' 
        ' btnDeviceSearchConn
        ' 
        btnDeviceSearchConn.Location = New Point(391, 24)
        btnDeviceSearchConn.Name = "btnDeviceSearchConn"
        btnDeviceSearchConn.Size = New Size(889, 44)
        btnDeviceSearchConn.TabIndex = 0
        btnDeviceSearchConn.Text = "2. 연결되어있는 장치를 검색"
        btnDeviceSearchConn.UseVisualStyleBackColor = True
        ' 
        ' btnGetDeviceInfo
        ' 
        btnGetDeviceInfo.Location = New Point(681, 78)
        btnGetDeviceInfo.Name = "btnGetDeviceInfo"
        btnGetDeviceInfo.Size = New Size(80, 259)
        btnGetDeviceInfo.TabIndex = 0
        btnGetDeviceInfo.Text = "장치 지원 기능 확인"
        btnGetDeviceInfo.UseVisualStyleBackColor = True
        ' 
        ' txtIP
        ' 
        txtIP.Location = New Point(185, 83)
        txtIP.Name = "txtIP"
        txtIP.Size = New Size(169, 23)
        txtIP.TabIndex = 1
        txtIP.Text = "192.168.0.242"
        ' 
        ' txtPort
        ' 
        txtPort.Location = New Point(185, 112)
        txtPort.Name = "txtPort"
        txtPort.Size = New Size(107, 23)
        txtPort.TabIndex = 1
        txtPort.Text = "51211"
        ' 
        ' lstDevices
        ' 
        lstDevices.FormattingEnabled = True
        lstDevices.ItemHeight = 15
        lstDevices.Location = New Point(391, 78)
        lstDevices.Name = "lstDevices"
        lstDevices.Size = New Size(194, 259)
        lstDevices.TabIndex = 2
        ' 
        ' btnConnectSelected
        ' 
        btnConnectSelected.Location = New Point(591, 78)
        btnConnectSelected.Name = "btnConnectSelected"
        btnConnectSelected.Size = New Size(84, 258)
        btnConnectSelected.TabIndex = 0
        btnConnectSelected.Text = "선택한 장치 연결"
        btnConnectSelected.UseVisualStyleBackColor = True
        ' 
        ' txtDeviceInfo
        ' 
        txtDeviceInfo.BackColor = Color.White
        txtDeviceInfo.Location = New Point(767, 78)
        txtDeviceInfo.Multiline = True
        txtDeviceInfo.Name = "txtDeviceInfo"
        txtDeviceInfo.ReadOnly = True
        txtDeviceInfo.ScrollBars = ScrollBars.Vertical
        txtDeviceInfo.Size = New Size(513, 259)
        txtDeviceInfo.TabIndex = 1
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1316, 795)
        Controls.Add(lstDevices)
        Controls.Add(txtPort)
        Controls.Add(txtDeviceInfo)
        Controls.Add(txtIP)
        Controls.Add(btnGetDeviceInfo)
        Controls.Add(btnConnectSelected)
        Controls.Add(btnDeviceSearchConn)
        Controls.Add(btnDeviceConn)
        Controls.Add(btnDLLLoad)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnDLLLoad As Button
    Friend WithEvents btnDeviceConn As Button
    Friend WithEvents btnDeviceSearchConn As Button
    Friend WithEvents btnGetDeviceInfo As Button
    Friend WithEvents txtIP As TextBox
    Friend WithEvents txtPort As TextBox
    Friend WithEvents lstDevices As ListBox
    Friend WithEvents btnConnectSelected As Button
    Friend WithEvents txtDeviceInfo As TextBox

End Class
