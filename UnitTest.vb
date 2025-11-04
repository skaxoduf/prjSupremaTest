'#define SDK_AUTO_CONNECTION
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Net
Imports System.Threading

Namespace Suprema
#If Not SDK_AUTO_CONNECTION
    Friend Class ReconnectionTask
        Implements IDisposable
        Private sdkContext As IntPtr
        Private running As Boolean
        Private thread As Thread
        Private ReadOnly locker As Object = New Object()
        Private eventWaitHandle As EventWaitHandle = New AutoResetEvent(False)
        Private deviceIDQueue As Queue(Of UInteger) = New Queue(Of UInteger)()
        Private Shared m_ip As String
        Private Shared m_port As UShort = CUShort(BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT)
        Public Sub New(sdkContext As IntPtr)
            Me.sdkContext = sdkContext
            thread = New Thread(AddressOf run)
        End Sub

        Public Shared Sub setIpPort(ip As String, port As UShort)
            m_ip = ip
            m_port = port
        End Sub

        Public Sub enqueue(deviceID As UInteger)
            Dim isAlreadyRequested = False

            SyncLock locker
                For Each targetDeviceID In deviceIDQueue
                    If targetDeviceID = deviceID Then
                        isAlreadyRequested = True
                        Exit For
                    End If
                Next

                If Not isAlreadyRequested Then
                    deviceIDQueue.Enqueue(deviceID)
                End If
            End SyncLock

            If Not isAlreadyRequested Then
                Console.WriteLine("enqueue Device[{0, 10}].", deviceID)
                eventWaitHandle.Set()
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            [stop]()
        End Sub

        Public Sub start()
            If Not running Then
                running = True
                thread.Start()
            End If
        End Sub

        Public Sub [stop]()
            If running Then
                running = False
                SyncLock locker
                    deviceIDQueue.Clear()
                End SyncLock
                eventWaitHandle.Set()
                thread.Join()
                eventWaitHandle.Close()
            End If
        End Sub

        Public Sub run()
            While running
                Dim deviceID As UInteger = 0

                SyncLock locker
                    If deviceIDQueue.Count > 0 Then
                        deviceID = deviceIDQueue.Dequeue()
                    End If
                End SyncLock

                If deviceID <> 0 Then
                    Console.WriteLine("trying to reconnect DeviceID[{0, 10}] IP:[{1}]port:[{2}].", deviceID, m_ip, m_port)

                    Dim result As BS2ErrorCode = New BS2ErrorCode()
                    Dim ptrIPAddr = Marshal.StringToHGlobalAnsi(m_ip)
                    While result <> BS2ErrorCode.BS_SDK_SUCCESS
                        result = CType(API.BS2_ConnectDeviceViaIP(sdkContext, ptrIPAddr, m_port, deviceID), BS2ErrorCode)

                        If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                            Console.WriteLine("Can't connect to device(errorCode : {0}).", result)
                        End If

                        Thread.Sleep(1000)
                    End While

                    ' 
                    ' BS2ErrorCode result = (BS2ErrorCode)API.BS2_ConnectDevice(sdkContext, deviceID);
                    ' if (result != BS2ErrorCode.BS_SDK_SUCCESS)
                    ' {
                    ' if (result != BS2ErrorCode.BS_SDK_ERROR_CANNOT_CONNECT_SOCKET)
                    ' {
                    ' Console.WriteLine("Can't connect to device(errorCode : {0}).", result);
                    ' return;
                    ' }
                    ' else
                    ' {
                    ' enqueue(deviceID);
                    ' }
                    ' }

                    Marshal.FreeHGlobal(ptrIPAddr)
                Else
                    eventWaitHandle.WaitOne()
                End If
            End While
        End Sub
    End Class
#End If

    Public MustInherit Class UnitTest
        Public Delegate Sub connectionPtr(sdkcontext As IntPtr, deviceID As UInteger)

        Private titleField As String
        Private cbOnDeviceFound As API.OnDeviceFound = Nothing
        Private cbOnDeviceAccepted As API.OnDeviceAccepted = Nothing
        Private cbOnDeviceConnected As API.OnDeviceConnected = Nothing
        Private cbOnDeviceDisconnected As API.OnDeviceDisconnected = Nothing

        Private cbOnDeviceConnectedImpl As connectionPtr = Nothing
        Private cbOnDeviceDisconnectedImpl As connectionPtr = Nothing

        Protected sdkContext As IntPtr = IntPtr.Zero
#If Not SDK_AUTO_CONNECTION
        Private reconnectionTask As ReconnectionTask = Nothing
#End If
        Private deviceIDForServerMode As UInteger = 0
        Private eventWaitHandle As EventWaitHandle = New AutoResetEvent(False)

        Private cbPreferMethod As API.PreferMethod = Nothing
        Private cbGetRootCaFilePath As API.GetRootCaFilePath = Nothing
        Private cbGetServerCaFilePath As API.GetServerCaFilePath = Nothing
        Private cbGetServerPrivateKeyFilePath As API.GetServerPrivateKeyFilePath = Nothing
        Private cbGetPassword As API.GetPassword = Nothing
        Private cbOnErrorOccured As API.OnErrorOccured = Nothing

        Private ssl_server_root_crt As String = "../../../../../resource/server/ssl_server_root.crt"
        Private ssl_server_crt As String = "../../../../../resource/server/ssl_server.crt"
        Private ssl_server_pem As String = "../../../../../resource/server/ssl_server.pem"
        Private ssl_server_passwd As String = "supremaserver"
        'private API.OnSendRootCA cbOnSendRootCA = null;
        Private cbDebugExPrint As API.CBDebugExPrint = Nothing

        Private ptr_server_root_crt As IntPtr = IntPtr.Zero
        Private ptr_server_crt As IntPtr = IntPtr.Zero
        Private ptr_server_pem As IntPtr = IntPtr.Zero
        Private ptr_server_passwd As IntPtr = IntPtr.Zero

        Protected MustOverride Sub runImpl(deviceID As UInteger)

        Protected Property Title As String
            Get
                Return titleField
            End Get
            Set(value As String)
                titleField = value
                Console.Title = value
            End Set
        End Property

        Public Sub setConnectionCb(cbConn As connectionPtr, cbDisconn As connectionPtr)
            cbOnDeviceConnectedImpl = cbConn
            cbOnDeviceDisconnectedImpl = cbDisconn
        End Sub

        Public Sub New()
            If Console.WindowWidth < 150 Then
                Console.WindowWidth = 150
            End If

            AddHandler AppDomain.CurrentDomain.ProcessExit, Sub(s, args)
                                                                If sdkContext <> IntPtr.Zero Then
                                                                    API.BS2_ReleaseContext(sdkContext)
                                                                    sdkContext = IntPtr.Zero
                                                                End If
                                                            End Sub
        End Sub

        Protected Overrides Sub Finalize()
            If ptr_server_root_crt <> IntPtr.Zero Then Marshal.FreeHGlobal(ptr_server_root_crt)
            If ptr_server_crt <> IntPtr.Zero Then Marshal.FreeHGlobal(ptr_server_crt)
            If ptr_server_pem <> IntPtr.Zero Then Marshal.FreeHGlobal(ptr_server_pem)
            If ptr_server_passwd <> IntPtr.Zero Then Marshal.FreeHGlobal(ptr_server_passwd)
        End Sub

        Private Sub printStructureSize(Of T)()
            Console.WriteLine("{0} size : {1}", GetType(T), Marshal.SizeOf(GetType(T)))
        End Sub

        Public Sub run()
            Dim result As BS2ErrorCode = BS2ErrorCode.BS_SDK_SUCCESS

            'cbDebugExPrint = null;
            'Console.WriteLine("Do you want print debug message? [y/n]");
            Console.WriteLine("Do you want output debug message to file? [Y/n]")
            Console.Write(">>>> ")
            If Util.IsYes() Then
                'cbDebugExPrint = new API.CBDebugExPrint(DebugExPrint);
                'result = (BS2ErrorCode)API.BS2_SetDebugExCallback(cbDebugExPrint, Constants.DEBUG_LOG_OPERATION_ALL, Constants.DEBUG_MODULE_ALL);
                'if (result != BS2ErrorCode.BS_SDK_SUCCESS)
                '{
                '    Console.WriteLine("Got error({0}).", result);
                '    return;
                '}

                Const CURRENT_DIR = "."
                Const MAX_SIZE_LOG_FILE = 100  ' 100MB
                Dim ptrDir = Marshal.StringToHGlobalAnsi(CURRENT_DIR)
                result = CType(API.BS2_SetDebugFileLogEx(Constants.DEBUG_LOG_ALL, Constants.DEBUG_MODULE_ALL, ptrDir, MAX_SIZE_LOG_FILE), BS2ErrorCode)
                Marshal.FreeHGlobal(ptrDir)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("Got error({0}).", result)
                    API.BS2_ReleaseContext(sdkContext)
                    sdkContext = IntPtr.Zero
                    Return
                End If

                'Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
                'Trace.AutoFlush = true;
                'Trace.Indent();
            End If

            Dim deviceID As UInteger = 0
            Dim versionPtr As IntPtr = API.BS2_Version()
            'bool bSsl = false;

            If titleField.Length > 0 Then
                Console.Title = titleField
            End If

            Console.WriteLine("SDK version : " & Marshal.PtrToStringAnsi(versionPtr))

            sdkContext = API.BS2_AllocateContext()
            If sdkContext = IntPtr.Zero Then
                Console.WriteLine("Can't allocate sdk context.")
                Return
            End If

            Console.WriteLine("Do you want to set up ssl configuration? [y/N]")
            Console.Write(">>>> ")
            If Not Util.IsNo() Then
                cbPreferMethod = New API.PreferMethod(AddressOf PreferMethodHandle)
                cbGetRootCaFilePath = New API.GetRootCaFilePath(AddressOf GetRootCaFilePathHandle)
                cbGetServerCaFilePath = New API.GetServerCaFilePath(AddressOf GetServerCaFilePathHandle)
                cbGetServerPrivateKeyFilePath = New API.GetServerPrivateKeyFilePath(AddressOf GetServerPrivateKeyFilePathHandle)
                cbGetPassword = New API.GetPassword(AddressOf GetPasswordHandle)
                cbOnErrorOccured = New API.OnErrorOccured(AddressOf OnErrorOccuredHandle)
                'ServicePointManager.SecurityProtocol = (SecurityProtocolType)SecurityProtocolType.Ssl3;

                Dim sdkResult As BS2ErrorCode = CType(API.BS2_SetSSLHandler(sdkContext, cbPreferMethod, cbGetRootCaFilePath, cbGetServerCaFilePath, cbGetServerPrivateKeyFilePath, cbGetPassword, Nothing), BS2ErrorCode)
                If sdkResult <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("BS2_SetSSLHandler failed with : {0}", sdkResult)
                    API.BS2_ReleaseContext(sdkContext)
                    sdkContext = IntPtr.Zero
                    Return
                Else
                    'bSsl = true;
                End If

            End If

            Console.WriteLine("+-----------------------------------------------------------+")
            Console.WriteLine("| 1. Search and connect device                              |")
            Console.WriteLine("| 2. Search and connect device within the subnetwork        |")
            Console.WriteLine("| 3. Connect to device via Ip                               |")
            Console.WriteLine("| 4. Server mode test                                       |")
            Console.WriteLine("+-----------------------------------------------------------+")
            Console.WriteLine("How to connect to device? [3(default)]")
            Console.Write(">>>> ")
            Dim selection As Integer = Util.GetInput(3)
            Select Case selection
                Case 2
                    Console.WriteLine("Enter the IP Address for host ethernet card.")
                    For Each ipAddr As String In Util.GetHostIPAddresses()
                        Console.WriteLine("* {0}", ipAddr)
                    Next
                    Console.Write(">>>> ")
                    Dim deviceIpAddress As String = Console.ReadLine()
                    Dim ipAddress As IPAddress

                    If Not IPAddress.TryParse(deviceIpAddress, ipAddress) Then
                        Console.WriteLine("Invalid ip : " & deviceIpAddress)
                        Return
                    End If

                    Dim ptrIPAddr = Marshal.StringToHGlobalAnsi(deviceIpAddress)

                    result = CType(API.BS2_InitializeEx(sdkContext, ptrIPAddr), BS2ErrorCode)
                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("SDK initialization failed with : {0}, {1}", result, deviceIpAddress)
                        API.BS2_ReleaseContext(sdkContext)
                        sdkContext = IntPtr.Zero
                        Return
                    End If

                Case Else
                    result = CType(API.BS2_Initialize(sdkContext), BS2ErrorCode)
                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("SDK initialization failed with : {0}", result)
                        API.BS2_ReleaseContext(sdkContext)
                        sdkContext = IntPtr.Zero
                        Return
                    End If
            End Select

            cbOnDeviceFound = New API.OnDeviceFound(AddressOf DeviceFound)
            cbOnDeviceAccepted = New API.OnDeviceAccepted(AddressOf DeviceAccepted)
            cbOnDeviceConnected = New API.OnDeviceConnected(AddressOf DeviceConnected)
            cbOnDeviceDisconnected = New API.OnDeviceDisconnected(AddressOf DeviceDisconnected)

            result = CType(API.BS2_SetDeviceEventListener(sdkContext, cbOnDeviceFound, cbOnDeviceAccepted, cbOnDeviceConnected, cbOnDeviceDisconnected), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("Can't register a callback function/method to a sdk.({0})", result)
                API.BS2_ReleaseContext(sdkContext)
                sdkContext = IntPtr.Zero
                Return
            End If

            ' 
            ' if (bSsl)
            ' {
            ' cbOnSendRootCA = new API.OnSendRootCA(SendRootCA);
            ' result = (BS2ErrorCode)API.BS2_SetDeviceSSLEventListener(sdkContext, cbOnSendRootCA);
            ' }
            ' 

#If SDK_AUTO_CONNECTION Then
            result = (BS2ErrorCode)API.BS2_SetAutoConnection(sdkContext, 1);
#End If

            Select Case selection
                Case 1, 2
                    If Not SearchAndConnectDevice(deviceID) Then
                        deviceID = 0
                    End If
                Case 3
                    If Not ConnectToDevice(deviceID) Then
                        deviceID = 0
                    End If
                Case 4
                    If deviceIDForServerMode = 0 Then
                        Console.WriteLine("Waiting for client connection")
                        eventWaitHandle.WaitOne()
                    End If



                    ' 
                    ' result = (BS2ErrorCode)API.BS2_ConnectDevice(sdkContext, deviceID);
                    ' 
                    ' if (result != BS2ErrorCode.BS_SDK_SUCCESS)
                    ' {
                    ' Console.WriteLine("Can't connect to device(errorCode : {0}).", result);
                    ' deviceID = 0;
                    ' }
                    ' else
                    ' {
                    ' Console.WriteLine(">>>> Successfully connected to the device[{0}].", deviceID);
                    ' }
                    ' 

                    deviceID = deviceIDForServerMode
                Case Else
                    Console.WriteLine("Invalid parameter : {0}", selection)
            End Select

            If deviceID > 0 Then
                Console.Title = String.Format("{0} connected deviceID[{1}]", titleField, deviceID)

#If Not SDK_AUTO_CONNECTION
                reconnectionTask = New ReconnectionTask(sdkContext)
                reconnectionTask.start()
#End If
                runImpl(deviceID)
#If Not SDK_AUTO_CONNECTION
                reconnectionTask.stop()
                reconnectionTask = Nothing
#End If

                Console.WriteLine("Trying to discconect device[{0}].", deviceID)
                result = CType(API.BS2_DisconnectDevice(sdkContext, deviceID), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("Got error({0}).", result)
                End If
            End If


            eventWaitHandle.Close()
            API.BS2_ReleaseContext(sdkContext)
            sdkContext = IntPtr.Zero

            cbOnDeviceFound = Nothing
            cbOnDeviceAccepted = Nothing
            cbOnDeviceConnected = Nothing
            cbOnDeviceDisconnected = Nothing
            'cbOnSendRootCA = null;
        End Sub

        Public Sub runWithoutConnection()
            Dim deviceID As UInteger = 0
            Dim versionPtr As IntPtr = API.BS2_Version()
            'bool bSsl = false;

            If titleField.Length > 0 Then
                Console.Title = titleField
            End If

            Console.WriteLine("SDK version : " & Marshal.PtrToStringAnsi(versionPtr))

            Console.WriteLine("Do you want output debug message to file? [y/n]")
            Console.Write(">>>> ")
            If Util.IsYes() Then
                Const CURRENT_DIR = "."
                Const MAX_SIZE_LOG_FILE = 100  ' 100MB
                Dim ptrDir = Marshal.StringToHGlobalAnsi(CURRENT_DIR)
                Dim res As BS2ErrorCode = CType(API.BS2_SetDebugFileLogEx(Constants.DEBUG_LOG_ALL, Constants.DEBUG_MODULE_ALL, ptrDir, MAX_SIZE_LOG_FILE), BS2ErrorCode)
                Marshal.FreeHGlobal(ptrDir)
                If res <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("Got error({0}).", res)
                    Return
                End If
            End If

            sdkContext = API.BS2_AllocateContext()
            If sdkContext = IntPtr.Zero Then
                Console.WriteLine("Can't allocate sdk context.")
                Return
            End If

            Dim result As BS2ErrorCode = CType(API.BS2_Initialize(sdkContext), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("SDK initialization failed with : {0}", result)
                API.BS2_ReleaseContext(sdkContext)
                sdkContext = IntPtr.Zero
                Return
            End If

            runImpl(deviceID)

            API.BS2_ReleaseContext(sdkContext)
            sdkContext = IntPtr.Zero

            cbOnDeviceFound = Nothing
            cbOnDeviceAccepted = Nothing
            cbOnDeviceConnected = Nothing
            cbOnDeviceDisconnected = Nothing
            'cbOnSendRootCA = null;
        End Sub

        Private Function SearchAndConnectDevice(ByRef deviceID As UInteger) As Boolean
            Console.WriteLine("Trying to broadcast on the network")

            Dim result As BS2ErrorCode = CType(API.BS2_SearchDevices(sdkContext), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("Got error : {0}.", result)
                Return False
            End If

            Dim deviceListObj = IntPtr.Zero
            Dim numDevice As UInteger = 0

            Const LONG_TIME_STANDBY_7S As UInteger = 7
            result = CType(API.BS2_SetDeviceSearchingTimeout(sdkContext, LONG_TIME_STANDBY_7S), BS2ErrorCode)
            If BS2ErrorCode.BS_SDK_SUCCESS <> result Then
                Console.WriteLine("Got error : {0}.", result)
                Return False
            End If

            result = CType(API.BS2_GetDevices(sdkContext, deviceListObj, numDevice), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("Got error : {0}.", result)
                Return False
            End If

            If numDevice > 0 Then
                Dim deviceInfo As BS2SimpleDeviceInfo

                Console.WriteLine("+----------------------------------------------------------------------------------------------------------+")
                Dim idx As UInteger = 0

                While idx < numDevice
                                        ''' Cannot convert AssignmentExpressionSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitAssignmentExpression(AssignmentExpressionSyntax node) in D:\GitWorkspace\CodeConverter\CodeConverter\VB\NodesVisitor.cs:line 991
'''    at Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping) in D:\GitWorkspace\CodeConverter\CodeConverter\VB\CommentConvertingVisitorWrapper.cs:line 20
''' 
''' Input:
'''                     deviceID = System.Convert.ToUInt32(System.Runtime.InteropServices.Marshal.ReadInt32(deviceListObj, (int)idx * sizeof(System.UInt32)))
''' 
                    result = CType(API.BS2_GetDeviceInfo(sdkContext, deviceID, deviceInfo), BS2ErrorCode)
                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("Can't get device information(errorCode : {0}).", result)
                        Return False
                    End If

                    Console.WriteLine("[{0, 3:##0}] ==> ID[{1, 10}] Type[{2, 20}] Connection mode[{3}] Ip[{4, 16}] port[{5, 5}] Master/Slave[{6}]", idx, deviceID, If(API.productNameDictionary.ContainsKey(CType(deviceInfo.type, BS2DeviceTypeEnum)), API.productNameDictionary(CType(deviceInfo.type, BS2DeviceTypeEnum)), (API.productNameDictionary(BS2DeviceTypeEnum.UNKNOWN).ToString() & "(" + deviceInfo.type.ToString() & ")")), CType(deviceInfo.connectionMode, BS2ConnectionModeEnum), New IPAddress(BitConverter.GetBytes(deviceInfo.ipv4Address)).ToString(), deviceInfo.port, deviceInfo.rs485Mode)
                    Interlocked.Increment(idx)
                End While
                Console.WriteLine("+----------------------------------------------------------------------------------------------------------+")
                Console.WriteLine("Please, choose the index of the Device which you want to connect to. [-1: quit]")
                Console.Write(">>>> ")

                deviceID = 0
                Dim selection As Integer = Util.GetInput()

                If selection >= 0 Then
                    If selection < numDevice Then
                                                ''' Cannot convert AssignmentExpressionSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitAssignmentExpression(AssignmentExpressionSyntax node) in D:\GitWorkspace\CodeConverter\CodeConverter\VB\NodesVisitor.cs:line 991
'''    at Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping) in D:\GitWorkspace\CodeConverter\CodeConverter\VB\CommentConvertingVisitorWrapper.cs:line 20
''' 
''' Input:
'''                         deviceID = System.Convert.ToUInt32(System.Runtime.InteropServices.Marshal.ReadInt32(deviceListObj, (int)selection * sizeof(System.UInt32)))
''' 
                    Else
                        Console.WriteLine("Invalid selection[{0}]", selection)
                    End If
                End If

                API.BS2_ReleaseObject(deviceListObj)
                If deviceID > 0 Then
                    Console.WriteLine("Trying to connect to device[{0}]", deviceID)
                    result = CType(API.BS2_ConnectDevice(sdkContext, deviceID), BS2ErrorCode)

                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("Can't connect to device(errorCode : {0}).", result)
                        Return False
                    End If

                    Console.WriteLine(">>>> Successfully connected to the device[{0}].", deviceID)
                    Return True
                End If
            Else
                Console.WriteLine("There is no device to launch.")
            End If

            Return False
        End Function

        Private Function ConnectToDevice(ByRef deviceID As UInteger) As Boolean
            Console.WriteLine("Enter the IP Address to connect device")
            Console.Write(">>>> ")
            Dim deviceIpAddress As String = Console.ReadLine()
            Dim ipAddress As IPAddress

            If Not IPAddress.TryParse(deviceIpAddress, ipAddress) Then
                Console.WriteLine("Invalid ip : " & deviceIpAddress)
                Return False
            End If

            Console.WriteLine("Enter the port number to connect device : default[{0}]", BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT)
            Console.Write(">>>> ")
            Dim port As UShort = Util.GetInput(CUShort(BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT))

            Console.WriteLine("Trying to connect to device [ip :{0}, port : {1}]", deviceIpAddress, port)


            Dim ptrIPAddr = Marshal.StringToHGlobalAnsi(deviceIpAddress)
            'BS2ErrorCode result = (BS2ErrorCode)API.BS2_ConnectDeviceViaIP(sdkContext, deviceIpAddress, port, out deviceID);
            Dim result As BS2ErrorCode = CType(API.BS2_ConnectDeviceViaIP(sdkContext, ptrIPAddr, port, deviceID), BS2ErrorCode)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("Can't connect to device(errorCode : {0}).", result)
                Return False
            End If
            Marshal.FreeHGlobal(ptrIPAddr)
#If Not SDK_AUTO_CONNECTION
            ReconnectionTask.setIpPort(deviceIpAddress, port)
#End If
            Console.WriteLine(">>>> Successfully connected to the device[{0}].", deviceID)
            Return True
        End Function

        Private Function ConnectToDeviceSSL(deviceIpAddress As String, ByRef deviceID As UInteger) As Boolean
            Dim port As UShort = Util.GetInput(CUShort(BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT))

            Dim nCnt = 0
            While True
                Dim ptrIPAddr = Marshal.StringToHGlobalAnsi(deviceIpAddress)
                'BS2ErrorCode result = (BS2ErrorCode)API.BS2_ConnectDeviceViaIP(sdkContext, deviceIpAddress, port, out deviceID);
                Dim result As BS2ErrorCode = CType(API.BS2_ConnectDeviceViaIP(sdkContext, ptrIPAddr, port, deviceID), BS2ErrorCode)
                Marshal.FreeHGlobal(ptrIPAddr)

                If nCnt > 7 Then
                    Console.WriteLine("Can't connect to device(errorCode : {0}).", result)
                    Return False
                End If

                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    nCnt += 1
                    Continue While
                Else
                    Exit While
                End If
            End While

            Console.WriteLine(">>>> Successfully connected to the device[{0}].", deviceID)
            Return True
        End Function

        Private Sub PrintDeviceInfo(deviceInfo As BS2SimpleDeviceInfo)
            Console.WriteLine("                        <Device information>")
            Console.WriteLine("+-------------------------------------------------------------+")
            Console.WriteLine("|  ID                                : {0}", deviceInfo.id)
            Console.WriteLine("|  Type                              : {0}({1})", If(API.productNameDictionary.ContainsKey(CType(deviceInfo.type, BS2DeviceTypeEnum)), API.productNameDictionary(CType(deviceInfo.type, BS2DeviceTypeEnum)), API.productNameDictionary(BS2DeviceTypeEnum.UNKNOWN)), deviceInfo.type)
            Console.WriteLine("|  Connection mode                   : {0}", CType(deviceInfo.connectionMode, BS2ConnectionModeEnum))
            Console.WriteLine("|  Ip address                        : {0}", New IPAddress(BitConverter.GetBytes(deviceInfo.ipv4Address)).ToString())
            Console.WriteLine("|  Port number                       : {0}", deviceInfo.port)
            Console.WriteLine("|  Maximum user                      : {0}", deviceInfo.maxNumOfUser)
            Console.WriteLine("|  Supporting user name              : {0}", Convert.ToBoolean(deviceInfo.userNameSupported))
            Console.WriteLine("|  Supporting user profile           : {0}", Convert.ToBoolean(deviceInfo.userPhotoSupported))
            Console.WriteLine("|  Supporting pin code               : {0}", Convert.ToBoolean(deviceInfo.pinSupported))
            Console.WriteLine("|  Supporting card                   : {0}", Convert.ToBoolean(deviceInfo.cardSupported))
            Console.WriteLine("|  Supporting fingerprint            : {0}", Convert.ToBoolean(deviceInfo.fingerSupported))
            Console.WriteLine("|  Supporting face recognition       : {0}", Convert.ToBoolean(deviceInfo.faceSupported))
            Console.WriteLine("|  Supporting wlan                   : {0}", Convert.ToBoolean(deviceInfo.wlanSupported))
            Console.WriteLine("|  Supporting T&A                    : {0}", Convert.ToBoolean(deviceInfo.tnaSupported))
            Console.WriteLine("|  Supporting trigger action         : {0}", Convert.ToBoolean(deviceInfo.triggerActionSupported))
            Console.WriteLine("|  Supporting wiegand                : {0}", Convert.ToBoolean(deviceInfo.wiegandSupported))
            Console.WriteLine("+-------------------------------------------------------------+")
        End Sub

        Private Sub DeviceFound(deviceID As UInteger)
            Console.WriteLine("[CB] Device[{0, 10}] has been found.", deviceID)
        End Sub

        Private Sub DeviceAccepted(deviceID As UInteger)
            Console.WriteLine("[CB] Device[{0, 10}] has been accepted.", deviceID)
            deviceIDForServerMode = deviceID
            eventWaitHandle.Set()
        End Sub

        Private Sub DeviceConnected(deviceID As UInteger)
            Console.WriteLine("[CB] Device[{0, 10}] has been connected.", deviceID)
            If cbOnDeviceConnectedImpl IsNot Nothing Then
                cbOnDeviceConnectedImpl(sdkContext, deviceID)
            Else
                Console.WriteLine("cbOnDeviceConnectedImpl is null")
            End If
        End Sub

        Private Sub DeviceDisconnected(deviceID As UInteger)
            Console.WriteLine("[CB] Device[{0, 10}] has been disconnected.", deviceID)
#If Not SDK_AUTO_CONNECTION
            If reconnectionTask IsNot Nothing Then
                Console.WriteLine("enqueue")
                reconnectionTask.enqueue(deviceID)

            End If
#End If
            If cbOnDeviceDisconnectedImpl IsNot Nothing Then
                cbOnDeviceDisconnectedImpl(sdkContext, deviceID)
            Else
                Console.WriteLine("cbOnDeviceDisconnectedImpl is null")
            End If
        End Sub

        Private Function PreferMethodHandle(deviceID As UInteger) As UInteger
            Return CUInt(BS2SslMethodMaskEnum.TLS1 Or BS2SslMethodMaskEnum.TLS1_1 Or BS2SslMethodMaskEnum.TLS1_2)
        End Function

        Private Function GetRootCaFilePathHandle(deviceID As UInteger) As IntPtr
            'return ssl_server_root_crt;
            If ptr_server_root_crt = IntPtr.Zero Then ptr_server_root_crt = Marshal.StringToHGlobalAnsi(ssl_server_root_crt)
            Return ptr_server_root_crt
        End Function

        Private Function GetServerCaFilePathHandle(deviceID As UInteger) As IntPtr
            'return ssl_server_crt;
            If ptr_server_crt = IntPtr.Zero Then ptr_server_crt = Marshal.StringToHGlobalAnsi(ssl_server_crt)
            Return ptr_server_crt
        End Function

        Private Function GetServerPrivateKeyFilePathHandle(deviceID As UInteger) As IntPtr
            'return ssl_server_pem;
            If ptr_server_pem = IntPtr.Zero Then ptr_server_pem = Marshal.StringToHGlobalAnsi(ssl_server_pem)
            Return ptr_server_pem
        End Function

        Private Function GetPasswordHandle(deviceID As UInteger) As IntPtr
            'return ssl_server_passwd;
            If ptr_server_passwd = IntPtr.Zero Then ptr_server_passwd = Marshal.StringToHGlobalAnsi(ssl_server_passwd)
            Return ptr_server_passwd
        End Function

        Private Sub OnErrorOccuredHandle(deviceID As UInteger, errCode As Integer)
            Console.WriteLine("Got ssl error{0} Device[{1, 10}].", CType(errCode, BS2ErrorCode), deviceID)
        End Sub

        Private Sub SendRootCA(result As Integer)
            If result = 1 Then
                Console.WriteLine("send RootCA Success!!" & vbLf)
            Else
                Console.WriteLine("send RootCA Fail!!" & vbLf)
            End If

        End Sub

        Private Sub DebugExPrint(level As UInteger, [module] As UInteger, msg As String)
            'string printmsg = String.Format("[{0}-{1}] {2}", getModuleName(module), getLevelName(level), msg);
            Dim printmsg = String.Format("{0}", msg)
            'Trace.WriteLine(printmsg);
            Console.WriteLine(printmsg)
        End Sub

        Private Function getModuleName([module] As UInteger) As String
            Select Case [module]
                Case Constants.DEBUG_MODULE_KEEP_ALIVE
                    Return "KAV"
                Case Constants.DEBUG_MODULE_SOCKET_MANAGER
                    Return "SOM"
                Case Constants.DEBUG_MODULE_SOCKET_HANDLER
                    Return "SOH"
                Case Constants.DEBUG_MODULE_DEVICE
                    Return "DEV"
                Case Constants.DEBUG_MODULE_DEVICE_MANAGER
                    Return "DVM"
                Case Constants.DEBUG_MODULE_EVENT_DISPATCHER
                    Return "DIS"
                Case Constants.DEBUG_MODULE_API
                    Return "API"
                Case Constants.DEBUG_MODULE_ALL
                    Return "ALL"
            End Select

            Return "UnK"
        End Function

        Private Function getLevelName(level As UInteger) As String
            Select Case level
                Case Constants.DEBUG_LOG_FATAL
                    Return "FAT"
                Case Constants.DEBUG_LOG_ERROR
                    Return "ERR"
                Case Constants.DEBUG_LOG_WARN
                    Return "WRN"
                Case Constants.DEBUG_LOG_INFO
                    Return "INF"
                Case Constants.DEBUG_LOG_TRACE
                    Return "TRC"
                Case Constants.DEBUG_LOG_OPERATION_ALL
                    Return "OPR"
                Case Constants.DEBUG_LOG_ALL
                    Return "ALL"
            End Select

            Return "UnK"
        End Function


        Private Function ToStringYesNo(value As Boolean) As String
            Return If(value, "y", "n")
        End Function

        Public noConnectionMode As Boolean = False
        Public Sub runWithIPv6()
            Dim delayTerminate = 0
            Dim deviceID As UInteger = 0
            Dim versionPtr As IntPtr = API.BS2_Version()
            'bool bSsl = false;
            Dim result As BS2ErrorCode = BS2ErrorCode.BS_SDK_SUCCESS

            If titleField.Length > 0 Then
                Console.Title = titleField
            End If

            Console.WriteLine("SDK version : " & Marshal.PtrToStringAnsi(versionPtr))

            cbDebugExPrint = Nothing
            Console.WriteLine("Do you want print debug message? [y/N]")
            Console.Write(">>>> ")
            If Not Util.IsNo() Then
                cbDebugExPrint = New API.CBDebugExPrint(AddressOf DebugExPrint)
                result = CType(API.BS2_SetDebugExCallback(cbDebugExPrint, Constants.DEBUG_LOG_OPERATION_ALL, Constants.DEBUG_MODULE_ALL), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("SetDebugExCallback: Got error({0}).", result)
                    ClearSDK(delayTerminate)
                    Return
                End If
            End If

            sdkContext = API.BS2_AllocateContext()
            If sdkContext = IntPtr.Zero Then
                Console.WriteLine("Can't allocate sdk context.")
                ClearSDK(delayTerminate)
                Return
            End If

            Dim responseTimeoutMs = 0
            result = CType(API.BS2_GetDefaultResponseTimeout(sdkContext, responseTimeoutMs), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("GetDefaultResponseTimeout: Got error({0}).", result)
                ClearSDK(delayTerminate)
                Return
            End If
            Console.WriteLine("How long do you have to wait by default for response time? [{0} ms (Default)]", responseTimeoutMs)
            Console.Write(">>>> ")
            responseTimeoutMs = CInt(Util.GetInput(responseTimeoutMs))
            result = CType(API.BS2_SetDefaultResponseTimeout(sdkContext, responseTimeoutMs), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("SetDefaultResponseTimeout: Got error({0}).", result)
                ClearSDK(delayTerminate)
                Return
            End If


            Dim IPv4 = 1
            Dim IPv6 = 0
            result = CType(API.BS2_GetEnableIPV4(sdkContext, IPv4), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("GetEnableIPV4: Got error({0}).", result)
                ClearSDK(delayTerminate)
                Return
            End If
            result = CType(API.BS2_GetEnableIPV6(sdkContext, IPv6), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("GetEnableIPV4: Got error({0}).", result)
                ClearSDK(delayTerminate)
                Return
            End If

            While True
                Console.WriteLine("What do you want to be active between IPv4 and IPv6? [0(IPv4: Default), 1(IPv6), 2(Both)]")
                Console.Write(">>>> ")
                Dim choiceIP As Byte = Util.GetInput(0)
                If choiceIP = 0 Then
                    IPv4 = 1
                    IPv6 = 0
                ElseIf choiceIP = 1 Then
                    IPv4 = 0
                    IPv6 = 1
                ElseIf choiceIP = 2 Then
                    IPv4 = 1
                    IPv6 = 1
                Else
                    Console.WriteLine("Wrong selection")
                    Continue While
                End If
                Exit While
            End While

            If IPv4 = 1 Then
                Dim port As UShort = BS2Environment.BS2_TCP_SERVER_PORT_DEFAULT
                result = CType(API.BS2_GetServerPort(sdkContext, port), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("GetServerPort: Got error({0}).", result)
                    ClearSDK(delayTerminate)
                    Return
                End If

                Console.WriteLine("What server port number will you use in IPv4? [{0} Default]", port)
                Console.Write(">>>> ")
                port = Util.GetInput(port)
                result = CType(API.BS2_SetServerPort(sdkContext, port), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("SetServerPort: Got error({0}).", result)
                    ClearSDK(delayTerminate)
                    Return
                End If
            End If

            If IPv6 = 1 Then
                Dim port As UShort = BS2Environment.BS2_TCP_SERVER_PORT_DEFAULT_V6
                result = CType(API.BS2_GetServerPortIPV6(sdkContext, port), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("GetServerPortIPV6: Got error({0}).", result)
                    ClearSDK(delayTerminate)
                    Return
                End If

                Console.WriteLine("What server port number will you use in IPv6? [{0} Default]", port)
                Console.Write(">>>> ")
                port = Util.GetInput(port)
                result = CType(API.BS2_SetServerPortIPV6(sdkContext, port), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("SetServerPortIPV6: Got error({0}).", result)
                    ClearSDK(delayTerminate)
                    Return
                End If
            End If

            result = CType(API.BS2_SetEnableIPV4(sdkContext, IPv4), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("SetEnableIPV4: Got error({0}).", result)
                ClearSDK(delayTerminate)
                Return
            End If

            result = CType(API.BS2_SetEnableIPV6(sdkContext, IPv6), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("SetEnableIPV6: Got error({0}).", result)
                ClearSDK(delayTerminate)
                Return
            End If

            Console.WriteLine("Do you want to set up ssl configuration? [Y/n]")
            Console.Write(">>>> ")
            If Util.IsYes() Then
                If IPv4 = 1 Then
                    Dim sslPort As UShort = BS2Environment.BS2_TCP_SSL_SERVER_PORT_DEFAULT
                    result = CType(API.BS2_GetSSLServerPort(sdkContext, sslPort), BS2ErrorCode)
                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("GetSSLServerPort: Got error({0}).", result)
                        ClearSDK(delayTerminate)
                        Return
                    End If

                    Console.WriteLine("What ssl server port number will you use in IPv4? [{0} Default]", sslPort)
                    Console.Write(">>>> ")
                    sslPort = Util.GetInput(sslPort)
                    result = CType(API.BS2_SetSSLServerPort(sdkContext, sslPort), BS2ErrorCode)
                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("SetSSLServerPort: Got error({0}).", result)
                        ClearSDK(delayTerminate)
                        Return
                    End If
                End If

                If IPv6 = 1 Then
                    Dim sslPort As UShort = BS2Environment.BS2_TCP_SSL_SERVER_PORT_DEFAULT_V6
                    result = CType(API.BS2_GetSSLServerPortIPV6(sdkContext, sslPort), BS2ErrorCode)
                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("GetSSLServerPortIPV6: Got error({0}).", result)
                        ClearSDK(delayTerminate)
                        Return
                    End If

                    Console.WriteLine("What ssl server port number will you use in IPv6? [{0} Default]", sslPort)
                    Console.Write(">>>> ")
                    sslPort = Util.GetInput(sslPort)
                    result = CType(API.BS2_SetSSLServerPortIPV6(sdkContext, sslPort), BS2ErrorCode)
                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("SetSSLServerPortIPV6: Got error({0}).", result)
                        ClearSDK(delayTerminate)
                        Return
                    End If
                End If

                cbPreferMethod = New API.PreferMethod(AddressOf PreferMethodHandle)
                cbGetRootCaFilePath = New API.GetRootCaFilePath(AddressOf GetRootCaFilePathHandle)
                cbGetServerCaFilePath = New API.GetServerCaFilePath(AddressOf GetServerCaFilePathHandle)
                cbGetServerPrivateKeyFilePath = New API.GetServerPrivateKeyFilePath(AddressOf GetServerPrivateKeyFilePathHandle)
                cbGetPassword = New API.GetPassword(AddressOf GetPasswordHandle)
                cbOnErrorOccured = New API.OnErrorOccured(AddressOf OnErrorOccuredHandle)
                'ServicePointManager.SecurityProtocol = (SecurityProtocolType)SecurityProtocolType.Ssl3;

                result = CType(API.BS2_SetSSLHandler(sdkContext, cbPreferMethod, cbGetRootCaFilePath, cbGetServerCaFilePath, cbGetServerPrivateKeyFilePath, cbGetPassword, Nothing), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("SetSSLHandler: Got error({0}).", result)
                    ClearSDK(delayTerminate)
                    Return
                Else
                    'bSsl = true;
                End If
            End If

            If IPv4 = 1 Then
                Dim serverPort As UShort = 0
                result = CType(API.BS2_GetServerPort(sdkContext, serverPort), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("GetServerPort: Got error({0}).", result)
                    ClearSDK(delayTerminate)
                    Return
                End If
                Console.WriteLine("Server Port on IPv4: {0}", serverPort)

                Dim sslServerPort As UShort = 0
                result = CType(API.BS2_GetSSLServerPort(sdkContext, sslServerPort), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("GetSSLServerPort: Got error({0}).", result)
                    ClearSDK(delayTerminate)
                    Return
                End If
                Console.WriteLine("SSL Server Port on IPv4: {0}", sslServerPort)
            End If

            If IPv6 = 1 Then
                Dim serverPort As UShort = 0
                result = CType(API.BS2_GetServerPortIPV6(sdkContext, serverPort), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("GetServerPortIPV6: Got error({0}).", result)
                    ClearSDK(delayTerminate)
                    Return
                End If
                Console.WriteLine("Server Port on IPv6: {0}", serverPort)

                Dim sslServerPort As UShort = 0
                result = CType(API.BS2_GetSSLServerPortIPV6(sdkContext, sslServerPort), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("GetSSLServerPort: Got error({0}).", result)
                    ClearSDK(delayTerminate)
                    Return
                End If
                Console.WriteLine("SSL Server Port on IPv6: {0}", sslServerPort)
            End If

            result = CType(API.BS2_Initialize(sdkContext), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("SDK initialization failed with : {0}", result)
                ClearSDK(delayTerminate)
                Return
            End If

            cbOnDeviceFound = New API.OnDeviceFound(AddressOf DeviceFound)
            cbOnDeviceAccepted = New API.OnDeviceAccepted(AddressOf DeviceAccepted)
            cbOnDeviceConnected = New API.OnDeviceConnected(AddressOf DeviceConnected)
            cbOnDeviceDisconnected = New API.OnDeviceDisconnected(AddressOf DeviceDisconnected)

            result = CType(API.BS2_SetDeviceEventListener(sdkContext, cbOnDeviceFound, cbOnDeviceAccepted, cbOnDeviceConnected, cbOnDeviceDisconnected), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("Can't register a callback function/method to a sdk.({0})", result)
                ClearSDK(delayTerminate)
                Return
            End If

            ' 
            ' if (bSsl)
            ' {
            ' cbOnSendRootCA = new API.OnSendRootCA(SendRootCA);
            ' result = (BS2ErrorCode)API.BS2_SetDeviceSSLEventListener(sdkContext, cbOnSendRootCA);
            ' }
            ' 

#If SDK_AUTO_CONNECTION Then
            result = (BS2ErrorCode)API.BS2_SetAutoConnection(sdkContext, 1);
#End If

            noConnectionMode = False
            Do
                Console.WriteLine("+-----------------------------------------------------------+")
                Console.WriteLine("| 1. Search and connect device                              |")
                Console.WriteLine("| 2. Connect to device via Ip                               |")
                Console.WriteLine("| 3. Server mode test                                       |")
                Console.WriteLine("| 4. Get IP Config via UDP                                  |")
                Console.WriteLine("| 5. Set IP Config via UDP                                  |")
                Console.WriteLine("| 6. Get IPV6 Config via UDP                                |")
                Console.WriteLine("| 7. Set IPV6 Config via UDP                                |")
                Console.WriteLine("| 8. No Connection for USB                                  |")
                Console.WriteLine("+-----------------------------------------------------------+")
                Console.WriteLine("How to connect to device? [2(default)]")
                Console.Write(">>>> ")
                Dim selection As Integer = Util.GetInput(2)

                Select Case selection
                    Case 1
                        If Not SearchAndConnectDeviceWithIPv6(deviceID) Then
                            deviceID = 0
                        End If
                    Case 2
                        If Not ConnectToDeviceWithIPv6(deviceID) Then
                            deviceID = 0
                        End If
                    Case 3
                        If deviceIDForServerMode = 0 Then
                            Console.WriteLine("Waiting for client connection")
                            eventWaitHandle.WaitOne()
                        End If



                        ' 
                        ' result = (BS2ErrorCode)API.BS2_ConnectDevice(sdkContext, deviceID);
                        ' 
                        ' if (result != BS2ErrorCode.BS_SDK_SUCCESS)
                        ' {
                        ' Console.WriteLine("Can't connect to device(errorCode : {0}).", result);
                        ' deviceID = 0;
                        ' }
                        ' else
                        ' {
                        ' Console.WriteLine(">>>> Successfully connected to the device[{0}].", deviceID);
                        ' }
                        ' 

                        deviceID = deviceIDForServerMode
                    Case 4
                        If Not GetIPConfigViaUDP(deviceID) Then
                            deviceID = 0
                        End If
                    Case 5
                        SetIPConfigViaUDP()
                    Case 6
                        If Not GetIPV6ConfigViaUDP(deviceID) Then
                            deviceID = 0
                        End If
                    Case 7
                        SetIPV6ConfigViaUDP()
                    Case 8
                        noConnectionMode = True
                    Case Else
                        Console.WriteLine("Invalid parameter : {0}", selection)
                End Select
            Loop While deviceID = 0 AndAlso noConnectionMode = False

            If noConnectionMode = False AndAlso deviceID > 0 Then
                Console.Title = String.Format("{0} connected deviceID[{1}]", titleField, deviceID)

#If Not SDK_AUTO_CONNECTION
                reconnectionTask = New ReconnectionTask(sdkContext)
                reconnectionTask.start()
#End If
                runImpl(deviceID)
#If Not SDK_AUTO_CONNECTION
                reconnectionTask.stop()
                reconnectionTask = Nothing
#End If

                Console.WriteLine("Trying to discconect device[{0}].", deviceID)
                result = CType(API.BS2_DisconnectDevice(sdkContext, deviceID), BS2ErrorCode)
                If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                    Console.WriteLine("Got error({0}).", result)
                    ClearSDK(delayTerminate)
                End If
            ElseIf noConnectionMode = True Then
                Console.Title = String.Format("{0} No Connection Mode", titleField)

                runImpl(deviceID)
            End If

            eventWaitHandle.Close()
            ClearSDK(delayTerminate)

            cbOnDeviceFound = Nothing
            cbOnDeviceAccepted = Nothing
            cbOnDeviceConnected = Nothing
            cbOnDeviceDisconnected = Nothing
            'cbOnSendRootCA = null;

        End Sub

        Private Sub ClearSDK(delayTerminate As Integer)
            If sdkContext <> IntPtr.Zero Then
                API.BS2_ReleaseContext(sdkContext)
            End If
            sdkContext = IntPtr.Zero
            Thread.Sleep(delayTerminate)
        End Sub

        Private Function SearchAndConnectDeviceWithIPv6(ByRef deviceID As UInteger) As Boolean
            Dim IPv6 = True
            Dim IPv4 = True
            Console.WriteLine("Which mode do you want to use between IPv4 and IPv6? [0(IPv4), 1(IPv6), 2(Both: Default)]")
            Console.Write(">>>> ")
            Dim choiceIP As Integer = Util.GetInput(2)
            If choiceIP = 0 Then
                IPv4 = True
                IPv6 = False
            ElseIf choiceIP = 1 Then
                IPv4 = False
                IPv6 = True
            End If

            Console.WriteLine("Trying to broadcast on the network")

            Dim ptrV4Broad = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_BROADCAST_IPV4_ADDRESS)
            Dim ptrV6Multi = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_MULTICAST_IPV6_ADDRESS)

            Dim result As BS2ErrorCode = BS2ErrorCode.BS_SDK_SUCCESS
            If IPv4 AndAlso IPv6 Then
                result = CType(API.BS2_SearchDevices(sdkContext), BS2ErrorCode)
            ElseIf IPv4 AndAlso Not IPv6 Then
                'result = (BS2ErrorCode)API.BS2_SearchDevicesEx(sdkContext, BS2Environment.DEFAULT_BROADCAST_IPV4_ADDRESS);
                result = CType(API.BS2_SearchDevicesEx(sdkContext, ptrV4Broad), BS2ErrorCode)
            ElseIf Not IPv4 AndAlso IPv6 Then
                'result = (BS2ErrorCode)API.BS2_SearchDevicesEx(sdkContext, BS2Environment.DEFAULT_MULTICAST_IPV6_ADDRESS);
                result = CType(API.BS2_SearchDevicesEx(sdkContext, ptrV6Multi), BS2ErrorCode)
            End If

            Marshal.FreeHGlobal(ptrV4Broad)
            Marshal.FreeHGlobal(ptrV6Multi)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("SearchDevices?? : Got error : {0}.", result)
                Return False
            End If

            Dim deviceListObj = IntPtr.Zero
            Dim numDevice As UInteger = 0

            Const LONG_TIME_STANDBY_7S As UInteger = 7
            result = CType(API.BS2_SetDeviceSearchingTimeout(sdkContext, LONG_TIME_STANDBY_7S), BS2ErrorCode)
            If BS2ErrorCode.BS_SDK_SUCCESS <> result Then
                Console.WriteLine("SetDeviceSearchingTimeout: Got error : {0}.", result)
                Return False
            End If

            result = CType(API.BS2_GetDevices(sdkContext, deviceListObj, numDevice), BS2ErrorCode)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("GetDevices: Got error : {0}.", result)
                Return False
            End If

            If numDevice > 0 Then
                Dim deviceInfo As BS2SimpleDeviceInfo
                Dim structType = GetType(BS2IPv6DeviceInfo)
                Dim structSize = Marshal.SizeOf(structType)
                Dim buffer = Marshal.AllocHGlobal(structSize)
                Dim outStructSize As UInteger = 0

                Console.WriteLine("+----------------------------------------------------------------------------------------------------------+")
                Dim idx As UInteger = 0

                While idx < numDevice
                                        ''' Cannot convert AssignmentExpressionSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitAssignmentExpression(AssignmentExpressionSyntax node) in D:\GitWorkspace\CodeConverter\CodeConverter\VB\NodesVisitor.cs:line 991
'''    at Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping) in D:\GitWorkspace\CodeConverter\CodeConverter\VB\CommentConvertingVisitorWrapper.cs:line 20
''' 
''' Input:
'''                     deviceID = System.Convert.ToUInt32(System.Runtime.InteropServices.Marshal.ReadInt32(deviceListObj, (int)idx * sizeof(System.UInt32)))
''' 

                    result = CType(API.BS2_GetSpecifiedDeviceInfo(sdkContext, deviceID, CUInt(BS2SpecifiedDeviceInfo.BS2_SPECIFIED_DEVICE_INFO_IPV6), buffer, structSize, outStructSize), BS2ErrorCode)
                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("GetSpecifiedDeviceInfo: Got error : {0}.", result)
                        Marshal.FreeHGlobal(buffer)
                        Return False
                    End If
                    Dim devicInfoIPv6 As BS2IPv6DeviceInfo = CType(Marshal.PtrToStructure(buffer, structType), BS2IPv6DeviceInfo)

                    result = CType(API.BS2_GetDeviceInfo(sdkContext, deviceID, deviceInfo), BS2ErrorCode)
                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("GetDeviceInfo: Got error : {0}.", result)
                        Marshal.FreeHGlobal(buffer)
                        Return False
                    End If

                    Console.WriteLine("[{0, 3:##0}] ==> ID[{1, 10}] Type[{2, 16}] Connection mode[{3}] IPv4[{4}] IPv4-Port[{5}], IPv6[{6}] IPv6-Port[{7}]", idx, deviceID, If(API.productNameDictionary.ContainsKey(CType(deviceInfo.type, BS2DeviceTypeEnum)), API.productNameDictionary(CType(deviceInfo.type, BS2DeviceTypeEnum)), (API.productNameDictionary(BS2DeviceTypeEnum.UNKNOWN).ToString() & "(" + deviceInfo.type.ToString() & ")")), CType(deviceInfo.connectionMode, BS2ConnectionModeEnum), New IPAddress(BitConverter.GetBytes(deviceInfo.ipv4Address)).ToString(), deviceInfo.port, Encoding.UTF8.GetString(devicInfoIPv6.ipv6Address).TrimEnd(ChrW(0)), devicInfoIPv6.portV6)
                    Interlocked.Increment(idx)
                End While

                Marshal.FreeHGlobal(buffer)

                Console.WriteLine("+----------------------------------------------------------------------------------------------------------+")
                Console.WriteLine("Please, choose the index of the Device which you want to connect to. [-1: quit]")
                Console.Write(">>>> ")

                deviceID = 0
                Dim selection As Integer = Util.GetInput()

                If selection >= 0 Then
                    If selection < numDevice Then
                                                ''' Cannot convert AssignmentExpressionSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitAssignmentExpression(AssignmentExpressionSyntax node) in D:\GitWorkspace\CodeConverter\CodeConverter\VB\NodesVisitor.cs:line 991
'''    at Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping) in D:\GitWorkspace\CodeConverter\CodeConverter\VB\CommentConvertingVisitorWrapper.cs:line 20
''' 
''' Input:
'''                         deviceID = System.Convert.ToUInt32(System.Runtime.InteropServices.Marshal.ReadInt32(deviceListObj, (int)selection * sizeof(System.UInt32)))
''' 
                    Else
                        Console.WriteLine("Invalid selection[{0}]", selection)
                    End If
                End If

                API.BS2_ReleaseObject(deviceListObj)
                If deviceID > 0 Then
                    Dim buffer1 = Marshal.AllocHGlobal(structSize)
                    result = CType(API.BS2_GetSpecifiedDeviceInfo(sdkContext, deviceID, CUInt(BS2SpecifiedDeviceInfo.BS2_SPECIFIED_DEVICE_INFO_IPV6), buffer1, structSize, outStructSize), BS2ErrorCode)
                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("GetSpecifiedDeviceInfo: Got error : {0}.", result)
                        Marshal.FreeHGlobal(buffer1)
                        Return False
                    End If
                    Dim connectIPv6 = False
                    Dim devicInfoIPv6 As BS2IPv6DeviceInfo = CType(Marshal.PtrToStructure(buffer1, structType), BS2IPv6DeviceInfo)
                    Dim tempAddress As IPAddress
                    Dim bCanUseIPv6 As Boolean = Encoding.UTF8.GetString(devicInfoIPv6.ipv6Address).TrimEnd(CChar(ChrW(0))).Length > 0 AndAlso IPAddress.TryParse(Encoding.UTF8.GetString(devicInfoIPv6.ipv6Address).TrimEnd(ChrW(0)), tempAddress) AndAlso tempAddress.AddressFamily = Sockets.AddressFamily.InterNetworkV6
                    If bCanUseIPv6 Then
                        Console.WriteLine("Do you want to connect via IPv6? [Y/n]")
                        Console.Write(">>>>")
                        If Util.IsYes() Then
                            connectIPv6 = True
                        End If
                    End If
                    Marshal.FreeHGlobal(buffer1)

                    Console.WriteLine("Trying to connect to device[{0}]", deviceID)

                    If connectIPv6 Then
                        result = CType(API.BS2_ConnectDeviceIPV6(sdkContext, deviceID), BS2ErrorCode)
                    Else
                        result = CType(API.BS2_ConnectDevice(sdkContext, deviceID), BS2ErrorCode)
                    End If

                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("BS2_ConnectDevice???: Got error : {0}.", result)
                        Return False
                    End If

                    Console.WriteLine(">>>> Successfully connected to the device[{0}].", deviceID)
                    Return True
                End If
            Else
                Console.WriteLine("There is no device to launch.")
            End If

            Return False
        End Function

        Private Function ConnectToDeviceWithIPv6(ByRef deviceID As UInteger) As Boolean
            Console.WriteLine("Enter the IP Address to connect device")
            Console.Write(">>>> ")
            Dim deviceIpAddress As String = Console.ReadLine()
            Dim ipAddress As IPAddress

            If Not IPAddress.TryParse(deviceIpAddress, ipAddress) Then
                Console.WriteLine("Invalid ip : " & deviceIpAddress)
                Return False
            End If

            Console.WriteLine("Enter the port number to connect device : default[{0}]", If(ipAddress.AddressFamily = Sockets.AddressFamily.InterNetworkV6, BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT_V6, BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT)) '[IPv6] <=
            Console.Write(">>>> ")
            Dim port As UShort = Util.GetInput(If(ipAddress.AddressFamily = Sockets.AddressFamily.InterNetworkV6, BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT_V6, BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT)) '[IPv6] <=

            Console.WriteLine("Trying to connect to device [ip :{0}, port : {1}]", deviceIpAddress, port)


            Dim ptrIPAddr = Marshal.StringToHGlobalAnsi(deviceIpAddress)
            'BS2ErrorCode result = (BS2ErrorCode)API.BS2_ConnectDeviceViaIP(sdkContext, deviceIpAddress, port, out deviceID);
            Dim result As BS2ErrorCode = CType(API.BS2_ConnectDeviceViaIP(sdkContext, ptrIPAddr, port, deviceID), BS2ErrorCode)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("BS2_ConnectDeviceViaIP: Got error : {0}.", result)
                Return False
            End If
            Marshal.FreeHGlobal(ptrIPAddr)

            Console.WriteLine(">>>> Successfully connected to the device[{0}].", deviceID)
            Return True
        End Function

        Private Sub print(config As BS2IpConfig)
            Console.WriteLine(">>>> IP configuration ")
            Console.WriteLine("     |--connectionMode : {0}", config.connectionMode)
            Console.WriteLine("     |--useDHCP : {0}", config.useDHCP)
            Console.WriteLine("     |--useDNS : {0}", config.useDNS)
            Console.WriteLine("     |--ipAddress : {0}", Encoding.UTF8.GetString(config.ipAddress), BitConverter.ToString(config.ipAddress))
            Console.WriteLine("     |--gateway : {0}", Encoding.UTF8.GetString(config.gateway), BitConverter.ToString(config.gateway))
            Console.WriteLine("     |--subnetMask : {0}", Encoding.UTF8.GetString(config.subnetMask), BitConverter.ToString(config.subnetMask))
            Console.WriteLine("     |--serverAddr : {0}", Encoding.UTF8.GetString(config.serverAddr), BitConverter.ToString(config.serverAddr))
            Console.WriteLine("     |--port : {0}", config.port)
            Console.WriteLine("     |--serverPort : {0}", config.serverPort)
            Console.WriteLine("     |--mtuSize : {0}", config.mtuSize)
            Console.WriteLine("     |--baseband : {0}", config.baseband)
            Console.WriteLine("     |--sslServerPort : {0}", config.sslServerPort)
            Console.WriteLine("<<<< ")
        End Sub

        Private Sub print(config As BS2IPV6Config)
            Console.WriteLine(">>>> IPV6 configuration ")
            Console.WriteLine("     |--useIPV6 : {0}", config.useIPV6)
            Console.WriteLine("     |--reserved1 : {0}", config.reserved1) ' useIPV4);
            Console.WriteLine("     |--useDhcpV6 : {0}", config.useDhcpV6)
            Console.WriteLine("     |--useDnsV6 : {0}", config.useDnsV6)
            Console.WriteLine("     |--staticIpAddressV6 : {0}", Encoding.UTF8.GetString(config.staticIpAddressV6), BitConverter.ToString(config.staticIpAddressV6))
            Console.WriteLine("     |--staticGatewayV6 : {0}", Encoding.UTF8.GetString(config.staticGatewayV6), BitConverter.ToString(config.staticGatewayV6))
            Console.WriteLine("     |--dnsAddrV6 : {0}", Encoding.UTF8.GetString(config.dnsAddrV6), BitConverter.ToString(config.dnsAddrV6))
            Console.WriteLine("     |--serverIpAddressV6 : {0}", Encoding.UTF8.GetString(config.serverIpAddressV6), BitConverter.ToString(config.serverIpAddressV6))
            Console.WriteLine("     |--serverPortV6 : {0}", config.serverPortV6)
            Console.WriteLine("     |--sslServerPortV6 : {0}", config.sslServerPortV6)
            Console.WriteLine("     |--portV6 : {0}", config.portV6)
            Console.WriteLine("     |--numOfAllocatedAddressV6 : {0}", config.numOfAllocatedAddressV6)
            Console.WriteLine("     |--numOfAllocatedGatewayV6 : {0}", config.numOfAllocatedGatewayV6)
            Dim tempIPV6 = New Byte(BS2Environment.BS2_IPV6_ADDR_SIZE - 1) {}
            Dim idx = 0

            While idx < config.numOfAllocatedAddressV6
                Array.Copy(config.allocatedIpAddressV6, idx * BS2Environment.BS2_IPV6_ADDR_SIZE, tempIPV6, 0, BS2Environment.BS2_IPV6_ADDR_SIZE)
                Console.WriteLine("     |--allocatedIpAddressV6[{0}] : {1}", idx, Encoding.UTF8.GetString(tempIPV6), BitConverter.ToString(tempIPV6))
                Interlocked.Increment(idx)
            End While
            idx = 0

            While idx < config.numOfAllocatedGatewayV6
                Array.Copy(config.allocatedGatewayV6, idx * BS2Environment.BS2_IPV6_ADDR_SIZE, tempIPV6, 0, BS2Environment.BS2_IPV6_ADDR_SIZE)
                Console.WriteLine("     |--allocatedGatewayV6[{0}] : {1}", idx, Encoding.UTF8.GetString(tempIPV6), BitConverter.ToString(tempIPV6))
                Interlocked.Increment(idx)
            End While
            Console.WriteLine("<<<< ")
        End Sub

        Private Function GetIPConfigViaUDP(ByRef deviceID As UInteger) As Boolean
            Console.WriteLine("What is the ID of the device for which you want to get IP config?")
            Console.Write(">>>> ")
            Dim inputID As UInteger = Util.GetInput(0)
            If inputID = 0 Then
                Console.WriteLine("Invalid Device ID")
                Return False
            End If

            Dim IPv6 = True
            Dim IPv4 = True
            Console.WriteLine("Which mode do you want to use between IPv4 and IPv6? [0(IPv4), 1(IPv6), 2(Both: Default)]")
            Console.Write(">>>> ")
            Dim choiceIP As Integer = Util.GetInput(2)
            If choiceIP = 0 Then
                IPv4 = True
                IPv6 = False
            ElseIf choiceIP = 1 Then
                IPv4 = False
                IPv6 = True
            End If

            Console.WriteLine("Trying to send packet via UDP on the network")

            Dim config As BS2IpConfig
            Dim result As BS2ErrorCode = BS2ErrorCode.BS_SDK_SUCCESS
            Dim ptrV4Broad = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_BROADCAST_IPV4_ADDRESS)
            Dim ptrV6Multi = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_MULTICAST_IPV6_ADDRESS)
            If IPv4 AndAlso IPv6 Then
                result = CType(API.BS2_GetIPConfigViaUDP(sdkContext, inputID, config), BS2ErrorCode)
            ElseIf IPv4 AndAlso Not IPv6 Then
                result = CType(API.BS2_GetIPConfigViaUDPEx(sdkContext, inputID, config, ptrV4Broad), BS2ErrorCode)
            ElseIf Not IPv4 AndAlso IPv6 Then
                result = CType(API.BS2_GetIPConfigViaUDPEx(sdkContext, inputID, config, ptrV6Multi), BS2ErrorCode)
            Else
                config = DirectCast(Nothing, BS2IpConfig)
            End If

            Marshal.FreeHGlobal(ptrV4Broad)
            Marshal.FreeHGlobal(ptrV6Multi)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("GetIPConfigViaUDP??: Got error : {0}.", result)
                Return False
            Else
                Me.print(config)

                Console.WriteLine("+----------------------------------------------------------------------------------------------------------+")

                Console.WriteLine("==> ID[{0, 10}] Connection mode[{1}] IPv4[{2}] IPv4-Port[{3}]", inputID, CType(config.connectionMode, BS2ConnectionModeEnum), Encoding.UTF8.GetString(config.ipAddress).TrimEnd(ChrW(0)), config.port)

                Console.WriteLine("+----------------------------------------------------------------------------------------------------------+")
                Console.WriteLine("Do you want to connect? [Y/n]")
                Console.Write(">>>> ")
                If Util.IsYes() Then

                    Console.WriteLine("Trying to connect to device[{0}]", inputID)

                    Dim ptrIPAddr = Marshal.StringToHGlobalAnsi(Encoding.UTF8.GetString(config.ipAddress).TrimEnd(ChrW(0)))
                    'result = (BS2ErrorCode)API.BS2_ConnectDeviceViaIP(sdkContext, Encoding.UTF8.GetString(config.ipAddress).TrimEnd('\0'), config.port, out deviceID);
                    result = CType(API.BS2_ConnectDeviceViaIP(sdkContext, ptrIPAddr, config.port, deviceID), BS2ErrorCode)

                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("ConnectDeviceViaIP: Got error : {0}.", result)
                        Return False
                    End If
                    Marshal.FreeHGlobal(ptrIPAddr)

                    Console.WriteLine(">>>> Successfully connected to the device[{0}].", deviceID)
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

        Private Function GetIPV6ConfigViaUDP(ByRef deviceID As UInteger) As Boolean
            Console.WriteLine("What is the ID of the device for which you want to get IP config?")
            Console.Write(">>>> ")
            Dim inputID As UInteger = Util.GetInput(0)
            If inputID = 0 Then
                Console.WriteLine("Invalid Device ID")
                Return False
            End If

            Dim IPv6 = True
            Dim IPv4 = True
            Console.WriteLine("Which mode do you want to use between IPv4 and IPv6? [0(IPv4), 1(IPv6), 2(Both: Default)]")
            Console.Write(">>>> ")
            Dim choiceIP As Integer = Util.GetInput(2)
            If choiceIP = 0 Then
                IPv4 = True
                IPv6 = False
            ElseIf choiceIP = 1 Then
                IPv4 = False
                IPv6 = True
            End If

            Console.WriteLine("Trying to send packet via UDP on the network")

            Dim config As BS2IPV6Config
            Dim result As BS2ErrorCode = BS2ErrorCode.BS_SDK_SUCCESS
            Dim ptrV4Broad = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_BROADCAST_IPV4_ADDRESS)
            Dim ptrV6Multi = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_MULTICAST_IPV6_ADDRESS)
            If IPv4 AndAlso IPv6 Then
                result = CType(API.BS2_GetIPV6ConfigViaUDP(sdkContext, inputID, config), BS2ErrorCode)
            ElseIf IPv4 AndAlso Not IPv6 Then
                result = CType(API.BS2_GetIPV6ConfigViaUDPEx(sdkContext, inputID, config, ptrV4Broad), BS2ErrorCode)
            ElseIf Not IPv4 AndAlso IPv6 Then
                result = CType(API.BS2_GetIPV6ConfigViaUDPEx(sdkContext, inputID, config, ptrV6Multi), BS2ErrorCode)
            Else
                config = DirectCast(Nothing, BS2IPV6Config)
            End If

            Marshal.FreeHGlobal(ptrV4Broad)
            Marshal.FreeHGlobal(ptrV6Multi)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("GetIPV6ConfigViaUDP??: Got error : {0}.", result)
                Return False
            Else
                Me.print(config)

                Console.WriteLine("+----------------------------------------------------------------------------------------------------------+")

                Dim allocatedIpAddressV6_0 = New Byte(BS2Environment.BS2_IPV6_ADDR_SIZE - 1) {}
                Array.Copy(config.allocatedIpAddressV6, 0, allocatedIpAddressV6_0, 0, BS2Environment.BS2_IPV6_ADDR_SIZE)
                Console.WriteLine("==> ID[{0, 10}] numOfAllocated[{1}] IPv6[{2}] IPv6-Port[{3}]", inputID, config.numOfAllocatedAddressV6, Encoding.UTF8.GetString(allocatedIpAddressV6_0).TrimEnd(ChrW(0)), config.portV6)

                Console.WriteLine("+----------------------------------------------------------------------------------------------------------+")
                Console.WriteLine("Do you want to connect? [Y/n]")
                Console.Write(">>>> ")
                If Util.IsYes() Then
                    Dim strIpAddressV6 = Encoding.UTF8.GetString(allocatedIpAddressV6_0).TrimEnd(ChrW(0))
                    If strIpAddressV6.IndexOf("/"c) <> -1 Then
                        strIpAddressV6 = strIpAddressV6.Substring(0, strIpAddressV6.IndexOf("/"c))
                    End If
                    Console.WriteLine("Trying to connect to device[{0}][{1}]", inputID, strIpAddressV6)

                    Dim ptrIPAddr = Marshal.StringToHGlobalAnsi(strIpAddressV6)
                    result = CType(API.BS2_ConnectDeviceViaIP(sdkContext, ptrIPAddr, config.portV6, deviceID), BS2ErrorCode)

                    If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                        Console.WriteLine("ConnectDeviceViaIP??: Got error : {0}.", result)
                        Return False
                    End If
                    Marshal.FreeHGlobal(ptrIPAddr)

                    Console.WriteLine(">>>> Successfully connected to the device[{0}].", deviceID)
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

        Public Sub SetIPConfigViaUDP()
            Console.WriteLine("What is the ID of the device for which you want to set IP config?")
            Console.Write(">>>> ")
            Dim inputID As UInteger = Util.GetInput(0)
            If inputID = 0 Then
                Console.WriteLine("Invalid Device ID")
                Return
            End If

            Dim IPv6 = True
            Dim IPv4 = True
            Console.WriteLine("Which mode do you want to use between IPv4 and IPv6? [0(IPv4), 1(IPv6), 2(Both: Default)]")
            Console.Write(">>>> ")
            Dim choiceIP As Integer = Util.GetInput(2)
            If choiceIP = 0 Then
                IPv4 = True
                IPv6 = False
            ElseIf choiceIP = 1 Then
                IPv4 = False
                IPv6 = True
            End If

            Dim config As BS2IpConfig
            Console.WriteLine("Trying to get Current IPConfig via UDP")
            Dim result As BS2ErrorCode = BS2ErrorCode.BS_SDK_SUCCESS
            If IPv4 AndAlso IPv6 Then
                result = CType(API.BS2_GetIPConfigViaUDP(sdkContext, inputID, config), BS2ErrorCode)
            ElseIf IPv4 AndAlso Not IPv6 Then
                Dim ptrV4Broad = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_BROADCAST_IPV4_ADDRESS)
                result = CType(API.BS2_GetIPConfigViaUDPEx(sdkContext, inputID, config, ptrV4Broad), BS2ErrorCode)
                Marshal.FreeHGlobal(ptrV4Broad)
            ElseIf Not IPv4 AndAlso IPv6 Then
                Dim ptrV6Multi = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_MULTICAST_IPV6_ADDRESS)
                result = CType(API.BS2_GetIPConfigViaUDPEx(sdkContext, inputID, config, ptrV6Multi), BS2ErrorCode)
                Marshal.FreeHGlobal(ptrV6Multi)
            Else
                Console.WriteLine("Wrong selection")
                Return
            End If

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("GetIPConfigViaUDP??: Got error : {0}.", result)
                Return
            Else
                Me.print(config)
            End If

            Do
                Console.WriteLine("useDhcp ? [{0}]", If(config.useDHCP <> 0, "Y/n", "y/N"))
                Console.Write(">>>> ")
                Dim bInput As Boolean = If(config.useDHCP <> 0, Util.IsYes(), Not Util.IsNo())
                config.useDHCP = If(bInput, 1, 0)

                Console.WriteLine("useDns ? [{0}]", If(config.useDNS <> 0, "Y/n", "y/N"))
                Console.Write(">>>> ")
                bInput = If(config.useDNS <> 0, Util.IsYes(), Not Util.IsNo())
                config.useDNS = If(bInput, 1, 0)

                Dim strInput As String
                Dim bytesInput As Byte() = Nothing
                If config.useDHCP = 0 Then
                    Console.WriteLine("ipAddress ? [(Blank:{0})]", Encoding.UTF8.GetString(config.ipAddress))
                    Console.Write(">>>> ")
                    strInput = Console.ReadLine()
                    If strInput.Length = 0 Then
                        Console.WriteLine("   Do you want to keep the value? [Y(keep) / n(clear)")
                        Console.Write("   >>>> ")
                        If Not Util.IsYes() Then
                            Array.Clear(config.ipAddress, 0, config.ipAddress.Length)
                        End If
                    Else
                        Array.Clear(config.ipAddress, 0, config.ipAddress.Length)
                        bytesInput = Encoding.UTF8.GetBytes(strInput)
                        Array.Copy(bytesInput, 0, config.ipAddress, 0, Math.Min(bytesInput.Length, config.ipAddress.Length))
                    End If
                    If Encoding.UTF8.GetString(config.ipAddress).Length > 0 Then
                        Dim dummy As IPAddress
                        If IPAddress.TryParse(Encoding.UTF8.GetString(config.ipAddress).TrimEnd(ChrW(0)), dummy) = False Then
                            Console.WriteLine("Wrong ipAddress: {0})", Encoding.UTF8.GetString(config.ipAddress))
                            Return
                        End If
                    End If

                    Console.WriteLine("gateway ? [(Blank:{0})]", Encoding.UTF8.GetString(config.gateway))
                    Console.Write(">>>> ")
                    strInput = Console.ReadLine()
                    bytesInput = Nothing
                    If strInput.Length = 0 Then
                        Console.WriteLine("   Do you want to keep the value? [Y(keep) / n(clear)]")
                        Console.Write("   >>>> ")
                        If Not Util.IsYes() Then
                            Array.Clear(config.gateway, 0, config.gateway.Length)
                        End If
                    Else
                        Array.Clear(config.gateway, 0, config.gateway.Length)
                        bytesInput = Encoding.UTF8.GetBytes(strInput)
                        Array.Copy(bytesInput, 0, config.gateway, 0, Math.Min(bytesInput.Length, config.gateway.Length))
                    End If
                    If Encoding.UTF8.GetString(config.gateway).Length > 0 Then
                        Dim dummy As IPAddress
                        If IPAddress.TryParse(Encoding.UTF8.GetString(config.gateway).TrimEnd(ChrW(0)), dummy) = False Then
                            Console.WriteLine("Wrong gateway: {0})", Encoding.UTF8.GetString(config.gateway))
                            Return
                        End If
                    End If

                    Console.WriteLine("subnetMask ? [(Blank:{0})]", Encoding.UTF8.GetString(config.subnetMask))
                    Console.Write(">>>> ")
                    strInput = Console.ReadLine()
                    bytesInput = Nothing
                    If strInput.Length = 0 Then
                        Console.WriteLine("   Do you want to keep the value? [Y(keep) / n(clear)]")
                        Console.Write("   >>>> ")
                        If Not Util.IsYes() Then
                            Array.Clear(config.subnetMask, 0, config.subnetMask.Length)
                        End If
                    Else
                        Array.Clear(config.subnetMask, 0, config.subnetMask.Length)
                        bytesInput = Encoding.UTF8.GetBytes(strInput)
                        Array.Copy(bytesInput, 0, config.subnetMask, 0, Math.Min(bytesInput.Length, config.subnetMask.Length))
                    End If
                    If Encoding.UTF8.GetString(config.subnetMask).Length > 0 Then
                        Dim dummy As IPAddress
                        If IPAddress.TryParse(Encoding.UTF8.GetString(config.subnetMask).TrimEnd(ChrW(0)), dummy) = False Then
                            Console.WriteLine("Wrong subnetMask: {0})", Encoding.UTF8.GetString(config.subnetMask))
                            Return
                        End If
                    End If
                End If

                Console.WriteLine("port ? [1~65535 (Blank:{0})]", BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT)
                Console.Write(">>>> ")
                Dim nInput As Integer = Util.GetInput(BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT)
                config.port = nInput

                Console.WriteLine("Do you want to use server to device connection mode? [Y/n]")
                Console.Write(">>>> ")
                If Util.IsYes() Then
                    config.connectionMode = CByte(BS2ConnectionModeEnum.SERVER_TO_DEVICE)
                Else
                    config.connectionMode = CByte(BS2ConnectionModeEnum.DEVICE_TO_SERVER)
                End If

                If config.connectionMode = CByte(BS2ConnectionModeEnum.DEVICE_TO_SERVER) Then
                    Console.WriteLine("serverAddr ? [(Blank:{0})]", Encoding.UTF8.GetString(config.serverAddr))
                    Console.Write(">>>> ")
                    strInput = Console.ReadLine()
                    bytesInput = Nothing
                    If strInput.Length = 0 Then
                        Console.WriteLine("   Do you want to keep the value? [Y(keep) / n(clear)]")
                        Console.Write("   >>>> ")
                        If Not Util.IsYes() Then
                            Array.Clear(config.serverAddr, 0, config.serverAddr.Length)
                        End If
                    Else
                        Array.Clear(config.serverAddr, 0, config.serverAddr.Length)
                        bytesInput = Encoding.UTF8.GetBytes(strInput)
                        Array.Copy(bytesInput, 0, config.serverAddr, 0, Math.Min(bytesInput.Length, config.serverAddr.Length))
                    End If
                    If Encoding.UTF8.GetString(config.serverAddr).TrimEnd(CChar(ChrW(0))).Length > 0 Then
                        Dim dummy As IPAddress
                        If IPAddress.TryParse(Encoding.UTF8.GetString(config.serverAddr).TrimEnd(ChrW(0)), dummy) = False Then
                            Console.WriteLine("Wrong serverAddr: {0})", Encoding.UTF8.GetString(config.serverAddr))
                            Return
                        End If
                    End If

                    Console.WriteLine("serverPort ? [1~65535 (Blank:{0})]", BS2Environment.BS2_TCP_SERVER_PORT_DEFAULT)
                    Console.Write(">>>> ")
                    nInput = Util.GetInput(BS2Environment.BS2_TCP_SERVER_PORT_DEFAULT)
                    config.serverPort = nInput

                    Console.WriteLine("sslServerPort ? [1~65535 (Blank:{0})]", BS2Environment.BS2_TCP_SSL_SERVER_PORT_DEFAULT)
                    Console.Write(">>>> ")
                    nInput = Util.GetInput(BS2Environment.BS2_TCP_SSL_SERVER_PORT_DEFAULT)
                    config.sslServerPort = nInput
                End If
            Loop While False

            Console.WriteLine("Trying to set IPConfig via UDP")
            Dim ptrV4Broad2 = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_BROADCAST_IPV4_ADDRESS)
            Dim ptrV6Multi2 = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_MULTICAST_IPV6_ADDRESS)
            If IPv4 AndAlso IPv6 Then
                result = CType(API.BS2_SetIPConfigViaUDP(sdkContext, inputID, config), BS2ErrorCode)
            ElseIf IPv4 AndAlso Not IPv6 Then
                result = CType(API.BS2_SetIPConfigViaUDPEx(sdkContext, inputID, config, ptrV4Broad2), BS2ErrorCode)
            ElseIf Not IPv4 AndAlso IPv6 Then
                result = CType(API.BS2_SetIPConfigViaUDPEx(sdkContext, inputID, config, ptrV6Multi2), BS2ErrorCode)
            End If
            Marshal.FreeHGlobal(ptrV4Broad2)
            Marshal.FreeHGlobal(ptrV6Multi2)
            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("SetIPConfigViaUDP??: Got error({0}).", result)
            Else
                Console.WriteLine(">>>> Successfully set")
            End If
        End Sub

        Public Sub SetIPV6ConfigViaUDP()
            Console.WriteLine("What is the ID of the device for which you want to get IP config?")
            Console.Write(">>>> ")
            Dim inputID As UInteger = Util.GetInput(0)
            If inputID = 0 Then
                Console.WriteLine("Invalid Device ID")
                Return
            End If

            Dim IPv6 = True
            Dim IPv4 = True
            Console.WriteLine("Which mode do you want to use between IPv4 and IPv6? [0(IPv4), 1(IPv6), 2(Both: Default)]")
            Console.Write(">>>> ")
            Dim choiceIP As Integer = Util.GetInput(2)
            If choiceIP = 0 Then
                IPv4 = True
                IPv6 = False
            ElseIf choiceIP = 1 Then
                IPv4 = False
                IPv6 = True
            End If

            Dim config As BS2IPV6Config
            Console.WriteLine("Trying to get Current IPV6Config via UDP")
            Dim result As BS2ErrorCode = BS2ErrorCode.BS_SDK_SUCCESS
            If IPv4 AndAlso IPv6 Then
                result = CType(API.BS2_GetIPV6ConfigViaUDP(sdkContext, inputID, config), BS2ErrorCode)
            ElseIf IPv4 AndAlso Not IPv6 Then
                Dim ptrV4Broad = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_BROADCAST_IPV4_ADDRESS)
                result = CType(API.BS2_GetIPV6ConfigViaUDPEx(sdkContext, inputID, config, ptrV4Broad), BS2ErrorCode)
                Marshal.FreeHGlobal(ptrV4Broad)
            ElseIf Not IPv4 AndAlso IPv6 Then
                Dim ptrV6Multi = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_MULTICAST_IPV6_ADDRESS)
                result = CType(API.BS2_GetIPV6ConfigViaUDPEx(sdkContext, inputID, config, ptrV6Multi), BS2ErrorCode)
                Marshal.FreeHGlobal(ptrV6Multi)
            Else
                Console.WriteLine("Wrong selection")
                Return
            End If

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("GetIPV6ConfigViaUDP??: Got error({0}).", result)
                Return
            Else
                Me.print(config)
            End If

            Do
                Console.WriteLine("useDhcpV6 ? [{0}]", If(config.useDhcpV6 <> 0, "Y/n", "y/N"))
                Console.Write(">>>> ")
                Dim bInput As Boolean = If(config.useDhcpV6 <> 0, Util.IsYes(), Not Util.IsNo())
                config.useDhcpV6 = If(bInput, 1, 0)

                Console.WriteLine("useDnsV6 ? [{0}]", If(config.useDnsV6 <> 0, "Y/n", "y/N"))
                Console.Write(">>>> ")
                bInput = If(config.useDnsV6 <> 0, Util.IsYes(), Not Util.IsNo())
                config.useDnsV6 = If(bInput, 1, 0)

                Dim strInput As String
                Dim bytesInput As Byte() = Nothing
                If config.useDhcpV6 = 0 Then
                    Console.WriteLine("staticIpAddressV6 ? [(Blank:{0})]", Encoding.UTF8.GetString(config.staticIpAddressV6))
                    Console.Write(">>>> ")
                    strInput = Console.ReadLine()
                    If strInput.Length = 0 Then
                        Console.WriteLine("   Do you want to keep the value? [Y(keep) / N(clear), (Blank:Y)]")
                        Console.Write("   >>>> ")
                        If Not Util.IsYes() Then
                            Array.Clear(config.staticIpAddressV6, 0, config.staticIpAddressV6.Length)
                        End If
                    Else
                        Array.Clear(config.staticIpAddressV6, 0, config.staticIpAddressV6.Length)
                        bytesInput = Encoding.UTF8.GetBytes(strInput)
                        Array.Copy(bytesInput, 0, config.staticIpAddressV6, 0, Math.Min(bytesInput.Length, config.staticIpAddressV6.Length))
                    End If
                    If Encoding.UTF8.GetString(config.staticIpAddressV6).Length > 0 Then
                        Dim dummy As IPAddress
                        If IPAddress.TryParse(Encoding.UTF8.GetString(config.staticIpAddressV6).TrimEnd(ChrW(0)), dummy) = False Then
                            Console.WriteLine("Wrong staticIpAddressV6: {0})", Encoding.UTF8.GetString(config.staticIpAddressV6))
                            Return
                        End If
                    End If


                    Console.WriteLine("staticGatewayV6 ? [(Blank:{0})]", Encoding.UTF8.GetString(config.staticGatewayV6))
                    Console.Write(">>>> ")
                    strInput = Console.ReadLine()
                    bytesInput = Nothing
                    If strInput.Length = 0 Then
                        Console.WriteLine("   Do you want to keep the value? [Y(keep) / n(clear)]")
                        Console.Write("   >>>> ")
                        If Not Util.IsYes() Then
                            Array.Clear(config.staticGatewayV6, 0, config.staticGatewayV6.Length)
                        End If
                    Else
                        Array.Clear(config.staticGatewayV6, 0, config.staticGatewayV6.Length)
                        bytesInput = Encoding.UTF8.GetBytes(strInput)
                        Array.Copy(bytesInput, 0, config.staticGatewayV6, 0, Math.Min(bytesInput.Length, config.staticGatewayV6.Length))
                    End If
                    If Encoding.UTF8.GetString(config.staticGatewayV6).Length > 0 Then
                        Dim dummy As IPAddress
                        If IPAddress.TryParse(Encoding.UTF8.GetString(config.staticGatewayV6).TrimEnd(ChrW(0)), dummy) = False Then
                            Console.WriteLine("Wrong staticGatewayV6: {0})", Encoding.UTF8.GetString(config.staticGatewayV6))
                            Return
                        End If
                    End If
                End If

                If config.useDnsV6 = 1 Then
                    Console.WriteLine("dnsAddrV6 ? [(Blank:{0})]", Encoding.UTF8.GetString(config.dnsAddrV6))
                    Console.Write(">>>> ")
                    strInput = Console.ReadLine()
                    bytesInput = Nothing
                    If strInput.Length = 0 Then
                        Console.WriteLine("   Do you want to keep the value? [Y(keep) / n(clear)]")
                        Console.Write("   >>>> ")
                        If Not Util.IsYes() Then
                            Array.Clear(config.dnsAddrV6, 0, config.dnsAddrV6.Length)
                        End If
                    Else
                        Array.Clear(config.dnsAddrV6, 0, config.dnsAddrV6.Length)
                        bytesInput = Encoding.UTF8.GetBytes(strInput)
                        Array.Copy(bytesInput, 0, config.dnsAddrV6, 0, Math.Min(bytesInput.Length, config.dnsAddrV6.Length))
                    End If
                    If Encoding.UTF8.GetString(config.dnsAddrV6).Length > 0 Then
                        Dim dummy As IPAddress
                        If IPAddress.TryParse(Encoding.UTF8.GetString(config.dnsAddrV6).TrimEnd(ChrW(0)), dummy) = False Then
                            Console.WriteLine("Wrong dnsAddrV6: {0})", Encoding.UTF8.GetString(config.dnsAddrV6))
                            Return
                        End If
                    End If
                End If

                Console.WriteLine("serverIpAddressV6 ? [(Blank:{0})]", Encoding.UTF8.GetString(config.serverIpAddressV6))
                Console.Write(">>>> ")
                strInput = Console.ReadLine()
                bytesInput = Nothing
                If strInput.Length = 0 Then
                    Console.WriteLine("   Do you want to keep the value? [Y(keep) / n(clear)]")
                    Console.Write("   >>>> ")
                    If Not Util.IsYes() Then
                        Array.Clear(config.serverIpAddressV6, 0, config.serverIpAddressV6.Length)
                    End If
                Else
                    Array.Clear(config.serverIpAddressV6, 0, config.serverIpAddressV6.Length)
                    bytesInput = Encoding.UTF8.GetBytes(strInput)
                    Array.Copy(bytesInput, 0, config.serverIpAddressV6, 0, Math.Min(bytesInput.Length, config.serverIpAddressV6.Length))
                End If
                If Encoding.UTF8.GetString(config.serverIpAddressV6).TrimEnd(CChar(ChrW(0))).Length > 0 Then
                    Dim dummy As IPAddress
                    If IPAddress.TryParse(Encoding.UTF8.GetString(config.serverIpAddressV6), dummy) = False Then
                        Console.WriteLine("Wrong serverIpAddressV6: {0})", Encoding.UTF8.GetString(config.serverIpAddressV6))
                        Return
                    End If
                End If

                Console.WriteLine("serverPortV6 ? [1~65535 (Blank:{0})]", BS2Environment.BS2_TCP_SERVER_PORT_DEFAULT_V6)
                Console.Write(">>>> ")
                Dim nInput As Integer = Util.GetInput(BS2Environment.BS2_TCP_SERVER_PORT_DEFAULT_V6)
                config.serverPortV6 = nInput

                Console.WriteLine("sslServerPortV6 ? [1~65535 (Blank:{0})]", BS2Environment.BS2_TCP_SSL_SERVER_PORT_DEFAULT_V6)
                Console.Write(">>>> ")
                nInput = Util.GetInput(BS2Environment.BS2_TCP_SSL_SERVER_PORT_DEFAULT_V6)
                config.sslServerPortV6 = nInput

                Console.WriteLine("portV6 ? [1~65535 (Blank:{0})]", BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT_V6)
                Console.Write(">>>> ")
                nInput = Util.GetInput(BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT_V6)
                config.portV6 = nInput

                config.numOfAllocatedAddressV6 = 0
                config.numOfAllocatedGatewayV6 = 0

            Loop While False

            Console.WriteLine("Trying to set IPV6Config via UDP")
            Dim ptrV4Broad2 = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_BROADCAST_IPV4_ADDRESS)
            Dim ptrV6Multi2 = Marshal.StringToHGlobalAnsi(BS2Environment.DEFAULT_MULTICAST_IPV6_ADDRESS)
            If IPv4 AndAlso IPv6 Then
                result = CType(API.BS2_SetIPV6ConfigViaUDP(sdkContext, inputID, config), BS2ErrorCode)
            ElseIf IPv4 AndAlso Not IPv6 Then
                result = CType(API.BS2_SetIPV6ConfigViaUDPEx(sdkContext, inputID, config, ptrV4Broad2), BS2ErrorCode)
            ElseIf Not IPv4 AndAlso IPv6 Then
                result = CType(API.BS2_SetIPV6ConfigViaUDPEx(sdkContext, inputID, config, ptrV6Multi2), BS2ErrorCode)
            End If

            Marshal.FreeHGlobal(ptrV4Broad2)
            Marshal.FreeHGlobal(ptrV6Multi2)

            If result <> BS2ErrorCode.BS_SDK_SUCCESS Then
                Console.WriteLine("SetIPV6ConfigViaUDP??: Got error({0}).", result)
            Else
                Console.WriteLine(">>>> Successfully set")
            End If
        End Sub
    End Class
End Namespace
