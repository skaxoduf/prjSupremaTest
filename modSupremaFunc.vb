Imports System.Runtime.InteropServices
Imports prjSupremaTest.Suprema
Imports System.IO
Imports System.Windows.Forms ' MessageBox 사용을 위해 필요
Imports Suprema.API ' API 함수 호출을 위해 필요

Module modSupremaFunc

    ' 슈프리마 sdkContext 및 deviceId 유효성 검사
    Public Function IsDeviceConnected(sdkContext As IntPtr, deviceId As UInteger) As Boolean
        If sdkContext = IntPtr.Zero OrElse deviceId = 0 Then
            MessageBox.Show("장치에 먼저 연결해주세요.")
            Return False
        End If
        Return True
    End Function
    Public Sub SetServerMatchingMode(sdkContext As IntPtr, deviceId As UInteger, useServer As Byte)

        Dim authConfig As BS2AuthConfig
        Dim result As BS2ErrorCode = API.BS2_GetAuthConfig(sdkContext, deviceId, authConfig)
        If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
            MessageBox.Show($"설정 조회 실패: {result}")
            Return
        End If

        ' 모드 변경 (0: 장비 자체 인증, 1: 서버 인증)
        authConfig.useServerMatching = useServer
        If useServer = 0 Then  ' 장비인증모드
            ' 장비인증 모드일때는 이걸 0으로해야 장비가 자체적으로 인증해서 인증성공 메시지를 바로 표시해준다. 
            ' 이걸 1로하면은 장치에서 인증성공 메시지를 3초~4초후에 표시해준다. 그 이유는 문 열어줘도돼? 라고 서버에 물어보고 응답이 3~4초간 없으면 그때 인증성공 하기때문..
            ' 서버인증 모드일때는 이 값이 0이든 1이든 아무 상관없다. 
            authConfig.useGlobalAPB = 0
        End If

        ' 설정 적용
        result = API.BS2_SetAuthConfig(sdkContext, deviceId, authConfig)
        If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
            MessageBox.Show($"설정 변경 실패: {result}")
        End If

    End Sub
    ' jpg 이미지 파일에서 얼굴 데이터를 추출하는 함수
    Public Function GetFaceDataFromImage(sdkContext As IntPtr, deviceId As UInteger, imagePath As String, ByRef outTemplate As BS2TemplateEx, ByRef outWarpedImage As Byte()) As Boolean
        If Not File.Exists(imagePath) Then Return False

        Dim rawImageBytes As Byte() = File.ReadAllBytes(imagePath)
        Dim ptrRawImage As IntPtr = Marshal.AllocHGlobal(rawImageBytes.Length)

        Dim warpedBufferSize As Integer = BS2Environment.BS2_MAX_WARPED_IMAGE_LENGTH
        Dim ptrWarpedImage As IntPtr = Marshal.AllocHGlobal(warpedBufferSize)
        Dim warpedLen As UInteger = 0

        Try
            Marshal.Copy(rawImageBytes, 0, ptrRawImage, rawImageBytes.Length)
            ' 정규화 수행  (BS2_GetNormalizedImageFaceEx : 각도가 안맞거나 얼굴이 기울어진 경우 자동으로 보정해주는거..)
            Dim result As BS2ErrorCode = API.BS2_GetNormalizedImageFaceEx(sdkContext, deviceId, ptrRawImage, CUInt(rawImageBytes.Length), ptrWarpedImage, warpedLen)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"[1단계 실패] 얼굴 정규화 실패.{vbCrLf}오류: {result} ({CInt(result)})")
                Return False
            End If

            ' 템플릿 추출
            result = API.BS2_ExtractTemplateFaceEx(sdkContext, deviceId, ptrWarpedImage, warpedLen, 1, outTemplate) ' isWarped 파라미터를 1(True)로 설정
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"[2단계 실패] 템플릿 추출 실패.{vbCrLf}오류: {result} ({CInt(result)})")
                Return False
            End If
            ReDim outWarpedImage(CInt(warpedLen) - 1)
            Marshal.Copy(ptrWarpedImage, outWarpedImage, 0, CInt(warpedLen))
            Return True
        Finally
            If ptrRawImage <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrRawImage)
            If ptrWarpedImage <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrWarpedImage)
        End Try

    End Function
    ' Base64 문자열에서 얼굴 데이터를 추출하는 함수
    Public Function GetFaceDataFromBase64(sdkContext As IntPtr, deviceId As UInteger, base64String As String, ByRef outTemplate As BS2TemplateEx, ByRef outWarpedImage As Byte()) As Boolean

        If String.IsNullOrEmpty(base64String) Then Return False

        Dim rawImageBytes As Byte() = Nothing
        Try
            rawImageBytes = Convert.FromBase64String(base64String)
        Catch ex As Exception
            MessageBox.Show("Base64 디코딩 실패: " & ex.Message)
            Return False
        End Try

        Dim ptrRawImage As IntPtr = Marshal.AllocHGlobal(rawImageBytes.Length)
        Dim warpedBufferSize As Integer = BS2Environment.BS2_MAX_WARPED_IMAGE_LENGTH
        Dim ptrWarpedImage As IntPtr = Marshal.AllocHGlobal(warpedBufferSize)
        Dim warpedLen As UInteger = 0

        Try
            Marshal.Copy(rawImageBytes, 0, ptrRawImage, rawImageBytes.Length)
            ' 정규화 수행 (BS2_GetNormalizedImageFaceEx : 각도가 안맞거나 얼굴이 기울어진 경우 자동으로 보정해주는거..)
            Dim result As BS2ErrorCode = API.BS2_GetNormalizedImageFaceEx(sdkContext, deviceId, ptrRawImage, CUInt(rawImageBytes.Length), ptrWarpedImage, warpedLen)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"[1단계 실패] 얼굴 정규화 실패.{vbCrLf}오류: {result} ({CInt(result)})")
                Return False
            End If
            ' 템플릿 추출
            result = API.BS2_ExtractTemplateFaceEx(sdkContext, deviceId, ptrWarpedImage, warpedLen, 1, outTemplate) ' isWarped 파라미터를 1(True)로 설정
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show($"[2단계 실패] 템플릿 추출 실패.{vbCrLf}오류: {result} ({CInt(result)})")
                Return False
            End If
            ' 결과 반환
            ReDim outWarpedImage(CInt(warpedLen) - 1)
            Marshal.Copy(ptrWarpedImage, outWarpedImage, 0, CInt(warpedLen))
            Return True
        Finally
            If ptrRawImage <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrRawImage)
            If ptrWarpedImage <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrWarpedImage)
        End Try

    End Function


    ' 이미지 파일에서 템플릿을 추출하는 함수
    Public Function ExtractTemplateFromImage(sdkContext As IntPtr, deviceId As UInteger, imagePath As String, ByRef outTemplate As BS2TemplateEx) As Boolean

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
            Dim result As BS2ErrorCode = API.BS2_ExtractTemplateFaceEx(sdkContext, deviceId, ptrImage, CUInt(imageBytes.Length), 0, outTemplate)

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

    ' 1번도어 기본으로 등록후 무조건 도어아이디 1번으로 문 열라고 시키면 실제로 장치에서 접점이 먹는다.
    Public Function UnlockDoor(sdkContext As IntPtr, deviceId As UInteger, targetDoorId As UInteger) As BS2ErrorCode
        Dim ptrDoorIds As IntPtr = Marshal.AllocHGlobal(4)
        Try
            Marshal.WriteInt32(ptrDoorIds, CInt(targetDoorId))
            ' BS2_UnlockDoor(Context, DeviceID, Flag, DoorIDs, DoorCount)
            Return API.BS2_UnlockDoor(sdkContext, deviceId, BS2DoorFlagEnum.NONE, ptrDoorIds, 1)
        Finally
            Marshal.FreeHGlobal(ptrDoorIds)
        End Try
    End Function

    ' 릴레이를 직접 작동시키는 함수 (Door ID 없이 제어)
    ' relayIndex: 보통 0번이 1번 릴레이
    ' 테스트해보면 실제 장치가 작동하지 않는다.  
    Public Function OpenRelay(sdkContext As IntPtr, deviceId As UInteger, relayIndex As Integer) As BS2ErrorCode
        Dim action As New BS2Action()
        action.deviceID = deviceId
        action.type = BS2ActionTypeEnum.RELAY
        action.stopFlag = 0
        action.delay = 0

        ' 릴레이 액션 세부 설정
        Dim relayAction As New BS2RelayAction()
        relayAction.relayIndex = CByte(relayIndex)
        relayAction.reserved = New Byte(2) {} ' 배열 초기화

        ' 신호 설정 (3초간 켜짐)
        relayAction.signal.signalID = 0
        relayAction.signal.count = 1          ' 1번 작동
        relayAction.signal.onDuration = 3000  ' 3000ms (3초) 켜짐
        relayAction.signal.offDuration = 0
        relayAction.signal.delay = 0

        ' actionUnion 배열 할당 (넉넉하게 잡음)
        action.actionUnion = New Byte(BS2Environment.BS2_MAX_TRIGGER_ACTION - 1) {}

        Dim sizeRelay As Integer = Marshal.SizeOf(GetType(BS2RelayAction))
        Dim ptrRelay As IntPtr = Marshal.AllocHGlobal(sizeRelay)

        Try
            Marshal.StructureToPtr(relayAction, ptrRelay, False)
            Marshal.Copy(ptrRelay, action.actionUnion, 0, sizeRelay)

            ' 장비에 액션 실행 명령 전송
            Return API.BS2_RunAction(sdkContext, deviceId, action)
        Finally
            Marshal.FreeHGlobal(ptrRelay)
        End Try
    End Function




End Module
