Imports System
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports BS2_SCHEDULE_ID = System.UInt32
Imports BS2_CONFIG_MASK = System.UInt32
Imports BS2_USER_MASK = System.UInt32

Namespace Suprema

    Friend Module API
        Public productNameDictionary As Dictionary(Of BS2DeviceTypeEnum, String) = New Dictionary(Of BS2DeviceTypeEnum, String)() From {
    {BS2DeviceTypeEnum.UNKNOWN, "Unknown Device"},
    {BS2DeviceTypeEnum.BIOENTRY_PLUS, "BioEntry Plus"},
    {BS2DeviceTypeEnum.BIOENTRY_W, "BioEntry W"},
    {BS2DeviceTypeEnum.BIOLITE_NET, "BioLite Net"},
    {BS2DeviceTypeEnum.XPASS, "Xpass"},
    {BS2DeviceTypeEnum.XPASS_S2, "Xpass S2"},
    {BS2DeviceTypeEnum.SECURE_IO_2, "Secure IO 2"},
    {BS2DeviceTypeEnum.DOOR_MODULE_20, "Door module 20"},
    {BS2DeviceTypeEnum.BIOSTATION_2, "BioStation 2"},
    {BS2DeviceTypeEnum.BIOSTATION_A2, "BioStation A2"},
    {BS2DeviceTypeEnum.FACESTATION_2, "FaceStation 2"},
    {BS2DeviceTypeEnum.IO_DEVICE, "IO device"},
    {BS2DeviceTypeEnum.BIOSTATION_L2, "BioStation L2"},
    {BS2DeviceTypeEnum.BIOENTRY_W2, "BioEntry W2"},
    {BS2DeviceTypeEnum.CORESTATION_40, "CoreStation40"},
    {BS2DeviceTypeEnum.OUTPUT_MODULE, "Output Module"},
    {BS2DeviceTypeEnum.INPUT_MODULE, "Inout Module"},
    {BS2DeviceTypeEnum.BIOENTRY_P2, "BioEntry P2"},
    {BS2DeviceTypeEnum.BIOLITE_N2, "BioLite N2"},
    {BS2DeviceTypeEnum.XPASS2, "XPass 2"},
    {BS2DeviceTypeEnum.XPASS_S3, "XPass S3"},
    {BS2DeviceTypeEnum.BIOENTRY_R2, "BioEntry R2"},
    {BS2DeviceTypeEnum.XPASS_D2, "XPass D2"},
    {BS2DeviceTypeEnum.DOOR_MODULE_21, "DoorModule 21"},
    {BS2DeviceTypeEnum.XPASS_D2_KEYPAD, "XPass D2 Keypad"},
    {BS2DeviceTypeEnum.FACELITE, "FaceLite"},
    {BS2DeviceTypeEnum.XPASS2_KEYPAD, "XPass 2 Keypad"},
    {BS2DeviceTypeEnum.XPASS_D2_REV, "XPass D2 Rev"},
    {BS2DeviceTypeEnum.XPASS_D2_KEYPAD_REV, "XPass D2 Keypad Rev"},
    {BS2DeviceTypeEnum.FACESTATION_F2_FP, "FaceStation F2 FP"},     ' FSF2 support
    {BS2DeviceTypeEnum.FACESTATION_F2, "FaceStation F2"},          ' FSF2 support
    {BS2DeviceTypeEnum.XSTATION_2_QR, "X-Station 2 QR"},
    {BS2DeviceTypeEnum.XSTATION_2, "X-Station 2"},
    {BS2DeviceTypeEnum.IM_120, "Input Module 120"},
    {BS2DeviceTypeEnum.XSTATION_2_FP, "X-Station 2 FP"},
    {BS2DeviceTypeEnum.BIOSTATION_3, "BioStation 3"},
    {BS2DeviceTypeEnum.THIRD_OSDP_DEVICE, "3rd party OSDP"},
    {BS2DeviceTypeEnum.THIRD_OSDP_IO_DEVICE, "3rd party OSDP IO"},
    {BS2DeviceTypeEnum.BIOSTATION_2A, "BioStation 2A"},
    {BS2DeviceTypeEnum.BIOENTRY_W3, "BioEntry W3"}
}

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnDeviceFound(deviceId As BS2_SCHEDULE_ID)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnDeviceAccepted(deviceId As BS2_SCHEDULE_ID)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnDeviceConnected(deviceId As BS2_SCHEDULE_ID)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnDeviceDisconnected(deviceId As BS2_SCHEDULE_ID)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnReadyToScan(deviceId As BS2_SCHEDULE_ID, sequence As BS2_SCHEDULE_ID)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnProgressChanged(deviceId As BS2_SCHEDULE_ID, progressPercentage As BS2_SCHEDULE_ID)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnLogReceived(deviceId As BS2_SCHEDULE_ID, log As IntPtr)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnLogReceivedEx(deviceId As BS2_SCHEDULE_ID, log As IntPtr, temperature As BS2_SCHEDULE_ID)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnAlarmFired(deviceId As BS2_SCHEDULE_ID, log As IntPtr)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnInputDetected(deviceId As BS2_SCHEDULE_ID, log As IntPtr)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnConfigChanged(deviceId As BS2_SCHEDULE_ID, configMask As BS2_SCHEDULE_ID)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnBarcodeScanned(deviceId As BS2_SCHEDULE_ID, barcode As String)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnVerifyUser(deviceId As BS2_SCHEDULE_ID, seq As UShort, isCard As Byte, cardType As Byte, data As IntPtr, dataLen As BS2_SCHEDULE_ID)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnIdentifyUser(deviceId As BS2_SCHEDULE_ID, seq As UShort, format As Byte, templateData As IntPtr, templateSize As BS2_SCHEDULE_ID)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnUserPhrase(deviceId As BS2_SCHEDULE_ID, seq As UShort, userID As String)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnOsdpStandardDeviceStatusChanged(deviceId As BS2_SCHEDULE_ID, notifyData As IntPtr)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function IsAcceptableUserID(uid As String) As Integer

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function PreferMethod(deviceID As BS2_SCHEDULE_ID) As BS2_SCHEDULE_ID

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function GetRootCaFilePath(deviceID As BS2_SCHEDULE_ID) As IntPtr

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function GetServerCaFilePath(deviceID As BS2_SCHEDULE_ID) As IntPtr

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function GetServerPrivateKeyFilePath(deviceID As BS2_SCHEDULE_ID) As IntPtr

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function GetPassword(deviceID As BS2_SCHEDULE_ID) As IntPtr

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnErrorOccured(deviceID As BS2_SCHEDULE_ID, errCode As Integer)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnSendRootCA(result As Integer)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnCheckGlobalAPBViolation(deviceId As BS2_SCHEDULE_ID, seq As UShort, userID_1 As String, userID_2 As String, isDualAuth As Boolean)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnCheckGlobalAPBViolationByDoorOpen(deviceId As BS2_SCHEDULE_ID, seq As UShort, userID_1 As String, userID_2 As String, isDualAuth As Boolean)

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub OnUpdateGlobalAPBViolationByDoorOpen(deviceId As BS2_SCHEDULE_ID, seq As UShort, userID_1 As String, userID_2 As String, isDualAuth As Boolean)

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_AllocateContext() As IntPtr
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_Initialize(context As IntPtr) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_InitializeEx(context As IntPtr, hostipAddr As IntPtr) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDeviceSearchingTimeout(context As IntPtr, second As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetMaxThreadCount(context As IntPtr, maxThreadCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_IsAutoConnection(context As IntPtr, ByRef enable As Integer) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetAutoConnection(context As IntPtr, enable As Integer) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDeviceEventListener(context As IntPtr, cbOnDeviceFound As OnDeviceFound, cbOnDeviceAccepted As OnDeviceAccepted, cbOnDeviceConnected As OnDeviceConnected, cbOnDeviceDisconnected As OnDeviceDisconnected) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetNotificationListener(context As IntPtr, cbOnAlarmFired As OnAlarmFired, cbOnInputDetected As OnInputDetected, cbOnConfigChanged As OnConfigChanged) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetBarcodeScanListener(context As IntPtr, cbOnBarcodeScanned As OnBarcodeScanned) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Sub BS2_ReleaseContext(context As IntPtr)
        End Sub

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Sub BS2_ReleaseObject(obj As IntPtr)
        End Sub

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetSSLHandler(context As IntPtr, cbPreferMethod As PreferMethod, cbGetRootCaFilePath As GetRootCaFilePath, cbGetServerCaFilePath As GetServerCaFilePath, cbGetServerPrivateKeyFilePath As GetServerPrivateKeyFilePath, cbGetPassword As GetPassword, cbOnErrorOccured As OnErrorOccured) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CharSet:=CharSet.Ansi, CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetServerPort(context As IntPtr, serverPort As UShort) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SearchDevices(context As IntPtr) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SearchDevicesEx(context As IntPtr, hostipAddr As IntPtr) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDevices(context As IntPtr, <Out> ByRef deviceListObj As IntPtr, <Out> ByRef numDevice As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDeviceInfo(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef deviceInfo As BS2SimpleDeviceInfo) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDeviceInfoEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef deviceInfo As BS2SimpleDeviceInfo, <Out> ByRef deviceInfoEx As BS2SimpleDeviceInfoEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ConnectDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CharSet:=CharSet.Ansi, CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ConnectDeviceViaIP(context As IntPtr, deviceAddress As IntPtr, devicePort As UShort, <Out> ByRef deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_DisconnectDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDeviceTopology(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef networkNodeObj As IntPtr, <Out> ByRef numNetworkNode As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDeviceTopology(context As IntPtr, deviceId As BS2_SCHEDULE_ID, networkNode As IntPtr, numNetworkNode As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< AccessControl API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAccessGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessGroupIds As IntPtr, accessGroupIdCount As BS2_SCHEDULE_ID, <Out> ByRef accessGroupObj As IntPtr, <Out> ByRef numAccessGroup As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllAccessGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef accessGroupObj As IntPtr, <Out> ByRef numAccessGroup As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetAccessGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessGroups As IntPtr, accessGroupCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAccessGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessGroupIds As IntPtr, accessGroupIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllAccessGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAccessLevel(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessLevelIds As IntPtr, accessLevelIdCount As BS2_SCHEDULE_ID, <Out> ByRef accessLevelObj As IntPtr, <Out> ByRef numAccessLevel As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllAccessLevel(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef accessLevelObj As IntPtr, <Out> ByRef numAccessLevel As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetAccessLevel(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessLevels As IntPtr, accessLevelCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAccessLevel(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessLevelIds As IntPtr, accessLevelIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllAccessLevel(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAccessSchedule(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessScheduleIds As IntPtr, accessScheduleIdCount As BS2_SCHEDULE_ID, <Out> ByRef accessScheduleObj As IntPtr, <Out> ByRef numAccessSchedule As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllAccessSchedule(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef accessScheduleObj As IntPtr, <Out> ByRef numAccessSchedule As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetAccessSchedule(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessSchedules As IntPtr, accessScheduleCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAccessSchedule(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessScheduleIds As IntPtr, accessScheduleIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllAccessSchedule(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetHolidayGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, holidayGroupIds As IntPtr, holidayGroupIdCount As BS2_SCHEDULE_ID, <Out> ByRef holidayGroupObj As IntPtr, <Out> ByRef numHolidayGroup As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllHolidayGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef holidayGroupObj As IntPtr, <Out> ByRef numHolidayGroup As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetHolidayGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, holidayGroups As IntPtr, holidayGroupCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveHolidayGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, holidayGroupIds As IntPtr, holidayGroupIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllHolidayGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Blacklist API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetBlackList(context As IntPtr, deviceId As BS2_SCHEDULE_ID, blacklists As IntPtr, blacklistCount As BS2_SCHEDULE_ID, <Out> ByRef blacklistObj As IntPtr, <Out> ByRef numBlacklist As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllBlackList(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef blacklistObj As IntPtr, <Out> ByRef numBlacklist As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetBlackList(context As IntPtr, deviceId As BS2_SCHEDULE_ID, blacklists As IntPtr, blacklistCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveBlackList(context As IntPtr, deviceId As BS2_SCHEDULE_ID, blacklists As IntPtr, blacklistCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllBlackList(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Card API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ScanCard(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef card As BS2Card, cbReadyToScan As OnReadyToScan) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_WriteCard(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef smartCard As BS2SmartCardData) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_EraseCard(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_WriteQRCode(qrText As IntPtr, ByRef card As BS2CSNCard) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Config API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ClearDatabase(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ResetConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <MarshalAs(UnmanagedType.I1)> includingDB As Boolean) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ResetConfigExceptNetInfo(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <MarshalAs(UnmanagedType.I1)> includingDB As Boolean) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef configs As BS2Configs) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef configs As BS2Configs) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetFactoryConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2FactoryConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetSystemConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2SystemConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetSystemConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2SystemConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAuthConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2AuthConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetAuthConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2AuthConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAuthConfigExt(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2AuthConfigExt) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetAuthConfigExt(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2AuthConfigExt) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetFaceConfigExt(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2FaceConfigExt) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetFaceConfigExt(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2FaceConfigExt) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetThermalCameraConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2ThermalCameraConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetThermalCameraConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2ThermalCameraConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetBarcodeConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2BarcodeConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetBarcodeConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2BarcodeConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetInputConfigEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2InputConfigEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetInputConfigEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2InputConfigEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetRelayActionConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2RelayActionConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetRelayActionConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2RelayActionConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetStatusConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2StatusConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetStatusConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2StatusConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDisplayConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2DisplayConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDisplayConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2DisplayConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetIPConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2IpConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetIPConfigExt(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2IpConfigExt) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetIPConfigViaUDP(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2IpConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetIPConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2IpConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetIPConfigExt(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2IpConfigExt) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetIPConfigViaUDP(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2IpConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetTNAConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2TNAConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetTNAConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2TNAConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetCardConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2CardConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetCardConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2CardConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetFingerprintConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2FingerprintConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetFingerprintConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2FingerprintConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetRS485Config(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2Rs485Config) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetRS485Config(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2Rs485Config) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetWiegandConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2WiegandConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetWiegandConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2WiegandConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetWiegandDeviceConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2WiegandDeviceConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetWiegandDeviceConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2WiegandDeviceConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetInputConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2InputConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetInputConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2InputConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetWlanConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2WlanConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetWlanConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2WlanConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetTriggerActionConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2TriggerActionConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetTriggerActionConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2TriggerActionConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetEventConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2EventConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetEventConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2EventConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetWiegandMultiConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2WiegandMultiConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetWiegandMultiConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2WiegandMultiConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetCard1xConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS1CardConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetCard1xConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS1CardConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetSystemExtConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2SystemConfigExt) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetSystemExtConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2SystemConfigExt) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetVoipConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2VoipConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetVoipConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2VoipConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetFaceConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2FaceConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetFaceConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2FaceConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetCardConfigEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2CardConfigEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetCardConfigEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2CardConfigEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetCustomCardConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2CustomCardConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetCustomCardConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2CustomCardConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetRS485ConfigEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2Rs485ConfigEX) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetRS485ConfigEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2Rs485ConfigEX) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDstConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2DstConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDstConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2DstConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDesFireCardConfigEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2DesFireCardConfigEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDesFireCardConfigEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2DesFireCardConfigEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetVoipConfigExt(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2VoipConfigExt) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetVoipConfigExt(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2VoipConfigExt) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetRtspConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2RtspConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetRtspConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2RtspConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetOsdpStandardConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2OsdpStandardConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetOsdpStandardConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2OsdpStandardConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetOsdpStandardActionConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2OsdpStandardActionConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetOsdpStandardActionConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2OsdpStandardActionConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLicenseConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2LicenseConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_AddOsdpStandardDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID, channelIndex As BS2_SCHEDULE_ID, ByRef osdpDevice As BS2OsdpStandardDeviceAdd, <Out> ByRef osdpChannelIndex As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetOsdpStandardDevice(context As IntPtr, osdpDeviceId As BS2_SCHEDULE_ID, <Out> ByRef osdpDevice As BS2OsdpStandardDevice) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_UpdateOsdpStandardDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID, osdpDevices As IntPtr, numOfDevice As BS2_SCHEDULE_ID, <Out> ByRef outResultObj As IntPtr, <Out> ByRef outNumOfResult As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveOsdpStandardDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID, osdpDeviceIds As IntPtr, numOfDevice As BS2_SCHEDULE_ID, <Out> ByRef outResultObj As IntPtr, <Out> ByRef outNumOfResult As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetOsdpStandardDeviceCapability(context As IntPtr, osdpDeviceId As BS2_SCHEDULE_ID, <Out> ByRef capability As BS2OsdpStandardDeviceCapability) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetOsdpStandardDeviceSecurityKey(context As IntPtr, masterOrSlaveId As BS2_SCHEDULE_ID, key As IntPtr) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetOsdpStandardDeviceStatusListener(context As IntPtr, ptrOsdpStandardDeviceStatus As OnOsdpStandardDeviceStatusChanged) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetOsdpStandardAvailableID(context As IntPtr, deviceId As BS2_SCHEDULE_ID, channelIndex As BS2_SCHEDULE_ID, <Out> ByRef osdpDeviceID As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAvailableOsdpStandardDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef osdpDevices As BS2OsdpStandardDeviceAvailable) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_EnableDeviceLicense(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef licenseBlob As BS2LicenseBlob, <Out> ByRef licenseResultObj As IntPtr, <Out> ByRef outNumOfResult As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_DisableDeviceLicense(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef licenseBlob As BS2LicenseBlob, <Out> ByRef licenseResultObj As IntPtr, <Out> ByRef outNumOfResult As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_QueryDeviceLicense(context As IntPtr, deviceId As BS2_SCHEDULE_ID, licenseType As UShort, <Out> ByRef licenseResultObj As IntPtr, <Out> ByRef outNumOfResult As BS2_SCHEDULE_ID) As Integer
        End Function
        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Door API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDoor(context As IntPtr, deviceId As BS2_SCHEDULE_ID, doorIds As IntPtr, doorIdCount As BS2_SCHEDULE_ID, <Out> ByRef doorObj As IntPtr, <Out> ByRef numDoor As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllDoor(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef doorObj As IntPtr, <Out> ByRef numDoor As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDoorStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, doorIds As IntPtr, doorIdCount As BS2_SCHEDULE_ID, <Out> ByRef doorStatusObj As IntPtr, <Out> ByRef numDoorStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllDoorStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef doorStatusObj As IntPtr, <Out> ByRef numDoorStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDoor(context As IntPtr, deviceId As BS2_SCHEDULE_ID, doors As IntPtr, doorCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDoorAlarm(context As IntPtr, deviceId As BS2_SCHEDULE_ID, flag As Byte, doorIds As IntPtr, doorIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveDoor(context As IntPtr, deviceId As BS2_SCHEDULE_ID, doors As IntPtr, doorCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllDoor(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ReleaseDoor(context As IntPtr, deviceId As BS2_SCHEDULE_ID, flag As Byte, doorIds As IntPtr, doorIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_LockDoor(context As IntPtr, deviceId As BS2_SCHEDULE_ID, flag As Byte, doorIds As IntPtr, doorIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_UnlockDoor(context As IntPtr, deviceId As BS2_SCHEDULE_ID, flag As Byte, doorIds As IntPtr, doorIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Fingerprint API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLastFingerprintImage(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef imageObj As IntPtr, <Out> ByRef imageWidth As BS2_SCHEDULE_ID, <Out> ByRef imageHeight As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ScanFingerprint(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef finger As BS2Fingerprint, templateIndex As BS2_SCHEDULE_ID, quality As BS2_SCHEDULE_ID, templateFormat As Byte, cbReadyToScan As OnReadyToScan) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ScanFingerprintEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef finger As BS2Fingerprint, templateIndex As BS2_SCHEDULE_ID, quality As BS2_SCHEDULE_ID, templateFormat As Byte, <Out> ByRef outquality As BS2_SCHEDULE_ID, cbReadyToScan As OnReadyToScan) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_VerifyFingerprint(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef finger As BS2Fingerprint) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetFingerTemplateQuality(templateBuffer As IntPtr, templateSize As BS2_SCHEDULE_ID, <Out> ByRef score As Integer) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Log API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLog(context As IntPtr, deviceId As BS2_SCHEDULE_ID, eventId As BS2_SCHEDULE_ID, amount As BS2_SCHEDULE_ID, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetImageLog(context As IntPtr, deviceId As BS2_SCHEDULE_ID, eventId As BS2_SCHEDULE_ID, <Out> ByRef imageObj As IntPtr, <Out> ByRef imageSize As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ClearLog(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_StartMonitoringLog(context As IntPtr, deviceId As BS2_SCHEDULE_ID, cbOnLogReceived As OnLogReceived) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_StartMonitoringLogEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, cbOnLogReceivedEx As OnLogReceivedEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_StopMonitoringLog(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLogBlob(context As IntPtr, deviceId As BS2_SCHEDULE_ID, eventMask As UShort, eventId As BS2_SCHEDULE_ID, amount As BS2_SCHEDULE_ID, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLogSmallBlobEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, eventMask As UShort, eventId As BS2_SCHEDULE_ID, amount As BS2_SCHEDULE_ID, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< MISC API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_FactoryReset(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RebootDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_LockDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_UnlockDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDeviceTime(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef gmtTime As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDeviceTime(context As IntPtr, deviceId As BS2_SCHEDULE_ID, gmtTime As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_UpgradeFirmware(context As IntPtr, deviceId As BS2_SCHEDULE_ID, firmwareData As IntPtr, firmwareDataLen As BS2_SCHEDULE_ID, keepVerifyingSlaveDevice As Byte, cbProgressChanged As OnProgressChanged) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_UpdateResource(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef resourceElement As BS2ResourceElement, keepVerifyingSlaveDevice As Byte, cbProgressChanged As OnProgressChanged) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDeviceCapabilities(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef info As BS2DeviceCapabilities) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RunAction(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef action As BS2Action) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Sub BS2_SetKeepAliveTimeout(context As IntPtr, ms As Long)
        End Sub

        <DllImport("BS_SDK_V2.dll", CharSet:=CharSet.Ansi, CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_MakePinCode(context As IntPtr, salt As IntPtr,
        <[In], Out> pinCode As IntPtr) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CharSet:=CharSet.Ansi, CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_MakePinCodeWithKey(context As IntPtr, salt As IntPtr,
        <[In], Out> pinCode As IntPtr, ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CharSet:=CharSet.Ansi, CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ComputeCRC16CCITT(data As IntPtr, dataLen As BS2_SCHEDULE_ID, ByRef crc As UShort) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CharSet:=CharSet.Ansi, CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetCardModel(modelName As IntPtr, <Out> ByRef cardModel As UShort) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CharSet:=CharSet.Ansi, CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_Version() As IntPtr
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDataEncryptKey(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDataEncryptKey(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveDataEncryptKey(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Slave Control API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetSlaveDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef slaveDeviceObj As IntPtr, <Out> ByRef slaveDeviceCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetSlaveDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID, slaveDeviceObj As IntPtr, slaveDeviceCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetSlaveBaudrate(context As IntPtr, deviceId As BS2_SCHEDULE_ID, slaveId As BS2_SCHEDULE_ID, baudrate As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Server Matching API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetServerMatchingHandler(context As IntPtr, cbOnVerifyUser As OnVerifyUser, cbOnIdentifyUser As OnIdentifyUser) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_VerifyUser(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, ByRef userBlob As BS2UserBlob) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_IdentifyUser(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, ByRef userBlob As BS2UserBlob) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_VerifyUserEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, ByRef userBlob As BS2UserBlobEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_IdentifyUserEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, ByRef userBlob As BS2UserBlobEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetUserPhraseHandler(context As IntPtr, cbOnQuery As OnUserPhrase) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ResponseUserPhrase(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, userPhrase As IntPtr) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< User API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatabaseInfo(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef numUsers As BS2_SCHEDULE_ID, <Out> ByRef numCards As BS2_SCHEDULE_ID, <Out> ByRef numFingers As BS2_SCHEDULE_ID, <Out> ByRef numFaces As BS2_SCHEDULE_ID, cbIsAcceptableUserID As IsAcceptableUserID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserList(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef outUidObjs As IntPtr, <Out> ByRef outNumUids As BS2_SCHEDULE_ID, cbIsAcceptableUserID As IsAcceptableUserID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserInfos(context As IntPtr, deviceId As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlob()) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatas(context As IntPtr, deviceId As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlob(), userMask As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_EnrolUser(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlob(), uidCount As BS2_SCHEDULE_ID, overwrite As Byte) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_EnrollUser(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlob(), uidCount As BS2_SCHEDULE_ID, overwrite As Byte) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveUser(context As IntPtr, deviceId As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllUser(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserInfosEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlobEx()) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatasEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlobEx(), userMask As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_EnrolUserEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlobEx(), uidCount As BS2_SCHEDULE_ID, overwrite As Byte) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_EnrollUserEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlobEx(), uidCount As BS2_SCHEDULE_ID, overwrite As Byte) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Wiegand Control API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SearchWiegandDevices(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef wiegandDeviceObj As IntPtr, <Out> ByRef numWiegandDevice As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetWiegandDevices(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef wiegandDeviceObj As IntPtr, <Out> ByRef numWiegandDevice As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_AddWiegandDevices(context As IntPtr, deviceId As BS2_SCHEDULE_ID, wiegandDevice As IntPtr, numWiegandDevice As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveWiegandDevices(context As IntPtr, deviceId As BS2_SCHEDULE_ID, wiegandDevice As IntPtr, numWiegandDevice As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Zone Control API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAntiPassbackZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneObj As IntPtr, <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllAntiPassbackZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneObj As IntPtr, <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAntiPassbackZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllAntiPassbackZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetAntiPassbackZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zones As IntPtr, zoneCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetAntiPassbackZoneAlarm(context As IntPtr, deviceId As BS2_SCHEDULE_ID, alarmed As Byte, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAntiPassbackZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllAntiPassbackZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ClearAntiPassbackZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneID As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ClearAllAntiPassbackZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneID As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetCheckGlobalAPBViolationHandler(context As IntPtr, ptrCheckGlobalAPBViolation As OnCheckGlobalAPBViolation) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_CheckGlobalAPBViolation(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, zoneID As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetGlobalAPBViolationByDoorOpenHandler(context As IntPtr, ptrCheck As OnCheckGlobalAPBViolationByDoorOpen, ptrUpdate As OnUpdateGlobalAPBViolationByDoorOpen) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_CheckGlobalAPBViolationByDoorOpen(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, zoneID As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetTimedAntiPassbackZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneObj As IntPtr, <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllTimedAntiPassbackZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneObj As IntPtr, <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetTimedAntiPassbackZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllTimedAntiPassbackZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetTimedAntiPassbackZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zones As IntPtr, zoneCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetTimedAntiPassbackZoneAlarm(context As IntPtr, deviceId As BS2_SCHEDULE_ID, alarmed As Byte, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveTimedAntiPassbackZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllTimedAntiPassbackZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ClearTimedAntiPassbackZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneID As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ClearAllTimedAntiPassbackZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneID As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetFireAlarmZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneObj As IntPtr, <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllFireAlarmZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneObj As IntPtr, <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetFireAlarmZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllFireAlarmZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetFireAlarmZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zones As IntPtr, zoneCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetFireAlarmZoneAlarm(context As IntPtr, deviceId As BS2_SCHEDULE_ID, alarmed As Byte, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveFireAlarmZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllFireAlarmZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetScheduledLockUnlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneObj As IntPtr, <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllScheduledLockUnlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneObj As IntPtr, <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetScheduledLockUnlockZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllScheduledLockUnlockZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetScheduledLockUnlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zones As IntPtr, zoneCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetScheduledLockUnlockZoneAlarm(context As IntPtr, deviceId As BS2_SCHEDULE_ID, alarmed As Byte, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveScheduledLockUnlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllScheduledLockUnlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLiftLockUnlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneObj As IntPtr, <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllLiftLockUnlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneObj As IntPtr, <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLiftLockUnlockZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllLiftLockUnlockZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetLiftLockUnlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zones As IntPtr, zoneCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetLiftLockUnlockZoneAlarm(context As IntPtr, deviceId As BS2_SCHEDULE_ID, alarmed As Byte, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveLiftLockUnlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllLiftLockUnlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Face API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ScanFace(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> face As BS2Face(), enrollmentThreshold As Byte, cbReadyToScan As OnReadyToScan) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ScanFaceEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> face As BS2FaceExWarped(), enrollmentThreshold As Byte, cbReadyToScan As OnReadyToScan) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ExtractTemplateFaceEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, imageData As IntPtr, imageDataLen As BS2_SCHEDULE_ID, isWarped As Integer, <Out> ByRef templateEx As BS2TemplateEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetNormalizedImageFaceEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, unwarpedImage As IntPtr, unwarpedImageLen As BS2_SCHEDULE_ID, warpedImage As IntPtr, <Out> ByRef warpedImageLen As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Lift API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLift(context As IntPtr, deviceId As BS2_SCHEDULE_ID, LiftIds As IntPtr, LiftIdCount As BS2_SCHEDULE_ID, <Out> ByRef LiftObj As IntPtr, <Out> ByRef numLift As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllLift(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef LiftObj As IntPtr, <Out> ByRef numLift As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLiftStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, LiftIds As IntPtr, LiftIdCount As BS2_SCHEDULE_ID, <Out> ByRef LiftObj As IntPtr, <Out> ByRef numLift As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllLiftStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef LiftObj As IntPtr, <Out> ByRef numLift As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetLift(context As IntPtr, deviceId As BS2_SCHEDULE_ID, Lifts As IntPtr, LiftCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetLiftAlarm(context As IntPtr, deviceId As BS2_SCHEDULE_ID, alarmFlag As Byte, Lifts As IntPtr, LiftCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveLift(context As IntPtr, deviceId As BS2_SCHEDULE_ID, LiftIds As IntPtr, LiftIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllLift(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ReleaseFloor(context As IntPtr, deviceId As BS2_SCHEDULE_ID, floorFlag As Byte, liftID As BS2_SCHEDULE_ID, FloorIndexs As IntPtr, floorIndexCount As Byte) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ActivateFloor(context As IntPtr, deviceId As BS2_SCHEDULE_ID, floorFlag As Byte, liftID As BS2_SCHEDULE_ID, FloorIndexs As IntPtr, floorIndexCount As Byte) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_DeActivateFloor(context As IntPtr, deviceId As BS2_SCHEDULE_ID, floorFlag As Byte, liftID As BS2_SCHEDULE_ID, FloorIndexs As IntPtr, floorIndexCount As Byte) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< SSL API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CharSet:=CharSet.Ansi, CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetSSLServerPort(context As IntPtr, sslServerPort As UShort) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDeviceSSLEventListener(context As IntPtr, cbOnSendRootCA As OnSendRootCA) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_DisableSSL(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< AuthGroup API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAuthGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, authGroupIds As IntPtr, authGroupIdCount As BS2_SCHEDULE_ID, <Out> ByRef authGroupObj As IntPtr, <Out> ByRef numAuthGroup As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllAuthGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef authGroupObj As IntPtr, <Out> ByRef numAuthGroup As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetAuthGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, authGroups As IntPtr, authGroupCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAuthGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID, authGroupIds As IntPtr, authGroupIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllAuthGroup(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Floor Level API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetFloorLevel(context As IntPtr, deviceId As BS2_SCHEDULE_ID, floorLevelIds As IntPtr, floorLevelIdCount As BS2_SCHEDULE_ID, <Out> ByRef floorLevelObj As IntPtr, <Out> ByRef numFloorLevel As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllFloorLevel(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef floorLevelObj As IntPtr, <Out> ByRef numFloorLevel As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetFloorLevel(context As IntPtr, deviceId As BS2_SCHEDULE_ID, floorLevels As IntPtr, floorLevelCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveFloorLevel(context As IntPtr, deviceId As BS2_SCHEDULE_ID, floorLevelIds As IntPtr, floorLevelIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllFloorLevel(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< USB Exported API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatabaseInfoFromDir(context As IntPtr, szDir As IntPtr, <Out> ByRef numUsers As BS2_SCHEDULE_ID, <Out> ByRef numCards As BS2_SCHEDULE_ID, <Out> ByRef numFingers As BS2_SCHEDULE_ID, <Out> ByRef numFaces As BS2_SCHEDULE_ID, cbIsAcceptableUserID As IsAcceptableUserID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserListFromDir(context As IntPtr, szDir As IntPtr, <Out> ByRef outUidObjs As IntPtr, <Out> ByRef outNumUids As BS2_SCHEDULE_ID, cbIsAcceptableUserID As IsAcceptableUserID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserInfosFromDir(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlob()) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatasFromDir(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlob(), userMask As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserInfosExFromDir(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlobEx()) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatasExFromDir(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlobEx(), userMask As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLogFromDir(context As IntPtr, szDir As IntPtr, eventId As BS2_SCHEDULE_ID, amount As BS2_SCHEDULE_ID, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetFilteredLogFromDir(context As IntPtr, szDir As IntPtr, uid As IntPtr, eventCode As UShort, start As BS2_SCHEDULE_ID, [end] As BS2_SCHEDULE_ID, tnakey As Byte, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLogBlobFromDir(context As IntPtr, szDir As IntPtr, eventMask As UShort, eventId As BS2_SCHEDULE_ID, amount As BS2_SCHEDULE_ID, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLogSmallBlobFromDir(context As IntPtr, szDir As IntPtr, eventMask As UShort, eventId As BS2_SCHEDULE_ID, amount As BS2_SCHEDULE_ID, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLogSmallBlobExFromDir(context As IntPtr, szDir As IntPtr, eventMask As UShort, eventId As BS2_SCHEDULE_ID, amount As BS2_SCHEDULE_ID, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID) As Integer
        End Function

        ' With key
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatabaseInfoFromDirWithKey(context As IntPtr, szDir As IntPtr, <Out> ByRef numUsers As BS2_SCHEDULE_ID, <Out> ByRef numCards As BS2_SCHEDULE_ID, <Out> ByRef numFingers As BS2_SCHEDULE_ID, <Out> ByRef numFaces As BS2_SCHEDULE_ID, cbIsAcceptableUserID As IsAcceptableUserID, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserListFromDirWithKey(context As IntPtr, szDir As IntPtr, <Out> ByRef outUidObjs As IntPtr, <Out> ByRef outNumUids As BS2_SCHEDULE_ID, cbIsAcceptableUserID As IsAcceptableUserID, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserInfosFromDirWithKey(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlob(), <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatasFromDirWithKey(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlob(), userMask As BS2_SCHEDULE_ID, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserInfosExFromDirWithKey(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlobEx(), <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatasExFromDirWithKey(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlobs As BS2UserBlobEx(), userMask As BS2_SCHEDULE_ID, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallInfosFromDirWithKey(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlob(), <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallDatasFromDirWithKey(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlob(), userMask As BS2_USER_MASK, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallInfosExFromDirWithKey(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlobEx(), <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallDatasExFromDirWithKey(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlobEx(), userMask As BS2_USER_MASK, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLogFromDirWithKey(context As IntPtr, szDir As IntPtr, eventId As BS2_SCHEDULE_ID, amount As BS2_SCHEDULE_ID, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLogBlobFromDirWithKey(context As IntPtr, szDir As IntPtr, eventMask As UShort, eventId As BS2_SCHEDULE_ID, amount As BS2_SCHEDULE_ID, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetFilteredLogFromDirWithKey(context As IntPtr, szDir As IntPtr, uid As IntPtr, eventCode As UShort, start As BS2_SCHEDULE_ID, [end] As BS2_SCHEDULE_ID, tnakey As Byte, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLogSmallBlobFromDirWithKey(context As IntPtr, szDir As IntPtr, eventMask As UShort, eventId As BS2_SCHEDULE_ID, amount As BS2_SCHEDULE_ID, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetLogSmallBlobExFromDirWithKey(context As IntPtr, szDir As IntPtr, eventMask As UShort, eventId As BS2_SCHEDULE_ID, amount As BS2_SCHEDULE_ID, <Out> ByRef logObjs As IntPtr, <Out> ByRef numLog As BS2_SCHEDULE_ID, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserInfosFaceExFromDirWithKey(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserFaceExBlob(), <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatasFaceExFromDirWithKey(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserFaceExBlob(), userMask As BS2_USER_MASK, <Out> ByRef keyInfo As BS2EncryptKey) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< WRAPPER >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 

        Public Function CSP_BS2_GetAllAccessSchedule(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef accessScheduleObj As CSP_BS2Schedule(), <Out> ByRef numAccessSchedule As BS2_SCHEDULE_ID) As BS2ErrorCode
            Return Util.CSP_BS2_GetAll(Of CSP_BS2Schedule, CXX_BS2Schedule)(context, deviceId, accessScheduleObj, numAccessSchedule, AddressOf API.BS2_GetAllAccessSchedule)
        End Function

        Public Function CSP_BS2_GetAccessSchedule(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessScheduleIds As BS2_SCHEDULE_ID(), accessScheduleIdCount As BS2_SCHEDULE_ID, <Out> ByRef accessScheduleObj As CSP_BS2Schedule(), <Out> ByRef numAccessSchedule As BS2_SCHEDULE_ID) As BS2ErrorCode
            Return Util.CSP_BS2_GetItems(Of BS2_SCHEDULE_ID, CSP_BS2Schedule, BS2_SCHEDULE_ID, CXX_BS2Schedule)(context, deviceId, accessScheduleIds, accessScheduleIdCount, accessScheduleObj, numAccessSchedule, AddressOf API.BS2_GetAccessSchedule)
        End Function

        Public Function CSP_BS2_RemoveAccessSchedule(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessScheduleIds As BS2_SCHEDULE_ID(), accessScheduleIdCount As BS2_SCHEDULE_ID) As BS2ErrorCode
            Return Util.CSP_BS2_RemoveItems(Of BS2_SCHEDULE_ID, BS2_SCHEDULE_ID)(context, deviceId, accessScheduleIds, accessScheduleIdCount, AddressOf API.BS2_RemoveAccessSchedule)
        End Function

        Public Function CSP_BS2_SetAccessSchedule(context As IntPtr, deviceId As BS2_SCHEDULE_ID, accessSchedules As CSP_BS2Schedule(), accessScheduleCount As BS2_SCHEDULE_ID) As BS2ErrorCode
            Return Util.CSP_BS2_SetItems(Of CSP_BS2Schedule, CXX_BS2Schedule)(context, deviceId, accessSchedules, accessScheduleCount, AddressOf API.BS2_SetAccessSchedule)
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< SlaveEx Control API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetSlaveExDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID, channelport As BS2_SCHEDULE_ID, <Out> ByRef slaveDeviceObj As IntPtr, <Out> ByRef outchannelport As BS2_SCHEDULE_ID, <Out> ByRef slaveDeviceCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetSlaveExDevice(context As IntPtr, deviceId As BS2_SCHEDULE_ID, channelport As BS2_SCHEDULE_ID, slaveDeviceObj As IntPtr, slaveDeviceCount As BS2_SCHEDULE_ID) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< IntrusionAlarmZone API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetIntrusionAlarmZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> zoneBlobs As BS2IntrusionAlarmZoneBlob(), <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetIntrusionAlarmZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllIntrusionAlarmZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetIntrusionAlarmZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> zoneBlobs As BS2IntrusionAlarmZoneBlob(), zoneCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetIntrusionAlarmZoneAlarm(context As IntPtr, deviceId As BS2_SCHEDULE_ID, alarmed As Byte, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveIntrusionAlarmZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllIntrusionAlarmZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetIntrusionAlarmZoneArm(context As IntPtr, deviceId As BS2_SCHEDULE_ID, alarmed As Byte, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

#Region "DEVICE_ZONE_SUPPORTED"
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDeviceZoneMasterConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2DeviceZoneMasterConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDeviceZoneMasterConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2DeviceZoneMasterConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveDeviceZoneMasterConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDeviceZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, Ids As IntPtr, IdCount As BS2_SCHEDULE_ID, <Out> ByRef deviceZoneObj As IntPtr, <Out> ByRef numDeviceZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllDeviceZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef deviceZoneObj As IntPtr, <Out> ByRef numDeviceZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDeviceZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, deviceZones As IntPtr, deviceZoneCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveDeviceZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, Ids As IntPtr, IdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllDeviceZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDeviceZoneConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2DeviceZoneConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDeviceZoneConfig(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2DeviceZoneConfig) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDeviceZoneAlarm(context As IntPtr, deviceId As BS2_SCHEDULE_ID, alarmed As Byte, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ClearDeviceZoneAccessRecord(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneID As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ClearAllDeviceZoneAccessRecord(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneID As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDeviceZoneAGEntranceLimit(context As IntPtr, deviceId As BS2_SCHEDULE_ID, Ids As IntPtr, IdCount As BS2_SCHEDULE_ID, <Out> ByRef deviceZoneAGEntranceLimitObj As IntPtr, <Out> ByRef numDeviceZoneAGEntranceLimit As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllDeviceZoneAGEntranceLimit(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef deviceZoneAGEntranceLimitObj As IntPtr, <Out> ByRef numDeviceZoneAGEntranceLimit As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDeviceZoneAGEntranceLimit(context As IntPtr, deviceId As BS2_SCHEDULE_ID, deviceZoneAGEntranceLimits As IntPtr, deviceZoneAGEntranceLimitCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveDeviceZoneAGEntranceLimit(context As IntPtr, deviceId As BS2_SCHEDULE_ID, Ids As IntPtr, IdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllDeviceZoneAGEntranceLimit(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

#End Region

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetSupportedConfigMask(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef configMask As BS2_CONFIG_MASK) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetSupportedUserMask(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef userMask As BS2_USER_MASK) As Integer
        End Function

        'Debugging
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub CBDebugPrint(msg As String)

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDebugLevel(ptrCBDebugPrint As CBDebugPrint, debugLevel As BS2_SCHEDULE_ID) As Integer
        End Function

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub CBDebugExPrint(level As BS2_SCHEDULE_ID, [module] As BS2_SCHEDULE_ID, msg As String)

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDebugExCallback(ptrCBDebugExPrint As CBDebugExPrint, level As BS2_SCHEDULE_ID, [module] As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDebugFileLog(level As BS2_SCHEDULE_ID, [module] As BS2_SCHEDULE_ID, logPath As IntPtr) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDebugFileLogEx(level As BS2_SCHEDULE_ID, [module] As BS2_SCHEDULE_ID, logPath As IntPtr, fileMaxSizeMB As Integer) As Integer
        End Function

        ' <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< InterlockZone API >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetInterlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> zoneBlob As BS2InterlockZoneBlob(), <Out> ByRef numZone As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetInterlockZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllInterlockZoneStatus(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef zoneStatusObj As IntPtr, <Out> ByRef numZoneStatus As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetInterlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> zoneBlobs As BS2InterlockZoneBlob(), zoneCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetInterlockZoneAlarm(context As IntPtr, deviceId As BS2_SCHEDULE_ID, alarmed As Byte, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveInterlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID, zoneIds As IntPtr, zoneIdCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllInterlockZone(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        '=> [IPv6]
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetIPConfigViaUDPEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2IpConfig, hostipAddr As IntPtr) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetIPConfigViaUDPEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2IpConfig, hostipAddr As IntPtr) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetIPV6Config(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2IPV6Config) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetIPV6Config(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2IPV6Config) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetIPV6ConfigViaUDP(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2IPV6Config) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetIPV6ConfigViaUDP(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2IPV6Config) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetIPV6ConfigViaUDPEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef config As BS2IPV6Config, hostipAddr As IntPtr) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetIPV6ConfigViaUDPEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, ByRef config As BS2IPV6Config, hostipAddr As IntPtr) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetEnableIPV4(context As IntPtr, <Out> ByRef enable As Integer) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetEnableIPV4(context As IntPtr, enable As Integer) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetEnableIPV6(context As IntPtr, <Out> ByRef enable As Integer) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetEnableIPV6(context As IntPtr, enable As Integer) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetServerPortIPV6(context As IntPtr, serverPort As UShort) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetServerPortIPV6(context As IntPtr, <Out> ByRef serverPort As UShort) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetSSLServerPortIPV6(context As IntPtr, sslServerPort As UShort) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetSSLServerPortIPV6(context As IntPtr, <Out> ByRef sslServerPort As UShort) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetSpecifiedDeviceInfo(context As IntPtr, deviceId As BS2_SCHEDULE_ID, specifiedDeviceInfo As BS2_SCHEDULE_ID, pOutDeviceInfo As IntPtr, nDeviceInfoSize As BS2_SCHEDULE_ID, <Out> ByRef pOutDeviceInfoSize As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_ConnectDeviceIPV6(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SearchDevicesCoreStationEx(context As IntPtr, hostipAddr As IntPtr) As Integer
        End Function
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetServerPort(context As IntPtr, <Out> ByRef serverPort As UShort) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetSSLServerPort(context As IntPtr, <Out> ByRef sslServerPort As UShort) As Integer
        End Function
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAuthOperatorLevelEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, userIDs As IntPtr, userIDCount As BS2_SCHEDULE_ID, <Out> ByRef operatorlevelObj As IntPtr, <Out> ByRef numOperatorlevel As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetAllAuthOperatorLevelEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef operatorlevelObj As IntPtr, <Out> ByRef numOperatorlevel As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetAuthOperatorLevelEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, operatorlevels As IntPtr, operatorlevelCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAuthOperatorLevelEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, userIDs As IntPtr, userIDCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_RemoveAllAuthOperatorLevelEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID) As Integer
        End Function
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_SetDefaultResponseTimeout(context As IntPtr, ms As Integer) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetDefaultResponseTimeout(context As IntPtr, <Out> ByRef poMs As Integer) As Integer
        End Function
        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_VerifyUserSmall(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, ByRef userBlob As BS2UserSmallBlob) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_IdentifyUserSmall(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, ByRef userBlob As BS2UserSmallBlob) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallInfos(context As IntPtr, deviceId As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlob()) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallDatas(context As IntPtr, deviceId As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlob(), userMask As BS2_USER_MASK) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_EnrollUserSmall(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlob(), userCount As BS2_SCHEDULE_ID, overwrite As Byte) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_VerifyUserSmallEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, ByRef userBlob As BS2UserSmallBlobEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_VerifyUserFaceEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, ByRef userBlob As BS2UserFaceExBlob) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_IdentifyUserSmallEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, seq As UShort, handleResult As Integer, ByRef userBlob As BS2UserSmallBlobEx) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallInfosEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlobEx()) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallDatasEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlobEx(), userMask As BS2_USER_MASK) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_EnrollUserSmallEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlobEx(), userCount As BS2_SCHEDULE_ID, overwrite As Byte) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallInfosFromDir(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlob()) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallDatasFromDir(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlob(), userMask As BS2_USER_MASK) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallInfosExFromDir(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlobEx()) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserSmallDatasExFromDir(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserSmallBlobEx(), userMask As BS2_USER_MASK) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_EnrollUserFaceEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserFaceExBlob(), userCount As BS2_SCHEDULE_ID, overwrite As Byte) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserInfosFaceEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserFaceExBlob()) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatasFaceEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserFaceExBlob(), userMask As BS2_USER_MASK) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserInfosFaceExFromDir(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserFaceExBlob()) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserDatasFaceExFromDir(context As IntPtr, szDir As IntPtr, uids As IntPtr, uidCount As BS2_SCHEDULE_ID,
        <[In], Out> userBlob As BS2UserFaceExBlob(), userMask As BS2_USER_MASK) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_PartialUpdateUser(context As IntPtr, deviceId As BS2_SCHEDULE_ID, mask As BS2_USER_MASK,
        <[In], Out> userBlob As BS2UserBlob(), userCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_PartialUpdateUserEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, mask As BS2_USER_MASK,
        <[In], Out> userBlob As BS2UserBlobEx(), userCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_PartialUpdateUserSmall(context As IntPtr, deviceId As BS2_SCHEDULE_ID, mask As BS2_USER_MASK,
        <[In], Out> userBlob As BS2UserSmallBlob(), userCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_PartialUpdateUserSmallEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, mask As BS2_USER_MASK,
        <[In], Out> userBlob As BS2UserSmallBlobEx(), userCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_PartialUpdateUserFaceEx(context As IntPtr, deviceId As BS2_SCHEDULE_ID, mask As BS2_USER_MASK,
        <[In], Out> userBlob As BS2UserFaceExBlob(), userCount As BS2_SCHEDULE_ID) As Integer
        End Function

        <DllImport("BS_SDK_V2.dll", CallingConvention:=CallingConvention.Cdecl)>
        Public Function BS2_GetUserStatistic(context As IntPtr, deviceId As BS2_SCHEDULE_ID, <Out> ByRef statistic As BS2UserStatistic) As Integer
        End Function
    End Module
End Namespace
