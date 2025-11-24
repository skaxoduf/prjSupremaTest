Imports System.IO
Imports System.Net
Imports System.Runtime.InteropServices
Imports prjSupremaTest.Suprema
Imports prjSupremaTest.Suprema.API
Imports Suprema
Imports Suprema.API

Public Class Form1

    Private sdkContext As IntPtr = IntPtr.Zero  ' SDK 컨텍스트 핸들
    Private connectedDeviceId As UInteger = 0   ' 연결된 장치 ID
    Private cbOnLogReceived As API.OnLogReceived = Nothing   ' 장비 이벤트 받는 콜백함수 선언


    '이 구조체는 오리지널 구조체가 복잡한 배열을 포함하고 있어 메모리 정렬 문제를 피하기 위해 단순화된 버전 (기본적으로 사용안함)
    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FaceExWarped_Safe
        Public faceIndex As Byte
        Public numOfTemplate As Byte
        Public flag As Byte
        Public reserved As Byte
        Public imageLen As UInteger
        Public irImageLen As UShort
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=6)>
        Public unused As Byte()
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BS2Environment.BS2_MAX_WARPED_IMAGE_LENGTH)>
        Public imageData As Byte()
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=BS2Environment.BS2_MAX_WARPED_IR_IMAGE_LENGTH)>
        Public irImageData As Byte()
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=11120)>
        Public templateExBlob As Byte()
    End Structure
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
                MessageBox.Show("장치 찾기 성공! Device ID: " & deviceId)
            Else
                MessageBox.Show($"장치 찾기 실패.{vbCrLf}오류 내용: {result}{vbCrLf}오류 코드: {CInt(result)}")
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

        ' --- 리스트박스 초기화 ---
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
                    ' 장치 ID 읽기
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

                ' 사용한 메모리 해제
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

        '장치와 연결이 완료되었다면 장치 정보를 가져와야 합니다.
        '장치 종류에 따라 일부 기능이 지원되지 않기 때문에 BioStar 애플리케이션은 장치에 맞춰 UI를 구성해야 합니다.
        '1) 장치 정보를 가져오기 위해서는 BS2_GetDeviceInfo 함수를 사용합니다.

        ' sdkContext : &H00000197843CCB90  정상이라는 뜻 
        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
        End If

        txtDeviceInfo.Clear()

        Dim selectedDeviceId As UInteger = 0
        If String.IsNullOrWhiteSpace(txtDeviceID.Text) Then
            MessageBox.Show("장치 ID가 입력되지 않았습니다. 먼저 장치를 연결해주세요.")
            txtDeviceID.Focus()
            txtDeviceID.SelectAll()
            Return
        End If
        If Not UInteger.TryParse(txtDeviceID.Text.Trim(), selectedDeviceId) Then
            MessageBox.Show("장치 ID는 숫자여야 합니다.")
            txtDeviceID.Focus()
            txtDeviceID.SelectAll()
            Return
        End If
        selectedDeviceId = txtDeviceID.Text.Trim

        Dim deviceInfo As BS2SimpleDeviceInfo
        Dim getInfoResult As BS2ErrorCode = BS2_GetDeviceInfo(sdkContext, selectedDeviceId, deviceInfo)

        If getInfoResult = BS2ErrorCode.BS_SDK_SUCCESS Then

            Dim infoString As New Text.StringBuilder
            infoString.AppendLine("장치 정보 가져오기 성공!")
            infoString.AppendLine("--------------------------")
            infoString.AppendLine($"Device ID: {deviceInfo.id}")

            Dim deviceType As BS2DeviceTypeEnum = deviceInfo.type
            Dim deviceName = "Unknown"
            If productNameDictionary.ContainsKey(deviceType) Then
                deviceName = productNameDictionary(deviceType)
            End If
            infoString.AppendLine($"Device Type: {deviceName} (Enum: {deviceInfo.type})")

            Dim ip = New IPAddress(BitConverter.GetBytes(deviceInfo.ipv4Address)).ToString
            infoString.AppendLine($"IP Address: {ip}")
            infoString.AppendLine($"Port: {deviceInfo.port}")
            infoString.AppendLine($"Max Users: {deviceInfo.maxNumOfUser}")
            infoString.AppendLine($"Fingerprint Supported: {Convert.ToBoolean(deviceInfo.fingerSupported)}")
            infoString.AppendLine($"Old Face Supported: {Convert.ToBoolean(deviceInfo.faceSupported)}")
            infoString.AppendLine($"Card Supported: {Convert.ToBoolean(deviceInfo.cardSupported)}")
            infoString.AppendLine($"PIN Supported: {Convert.ToBoolean(deviceInfo.pinSupported)}")
            infoString.AppendLine($"WLAN Supported: {Convert.ToBoolean(deviceInfo.wlanSupported)}")


            ' 신형장비 스펙 조회 (BS2_GetDeviceCapabilities) - 여기서 나오는 값이 진짜 얼굴인식 지원여부 값이다.
            Dim cap As New BS2DeviceCapabilities()
            Dim capResult As BS2ErrorCode = API.BS2_GetDeviceCapabilities(sdkContext, selectedDeviceId, cap)

            If capResult = BS2ErrorCode.BS_SDK_SUCCESS Then
                infoString.AppendLine(vbCrLf & "=== 장비 상세 스펙2 ===")

                Dim isFaceExSupported As Boolean = (cap.systemSupported And BS2CapabilitySystemSupport.SYSTEM_SUPPORT_FACEEX) > 0

                infoString.AppendLine($"New Face Supported: {isFaceExSupported}")
                infoString.AppendLine($"Max Users: {cap.maxUsers}")
                infoString.AppendLine($"Max Faces: {cap.maxFaces}")

                If isFaceExSupported Then
                    infoString.AppendLine("=> 이 장치는 신형 얼굴 인식(FaceEx)을 지원합니다.")
                ElseIf Convert.ToBoolean(deviceInfo.faceSupported) Then
                    infoString.AppendLine("=> 이 장치는 구형 얼굴 인식(Face)을 지원합니다.")
                Else
                    infoString.AppendLine("=> 이 장치는 얼굴 인식을 지원하지 않습니다.")
                End If
            Else
                infoString.AppendLine("상세 능력치 조회 실패: " & capResult.ToString())
            End If
            txtDeviceInfo.Text = infoString.ToString
        Else
            txtDeviceInfo.Text = $"장치 정보 가져오기 실패. (ID: {selectedDeviceId})" & vbCrLf
            txtDeviceInfo.AppendText("오류 코드: 0x" & getInfoResult.ToString("X"))
        End If

    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If sdkContext <> IntPtr.Zero Then
            ' 종료 전 연결된 장치 해제
            If connectedDeviceId <> 0 Then
                BS2_DisconnectDevice(sdkContext, connectedDeviceId)
            End If
            BS2_ReleaseContext(sdkContext)
            sdkContext = IntPtr.Zero
        End If

    End Sub

    Private Sub btnDLLUnLoad_Click(sender As Object, e As EventArgs) Handles btnDLLUnLoad.Click

        ' 종료 전 연결된 장치 해제
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
            BS2_SetDeviceSearchingTimeout(sdkContext, 7)
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
        ' 최신장비의 Visual Face (FaceEx) 지원 여부 : 바이오 스테이션
        Dim isFaceExSupported = (cap.systemSupported And BS2CapabilitySystemSupport.SYSTEM_SUPPORT_FACEEX) > 0
        ' 구형장비의 Legacy Face 지원 여부 : 페이스 스테이션
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

        ' ==================================================================
        ' 얼굴 등록 jpg랑 장비스캔 두가지 케이스 처리
        ' ==================================================================
        Dim ptrFaceBuf = IntPtr.Zero

        ' >>> [CASE 1] JPG 파일 불러오기 <<<
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


                    '' 재부팅되고 그러면 아래 코드로 변경..
                    'userBlob.user.numFaces = 1

                    '' 구조체 사용
                    'Dim faceExSafe As New BS2FaceExWarped_Safe()
                    'faceExSafe.faceIndex = 0
                    'faceExSafe.numOfTemplate = 1
                    'faceExSafe.flag = 1
                    'faceExSafe.unused = New Byte(5) {}

                    '' 이미지 데이터 복사
                    'faceExSafe.imageLen = CUInt(warpedImageBytes.Length)
                    'faceExSafe.imageData = New Byte(BS2Environment.BS2_MAX_WARPED_IMAGE_LENGTH - 1) {}
                    'Array.Copy(warpedImageBytes, faceExSafe.imageData, warpedImageBytes.Length)

                    '' IR 데이터 초기화
                    'faceExSafe.irImageLen = 0
                    'faceExSafe.irImageData = New Byte(BS2Environment.BS2_MAX_WARPED_IR_IMAGE_LENGTH - 1) {}

                    '' 템플릿 데이터를 바이트 Blob 배열로 직접 복사
                    'Dim templateTotalSize As Integer = 11120 ' 20개 * 556바이트
                    'faceExSafe.templateExBlob = New Byte(templateTotalSize - 1) {}

                    'Dim tempSize As Integer = Marshal.SizeOf(GetType(BS2TemplateEx))
                    'Dim ptrTemp As IntPtr = Marshal.AllocHGlobal(tempSize)

                    'Try
                    '    Marshal.StructureToPtr(extractedTemplate, ptrTemp, False)
                    '    Marshal.Copy(ptrTemp, faceExSafe.templateExBlob, 0, tempSize)
                    'Finally
                    '    Marshal.FreeHGlobal(ptrTemp)
                    'End Try

                    '' 메모리 할당 및 연결 
                    'Dim sizeOfSafeStruct As Integer = Marshal.SizeOf(GetType(BS2FaceExWarped_Safe))
                    'ptrFaceBuf = Marshal.AllocHGlobal(sizeOfSafeStruct)
                    'Marshal.StructureToPtr(faceExSafe, ptrFaceBuf, False)

                    'userBlob.faceExObjs = ptrFaceBuf
                Else
                    Return ' 실패 시 중단
                End If
            Else
                MessageBox.Show("이 장치는 이미지 파일 등록(Visual Face)을 지원하지 않습니다.")
                Return
            End If

            ' >>> [CASE 2] 장비에서 촬영하기 <<<
        ElseIf rbScanDevice.Checked Then

            If isFaceExSupported Then
                MessageBox.Show("[Visual Face] 장치 화면을 봐주세요.")
                Dim faces(0) As BS2FaceExWarped
                Dim resultScan As BS2ErrorCode = BS2_ScanFaceEx(sdkContext, connectedDeviceId, faces, BS2FaceEnrollThreshold.THRESHOLD_DEFAULT, Nothing)

                If resultScan = BS2ErrorCode.BS_SDK_SUCCESS Then
                    userBlob.user.numFaces = 1
                    Dim sizeOfFaceEx = Marshal.SizeOf(GetType(BS2FaceExWarped))
                    ptrFaceBuf = Marshal.AllocHGlobal(sizeOfFaceEx)
                    Marshal.StructureToPtr(faces(0), ptrFaceBuf, False)
                    userBlob.faceExObjs = ptrFaceBuf
                Else
                    MessageBox.Show("스캔 실패: " & resultScan.ToString)
                    Return
                End If

            ElseIf isLegacyFaceSupported Then
                MessageBox.Show("[Legacy Face] 얼굴 등록을 시작합니다.")
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
        ' ==================================================================

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

            Dim updateMask As Integer = BS2UserMaskEnum.FACE_EX Or BS2UserMaskEnum.NAME
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
    End Sub

    Private Sub btnSetServerMode_Click(sender As Object, e As EventArgs) Handles btnSetServerMode.Click
        If Not modSupremaFunc.IsDeviceConnected(sdkContext, connectedDeviceId) Then Return
        SetServerMatchingMode(sdkContext, connectedDeviceId, 1)   ' 서버에서 인증 (장비에는 얼굴등록 안 하고 서버 DB와 대조해서 서버에서 수동으로 문 열어준다.)
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

        ' Invoke: UI 작업이 끝날 때까지 SDK 통신 스레드를 붙잡고 있음 (장비 대기 발생 -> 딜레이 원인)
        ' BeginInvoke: "이거 나중에 처리해" 하고 SDK 스레드는 즉시 장비에 응답(ACK)을 보냄 (딜레이 해결)
        Me.BeginInvoke(Sub()
                           Dim curTime As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                           Dim eventName As String = CType(eventCode, BS2EventCodeEnum).ToString()

                           txtRealTimeLog.AppendText($"[{curTime}] ::: ID:{userId} / {eventName} (0x{eventCode:X4})" & vbCrLf)

                           Dim isFaceSuccess As Boolean = (eventCode = BS2EventCodeEnum.IDENTIFY_SUCCESS_FACE)
                           ' 장비에서 얼굴 인증 성공 이벤트인 경우에만 문 열기 시도
                           If isFaceSuccess Then

                               ' 문 열지 말지 결정하는 부분 
                               ' 게이트데몬에서 api 호출하는 부분 코딩해야함..



                               If userId = "1234" Then
                                   txtRealTimeLog.AppendText(">> [문을 연다] 얼굴 인증 성공 & ID 1234 확인됨!" & vbCrLf)

                                   'UnlockDoor(deviceId, 1)  ' 장비에 연결된 릴레이 번호를 아는 경우 이렇게 명령하고..(1번 릴레이를 열어라..)
                                   OpenRelay(deviceId, 0)  ' 장비에 연결된 릴레이 번호를 모를 경우 그냥 첫 번째 릴레이를 작동시켜라..
                                   'TestBuzzer(deviceId)  ' 테스트용으로 부저음 울리기
                               Else
                                   txtRealTimeLog.AppendText($">> [문을 열지않음] (ID: {userId})" & vbCrLf)
                               End If
                           End If
                       End Sub)
    End Sub
    ' 도어 ID를 지정하여 문을 여는 함수
    Private Sub UnlockDoor(deviceId As UInteger, targetDoorId As UInteger)

        Dim ptrDoorIds As IntPtr = Marshal.AllocHGlobal(4)
        Try
            Marshal.WriteInt32(ptrDoorIds, CInt(targetDoorId))
            Dim result As BS2ErrorCode = API.BS2_UnlockDoor(sdkContext, deviceId, BS2DoorFlagEnum.NONE, ptrDoorIds, 1)
            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                txtRealTimeLog.AppendText($">> [실제로 문이 열림]" & vbCrLf)
            Else
                txtRealTimeLog.AppendText($">> [실제로 문 열기 실패] : {result}" & vbCrLf)
            End If
        Finally
            Marshal.FreeHGlobal(ptrDoorIds)
        End Try

    End Sub
    ' Door ID가 없어도 릴레이를 직접 작동시킨다.
    ' relayIndex: 보통 0번이 1번 릴레이..
    ' deviceId: 장비 아이디
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

        ' actionUnion 배열을 초기화
        action.actionUnion = New Byte(31) {}

        ' RelayAction을 바이트 배열로 변환
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
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            txtRealTimeLog.AppendText(">> [성공] 릴레이 작동 명령 전송 완료!" & vbCrLf)
        Else
            txtRealTimeLog.AppendText($">> [실패] 릴레이 작동 실패 : {result}" & vbCrLf)
        End If

    End Sub
    Private Sub btnRemoveAllDoors_Click(sender As Object, e As EventArgs) Handles btnRemoveAllDoors.Click

        If Not IsDeviceConnected(sdkContext, connectedDeviceId) Then Return

        ' 도어설정을 삭제해야 수동으로 접점신호를 주는게 가능하다.
        Dim confirm = MessageBox.Show("장치에 설정된 도어정보가 모두 삭제됨!!!" & vbCrLf &
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
                txtDoorList.AppendText(">> 설정된 문(Door)이 없습니다." & vbCrLf)
                Return
            End If

            txtDoorList.AppendText($"총 {numDoor}개의 문이 검색되었습니다." & vbCrLf)

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

    Private Sub TestBuzzer(deviceId As UInteger)

        Dim action As New BS2Action()
        action.deviceID = deviceId
        action.type = BS2ActionTypeEnum.BUZZER
        action.stopFlag = 0
        action.delay = 0

        ' 부저 액션 세부 설정
        Dim buzzerAction As New BS2BuzzerAction()
        buzzerAction.count = 1                 ' 1회 재생
        buzzerAction.reserved = New Byte(1) {} ' 초기화

        buzzerAction.signal = New BS2BuzzerSignal(BS2Environment.BS2_BUZZER_SIGNAL_NUM - 1) {}

        ' 첫 번째 신호 설정 (삐- 소리)
        buzzerAction.signal(0).tone = BS2BuzzerToneEnum.HIGH  ' 높은소리
        buzzerAction.signal(0).fadeout = 0                    ' 페이드아웃 없음
        buzzerAction.signal(0).duration = 2000    ' 1000 : 1초
        buzzerAction.signal(0).delay = 0

        ' 나머지 신호 초기화 (사용 안 해도 초기화 필수)
        For i As Integer = 1 To BS2Environment.BS2_BUZZER_SIGNAL_NUM - 1
            buzzerAction.signal(i) = New BS2BuzzerSignal()
        Next

        ' 구조체를 바이트 배열로 변환하여 Union에 복사
        action.actionUnion = New Byte(31) {} ' 32바이트 할당
        Dim sizeBuzzer As Integer = Marshal.SizeOf(GetType(BS2BuzzerAction))
        Dim ptrBuzzer As IntPtr = Marshal.AllocHGlobal(sizeBuzzer)

        Try
            Marshal.StructureToPtr(buzzerAction, ptrBuzzer, False)
            Marshal.Copy(ptrBuzzer, action.actionUnion, 0, sizeBuzzer)
        Finally
            Marshal.FreeHGlobal(ptrBuzzer)
        End Try

        ' 명령 전송
        Dim result As BS2ErrorCode = API.BS2_RunAction(sdkContext, deviceId, action)

        Me.Invoke(Sub()
                      If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                          txtRealTimeLog.AppendText(">> [성공] 부저(Buzzer) 작동 명령 전송 완료! 실제 소리 확인필요!!" & vbCrLf)
                      Else
                          txtRealTimeLog.AppendText($">> [실패] 부저 작동 실패 : {result} ({CInt(result)})" & vbCrLf)
                      End If
                  End Sub)
    End Sub
    Private Sub btnDisableImageLog_Click(sender As Object, e As EventArgs) Handles btnDisableImageLog.Click
        DisableImageLog(connectedDeviceId)
    End Sub
    Private Sub DisableImageLog(deviceId As UInteger)

        If Not modSupremaFunc.IsDeviceConnected(sdkContext, deviceId) Then Return

        ' 1. 현재 이벤트 설정 가져오기
        Dim eventConfig As IntPtr = IntPtr.Zero
        Dim configSize As Integer = Marshal.SizeOf(GetType(BS2EventConfig))

        ' 구조체 메모리 할당
        Dim ptrConfig As IntPtr = Marshal.AllocHGlobal(configSize)
        Dim simpleConfig As New BS2EventConfig()

        Try
            ' 현재 설정 조회
            Dim result As BS2ErrorCode = API.BS2_GetEventConfig(sdkContext, deviceId, simpleConfig)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"설정 조회 실패: {result}")
                Return
            End If

            ' 2. 이미지 로그 필터 초기화 (이미지를 보낼 이벤트 개수를 0으로 설정)
            simpleConfig.numImageEventFilter = 0

            ' (참고: imageEventFilter 배열 등은 0으로 두면 됨)

            ' 3. 설정 적용
            result = API.BS2_SetEventConfig(sdkContext, deviceId, simpleConfig)

            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show("이미지 로그 전송 비활성화 성공!")
            Else
                MessageBox.Show($"설정 적용 실패: {result}")
            End If

        Finally
            If ptrConfig <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrConfig)
        End Try

    End Sub
End Class