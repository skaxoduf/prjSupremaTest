Imports System.IO
Imports System.Net
Imports System.Runtime.InteropServices
Imports prjSupremaTest.Suprema
Imports prjSupremaTest.Suprema.API
Imports Suprema
Imports Suprema.API

Public Class Form1

    Private sdkContext As IntPtr = IntPtr.Zero  ' SDK 컨텍스트 핸들
    Private connectedDeviceId As UInteger  ' 연결된 장치 ID
    'Private connectedDeviceIds As New List(Of UInteger)()  ' 연결된 장치 ID 목록
    Private cbOnLogReceived As API.OnLogReceived = Nothing   ' 장비 이벤트 받는 콜백함수 선언
    Private cbOnVerifyUser As API.OnVerifyUser = Nothing     ' 1:1 인증용 (ID+얼굴)  ' 서버인증모드에서 사용하는 콜백함수 (대양프로그램은 사용안함)
    Private cbOnIdentifyUser As API.OnIdentifyUser = Nothing ' 1:N 인증용 (얼굴만)   ' 서버인증모드에서 사용하는 콜백함수 (대양프로그램은 사용안함)
    Private cbGlobalAPB As API.OnCheckGlobalAPBViolation = Nothing  ' 안티패스백 경보가 발생했을 때 글로벌 판정을 위한 콜백 함수입니다. (대양프로그램 사용)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateImageUI()
    End Sub

    Private Sub rbLoadImage_CheckedChanged(sender As Object, e As EventArgs) Handles rbLoadImage.CheckedChanged
        UpdateImageUI()
    End Sub

    Private Sub UpdateImageUI()
        txtImagePath.Enabled = rbLoadImage.Checked
        If Not rbLoadImage.Checked Then
            txtImagePath.Clear()
        End If
    End Sub

    Private Sub txtImagePath_DragEnter(sender As Object, e As DragEventArgs) Handles txtImagePath.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub txtImagePath_DragDrop(sender As Object, e As DragEventArgs) Handles txtImagePath.DragDrop
        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
        If files.Length > 0 Then
            Dim filePath As String = files(0)
            Dim ext As String = System.IO.Path.GetExtension(filePath).ToLower()
            If ext = ".jpg" OrElse ext = ".jpeg" OrElse ext = ".png" Then
                txtImagePath.Text = filePath
            Else
                MessageBox.Show("이미지 확장자(jpg, png)만 가능!!!")
            End If
        End If
    End Sub
    Private Sub btnDLLLoad_Click(sender As Object, e As EventArgs) Handles btnDLLLoad.Click

        If sdkContext <> IntPtr.Zero Then
            MessageBox.Show("SDK가 이미 초기화되었습니다.")
            Return
        End If

        Dim result As BS2ErrorCode
        sdkContext = BS2_AllocateContext()

        ' sdkContext : &H00000197843CCB90  정상이라는 뜻 
        If sdkContext <> IntPtr.Zero Then
            result = CType(BS2_Initialize(sdkContext), BS2ErrorCode)
            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                BS2_SetKeepAliveTimeout(sdkContext, 30 * 1000)  ' 30초에 한번씩 장치연결 정상적인지 확인하는부분
                MessageBox.Show("DLL 초기화 성공!")
            Else
                BS2_ReleaseContext(sdkContext)
                sdkContext = IntPtr.Zero
                MessageBox.Show("DLL 초기화 실패. 오류 코드: " & result)
            End If
        Else
            MessageBox.Show("DLL 컨텍스트 할당 실패")
        End If

    End Sub

    Private Sub btnDeviceConn_Click(sender As Object, e As EventArgs) Handles btnDeviceConn.Click

        'BioStar 애플리케이션과 장치는 server mode와 direct mode로 연결할 수 있습니다.
        'server mode는 장치가 BioStar 애플리케이션으로 신호를 보내서 연결하는 방식이고,
        'direct mode는 BioStar 애플리케이션이 장치로 신호를 보내서 연결하는 방식입니다.
        '장치는 direct mode가 초기값으로 설정되어 있으며,
        'direct mode로 연결하는 방법은 아래와 같습니다.


        ' IP 주소와 Port를 이미 알고 있는 경우
        ' sdkContext : &H00000197843CCB90  정상이라는 뜻 
        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
        End If

        ' 기존에 연결된 장치가 있다면 먼저 연결을 해제
        If connectedDeviceId <> 0 Then
            BS2_DisconnectDevice(sdkContext, connectedDeviceId)
            connectedDeviceId = 0
        End If

        Dim deviceAddress As String = txtIP.Text.Trim
        Dim devicePort As UShort = txtPort.Text.Trim
        Dim deviceId As UInteger = 0
        Dim result As BS2ErrorCode
        Dim ptrDeviceAddress As IntPtr = IntPtr.Zero

        Try
            ptrDeviceAddress = Marshal.StringToHGlobalAnsi(deviceAddress)

            result = CType(BS2_ConnectDeviceViaIP(sdkContext, ptrDeviceAddress, devicePort, deviceId), BS2ErrorCode)

            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                connectedDeviceId = deviceId
                txtDeviceID.Text = deviceId.ToString()

                MessageBox.Show("장치 연결 성공! Device ID: " & deviceId)

                'FixDeviceConfig(deviceId)      ' 1. Global APB & 서버매칭 끄기 (3초 딜레이 해결)
                'DisableImageLog(deviceId)      ' 2. 이미지 로그 끄기 (4~5초 릴레이 딜레이 해결)
                'SetDeviceVolume(connectedDeviceId, 20)    ' 장비 볼륨 설정 (100이 최대)
                'CheckStatusConfig(deviceId)   ' 장비의 부저/LED 기능이 켜져 있는지 확인하는 함수

                btnSetGlobalAPB_Click(Nothing, Nothing)
            Else
                MessageBox.Show($"장치 연결 실패.{vbCrLf}오류 내용: {result}{vbCrLf}오류 코드: {CInt(result)}")
            End If
        Catch ex As Exception
            MessageBox.Show("오류 발생: " & ex.Message)
        Finally
            If ptrDeviceAddress <> IntPtr.Zero Then
                Marshal.FreeHGlobal(ptrDeviceAddress)
            End If
        End Try

    End Sub

    Private Sub btnDeviceSearchConn_Click(sender As Object, e As EventArgs) Handles btnDeviceSearchConn.Click

        '================ 이거는 권장하는 방식이 아님 ================

        'BioStar 애플리케이션과 장치는 server mode와 direct mode로 연결할 수 있습니다.
        'server mode는 장치가 BioStar 애플리케이션으로 신호를 보내서 연결하는 방식이고,
        'direct mode는 BioStar 애플리케이션이 장치로 신호를 보내서 연결하는 방식입니다.
        '장치는 direct mode가 초기값으로 설정되어 있으며,
        'direct mode로 연결하는 방법은 아래와 같습니다.


        ' 네트워크망에 물려있는 장치를 검색
        ' sdkContext : &H00000197843CCB90  정상이라는 뜻 
        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
        End If

        lstDevices.Items.Clear()

        Dim timeoutResult As BS2ErrorCode = CType(BS2_SetDeviceSearchingTimeout(sdkContext, 5), BS2ErrorCode)  ' 5초 동안 장치 검색
        If timeoutResult <> BS2ErrorCode.BS_SDK_SUCCESS Then
            Debug.WriteLine("타임아웃 설정 실패: " & timeoutResult.ToString())
        End If

        Dim result As BS2ErrorCode
        result = CType(BS2_SearchDevices(sdkContext), BS2ErrorCode)

        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            Dim deviceListObj = IntPtr.Zero
            Dim numDevice As UInteger = 0

            ' 검색된 장치 목록 가져오기
            result = CType(BS2_GetDevices(sdkContext, deviceListObj, numDevice), BS2ErrorCode)

            If result = BS2ErrorCode.BS_SDK_SUCCESS AndAlso numDevice > 0 Then

                MessageBox.Show($"총 {numDevice}대의 장치를 찾음!!")

                ' --- 찾은 장치갯수만큼 루프돌면서 리스트박스에 추가 
                For idx As Integer = 0 To numDevice - 1
                    Dim currentDeviceId As UInteger = Marshal.ReadInt32(deviceListObj, idx * 4)

                    ' 장치 상세 정보 가져오기
                    Dim deviceInfo As BS2SimpleDeviceInfo
                    Dim infoResult As BS2ErrorCode = CType(API.BS2_GetDeviceInfo(sdkContext, currentDeviceId, deviceInfo), BS2ErrorCode)

                    If infoResult = BS2ErrorCode.BS_SDK_SUCCESS Then
                        Dim deviceType As BS2DeviceTypeEnum = CType(deviceInfo.type, BS2DeviceTypeEnum)
                        Dim deviceName As String = If(API.productNameDictionary.ContainsKey(deviceType), API.productNameDictionary(deviceType), "Unknown")
                        Dim ip As String = New IPAddress(BitConverter.GetBytes(deviceInfo.ipv4Address)).ToString()

                        Dim displayText As String = $"ID: {currentDeviceId} | IP: {ip} | Type: {deviceName}"
                        lstDevices.Items.Add(displayText)
                    Else
                        lstDevices.Items.Add($"ID: {currentDeviceId} | (정보 가져오기 실패)")
                    End If
                Next
                BS2_ReleaseObject(deviceListObj)
            ElseIf numDevice = 0 Then
                MessageBox.Show("검색된 장치가 없습니다.")
            Else
                MessageBox.Show("장치 목록 가져오기 실패. 오류 코드: 0x" & result.ToString("X"))
            End If
        Else
            MessageBox.Show("장치 검색 시작 실패. 오류 코드: 0x" & result.ToString("X"))
        End If

    End Sub
    Private Sub btnConnectSelected_Click(sender As Object, e As EventArgs) Handles btnConnectSelected.Click

        '리스트박스에서 선택한 장치 연결

        ' sdkContext : &H00000197843CCB90  정상이라는 뜻 
        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
        End If

        If lstDevices.SelectedItem Is Nothing Then
            MessageBox.Show("먼저 목록에서 연결할 장치를 선택하세요.")
            Return
        End If

        ' 기존에 연결된 장치가 있다면 먼저 연결을 해제
        If connectedDeviceId <> 0 Then
            BS2_DisconnectDevice(sdkContext, connectedDeviceId)
            connectedDeviceId = 0
        End If

        ' 선택된 항목의 문자열을 가져옴 
        Dim selectedString = lstDevices.SelectedItem.ToString

        ' 문자열에서 ID만 파싱
        Try
            Dim parts = selectedString.Split({"ID: ", " |"}, StringSplitOptions.RemoveEmptyEntries)
            Dim deviceIdToConnect = UInteger.Parse(parts(0).Trim)

            Dim result As BS2ErrorCode = BS2_ConnectDevice(sdkContext, deviceIdToConnect)
            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                connectedDeviceId = deviceIdToConnect
                MessageBox.Show("장치 연결 성공! ID: " & deviceIdToConnect)
            Else
                MessageBox.Show($"장치 연결 실패.{vbCrLf}오류 내용: {result}{vbCrLf}오류 코드: {CInt(result)}")
            End If

        Catch ex As Exception
            MessageBox.Show("선택된 장치 ID를 파싱하는 데 실패했습니다. " & ex.Message)
        End Try

    End Sub

    Private Sub btnGetDeviceInfo_Click(sender As Object, e As EventArgs) Handles btnGetDeviceInfo.Click

        ' 1. 연결 상태 확인
        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        txtDeviceInfo.Clear()
        txtDeviceInfo.AppendText("=== [장비 종합 상태 점검 시작] ===" & vbCrLf)
        Application.DoEvents()

        Dim sb As New System.Text.StringBuilder()
        Dim result As BS2ErrorCode

        ' =========================================================
        ' 1. 기본 하드웨어 정보 (BS2_GetDeviceInfo)
        ' =========================================================
        Dim deviceInfo As BS2SimpleDeviceInfo
        result = API.BS2_GetDeviceInfo(sdkContext, connectedDeviceId, deviceInfo)

        sb.AppendLine(vbCrLf & "== 1. 기본 하드웨어 정보")
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            sb.AppendLine($" - Device ID: {deviceInfo.id}")

            Dim deviceType As BS2DeviceTypeEnum = CType(deviceInfo.type, BS2DeviceTypeEnum)
            Dim deviceName As String = "Unknown"
            If API.productNameDictionary.ContainsKey(deviceType) Then
                deviceName = API.productNameDictionary(deviceType)
            End If
            sb.AppendLine($" - 모델명: {deviceName} (Type: {deviceInfo.type})")
            sb.AppendLine($" - 접속 모드: {If(deviceInfo.connectionMode = 1, "장치 -> 서버", "서버 -> 장치")}")
            sb.AppendLine($" - IP 주소: {New IPAddress(BitConverter.GetBytes(deviceInfo.ipv4Address))}")
            sb.AppendLine($" - 포트: {deviceInfo.port}")
        Else
            sb.AppendLine($" [조회 실패] 오류 코드: {result}")
        End If

        ' =========================================================
        ' 2. 장비 능력치 (BS2_GetDeviceCapabilities)
        ' =========================================================
        Dim cap As New BS2DeviceCapabilities()
        result = API.BS2_GetDeviceCapabilities(sdkContext, connectedDeviceId, cap)

        sb.AppendLine(vbCrLf & "== 2. 장비 능력치 (기능 지원 여부)")
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            Dim isFaceEx As Boolean = (cap.systemSupported And BS2CapabilitySystemSupport.SYSTEM_SUPPORT_FACEEX) > 0
            sb.AppendLine($" - 신형 얼굴인식(Visual Face) 지원: {isFaceEx}")
            sb.AppendLine($" - 구형 얼굴인식(IR Face) 지원: {Convert.ToBoolean(deviceInfo.faceSupported)}")
            sb.AppendLine($" - 최대 사용자 수: {cap.maxUsers:N0}")
            sb.AppendLine($" - 최대 얼굴 템플릿 수: {cap.maxFaces:N0}")
            sb.AppendLine($" - 최대 로그 저장 수: {cap.maxEventLogs:N0}")
        Else
            sb.AppendLine($" [조회 실패] 오류 코드: {result}")
        End If

        ' =========================================================
        ' 3. 인증 설정 (BS2_GetAuthConfig)
        ' =========================================================
        Dim authConfig As New BS2AuthConfig()
        result = API.BS2_GetAuthConfig(sdkContext, connectedDeviceId, authConfig)

        sb.AppendLine(vbCrLf & "== 3. 인증 설정 (운영 모드)")
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            sb.AppendLine($" - 서버 매칭 모드 (useServerMatching): {authConfig.useServerMatching}")
            If authConfig.useServerMatching = 1 Then
                sb.AppendLine("   => [주의] 1 (서버 인증): 장비가 서버 응답을 기다립니다.")
            Else
                sb.AppendLine("   => [정상] 0 (장비 인증): 장비가 스스로 인증합니다.")
            End If

            sb.AppendLine($" - 전역 APB (useGlobalAPB): {authConfig.useGlobalAPB}")
            If authConfig.useGlobalAPB = 1 Then
                sb.AppendLine("   => [확인] 1 (서버 확인): 인증 후 서버에 출입 권한을 물어봅니다.")
            Else
                sb.AppendLine("   => [확인] 0 (즉시 통과): 서버 확인 없이 즉시 통과시킵니다.")
            End If
        Else
            sb.AppendLine($" [조회 실패] 오류 코드: {result}")
        End If

        ' =========================================================
        ' 4. 이벤트 설정 (BS2_GetEventConfig)
        ' =========================================================
        Dim eventConfig As New BS2EventConfig()
        eventConfig.imageEventFilter = New BS2ImageEventFilter(BS2Environment.BS2_EVENT_MAX_IMAGE_CODE_COUNT - 1) {}

        result = API.BS2_GetEventConfig(sdkContext, connectedDeviceId, eventConfig)

        sb.AppendLine(vbCrLf & "== 4. 이벤트 설정 (이미지 로그)")
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            sb.AppendLine($" - 이미지 전송 필터 개수: {eventConfig.numImageEventFilter}")
            If eventConfig.numImageEventFilter > 0 Then
                sb.AppendLine("   => [주의] 켜짐: 대용량 사진을 전송하느라 릴레이 반응이 느려질 수 있습니다.")
            Else
                sb.AppendLine("   => [정상] 꺼짐: 텍스트 로그만 전송하여 반응 속도가 빠릅니다.")
            End If
        Else
            sb.AppendLine($" [조회 실패] 오류 코드: {result}")
        End If

        ' =========================================================
        ' 5. 상태 설정 (BS2_GetStatusConfig)
        ' =========================================================
        Dim statusConfig As New BS2StatusConfig()
        statusConfig.led = New BS2LedStatusConfig(BS2Environment.BS2_DEVICE_STATUS_NUM - 1) {}
        statusConfig.buzzer = New BS2BuzzerStatusConfig(BS2Environment.BS2_DEVICE_STATUS_NUM - 1) {}

        result = API.BS2_GetStatusConfig(sdkContext, connectedDeviceId, statusConfig)

        sb.AppendLine(vbCrLf & "== 5. 상태 설정 (소리/LED)")
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            sb.AppendLine($" - LED 기능 사용 여부: {statusConfig.led(0).enabled}")
            Dim buzzerOn As Byte = statusConfig.buzzer(0).enabled
            sb.AppendLine($" - 부저(소리) 기능 사용 여부: {buzzerOn}")
            If buzzerOn = 0 Then
                sb.AppendLine("   => [주의] 부저 기능이 꺼져 있어 명령을 보내도 소리가 나지 않습니다.")
            Else
                sb.AppendLine("   => [정상] 부저 기능이 켜져 있습니다.")
            End If
        Else
            sb.AppendLine($" [조회 실패] 오류 코드: {result}")
        End If

        ' =========================================================
        ' 6. 시스템 설정 (BS2_GetSystemConfig)
        ' =========================================================
        Dim sysConfig As New BS2SystemConfig()
        ' 안전을 위한 배열 초기화
        sysConfig.notUsed = New Byte(16 * 16 * 3 - 1) {}
        sysConfig.reserved = New Byte(1) {}
        sysConfig.reserved2 = New Byte(15) {}

        result = API.BS2_GetSystemConfig(sdkContext, connectedDeviceId, sysConfig)

        sb.AppendLine(vbCrLf & "== 6. 시스템 설정 (System Config)")
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            sb.AppendLine($" - 타임존(Timezone): {sysConfig.timezone}")
            sb.AppendLine($" - 카메라 주파수(Camera Freq): {sysConfig.cameraFrequency} Hz")
            sb.AppendLine($" - 보안 탬퍼(Secure Tamper): {sysConfig.secureTamper}")
            sb.AppendLine($" - 카드 동작 마스크: {sysConfig.useCardOperationMask}")
        Else
            sb.AppendLine($" [조회 실패] 오류 코드: {result}")
        End If

        ' =========================================================
        ' 7. 공장 초기 설정 (BS2_GetFactoryConfig)
        ' =========================================================
        Dim factoryConfig As New BS2FactoryConfig()
        factoryConfig.macAddr = New Byte(BS2Environment.BS2_MAC_ADDR_LEN - 1) {}
        factoryConfig.modelName = New Byte(BS2Environment.BS2_MODEL_NAME_LEN - 1) {}
        factoryConfig.kernelRev = New Byte(BS2Environment.BS2_KERNEL_REV_LEN - 1) {}
        factoryConfig.bscoreRev = New Byte(BS2Environment.BS2_BSCORE_REV_LEN - 1) {}
        factoryConfig.firmwareRev = New Byte(BS2Environment.BS2_FIRMWARE_REV_LEN - 1) {}
        factoryConfig.reserved2 = New Byte(31) {}

        result = API.BS2_GetFactoryConfig(sdkContext, connectedDeviceId, factoryConfig)

        sb.AppendLine(vbCrLf & "== 7. 공장 초기 정보 (Factory Config)")
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            Dim mac As String = BitConverter.ToString(factoryConfig.macAddr)
            Dim model As String = System.Text.Encoding.ASCII.GetString(factoryConfig.modelName).TrimEnd(Chr(0))

            Dim major As Byte = factoryConfig.firmwareVer.major
            Dim minor As Byte = factoryConfig.firmwareVer.minor
            Dim ext As Byte = factoryConfig.firmwareVer.ext
            Dim buildDate As String = System.Text.Encoding.ASCII.GetString(factoryConfig.firmwareRev).TrimEnd(Chr(0))
            sb.AppendLine($" - MAC Address: {mac}")
            sb.AppendLine($" - 모델명: {model}")
            sb.AppendLine($" - 펌웨어 버전: v{major}.{minor}.{ext} (Build: {buildDate})")
        Else
            sb.AppendLine($" [조회 실패] 오류 코드: {result}")
        End If

        ' =========================================================
        ' 8. 디스플레이 설정 (BS2_GetDisplayConfig)
        ' =========================================================
        Dim displayConfig As New BS2DisplayConfig()
        displayConfig.shortcutHome = New Byte(BS2Environment.BS2_MAX_SHORTCUT_HOME - 1) {}
        displayConfig.tnaIcon = New Byte(BS2Environment.BS2_MAX_TNA_KEY - 1) {}
        displayConfig.reserved3 = New Byte(26) {}

        result = API.BS2_GetDisplayConfig(sdkContext, connectedDeviceId, displayConfig)

        sb.AppendLine(vbCrLf & "== 8. 디스플레이 설정 (Display Config)")
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            sb.AppendLine($" - 볼륨(Volume): {displayConfig.volume}")
            sb.AppendLine($" - 배경 테마: {displayConfig.bgTheme}")
            sb.AppendLine($" - 메뉴 타임아웃: {displayConfig.menuTimeout}초")
            sb.AppendLine($" - 화면보호기 사용: {displayConfig.useScreenSaver}")
        Else
            sb.AppendLine($" [조회 실패] 오류 코드: {result}")
        End If

        ' =========================================================
        ' 9. 얼굴 설정 (BS2_GetFaceConfig)
        ' =========================================================
        Dim faceConfig As New BS2FaceConfig()
        faceConfig.reserved = New Byte(12) {}

        result = API.BS2_GetFaceConfig(sdkContext, connectedDeviceId, faceConfig)

        sb.AppendLine(vbCrLf & "== 9. 얼굴 설정 (Face Config)")
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            sb.AppendLine($" - 보안 등급(Security Level): {faceConfig.securityLevel}")
            sb.AppendLine($" - 조명 조건(Light Condition): {faceConfig.lightCondition}")
            sb.AppendLine($" - 등록 임계값(Enroll Threshold): {faceConfig.enrollThreshold}")
            sb.AppendLine($" - 감지 민감도(Detect Sensitivity): {faceConfig.detectSensitivity}")
            sb.AppendLine($" - LFD(가짜 얼굴 감지) 레벨: {faceConfig.lfdLevel}")
        Else
            sb.AppendLine($" [조회 실패] 오류 코드: {result}")
        End If

        ' =========================================================
        ' 10. IP 설정 (BS2_GetIPConfig)
        ' =========================================================
        Dim ipConfig As New BS2IpConfig()
        ipConfig.ipAddress = New Byte(BS2Environment.BS2_IPV4_ADDR_SIZE - 1) {}
        ipConfig.gateway = New Byte(BS2Environment.BS2_IPV4_ADDR_SIZE - 1) {}
        ipConfig.subnetMask = New Byte(BS2Environment.BS2_IPV4_ADDR_SIZE - 1) {}
        ipConfig.serverAddr = New Byte(BS2Environment.BS2_IPV4_ADDR_SIZE - 1) {}
        ipConfig.reserved3 = New Byte(29) {}

        result = API.BS2_GetIPConfig(sdkContext, connectedDeviceId, ipConfig)

        sb.AppendLine(vbCrLf & "== 10. IP 설정 (IP Config)")
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            sb.AppendLine($" - DHCP 사용: {ipConfig.useDHCP}")
            sb.AppendLine($" - IP Addr: {System.Text.Encoding.UTF8.GetString(ipConfig.ipAddress).TrimEnd(Chr(0))}")
            sb.AppendLine($" - Gateway: {System.Text.Encoding.UTF8.GetString(ipConfig.gateway).TrimEnd(Chr(0))}")
            sb.AppendLine($" - Subnet: {System.Text.Encoding.UTF8.GetString(ipConfig.subnetMask).TrimEnd(Chr(0))}")
            sb.AppendLine($" - Server IP: {System.Text.Encoding.UTF8.GetString(ipConfig.serverAddr).TrimEnd(Chr(0))}")
            sb.AppendLine($" - Server Port: {ipConfig.serverPort}")
        Else
            sb.AppendLine($" [조회 실패] 오류 코드: {result}")
        End If



        ' =========================================================
        ' 11. 관리자 설정 (Admin Config)
        ' =========================================================
        ' 위에서 사용한 authConfig 변수를 재활용하거나 새로 선언해서 조회합니다.
        ' 확실하게 하기 위해 새로 조회합니다.
        Dim authConfigAdmin As New BS2AuthConfig()
        result = API.BS2_GetAuthConfig(sdkContext, connectedDeviceId, authConfigAdmin)

        sb.AppendLine(vbCrLf & "== 11. 관리자 설정 (Admin Config)")
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            sb.AppendLine($" - 등록된 관리자 수: {authConfigAdmin.numOperators}")

            If authConfigAdmin.numOperators > 0 Then
                For i As Integer = 0 To authConfigAdmin.numOperators - 1
                    Dim op As BS2AuthOperatorLevel = authConfigAdmin.operators(i)
                    Dim adminID As String = System.Text.Encoding.UTF8.GetString(op.userID).TrimEnd(Chr(0))

                    ' 관리자 레벨 확인 (0:None, 1:Admin, 2:Config, 3:User)
                    ' 보통 1(Admin)이 전체 관리자입니다.
                    Dim levelStr As String = ""
                    Select Case op.level
                        Case 1 : levelStr = "Administrator (전체 권한)"
                        Case 2 : levelStr = "Configuration (설정 권한)"
                        Case 3 : levelStr = "User (사용자)"
                        Case Else : levelStr = $"Level {op.level}"
                    End Select

                    sb.AppendLine($"   [{i + 1}] ID: {adminID}  /  권한: {levelStr}")
                Next
            Else
                sb.AppendLine("   => 등록된 관리자가 없습니다. (누구나 메뉴 접근 가능)")
            End If
        Else
            sb.AppendLine($" [조회 실패] 오류 코드: {result}")
        End If

        ' 최종 출력
        txtDeviceInfo.Text = sb.ToString()
        txtDeviceInfo.SelectionStart = 0
        txtDeviceInfo.ScrollToCaret()


        ' 실제로 이 세팅값이어야 운영할 수 있음.......
        '== 1. 기본 하드웨어 정보
        ' - Device ID: 538212289
        ' - 모델명: BioStation 3(Type:35)
        ' - 접속 모드: 서버 -> 장치
        ' - IP 주소: 192.168.0.156
        ' - 포트: 51211

        '== 2. 장비 기능 지원 여부
        ' - 신형 얼굴인식(Visual Face) 지원: True   -- 바이오스테이션에서 True로 나와야 사용가능..(젤 중요)
        ' - 구형 얼굴인식(IR Face) 지원: False  -- 바이오스테이션이 아닌 옛날에 나온 구형장비에서 사용하는 옵션...바이오스테이션에서는 False로 나와도 상관없다.
        ' - 최대 사용자 수: 100,000
        ' - 최대 얼굴 템플릿 수: 100,000
        ' - 최대 로그 저장 수: 5,000,000

        '== 3. 인증 설정 (운영 모드)
        ' - 서버 매칭 모드 (useServerMatching): 0
        '   => [정상] 0 (장비 인증): 장비가 스스로 인증합니다.  -- 장비가 스스로 인증하게 설정하고 릴레이는 비활성화 시켜놓고
        ' - 전역 APB (useGlobalAPB): 1
        '   => [확인] 1 (서버 확인): 인증 후 서버에 출입 권한을 물어봅니다.  -- 전역 APB 설정값이 켜져 있어야 장비에서 안면들이댔을때 유저아이디가 날라온다.

        '== 4. 이벤트 설정 (이미지 로그)   -- 필요없음
        ' - 이미지 전송 필터 개수: 0
        '   => [정상] 꺼짐: 텍스트 로그만 전송하여 반응 속도가 빠릅니다. 

        '== 5. 상태 설정 (소리/LED)   -- 필요없음
        ' - LED 기능 사용 여부: 1
        ' - 부저(소리) 기능 사용 여부: 1
        '   => [정상] 부저 기능이 켜져 있습니다.

        '== 6. 시스템 설정 (System Config)   -- 필요없음
        ' - 타임존(Timezone): 32400
        ' - 카메라 주파수(Camera Freq): 2 Hz
        ' - 보안 탬퍼(Secure Tamper): 0
        ' - 카드 동작 마스크: 2147496757

        '== 7. 공장 초기 정보 (Factory Config)
        ' - MAC Address: 00-17-FC-14-77-C1
        ' - 모델명: BS3-DB
        ' - 펌웨어 버전: v100.2.1 (Build:   2023/11/13 12:43:42)

        '== 8. 디스플레이 설정 (Display Config)
        ' - 볼륨(Volume): 20  -- 볼륨 최대값 100(적당하게 조절해서 사용)
        ' - 배경 테마: 0
        ' - 메뉴 타임아웃: 20초
        ' - 화면보호기 사용: 1

        '== 9. 얼굴 설정 (Face Config)
        ' - 보안 등급(Security Level): 0
        ' - 조명 조건(Light Condition): 0
        ' - 등록 임계값(Enroll Threshold): 4
        ' - 감지 민감도(Detect Sensitivity): 2
        ' - LFD(가짜 얼굴 감지) 레벨: 1

        '== 10. IP 설정 (IP Config)
        ' - DHCP 사용: 1
        ' - IP Addr: 192.168.0.156
        ' - Gateway: 192.168.0.1
        ' - Subnet: 255.255.255.0
        ' - Server IP: 
        ' - Server Port 51212


    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If sdkContext <> IntPtr.Zero Then
            If connectedDeviceId <> 0 Then
                BS2_DisconnectDevice(sdkContext, connectedDeviceId)
            End If
            BS2_ReleaseContext(sdkContext)
            sdkContext = IntPtr.Zero
        End If

    End Sub

    Private Sub btnDLLUnLoad_Click(sender As Object, e As EventArgs) Handles btnDLLUnLoad.Click

        If connectedDeviceId <> 0 Then
            BS2_DisconnectDevice(sdkContext, connectedDeviceId)
        End If
        BS2_ReleaseContext(sdkContext)
        sdkContext = IntPtr.Zero

        MessageBox.Show("연결된 장치를 해제 하였습니다.")

    End Sub

    Private Sub btnConnectConnDevice_Click(sender As Object, e As EventArgs)

        ' SDK 초기화 및 입력값 확인
        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
        End If


        If txtDeviceID.Text.Trim = "" Then
            MessageBox.Show("연결할 장치 ID를 입력하세요.")
            Return
        End If

        ' 텍스트박스에서 목표 Device ID 가져오기
        Dim targetDeviceId As UInteger = 0
        Try
            targetDeviceId = UInteger.Parse(txtDeviceID.Text.Trim)
        Catch ex As Exception
            MessageBox.Show("올바른 숫자 형식의 ID를 입력하세요.")
            Return
        End Try

        ' 기존 연결 해제
        If connectedDeviceId <> 0 Then
            BS2_DisconnectDevice(sdkContext, connectedDeviceId)
            connectedDeviceId = 0
        End If

        ' 바로 연결 시도해보기
        Dim result As BS2ErrorCode = BS2_ConnectDevice(sdkContext, targetDeviceId)

        ' 만약 실패했다면? -> "검색(Search)"을 한번 돌려서 IP를 알아내고 다시 시도
        If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
            ' 검색 타임아웃 설정 (3~5초)
            BS2_SetDeviceSearchingTimeout(sdkContext, 5)
            ' 검색 수행
            BS2_SearchDevices(sdkContext)
            ' 다시 연결 시도
            result = CType(BS2_ConnectDevice(sdkContext, targetDeviceId), BS2ErrorCode)
        End If

        ' 최종 결과 처리
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            connectedDeviceId = targetDeviceId
            MessageBox.Show("장치 연결 성공! ID: " & targetDeviceId)
        Else
            MessageBox.Show($"연결 실패.{vbCrLf}먼저 '검색' 버튼을 눌러 장치가 리스트에 뜨는지 확인하거나,{vbCrLf}IP 연결 방식을 사용하세요.{vbCrLf}오류 코드: {CInt(result)}")
        End If


    End Sub

    Private Sub btnEnrollUser_Click(sender As Object, e As EventArgs) Handles btnEnrollUser.Click

        ' 연결 상태 확인
        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        BS2_SetDefaultResponseTimeout(sdkContext, 10 * 1000)  ' 타임아웃 10초설정

        ' 장치 정보 조회
        Dim deviceInfo As BS2SimpleDeviceInfo
        BS2_GetDeviceInfo(sdkContext, connectedDeviceId, deviceInfo)

        Dim cap As New BS2DeviceCapabilities
        BS2_GetDeviceCapabilities(sdkContext, connectedDeviceId, cap)


        ' 안면 지원 여부 확인
        ' 최신장비의 Visual Face (FaceEx) 지원 여부 : 바이오 스테이션 시리즈
        Dim isFaceExSupported = (cap.systemSupported And BS2CapabilitySystemSupport.SYSTEM_SUPPORT_FACEEX) > 0
        ' 구형장비의 Legacy Face 지원 여부 : 페이스 스테이션 등
        Dim isLegacyFaceSupported = Convert.ToBoolean(deviceInfo.faceSupported)

        ' 사용자 구조체 초기화
        Dim userBlob As New BS2UserFaceExBlob
        userBlob.user.userID = New Byte(BS2_USER_ID_SIZE - 1) {}
        userBlob.name = New Byte(BS2_USER_NAME_LEN - 1) {}
        userBlob.pin = New Byte(BS2_PIN_HASH_SIZE - 1) {}

        userBlob.cardObjs = IntPtr.Zero
        userBlob.fingerObjs = IntPtr.Zero
        userBlob.faceObjs = IntPtr.Zero
        userBlob.faceExObjs = IntPtr.Zero

        ' 유저아이디 설정
        Dim newUserId = txtUserID.Text.Trim
        Dim uidBytes = System.Text.Encoding.UTF8.GetBytes(newUserId)
        Array.Copy(uidBytes, userBlob.user.userID, Math.Min(uidBytes.Length, userBlob.user.userID.Length))

        ' 기본 설정
        Dim now = Date.Now
        userBlob.setting.startTime = CType(Util.ConvertToUnixTimestamp(now), UInteger)
        userBlob.setting.endTime = CType(Util.ConvertToUnixTimestamp(now.AddYears(10)), UInteger)
        userBlob.setting.idAuthMode = BS2AuthModeEnum.BS2_AUTH_MODE_NONE
        userBlob.setting.securityLevel = BS2UserSecurityLevelEnum.DEFAULT

        Dim ptrFaceBuf = IntPtr.Zero

        ' JPG 파일을 가지고 안면등록하기
        If rbLoadImage.Checked Then

            Dim imagePath = txtImagePath.Text.Trim
            If String.IsNullOrEmpty(imagePath) OrElse Not File.Exists(imagePath) Then
                MessageBox.Show("유효한 이미지 파일 경로를 입력해주세요.")
                Return
            End If

            If isFaceExSupported Then
                Dim extractedTemplate As New BS2TemplateEx
                Dim warpedImageBytes As Byte() = Nothing ' 정규화된 이미지를 받을 변수

                ' 새로 만든 함수 호출
                If GetFaceDataFromImage(sdkContext, connectedDeviceId, imagePath, extractedTemplate, warpedImageBytes) Then
                    userBlob.user.numFaces = 1

                    ' 표준 구조체
                    Dim faceExWarped As New BS2FaceExWarped
                    faceExWarped.faceIndex = 0
                    faceExWarped.numOfTemplate = 1

                    ' 재부팅 방지를 위해 Flag=1 (WARPED) 유지
                    faceExWarped.flag = 1  ' 0으로 하면 장비가 갑자기 재부팅됨..
                    faceExWarped.reserved = 0

                    ' 배열 초기화 (이거 안 하면 에러 뜸)
                    faceExWarped.unused = New Byte(5) {}

                    ' 이미지 데이터 복사
                    faceExWarped.imageLen = CUInt(warpedImageBytes.Length)
                    faceExWarped.imageData = New Byte(BS2_MAX_WARPED_IMAGE_LENGTH - 1) {}
                    Array.Copy(warpedImageBytes, faceExWarped.imageData, warpedImageBytes.Length)

                    ' IR 데이터 초기화
                    faceExWarped.irImageLen = 0
                    faceExWarped.irImageData = New Byte(BS2_MAX_WARPED_IR_IMAGE_LENGTH - 1) {}

                    ' 템플릿 배열 초기화 및 할당
                    faceExWarped.templateEx = New BS2TemplateEx(BS2_MAX_TEMPLATES_PER_FACE_EX - 1) {}
                    faceExWarped.templateEx(0) = extractedTemplate
                    For i = 1 To BS2_MAX_TEMPLATES_PER_FACE_EX - 1
                        faceExWarped.templateEx(i) = New BS2TemplateEx
                        faceExWarped.templateEx(i).data = New Byte(BS2_FACE_EX_TEMPLATE_SIZE - 1) {}
                        faceExWarped.templateEx(i).reserved = New Byte(2) {}
                    Next

                    ' 메모리 할당 및 연결
                    Dim sizeOfFaceEx = Marshal.SizeOf(GetType(BS2FaceExWarped))
                    ptrFaceBuf = Marshal.AllocHGlobal(sizeOfFaceEx)

                    ' 구조체를 포인터로 변환 (여기서 잘못꼬이면 장비가 재부팅함.....ㅠㅠ)
                    Marshal.StructureToPtr(faceExWarped, ptrFaceBuf, False)
                    userBlob.faceExObjs = ptrFaceBuf
                Else
                    Return
                End If
            Else
                MessageBox.Show("이 장치는 이미지 파일 등록(Visual Face)을 지원하지 않습니다.")
                Return
            End If

            ' 장비에서 촬영후 안면등록 (비주얼 페이스 기능 활용)
        ElseIf rbScanDevice.Checked Then

            If isFaceExSupported Then
                MessageBox.Show("[신형장비 안면등록] 장치 화면을 통해 안면 등록을 해주시기 바랍니다.")

                Dim faces(0) As BS2FaceExWarped
                Dim resultScan As BS2ErrorCode = BS2_ScanFaceEx(sdkContext, connectedDeviceId, faces, BS2FaceEnrollThreshold.THRESHOLD_DEFAULT, Nothing)

                If resultScan = BS2ErrorCode.BS_SDK_SUCCESS Then


                    ' 스캔된 얼굴 이미지를 JPG 파일로 저장
                    Try
                        Dim imgLen As Integer = CInt(faces(0).imageLen)
                        If imgLen > 0 Then
                            Dim imgBytes(imgLen - 1) As Byte
                            Array.Copy(faces(0).imageData, imgBytes, imgLen)


                            ' 바이트 배열 -> Base64 문자열 변환
                            Dim base64String As String = Convert.ToBase64String(imgBytes)

                            ' 디버그 출력 (출력창에서 확인 가능)
                            Debug.WriteLine("=== [Base64 Image Start] ===")
                            Debug.WriteLine(base64String)
                            Debug.WriteLine("=== [Base64 Image End] ===")

                            ' Base64 문자열을 텍스트 파일로 저장
                            System.IO.File.WriteAllText(System.IO.Path.Combine(Application.StartupPath, "FaceBase64.txt"), base64String)


                            ' 파일로 저장
                            Dim fileName As String = $"Capture_{newUserId}_{DateTime.Now:yyyyMMddHHmmss}.jpg"
                            Dim savePath As String = System.IO.Path.Combine(Application.StartupPath, fileName)

                            System.IO.File.WriteAllBytes(savePath, imgBytes)
                            Debug.WriteLine($"[이미지 저장] {savePath}")
                        End If
                    Catch ex As Exception
                        Debug.WriteLine("이미지 저장 중 오류: " & ex.Message)
                    End Try


                    userBlob.user.numFaces = 1  ' 얼굴 1개 등록
                    Dim sizeOfFaceEx = Marshal.SizeOf(GetType(BS2FaceExWarped))
                    ptrFaceBuf = Marshal.AllocHGlobal(sizeOfFaceEx)
                    Marshal.StructureToPtr(faces(0), ptrFaceBuf, False)
                    userBlob.faceExObjs = ptrFaceBuf
                Else
                    MessageBox.Show("스캔 실패: " & resultScan.ToString)
                    Return
                End If

            ElseIf isLegacyFaceSupported Then
                MessageBox.Show("[구형장비 안면등록] 장치 화면을 통해 안면 등록을 해주시기 바랍니다.")

                Dim faces(0) As BS2Face
                Dim resultScan As BS2ErrorCode = BS2_ScanFace(sdkContext, connectedDeviceId, faces, BS2FaceEnrollThreshold.THRESHOLD_DEFAULT, Nothing)

                If resultScan = BS2ErrorCode.BS_SDK_SUCCESS Then
                    userBlob.user.numFaces = 1
                    Dim sizeOfFace = Marshal.SizeOf(GetType(BS2Face))
                    ptrFaceBuf = Marshal.AllocHGlobal(sizeOfFace)
                    Marshal.StructureToPtr(faces(0), ptrFaceBuf, False)
                    userBlob.faceObjs = ptrFaceBuf
                Else
                    MessageBox.Show("스캔 실패: " & resultScan.ToString)
                    Return
                End If
            End If
        End If

        ' 회원 이름
        If Convert.ToBoolean(deviceInfo.userNameSupported) Then
            Dim userName = txtMemNm.Text.Trim
            Dim nameBytes = System.Text.Encoding.UTF8.GetBytes(userName)
            Array.Copy(nameBytes, userBlob.name, Math.Min(nameBytes.Length, userBlob.name.Length))
        End If

        ' 장비에 전송
        Try
            Dim userList = {userBlob}
            Dim result As BS2ErrorCode = BS2_EnrollUserFaceEx(sdkContext, connectedDeviceId, userList, 1, 1)

            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"사용자({newUserId}) 등록 성공!")
            Else
                MessageBox.Show($"등록 전송 실패. 오류 코드: {result}")
            End If
        Finally
            If ptrFaceBuf <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrFaceBuf)
        End Try

    End Sub


    ' 테스트용: 장비가 얼굴을 제대로 정규화(Normalize) 하는지 확인하는 함수
    Private Sub TestNormalizeImage(imagePath As String)
        If Not System.IO.File.Exists(imagePath) Then Return

        Dim imageBytes As Byte() = System.IO.File.ReadAllBytes(imagePath)

        ' 원본 이미지 메모리 할당
        Dim ptrRawImage As IntPtr = Marshal.AllocHGlobal(imageBytes.Length)

        ' 결과물(Warped Image)을 받을 버퍼 (최대 40KB 정도면 충분)
        Dim warpedSize As Integer = 40 * 1024
        Dim ptrWarpedImage As IntPtr = Marshal.AllocHGlobal(warpedSize)
        Dim outLen As UInteger = 0

        Try
            Marshal.Copy(imageBytes, 0, ptrRawImage, imageBytes.Length)

            ' 정규화된 이미지 요청 API 호출
            Dim result As BS2ErrorCode = API.BS2_GetNormalizedImageFaceEx(sdkContext, connectedDeviceId, ptrRawImage, CUInt(imageBytes.Length), ptrWarpedImage, outLen)

            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"[성공] 장비가 얼굴 정규화에 성공했습니다!{vbCrLf}데이터 크기: {outLen} bytes{vbCrLf}이 이미지는 등록 가능합니다.")
            Else
                Dim errCode As Integer = CInt(result)
                Dim msg As String = ""
                Select Case errCode
                    Case -308 : msg = "품질 낮음 (조명, 흐림)"
                    Case -322 : msg = "정규화 실패 (각도, EXIF 문제 등)"
                    Case -326 : msg = "회전됨 (기울어짐)"
                    Case Else : msg = "기타 원인"
                End Select

                MessageBox.Show($"[실패] 장비가 이미지를 거부했습니다.{vbCrLf}오류: {result} ({errCode}){vbCrLf}원인: {msg}")
            End If

        Finally
            If ptrRawImage <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrRawImage)
            If ptrWarpedImage <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrWarpedImage)
        End Try

    End Sub
    Private Sub btnTestImage_Click(sender As Object, e As EventArgs) Handles btnTestImage.Click

        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        Dim imagePath As String = txtImagePath.Text.Trim()
        If String.IsNullOrEmpty(imagePath) OrElse Not System.IO.File.Exists(imagePath) Then
            MessageBox.Show("텍스트박스에 유효한 이미지 경로가 없거나 파일을 찾을 수 없습니다.")
            Return
        End If

        TestNormalizeImage(imagePath)

    End Sub

    Private Sub btnUpdateFace_Click(sender As Object, e As EventArgs) Handles btnUpdateFace.Click

        If Not IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        Dim targetUserId = txtUserID.Text.Trim
        If String.IsNullOrEmpty(targetUserId) Then
            MessageBox.Show("수정할 사용자 ID를 입력해주세요.")
            txtUserID.Focus()
            Return
        End If

        ' 이미지 파일 확인
        If Not rbLoadImage.Checked Then
            MessageBox.Show("JPG 등록일 경우에만 수정 가능!!")
            Return
        End If

        Dim imagePath = txtImagePath.Text.Trim
        If String.IsNullOrEmpty(imagePath) OrElse Not File.Exists(imagePath) Then
            MessageBox.Show("유효한 이미지 파일 경로가 없습니다.")
            Return
        End If


        ' 사용자 구조체 준비
        Dim userBlob As New BS2UserFaceExBlob()

        ' 유저 아이디 설정
        userBlob.user.userID = New Byte(BS2Environment.BS2_USER_ID_SIZE - 1) {}
        Dim uidBytes = System.Text.Encoding.UTF8.GetBytes(targetUserId)
        Array.Copy(uidBytes, userBlob.user.userID, Math.Min(uidBytes.Length, userBlob.user.userID.Length))

        ' 이름 세팅
        userBlob.name = New Byte(BS2Environment.BS2_USER_NAME_LEN - 1) {}
        Dim newName As String = txtMemNm.Text.Trim()
        If Not String.IsNullOrEmpty(newName) Then
            Dim nameBytes = System.Text.Encoding.UTF8.GetBytes(newName)
            Array.Copy(nameBytes, userBlob.name, Math.Min(nameBytes.Length, userBlob.name.Length))
        End If

        ' 배열 초기화
        userBlob.pin = New Byte(BS2Environment.BS2_PIN_HASH_SIZE - 1) {}
        userBlob.phrase = New Byte(BS2Environment.BS2_USER_PHRASE_SIZE - 1) {}
        userBlob.accessGroupId = New UInt32(BS2Environment.BS2_MAX_ACCESS_GROUP_PER_USER - 1) {}
        userBlob.settingEx = New BS2UserSettingEx()
        userBlob.settingEx.reserved = New Byte(27) {}
        userBlob.cardObjs = IntPtr.Zero
        userBlob.fingerObjs = IntPtr.Zero
        userBlob.faceObjs = IntPtr.Zero
        userBlob.faceExObjs = IntPtr.Zero
        userBlob.user_photo_obj = IntPtr.Zero


        ' 새 얼굴 데이터 생성
        Dim ptrFaceBuf As IntPtr = IntPtr.Zero
        Try
            Dim extractedTemplate As New BS2TemplateEx()
            Dim warpedImageBytes As Byte() = Nothing

            'GetFaceDataFromBase64  = : base64 문자열에서 템플릿 추출
            If GetFaceDataFromImage(sdkContext, connectedDeviceId, imagePath, extractedTemplate, warpedImageBytes) Then

                userBlob.user.numFaces = 1

                Dim faceExWarped As New BS2FaceExWarped()
                faceExWarped.faceIndex = 0
                faceExWarped.numOfTemplate = 1
                faceExWarped.flag = 1
                faceExWarped.reserved = 0
                faceExWarped.unused = New Byte(5) {}

                ' 이미지 데이터 복사
                faceExWarped.imageLen = CUInt(warpedImageBytes.Length)
                faceExWarped.imageData = New Byte(BS2Environment.BS2_MAX_WARPED_IMAGE_LENGTH - 1) {}
                Array.Copy(warpedImageBytes, faceExWarped.imageData, warpedImageBytes.Length)

                ' IR 데이터 초기화
                faceExWarped.irImageLen = 0
                faceExWarped.irImageData = New Byte(BS2Environment.BS2_MAX_WARPED_IR_IMAGE_LENGTH - 1) {}

                ' 템플릿 배열 초기화 및 할당
                faceExWarped.templateEx = New BS2TemplateEx(BS2Environment.BS2_MAX_TEMPLATES_PER_FACE_EX - 1) {}
                faceExWarped.templateEx(0) = extractedTemplate

                For i As Integer = 1 To BS2Environment.BS2_MAX_TEMPLATES_PER_FACE_EX - 1
                    faceExWarped.templateEx(i) = New BS2TemplateEx()
                    faceExWarped.templateEx(i).data = New Byte(BS2Environment.BS2_FACE_EX_TEMPLATE_SIZE - 1) {}
                    faceExWarped.templateEx(i).reserved = New Byte(2) {}
                Next

                ' 메모리 할당 및 연결
                Dim sizeOfFaceEx As Integer = Marshal.SizeOf(GetType(BS2FaceExWarped))
                ptrFaceBuf = Marshal.AllocHGlobal(sizeOfFaceEx)
                Marshal.StructureToPtr(faceExWarped, ptrFaceBuf, False)
                userBlob.faceExObjs = ptrFaceBuf
            Else
                Return
            End If

            Dim updateMask As Integer = BS2UserMaskEnum.FACE_EX Or BS2UserMaskEnum.NAME  ' 얼굴과 이름 수정
            Dim userList As BS2UserFaceExBlob() = {userBlob}

            ' API 호출
            Dim result As BS2ErrorCode = API.BS2_PartialUpdateUserFaceEx(sdkContext, connectedDeviceId, updateMask, userList, 1)
            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"사용자({targetUserId}) 정보(이름, 얼굴) 수정 완료!")
            ElseIf result = BS2ErrorCode.BS_SDK_ERROR_CANNOT_FIND_USER Then
                MessageBox.Show("수정 실패: 존재하지 않는 사용자 ID입니다.")
            Else
                MessageBox.Show($"수정 실패. 오류 코드: {result}")
            End If
        Finally
            If ptrFaceBuf <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrFaceBuf)
        End Try


    End Sub

    Private Sub btnGetUserList_Click(sender As Object, e As EventArgs) Handles btnGetUserList.Click

        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        txtUserList.Clear()
        Application.DoEvents()

        Dim uidObj As IntPtr = IntPtr.Zero
        Dim numUser As UInteger = 0

        Try
            ' 전체 사용자 ID 목록 가져오기
            Dim result As BS2ErrorCode = API.BS2_GetUserList(sdkContext, connectedDeviceId, uidObj, numUser, Nothing)  ' Nothing : 전체 목록을 한 번에 받음..

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"리스트 조회 실패. 오류: {result}")
                Return
            End If

            If numUser = 0 Then
                txtUserList.AppendText("등록된 사용자가 없습니다.")
                Return
            End If

            txtUserList.AppendText($"총 {numUser}명의 사용자가 검색되었습니다." & vbCrLf & "--------------------------------" & vbCrLf)


            ' 상세 정보(이름) 조회 (BS2_GetUserDatas)
            ' 사용자가 너무 많으면(수천 명) 한 번에 가져올 때 메모리 부족이 발생할 수 있으므로
            ' 실제로는 100~500명씩 끊어서 가져오는 것이 좋음...테스트에서는 전체를 요청.
            Dim userBlobs(CInt(numUser) - 1) As BS2UserBlob

            ' 구조체 배열 초기화
            For i As Integer = 0 To userBlobs.Length - 1
                userBlobs(i).user.userID = New Byte(BS2Environment.BS2_USER_ID_SIZE - 1) {}
                userBlobs(i).name = New Byte(BS2Environment.BS2_USER_NAME_LEN - 1) {}
                userBlobs(i).pin = New Byte(BS2Environment.BS2_PIN_HASH_SIZE - 1) {}
                userBlobs(i).accessGroupId = New UInt32(BS2Environment.BS2_MAX_ACCESS_GROUP_PER_USER - 1) {}
                userBlobs(i).photo.data = New Byte(BS2Environment.BS2_USER_PHOTO_SIZE - 1) {} ' 사진 데이터 공간도 확보
                userBlobs(i).cardObjs = IntPtr.Zero
                userBlobs(i).fingerObjs = IntPtr.Zero
                userBlobs(i).faceObjs = IntPtr.Zero
            Next

            ' ID 목록 포인터(uidObj)를 그대로 사용하여 상세 정보 요청
            ' 마스크: DATA(기본정보) Or NAME(이름)
            Dim mask As UInteger = BS2UserMaskEnum.DATA Or BS2UserMaskEnum.NAME
            result = API.BS2_GetUserDatas(sdkContext, connectedDeviceId, uidObj, numUser, userBlobs, mask)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                txtUserList.AppendText($"상세 정보 조회 실패 (오류: {result})" & vbCrLf)
            Else
                For i As Integer = 0 To CInt(numUser) - 1
                    Dim userid As String = System.Text.Encoding.UTF8.GetString(userBlobs(i).user.userID).TrimEnd(Chr(0))
                    Dim name As String = System.Text.Encoding.UTF8.GetString(userBlobs(i).name).TrimEnd(Chr(0))
                    If String.IsNullOrEmpty(name) Then name = "(이름 없음)"
                    txtUserList.AppendText($"[{i + 1}] UserID: {userid}  /  이름: {name}" & vbCrLf)
                Next
            End If
        Finally
            If uidObj <> IntPtr.Zero Then
                API.BS2_ReleaseObject(uidObj)
            End If
        End Try

    End Sub
    Private Sub btnSetDeviceMode_Click(sender As Object, e As EventArgs) Handles btnSetDeviceMode.Click
        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return
        SetServerMatchingMode(sdkContext, connectedDeviceId, 0)  ' 장비에서 인증 (장비에 얼굴등록되어있으면 문 열어주기)
        txtRealTimeLog.AppendText(">> 장비 인증후 장비가 알아서 도어접점하는 모드 준비 완료. (장비자체에 릴레이 설정 활성화 되어있어야함)" & vbCrLf)
    End Sub

    Private Sub btnSetServerMode_Click(sender As Object, e As EventArgs) Handles btnSetServerMode.Click
        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return
        SetServerMatchingMode(sdkContext, connectedDeviceId, 1)   ' 서버에서 인증 (장비에는 얼굴등록 안 하고 서버 DB와 대조해서 서버에서 수동으로 문 열어준다.)

        ' 이벤트 핸들러 등록
        cbOnVerifyUser = New API.OnVerifyUser(AddressOf HandleVerifyUser)
        cbOnIdentifyUser = New API.OnIdentifyUser(AddressOf HandleIdentifyUser)

        Dim result As BS2ErrorCode = API.BS2_SetServerMatchingHandler(sdkContext, cbOnVerifyUser, cbOnIdentifyUser)
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            txtRealTimeLog.AppendText(">> 장비 인증후 서버에서 인증후 도어접점하는 모드 준비 완료." & vbCrLf)
        Else
            txtRealTimeLog.AppendText($">> 장비 인증후 서버에서 인증후 도어접점하는 모드 준비 실패: {result}" & vbCrLf)
        End If

    End Sub
    ' [1:1 인증 핸들러] (사용 안 함)
    ' SDK 구조상 등록은 해야 하므로, 내용은 비워두어 요청이 와도 무시...
    Private Sub HandleVerifyUser(deviceId As UInteger, seq As UShort, isCard As Byte, cardType As Byte, data As IntPtr, dataLen As UInteger)
    End Sub

    ' 장비가 얼굴을 감지하면 이 함수가 호출되는데......
    ' 이 방식은 현실적으로 사용할 수가 없다. 장비가 유저아이디를 주지않고 이미지 템플릿을 주기때문에 디비에서 비교할수가없다.ㅠㅠ
    Private Sub HandleIdentifyUser(deviceId As UInteger, seq As UShort, format As Byte, templateData As IntPtr, templateSize As UInteger)

        Dim tempUserId As String = "1234"
        Dim isDbCheckPassed As Boolean = True '  테스트 무조건 트루 리턴
        Dim responseResult As Integer
        Dim userBlob As New BS2UserBlob()

        ' 사용자 정보 채우기 (장비 화면에 표시하기 위한 용도)
        userBlob.user.userID = New Byte(BS2Environment.BS2_USER_ID_SIZE - 1) {}
        Dim uidBytes = System.Text.Encoding.UTF8.GetBytes(tempUserId)
        Array.Copy(uidBytes, userBlob.user.userID, Math.Min(uidBytes.Length, userBlob.user.userID.Length))

        ' 배열 초기화
        userBlob.name = New Byte(BS2Environment.BS2_USER_NAME_LEN - 1) {}
        userBlob.pin = New Byte(BS2Environment.BS2_PIN_HASH_SIZE - 1) {}
        userBlob.accessGroupId = New UInt32(BS2Environment.BS2_MAX_ACCESS_GROUP_PER_USER - 1) {}
        userBlob.cardObjs = IntPtr.Zero
        userBlob.fingerObjs = IntPtr.Zero
        userBlob.faceObjs = IntPtr.Zero

        If isDbCheckPassed Then
            responseResult = BS2ErrorCode.BS_SDK_SUCCESS
            API.BS2_IdentifyUser(sdkContext, deviceId, seq, responseResult, userBlob)
            Task.Run(Sub()
                         OpenRelay(deviceId, 0)
                         Me.BeginInvoke(Sub() txtRealTimeLog.AppendText($">> [서버인증 승인] 문 열림 명령 전송 ({tempUserId})" & vbCrLf))
                     End Sub)
        Else
            responseResult = BS2ErrorCode.BS_SDK_ERROR_ACCESS_RULE_VIOLATION
            API.BS2_IdentifyUser(sdkContext, deviceId, seq, responseResult, userBlob)

            Me.BeginInvoke(Sub() txtRealTimeLog.AppendText($">> [서버인증 거부] DB 조회 실패" & vbCrLf))
        End If

    End Sub
    Private Sub btnRemoveUser_Click(sender As Object, e As EventArgs) Handles btnRemoveUser.Click

        '============= 사용자 삭제  ==================
        If Not IsDeviceConnected(sdkContext, connectedDeviceId) Then Return


        If chkAllUserDel.Checked Then   ' 전체삭제
            Dim confirm = MessageBox.Show("장치에 저장된 모든 사용자가 삭제됩니다." & vbCrLf & "계속 진행하시겠습니까?",
                                            "전체 삭제 경고",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Warning)

            If confirm = DialogResult.No Then Return

            Dim result As BS2ErrorCode = BS2_RemoveAllUser(sdkContext, connectedDeviceId)

            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show("모든 사용자가 정상적으로 삭제되었습니다.")
            Else
                MessageBox.Show($"전체 삭제 실패.{vbCrLf}오류 코드: {result} ({CInt(result)})")
            End If
        Else  ' 개별삭제
            Dim targetUserId = txtUserID.Text.Trim
            If String.IsNullOrEmpty(targetUserId) Then
                MessageBox.Show("삭제할 사용자 ID를 입력해주세요.")
                txtUserID.Focus()
                Return
            End If

            Dim uidBytes(BS2_USER_ID_SIZE - 1) As Byte
            Dim strBytes = System.Text.Encoding.UTF8.GetBytes(targetUserId)
            Array.Copy(strBytes, uidBytes, Math.Min(strBytes.Length, uidBytes.Length))

            Dim ptrUid = Marshal.AllocHGlobal(BS2_USER_ID_SIZE)
            Try
                Marshal.Copy(uidBytes, 0, ptrUid, BS2_USER_ID_SIZE)
                Dim result As BS2ErrorCode = BS2_RemoveUser(sdkContext, connectedDeviceId, ptrUid, 1)
                If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                    MessageBox.Show($"사용자({targetUserId}) 삭제 완료!")
                ElseIf result = BS2ErrorCode.BS_SDK_ERROR_CANNOT_FIND_USER Then
                    MessageBox.Show("삭제 실패: 장치에 없는 사용자 ID입니다.")
                Else
                    MessageBox.Show($"삭제 실패 (오류: {result})")
                End If
            Finally
                If ptrUid <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrUid)
            End Try
        End If

    End Sub

    Private Sub btnStartMonitoring_Click(sender As Object, e As EventArgs) Handles btnStartMonitoring.Click

        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        cbOnLogReceived = New API.OnLogReceived(AddressOf RealTimeLogHandler)
        Dim result As BS2ErrorCode = API.BS2_StartMonitoringLog(sdkContext, connectedDeviceId, cbOnLogReceived)
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            MessageBox.Show("실시간 장비 로그 모니터링 시작!!!!!")   ' 장비에서 발생되는 모든 이벤트 로그를 실시간으로 받아서 텍스트박스에 뿌려준다.
        Else
            MessageBox.Show($"모니터링 시작 실패: {result}")
        End If

    End Sub
    ' 실시간 로그 처리 함수 : 장비에서 이벤트가 발생할 때마다 호출됨
    Private Sub RealTimeLogHandler(deviceId As UInteger, logPtr As IntPtr)

        If logPtr = IntPtr.Zero Then Return

        Dim log As BS2Event = Marshal.PtrToStructure(Of BS2Event)(logPtr)
        Dim eventCode As UShort = log.code
        Dim userId As String = System.Text.Encoding.UTF8.GetString(log.userID).TrimEnd(Chr(0))

        Me.Invoke(Sub()
                      Dim curTime As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                      Dim eventName As String = CType(eventCode, BS2EventCodeEnum).ToString()
                      txtRealTimeLog.AppendText($"[{curTime}] ::: ID:{userId} / {eventName} (0x{eventCode:X4})" & vbCrLf)

                      'Dim isFaceSuccess As Boolean = (eventCode = BS2EventCodeEnum.IDENTIFY_SUCCESS_FACE)
                      '' 장비에서 얼굴 인증 성공 이벤트인 경우에만 문 열기 시도
                      'If isFaceSuccess Then

                      '    If userId = "1234" Then
                      '        txtRealTimeLog.AppendText(">> [문을 연다] 얼굴 인증 성공 & ID 1234 확인됨!" & vbCrLf)

                      '        ' 실시간 로그처리에서 디비조회후 도어제어를 하지 않는다. 

                      '        'UnlockDoor(deviceId, 1)  ' 장비에 연결된 릴레이 번호를 아는 경우 이렇게 명령하고..(1번 릴레이를 열어라..)  -- 릴레이 접점 신호 안남....
                      '        'OpenRelay(deviceId, 0)  ' 장비에 연결된 릴레이 번호를 모를 경우 그냥 첫 번째 릴레이를 작동시켜라..  -- 릴레이 접점 신호 안남....
                      '        'TestBuzzer(deviceId)  ' 테스트용으로 부저음 울리기  -- 릴레이 접점 신호 안남....
                      '        'TestBuzzer_HardCoded(deviceId)  ' 테스트용으로 부저음 울리기 (하드코딩 버전))  -- 릴레이 접점 신호 안남....
                      '        'TestLED(deviceId)     ' 테스트용으로 LED 점멸시키기)  -- 릴레이 접점 신호 안남....
                      '    Else
                      '        txtRealTimeLog.AppendText($">> [문을 열지않음] (ID: {userId})" & vbCrLf)
                      '    End If
                      'End If
                  End Sub)
    End Sub
    ' 도어 ID를 지정하여 문을 여는 함수
    Private Sub UnlockDoor(deviceId As UInteger, targetDoorId As UInteger)

        Dim ptrDoorIds As IntPtr = Marshal.AllocHGlobal(4)
        Try
            Marshal.WriteInt32(ptrDoorIds, CInt(targetDoorId))
            Dim result As BS2ErrorCode = API.BS2_UnlockDoor(sdkContext, deviceId, BS2DoorFlagEnum.NONE, ptrDoorIds, 1)
            Me.Invoke(Sub()
                          If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                              txtRealTimeLog.AppendText(">> [성공] [실제로 문이 열림]" & vbCrLf)
                          Else
                              txtRealTimeLog.AppendText($">> [실패] [실제로 문 열기 실패] : {result}" & vbCrLf)
                          End If
                      End Sub)
        Finally
            Marshal.FreeHGlobal(ptrDoorIds)
        End Try

    End Sub
    ' Door ID가 없어도 릴레이를 직접 작동시킨다.
    ' relayIndex: 보통 0번이 1번 릴레이..
    ' deviceId: 장비 아이디
    ' 
    Private Sub OpenRelay(deviceId As UInteger, relayIndex As Integer)

        Dim action As New BS2Action()
        action.deviceID = deviceId
        action.type = BS2ActionTypeEnum.RELAY
        action.stopFlag = 0
        action.delay = 0

        ' 릴레이 액션 세부 설정
        Dim relayAction As New BS2RelayAction()
        relayAction.relayIndex = CByte(relayIndex)

        relayAction.reserved = New Byte(2) {}  ' 배열 초기화

        ' 신호 설정 (3초간 켜짐)
        relayAction.signal.signalID = 0
        relayAction.signal.count = 1          ' 1번 작동
        relayAction.signal.onDuration = 3000  ' 3000ms (3초) 켜짐
        relayAction.signal.offDuration = 0
        relayAction.signal.delay = 0

        action.actionUnion = New Byte(23) {}

        Dim sizeRelay As Integer = Marshal.SizeOf(GetType(BS2RelayAction))
        Dim ptrRelay As IntPtr = Marshal.AllocHGlobal(sizeRelay)

        Try
            Marshal.StructureToPtr(relayAction, ptrRelay, False)
            Marshal.Copy(ptrRelay, action.actionUnion, 0, sizeRelay)
        Finally
            Marshal.FreeHGlobal(ptrRelay)
        End Try

        ' BS2_RunAction api는 장비에 어떤 명령이든 줄수있는 토탈api이다. 이 api에다가 릴레이 작동명령을 넣어서 장비에 전송한다.
        Dim result As BS2ErrorCode = API.BS2_RunAction(sdkContext, deviceId, action)
        Me.Invoke(Sub()
                      If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                          txtRealTimeLog.AppendText(">> [성공] 릴레이 작동 명령 전송 완료!" & vbCrLf)
                      Else
                          txtRealTimeLog.AppendText($">> [실패] 릴레이 작동 실패 : {result}" & vbCrLf)
                      End If
                  End Sub)

    End Sub
    Private Sub btnRemoveAllDoors_Click(sender As Object, e As EventArgs) Handles btnRemoveAllDoors.Click

        If Not IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        Dim confirm = MessageBox.Show("장치에 설정된 도어정보가 모두 삭제됨!!!" & vbCrLf &
                                        "도어정보가 없으면 접점명령을 줄 수 없습니다." & vbCrLf &
                                        "계속하시겠습니까?",
                                        "도어정보 전체 삭제",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Warning)

        If confirm = DialogResult.No Then Return
        Dim result As BS2ErrorCode = BS2_RemoveAllDoor(sdkContext, connectedDeviceId)
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            MessageBox.Show("모든 도어정보 삭제완료!!")
        Else
            MessageBox.Show($"도어정보 삭제 실패.{vbCrLf}오류 코드: {result} ({CInt(result)})")
        End If

    End Sub

    Private Sub btnGetDoorList_Click(sender As Object, e As EventArgs) Handles btnGetDoorList.Click

        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        txtDoorList.Clear()

        Dim doorObj As IntPtr = IntPtr.Zero
        Dim numDoor As UInteger = 0

        Try
            Dim result As BS2ErrorCode = API.BS2_GetAllDoor(sdkContext, connectedDeviceId, doorObj, numDoor)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"도어 목록 조회 실패: {result}")
                Return
            End If
            If numDoor = 0 Then
                txtDoorList.AppendText(">> 도어가 없음." & vbCrLf)
                Return
            End If
            txtDoorList.AppendText($"총 {numDoor}개의 도어가 검색됨." & vbCrLf)

            Dim curPtr As IntPtr = doorObj
            Dim structSize As Integer = Marshal.SizeOf(GetType(BS2Door))

            For i As Integer = 0 To CInt(numDoor) - 1
                Dim door As BS2Door = Marshal.PtrToStructure(Of BS2Door)(curPtr)
                Dim doorId As UInt32 = door.doorID
                Dim doorName As String = System.Text.Encoding.UTF8.GetString(door.name).TrimEnd(Chr(0))
                Dim entryDevice As UInt32 = door.entryDeviceID
                Dim relayIndex As Integer = door.relay.port

                txtDoorList.AppendText($"[{i + 1}] Door ID: {doorId} / Name: {doorName}" & vbCrLf)
                txtDoorList.AppendText($"    - Entry Device: {entryDevice}" & vbCrLf)
                txtDoorList.AppendText($"    - Relay Port: {relayIndex}" & vbCrLf)
                txtDoorList.AppendText("--------------------------------" & vbCrLf)
                curPtr = New IntPtr(curPtr.ToInt64() + structSize)
            Next
        Finally
            If doorObj <> IntPtr.Zero Then
                API.BS2_ReleaseObject(doorObj)
            End If
        End Try

    End Sub


    Private Sub btnDisableImageLog_Click(sender As Object, e As EventArgs) Handles btnDisableImageLog.Click
        DisableImageLog(connectedDeviceId)
    End Sub
    'Private Sub DisableImageLog(deviceId As UInteger)

    '    If Not modSupremaFunc.IsDeviceConnected(sdkContext, deviceId) Then Return

    '    Dim eventConfig As IntPtr = IntPtr.Zero
    '    Dim configSize As Integer = Marshal.SizeOf(GetType(BS2EventConfig))
    '    Dim ptrConfig As IntPtr = Marshal.AllocHGlobal(configSize)
    '    Dim simpleConfig As New BS2EventConfig()

    '    Try
    '        Dim result As BS2ErrorCode = API.BS2_GetEventConfig(sdkContext, deviceId, simpleConfig)
    '        If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
    '            MessageBox.Show($"설정 조회 실패: {result}")
    '            Return
    '        End If

    '        simpleConfig.numImageEventFilter = 0
    '        result = API.BS2_SetEventConfig(sdkContext, deviceId, simpleConfig)

    '        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
    '            MessageBox.Show("이미지 로그 전송 비활성화 성공!")
    '        Else
    '            MessageBox.Show($"설정 적용 실패: {result}")
    '        End If
    '    Finally
    '        If ptrConfig <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrConfig)
    '    End Try

    'End Sub

    ' 장비 인증 모드 세팅
    Private Sub FixDeviceConfig(deviceId As UInteger)
        Dim config As New BS2AuthConfig()
        If API.BS2_GetAuthConfig(sdkContext, deviceId, config) = BS2ErrorCode.BS_SDK_SUCCESS Then
            config.useServerMatching = 0 ' 장비 인증 모드 (장비 초기화를 하면 이게 기본값이긴 하다...)
            config.useGlobalAPB = 0      ' 서버 응답 대기 끄기 (장비 초기화를 하면 이게 기본값이긴 하다...)
            API.BS2_SetAuthConfig(sdkContext, deviceId, config)
        End If
    End Sub

    ' 인증시 이미지 전송 끄기 세팅  (릴레이 반응 속도 향상)
    Private Sub DisableImageLog(deviceId As UInteger)
        Dim config As New BS2EventConfig()
        If API.BS2_GetEventConfig(sdkContext, deviceId, config) = BS2ErrorCode.BS_SDK_SUCCESS Then
            config.numImageEventFilter = 0
            API.BS2_SetEventConfig(sdkContext, deviceId, config)
        End If
    End Sub

    'Private Sub btnCheckConfig_Click(sender As Object, e As EventArgs)

    '    If Not IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

    '    Dim sb As New Text.StringBuilder
    '    sb.AppendLine("=== 장비 설정 상태 점검 ===")

    '    ' 1. 인증 설정 확인 (Global APB, Server Matching)
    '    Dim authConfig As New BS2AuthConfig
    '    Dim resAuth As BS2ErrorCode = BS2_GetAuthConfig(sdkContext, connectedDeviceId, authConfig)

    '    If resAuth = BS2ErrorCode.BS_SDK_SUCCESS Then
    '        sb.AppendLine($"[인증 설정]")
    '        sb.AppendLine($" - 서버 인증 모드 (useServerMatching): {authConfig.useServerMatching}")
    '        sb.AppendLine($" - 전역 APB (useGlobalAPB): {authConfig.useGlobalAPB}")

    '        If authConfig.useGlobalAPB = 1 Then
    '            sb.AppendLine("   => [확인 필요] Global APB가 켜져 있습니다.")
    '        Else
    '            sb.AppendLine("   => [정상] Global APB가 꺼져 있습니다.")
    '        End If
    '    Else
    '        sb.AppendLine($"[인증 설정] 조회 실패: {resAuth}")
    '    End If

    '    sb.AppendLine("-----------------------------")

    '    ' 2. 이벤트 설정 확인 (Image Log)
    '    Dim eventConfig As New BS2EventConfig
    '    ' (참고: 구조체 내 배열 초기화가 없으면 Get호출 시 에러날 수 있으므로 간단 초기화)
    '    eventConfig.imageEventFilter = New BS2ImageEventFilter(BS2_EVENT_MAX_IMAGE_CODE_COUNT - 1) {}

    '    Dim resEvent As BS2ErrorCode = BS2_GetEventConfig(sdkContext, connectedDeviceId, eventConfig)

    '    If resEvent = BS2ErrorCode.BS_SDK_SUCCESS Then
    '        sb.AppendLine($"[이벤트 설정]")
    '        sb.AppendLine($" - 이미지 로그 필터 개수 (numImageEventFilter): {eventConfig.numImageEventFilter}")

    '        If eventConfig.numImageEventFilter > 0 Then
    '            sb.AppendLine("   => [문제 발견] 이미지 로그가 켜져 있습니다. (전송 딜레이 원인)")
    '        Else
    '            sb.AppendLine("   => [정상] 이미지 로그가 꺼져 있습니다.")
    '        End If
    '    Else
    '        sb.AppendLine($"[이벤트 설정] 조회 실패: {resEvent}")
    '    End If
    '    txtDeviceInfo.Text = sb.ToString

    'End Sub

    Private Sub btnDeepClean_Click(sender As Object, e As EventArgs) Handles btnDeepClean.Click

        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        Dim sb As New System.Text.StringBuilder()

        '' 장치 내의 모든 로그 삭제
        'Dim resLog As BS2ErrorCode = API.BS2_ClearLog(sdkContext, connectedDeviceId)
        'sb.AppendLine($"로그 삭제: {resLog}")

        '' 모든 존(Zone) 설정 삭제
        'Dim resZone1 As BS2ErrorCode = API.BS2_RemoveAllAntiPassbackZone(sdkContext, connectedDeviceId)
        'Dim resZone2 As BS2ErrorCode = API.BS2_RemoveAllFireAlarmZone(sdkContext, connectedDeviceId)
        'Dim resZone3 As BS2ErrorCode = API.BS2_RemoveAllInterlockZone(sdkContext, connectedDeviceId)
        'sb.AppendLine($"Zone 설정 삭제: {resZone1}, {resZone2}, {resZone3}")

        '' 모든 도어 설정 삭제
        'Dim resDoor As BS2ErrorCode = API.BS2_RemoveAllDoor(sdkContext, connectedDeviceId)
        'sb.AppendLine($"Door 설정 삭제: {resDoor}")

        ' 장비 재부팅 
        Dim resReboot As BS2ErrorCode = API.BS2_RebootDevice(sdkContext, connectedDeviceId)
        sb.AppendLine($"장비 재부팅: {resReboot}")

        MessageBox.Show(sb.ToString() & vbCrLf & "장비가 재부팅됩니다. 재부팅 후 다시 연결할 것!")

    End Sub
    ' 장비 볼륨 설정하는 함수 (최대 100)
    Private Sub SetDeviceVolume(deviceId As UInteger, volume As Byte)

        If Not modSupremaFunc.IsDeviceConnected(sdkContext, deviceId) Then Return

        ' 1. 디스플레이/사운드 설정 가져오기
        Dim displayConfig As New BS2DisplayConfig()
        Dim result As BS2ErrorCode = API.BS2_GetDisplayConfig(sdkContext, deviceId, displayConfig)

        If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
            MessageBox.Show($"설정 조회 실패: {result}")
            Return
        End If

        ' 2. 현재 볼륨 확인 및 변경
        Dim oldVol As Byte = displayConfig.volume
        displayConfig.volume = volume ' (0 = 음소거, 100 = 최대)

        ' 3. 설정 적용
        result = API.BS2_SetDisplayConfig(sdkContext, deviceId, displayConfig)

        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            MessageBox.Show($"볼륨 변경 완료! (기존: {oldVol} -> 변경: {volume})")
        Else
            MessageBox.Show($"볼륨 변경 실패: {result}")
        End If

    End Sub
    ' 장비의 부저/LED 기능이 켜져 있는지 확인하는 함수
    Private Sub CheckStatusConfig(deviceId As UInteger)

        If Not modSupremaFunc.IsDeviceConnected(sdkContext, deviceId) Then Return

        Dim config As New BS2StatusConfig()
        config.led = New BS2LedStatusConfig(BS2Environment.BS2_DEVICE_STATUS_NUM - 1) {}
        config.buzzer = New BS2BuzzerStatusConfig(BS2Environment.BS2_DEVICE_STATUS_NUM - 1) {}

        Dim result As BS2ErrorCode = API.BS2_GetStatusConfig(sdkContext, deviceId, config)
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            ' 부저 설정 확인 (보통 0번 인덱스가 기본)
            Dim isBuzzerOn As Boolean = (config.buzzer(0).enabled = 1)

            Dim msg As String = $"부저(Buzzer) 기능: {If(isBuzzerOn, "켜짐 (ON)", "꺼짐 (OFF)")}"

            If Not isBuzzerOn Then
                msg &= vbCrLf & ">> 부저 기능이 꺼져 있어서 소리가 안 났습니다." & vbCrLf & ">> '켜기'로 설정을 변경합니다..."

                ' 강제로 켜기
                config.buzzer(0).enabled = 1
                API.BS2_SetStatusConfig(sdkContext, deviceId, config)
            End If
            MessageBox.Show(msg)
        Else
            MessageBox.Show($"상태 설정 조회 실패: {result}")
        End If
    End Sub

    Private Sub btnSetGlobalAPB_Click(sender As Object, e As EventArgs) Handles btnSetGlobalAPB.Click

        ' 장비인증모드이지만 찾은 ID로 서버에 출입 가능 여부 물어보는 Global APB 모드로 설정
        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        Dim authConfig As New BS2AuthConfig()
        API.BS2_GetAuthConfig(sdkContext, connectedDeviceId, authConfig)

        authConfig.useServerMatching = 0 ' 장비가 얼굴 식별 (ID 찾음)
        authConfig.useGlobalAPB = 1      ' 찾은 ID로 서버에 출입 가능 여부 물어봄

        API.BS2_SetAuthConfig(sdkContext, connectedDeviceId, authConfig)

        ' 실제 게이트데몬에서는 프로그램 시작시 이 핸들러를 등록해서 사용한다.
        cbGlobalAPB = New API.OnCheckGlobalAPBViolation(AddressOf HandleGlobalAPB)

        API.BS2_SetCheckGlobalAPBViolationHandler(sdkContext, cbGlobalAPB)

        txtRealTimeLog.AppendText(">> 장비 인증후 유저아이디 넘겨받아 도어접점하는 모드 준비 완료. (장비자체에 릴레이 설정을 비활성화 시켜야함)" & vbCrLf)

    End Sub
    ' 장비에서 얼굴을 인증하면 이 함수가 호출된다.
    Private Sub HandleGlobalAPB(deviceId As UInteger, seq As UShort, userID_1 As String, userID_2 As String, isDualAuth As Boolean)

        '<UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        'Public Delegate Sub OnCheckGlobalAPBViolation(deviceId As BS2_SCHEDULE_ID, seq As UShort, userID_1 As String, userID_2 As String, isDualAuth As Boolean)
        '실제 얼굴을 인증한 장치의 deviceId 와 사용자의 ID가 userID_1에 담겨서 온다.

        Dim responseResult As Integer
        Dim resultMsg As String = CheckTest(userID_1)

        Select Case resultMsg
            Case "1"
                responseResult = BS2ErrorCode.BS_SDK_SUCCESS
                Task.Run(Sub()
                             Dim resDoor = modSupremaFunc.UnlockDoor(sdkContext, deviceId, 1)  ' 1번 도어를 열어라..
                             Me.BeginInvoke(Sub()
                                                If resDoor = BS2ErrorCode.BS_SDK_SUCCESS Then
                                                    txtRealTimeLog.AppendText(">> [성공] 문 열림 명령 전송 완료" & vbCrLf)
                                                Else
                                                    txtRealTimeLog.AppendText($">> [실패] 문 열기 실패: {resDoor}" & vbCrLf)
                                                End If
                                            End Sub)
                         End Sub)
            Case "-1"
                responseResult = BS2ErrorCode.BS_SDK_ERROR_EXPIRED
                Me.BeginInvoke(Sub() txtRealTimeLog.AppendText($">> [거부] ID:{userID_1} - {responseResult}" & vbCrLf))
            Case Else
                responseResult = BS2ErrorCode.BS_SDK_ERROR_ACCESS_RULE_VIOLATION
                Me.BeginInvoke(Sub() txtRealTimeLog.AppendText($">> [거부] ID:{userID_1} - {responseResult}" & vbCrLf))
        End Select

        API.BS2_CheckGlobalAPBViolation(sdkContext, deviceId, seq, responseResult, 0)

    End Sub
    Private Function CheckTest(userId As String) As String
        ' 테스트용 
        Select Case userId
            Case "1234"
                Return "1"  ' 정상
            Case "5895"
                Return "-1"  ' 만료
            Case Else
                Return "-999"  ' 기타오류
        End Select

    End Function


    Private Sub btnSetDoor_Click(sender As Object, e As EventArgs) Handles btnSetDoor.Click

        If Not IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        ' 도어 정보 설정 (ID: 1, 릴레이: 0번)
        Dim doorId As UInteger = 1
        Dim relayIndex = 0

        Dim door As New BS2Door
        door.doorID = doorId

        ' 도어 이름 설정
        door.name = New Byte(BS2_MAX_DOOR_NAME_LEN - 1) {}
        Dim nameBytes = System.Text.Encoding.UTF8.GetBytes("Door 1")   ' 도어이름 : Door 1
        Array.Copy(nameBytes, door.name, Math.Min(nameBytes.Length, door.name.Length))

        ' 릴레이 연결 설정
        door.relay.deviceID = connectedDeviceId
        door.relay.port = CByte(relayIndex) ' 0번 릴레이 포트 사용
        door.relay.reserved = New Byte(2) {}

        ' 기타 필수 배열 초기화
        door.sensor = New BS2DoorSensor
        door.sensor.reserved = New Byte(0) {}
        door.button = New BS2ExitButton
        door.button.reserved = New Byte(1) {}

        ' 도어 옵션 설정
        door.autoLockTimeout = 3   ' 3초후 자동 잠김
        door.heldOpenTimeout = 0
        door.instantLock = 0
        door.unlockFlags = 0
        door.lockFlags = 0
        door.unconditionalLock = 0

        ' 알람 액션 배열 초기화
        door.forcedOpenAlarm = New BS2Action(BS2_MAX_FORCED_OPEN_ALARM_ACTION - 1) {}
        For i = 0 To door.forcedOpenAlarm.Length - 1
            door.forcedOpenAlarm(i).actionUnion = New Byte(31) {}
        Next
        door.heldOpenAlarm = New BS2Action(BS2_MAX_HELD_OPEN_ALARM_ACTION - 1) {}
        For i = 0 To door.heldOpenAlarm.Length - 1
            door.heldOpenAlarm(i).actionUnion = New Byte(31) {}
        Next

        ' 예비 공간 초기화
        door.reserved = New Byte(1) {}
        door.reserved2 = New Byte(2) {}
        door.dualAuthApprovalGroupID = New UInteger(BS2_MAX_DUAL_AUTH_APPROVAL_GROUP - 1) {}

        ' APB Zone 초기화
        door.apbZone = New BS2AntiPassbackZone
        door.apbZone.name = New Byte(BS2_MAX_ZONE_NAME_LEN - 1) {}
        door.apbZone.reserved = New Byte(2) {}
        door.apbZone.reserved2 = New Byte(511) {}
        door.apbZone.bypassGroupIDs = New UInteger(BS2_MAX_BYPASS_GROUPS_PER_APB_ZONE - 1) {}
        door.apbZone.alarm = New BS2Action(BS2_MAX_APB_ALARM_ACTION - 1) {}
        For i = 0 To door.apbZone.alarm.Length - 1
            door.apbZone.alarm(i).actionUnion = New Byte(31) {}
        Next
        door.apbZone.readers = New BS2ApbMember(BS2_MAX_READERS_PER_APB_ZONE - 1) {}
        For i = 0 To door.apbZone.readers.Length - 1
            door.apbZone.readers(i).reserved = New Byte(2) {}
        Next


        ' 도어정보를 장비에 등록하기
        Dim ptrDoor = IntPtr.Zero

        Try
            Dim sizeDoor = Marshal.SizeOf(GetType(BS2Door))
            ptrDoor = Marshal.AllocHGlobal(sizeDoor)
            Marshal.StructureToPtr(door, ptrDoor, False)

            Dim result As BS2ErrorCode = BS2_SetDoor(sdkContext, connectedDeviceId, ptrDoor, 1)
            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"도어(ID:{doorId}) 등록 완료!")
                If txtRealTimeLog IsNot Nothing Then
                    txtRealTimeLog.AppendText($">> [설정] 도어 등록 완료 (ID: {doorId}, Relay: {relayIndex})" & vbCrLf)
                End If
            Else
                MessageBox.Show($"도어 등록 실패. 오류 코드: {result}")
            End If
        Finally
            If ptrDoor <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrDoor)
        End Try

    End Sub

    Private Sub btnFactoryReset_Click(sender As Object, e As EventArgs) Handles btnFactoryReset.Click

        ' 1. 초기화 진행 여부를 묻는 메시지 박스 띄우기
        Dim confirmResult As DialogResult = MessageBox.Show(
        "정말로 장비를 공장 초기화하시겠습니까?" & vbCrLf & vbCrLf &
        "주의: 관리자 정보, 사용자 데이터, 네트워크 설정 등 모든 데이터가 삭제되며 복구할 수 없습니다.",
        "공장 초기화 경고",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning,
        MessageBoxDefaultButton.Button2) ' 실수 방지를 위해 기본 선택을 '아니오'로 설정

        ' 2. 사용자가 '예(Yes)'를 눌렀을 때만 실행
        If confirmResult = DialogResult.Yes Then

            ' 장비 공장 초기화 (모든 설정 및 사용자 데이터 삭제)
            Dim result As BS2ErrorCode = API.BS2_FactoryReset(sdkContext, connectedDeviceId)

            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show("장비가 성공적으로 공장 초기화되었습니다." & vbCrLf & "장비가 재부팅될 수 있습니다.", "초기화 완료", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("초기화 실패: " & result.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        End If


    End Sub

    ' [기능 추가] 장비 세팅값 커스텀 초기화 (시간, 언어, IP)
    Private Sub ResetDeviceToMyStandard()

        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        Dim result As BS2ErrorCode
        Dim sbLog As New System.Text.StringBuilder()
        sbLog.AppendLine("=== 장비 사용자 정의 초기화 시작 ===")

        ' ---------------------------------------------------------
        ' 1. 시스템 설정: 타임존을 한국 표준시(KST, UTC+9)로 설정
        ' ---------------------------------------------------------
        Dim sysConfig As New BS2SystemConfig()
        ' 구조체 내부 배열 초기화 (필수)
        sysConfig.notUsed = New Byte(16 * 16 * 3 - 1) {}
        sysConfig.reserved = New Byte(1) {}
        sysConfig.reserved2 = New Byte(15) {}

        ' 기존 설정 불러오기
        result = API.BS2_GetSystemConfig(sdkContext, connectedDeviceId, sysConfig)
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            sysConfig.timezone = 9 * 3600  ' GMT+9 (32400초)
            sysConfig.syncTime = 1         ' 서버 시간 동기화 사용 (선택사항)

            result = API.BS2_SetSystemConfig(sdkContext, connectedDeviceId, sysConfig)
            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                sbLog.AppendLine("[성공] 타임존 설정 (한국 표준시)")
            Else
                sbLog.AppendLine($"[실패] 타임존 설정 오류: {result}")
            End If
        Else
            sbLog.AppendLine($"[실패] 시스템 설정 조회 오류: {result}")
        End If


        ' ---------------------------------------------------------
        ' 2. 인증 설정 저장 (핵심: 초기 관리자 등록 화면 스킵)
        '    관리자 수(numOperators)를 0으로 설정하여 전송하면 
        '    장비는 '설정 완료'로 인식하고 대기 화면으로 넘어갑니다.
        ' ---------------------------------------------------------
        Dim authConfig As New BS2AuthConfig()
        authConfig.authSchedule = New UInteger(BS2Environment.BS2_NUM_OF_AUTH_MODE - 1) {}
        authConfig.reserved = New Byte(29) {}
        authConfig.operators = New BS2AuthOperatorLevel(BS2Environment.BS2_MAX_OPERATORS - 1) {}

        result = API.BS2_GetAuthConfig(sdkContext, connectedDeviceId, authConfig)
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            authConfig.numOperators = 0 ' 관리자 없음으로 설정
            result = API.BS2_SetAuthConfig(sdkContext, connectedDeviceId, authConfig)
            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                sbLog.AppendLine("[성공] 인증 설정 (초기 관리자 등록 스킵)")
            Else
                sbLog.AppendLine($"[실패] 인증 설정 오류: {result}")
            End If
        End If


        ' ---------------------------------------------------------
        ' 2. 장비 시간: 현재 PC(한국) 시간으로 즉시 동기화
        ' ---------------------------------------------------------
        ' 현재 시간을 Unix Timestamp(UTC 기준)로 변환
        Dim now As DateTime = DateTime.UtcNow
        Dim timestamp As UInteger = CType(Util.ConvertToUnixTimestamp(now), UInteger)

        result = API.BS2_SetDeviceTime(sdkContext, connectedDeviceId, timestamp)
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            sbLog.AppendLine("[성공] 장비 시간 동기화 완료")
        Else
            sbLog.AppendLine($"[실패] 시간 설정 오류: {result}")
        End If

        ' ---------------------------------------------------------
        ' 3. 디스플레이 설정: 언어를 한국어로 설정
        ' ---------------------------------------------------------
        Dim displayConfig As New BS2DisplayConfig()
        displayConfig.shortcutHome = New Byte(BS2Environment.BS2_MAX_SHORTCUT_HOME - 1) {}
        displayConfig.tnaIcon = New Byte(BS2Environment.BS2_MAX_TNA_KEY - 1) {}
        displayConfig.reserved3 = New Byte(26) {}

        result = API.BS2_GetDisplayConfig(sdkContext, connectedDeviceId, displayConfig)
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            displayConfig.language = BS2LanguageEnum.KOREAN ' 0: 한국어
            result = API.BS2_SetDisplayConfig(sdkContext, connectedDeviceId, displayConfig)
            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                sbLog.AppendLine("[성공] 언어 설정 (한국어)")
            Else
                sbLog.AppendLine($"[실패] 언어 설정 오류: {result}")
            End If
        End If

        ' ---------------------------------------------------------
        ' 4. IP 설정: 고정 IP(192.168.0.157) 사용 (DHCP 끄기)
        ' ---------------------------------------------------------
        Dim ipConfig As New BS2IpConfig()

        ' 구조체 배열 초기화 (필수)
        ipConfig.ipAddress = New Byte(BS2Environment.BS2_IPV4_ADDR_SIZE - 1) {}
        ipConfig.gateway = New Byte(BS2Environment.BS2_IPV4_ADDR_SIZE - 1) {}
        ipConfig.subnetMask = New Byte(BS2Environment.BS2_IPV4_ADDR_SIZE - 1) {}
        ipConfig.serverAddr = New Byte(BS2Environment.BS2_IPV4_ADDR_SIZE - 1) {}
        ipConfig.reserved3 = New Byte(29) {}

        ' 기존 설정값을 먼저 읽어옴 (기존 포트 설정 등을 유지하기 위해)
        result = API.BS2_GetIPConfig(sdkContext, connectedDeviceId, ipConfig)

        If result = BS2ErrorCode.BS_SDK_SUCCESS Then

            ' (1) DHCP 끄기 (0: 고정 IP 사용, 1: DHCP 사용)
            ipConfig.useDHCP = 0

            ' (2) 고정 IP 설정 (192.168.0.157)
            Dim ipBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(txtIP.Text.Trim)
            Array.Clear(ipConfig.ipAddress, 0, ipConfig.ipAddress.Length)
            Array.Copy(ipBytes, ipConfig.ipAddress, Math.Min(ipBytes.Length, ipConfig.ipAddress.Length))

            ' (3) 서브넷 마스크 설정 (일반적으로 255.255.255.0)
            Dim subBytes As Byte() = System.Text.Encoding.UTF8.GetBytes("255.255.255.0")
            Array.Clear(ipConfig.subnetMask, 0, ipConfig.subnetMask.Length)
            Array.Copy(subBytes, ipConfig.subnetMask, Math.Min(subBytes.Length, ipConfig.subnetMask.Length))

            ' (4) 게이트웨이 설정 (보통 IP의 끝자리가 1, 예: 192.168.0.1)
            ' ※ 주의: 게이트웨이가 틀리면 외부망 통신이 안 될 수 있습니다. 현장 상황에 맞춰야 합니다.
            Dim gateBytes As Byte() = System.Text.Encoding.UTF8.GetBytes("192.168.0.1")
            Array.Clear(ipConfig.gateway, 0, ipConfig.gateway.Length)
            Array.Copy(gateBytes, ipConfig.gateway, Math.Min(gateBytes.Length, ipConfig.gateway.Length))

            ' 설정 적용
            result = API.BS2_SetIPConfig(sdkContext, connectedDeviceId, ipConfig)
            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                sbLog.AppendLine("[성공] 고정 IP 설정 완료 (IP: " & txtIP.Text.Trim & " / DHCP: OFF)")
            Else
                sbLog.AppendLine($"[실패] IP 설정 오류: {result}")
            End If
        Else
            sbLog.AppendLine($"[실패] 기존 IP 설정 조회 오류: {result}")
        End If

        'RegisterFactoryAdmin()  ' 장치 초기화면 스킵용 기본 관리자 등록 (현장에서 사용하면 안됨)

        ' 결과 출력
        MessageBox.Show(sbLog.ToString(), "초기화 결과")
        txtRealTimeLog.AppendText(sbLog.ToString() & vbCrLf)

    End Sub
    ' [기능 추가] 초기화면 스킵용 기본 관리자 등록 (ID: 1, PW: 1234)
    Private Sub RegisterFactoryAdmin()
        ' 1. 사용자 구조체 생성
        Dim userBlob As New BS2UserBlob()
        userBlob.user.userID = New Byte(BS2Environment.BS2_USER_ID_SIZE - 1) {}
        userBlob.name = New Byte(BS2Environment.BS2_USER_NAME_LEN - 1) {}
        userBlob.pin = New Byte(BS2Environment.BS2_PIN_HASH_SIZE - 1) {}
        userBlob.accessGroupId = New UInt32(BS2Environment.BS2_MAX_ACCESS_GROUP_PER_USER - 1) {}
        userBlob.photo.data = New Byte(BS2Environment.BS2_USER_PHOTO_SIZE - 1) {}

        ' 포인터 초기화
        userBlob.cardObjs = IntPtr.Zero
        userBlob.fingerObjs = IntPtr.Zero
        userBlob.faceObjs = IntPtr.Zero

        ' 2. ID 설정 (ID: "0000")
        Dim uidBytes = System.Text.Encoding.UTF8.GetBytes("0000")
        Array.Copy(uidBytes, userBlob.user.userID, Math.Min(uidBytes.Length, userBlob.user.userID.Length))

        ' 3. PIN 설정 (PW: "0000") - 암호화 필요
        Dim pinCode As String = "0000"   ' 암호 
        Dim ptrPin = Marshal.AllocHGlobal(BS2Environment.BS2_PIN_HASH_SIZE)
        Dim ptrPinString = Marshal.StringToHGlobalAnsi(pinCode)

        Try
            ' SDK가 제공하는 해시 함수로 PIN 변환
            API.BS2_MakePinCode(sdkContext, ptrPinString, ptrPin)
            Marshal.Copy(ptrPin, userBlob.pin, 0, BS2Environment.BS2_PIN_HASH_SIZE)
        Finally
            Marshal.FreeHGlobal(ptrPin)
            Marshal.FreeHGlobal(ptrPinString)
        End Try

        ' 4. 이름 설정
        Dim nameBytes = System.Text.Encoding.UTF8.GetBytes("Admin")
        Array.Copy(nameBytes, userBlob.name, Math.Min(nameBytes.Length, userBlob.name.Length))

        ' 5. 기본 설정 및 ***관리자 권한 부여*** (이게 핵심)
        userBlob.user.flag = 0
        userBlob.user.version = 0
        userBlob.user.authGroupID = 0
        userBlob.user.faceChecksum = 0

        ' [중요] 관리자로 설정 (BS2UserOperatorEnum.ADMIN = 1)
        ' 주의: BS2User 구조체에는 operator 필드가 없고, 별도 API나 AuthConfig로 설정해야 하지만,
        ' 일부 최신 SDK/장비는 사용자 정보 자체에 권한을 묻는 경우가 있습니다. 
        ' 하지만 표준적인 방법은 'BS2AuthConfig'의 Operators에 이 ID를 추가하는 것입니다.

        ' 우선 사용자 등록
        Dim userList As BS2UserBlob() = {userBlob}
        Dim result As BS2ErrorCode = API.BS2_EnrollUser(sdkContext, connectedDeviceId, userList, 1, 1)

        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            ' 6. 등록된 사용자를 '관리자'로 승격시킴
            Dim authConfigExt As New BS2AuthConfigExt()
            ' 배열 초기화 (필수)
            authConfigExt.extAuthSchedule = New UInteger(BS2Environment.BS2_MAX_NUM_OF_EXT_AUTH_MODE - 1) {}
            authConfigExt.operators = New BS2AuthOperatorLevel(BS2Environment.BS2_MAX_OPERATORS - 1) {}
            For i As Integer = 0 To authConfigExt.operators.Length - 1
                authConfigExt.operators(i).userID = New Byte(BS2Environment.BS2_USER_ID_SIZE - 1) {}
                authConfigExt.operators(i).reserved = New Byte(2) {}
            Next
            authConfigExt.reserved2 = New Byte(3) {}
            authConfigExt.reserved3 = New Byte(0) {}
            authConfigExt.reserved4 = New Byte(255) {}

            ' 기존 설정 읽기
            API.BS2_GetAuthConfigExt(sdkContext, connectedDeviceId, authConfigExt)

            ' 첫 번째 관리자 슬롯에 ID '1' 등록
            authConfigExt.numOperators = 1
            Array.Copy(uidBytes, authConfigExt.operators(0).userID, Math.Min(uidBytes.Length, BS2Environment.BS2_USER_ID_SIZE))
            authConfigExt.operators(0).level = 1 ' 1: Admin (전체 권한)

            ' 설정 적용
            result = API.BS2_SetAuthConfigExt(sdkContext, connectedDeviceId, authConfigExt)

            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                txtRealTimeLog.AppendText(">> [완료] 기본 관리자(ID:1 / PW:1234) 등록 완료. (초기 설정 마법사 해제됨)" & vbCrLf)
            End If
        Else
            txtRealTimeLog.AppendText($">> [실패] 관리자 사용자 등록 실패: {result}" & vbCrLf)
        End If

    End Sub

    Private Sub btnCustomReset_Click(sender As Object, e As EventArgs) Handles btnCustomReset.Click

        ' 1. 초기화 진행 여부를 묻는 메시지 박스 띄우기
        Dim confirmResult = MessageBox.Show(
        "장치 세팅을 초기화 하시겠습니까?" & vbCrLf & vbCrLf &
        "주의: 아이피 세팅, 언어 설정 등이 변경됩니다.",
        "세팅 초기화 경고",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning,
        MessageBoxDefaultButton.Button2) ' 실수 방지를 위해 기본 선택을 '아니오'로 설정

        If confirmResult = DialogResult.Yes Then
            ResetDeviceToMyStandard()
        End If

    End Sub

    Private Sub btnSettingCopy_Click(sender As Object, e As EventArgs) Handles btnSettingCopy.Click

        ' 1. 복사할 원본 장비 IP 입력 받기
        Dim sourceIp As String = InputBox("설정을 복사해올 원본 장비의 IP 주소를 입력하세요:", "장비 설정 복사", "192.168.0.100")

        If String.IsNullOrEmpty(sourceIp) Then Return ' 취소 시 종료

        ' 2. 설정 복사 함수 실행 (포트는 기본 51211)
        CloneDeviceSettings(sourceIp, 51211)
    End Sub
    ' [기능 추가] 다른 IP의 장비 설정을 가져와서 현재 장비에 똑같이 적용하기
    Private Sub CloneDeviceSettings(sourceIp As String, sourcePort As UShort)

        ' 1. 대상 장비(현재 연결된 장비) 확인
        If connectedDeviceId = 0 Then
            MessageBox.Show("설정을 적용할 대상 장비가 먼저 연결되어 있어야 합니다.")
            Return
        End If

        Dim sourceDeviceId As UInteger = 0
        Dim result As BS2ErrorCode

        ' 2. 원본 장비(Source) 연결 시도
        Dim ptrIp As IntPtr = Marshal.StringToHGlobalAnsi(sourceIp)
        Try
            ' SDK Context는 기존 것을 공유해서 사용
            result = API.BS2_ConnectDeviceViaIP(sdkContext, ptrIp, sourcePort, sourceDeviceId)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"원본 장비({sourceIp}) 연결 실패: {result}")
                Return
            End If
        Finally
            Marshal.FreeHGlobal(ptrIp)
        End Try

        ' 3. 원본 장비에서 설정값 읽어오기
        Dim sbLog As New System.Text.StringBuilder()
        sbLog.AppendLine($"=== 장비 설정 복사 시작 ({sourceIp} -> {connectedDeviceId}) ===")

        Try
            ' (1) 시스템 설정 (타임존, 카메라 주파수 등)
            Dim sysConfig As New BS2SystemConfig()
            sysConfig.notUsed = New Byte(16 * 16 * 3 - 1) {}
            sysConfig.reserved = New Byte(1) {}
            sysConfig.reserved2 = New Byte(15) {}

            Dim resSys = API.BS2_GetSystemConfig(sdkContext, sourceDeviceId, sysConfig)

            ' (2) 디스플레이 설정 (언어, 배경화면, 볼륨 등)
            Dim displayConfig As New BS2DisplayConfig()
            displayConfig.shortcutHome = New Byte(BS2Environment.BS2_MAX_SHORTCUT_HOME - 1) {}
            displayConfig.tnaIcon = New Byte(BS2Environment.BS2_MAX_TNA_KEY - 1) {}
            displayConfig.reserved3 = New Byte(26) {}

            Dim resDisp = API.BS2_GetDisplayConfig(sdkContext, sourceDeviceId, displayConfig)

            ' (3) 인증 설정 (인증 모드, 글로벌 APB 등)
            Dim authConfig As New BS2AuthConfig()

            ' [중요] 관리자 정보를 담을 배열 초기화 (이 부분이 없으면 관리자 정보가 복사되지 않을 수 있음)
            authConfig.authSchedule = New UInteger(BS2Environment.BS2_NUM_OF_AUTH_MODE - 1) {}
            authConfig.reserved = New Byte(29) {}
            authConfig.operators = New BS2AuthOperatorLevel(BS2Environment.BS2_MAX_OPERATORS - 1) {}

            ' 각 관리자 슬롯의 내부 배열도 초기화
            For i As Integer = 0 To authConfig.operators.Length - 1
                authConfig.operators(i).userID = New Byte(BS2Environment.BS2_USER_ID_SIZE - 1) {}
                authConfig.operators(i).reserved = New Byte(2) {}
            Next

            Dim resAuth = API.BS2_GetAuthConfig(sdkContext, sourceDeviceId, authConfig)



            ' (4) 상태 설정 (LED, 부저)
            Dim statusConfig As New BS2StatusConfig()
            statusConfig.led = New BS2LedStatusConfig(BS2Environment.BS2_DEVICE_STATUS_NUM - 1) {}
            statusConfig.buzzer = New BS2BuzzerStatusConfig(BS2Environment.BS2_DEVICE_STATUS_NUM - 1) {}

            Dim resStat = API.BS2_GetStatusConfig(sdkContext, sourceDeviceId, statusConfig)

            ' (5) 얼굴 인식 설정 (보안 등급, 감지 거리 등)
            Dim faceConfig As New BS2FaceConfig()
            faceConfig.reserved = New Byte(12) {}

            Dim resFace = API.BS2_GetFaceConfig(sdkContext, sourceDeviceId, faceConfig)

            ' 4. 원본 장비 연결 해제 (설정 다 읽었으므로 끊기)
            API.BS2_DisconnectDevice(sdkContext, sourceDeviceId)


            ' 5. 현재 장비(Target)에 설정 덮어쓰기
            ' 주의: IP 설정(BS2IpConfig)은 복사하지 않음 (IP 충돌 방지)

            ' (1) 시스템 설정 적용
            If resSys = BS2ErrorCode.BS_SDK_SUCCESS Then
                If API.BS2_SetSystemConfig(sdkContext, connectedDeviceId, sysConfig) = BS2ErrorCode.BS_SDK_SUCCESS Then
                    sbLog.AppendLine("[성공] 시스템 설정 복사 완료")
                Else
                    sbLog.AppendLine("[실패] 시스템 설정 적용 실패")
                End If
            End If

            ' (2) 디스플레이 설정 적용
            If resDisp = BS2ErrorCode.BS_SDK_SUCCESS Then
                If API.BS2_SetDisplayConfig(sdkContext, connectedDeviceId, displayConfig) = BS2ErrorCode.BS_SDK_SUCCESS Then
                    sbLog.AppendLine("[성공] 디스플레이/소리 설정 복사 완료")
                Else
                    sbLog.AppendLine("[실패] 디스플레이 설정 적용 실패")
                End If
            End If

            ' (3) 인증 설정 적용
            If resAuth = BS2ErrorCode.BS_SDK_SUCCESS Then
                If API.BS2_SetAuthConfig(sdkContext, connectedDeviceId, authConfig) = BS2ErrorCode.BS_SDK_SUCCESS Then
                    sbLog.AppendLine("[성공] 인증 모드 설정 복사 완료")
                Else
                    sbLog.AppendLine("[실패] 인증 설정 적용 실패")
                End If
            End If

            ' (4) 상태 설정 적용
            If resStat = BS2ErrorCode.BS_SDK_SUCCESS Then
                If API.BS2_SetStatusConfig(sdkContext, connectedDeviceId, statusConfig) = BS2ErrorCode.BS_SDK_SUCCESS Then
                    sbLog.AppendLine("[성공] LED/부저 설정 복사 완료")
                Else
                    sbLog.AppendLine("[실패] 상태 설정 적용 실패")
                End If
            End If

            ' (5) 얼굴 설정 적용
            If resFace = BS2ErrorCode.BS_SDK_SUCCESS Then
                If API.BS2_SetFaceConfig(sdkContext, connectedDeviceId, faceConfig) = BS2ErrorCode.BS_SDK_SUCCESS Then
                    sbLog.AppendLine("[성공] 얼굴 인식 설정 복사 완료")
                Else
                    sbLog.AppendLine("[실패] 얼굴 설정 적용 실패")
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("설정 복사 중 오류 발생: " & ex.Message)
        End Try

        ' 결과 리포트
        MessageBox.Show(sbLog.ToString())
        txtRealTimeLog.AppendText(sbLog.ToString() & vbCrLf)

    End Sub
End Class