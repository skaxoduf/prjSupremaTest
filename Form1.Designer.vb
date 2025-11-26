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
        btnSetDeviceMode = New Button()
        btnUpdateFace = New Button()
        Label4 = New Label()
        btnGetUserList = New Button()
        txtUserList = New TextBox()
        chkAllUserDel = New CheckBox()
        btnSetServerMode = New Button()
        btnRemoveUser = New Button()
        btnStartMonitoring = New Button()
        txtRealTimeLog = New TextBox()
        btnRemoveAllDoors = New Button()
        btnGetDoorList = New Button()
        txtDoorList = New TextBox()
        btnDisableImageLog = New Button()
        btnDeepClean = New Button()
        btnSetGlobalAPB = New Button()
        btnSetDoor = New Button()
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
        btnGetDeviceInfo.Size = New Size(176, 44)
        btnGetDeviceInfo.TabIndex = 0
        btnGetDeviceInfo.Text = "3. 장치 세팅정보 확인"
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
        txtDeviceInfo.ScrollBars = ScrollBars.Vertical
        txtDeviceInfo.Size = New Size(444, 194)
        txtDeviceInfo.TabIndex = 1
        ' 
        ' btnDLLUnLoad
        ' 
        btnDLLUnLoad.Location = New Point(1174, 739)
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
        btnEnrollUser.Size = New Size(130, 44)
        btnEnrollUser.TabIndex = 0
        btnEnrollUser.Text = "4. 사용자 등록"
        btnEnrollUser.UseVisualStyleBackColor = True
        ' 
        ' txtMemNm
        ' 
        txtMemNm.Location = New Point(71, 379)
        txtMemNm.Name = "txtMemNm"
        txtMemNm.Size = New Size(63, 23)
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
        txtImagePath.Location = New Point(79, 443)
        txtImagePath.Multiline = True
        txtImagePath.Name = "txtImagePath"
        txtImagePath.Size = New Size(291, 40)
        txtImagePath.TabIndex = 1
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(25, 443)
        Label3.Name = "Label3"
        Label3.Size = New Size(48, 15)
        Label3.TabIndex = 3
        Label3.Text = "jpg경로"
        ' 
        ' rbLoadImage
        ' 
        rbLoadImage.AutoSize = True
        rbLoadImage.Checked = True
        rbLoadImage.Location = New Point(160, 336)
        rbLoadImage.Name = "rbLoadImage"
        rbLoadImage.Size = New Size(72, 19)
        rbLoadImage.TabIndex = 4
        rbLoadImage.TabStop = True
        rbLoadImage.Text = "JPG 등록"
        rbLoadImage.UseVisualStyleBackColor = True
        ' 
        ' rbScanDevice
        ' 
        rbScanDevice.AutoSize = True
        rbScanDevice.Location = New Point(249, 336)
        rbScanDevice.Name = "rbScanDevice"
        rbScanDevice.Size = New Size(125, 19)
        rbScanDevice.TabIndex = 4
        rbScanDevice.Text = "안면장비에서 촬영"
        rbScanDevice.UseVisualStyleBackColor = True
        ' 
        ' btnTestImage
        ' 
        btnTestImage.Location = New Point(258, 404)
        btnTestImage.Name = "btnTestImage"
        btnTestImage.Size = New Size(112, 35)
        btnTestImage.TabIndex = 0
        btnTestImage.Text = "jpg 유효성 테스트"
        btnTestImage.UseVisualStyleBackColor = True
        ' 
        ' btnSetDeviceMode
        ' 
        btnSetDeviceMode.Location = New Point(391, 321)
        btnSetDeviceMode.Name = "btnSetDeviceMode"
        btnSetDeviceMode.Size = New Size(422, 42)
        btnSetDeviceMode.TabIndex = 0
        btnSetDeviceMode.Text = "장비 인증 모드로 세팅하기 (장비자체에 얼굴있으면 무조건 도어접점)"
        btnSetDeviceMode.UseVisualStyleBackColor = True
        ' 
        ' btnUpdateFace
        ' 
        btnUpdateFace.Location = New Point(25, 494)
        btnUpdateFace.Name = "btnUpdateFace"
        btnUpdateFace.Size = New Size(130, 44)
        btnUpdateFace.TabIndex = 0
        btnUpdateFace.Text = "5. 사용자 수정"
        btnUpdateFace.UseVisualStyleBackColor = True
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(185, 170)
        Label4.Name = "Label4"
        Label4.Size = New Size(71, 15)
        Label4.TabIndex = 3
        Label4.Text = "장치 아이디"
        ' 
        ' btnGetUserList
        ' 
        btnGetUserList.Location = New Point(25, 544)
        btnGetUserList.Name = "btnGetUserList"
        btnGetUserList.Size = New Size(130, 44)
        btnGetUserList.TabIndex = 0
        btnGetUserList.Text = "7. 사용자 리스트"
        btnGetUserList.UseVisualStyleBackColor = True
        ' 
        ' txtUserList
        ' 
        txtUserList.BackColor = Color.White
        txtUserList.Location = New Point(25, 594)
        txtUserList.Multiline = True
        txtUserList.Name = "txtUserList"
        txtUserList.ScrollBars = ScrollBars.Vertical
        txtUserList.Size = New Size(349, 189)
        txtUserList.TabIndex = 1
        ' 
        ' chkAllUserDel
        ' 
        chkAllUserDel.AutoSize = True
        chkAllUserDel.Location = New Point(240, 544)
        chkAllUserDel.Name = "chkAllUserDel"
        chkAllUserDel.Size = New Size(74, 19)
        chkAllUserDel.TabIndex = 5
        chkAllUserDel.Text = "전체삭제"
        chkAllUserDel.UseVisualStyleBackColor = True
        ' 
        ' btnSetServerMode
        ' 
        btnSetServerMode.Location = New Point(390, 411)
        btnSetServerMode.Name = "btnSetServerMode"
        btnSetServerMode.Size = New Size(423, 42)
        btnSetServerMode.TabIndex = 0
        btnSetServerMode.Text = "서버 인증 모드로 세팅하기 (장비에서 인증후 서버에서 판단후 도어접점)     (장비가 얼굴템플릿을 넘겨줌 - 개발불가)"
        btnSetServerMode.UseVisualStyleBackColor = True
        ' 
        ' btnRemoveUser
        ' 
        btnRemoveUser.Location = New Point(240, 494)
        btnRemoveUser.Name = "btnRemoveUser"
        btnRemoveUser.Size = New Size(130, 44)
        btnRemoveUser.TabIndex = 0
        btnRemoveUser.Text = "6. 사용자 삭제"
        btnRemoveUser.UseVisualStyleBackColor = True
        ' 
        ' btnStartMonitoring
        ' 
        btnStartMonitoring.Location = New Point(390, 459)
        btnStartMonitoring.Name = "btnStartMonitoring"
        btnStartMonitoring.Size = New Size(268, 43)
        btnStartMonitoring.TabIndex = 6
        btnStartMonitoring.Text = "== 장비 실시간 모니터링 시작 =="
        btnStartMonitoring.UseVisualStyleBackColor = True
        ' 
        ' txtRealTimeLog
        ' 
        txtRealTimeLog.BackColor = Color.White
        txtRealTimeLog.Location = New Point(390, 508)
        txtRealTimeLog.Multiline = True
        txtRealTimeLog.Name = "txtRealTimeLog"
        txtRealTimeLog.ScrollBars = ScrollBars.Vertical
        txtRealTimeLog.Size = New Size(421, 275)
        txtRealTimeLog.TabIndex = 1
        ' 
        ' btnRemoveAllDoors
        ' 
        btnRemoveAllDoors.Location = New Point(996, 322)
        btnRemoveAllDoors.Name = "btnRemoveAllDoors"
        btnRemoveAllDoors.Size = New Size(137, 43)
        btnRemoveAllDoors.TabIndex = 7
        btnRemoveAllDoors.Text = "== 도어 전체 삭제 =="
        btnRemoveAllDoors.UseVisualStyleBackColor = True
        ' 
        ' btnGetDoorList
        ' 
        btnGetDoorList.Location = New Point(860, 322)
        btnGetDoorList.Name = "btnGetDoorList"
        btnGetDoorList.Size = New Size(130, 43)
        btnGetDoorList.TabIndex = 7
        btnGetDoorList.Text = "== 도어 리스트 =="
        btnGetDoorList.UseVisualStyleBackColor = True
        ' 
        ' txtDoorList
        ' 
        txtDoorList.BackColor = Color.White
        txtDoorList.Location = New Point(860, 371)
        txtDoorList.Multiline = True
        txtDoorList.Name = "txtDoorList"
        txtDoorList.ReadOnly = True
        txtDoorList.ScrollBars = ScrollBars.Vertical
        txtDoorList.Size = New Size(266, 176)
        txtDoorList.TabIndex = 1
        ' 
        ' btnDisableImageLog
        ' 
        btnDisableImageLog.Enabled = False
        btnDisableImageLog.Location = New Point(665, 459)
        btnDisableImageLog.Name = "btnDisableImageLog"
        btnDisableImageLog.Size = New Size(148, 43)
        btnDisableImageLog.TabIndex = 7
        btnDisableImageLog.Text = "이미지 로그 비활성화"
        btnDisableImageLog.UseVisualStyleBackColor = True
        ' 
        ' btnDeepClean
        ' 
        btnDeepClean.Location = New Point(1039, 740)
        btnDeepClean.Name = "btnDeepClean"
        btnDeepClean.Size = New Size(129, 43)
        btnDeepClean.TabIndex = 7
        btnDeepClean.Text = "장치 재부팅"
        btnDeepClean.UseVisualStyleBackColor = True
        ' 
        ' btnSetGlobalAPB
        ' 
        btnSetGlobalAPB.Location = New Point(390, 365)
        btnSetGlobalAPB.Name = "btnSetGlobalAPB"
        btnSetGlobalAPB.Size = New Size(423, 42)
        btnSetGlobalAPB.TabIndex = 0
        btnSetGlobalAPB.Text = "복합 인증 모드로 세팅하기 (장비에서 인증후 서버에서 판단후 도어접점)       (장비가 UserID를 넘겨줌 - 개발가능)"
        btnSetGlobalAPB.UseVisualStyleBackColor = True
        ' 
        ' btnSetDoor
        ' 
        btnSetDoor.Location = New Point(1139, 322)
        btnSetDoor.Name = "btnSetDoor"
        btnSetDoor.Size = New Size(130, 43)
        btnSetDoor.TabIndex = 7
        btnSetDoor.Text = "== 도어 등록 =="
        btnSetDoor.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1316, 795)
        Controls.Add(btnGetDoorList)
        Controls.Add(btnDisableImageLog)
        Controls.Add(btnDeepClean)
        Controls.Add(btnSetDoor)
        Controls.Add(btnRemoveAllDoors)
        Controls.Add(btnStartMonitoring)
        Controls.Add(chkAllUserDel)
        Controls.Add(rbScanDevice)
        Controls.Add(rbLoadImage)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label4)
        Controls.Add(Label1)
        Controls.Add(lstDevices)
        Controls.Add(txtUserID)
        Controls.Add(txtImagePath)
        Controls.Add(txtMemNm)
        Controls.Add(txtDeviceID)
        Controls.Add(txtPort)
        Controls.Add(txtUserList)
        Controls.Add(txtDoorList)
        Controls.Add(txtRealTimeLog)
        Controls.Add(txtDeviceInfo)
        Controls.Add(txtIP)
        Controls.Add(btnGetDeviceInfo)
        Controls.Add(btnConnectSelected)
        Controls.Add(btnDeviceSearchConn)
        Controls.Add(btnDeviceConn)
        Controls.Add(btnDLLUnLoad)
        Controls.Add(btnTestImage)
        Controls.Add(btnRemoveUser)
        Controls.Add(btnUpdateFace)
        Controls.Add(btnGetUserList)
        Controls.Add(btnSetGlobalAPB)
        Controls.Add(btnSetServerMode)
        Controls.Add(btnSetDeviceMode)
        Controls.Add(btnEnrollUser)
        Controls.Add(btnDLLLoad)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
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
    Friend WithEvents btnSetDeviceMode As Button
    Friend WithEvents btnUpdateFace As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents btnGetUserList As Button
    Friend WithEvents txtUserList As TextBox
    Friend WithEvents chkAllUserDel As CheckBox
    Friend WithEvents btnSetServerMode As Button
    Friend WithEvents btnRemoveUser As Button
    Friend WithEvents btnStartMonitoring As Button
    Friend WithEvents txtRealTimeLog As TextBox
    Friend WithEvents btnRemoveAllDoors As Button
    Friend WithEvents btnGetDoorList As Button
    Friend WithEvents txtDoorList As TextBox
    Friend WithEvents btnDisableImageLog As Button
    Friend WithEvents btnDeepClean As Button
    Friend WithEvents btnSetGlobalAPB As Button
    Friend WithEvents btnSetDoor As Button

End Class
