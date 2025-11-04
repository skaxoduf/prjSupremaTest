Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Globalization
Imports System.IO
Imports System.Reflection

Namespace Suprema
    Friend Class Util
        Public Shared Function BytesToStruct(Of T)(ByRef source As Byte()) As T
            Dim structType = GetType(T)
            Dim structSize = Marshal.SizeOf(structType)
            Dim buffer = Marshal.AllocHGlobal(structSize)
            Marshal.Copy(source, 0, buffer, structSize)
            Dim instance As T = Marshal.PtrToStructure(buffer, structType)
            Marshal.FreeHGlobal(buffer)

            Return instance
        End Function

        Public Shared Function BytesToStruct(Of T)(ByRef source As Byte(), startIndex As Integer) As T
            Dim structType = GetType(T)
            Dim structSize = Marshal.SizeOf(structType)
            Dim buffer = Marshal.AllocHGlobal(structSize)
            Marshal.Copy(source, startIndex, buffer, structSize)
            Dim instance As T = Marshal.PtrToStructure(buffer, structType)
            Marshal.FreeHGlobal(buffer)

            Return instance
        End Function

        Public Sub CopyMemory(Of T)(ByRef source As IntPtr, ByRef target As IntPtr)
            Dim structType = GetType(T)
            Dim structSize = Marshal.SizeOf(structType)
            CopyMemory(target, source, structSize)
        End Sub

        Public Shared Function StructToBytes(Of T)(ByRef source As T) As Byte()
            Dim structType = GetType(T)
            Dim structSize = Marshal.SizeOf(structType)
            Dim buffer = Marshal.AllocHGlobal(structSize)
            Marshal.StructureToPtr(source, buffer, True)
            Dim output = New Byte(structSize - 1) {}
            Marshal.Copy(buffer, output, 0, structSize)
            Marshal.FreeHGlobal(buffer)

            Return output
        End Function
        Public Shared Sub TranslatePrimitive(Of TSource, TOutput)(ByRef src As TSource, ByRef output As TOutput)
            Dim typeSrc = GetType(TSource)
            Dim typeOut = GetType(TOutput)

            ' 1. ByRef로 넘어온 output을 Object로 박싱합니다.
            '    (리플렉션 SetValue는 객체 참조에 대해서만 동작하기 때문)
            Dim boxedOutput As Object = output

            Dim srcInfos = typeSrc.GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic)
            Dim outInfos = typeOut.GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic)

            For Each srcInfo In srcInfos
                Dim matchs = outInfos.Where(Function(x) Equals(x.Name, srcInfo.Name) AndAlso x.FieldType Is srcInfo.FieldType)

                For Each outInfo In matchs
                    ' 2. GetValue를 사용해 src에서 값을 가져옵니다.
                    Dim valueToSet = srcInfo.GetValue(src)

                    ' 3. SetValue를 사용해 '박싱된' output 객체에 값을 설정합니다.
                    '    (trOut 대신 boxedOutput 사용)
                    outInfo.SetValue(boxedOutput, valueToSet)

                    Exit For
                Next
            Next

            ' 4. 모든 필드 값이 변경된 박싱된 객체를
            '    원래 ByRef 변수인 output에 다시 언박싱하여 할당합니다.
            output = DirectCast(boxedOutput, TOutput)

        End Sub


        Public Delegate Function FUNC_BS2_GetAll(context As IntPtr, deviceId As UInteger, <Out> ByRef obj As IntPtr, <Out> ByRef numItem As UInteger) As Integer
        Public Shared Function CSP_BS2_GetAll(Of CSP_T, CXX_T)(context As IntPtr, deviceId As UInteger, <Out> ByRef ItemsObj As CSP_T(), <Out> ByRef numItem As UInteger, func As FUNC_BS2_GetAll) As BS2ErrorCode
            Dim _itemsObj = IntPtr.Zero
            Dim _itemNum As UInteger = 0
            Dim result As BS2ErrorCode = CType(func(context, deviceId, _itemsObj, _itemNum), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                numItem = 0
                ItemsObj = AllocateStructureArray(Of CSP_T)(0)
                Return result
            End If

            Dim _items = AllocateStructureArray(Of CSP_T)(_itemNum)

            Dim transItem As Translator(Of CXX_T, CSP_T) = New Translator(Of CXX_T, CSP_T)()
            Dim ItemSize = Marshal.SizeOf(GetType(CXX_T))
            Dim curItemObj = _itemsObj
            Dim idx = 0

            While idx < _itemNum
                Dim item As CXX_T = Marshal.PtrToStructure(curItemObj, GetType(CXX_T))
                transItem.Translate(item, _items(idx))
                curItemObj += ItemSize
                Threading.Interlocked.Increment(idx)
            End While

            API.BS2_ReleaseObject(_itemsObj)

            numItem = _itemNum
            ItemsObj = _items
            Return result
        End Function

        Public Delegate Function FUNC_BS2_GetItems(context As IntPtr, deviceId As UInteger, Ids As IntPtr, IdCount As UInteger, <Out> ByRef obj As IntPtr, <Out> ByRef numItem As UInteger) As Integer
        Public Shared Function CSP_BS2_GetItems(Of CSP_ID_T, CSP_T, CXX_ID_T, CXX_T)(context As IntPtr, deviceId As UInteger, Ids As CSP_ID_T(), idCount As UInteger, <Out> ByRef ItemsObj As CSP_T(), <Out> ByRef numItem As UInteger, func As FUNC_BS2_GetItems) As BS2ErrorCode
            Dim transID As Translator(Of CSP_ID_T, CXX_ID_T) = New Translator(Of CSP_ID_T, CXX_ID_T)()
            Dim Id As CXX_ID_T = AllocateStructure(Of CXX_ID_T)()

            Dim IdSize = Marshal.SizeOf(GetType(CXX_ID_T))
            Dim _IDObj = Marshal.AllocHGlobal(IdSize * CInt(idCount))
            Dim _curIDObj = _IDObj
            Dim idx = 0

            While idx < idCount
                transID.Translate(Ids(idx), Id)
                Marshal.StructureToPtr(Id, _curIDObj, True)
                _curIDObj += IdSize
                Threading.Interlocked.Increment(idx)
            End While

            Dim _itemsObj = IntPtr.Zero
            Dim _itemNum As UInteger = 0
            Dim result As BS2ErrorCode = CType(func(context, deviceId, _IDObj, idCount, _itemsObj, _itemNum), BS2ErrorCode)
            Marshal.FreeHGlobal(_IDObj)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                numItem = 0
                ItemsObj = New CSP_T(-1) {}
                Return result
            End If

            Dim _items = AllocateStructureArray(Of CSP_T)(_itemNum)

            Dim transItem As Translator(Of CXX_T, CSP_T) = New Translator(Of CXX_T, CSP_T)()
            Dim ItemSize = Marshal.SizeOf(GetType(CXX_T))
            Dim curItemObj = _itemsObj
            idx = 0

            While idx < _itemNum
                Dim item As CXX_T = Marshal.PtrToStructure(curItemObj, GetType(CXX_T))
                transItem.Translate(item, _items(idx))
                curItemObj += ItemSize
                Threading.Interlocked.Increment(idx)
            End While

            API.BS2_ReleaseObject(_itemsObj)

            numItem = _itemNum
            ItemsObj = _items
            Return result
        End Function

        Public Delegate Function FUNC_BS2_RemoveItems(context As IntPtr, deviceId As UInteger, Ids As IntPtr, IdCount As UInteger) As Integer
        Public Shared Function CSP_BS2_RemoveItems(Of CSP_ID_T, CXX_ID_T)(context As IntPtr, deviceId As UInteger, Ids As CSP_ID_T(), idCount As UInteger, func As FUNC_BS2_RemoveItems) As BS2ErrorCode
            Dim transID As Translator(Of CSP_ID_T, CXX_ID_T) = New Translator(Of CSP_ID_T, CXX_ID_T)()
            Dim Id As CXX_ID_T = AllocateStructure(Of CXX_ID_T)()

            Dim IdSize = Marshal.SizeOf(GetType(CXX_ID_T))
            Dim _IDObj = Marshal.AllocHGlobal(IdSize * CInt(idCount))
            Dim _curIDObj = _IDObj
            Dim idx = 0

            While idx < idCount
                transID.Translate(Ids(idx), Id)
                Marshal.StructureToPtr(Id, _curIDObj, True)
                _curIDObj += IdSize
                Threading.Interlocked.Increment(idx)
            End While

            Dim result As BS2ErrorCode = CType(func(context, deviceId, _IDObj, idCount), BS2ErrorCode)
            Marshal.FreeHGlobal(_IDObj)
            Return result
        End Function


        Public Delegate Function FUNC_BS2_SetItems(context As IntPtr, deviceId As UInteger, ItemsObj As IntPtr, ItemCount As UInteger) As Integer
        Public Shared Function CSP_BS2_SetItems(Of CSP_T, CXX_T)(context As IntPtr, deviceId As UInteger, ItemsObj As CSP_T(), ItemCount As UInteger, func As FUNC_BS2_SetItems) As BS2ErrorCode
            Dim transItem As Translator(Of CSP_T, CXX_T) = New Translator(Of CSP_T, CXX_T)()
            Dim Item As CXX_T = AllocateStructure(Of CXX_T)()

            Dim ItemSize = Marshal.SizeOf(GetType(CXX_T))
            Dim _ItemsObj = Marshal.AllocHGlobal(ItemSize * CInt(ItemCount))
            Dim _curItemObj = _ItemsObj
            Dim idx = 0

            While idx < ItemCount
                transItem.Translate(ItemsObj(idx), Item)
                Marshal.StructureToPtr(Item, _curItemObj, False)
                _curItemObj += ItemSize
                Threading.Interlocked.Increment(idx)
            End While

            Dim result As BS2ErrorCode = CType(func(context, deviceId, _ItemsObj, ItemCount), BS2ErrorCode)
            Marshal.FreeHGlobal(_ItemsObj)
            Return result
        End Function


        Public Shared Function AllocateStructure(Of T)() As T
            Dim structSize = Marshal.SizeOf(GetType(T))
            Dim empty = New Byte(structSize - 1) {}
            Array.Clear(empty, 0, empty.Length)
            Dim buffer = Marshal.AllocHGlobal(structSize)
            Marshal.Copy(empty, 0, buffer, structSize)
            Dim instance As T = Marshal.PtrToStructure(buffer, GetType(T))
            Marshal.FreeHGlobal(buffer)

            Return instance

        End Function

        Public Shared Function AllocateStructureArray(Of T)(count As Integer) As T()
            Dim result = New T(count - 1) {}
            Dim structSize = Marshal.SizeOf(GetType(T))
            Dim empty = New Byte(structSize * count - 1) {}
            Array.Clear(empty, 0, empty.Length)
            Dim buffer = Marshal.AllocHGlobal(structSize * count)
            Dim curBuffer = buffer
            Marshal.Copy(empty, 0, buffer, structSize * count)
            For idx = 0 To count - 1
                result(idx) = CType(Marshal.PtrToStructure(curBuffer, GetType(T)), T)
                curBuffer = CType(CLng(curBuffer) + structSize, IntPtr)
            Next

            Marshal.FreeHGlobal(buffer)
            Return result

        End Function

        Public Shared Function StringToByte(allocSize As Integer, source As String) As Byte()
            Dim result = New Byte(allocSize - 1) {}
            Array.Clear(result, 0, result.Length)
            Dim sourceByte = Encoding.UTF8.GetBytes(source)
            Dim copySize = Math.Min(allocSize, sourceByte.Length)
            Buffer.BlockCopy(sourceByte, 0, result, 0, copySize)
            Return result
        End Function

        Public Shared Function ConvertTo(Of T)(src As Byte()) As T
            If src.Length < Marshal.SizeOf(GetType(T)) Then
                Throw New ArgumentException("array size is less than object size", "src")
            End If

            Dim buffer = Marshal.AllocHGlobal(src.Length)
            Marshal.Copy(src, 0, buffer, src.Length)
            Dim item As T = Marshal.PtrToStructure(buffer, GetType(T))
            Marshal.FreeHGlobal(buffer)

            Return item
        End Function

        Public Shared Function ConvertTo(Of T)(ByRef instance As T) As Byte()
            Dim size = Marshal.SizeOf(GetType(T))
            Dim arr = New Byte(size - 1) {}

            Dim ptr = Marshal.AllocHGlobal(size)
            Marshal.StructureToPtr(instance, ptr, False)
            Marshal.Copy(ptr, arr, 0, size)
            Marshal.FreeHGlobal(ptr)

            Return arr
        End Function

        Public Shared Function GetInput(<Out> ByRef input As Integer) As Boolean
            Dim inputStr As String = Console.ReadLine()
            If inputStr.Length > 0 Then
                Return Integer.TryParse(inputStr, input)
            Else
                input = 0
                Return False
            End If
        End Function

        Public Shared Function GetInput() As Integer
            Do
                Dim inputStr As String = Console.ReadLine()
                If inputStr.Length > 0 Then
                    Return Convert.ToInt32(inputStr)
                End If
            Loop While True
        End Function

        Public Shared Function GetInputToNumeric(Of T)(defaultValue As T) As T
            Dim inputStr As String = Console.ReadLine()

            Try
                Dim convertedValue As T = Convert.ChangeType(inputStr, GetType(T))
                Return convertedValue
            Catch __unusedException1__ As Exception
                Return defaultValue
            End Try
        End Function

        Public Shared Function GetInputHexa() As UInteger
            Do
                Dim inputStr As String = Console.ReadLine()
                If inputStr.Length > 0 Then
                    Try
                        Return Convert.ToUInt32(inputStr, 16)
                    Catch __unusedFormatException1__ As FormatException
                        Console.WriteLine("Please enter the correct value in hexadecimal.")
                    Catch __unusedOverflowException2__ As OverflowException
                        Console.WriteLine("The input value is out of range.")
                    End Try
                End If
            Loop While True
        End Function


        Public Shared Function GetInput(defaultValue As Char) As Char
            Dim inputStr As String = Console.ReadLine()
            If inputStr.Length > 0 Then
                Return Convert.ToChar(inputStr)
            End If

            Return defaultValue
        End Function

        Public Shared Function GetInput(defaultValue As Byte) As Byte
            Dim inputStr As String = Console.ReadLine()
            If inputStr.Length > 0 Then
                Return Convert.ToByte(inputStr)
            End If

            Return defaultValue
        End Function

        Public Shared Function GetInput(defaultValue As UShort) As UShort
            Dim inputStr As String = Console.ReadLine()
            If inputStr.Length > 0 Then
                Return Convert.ToUInt16(inputStr)
            End If

            Return defaultValue
        End Function

        Public Shared Function GetInput(defaultValue As UInteger) As UInteger
            Dim inputStr As String = Console.ReadLine()
            If inputStr.Length > 0 Then
                Return Convert.ToUInt32(inputStr)
            End If

            Return defaultValue
        End Function

        Public Shared Function GetInput(defaultValue As Single) As Single
            Dim inputStr As String = Console.ReadLine()
            If inputStr.Length > 0 Then
                Return Single.Parse(inputStr)
            End If

            Return defaultValue
        End Function

        Public Shared Function ConvertHexByte2String(convertArr As Byte()) As String
            Dim converted = String.Empty
            converted = String.Concat(Array.ConvertAll(convertArr, Function(byt) byt.ToString("X2")))
            Return converted
        End Function

        Public Shared Function ConvertString2HexByte(convertStr As String) As Byte()
            Dim converted = New Byte(convertStr.Length / 2 - 1) {}
            For i = 0 To converted.Length - 1
                converted(i) = Convert.ToByte(convertStr.Substring(i * 2, 2), 16)
            Next
            Return converted
        End Function

        Public Shared Function GetTimestamp(formatString As String, defaultValue As UInteger, <Out> ByRef timestamp As UInteger) As Boolean
            Dim inputStr As String = Console.ReadLine()

            If defaultValue = 0 Then
                defaultValue = Convert.ToUInt32(ConvertToUnixTimestamp(Date.Now))
            End If

            timestamp = defaultValue
            If inputStr.Length > 0 Then
                Dim dateTime As Date
                If Not Date.TryParseExact(inputStr, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, dateTime) Then
                    Console.WriteLine("Invalid datetime : {0}", inputStr)
                    Return False
                Else
                    'timestamp = Convert.ToUInt32(Util.ConvertToUnixTimestamp(dateTime));
                    timestamp = Convert.ToUInt32(GetPosixTime(dateTime))
                End If
            End If

            Return True
        End Function

        Public Shared Function GetTime(formatString As String, defaultValue As UInteger, <Out> ByRef time As UInteger) As Boolean
            Dim inputStr As String = Console.ReadLine()
            time = 0
            If inputStr.Length > 0 Then
                Dim dateTime As Date
                If Not Date.TryParseExact(inputStr, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, dateTime) Then
                    Console.WriteLine("Invalid datetime : {0}", inputStr)
                    Return False
                Else
                    time = CUInt(dateTime.Hour * 60 * 60 + dateTime.Minute * 60 + dateTime.Second)
                End If
            End If

            Return True
        End Function

        Public Shared Function IsYes() As Boolean
            Dim inputStr As String = Console.ReadLine()
            If inputStr.Length = 0 OrElse String.Compare(inputStr, "Y", True) = 0 Then
                Return True
            End If

            Return False
        End Function

        Public Shared Function IsNo() As Boolean
            Dim inputStr As String = Console.ReadLine()
            If inputStr.Length = 0 OrElse String.Compare(inputStr, "N", True) = 0 Then
                Return True
            End If

            Return False
        End Function

        Public Shared Function ConvertFromUnixTimestamp(timestamp As Double) As Date
            Dim origin As Date = New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
            Return origin.AddSeconds(timestamp)
        End Function

        Public Shared Function ConvertToUnixTimestamp([date] As Date) As Double
            Dim origin As Date = New DateTime(1970, 1, 1, 0, 0, 0, 0)
            Dim diff As TimeSpan = [date].ToUniversalTime() + If(TimeZoneInfo.Local.IsDaylightSavingTime([date]), TimeZoneInfo.Local.BaseUtcOffset, TimeSpan.Zero) - origin.ToUniversalTime()
            Return Math.Floor(diff.TotalSeconds)
        End Function

        Public Shared Function GetPosixTime([date] As Date) As Double
            Dim baseDate As Date = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ' FISF-934 Set 0 problem when converted posix time
            'DateTime currDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            Dim currDate As Date = New DateTime([date].Year, [date].Month, [date].Day, [date].Hour, [date].Minute, [date].Second, DateTimeKind.Utc)
            ' FISF-934 Set 0 problem when converted posix time
            Return currDate.Subtract(baseDate).TotalSeconds
        End Function

        Public Shared Function LoadBinary(filePath As String, <Out> ByRef binaryData As IntPtr, <Out> ByRef binaryDataLen As UInteger) As Boolean
            Dim handled = False
            Dim fs As FileStream = Nothing

            binaryData = IntPtr.Zero
            binaryDataLen = 0
            Try
                fs = New FileStream(filePath, FileMode.Open, FileAccess.Read)
                Dim fileSize As Integer = fs.Length
                Dim totalReadCount = 0
                Dim readBuffer = New Byte(fileSize - 1) {}

                While totalReadCount < fileSize
                    Dim readCount = fs.Read(readBuffer, totalReadCount, fileSize - totalReadCount)
                    If readCount > 0 Then
                        totalReadCount += readCount
                    Else
                        Console.WriteLine("I/O error occurred while reading firmware file.")
                        Exit While
                    End If
                End While

                If totalReadCount = fileSize Then
                    binaryData = Marshal.AllocHGlobal(fileSize)
                    Marshal.Copy(readBuffer, 0, binaryData, fileSize)
                    binaryDataLen = CUInt(fileSize)
                    handled = True
                End If
            Catch e As Exception
                Console.WriteLine("Error reading from {0}. Message = {1}", filePath, e.Message)
            Finally
                If fs IsNot Nothing Then
                    fs.Close()
                End If
            End Try

            Return handled
        End Function

        Public Shared Function getActionMsg(action As BS2Action) As String
            Dim actionType As BS2ActionTypeEnum = CType(action.type, BS2ActionTypeEnum)

            Select Case actionType
                Case BS2ActionTypeEnum.NONE
                    Return "Not specified"
                Case BS2ActionTypeEnum.RELAY
                    Dim relay As BS2RelayAction = Util.ConvertTo(Of BS2RelayAction)(action.actionUnion)
                    Return String.Format("RelayAction relayIndex[{0}] signalID[{1}] count[{2}] onDuration[{3}ms] offDuration[{4}ms] delay[{5}ms]", relay.relayIndex, relay.signal.signalID, relay.signal.count, relay.signal.onDuration, relay.signal.offDuration, relay.signal.delay)
                Case BS2ActionTypeEnum.TTL
                    Dim outputPort As BS2OutputPortAction = Util.ConvertTo(Of BS2OutputPortAction)(action.actionUnion)
                    Return String.Format("OutputPortAction relayIndex[{0}] signalID[{1}] count[{2}] onDuration[{3}ms] offDuration[{4}ms] delay[{5}ms]", outputPort.portIndex, outputPort.signal.signalID, outputPort.signal.count, outputPort.signal.onDuration, outputPort.signal.offDuration, outputPort.signal.delay)
                Case BS2ActionTypeEnum.DISPLAY
                    Dim display As BS2DisplayAction = Util.ConvertTo(Of BS2DisplayAction)(action.actionUnion)
                    Return String.Format("DisplayAction displayID[{0}] resourceID[{1}] delay[{2}ms]", display.displayID, display.resourceID, display.duration)
                Case BS2ActionTypeEnum.SOUND
                    Dim sound As BS2SoundAction = Util.ConvertTo(Of BS2SoundAction)(action.actionUnion)
                    Return String.Format("SoundAction soundIndex[{0}] count[{1}]", sound.soundIndex, sound.count)
                Case BS2ActionTypeEnum.LED
                    Dim led As BS2LedAction = Util.ConvertTo(Of BS2LedAction)(action.actionUnion)
                    Dim ledSignalStr = ""
                    Dim idx = 0

                    While idx < BS2Environment.BS2_LED_SIGNAL_NUM
                        ledSignalStr += String.Format("[color[{0}] duration[{1}ms] delay[{2}ms]]", CType(led.signal(CInt(idx)).color, BS2LedColorEnum), led.signal(CInt(idx)).duration, led.signal(CInt(idx)).delay)

                        If idx + 1 < BS2Environment.BS2_LED_SIGNAL_NUM Then
                            ledSignalStr += ", "
                        End If

                        Threading.Interlocked.Increment(idx)
                    End While

                    Return String.Format("LedAction count[{0}] {1}", led.count, ledSignalStr)
                Case BS2ActionTypeEnum.BUZZER
                    Dim buzzer As BS2BuzzerAction = Util.ConvertTo(Of BS2BuzzerAction)(action.actionUnion)
                    Dim buzzerSignalStr = ""
                    Dim idx = 0

                    While idx < BS2Environment.BS2_BUZZER_SIGNAL_NUM
                        buzzerSignalStr += String.Format("[tone[{0}] fadeout[{1}] duration[{2}ms] delay[{3}ms]]", CType(buzzer.signal(CInt(idx)).tone, BS2BuzzerToneEnum), Convert.ToBoolean(buzzer.signal(CInt(idx)).fadeout), buzzer.signal(CInt(idx)).duration, buzzer.signal(CInt(idx)).delay)

                        If idx + 1 < BS2Environment.BS2_BUZZER_SIGNAL_NUM Then
                            buzzerSignalStr += ", "
                        End If

                        Threading.Interlocked.Increment(idx)
                    End While

                    Return String.Format("BuzzerAction count[{0}] {1}", buzzer.count, buzzerSignalStr)
                Case BS2ActionTypeEnum.LIFT
                    Dim lift As BS2LiftAction = Util.ConvertTo(Of BS2LiftAction)(action.actionUnion)
                    Return String.Format("LiftAction deviceID[{0}] type[{1}]", lift.liftID, CType(lift.type, BS2LiftActionTypeEnum))
                Case Else
                    Return "Not implemented yet."
            End Select
        End Function

        Public Shared Function GetLogMsg(eventLog As BS2Event) As String
#If False
            return "eventlog : ";
#Else
            Select Case CType(eventLog.code, BS2EventCodeEnum) And BS2EventCodeEnum.MASK
                Case BS2EventCodeEnum.DOOR_LOCKED, BS2EventCodeEnum.DOOR_UNLOCKED, BS2EventCodeEnum.DOOR_CLOSED, BS2EventCodeEnum.DOOR_OPENED, BS2EventCodeEnum.DOOR_FORCED_OPEN, BS2EventCodeEnum.DOOR_FORCED_OPEN_ALARM, BS2EventCodeEnum.DOOR_FORCED_OPEN_ALARM_CLEAR, BS2EventCodeEnum.DOOR_HELD_OPEN, BS2EventCodeEnum.DOOR_HELD_OPEN_ALARM, BS2EventCodeEnum.DOOR_HELD_OPEN_ALARM_CLEAR, BS2EventCodeEnum.DOOR_APB_ALARM, BS2EventCodeEnum.DOOR_APB_ALARM_CLEAR
                    Return Util.GetDoorIdMsg(eventLog)
                Case BS2EventCodeEnum.ZONE_APB_ALARM, BS2EventCodeEnum.ZONE_APB_ALARM_CLEAR, BS2EventCodeEnum.ZONE_TIMED_APB_ALARM, BS2EventCodeEnum.ZONE_TIMED_APB_ALARM_CLEAR, BS2EventCodeEnum.ZONE_FIRE_ALARM, BS2EventCodeEnum.ZONE_FIRE_ALARM_CLEAR, BS2EventCodeEnum.ZONE_SCHEDULED_LOCK_VIOLATION, BS2EventCodeEnum.ZONE_SCHEDULED_LOCK_START, BS2EventCodeEnum.ZONE_SCHEDULED_LOCK_END, BS2EventCodeEnum.ZONE_SCHEDULED_UNLOCK_START, BS2EventCodeEnum.ZONE_SCHEDULED_UNLOCK_END, BS2EventCodeEnum.ZONE_SCHEDULED_LOCK_ALARM, BS2EventCodeEnum.ZONE_SCHEDULED_LOCK_ALARM_CLEAR
                    Return Util.GetZoneIdMsg(eventLog)
                Case BS2EventCodeEnum.SUPERVISED_INPUT_OPEN, BS2EventCodeEnum.SUPERVISED_INPUT_SHORT, BS2EventCodeEnum.DEVICE_INPUT_DETECTED
                    Return Util.GetIOInfoMsg(eventLog)
                Case BS2EventCodeEnum.USER_ENROLL_SUCCESS, BS2EventCodeEnum.USER_ENROLL_FAIL, BS2EventCodeEnum.USER_UPDATE_SUCCESS, BS2EventCodeEnum.USER_UPDATE_FAIL, BS2EventCodeEnum.USER_DELETE_SUCCESS, BS2EventCodeEnum.USER_DELETE_FAIL, BS2EventCodeEnum.USER_ISSUE_AOC_SUCCESS, BS2EventCodeEnum.USER_DUPLICATE_CREDENTIAL, BS2EventCodeEnum.USER_UPDATE_PARTIAL_SUCCESS, BS2EventCodeEnum.USER_UPDATE_PARTIAL_FAIL
                    Return Util.GetUserIdMsg(eventLog)
                Case BS2EventCodeEnum.VERIFY_SUCCESS, BS2EventCodeEnum.VERIFY_FAIL, BS2EventCodeEnum.VERIFY_DURESS, BS2EventCodeEnum.IDENTIFY_SUCCESS, BS2EventCodeEnum.IDENTIFY_FAIL, BS2EventCodeEnum.IDENTIFY_DURESS
                    Return Util.GetUserIdAndTnaKeyMsg(eventLog)
                Case BS2EventCodeEnum.RELAY_ACTION_ON, BS2EventCodeEnum.RELAY_ACTION_OFF, BS2EventCodeEnum.RELAY_ACTION_KEEP
                    Return Util.GetRelayActionMsg(eventLog)
                Case Else
                    Return Util.GetGeneralMsg(eventLog)
            End Select
#End If
        End Function

        Private Shared Function GetDoorIdMsg(eventLog As BS2Event) As String
            Dim eventTime = ConvertFromUnixTimestamp(eventLog.dateTime)
            Dim eventDetail As BS2EventDetail = ConvertTo(Of BS2EventDetail)(eventLog.userID)

            Return String.Format("Log => device[{0, 10}] : timestamp[{1}] event id[{2, 10}] event code[{3}] doorID[{4}] image[{5}]", eventLog.deviceID, eventTime.ToString("yyyy-MM-dd HH:mm:ss"), eventLog.id, CType(eventLog.code, BS2EventCodeEnum), eventDetail.doorID, Convert.ToBoolean(eventLog.image And CByte(BS2EventImageBitPos.BS2_IMAGEFIELD_POS_IMAGE)))
        End Function

        Private Shared Function GetZoneIdMsg(eventLog As BS2Event) As String
            Dim eventTime = ConvertFromUnixTimestamp(eventLog.dateTime)
            Dim eventDetail As BS2EventDetail = ConvertTo(Of BS2EventDetail)(eventLog.userID)

            Return String.Format("Log => device[{0, 10}] : timestamp[{1}] event id[{2, 10}] event code[{3}] zoneID[{4}] image[{5}]", eventLog.deviceID, eventTime.ToString("yyyy-MM-dd HH:mm:ss"), eventLog.id, CType(eventLog.code, BS2EventCodeEnum), eventDetail.zoneID, Convert.ToBoolean(eventLog.image And CByte(BS2EventImageBitPos.BS2_IMAGEFIELD_POS_IMAGE)))
        End Function

        Private Shared Function GetIOInfoMsg(eventLog As BS2Event) As String
            Dim eventTime = ConvertFromUnixTimestamp(eventLog.dateTime)
            Dim eventDetail As BS2EventDetail = ConvertTo(Of BS2EventDetail)(eventLog.userID)

            Return String.Format("Log => device[{0, 10}] : timestamp[{1}] event id[{2, 10}] event code[{3}] device[{4, 10}] port[{5}] value[{6}] image[{7}]", eventLog.deviceID, eventTime.ToString("yyyy-MM-dd HH:mm:ss"), eventLog.id, CType(eventLog.code, BS2EventCodeEnum), eventDetail.ioDeviceID, eventDetail.port, eventDetail.value, Convert.ToBoolean(eventLog.image And CByte(BS2EventImageBitPos.BS2_IMAGEFIELD_POS_IMAGE)))
        End Function

        Private Shared Function GetUserIdMsg(eventLog As BS2Event) As String
            Dim eventTime = ConvertFromUnixTimestamp(eventLog.dateTime)
            Dim userID = Encoding.ASCII.GetString(eventLog.userID).TrimEnd(ChrW(0))
            If userID.Length = 0 Then
                userID = "unknown"
            End If

            Return String.Format("Log => device[{0, 10}] : timestamp[{1}] event id[{2, 10}] event code[{3}] userID[{4}] image[{5}] where[{6}]", eventLog.deviceID, eventTime.ToString("yyyy-MM-dd HH:mm:ss"), eventLog.id, CType(eventLog.code, BS2EventCodeEnum), userID, Convert.ToBoolean(eventLog.image And CByte(BS2EventImageBitPos.BS2_IMAGEFIELD_POS_IMAGE)), If(Convert.ToBoolean(eventLog.param), "Device", "Server"))
        End Function

        Private Shared Function GetUserIdAndTnaKeyMsg(eventLog As BS2Event) As String
            Dim eventTime = ConvertFromUnixTimestamp(eventLog.dateTime)
            Dim userID = Encoding.ASCII.GetString(eventLog.userID).TrimEnd(ChrW(0))

            If userID.Length = 0 Then
                userID = "unknown"
            End If

            Dim subMsg = ""
            If CType(eventLog.code, BS2EventCodeEnum) <> BS2EventCodeEnum.VERIFY_FAIL_CARD Then
                Dim tnaKeyEnum As BS2TNAKeyEnum = CType(eventLog.param, BS2TNAKeyEnum)
                If tnaKeyEnum <> BS2TNAKeyEnum.UNSPECIFIED Then
                    subMsg = String.Format("userID[{0}] T&A[{1}]", userID, tnaKeyEnum.ToString())
                Else
                    subMsg = String.Format("userID[{0}]", userID)
                End If
            Else
                subMsg = String.Format("cardID[{0}]", userID)
            End If

            Return String.Format("Log => device[{0, 10}] : timestamp[{1}] event id[{2, 10}] event code[{3}] {4} image[{5}]", eventLog.deviceID, eventTime.ToString("yyyy-MM-dd HH:mm:ss"), eventLog.id, CType(eventLog.code, BS2EventCodeEnum), subMsg, Convert.ToBoolean(eventLog.image And CByte(BS2EventImageBitPos.BS2_IMAGEFIELD_POS_IMAGE)))
        End Function

        Private Shared Function GetGeneralMsg(eventLog As BS2Event) As String
            Dim eventTime = ConvertFromUnixTimestamp(eventLog.dateTime)
            Return String.Format("Log => device[{0, 10}] : timestamp[{1}] event id[{2, 10}] event code[{3}] image[{4}]", eventLog.deviceID, eventTime.ToString("yyyy-MM-dd HH:mm:ss"), eventLog.id, CType(eventLog.code, BS2EventCodeEnum), Convert.ToBoolean(eventLog.image And CByte(BS2EventImageBitPos.BS2_IMAGEFIELD_POS_IMAGE)))
        End Function

        Private Shared Function GetRelayActionMsg(eventLog As BS2Event) As String
            Dim eventTime = ConvertFromUnixTimestamp(eventLog.dateTime)
            Dim eventDetail As BS2EventDetail = ConvertTo(Of BS2EventDetail)(eventLog.userID)

            Return String.Format("Log => device[{0, 10}] : timestamp[{1}] event id[{2, 10}] event code[{3}] inputPort[{4}] relayPort[{5}]", eventLog.deviceID, eventTime.ToString("yyyy-MM-dd HH:mm:ss"), eventLog.id, CType(eventLog.code, BS2EventCodeEnum), eventDetail.relayActionInputPort, eventDetail.relayActionRelayPort)
        End Function

        Public Shared Sub HighlightLine(fullStr As String, highlightStr As String, Optional highlightColor As ConsoleColor = ConsoleColor.Green)
            Dim first = fullStr.IndexOf(highlightStr)
            Dim len = highlightStr.Length
            Dim second = first + len
            Dim oldColor = Console.ForegroundColor

            Console.Write(fullStr.Substring(0, first))
            Console.ForegroundColor = highlightColor
            Console.Write(fullStr.Substring(first, len))
            Console.ForegroundColor = oldColor
            Console.WriteLine(fullStr.Substring(second))
        End Sub

        Public Shared Sub HighlightLineMulti(fullStr As String, ParamArray highlightStrObj As Object())
            Dim before = 0
            Dim oldColor = Console.ForegroundColor
            For Each s In highlightStrObj
                Dim tempStr As String = s.ToString()
                Dim first = fullStr.IndexOf(tempStr)
                Dim len As Integer = tempStr.ToString().Length

                Console.ForegroundColor = oldColor
                Console.Write(fullStr.Substring(before, first - before))
                Console.ForegroundColor = ConsoleColor.Green
                Console.Write(fullStr.Substring(first, len))
                before = first + len
            Next

            Console.ForegroundColor = oldColor
            Console.WriteLine(fullStr.Substring(before))
        End Sub

        Public Shared Function GetInputMasterOrSlaveID(masterID As UInteger) As UInteger
            Dim id As UInteger = 0

            Console.WriteLine(">>>> Do you want to process it with master ID? [Y/n]")
            Console.Write(">>>> ")
            If IsYes() Then
                id = masterID
            Else
                id = GetInputSlaveID()
            End If

            Return id
        End Function

        Public Shared Function GetInputSlaveID() As UInteger
            Dim id As UInteger = 0

            Console.WriteLine(">>>> Select the slave ID:")
            Console.Write(">>>> ")
            id = CUInt(GetInput())

            Return id
        End Function


        Public Shared Function GetHostIPAddresses() As List(Of String)
            Dim ipAddrs As List(Of String) = New List(Of String)()
            Dim allInterfaces As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()

            For Each netInterface In allInterfaces
                If netInterface.NetworkInterfaceType <> NetworkInterfaceType.Loopback AndAlso netInterface.NetworkInterfaceType <> NetworkInterfaceType.Tunnel AndAlso netInterface.OperationalStatus = OperationalStatus.Up Then
                    Dim ipProp As IPInterfaceProperties = netInterface.GetIPProperties()
                    For Each ipInfo In ipProp.UnicastAddresses
                        If ipInfo.Address.AddressFamily = Sockets.AddressFamily.InterNetwork Then ipAddrs.Add(ipInfo.Address.ToString())
                    Next
                End If
            Next

            Return ipAddrs
        End Function

        Public Shared Function GetStringUTF8(byteArray As Byte()) As String
            Dim str = Encoding.UTF8.GetString(byteArray)
            Dim nullIdx = str.IndexOf(ChrW(0))
            If nullIdx = -1 Then
                str += ChrW(0)
                nullIdx = str.Length - 1
            End If
            Return str.Substring(0, nullIdx)
        End Function

        <DllImport("kernel32.dll", EntryPoint:="CopyMemory", SetLastError:=False)>
        Public Shared Sub CopyMemory(dest As IntPtr, src As IntPtr, count As UInteger)
        End Sub
    End Class
End Namespace
