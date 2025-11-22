Imports System.Net
Imports System.Runtime.InteropServices
Imports prjSupremaTest.Suprema
Imports System.IO
Imports Suprema
Imports Suprema.API

Public Class Form1

    Private sdkContext As IntPtr = IntPtr.Zero
    Private connectedDeviceId As UInteger = 0
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

        ' [핵심 변경] 복잡한 구조체 배열 대신 '단순 바이트 배열'로 선언하여 메모리 꼬임 방지
        ' 크기 = 템플릿 개수(20) * 템플릿 사이즈(556: data 552 + isIR 1 + reserved 3) = 11120
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=11120)>
        Public templateExBlob As Byte()
    End Structure

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

                MessageBox.Show($"총 {numDevice}대의 장치를 찾았습니다.")

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
            infoString.AppendLine($"Face Supported: {Convert.ToBoolean(deviceInfo.faceSupported)}")
            infoString.AppendLine($"Card Supported: {Convert.ToBoolean(deviceInfo.cardSupported)}")
            infoString.AppendLine($"PIN Supported: {Convert.ToBoolean(deviceInfo.pinSupported)}")
            infoString.AppendLine($"WLAN Supported: {Convert.ToBoolean(deviceInfo.wlanSupported)}")


            ' ---------------------------------------------------------
            ' 2. 상세 능력치 조회 (BS2_GetDeviceCapabilities) - 여기서 진짜 얼굴인식 지원 여부 확인
            ' ---------------------------------------------------------
            Dim cap As New BS2DeviceCapabilities()
            Dim capResult As BS2ErrorCode = API.BS2_GetDeviceCapabilities(sdkContext, selectedDeviceId, cap)

            If capResult = BS2ErrorCode.BS_SDK_SUCCESS Then
                infoString.AppendLine(vbCrLf & "=== [2] 신형 장비 상세 능력치 (Capabilities) ===")

                ' FaceEx (Visual Face) 지원 여부 확인 비트 연산
                ' SYSTEM_SUPPORT_FACEEX = 0x40 (64)
                Dim isFaceExSupported As Boolean = (cap.systemSupported And BS2CapabilitySystemSupport.SYSTEM_SUPPORT_FACEEX) > 0

                infoString.AppendLine($"Visual Face (FaceEx) Supported: {isFaceExSupported}")
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

        ' 1. SDK 초기화 및 입력값 확인
        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
        End If

        If txtDeviceID.Text.Trim = "" Then
            MessageBox.Show("연결할 장치 ID를 입력하세요.")
            Return
        End If

        ' 2. 텍스트박스에서 목표 Device ID 가져오기
        Dim targetDeviceId As UInteger = 0
        Try
            targetDeviceId = UInteger.Parse(txtDeviceID.Text.Trim)
        Catch ex As Exception
            MessageBox.Show("올바른 숫자 형식의 ID를 입력하세요.")
            Return
        End Try

        ' 3. 기존 연결 해제
        If connectedDeviceId <> 0 Then
            BS2_DisconnectDevice(sdkContext, connectedDeviceId)
            connectedDeviceId = 0
        End If

        ' 4-1. 바로 연결 시도해보기
        Dim result As BS2ErrorCode = BS2_ConnectDevice(sdkContext, targetDeviceId)

        ' 4-2. 만약 실패했다면? -> "검색(Search)"을 한번 돌려서 IP를 알아내고 다시 시도
        If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
            ' 검색 타임아웃 설정 (3~5초)
            BS2_SetDeviceSearchingTimeout(sdkContext, 7)
            ' 검색 수행
            BS2_SearchDevices(sdkContext)
            ' 다시 연결 시도
            result = CType(BS2_ConnectDevice(sdkContext, targetDeviceId), BS2ErrorCode)
        End If

        ' 5. 최종 결과 처리
        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            connectedDeviceId = targetDeviceId
            MessageBox.Show("장치 연결 성공! ID: " & targetDeviceId)
        Else
            MessageBox.Show($"연결 실패.{vbCrLf}먼저 '검색' 버튼을 눌러 장치가 리스트에 뜨는지 확인하거나,{vbCrLf}IP 연결 방식을 사용하세요.{vbCrLf}오류 코드: {CInt(result)}")
        End If


    End Sub

    Private Sub btnEnrollUser_Click(sender As Object, e As EventArgs) Handles btnEnrollUser.Click

        ' 1. 연결 상태 확인
        If sdkContext = IntPtr.Zero OrElse connectedDeviceId = 0 Then
            MessageBox.Show("장치에 먼저 연결해주세요.")
            Return
        End If

        API.BS2_SetDefaultResponseTimeout(sdkContext, 10 * 1000)  ' 타임아우스 10초설정

        ' 2. 장치 정보 조회
        Dim deviceInfo As BS2SimpleDeviceInfo
        API.BS2_GetDeviceInfo(sdkContext, connectedDeviceId, deviceInfo)

        Dim cap As New BS2DeviceCapabilities()
        API.BS2_GetDeviceCapabilities(sdkContext, connectedDeviceId, cap)


        ' 안면 지원 여부 확인
        Dim isFaceExSupported As Boolean = (cap.systemSupported And BS2CapabilitySystemSupport.SYSTEM_SUPPORT_FACEEX) > 0
        Dim isLegacyFaceSupported As Boolean = Convert.ToBoolean(deviceInfo.faceSupported)

        ' 3. 사용자 구조체 초기화
        Dim userBlob As New BS2UserFaceExBlob()
        userBlob.user.userID = New Byte(BS2Environment.BS2_USER_ID_SIZE - 1) {}
        userBlob.name = New Byte(BS2Environment.BS2_USER_NAME_LEN - 1) {}
        userBlob.pin = New Byte(BS2Environment.BS2_PIN_HASH_SIZE - 1) {}

        userBlob.cardObjs = IntPtr.Zero
        userBlob.fingerObjs = IntPtr.Zero
        userBlob.faceObjs = IntPtr.Zero
        userBlob.faceExObjs = IntPtr.Zero

        ' 4. ID 설정 (예시)
        Dim newUserId As String = txtUserID.Text.Trim()
        Dim uidBytes = System.Text.Encoding.UTF8.GetBytes(newUserId)
        Array.Copy(uidBytes, userBlob.user.userID, Math.Min(uidBytes.Length, userBlob.user.userID.Length))

        ' 기본 설정
        Dim now As DateTime = DateTime.Now
        userBlob.setting.startTime = CType(Util.ConvertToUnixTimestamp(now), UInt32)
        userBlob.setting.endTime = CType(Util.ConvertToUnixTimestamp(now.AddYears(10)), UInt32)
        userBlob.setting.idAuthMode = BS2AuthModeEnum.BS2_AUTH_MODE_NONE
        userBlob.setting.securityLevel = BS2UserSecurityLevelEnum.DEFAULT

        ' ==================================================================
        ' (C) 얼굴 등록 로직 (라디오 버튼 분기)
        ' ==================================================================
        Dim ptrFaceBuf As IntPtr = IntPtr.Zero

        ' >>> [CASE 1] JPG 파일 불러오기 <<<
        If rbLoadImage.Checked Then

            Dim imagePath As String = txtImagePath.Text.Trim()
            If String.IsNullOrEmpty(imagePath) OrElse Not System.IO.File.Exists(imagePath) Then
                MessageBox.Show("유효한 이미지 파일 경로를 입력해주세요.")
                Return
            End If

            If isFaceExSupported Then
                Dim extractedTemplate As New BS2TemplateEx()
                Dim warpedImageBytes As Byte() = Nothing ' 정규화된 이미지를 받을 변수

                ' [수정] 새로 만든 함수 호출
                If GetFaceDataFromImage(imagePath, extractedTemplate, warpedImageBytes) Then
                    userBlob.user.numFaces = 1

                    ' 표준 구조체(BS2FaceExWarped) 사용
                    Dim faceExWarped As New BS2FaceExWarped()
                    faceExWarped.faceIndex = 0
                    faceExWarped.numOfTemplate = 1

                    ' 재부팅 방지를 위해 Flag=1 (WARPED) 유지
                    faceExWarped.flag = 1
                    faceExWarped.reserved = 0

                    ' 배열 초기화 (이거 안 하면 에러 뜸)
                    faceExWarped.unused = New Byte(5) {}

                    ' 이미지 데이터 복사
                    faceExWarped.imageLen = CUInt(warpedImageBytes.Length)
                    faceExWarped.imageData = New Byte(BS2Environment.BS2_MAX_WARPED_IMAGE_LENGTH - 1) {}
                    Array.Copy(warpedImageBytes, faceExWarped.imageData, warpedImageBytes.Length)

                    ' IR 데이터 초기화 (필수)
                    faceExWarped.irImageLen = 0
                    faceExWarped.irImageData = New Byte(BS2Environment.BS2_MAX_WARPED_IR_IMAGE_LENGTH - 1) {}

                    ' 템플릿 배열 초기화 및 할당
                    ' Safe 구조체처럼 바이트 복사가 아니라, 구조체 배열에 대입하는 원래 방식입니다.
                    faceExWarped.templateEx = New BS2TemplateEx(BS2Environment.BS2_MAX_TEMPLATES_PER_FACE_EX - 1) {}

                    ' 0번 인덱스에 추출한 템플릿 넣기
                    faceExWarped.templateEx(0) = extractedTemplate

                    ' 나머지 19개 슬롯도 Nothing이 되지 않도록 초기화
                    For i As Integer = 1 To BS2Environment.BS2_MAX_TEMPLATES_PER_FACE_EX - 1
                        faceExWarped.templateEx(i) = New BS2TemplateEx()
                        faceExWarped.templateEx(i).data = New Byte(BS2Environment.BS2_FACE_EX_TEMPLATE_SIZE - 1) {}
                        faceExWarped.templateEx(i).reserved = New Byte(2) {}
                    Next

                    ' 메모리 할당 및 연결
                    Dim sizeOfFaceEx As Integer = Marshal.SizeOf(GetType(BS2FaceExWarped))
                    ptrFaceBuf = Marshal.AllocHGlobal(sizeOfFaceEx)

                    ' 구조체를 포인터로 변환 (여기서 재부팅 여부가 갈림)
                    Marshal.StructureToPtr(faceExWarped, ptrFaceBuf, False)
                    userBlob.faceExObjs = ptrFaceBuf


                    '' [구버전 - 문제 발생시 아래 코드로 구조체 사용 시도
                    'userBlob.user.numFaces = 1

                    '' [수정] Safe 구조체 사용
                    'Dim faceExSafe As New BS2FaceExWarped_Safe()
                    'faceExSafe.faceIndex = 0
                    'faceExSafe.numOfTemplate = 1
                    'faceExSafe.flag = 1
                    'faceExSafe.unused = New Byte(5) {}

                    '' 2. 이미지 데이터 복사
                    'faceExSafe.imageLen = CUInt(warpedImageBytes.Length)
                    'faceExSafe.imageData = New Byte(BS2Environment.BS2_MAX_WARPED_IMAGE_LENGTH - 1) {}
                    'Array.Copy(warpedImageBytes, faceExSafe.imageData, warpedImageBytes.Length)

                    '' 3. IR 데이터 초기화
                    'faceExSafe.irImageLen = 0
                    'faceExSafe.irImageData = New Byte(BS2Environment.BS2_MAX_WARPED_IR_IMAGE_LENGTH - 1) {}

                    '' 4. [핵심] 템플릿 데이터를 바이트 배열(Blob)로 직접 복사
                    '' 구조체(BS2TemplateEx)를 바이트 배열로 변환하여 flat하게 넣습니다.
                    'Dim templateTotalSize As Integer = 11120 ' 20개 * 556바이트
                    'faceExSafe.templateExBlob = New Byte(templateTotalSize - 1) {}

                    '' 추출된 템플릿(1개)을 바이트로 변환
                    'Dim tempSize As Integer = Marshal.SizeOf(GetType(BS2TemplateEx))
                    'Dim ptrTemp As IntPtr = Marshal.AllocHGlobal(tempSize)

                    'Try
                    '    ' 구조체 -> 포인터 -> 바이트 배열 복사
                    '    Marshal.StructureToPtr(extractedTemplate, ptrTemp, False)
                    '    Marshal.Copy(ptrTemp, faceExSafe.templateExBlob, 0, tempSize)
                    '    ' (나머지 19개 슬롯은 이미 0으로 초기화되어 있으므로 신경 안 써도 됨)
                    'Finally
                    '    Marshal.FreeHGlobal(ptrTemp)
                    'End Try

                    '' 5. 메모리 할당 및 연결 (Safe 구조체 사용)
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
                Dim resultScan As BS2ErrorCode = API.BS2_ScanFaceEx(sdkContext, connectedDeviceId, faces, BS2FaceEnrollThreshold.THRESHOLD_DEFAULT, Nothing)

                If resultScan = BS2ErrorCode.BS_SDK_SUCCESS Then
                    userBlob.user.numFaces = 1
                    Dim sizeOfFaceEx As Integer = Marshal.SizeOf(GetType(BS2FaceExWarped))
                    ptrFaceBuf = Marshal.AllocHGlobal(sizeOfFaceEx)
                    Marshal.StructureToPtr(faces(0), ptrFaceBuf, False)
                    userBlob.faceExObjs = ptrFaceBuf
                Else
                    MessageBox.Show("스캔 실패: " & resultScan.ToString())
                    Return
                End If

            ElseIf isLegacyFaceSupported Then
                MessageBox.Show("[Legacy Face] 얼굴 등록을 시작합니다.")
                Dim faces(0) As BS2Face
                Dim resultScan As BS2ErrorCode = API.BS2_ScanFace(sdkContext, connectedDeviceId, faces, BS2FaceEnrollThreshold.THRESHOLD_DEFAULT, Nothing)

                If resultScan = BS2ErrorCode.BS_SDK_SUCCESS Then
                    userBlob.user.numFaces = 1
                    Dim sizeOfFace As Integer = Marshal.SizeOf(GetType(BS2Face))
                    ptrFaceBuf = Marshal.AllocHGlobal(sizeOfFace)
                    Marshal.StructureToPtr(faces(0), ptrFaceBuf, False)
                    userBlob.faceObjs = ptrFaceBuf
                Else
                    MessageBox.Show("스캔 실패: " & resultScan.ToString())
                    Return
                End If
            End If
        End If
        ' ==================================================================

        ' (D) 이름 설정
        If Convert.ToBoolean(deviceInfo.userNameSupported) Then
            Dim userName As String = txtMemNm.Text.Trim()
            Dim nameBytes = System.Text.Encoding.UTF8.GetBytes(userName)
            Array.Copy(nameBytes, userBlob.name, Math.Min(nameBytes.Length, userBlob.name.Length))
        End If

        ' 6. 등록 전송
        Try
            Dim userList As BS2UserFaceExBlob() = {userBlob}
            Dim result As BS2ErrorCode = API.BS2_EnrollUserFaceEx(sdkContext, connectedDeviceId, userList, 1, 1)

            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"사용자({newUserId}) 등록 성공!")
            Else
                MessageBox.Show($"등록 전송 실패. 오류 코드: {result}")
            End If
        Finally
            If ptrFaceBuf <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrFaceBuf)
        End Try

    End Sub

    ' 이미지 파일에서 템플릿을 추출하는 함수
    Private Function ExtractTemplateFromImage(imagePath As String, ByRef outTemplate As BS2TemplateEx) As Boolean

        ' 1. 파일 존재 확인
        If Not File.Exists(imagePath) Then
            MessageBox.Show("이미지 파일이 없습니다: " & imagePath)
            Return False
        End If

        ' 2. JPG 파일 읽기 (바이트 배열)
        Dim imageBytes As Byte() = File.ReadAllBytes(imagePath)

        ' 3. 비관리 메모리 할당 및 복사
        Dim ptrImage As IntPtr = Marshal.AllocHGlobal(imageBytes.Length)
        Try
            Marshal.Copy(imageBytes, 0, ptrImage, imageBytes.Length)

            ' 4. 템플릿 추출 API 호출 (BS2_ExtractTemplateFaceEx)
            ' SFApi.vb 에 정의된 함수 사용
            ' isWarped: 0 (일반 이미지인 경우)
            Dim result As BS2ErrorCode = API.BS2_ExtractTemplateFaceEx(sdkContext, connectedDeviceId, ptrImage, CUInt(imageBytes.Length), 0, outTemplate)

            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                Return True
            Else
                Dim errCode As Integer = CInt(result)
                Dim msg As String = ""
                Select Case errCode
                    Case -300 ' BS_SDK_ERROR_EXTRACTION_FAIL
                        msg = "얼굴 특징 추출 실패 (일반적인 실패, 이미지가 손상되었거나 지원되지 않는 포맷일 수 있음)"
                    Case -308 ' BS_SDK_ERROR_EXTRACTION_LOW_QUALITY
                        msg = "이미지 품질이 너무 낮습니다. (흐릿함, 노이즈, 조명 불량)"
                    Case -314 ' BS_SDK_ERROR_CANNOT_FIND_FACE
                        msg = "얼굴을 찾을 수 없습니다. (배경이 복잡하거나 얼굴이 아님)"
                    Case -321 ' BS_SDK_ERROR_CANNOT_ESTIMATE
                        msg = "얼굴 랜드마크(눈, 코, 입)를 추정할 수 없습니다."
                    Case -322 ' BS_SDK_ERROR_NORMALIZE_FACE
                        msg = "얼굴 정규화 실패 (얼굴 각도가 너무 틀어짐)"
                    Case -323 ' BS_SDK_ERROR_SMALL_DETECTION
                        msg = "얼굴 크기가 너무 작습니다. (해상도를 높이거나 얼굴을 크게 편집)"
                    Case -324 ' BS_SDK_ERROR_LARGE_DETECTION
                        msg = "얼굴 크기가 너무 큽니다. (해상도를 줄이세요)"
                    Case -325 ' BS_SDK_ERROR_BIASED_DETECTION
                        msg = "얼굴이 한쪽으로 치우쳐 있습니다. (중앙에 위치시켜 주세요)"
                    Case -326 ' BS_SDK_ERROR_ROTATED_FACE
                        msg = "얼굴이 기울어져 있습니다. (정면을 똑바로 응시해야 함)"
                    Case -327 ' BS_SDK_ERROR_OVERLAPPED_FACE
                        msg = "얼굴이 겹쳐 보이거나 여러 명의 얼굴이 감지되었습니다."
                    Case -328 ' BS_SDK_ERROR_UNOPENED_EYES
                        msg = "눈을 감고 있거나 안경/빛 반사로 눈을 찾을 수 없습니다."
                    Case -329 ' BS_SDK_ERROR_NOT_LOOKING_FRONT
                        msg = "시선이 정면이 아닙니다. (측면, 위, 아래)"
                    Case -330 ' BS_SDK_ERROR_OCCLUDED_MOUTH
                        msg = "입이 가려져 있습니다. (마스크 등)"
                    Case -332 ' BS_SDK_ERROR_INCOMPATIBLE_FACE
                        msg = "호환되지 않는 얼굴 데이터입니다."
                    Case -10008 ' BS_SDK_ERROR_INTERNAL
                        msg = "장치 내부 오류 (장치 재부팅 필요할 수 있음)"
                    Case Else
                        msg = "알 수 없는 오류"
                End Select
                MessageBox.Show($"템플릿 추출 실패.{vbCrLf}코드: {errCode} ({result}){vbCrLf}원인: {msg}")
                Return False
            End If

        Finally
            ' 메모리 해제
            If ptrImage <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrImage)
        End Try

    End Function
    ' 테스트용: 장비가 얼굴을 제대로 정규화(Normalize) 하는지 확인하는 함수
    ' [테스트용 함수] 장비가 얼굴 이미지를 받아들일 수 있는지 확인
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
            ' 메모리 해제
            If ptrRawImage <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrRawImage)
            If ptrWarpedImage <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrWarpedImage)
        End Try
    End Sub
    ' [수정된 함수] 원본 이미지를 정규화(Warping)한 뒤, 그 이미지로 템플릿을 추출합니다.
    ' outTemplate: 추출된 템플릿 반환
    ' outWarpedImage: 정규화된 이미지 데이터 반환 (등록 시 이 이미지를 써야 함)
    Private Function GetFaceDataFromImage(imagePath As String, ByRef outTemplate As BS2TemplateEx, ByRef outWarpedImage As Byte()) As Boolean
        If Not File.Exists(imagePath) Then Return False

        Dim rawImageBytes As Byte() = File.ReadAllBytes(imagePath)
        Dim ptrRawImage As IntPtr = Marshal.AllocHGlobal(rawImageBytes.Length)

        ' Warped Image(펴진 얼굴)를 받을 버퍼 (최대 40KB 정도)
        Dim warpedBufferSize As Integer = BS2Environment.BS2_MAX_WARPED_IMAGE_LENGTH
        Dim ptrWarpedImage As IntPtr = Marshal.AllocHGlobal(warpedBufferSize)
        Dim warpedLen As UInteger = 0

        Try
            ' 1. 원본 메모리 복사
            Marshal.Copy(rawImageBytes, 0, ptrRawImage, rawImageBytes.Length)

            ' 2. [1단계] 정규화(Warping) 수행 - 성공했던 그 기능!
            Dim result As BS2ErrorCode = API.BS2_GetNormalizedImageFaceEx(sdkContext, connectedDeviceId, ptrRawImage, CUInt(rawImageBytes.Length), ptrWarpedImage, warpedLen)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"[1단계 실패] 얼굴 정규화 실패.{vbCrLf}오류: {result} ({CInt(result)})")
                Return False
            End If

            ' 3. [2단계] 정규화된 이미지(Warped)로 템플릿 추출
            ' 중요: isWarped 파라미터를 1(True)로 설정해야 합니다.
            result = API.BS2_ExtractTemplateFaceEx(sdkContext, connectedDeviceId, ptrWarpedImage, warpedLen, 1, outTemplate)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"[2단계 실패] 템플릿 추출 실패.{vbCrLf}오류: {result} ({CInt(result)})")
                Return False
            End If

            ' 4. 성공 시, Warped Image 데이터를 바이트 배열로 반환
            ReDim outWarpedImage(CInt(warpedLen) - 1)
            Marshal.Copy(ptrWarpedImage, outWarpedImage, 0, CInt(warpedLen))

            Return True

        Finally
            If ptrRawImage <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrRawImage)
            If ptrWarpedImage <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrWarpedImage)
        End Try
    End Function
    ' [폼 로드 시 초기 설정]
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 초기 상태 설정
        UpdateImageUI()
    End Sub

    ' [라디오 버튼 변경 시 UI 상태 업데이트]
    Private Sub rbLoadImage_CheckedChanged(sender As Object, e As EventArgs) Handles rbLoadImage.CheckedChanged
        UpdateImageUI()
    End Sub

    Private Sub UpdateImageUI()
        ' 파일 불러오기 모드일 때만 텍스트박스 활성화
        txtImagePath.Enabled = rbLoadImage.Checked
        If Not rbLoadImage.Checked Then
            txtImagePath.Clear()
        End If
    End Sub

    ' [드래그 들어올 때 이벤트] - 마우스 커서 모양 변경
    Private Sub txtImagePath_DragEnter(sender As Object, e As DragEventArgs) Handles txtImagePath.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    ' [드래그 놓았을 때 이벤트] - 파일 경로 가져오기
    Private Sub txtImagePath_DragDrop(sender As Object, e As DragEventArgs) Handles txtImagePath.DragDrop
        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
        If files.Length > 0 Then
            ' 첫 번째 파일의 경로를 확인
            Dim filePath As String = files(0)
            Dim ext As String = System.IO.Path.GetExtension(filePath).ToLower()

            ' JPG/PNG 파일인지 확장자 체크
            If ext = ".jpg" OrElse ext = ".jpeg" OrElse ext = ".png" Then
                txtImagePath.Text = filePath
            Else
                MessageBox.Show("이미지 파일(jpg, png)만 지원합니다.")
            End If
        End If
    End Sub

    Private Sub btnTestImage_Click(sender As Object, e As EventArgs) Handles btnTestImage.Click

        ' 1. SDK 초기화 확인
        If sdkContext = IntPtr.Zero OrElse connectedDeviceId = 0 Then
            MessageBox.Show("장치에 먼저 연결해주세요.")
            Return
        End If

        ' 2. 이미지 경로 확인
        Dim imagePath As String = txtImagePath.Text.Trim()
        If String.IsNullOrEmpty(imagePath) OrElse Not System.IO.File.Exists(imagePath) Then
            MessageBox.Show("텍스트박스에 유효한 이미지 경로가 없거나 파일을 찾을 수 없습니다.")
            Return
        End If

        ' 3. 테스트 함수 호출
        TestNormalizeImage(imagePath)

    End Sub
End Class