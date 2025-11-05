Imports System.Net
Imports System.Runtime.InteropServices
Imports prjSupremaTest.Suprema
Imports Suprema
Imports Suprema.API

Public Class Form1

    Private sdkContext As IntPtr = IntPtr.Zero
    Private Sub btnDLLLoad_Click(sender As Object, e As EventArgs) Handles btnDLLLoad.Click

        If sdkContext <> IntPtr.Zero Then
            MessageBox.Show("SDK가 이미 초기화되었습니다.")
            Return
        End If

        Dim result As BS2ErrorCode
        sdkContext = BS2_AllocateContext()

        If sdkContext <> IntPtr.Zero Then
            result = CType(BS2_Initialize(sdkContext), BS2ErrorCode)

            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show("DLL 초기화 성공!")
            Else
                BS2_ReleaseContext(sdkContext)
                sdkContext = IntPtr.Zero
                MessageBox.Show("DLL 초기화 실패. 오류 코드: " & result)
            End If
        Else
            MessageBox.Show("컨텍스트 할당 실패 (메모리 부족 등...)")
        End If

    End Sub


    Private Sub btnDeviceConn_Click(sender As Object, e As EventArgs) Handles btnDeviceConn.Click

        'BioStar 애플리케이션과 장치는 server mode와 direct mode로 연결할 수 있습니다.
        'server mode는 장치가 BioStar 애플리케이션으로 신호를 보내서 연결하는 방식이고,
        'direct mode는 BioStar 애플리케이션이 장치로 신호를 보내서 연결하는 방식입니다.
        '장치는 direct mode가 초기값으로 설정되어 있으며,
        'direct mode로 연결하는 방법은 아래와 같습니다.


        ' IP 주소와 Port를 이미 알고 있는 경우

        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
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
                MessageBox.Show("장치 연결 성공! Device ID: " & deviceId)
            Else
                MessageBox.Show("장치 연결 실패. 오류 코드: 0x" & result.ToString("X"))
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


        ' 장치를 검색
        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
        End If

        ' --- 리스트박스 초기화 ---
        lstDevices.Items.Clear()

        Dim result As BS2ErrorCode
        result = CType(BS2_SearchDevices(sdkContext), BS2ErrorCode)

        If result = BS2ErrorCode.BS_SDK_SUCCESS Then
            Dim deviceListObj = IntPtr.Zero
            Dim numDevice As UInteger = 0

            result = CType(BS2_GetDevices(sdkContext, deviceListObj, numDevice), BS2ErrorCode)

            If result = BS2ErrorCode.BS_SDK_SUCCESS AndAlso numDevice > 0 Then

                MessageBox.Show($"총 {numDevice}대의 장치를 찾았습니다.")

                ' --- 찾은 장치 개수(numDevice)만큼 반복 ---
                For idx As Integer = 0 To numDevice - 1
                    ' 목록에서 idx 번째 장치의 ID를 읽어옵니다. (UInt32는 4바이트)
                    Dim currentDeviceId As UInteger = Marshal.ReadInt32(deviceListObj, idx * 4)

                    ' 장치 ID로 상세 정보(이름, IP 등)를 가져옵니다.
                    Dim deviceInfo As BS2SimpleDeviceInfo
                    Dim infoResult As BS2ErrorCode = CType(API.BS2_GetDeviceInfo(sdkContext, currentDeviceId, deviceInfo), BS2ErrorCode)

                    If infoResult = BS2ErrorCode.BS_SDK_SUCCESS Then
                        ' 리스트박스에 표시할 문자열 생성
                        Dim deviceType As BS2DeviceTypeEnum = CType(deviceInfo.type, BS2DeviceTypeEnum)
                        Dim deviceName As String = If(API.productNameDictionary.ContainsKey(deviceType), API.productNameDictionary(deviceType), "Unknown")
                        Dim ip As String = New IPAddress(BitConverter.GetBytes(deviceInfo.ipv4Address)).ToString()

                        Dim displayText As String = $"ID: {currentDeviceId} | IP: {ip} | Type: {deviceName}"

                        ' 리스트박스에 항목 추가
                        lstDevices.Items.Add(displayText)
                    Else
                        ' deviceInfo 가져오기 실패 시
                        lstDevices.Items.Add($"ID: {currentDeviceId} | (정보 가져오기 실패)")
                    End If
                Next

                BS2_ReleaseObject(deviceListObj) ' 리소스 해제

            ElseIf numDevice = 0 Then
                MessageBox.Show("검색된 장치가 없습니다.")
            Else
                MessageBox.Show("장치 목록 가져오기 실패. 오류 코드: 0x" & result.ToString("X"))
            End If
        Else
            MessageBox.Show("장치 검색 실패. 오류 코드: 0x" & result.ToString("X"))
        End If

    End Sub
    Private Sub btnConnectSelected_Click(sender As Object, e As EventArgs) Handles btnConnectSelected.Click

        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
        End If

        ' 리스트박스에서 선택된 항목이 있는지 확인
        If lstDevices.SelectedItem Is Nothing Then
            MessageBox.Show("먼저 목록에서 연결할 장치를 선택하세요.")
            Return
        End If

        ' 선택된 항목의 문자열을 가져옵니다. (예: "ID: 123456 | ...")
        Dim selectedString As String = lstDevices.SelectedItem.ToString()

        ' 문자열에서 ID만 파싱(Parsing)합니다.
        Try
            Dim parts() As String = selectedString.Split({"ID: ", " |"}, StringSplitOptions.RemoveEmptyEntries)
            Dim deviceIdToConnect As UInteger = UInteger.Parse(parts(0).Trim())

            ' 선택된 ID로 연결 시도
            Dim result As BS2ErrorCode = CType(BS2_ConnectDevice(sdkContext, deviceIdToConnect), BS2ErrorCode)

            If result = BS2ErrorCode.BS_SDK_SUCCESS Then
                MessageBox.Show("장치 연결 성공! ID: " & deviceIdToConnect)
            Else
                MessageBox.Show("장치 연결 실패. 오류 코드: 0x" & result.ToString("X"))
            End If

        Catch ex As Exception
            MessageBox.Show("선택된 장치 ID를 파싱하는 데 실패했습니다. " & ex.Message)
        End Try

    End Sub

    Private Sub btnGetDeviceInfo_Click(sender As Object, e As EventArgs) Handles btnGetDeviceInfo.Click

        '장치와 연결이 완료되었다면 장치 정보를 가져와야 합니다.
        '장치 종류에 따라 일부 기능이 지원되지 않기 때문에 BioStar 애플리케이션은 장치에 맞춰 UI를 구성해야 합니다.
        '1) 장치 정보를 가져오기 위해서는 BS2_GetDeviceInfo 함수를 사용합니다.


        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
        End If

        If lstDevices.SelectedItem Is Nothing Then
            MessageBox.Show("먼저 목록에서 정보를 조회할 장치를 선택하세요.")
            Return
        End If

        txtDeviceInfo.Clear()

        Dim selectedDeviceId As UInteger = 0

        Try
            Dim selectedString As String = lstDevices.SelectedItem.ToString()
            Dim parts() As String = selectedString.Split({"ID: ", " |"}, StringSplitOptions.RemoveEmptyEntries)
            selectedDeviceId = UInteger.Parse(parts(0).Trim())
        Catch ex As Exception
            MessageBox.Show("선택된 장치 ID를 파싱하는 데 실패했습니다. " & ex.Message)
            Return
        End Try

        Dim deviceInfo As BS2SimpleDeviceInfo
        Dim getInfoResult As BS2ErrorCode = CType(API.BS2_GetDeviceInfo(sdkContext, selectedDeviceId, deviceInfo), BS2ErrorCode)

        If getInfoResult = BS2ErrorCode.BS_SDK_SUCCESS Then

            Dim infoString As New System.Text.StringBuilder()
            infoString.AppendLine("장치 정보 가져오기 성공!")
            infoString.AppendLine("--------------------------")
            infoString.AppendLine($"Device ID: {deviceInfo.id}")

            Dim deviceType As BS2DeviceTypeEnum = CType(deviceInfo.type, BS2DeviceTypeEnum)
            Dim deviceName As String = "Unknown"
            If API.productNameDictionary.ContainsKey(deviceType) Then
                deviceName = API.productNameDictionary(deviceType)
            End If
            infoString.AppendLine($"Device Type: {deviceName} (Enum: {deviceInfo.type})")

            Dim ip As String = New IPAddress(BitConverter.GetBytes(deviceInfo.ipv4Address)).ToString()
            infoString.AppendLine($"IP Address: {ip}")
            infoString.AppendLine($"Port: {deviceInfo.port}")
            infoString.AppendLine($"Max Users: {deviceInfo.maxNumOfUser}")
            infoString.AppendLine($"Fingerprint Supported: {Convert.ToBoolean(deviceInfo.fingerSupported)}")
            infoString.AppendLine($"Face Supported: {Convert.ToBoolean(deviceInfo.faceSupported)}")
            infoString.AppendLine($"Card Supported: {Convert.ToBoolean(deviceInfo.cardSupported)}")
            infoString.AppendLine($"PIN Supported: {Convert.ToBoolean(deviceInfo.pinSupported)}")
            infoString.AppendLine($"WLAN Supported: {Convert.ToBoolean(deviceInfo.wlanSupported)}")

            txtDeviceInfo.Text = infoString.ToString()

        Else
            txtDeviceInfo.Text = $"장치 정보 가져오기 실패. (ID: {selectedDeviceId})" & vbCrLf
            txtDeviceInfo.AppendText("오류 코드: 0x" & getInfoResult.ToString("X"))
        End If


    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If sdkContext <> IntPtr.Zero Then
            BS2_ReleaseContext(sdkContext)
            sdkContext = IntPtr.Zero
        End If
    End Sub

End Class