Imports System.Net
Imports System.Runtime.InteropServices

Public Class Form1

    ' --- SDK 상수 ---
    Private Const BS_SDK_SUCCESS As Integer = 0

    ' --- SDK 컨텍스트 ---
    Private sdkContext As IntPtr = IntPtr.Zero

#Region "SDK DllImport 선언"
    ' 컨텍스트 할당 함수
    <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function BS2_AllocateContext() As IntPtr
    End Function

    ' 초기화 함수
    <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function BS2_Initialize(ByVal context As IntPtr) As Integer
    End Function

    ' 컨텍스트 해제 함수
    <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function BS2_ReleaseContext(ByVal context As IntPtr) As Integer
    End Function

    ' --- 1. IP로 직접 연결 ---
    <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Shared Function BS2_ConnectDeviceViaIP(
        ByVal context As IntPtr,
        <MarshalAs(UnmanagedType.LPStr)> ByVal deviceAddress As String,
        ByVal devicePort As UShort,
        ByRef deviceId As UInteger
    ) As Integer
    End Function

    ' --- 2. 장치 검색 ---
    <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function BS2_SearchDevices(ByVal context As IntPtr) As Integer
    End Function

    ' --- 3. 검색된 장치 목록 가져오기 ---
    <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function BS2_GetDevices(
        ByVal context As IntPtr,
        ByRef deviceListObj As IntPtr,
        ByRef numDevice As UInteger
    ) As Integer
    End Function

    ' --- 4. SDK 할당 객체 해제 ---
    <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Sub BS2_ReleaseObject(ByVal obj As IntPtr)
    End Sub

    ' --- [신규] 5. ID로 장치 연결 ---
    ' C++: int BS2_ConnectDevice(void* context, uint32_t deviceId);
    <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function BS2_ConnectDevice(
        ByVal context As IntPtr,
        ByVal deviceId As UInteger
    ) As Integer
    End Function
#End Region


    Private Sub btnDLLLoad_Click(sender As Object, e As EventArgs) Handles btnDLLLoad.Click

        ' 이미 초기화되었다면 중복 실행 방지
        If sdkContext <> IntPtr.Zero Then
            MessageBox.Show("SDK가 이미 초기화되었습니다.")
            Return
        End If

        Dim result As Integer
        sdkContext = BS2_AllocateContext()

        If sdkContext <> IntPtr.Zero Then
            result = BS2_Initialize(sdkContext)

            If result = BS_SDK_SUCCESS Then
                MessageBox.Show("DLL 초기화 성공!")
            Else
                ' 초기화 실패 시 컨텍스트 즉시 해제
                BS2_ReleaseContext(sdkContext)
                sdkContext = IntPtr.Zero
                MessageBox.Show("DLL 초기화 실패. 오류 코드: " & result)
            End If

            ' (중요) 성공했을 때는 BS2_ReleaseContext()를 호출하면 안 됩니다.
            ' 폼이 닫힐 때까지 컨텍스트를 유지해야 합니다.
        Else
            MessageBox.Show("컨텍스트 할당 실패 (메모리 부족?)")
        End If

    End Sub
    ' ip로 장비연결 
    Private Sub btnDeviceConn_Click(sender As Object, e As EventArgs) Handles btnDeviceConn.Click

        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
        End If

        Dim deviceAddress As String = "192.168.1.2"
        Dim devicePort As UShort = 51211
        Dim deviceId As UInteger = 0
        Dim result As Integer

        result = BS2_ConnectDeviceViaIP(sdkContext, deviceAddress, devicePort, deviceId)

        If result = BS_SDK_SUCCESS Then
            MessageBox.Show("장치 연결 성공! Device ID: " & deviceId)
        Else
            MessageBox.Show("장치 연결 실패. 오류 코드: 0x" & result.ToString("X"))
        End If

    End Sub
    ' 현재 연결된 장치가 있는지 검색후 장치를 연결 
    Private Sub btnDeviceSearchConn_Click(sender As Object, e As EventArgs) Handles btnDeviceSearchConn.Click
        If sdkContext = IntPtr.Zero Then
            MessageBox.Show("먼저 SDK 초기화를 실행하세요.")
            Return
        End If

        Dim result As Integer
        result = BS2_SearchDevices(sdkContext)

        If result = BS_SDK_SUCCESS Then
            Dim deviceListObj As IntPtr = IntPtr.Zero ' 장치 ID 배열을 가리킬 포인터
            Dim numDevice As UInteger = 0           ' 검색된 장치 수

            result = BS2_GetDevices(sdkContext, deviceListObj, numDevice)

            If result = BS_SDK_SUCCESS AndAlso numDevice > 0 Then
                ' C++ 샘플(deviceListObj[0])처럼 첫 번째 장치를 선택합니다.
                ' deviceListObj는 32비트(4바이트) ID 배열의 시작 주소입니다.
                ' Marshal.ReadInt32로 포인터가 가리키는 첫 4바이트(첫 번째 ID)를 읽습니다.
                Dim selectedDeviceId As UInteger = CType(Marshal.ReadInt32(deviceListObj), UInteger)

                ' (중요) SDK가 할당해 준 deviceListObj 메모리를 해제합니다.
                BS2_ReleaseObject(deviceListObj)

                ' 첫 번째 장치에 연결 시도
                result = BS2_ConnectDevice(sdkContext, selectedDeviceId)
                If result = BS_SDK_SUCCESS Then
                    MessageBox.Show("장치 연결 성공! (검색됨) ID: " & selectedDeviceId)
                Else
                    MessageBox.Show("장치 연결 실패. 오류 코드: 0x" & result.ToString("X"))
                End If

            ElseIf numDevice = 0 Then
                MessageBox.Show("검색된 장치가 없습니다.")
            Else
                MessageBox.Show("장치 목록 가져오기 실패. 오류 코드: 0x" & result.ToString("X"))
            End If
        Else
            MessageBox.Show("장치 검색 실패. 오류 코드: 0x" & result.ToString("X"))
        End If

    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' 폼이 닫힐 때 초기화되었던 컨텍스트를 정리합니다.
        If sdkContext <> IntPtr.Zero Then
            ' (참고: 실제 프로그램에서는 BS2_DisconnectDevice 등
            ' 연결 해제 코드를 먼저 호출해야 할 수도 있습니다.)
            BS2_ReleaseContext(sdkContext)
            sdkContext = IntPtr.Zero
        End If
    End Sub
End Class
