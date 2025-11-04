Imports System
Imports System.Collections.Generic
Imports System.Threading

Namespace Suprema
    Public MustInherit Class FunctionModule
        Protected deviceInfo As BS2SimpleDeviceInfo
        Protected deviceInfoEx As BS2SimpleDeviceInfoEx
        Protected MustOverride Function getFunctionList(sdkContext As IntPtr, deviceID As UInteger, isMasterDevice As Boolean) As List(Of KeyValuePair(Of String, Action(Of IntPtr, UInteger, Boolean)))

        Public Sub execute(sdkContext As IntPtr, deviceID As UInteger, isMasterDevice As Boolean, Optional noConnection As Boolean = False)
#If OLD_CODE Then
            If noConnection = False Then
                Dim result As BS2ErrorCode = CType(API.BS2_GetDeviceInfo(sdkContext, deviceID, deviceInfo), BS2ErrorCode)

                ' --- 수정된 부분 1: IsNot -> <> ---
                ' 값 형식(Enum) 비교이므로 IsNot 대신 <> 사용
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("Can't get device information(errorCode : {0}).", result)
                    Return
                End If
            End If
#Else
            If noConnection = False Then
                Dim result As BS2ErrorCode = CType(API.BS2_GetDeviceInfoEx(sdkContext, deviceID, deviceInfo, deviceInfoEx), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("Can't get device information(errorCode : {0}).", result)
                    Return
                End If
            End If
#End If

            Dim functionList = getFunctionList(sdkContext, deviceID, isMasterDevice)

            If functionList.Count > 0 Then
                Dim selection As Integer
                Dim running = True

                While running
                    Console.WriteLine("+-----------------------------------------------------------+")
                    Dim idx = 1
                    While idx <= functionList.Count
                        Console.WriteLine("|{0,3}. {1,-54}|", idx, functionList(idx - 1).Key)
                        idx += 1
                    End While
                    Console.WriteLine("|{0,3}. Exit                                                  |", idx)
                    Console.WriteLine("+-----------------------------------------------------------+")

                    Console.WriteLine("What would you like to do?")
                    Console.Write(">>>> ")

                    If Util.GetInput(selection) Then
                        If selection > 0 AndAlso selection <= functionList.Count Then
                            If Nothing IsNot functionList(selection - 1).Value Then
                                functionList(CInt(selection - 1)).Value.Invoke(sdkContext, deviceID, isMasterDevice)
                            End If
                        ElseIf selection = functionList.Count + 1 Then
                            running = False
                        Else
                            Console.WriteLine("Invalid parameter : {0}", selection)
                        End If
                    End If
                End While
            End If
        End Sub
    End Class
End Namespace