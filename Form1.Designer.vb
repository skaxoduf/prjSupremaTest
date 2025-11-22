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
        btnDLLUnLoad = New Button()
        txtDeviceID = New TextBox()
        btnEnrollUser = New Button()
        txtMemNm = New TextBox()
        Label1 = New Label()
        txtUserID = New TextBox()
        Label2 = New Label()
        txtImagePath = New TextBox()
        Label3 = New Label()
        rbLoadImage = New RadioButton()
        rbScanDevice = New RadioButton()
        btnTestImage = New Button()
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
        btnDeviceConn.Size = New Size(185, 44)
        btnDeviceConn.TabIndex = 0
        btnDeviceConn.Text = "2. IP주소와 포트로 장치연결"
        btnDeviceConn.UseVisualStyleBackColor = True
        ' 
        ' btnDeviceSearchConn
        ' 
        btnDeviceSearchConn.Location = New Point(391, 24)
        btnDeviceSearchConn.Name = "btnDeviceSearchConn"
        btnDeviceSearchConn.Size = New Size(442, 44)
        btnDeviceSearchConn.TabIndex = 0
        btnDeviceSearchConn.Text = "2. 주변장치 검색(이 방식은 잘 안됨)"
        btnDeviceSearchConn.UseVisualStyleBackColor = True
        ' 
        ' btnGetDeviceInfo
        ' 
        btnGetDeviceInfo.Location = New Point(860, 24)
        btnGetDeviceInfo.Name = "btnGetDeviceInfo"
        btnGetDeviceInfo.Size = New Size(444, 44)
        btnGetDeviceInfo.TabIndex = 0
        btnGetDeviceInfo.Text = "3. 장치 세팅정보 가져오기"
        btnGetDeviceInfo.UseVisualStyleBackColor = True
        ' 
        ' txtIP
        ' 
        txtIP.Location = New Point(185, 83)
        txtIP.Name = "txtIP"
        txtIP.Size = New Size(137, 23)
        txtIP.TabIndex = 1
        txtIP.Text = "192.168.0.156"
        ' 
        ' txtPort
        ' 
        txtPort.Location = New Point(185, 112)
        txtPort.Name = "txtPort"
        txtPort.Size = New Size(137, 23)
        txtPort.TabIndex = 1
        txtPort.Text = "51211"
        ' 
        ' lstDevices
        ' 
        lstDevices.FormattingEnabled = True
        lstDevices.ItemHeight = 15
        lstDevices.Location = New Point(392, 78)
        lstDevices.Name = "lstDevices"
        lstDevices.Size = New Size(327, 199)
        lstDevices.TabIndex = 2
        ' 
        ' btnConnectSelected
        ' 
        btnConnectSelected.Location = New Point(725, 78)
        btnConnectSelected.Name = "btnConnectSelected"
        btnConnectSelected.Size = New Size(108, 199)
        btnConnectSelected.TabIndex = 0
        btnConnectSelected.Text = "선택한 장치 연결(잘 안됨)"
        btnConnectSelected.UseVisualStyleBackColor = True
        ' 
        ' txtDeviceInfo
        ' 
        txtDeviceInfo.BackColor = Color.White
        txtDeviceInfo.Location = New Point(860, 83)
        txtDeviceInfo.Multiline = True
        txtDeviceInfo.Name = "txtDeviceInfo"
        txtDeviceInfo.ReadOnly = True
        txtDeviceInfo.ScrollBars = ScrollBars.Vertical
        txtDeviceInfo.Size = New Size(444, 194)
        txtDeviceInfo.TabIndex = 1
        ' 
        ' btnDLLUnLoad
        ' 
        btnDLLUnLoad.Location = New Point(1150, 739)
        btnDLLUnLoad.Name = "btnDLLUnLoad"
        btnDLLUnLoad.Size = New Size(130, 44)
        btnDLLUnLoad.TabIndex = 0
        btnDLLUnLoad.Text = "SDK 해제"
        btnDLLUnLoad.UseVisualStyleBackColor = True
        ' 
        ' txtDeviceID
        ' 
        txtDeviceID.Location = New Point(185, 191)
        txtDeviceID.Name = "txtDeviceID"
        txtDeviceID.Size = New Size(137, 23)
        txtDeviceID.TabIndex = 1
        ' 
        ' btnEnrollUser
        ' 
        btnEnrollUser.Location = New Point(25, 322)
        btnEnrollUser.Name = "btnEnrollUser"
        btnEnrollUser.Size = New Size(177, 44)
        btnEnrollUser.TabIndex = 0
        btnEnrollUser.Text = "4. 사용자 등록"
        btnEnrollUser.UseVisualStyleBackColor = True
        ' 
        ' txtMemNm
        ' 
        txtMemNm.Location = New Point(71, 379)
        txtMemNm.Name = "txtMemNm"
        txtMemNm.Size = New Size(93, 23)
        txtMemNm.TabIndex = 1
        txtMemNm.Text = "남태열"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(25, 382)
        Label1.Name = "Label1"
        Label1.Size = New Size(31, 15)
        Label1.TabIndex = 3
        Label1.Text = "이름"
        ' 
        ' txtUserID
        ' 
        txtUserID.Location = New Point(71, 411)
        txtUserID.Name = "txtUserID"
        txtUserID.Size = New Size(93, 23)
        txtUserID.TabIndex = 1
        txtUserID.Text = "1234"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(25, 411)
        Label2.Name = "Label2"
        Label2.Size = New Size(40, 15)
        Label2.TabIndex = 3
        Label2.Text = "UserId"
        ' 
        ' txtImagePath
        ' 
        txtImagePath.AllowDrop = True
        txtImagePath.Location = New Point(79, 455)
        txtImagePath.Name = "txtImagePath"
        txtImagePath.Size = New Size(291, 23)
        txtImagePath.TabIndex = 1
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(25, 458)
        Label3.Name = "Label3"
        Label3.Size = New Size(48, 15)
        Label3.TabIndex = 3
        Label3.Text = "jpg경로"
        ' 
        ' rbLoadImage
        ' 
        rbLoadImage.AutoSize = True
        rbLoadImage.Location = New Point(25, 507)
        rbLoadImage.Name = "rbLoadImage"
        rbLoadImage.Size = New Size(112, 19)
        rbLoadImage.TabIndex = 4
        rbLoadImage.TabStop = True
        rbLoadImage.Text = "JPG 파일로 등록"
        rbLoadImage.UseVisualStyleBackColor = True
        ' 
        ' rbScanDevice
        ' 
        rbScanDevice.AutoSize = True
        rbScanDevice.Location = New Point(165, 507)
        rbScanDevice.Name = "rbScanDevice"
        rbScanDevice.Size = New Size(101, 19)
        rbScanDevice.TabIndex = 4
        rbScanDevice.TabStop = True
        rbScanDevice.Text = "장비에서 촬영"
        rbScanDevice.UseVisualStyleBackColor = True
        ' 
        ' btnTestImage
        ' 
        btnTestImage.Location = New Point(25, 554)
        btnTestImage.Name = "btnTestImage"
        btnTestImage.Size = New Size(177, 44)
        btnTestImage.TabIndex = 0
        btnTestImage.Text = "jpg 유효성 테스트"
        btnTestImage.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1316, 795)
        Controls.Add(rbScanDevice)
        Controls.Add(rbLoadImage)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(lstDevices)
        Controls.Add(txtUserID)
        Controls.Add(txtImagePath)
        Controls.Add(txtMemNm)
        Controls.Add(txtDeviceID)
        Controls.Add(txtPort)
        Controls.Add(txtDeviceInfo)
        Controls.Add(txtIP)
        Controls.Add(btnGetDeviceInfo)
        Controls.Add(btnConnectSelected)
        Controls.Add(btnDeviceSearchConn)
        Controls.Add(btnDeviceConn)
        Controls.Add(btnDLLUnLoad)
        Controls.Add(btnTestImage)
        Controls.Add(btnEnrollUser)
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
    Friend WithEvents btnDLLUnLoad As Button
    Friend WithEvents txtDeviceID As TextBox
    Friend WithEvents btnEnrollUser As Button
    Friend WithEvents txtMemNm As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtUserID As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtImagePath As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents rbLoadImage As RadioButton
    Friend WithEvents rbScanDevice As RadioButton
    Friend WithEvents btnTestImage As Button

End Class
