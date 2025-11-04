Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Reflection

    Imports BS2_SCHEDULE_ID = System.UInt32
	
Namespace Suprema

    Public Module BS2Environment
        Public Const BS2_TCP_DEVICE_PORT_DEFAULT As Integer = 51211
        Public Const BS2_TCP_SERVER_PORT_DEFAULT As Integer = 51212
        Public Const BS2_TCP_SSL_SERVER_PORT_DEFAULT As Integer = 51213 '=> [Get ServerPort] <=

        '=> [IPv6]
        Public Const BS2_IPV6_ADDR_SIZE As Integer = 40
        Public Const BS2_MAX_IPV6_ALLOCATED_ADDR As Integer = 8
        '<=

        '=> [IPv6]
        Public Const BS2_TCP_DEVICE_PORT_DEFAULT_V6 As Integer = 52211
        Public Const BS2_TCP_SERVER_PORT_DEFAULT_V6 As Integer = 52212
        Public Const BS2_TCP_SSL_SERVER_PORT_DEFAULT_V6 As Integer = 52213
        '<=

        Public Const BS2_MAX_NUM_OF_FINGER_PER_USER As Integer = 10
        Public Const BS2_MAX_NUM_OF_CARD_PER_USER As Integer = 8
        Public Const BS2_MAX_NUM_OF_FACE_PER_USER As Integer = 5
        Public Const BS2_NUM_OF_AUTH_MODE As Integer = 11
        Public Const BS2_MAC_ADDR_LEN As Integer = 6
        Public Const BS2_MODEL_NAME_LEN As Integer = 32
        Public Const BS2_KERNEL_REV_LEN As Integer = 32
        Public Const BS2_BSCORE_REV_LEN As Integer = 32
        Public Const BS2_FIRMWARE_REV_LEN As Integer = 32
        Public Const BS2_IPV4_ADDR_SIZE As Integer = 16
        Public Const BS2_URL_SIZE As Integer = 256
        Public Const BS2_USER_ID_SIZE As Integer = 32
        Public Const BS2_USER_NAME_LEN As Integer = 48 * 4
        Public Const BS2_USER_PHOTO_SIZE As Integer = 16 * 1024
        Public Const BS2_PIN_HASH_SIZE As Integer = 32
        Public Const BS2_MAX_OPERATORS As Integer = 10
        Public Const BS2_DEVICE_STATUS_NUM As Integer = CInt(BS2DeviceStatus.NUM_OF_STATUS)
        Public Const BS2_MAX_SHORTCUT_HOME As Integer = 8
        Public Const BS2_MAX_TNA_KEY As Integer = 16
        Public Const BS2_MAX_TNA_LABEL_LEN As Integer = 16 * 3
        Public Const BS2_CARD_KEY_SIZE As Integer = 32
        Public Const BS2_CARD_DATA_SIZE As Integer = 32
        Public Const BS2_FINGER_TEMPLATE_SIZE As Integer = 384
        Public Const BS2_TEMPLATE_PER_FINGER As Integer = 2
        Public Const BS2_FACE_TEMPLATE_LENGTH As Integer = 3008
        Public Const BS2_TEMPLATE_PER_FACE As Integer = 30
        Public Const BS2_WIEGAND_MAX_FIELDS As Integer = 4
        Public Const BS2_WIEGAND_MAX_PARITIES As Integer = 4
        Public Const BS2_WIEGAND_FIELD_SIZE As Integer = 32
        Public Const BS2_MAX_ACCESS_GROUP_NAME_LEN As Integer = 144
        Public Const BS2_MAX_ACCESS_GROUP_PER_USER As Integer = 16
        Public Const BS2_MAX_ACCESS_LEVEL_PER_ACCESS_GROUP As Integer = 128
        Public Const BS2_MAX_ACCESS_LEVEL_NAME_LEN As Integer = 144
        Public Const BS2_MAX_ACCESS_LEVEL_ITEMS As Integer = 128
        Public Const BS2_MAX_TIME_PERIODS_PER_DAY As Integer = 5
        Public Const BS2_NUM_WEEKDAYS As Integer = 7
        Public Const BS2_MAX_DAYS_PER_DAILY_SCHEDULE As Integer = 90
        Public Const BS2_MAX_SCHEDULE_NAME_LEN As Integer = 144
        Public Const BS2_MAX_HOLIDAY_GROUPS_PER_SCHEDULE As Integer = 4
        Public Const BS2_MAX_HOLIDAY_GROUP_NAME_LEN As Integer = 144
        Public Const BS2_MAX_HOLIDAYS_PER_GROUP As Integer = 128
        Public Const BS2_RS485_MAX_CHANNELS As Integer = 4
        Public Const BS2_RS485_MAX_SLAVES_PER_CHANNEL As Integer = 32
        Public Const BS2_WIEGAND_STATUS_NUM As Integer = 2
        Public Const BS2_MAX_INPUT_NUM As Integer = 8
        Public Const BS2_WLAN_SSID_SIZE As Integer = 32
        Public Const BS2_WLAN_KEY_SIZE As Integer = 64
        Public Const BS2_EVENT_MAX_IMAGE_CODE_COUNT As Integer = 32
        Public Const MAX_WIEGAND_IN_COUNT As Integer = 15
        Public Const BS2_LED_SIGNAL_NUM As Integer = 3
        Public Const BS2_BUZZER_SIGNAL_NUM As Integer = 3
        Public Const BS2_MAX_TRIGGER_ACTION As Integer = 128
        Public Const BS2_MAX_DOOR_NAME_LEN As Integer = 144
        Public Const BS2_MAX_FORCED_OPEN_ALARM_ACTION As Integer = 5
        Public Const BS2_MAX_HELD_OPEN_ALARM_ACTION As Integer = 5
        Public Const BS2_MAX_DUAL_AUTH_APPROVAL_GROUP As Integer = 16
        Public Const BS2_MAX_RESOURCE_ITEM_COUNT As Integer = 128
        Public Const BS2_MAX_BLACK_LIST_SLOTS As Integer = 1000        
        Public Const BS2_SMART_CARD_MAX_TEMPLATE_COUNT As Integer = 4
        Public Const BS2_SMART_CARD_MAX_ACCESS_GROUP_COUNT As Integer = 16
        Public Const BS2_SMART_CARD_MIN_TEMPLATE_SIZE As Integer = 300
        Public Const BS2_MAX_ZONE_NAME_LEN As Integer = 144
        Public Const BS2_MAX_APB_ALARM_ACTION As Integer = 5
        Public Const BS2_MAX_READERS_PER_APB_ZONE As Integer = 64
        Public Const BS2_MAX_BYPASS_GROUPS_PER_APB_ZONE As Integer = 16
        Public Const BS2_MAX_TIMED_APB_ALARM_ACTION As Integer = 5
        Public Const BS2_MAX_READERS_PER_TIMED_APB_ZONE As Integer = 64
        Public Const BS2_MAX_BYPASS_GROUPS_PER_TIMED_APB_ZONE As Integer = 16
        Public Const BS2_MAX_FIRE_SENSORS_PER_FIRE_ALARM_ZONE As Integer = 8
        Public Const BS2_MAX_FIRE_ALARM_ACTION As Integer = 5
        Public Const BS2_MAX_DOORS_PER_FIRE_ALARM_ZONE As Integer = 32
        Public Const BS2_MAX_SCHEDULED_LOCK_UNLOCK_ALARM_ACTION As Integer = 5
        Public Const BS2_MAX_DOORS_IN_SCHEDULED_LOCK_UNLOCK_ZONE As Integer = 32
        Public Const BS2_MAX_BYPASS_GROUPS_IN_SCHEDULED_LOCK_UNLOCK_ZONE As Integer = 16
        Public Const BS2_MAX_UNLOCK_GROUPS_IN_SCHEDULED_LOCK_UNLOCK_ZONE As Integer = 16
        Public Const BS2_VOIP_MAX_PHONEBOOK As Integer = 32
        Public Const BS2_VOIP_MAX_PHONEBOOK_EXT As Integer = 128
        Public Const BS2_MAX_DESCRIPTION_NAME_LEN As Integer = 144
        Public Const BS2_VOIP_MAX_DESCRIPTION_LEN_EXT As Integer = 48 * 3
        Public Const BS2_FACE_WIDTH_MIN_DEFAULT As Integer = 66               ' F2
        Public Const BS2_FACE_WIDTH_MAX_DEFAULT As Integer = 250              ' F2
        Public Const BS2_FACE_SEARCH_RANGE_X_DEFAULT As Integer = 144         ' F2
        Public Const BS2_FACE_SEARCH_RANGE_WIDTH_DEFAULT As Integer = 432     ' F2
        Public Const BS2_FACE_DETECT_DISTANCE_MIN_MIN As Integer = 30         ' BS3
        Public Const BS2_FACE_DETECT_DISTANCE_MIN_MAX As Integer = 100        ' BS3
        Public Const BS2_FACE_DETECT_DISTANCE_MIN_DEFAULT As Integer = 30     ' BS3      (60 -> 30)
        Public Const BS2_FACE_DETECT_DISTANCE_MAX_MIN As Integer = 40         ' BS3
        Public Const BS2_FACE_DETECT_DISTANCE_MAX_MAX As Integer = 100        ' BS3
        Public Const BS2_FACE_DETECT_DISTANCE_MAX_INF As Integer = 255        ' BS3
        Public Const BS2_FACE_DETECT_DISTANCE_MAX_DEFAULT As Integer = 100    ' BS3
        Public Const BS2_FACE_IMAGE_SIZE As Integer = 16 * 1024
        Public Const BS2_CAPTURE_IMAGE_MAXSIZE As Integer = 1280 * 720 * 3
		Public Const BS2_MAX_AUTH_GROUP_NAME_LEN As Integer = 144
        Public Const BS2_MAX_JOB_SIZE As Integer = 16
        Public Const BS2_MAX_JOBLABEL_LEN As Integer = 48
        Public Const BS2_USER_PHRASE_SIZE As Integer = 128
		Public Const BS2_MAX_FLOOR_LEVEL_NAME_LEN As Integer = 144
        Public Const BS2_MAX_FLOOR_LEVEL_ITEMS As Integer = 128
		Public Const BS2_MAX_LIFT_NAME_LEN As Integer = 144
        Public Const BS2_MAX_DEVICES_ON_LIFT As Integer = 4
        Public Const BS2_MAX_FLOORS_ON_LIFT As Integer = 255
        Public Const BS2_MAX_DUAL_AUTH_APPROVAL_GROUP_ON_LIFT As Integer = 16
        Public Const BS2_MAX_ALARMS_ON_LIFT As Integer = 2
        Public Const BS2_EVENT_MAX_IMAGE_SIZE As Integer = 16384
        Public Const BS2_RS485_MAX_CHANNELS_EX As Integer = 8
        Public Const BS2_MAX_DST_SCHEDULE As Integer = 2

        Public Const BS2_ENC_KEY_SIZE As Integer = 32

        ' F2 support
        Public Const BS2_MAX_NUM_OF_EXT_AUTH_MODE As Integer = 128

        Public Const BS2_FACE_EX_TEMPLATE_SIZE As Integer = 552
	    Public Const BS2_VISUAL_TEMPLATES_PER_FACE_EX As Integer = 10
	    Public Const BS2_IR_TEMPLATES_PER_FACE_EX As Integer = 10
	    Public Const BS2_MAX_TEMPLATES_PER_FACE_EX As Integer = Suprema.BS2Environment.BS2_VISUAL_TEMPLATES_PER_FACE_EX + Suprema.BS2Environment.BS2_IR_TEMPLATES_PER_FACE_EX

        Public Const BS2_MAX_WARPED_IMAGE_LENGTH As Integer = 40 * 1024
        Public Const BS2_MAX_WARPED_IR_IMAGE_LENGTH As Integer = 30 * 1024
        ' F2 support

        Public Const BS2_THERMAL_CAMERA_DISTANCE_DEFAULT As Integer = 100
        <System.ObsoleteAttribute>
        Public Const BS2_THERMAL_CAMERA_EMISSION_RATE_DEFAULT As Integer = 98
        Public Const BS2_THERMAL_CAMERA_EMISSIVITY_DEFAULT As Integer = 98

        ' Default (F2)
        Public Const BS2_THERMAL_CAMERA_ROI_X_DEFAULT As Integer = 30
        Public Const BS2_THERMAL_CAMERA_ROI_Y_DEFAULT As Integer = 25
        Public Const BS2_THERMAL_CAMERA_ROI_WIDTH_DEFAULT As Integer = 50
        Public Const BS2_THERMAL_CAMERA_ROI_HEIGHT_DEFAULT As Integer = 55

        ' Default (FS2)
	    Public Const BS2_THERMAL_CAMERA_ROI_X_DEFAULT_FS2 As Integer = 47
	    Public Const BS2_THERMAL_CAMERA_ROI_Y_DEFAULT_FS2 As Integer = 45
	    Public Const BS2_THERMAL_CAMERA_ROI_WIDTH_DEFAULT_FS2 As Integer = 15
        Public Const BS2_THERMAL_CAMERA_ROI_HEIGHT_DEFAULT_FS2 As Integer = 10

        Public Const BS2_MOBILE_ACCESS_KEY_SIZE As Integer = 124

        ' BS2InputConfigEx
        Public Const BS2_MAX_INPUT_NUM_EX As Integer = 16

        Public Const BS2_INPUT_NONE As Integer = 0
        Public Const BS2_INPUT_AUX0 As Integer = 1
        Public Const BS2_INPUT_AUX1 As Integer = 2
        Public Const BS2_INPUT_AUXTYPENO As Integer = 0
        Public Const BS2_INPUT_AUXTYPENC As Integer = 1

        ' BS2RelayActionConfig
        Public Const BS2_MAX_RELAY_ACTION As Integer = 4
        Public Const BS2_MAX_RELAY_ACTION_INPUT As Integer = 16

#Region "DEVICE_ZONE_SUPPORTED"
        Public Const BS2_TCP_DEVICE_ZONE_MASTER_PORT_DEFAULT As Integer = 51214
        Public Const BS2_MAX_DEVICE_ZONE As Integer = 8
#End Region

#Region "ENTRNACE_LIMIT"
        Public Const BS2_MAX_READERS_PER_DEVICE_ZONE_ENTRANCE_LIMIT As Integer = 64
	    Public Const BS2_MAX_BYPASS_GROUPS_PER_DEVICE_ZONE_ENTRANCE_LIMIT As Integer = 16
	    Public Const BS2_MAX_DEVICE_ZONE_ENTRANCE_LIMIT_ALARM_ACTION As Integer = 5
	    Public Const BS2_MAX_ENTRANCE_LIMIT_PER_ZONE As Integer = 24
        Public Const BS2_MAX_ACCESS_GROUP_ENTRANCE_LIMIT_PER_ENTRACE_LIMIT As Integer = 16
        Public Const BS2_ENTRY_COUNT_FOR_ACCESS_GROUP_ENTRANCE_LIMIT As Integer = -2
        Public Const BS2_OTHERWISE_ACCESS_GROUP_ID As Integer = -1
        Public Const BS2_ENTRY_COUNT_NO_LIMIT As Integer = -1
#End Region


#Region "FIRE_ALARM"
        Public Const BS2_MAX_READERS_PER_DEVICE_ZONE_FIRE_ALARM As Integer = 64
        Public Const BS2_MAX_DEVICE_ZONE_FIRE_ALARM_ALARM_ACTION As Integer = 5
        Public Const BS2_MAX_FIRE_SENSORS_PER_DEVICE_ZONE_FIRE_ALARM_MEMBER As Integer = 8
        Public Const BS2_MAX_DOORS_PER_DEVICE_ZONE_FIRE_ALARM_MEMBER As Integer = 8
#End Region


#Region "INTERLOCK_ZONE_SUPPORTED"
        Public Const BS2_MAX_INTERLOCK_ZONE As Integer = 32
        Public Const BS2_MAX_INPUTS_IN_INTERLOCK_ZONE As Integer = 4
        Public Const BS2_MAX_OUTPUTS_IN_INTERLOCK_ZONE As Integer = 8
        Public Const BS2_MAX_DOORS_IN_INTERLOCK_ZONE As Integer = 4
#End Region


        Public ReadOnly All_routers_in_the_site_local As String = "FF05::2" 'All router multicast (in the site local)
        Public ReadOnly DEFAULT_MULTICAST_IPV6_ADDRESS As String = Suprema.BS2Environment.All_routers_in_the_site_local
        Public ReadOnly DEFAULT_BROADCAST_IPV4_ADDRESS As String = "255.255.255.255"


        Public Const BS2_MAX_LIFT_LOCK_UNLOCK_ALARM_ACTION As Integer = 5
	    Public Const BS2_MAX_LIFTS_IN_LIFT_LOCK_UNLOCK_ZONE As Integer = 32
	    Public Const BS2_MAX_BYPASS_GROUPS_IN_LIFT_LOCK_UNLOCK_ZONE As Integer = 16
	    Public Const BS2_MAX_UNLOCK_GROUPS_IN_LIFT_LOCK_UNLOCK_ZONE As Integer = 16

	    Public Const BS2_DEVICE_ID_MIN As Integer = &H01000000
        Public Const BS2_DEVICE_ID_MAX As Integer = &H3FFFFFFF

        Public Const BS2_BARCODE_TIMEOUT_DEFAULT As Integer = 4
        Public Const BS2_BARCODE_TIMEOUT_MIN As Integer = Suprema.BS2Environment.BS2_BARCODE_TIMEOUT_DEFAULT
        Public Const BS2_BARCODE_TIMEOUT_MAX As Integer = 10
        Public Const BS2_VISUAL_BARCODE_TIMEOUT_DEFAULT As Integer = 10
        Public Const BS2_VISUAL_BARCODE_TIMEOUT_MIN As Integer = 3
        Public Const BS2_VISUAL_BARCODE_TIMEOUT_MAX As Integer = 20

        ' Intelligent PD
        Public Const BS2_RS485_MAX_EXCEPTION_CODE_LEN As Integer = 8
        Public Const BS2_IPD_OUTPUT_CARDID As Integer = 0
	    Public Const BS2_IPD_OUTPUT_USERID As Integer = 1

        ' Device license
        Public Const BS2_MAX_LICENSE_COUNT As Integer = 16

        ' OSDP standard
        Public Const BS2_OSDP_STANDARD_ACTION_MAX_COUNT As Integer = 32
        Public Const BS2_OSDP_STANDARD_ACTION_MAX_LED As Integer = 2
        Public Const BS2_OSDP_STANDARD_KEY_SIZE As Integer = 16
        Public Const BS2_OSDP_STANDARD_MAX_DEVICE_PER_CHANNEL As Integer = 8
    End Module

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Version
        Public major As Byte
        Public minor As Byte
        Public ext As Byte
        Public reserved As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FactoryConfig
        Public deviceID As System.UInt32        
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAC_ADDR_LEN)>
        Public macAddr As Byte()
        Public reserved As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MODEL_NAME_LEN)>
        Public modelName As Byte()
        Public boardVer As Suprema.BS2Version
        Public kernelVer As Suprema.BS2Version
        Public bscoreVer As Suprema.BS2Version
        Public firmwareVer As Suprema.BS2Version
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_KERNEL_REV_LEN)>
        Public kernelRev As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_BSCORE_REV_LEN)>
        Public bscoreRev As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_FIRMWARE_REV_LEN)>
        Public firmwareRev As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SystemConfig
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16 * 16 * 3)>
        Public notUsed As Byte()
        Public timezone As System.Int32 'offset of GMT in second
        Public syncTime As Byte
        Public serverSync As Byte
        Public deviceLocked As Byte
        Public useInterphone As Byte
        Public useUSBConnection As Byte
        Public keyEncrypted As Byte
        Public useJobCode As Byte
        Public useAlphanumericID As Byte
        Public cameraFrequency As System.UInt32
        Public secureTamper As Byte
        Public reserved0 As Byte                  ' write protected
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
        Public useCardOperationMask As System.UInt32     ' [+2.6.4]
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AuthOperatorLevel
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public userID As Byte()
        Public level As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AuthConfig
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_NUM_OF_AUTH_MODE)>
        Public authSchedule As System.UInt32()
        Public useGlobalAPB As Byte
        Public globalAPBFailAction As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=30)>
        Public reserved As Byte()
        Public usePrivateAuth As Byte
        Public faceDetectionLevel As Byte
        Public useServerMatching As Byte
        Public useFullAccess As Byte
        Public matchTimeout As Byte
        Public authTimeout As Byte
        Public numOperators As Byte
        Public reserved2 As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_OPERATORS)>
        Public operators As Suprema.BS2AuthOperatorLevel()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AuthConfigExt
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_NUM_OF_EXT_AUTH_MODE)>
        Public extAuthSchedule As System.UInt32()
        Public useGlobalAPB As Byte
        Public globalAPBFailAction As Byte
	    Public useGroupMatching As Byte
        Public reserved As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4)>
        Public reserved2 As Byte()
        Public usePrivateAuth As Byte
        Public faceDetectionLevel As Byte
        Public useServerMatching As Byte
        Public useFullAccess As Byte
        Public matchTimeout As Byte
        Public authTimeout As Byte
        Public numOperators As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=1)>
        Public reserved3 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_OPERATORS)>
        Public operators As Suprema.BS2AuthOperatorLevel()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=256)>
        Public reserved4 As Byte()
	End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FaceConfigExt
        Public thermalCheckMode As Byte
        Public maskCheckMode As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()

        Public thermalFormat As Byte
        Public reserved2 As Byte

        Public thermalThresholdLow As System.UInt16
        Public thermalThresholdHigh As System.UInt16
        Public maskDetectionLevel As Byte
        Public auditTemperature As Byte

        Public useRejectSound As Byte
        Public useOverlapThermal As Byte
        Public useDynamicROI As Byte
        Public faceCheckOrder As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ThermalCameraROI
        Public x As System.UInt16
        Public y As System.UInt16
        Public width As System.UInt16
        Public height As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ThermalCameraConfig
        Public distance As Byte
        Public emissionRate As Byte       ' (emissivity)
        Public roi As Suprema.BS2ThermalCameraROI
        Public useBodyCompensation As Byte
        Public compensationTemperature As SByte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LedStatusConfig
        Public enabled As Byte
        Public reserved As Byte
        Public count As System.UInt16 '(0 = infinite)
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_LED_SIGNAL_NUM)>
        Public signal As Suprema.BS2LedSignal()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2BuzzerStatusConfig
        Public enabled As Byte
        Public reserved As Byte
        Public count As System.UInt16 '(0 = infinite)
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_LED_SIGNAL_NUM)>
        Public signal As Suprema.BS2BuzzerSignal()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2StatusConfig
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_DEVICE_STATUS_NUM)>
        Public led As Suprema.BS2LedStatusConfig()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved1 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_DEVICE_STATUS_NUM)>
        Public buzzer As Suprema.BS2BuzzerStatusConfig()
        Public configSyncRequired As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=31)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DisplayConfig
        Public language As System.UInt32
        Public background As Byte
        Public volume As Byte '(0-100)
        Public bgTheme As Byte
        Public dateFormat As Byte
        Public menuTimeout As System.UInt16'(0-255 sec)
        Public msgTimeout As System.UInt16'(500-5000ms)
        Public backlightTimeout As System.UInt16 'in seconds
        Public displayDateTime As Byte
        Public useVoice As Byte
        Public timeFormat As Byte
        Public homeFormation As Byte
        Public useUserPhrase As Byte
        Public queryUserPhrase As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_SHORTCUT_HOME)>
        Public shortcutHome As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_TNA_KEY)>
        Public tnaIcon As Byte()
        Public useScreenSaver As Byte         ' FS2, F2
        Public showOsdpResult As Byte
        Public authMsgUserName As Byte
        Public authMsgUserId As Byte
        Public scrambleKeyboardMode As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=27)>		' FISSDK-83 memory resizing bug when adding useScreenSaver (32->31)
        Public reserved3 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2IpConfig
        Public connectionMode As Byte
        Public useDHCP As Byte
        Public useDNS As Byte
        Public reserved As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV4_ADDR_SIZE)>
        Public ipAddress As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV4_ADDR_SIZE)>
        Public gateway As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV4_ADDR_SIZE)>
        Public subnetMask As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV4_ADDR_SIZE)>
        Public serverAddr As Byte()
        Public port As System.UInt16
        Public serverPort As System.UInt16
        Public mtuSize As System.UInt16
        Public baseband As Byte
        Public reserved2 As Byte
        Public sslServerPort As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=30)>
        Public reserved3 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2IpConfigExt
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV4_ADDR_SIZE)>
        Public dnsAddr As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_URL_SIZE)>
        Public serverUrl As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2TNAInfo
        Public tnaMode As Byte
        Public tnaKey As Byte
        Public tnaRequired As Byte
        Public reserved As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_TNA_KEY)>
        Public tnaSchedule As System.UInt32()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_TNA_KEY)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2TNAExtInfo
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_TNA_KEY * Suprema.BS2Environment.BS2_MAX_TNA_LABEL_LEN)>
        Public tnaLabel As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_TNA_KEY)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2TNAConfig
        Public tnaInfo As Suprema.BS2TNAInfo
        Public tnaExtInfo As Suprema.BS2TNAExtInfo
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2CSNCard
        Public type As Byte
        Public size As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_CARD_DATA_SIZE)>
        Public data As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SmartCardHeader
        Public hdrCRC As System.UInt16
        Public cardCRC As System.UInt16
        Public cardType As Byte
        Public numOfTemplate As Byte
        Public templateSize As System.UInt16
        Public issueCount As System.UInt16
        Public duressMask As Byte
        Public cardAuthMode As Byte
        Public useAlphanumericID As Byte
        Public cardAuthModeEx As Byte         ' for FaceStation F2 only
        Public numOfFaceTemplate As Byte      ' for FaceStation F2 only
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=1)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SmartCardCredentials
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_PIN_HASH_SIZE)>
        Public pin As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_SMART_CARD_MAX_TEMPLATE_COUNT * Suprema.BS2Environment.BS2_FINGER_TEMPLATE_SIZE)>
        Public templateData As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AccessOnCardData
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_SMART_CARD_MAX_ACCESS_GROUP_COUNT)>
        Public accessGroupID As System.UInt16()
        Public startTime As System.UInt32
        Public endTime As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SmartCardData
        Public header As Suprema.BS2SmartCardHeader
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_CARD_DATA_SIZE)>
        Public cardID As Byte()
        Public credentials As Suprema.BS2SmartCardCredentials
        Public accessOnData As Suprema.BS2AccessOnCardData
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Card
        Public isSmartCard As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=1656)>
        Public cardUnion As Byte() ' BS2CSNCard or BS2SmartCardData  
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2MifareCard
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public primaryKey As Byte()
        Public reserved1 As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public secondaryKey As Byte()
        Public reserved2 As System.UInt16
        Public startBlockIndex As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2IClassCard
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8)>
        Public primaryKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8)>
        Public secondaryKey As Byte()
        Public startBlockIndex As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DesFireCard
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public primaryKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public secondaryKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public appID As Byte()
        Public fileID As Byte
        Public encryptionType As Byte                 ' 0: DES/3DES, 1: AES
        Public operationMode As Byte                  ' 0: legacy(use picc master key), 1: new mode(use app master, file read, file write key)
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DesFireAppLevelKey
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public appMasterKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public fileReadKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public fileWriteKey As Byte()
        Public fileReadKeyNumber As Byte
        Public fileWriteKeyNumber As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DesFireCardConfigEx
        Public desfireAppKey As Suprema.BS2DesFireAppLevelKey
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2CardConfig
        ' CSN
        Public byteOrder As Byte
        Public useWiegandFormat As Byte

        ' Smart card
        Public dataType As Byte
        Public useSecondaryKey As Byte
        Public mifare As Suprema.BS2MifareCard
        Public iclass As Suprema.BS2IClassCard
        Public desfire As Suprema.BS2DesFireCard
        Public formatID As System.UInt32 'card format ID, use only application for effective management

        Public cipher As Byte     ' 1 byte (true : make card data from key) for XPASS - D2 KEYPAD

        Public smartCardByteOrder As Byte             ' [+ V2.8.2.7]
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=22)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Fingerprint
        Public index As Byte
        Public flag As Byte
        Public reserved As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_TEMPLATE_PER_FINGER * Suprema.BS2Environment.BS2_FINGER_TEMPLATE_SIZE)>
        Public data As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2VerifyFingerprint
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public userID As Byte()
	    <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_FINGER_TEMPLATE_SIZE)>
	    Public fingerTemplate As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FingerprintConfig
        Public securityLevel As Byte
        Public fastMode As Byte
        Public sensitivity As Byte
        Public sensorMode As Byte
        Public templateFormat As Byte
        Public reserved As Byte
        Public scanTimeout As System.UInt16
        Public successiveScan As Byte
        Public advancedEnrollment As Byte
        Public showImage As Byte
        Public lfdLevel As Byte '0: off, 1~3: on
        Public checkDuplicate As Byte    ' [+2.6.4]
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=31)>
        Public reserved3 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Rs485SlaveDevice
        Public deviceID As System.UInt32
        Public deviceType As System.UInt16
        Public enableOSDP As Byte
        Public connected As Byte
    End Structure

    ' [+2.8]
    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2IntelligentPDInfo
        Public supportConfig As Byte
        Public useExceptionCode As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_RS485_MAX_EXCEPTION_CODE_LEN)>
        Public exceptionCode As Byte()
        Public outputFormat As Byte
        Public osdpID As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Rs485Channel
        Public baudRate As System.UInt32
        Public channelIndex As Byte
        Public useRegistance As Byte
        Public numOfDevices As Byte
        Public reserved As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_RS485_MAX_SLAVES_PER_CHANNEL)>
        Public slaveDevices As Suprema.BS2Rs485SlaveDevice()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Rs485Config
        Public mode As Byte
        Public numOfChannels As Byte
        Public reserved As System.UInt16
        Public intelligentInfo As Suprema.BS2IntelligentPDInfo            ' [+2.8]
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>   ' [*2.8]  32->16
        Public reserved1 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_RS485_MAX_CHANNELS)>
        Public channels As Suprema.BS2Rs485Channel()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2WiegandFormat
        Public length As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_WIEGAND_MAX_FIELDS * Suprema.BS2Environment.BS2_WIEGAND_FIELD_SIZE)>
        Public idFields As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_WIEGAND_MAX_PARITIES * Suprema.BS2Environment.BS2_WIEGAND_FIELD_SIZE)>
        Public parityFields As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_WIEGAND_MAX_PARITIES)>
        Public parityType As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_WIEGAND_MAX_PARITIES)>
        Public parityPos As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2WiegandConfig
        Public mode As Byte
        Public useWiegandBypass As Byte
        Public useFailCode As Byte
        Public failCode As Byte
        Public outPulseWidth As System.UInt16 '(20 ~ 100 us)
        Public outPulseInterval As System.UInt16 '(200 ~ 20000 us)
        Public formatID As System.UInt32
        Public format As Suprema.BS2WiegandFormat
        Public wiegandInputMask As System.UInt16 '(bitmask , no use 0 postion bit, 1~15 bit)
        Public wiegandCardMask As System.UInt16 '(bitmask , no use 0 postion bit, 1~15 bit)
        Public wiegandCSNIndex As Byte '(1~15)
        Public useWiegandUserID As Byte           ' [+ V2.7.2]
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=26)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2WiegandTamperInput
        Public deviceID As System.UInt32
        Public port As System.UInt16
        Public switchType As Byte
        Public reserved As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2WiegandLedOutput
        Public deviceID As System.UInt32
        Public port As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=10)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2WiegandBuzzerOutput
        Public deviceID As System.UInt32
        Public port As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=34)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2WiegandDeviceConfig
        Public tamper As Suprema.BS2WiegandTamperInput
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_WIEGAND_STATUS_NUM)>
        Public led As Suprema.BS2WiegandLedOutput()
        Public buzzer As Suprema.BS2WiegandBuzzerOutput
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SVInputRange
        Public minValue As System.UInt16 '0 ~ 3300 (0 ~ 3.3v)
        Public maxValue As System.UInt16 '0 ~ 3300 (0 ~ 3.3v)
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SupervisedInputConfig
        Public shortInput As Suprema.BS2SVInputRange
        Public openInput As Suprema.BS2SVInputRange
        Public onInput As Suprema.BS2SVInputRange
        Public offInput As Suprema.BS2SVInputRange
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SupervisedInputConfigSet
        Public portIndex As Byte
        Public enabled As Byte
        Public supervised_index As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=5)>
        Public reserved As Byte()
        Public config As Suprema.BS2SupervisedInputConfig
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BSAuxConfig
        Public _value As System.UInt16

        Public Property tamperAuxIndex As System.UInt16
            Get
                Return CUShort((Me._value And &H03))
            End Get
            Set(value As System.UInt16)
                Me._value = CUShort(((Me._value And Not &H03) Or (value And &H03)))
            End Set
        End Property
        Public Property acFailAuxIndex As System.UInt16
            Get
                Return CUShort(((Me._value And &H30) >> 4))
            End Get
            Set(value As System.UInt16)
                Me._value = CUShort(((Me._value And Not &H30) Or ((value << 4) And &H30)))
            End Set
        End Property
        Public Property aux0Type As System.UInt16
            Get
                Return CUShort(((Me._value And &H100) >> 8))
            End Get
            Set(value As System.UInt16)
                Me._value = CUShort(((Me._value And Not &H100) Or ((value << 8) And &H100)))
            End Set
        End Property
        Public Property aux1Type As System.UInt16
            Get
                Return CUShort(((Me._value And &H200) >> 9))
            End Get
            Set(value As System.UInt16)
                Me._value = CUShort(((Me._value And Not &H200) Or ((value << 9) And &H200)))
            End Set
        End Property
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2InputConfig
        Public numInputs As Byte
        Public numSupervised As Byte
        Public aux As Suprema.BSAuxConfig
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_INPUT_NUM)>
        Public supervised_inputs As Suprema.BS2SupervisedInputConfigSet()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2WlanConfig
        Public enabled As Byte
        Public operationMode As Byte ' BS2WlanOperationModeEnum
        Public authType As Byte ' BS2WlanAuthTypeEnum
        Public encryptionType As Byte ' BS2WlanEncryptionTypeEnum
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_WLAN_SSID_SIZE)>
        Public essid As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_WLAN_KEY_SIZE)>
        Public authKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved As Byte()
    End Structure   

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2EventTrigger
        Public code As System.UInt16
        Public reserved As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2InputTrigger
        Public port As Byte
        Public switchType As Byte
        Public duration As System.UInt16
        Public scheduleID As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ScheduleTrigger
        Public type As System.UInt32
        Public scheduleID As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Trigger
        Public deviceID As System.UInt32
        Public type As Byte
        Public reserved As Byte
        Public ignoreSignalTime As System.UInt16             ' [+ 2.9.6]
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8)>
        Public triggerUnion As Byte() 'BS2XXXXTrigger
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Signal
        Public signalID As System.UInt32
        Public count As System.UInt16
        Public onDuration As System.UInt16
        Public offDuration As System.UInt16
        Public delay As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OutputPortAction
        Public portIndex As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        Public signal As Suprema.BS2Signal
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2RelayAction
        Public relayIndex As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        Public signal As Suprema.BS2Signal
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit, Size:=4)>
    Public Structure BS2ReleaseAlarmUnion
        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public deviceID As System.UInt32

        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public doorID As System.UInt32

        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public zoneID As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ReleaseAlarmAction
        Public targetType As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        Public releaseAlarm As Suprema.BS2ReleaseAlarmUnion
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LedSignal
        Public color As Byte
        Public reserved As Byte
        Public duration As System.UInt16
        Public delay As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LedAction
        Public count As System.UInt16 '(0 = infinite)
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_LED_SIGNAL_NUM)>
        Public signal As Suprema.BS2LedSignal()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2BuzzerSignal
        Public tone As Byte
        Public fadeout As Byte
        Public duration As System.UInt16
        Public delay As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2BuzzerAction
        Public count As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_BUZZER_SIGNAL_NUM)>
        Public signal As Suprema.BS2BuzzerSignal()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DisplayAction
        Public duration As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        Public displayID As System.UInt32
        Public resourceID As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SoundAction
        Public count As Byte
        Public unused As Byte             ' padding      BS2AFW-241
        Public soundIndex As System.UInt16
        Public delay As System.UInt16        
''' <deprecated/>
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public reserved As Byte()
    End Structure

	<System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LiftAction
        Public liftID As System.UInt32
        Public type As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()     ' [+ V2.8]
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Action
        Public deviceID As System.UInt32
        Public type As Byte
        Public stopFlag As Byte
        Public delay As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=24)>
        Public actionUnion As Byte() ' BS2XXXAction  
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2TriggerAction
        Public trigger As Suprema.BS2Trigger
        Public action As Suprema.BS2Action
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2TriggerActionConfig
        Public numItems As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_TRIGGER_ACTION)>
        Public items As Suprema.BS2TriggerAction()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved1 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ImageEventFilter
        Public mainEventCode As Byte
        Public subEventCode As Byte               ' [+ V2.8]
        Public subCodeIncluded As Byte            ' [+ V2.8]
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=1)>
        Public reserved2 As Byte()
        Public scheduleID As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2EventConfig
        Public numImageEventFilter As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_EVENT_MAX_IMAGE_CODE_COUNT)>
        Public imageEventFilter As Suprema.BS2ImageEventFilter()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public unused As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2WiegandInConfig
        Public formatID As System.UInt32
        Public format As Suprema.BS2WiegandFormat
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2WiegandMultiConfig
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.MAX_WIEGAND_IN_COUNT)>
        Public formats As Suprema.BS2WiegandInConfig()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS1CardConfig
        Public magicNo As System.UInt32
        Public disabled As System.UInt32
        Public useCSNOnly As System.UInt32
        Public bioentryCompatible As System.UInt32
        Public useSecondaryKey As System.UInt32
        Public reserved1 As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public primaryKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved2 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public secondaryKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved3 As Byte()
        Public cisIndex As System.UInt32
        Public numOfTemplate As System.UInt32
        Public templateSize As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4)>
        Public templateStartBlock As System.UInt32()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=15)>
        Public reserve4 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SystemConfigExt
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public primarySecureKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public secondarySecureKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved3 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2UserPhoneItem
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public phoneNumber As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DESCRIPTION_NAME_LEN)>
        Public descript As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2VoipConfig
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_URL_SIZE)>
        Public serverUrl As Byte()
        Public serverPort As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public userID As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public userPW As Byte()
        Public exitButton As Byte
        Public dtmfMode As Byte
        Public bUse As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reseverd As Byte()
        Public numPhonBook As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_VOIP_MAX_PHONEBOOK)>
        Public phonebook As Suprema.BS2UserPhoneItem()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FaceWidth
        Public min As System.UInt16
        Public max As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SearchRange
        Public x As System.UInt16
        Public width As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DetectDistance
        Public min As Byte
        Public max As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FaceConfig
        Public securityLevel As Byte
        Public lightCondition As Byte
        Public enrollThreshold As Byte
        Public detectSensitivity As Byte
        Public enrollTimeout As System.UInt16
        Public lfdLevel As Byte
        Public quickEnrollment As Byte			    ' [+ 2.6.4]
        Public previewOption As Byte			        ' [+ 2.6.4]

        Public checkDuplicate As Byte                 ' [+ 2.6.4]
        Public operationMode As Byte                  ' [+ 2.7.1]        FSF2 support
        Public maxRotation As Byte                    ' [+ 2.7.1]        FSF2 support
        Public faceWidth As Suprema.BS2FaceWidth              ' [+ 2.7.1]        FSF2 support
        Public searchRange As Suprema.BS2SearchRange          ' [+ 2.7.1]        FSF2 support
        Public detectDistance As Suprema.BS2DetectDistance	' [+ 2.8.3]        BS3 support
        Public wideSearch As Byte                 	' [+ 2.8.3]        BS3 support

        Public unused As Byte

        Public unableToSaveImageOfVisualFace As Byte  ' [+ 2.9.6]        Save template only
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=13)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Rs485SlaveDeviceEX
        Public deviceID As System.UInt32
        Public deviceType As System.UInt16
        Public enableOSDP As Byte
        Public connected As Byte
        Public channelInfo As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reseverd As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Rs485ChannelEX
        Public baudRate As System.UInt32
        Public channelIndex As Byte
        Public useRegistance As Byte
        Public numOfDevices As Byte
        Public channelType As Byte			        ' + 2.9.1
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_RS485_MAX_SLAVES_PER_CHANNEL)>
        Public slaveDevices As Suprema.BS2Rs485SlaveDeviceEX()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Rs485ConfigEX
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8)>
        Public mode As Byte()
        Public numOfChannels As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reseverd As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved1 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_RS485_MAX_CHANNELS_EX)>
        Public channels As Suprema.BS2Rs485ChannelEX()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SeosCard
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=13)>
        Public oid_ADF As Byte()
        Public size_ADF As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved1 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8)>
        Public oid_DataObjectID As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8)>
        Public size_DataObject As System.UInt16()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public primaryKeyAuth As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public secondaryKeyAuth As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=24)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2CardConfigEx
        Public seos As Suprema.BS2SeosCard        
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=24)>
        Public reserved As Byte()
    End Structure

#Region "DEVICE_ZONE_SUPPORTED"
    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DeviceZoneMasterConfig
        Public enable As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=1)>
        Private reserved1 As Byte()
        Public listenPort As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4)>
        Private reserved As Byte()
    End Structure
#End Region

#Region "ENTRANCE_LIMIT"
    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DeviceZoneEntranceLimitMemberInfo
        Public readerID As System.UInt32
    End Structure
#End Region

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DeviceZoneEntranceLimitMaster
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ZONE_NAME_LEN)>
        Public name As Byte()

        Public type As Byte 'BS2_DEVICE_ZONE_ENTRANCE_LIMIT_TYPE
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Private reserved1 As Byte()

        Public entryLimitInterval_s As System.UInt32

        Public numEntranceLimit As Byte
        Public numReaders As Byte
        Public numAlarm As Byte
        Public numBypassGroups As Byte

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ENTRANCE_LIMIT_PER_ZONE)>
        Public maxEntry As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ENTRANCE_LIMIT_PER_ZONE)>
        Public periodStart_s As System.UInt32()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ENTRANCE_LIMIT_PER_ZONE)>
        Public periodEnd_s As System.UInt32()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_READERS_PER_DEVICE_ZONE_ENTRANCE_LIMIT)>
        Public readers As Suprema.BS2DeviceZoneEntranceLimitMemberInfo()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DEVICE_ZONE_ENTRANCE_LIMIT_ALARM_ACTION)>
        Public alarm As Suprema.BS2Action()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_BYPASS_GROUPS_PER_DEVICE_ZONE_ENTRANCE_LIMIT)>
        Public bypassGroupIDs As System.UInt32()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8 * 4)>
        Private reserved3 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DeviceZoneAGEntranceLimit
        Public zoneID As System.UInt32
        Public numAGEntranceLimit As System.UInt16
        Public reserved1 As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ENTRANCE_LIMIT_PER_ZONE)>
        Public periodStart_s As System.UInt32()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ENTRANCE_LIMIT_PER_ZONE)>
        Public periodEnd_s As System.UInt32()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ENTRANCE_LIMIT_PER_ZONE)>
        Public numEntry As System.UInt16()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ENTRANCE_LIMIT_PER_ZONE * Suprema.BS2Environment.BS2_MAX_ACCESS_GROUP_ENTRANCE_LIMIT_PER_ENTRACE_LIMIT)>
        Public maxEntry As System.UInt16()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ENTRANCE_LIMIT_PER_ZONE * Suprema.BS2Environment.BS2_MAX_ACCESS_GROUP_ENTRANCE_LIMIT_PER_ENTRACE_LIMIT)>
        Public accessGroupID As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DeviceZoneEntranceLimitMember
        Public masterPort As System.UInt16
        Public actionInDisconnect As Byte 'BS2_DEVICE_ZONE_ENTRANCE_LIMIT_DISCONNECTED_ACTION_TYPE
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=1)>
        Private reserved1 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV4_ADDR_SIZE)>
        Public masterIP As Byte()
    End Structure

#Region "FIRE_ALARM"
    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DeviceZoneFireSensor
        Public deviceID As System.UInt32
        Public port As Byte
        Public switchType As Byte     'BS2SwitchTypeEnum
        Public duration As System.UInt16
    End Structure
#End Region

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DeviceZoneFireAlarmMemberInfo
        Public readerID As System.UInt32			
''' </>
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DeviceZoneFireAlarmMaster
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ZONE_NAME_LEN)>
        Public name As Byte()

        Public numReaders As Byte
        Public numAlarm As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Private reserved1 As Byte()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_READERS_PER_DEVICE_ZONE_FIRE_ALARM)>
        Public readers As Suprema.BS2DeviceZoneFireAlarmMemberInfo()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DEVICE_ZONE_FIRE_ALARM_ALARM_ACTION)>
        Public alarm As Suprema.BS2Action()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8 * 40)>
        Private reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DeviceZoneFireAlarmMember
        Public masterPort As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Private reserved1 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV4_ADDR_SIZE)>
        Public masterIP As Byte()

        Public numSensors As Byte
        Public numDoors As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Private reserved2 As Byte()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_FIRE_SENSORS_PER_DEVICE_ZONE_FIRE_ALARM_MEMBER)>
        Public sensor As Suprema.BS2DeviceZoneFireSensor()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DOORS_PER_DEVICE_ZONE_FIRE_ALARM_MEMBER)>
        Public doorIDs As System.UInt32()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=40)>
        Private reserved3 As Byte()
    End Structure

    Public Structure BS2DeviceZone
        Public zoneID As System.UInt32						
''' </>
        Public zoneType As Byte 'BS2_DEVICE_ZONE_TYPE
        Public nodeType As Byte 'BS2_DEVICE_ZONE_NODE_TYPE
        Public enable As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=1)>
        Public reserved As Byte()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=884)>
        Public zoneUnion As Byte()
    End Structure

    Public Structure BS2DeviceZoneConfig
        Public numOfZones As System.Int32 '0 ~ BS_MAX_ZONE_PER_NODE
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DEVICE_ZONE)>
        Public zone As Suprema.BS2DeviceZone()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Configs
        Public configMask As System.UInt32
        Public factoryConfig As Suprema.BS2FactoryConfig
        Public systemConfig As Suprema.BS2SystemConfig
        Public authConfig As Suprema.BS2AuthConfig
        Public statusConfig As Suprema.BS2StatusConfig
        Public displayConfig As Suprema.BS2DisplayConfig
        Public ipConfig As Suprema.BS2IpConfig
        Public ipConfigExt As Suprema.BS2IpConfigExt
        Public tnaConfig As Suprema.BS2TNAConfig
        Public cardConfig As Suprema.BS2CardConfig
        Public fingerprintConfig As Suprema.BS2FingerprintConfig
        Public rs485Config As Suprema.BS2Rs485Config
        Public wiegandConfig As Suprema.BS2WiegandConfig
        Public wiegandDeviceConfig As Suprema.BS2WiegandDeviceConfig
        Public inputConfig As Suprema.BS2InputConfig
        Public wlanConfig As Suprema.BS2WlanConfig
        Public triggerActionConfig As Suprema.BS2TriggerActionConfig
        Public eventConfig As Suprema.BS2EventConfig
        Public wiegandMultiConfig As Suprema.BS2WiegandMultiConfig
        Public card1xConfig As Suprema.BS1CardConfig
        Public systemExtConfig As Suprema.BS2SystemConfigExt
        Public voipConfig As Suprema.BS2VoipConfig
        Public faceConfig As Suprema.BS2FaceConfig        
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DoorRelay
        Public deviceID As System.UInt32
        Public port As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DoorSensor
        Public deviceID As System.UInt32
        Public port As Byte
        Public switchType As Byte
        Public apbUseDoorSensor As Byte	' [+2.7.0]
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=1)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ExitButton
        Public deviceID As System.UInt32
        Public port As Byte
        Public switchType As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DoorStatus
        Public id As System.UInt32
        Public opened As Byte
        Public unlocked As Byte
        Public heldOpened As Byte
        Public unlockFlags As Byte
        Public lockFlags As Byte
        Public alarmFlags As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
        Public lastOpenTime As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=4)>
    Public Structure BS2Door
        Public doorID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DOOR_NAME_LEN)>
        Public name As Byte()
        Public entryDeviceID As System.UInt32
        Public exitDeviceID As System.UInt32
        Public relay As Suprema.BS2DoorRelay
        Public sensor As Suprema.BS2DoorSensor
        Public button As Suprema.BS2ExitButton
        Public autoLockTimeout As System.UInt32
        Public heldOpenTimeout As System.UInt32
        Public instantLock As Byte
        Public unlockFlags As Byte
        Public lockFlags As Byte
        Public unconditionalLock As Byte 'alarmFlags;
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_FORCED_OPEN_ALARM_ACTION)>
        Public forcedOpenAlarm As Suprema.BS2Action()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_HELD_OPEN_ALARM_ACTION)>
        Public heldOpenAlarm As Suprema.BS2Action()
        Public dualAuthScheduleID As System.UInt32
        Public dualAuthDevice As Byte
        Public dualAuthApprovalType As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()         ' [+ V2.8]
        Public dualAuthTimeout As System.UInt32
        Public numDualAuthApprovalGroups As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved2 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DUAL_AUTH_APPROVAL_GROUP)>
        Public dualAuthApprovalGroupID As System.UInt32()
        Public apbZone As Suprema.BS2AntiPassbackZone
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ResourceItem
        Public index As Byte
        Public dataLen As System.UInt32
        Public data As System.IntPtr
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ResourceElement
        Public type As Byte
        Public numResData As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_RESOURCE_ITEM_COUNT)>
        Public resData As Suprema.BS2ResourceItem()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Face
        Public faceIndex As Byte
        Public numOfTemplate As Byte
        Public flag As Byte
        Public reserved As Byte
        Public imageLen As System.UInt16
        Public reserved2 As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_FACE_IMAGE_SIZE)>
        Public imageData As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_TEMPLATE_PER_FACE * Suprema.BS2Environment.BS2_FACE_TEMPLATE_LENGTH)>
        Public templateData As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SimpleDeviceInfo
        Public id As System.UInt32
        Public type As System.UInt16
        Public connectionMode As Byte
        Public ipv4Address As System.UInt32
        Public port As System.UInt16
        Public maxNumOfUser As System.UInt32
        Public userNameSupported As Byte
        Public userPhotoSupported As Byte
        Public pinSupported As Byte
        Public cardSupported As Byte
        Public fingerSupported As Byte
        Public faceSupported As Byte
        Public wlanSupported As Byte
        Public tnaSupported As Byte
        Public triggerActionSupported As Byte
        Public wiegandSupported As Byte
        Public imageLogSupported As Byte
        Public dnsSupported As Byte
        Public jobCodeSupported As Byte        
        Public wiegandMultiSupported As Byte
        Public rs485Mode As Byte
        Public sslSupported As Byte
        Public rootCertExist As Byte
        Public dualIDSupported As Byte
        Public useAlphanumericID As Byte
        Public connectedIP As System.UInt32
        Public phraseCodeSupported As Byte
        Public card1xSupported As Byte
        Public systemExtSupported As Byte
        Public voipSupported As Byte
        Public rs485ExSupported As Byte
        Public cardExSupported As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2SimpleDeviceInfoEx
        Public supported As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2User
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public userID As Byte()
        Public formatVersion As Byte
        Public flag As Byte
        Public version As System.UInt16
        Public numCards As Byte
        Public numFingers As Byte
        Public numFaces As Byte
        Public infoMask As Byte       ' [+V2.8.3]
#If OLD_CODE Then
        public UInt32 fingerChecksum;
#Else
        Public authGroupID As System.UInt32
#End If
        Public faceChecksum As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2UserSetting
        Public startTime As System.UInt32
        Public endTime As System.UInt32
        Public fingerAuthMode As Byte
        Public cardAuthMode As Byte
        Public idAuthMode As Byte
        Public securityLevel As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2UserPhoto
        Public size As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_PHOTO_SIZE)>
        Public data As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2JobData
        Public code As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_JOBLABEL_LEN)>
        Public label As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Job
        Public numJobs As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_JOB_SIZE)>
        Public jobs As Suprema.BS2JobData()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2UserBlob
        Public user As Suprema.BS2User
        Public setting As Suprema.BS2UserSetting
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_NAME_LEN)>
        Public name As Byte()
        Public photo As Suprema.BS2UserPhoto
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_PIN_HASH_SIZE)>
        Public pin As Byte()
        Public cardObjs As System.IntPtr
        Public fingerObjs As System.IntPtr
        Public faceObjs As System.IntPtr        
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ACCESS_GROUP_PER_USER)>
        Public accessGroupId As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2UserBlobEx
        Public user As Suprema.BS2User
        Public setting As Suprema.BS2UserSetting
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_NAME_LEN)>
        Public name As Byte()
        Public photo As Suprema.BS2UserPhoto
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_PIN_HASH_SIZE)>
        Public pin As Byte()
        Public cardObjs As System.IntPtr
        Public fingerObjs As System.IntPtr
        Public faceObjs As System.IntPtr
        Public job As Suprema.BS2Job
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_PHRASE_SIZE)>
        Public phrase As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ACCESS_GROUP_PER_USER)>
        Public accessGroupId As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>     ' [+V2.8.3]
    Public Structure BS2UserStatistic
        Public numUsers As System.UInt32
        Public numCards As System.UInt32
        Public numFingerprints As System.UInt32
        Public numFaces As System.UInt32
        Public numNames As System.UInt32
        Public numImages As System.UInt32
        Public numPhrases As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit, Size:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
    Public Structure BS2EventDetail
        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public doorID As System.UInt32

        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public liftID As System.UInt32

        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public zoneID As System.UInt32

        ' IO
        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public ioDeviceID As System.UInt32
        <System.Runtime.InteropServices.FieldOffsetAttribute(4)>
        Public port As System.UInt16
        <System.Runtime.InteropServices.FieldOffsetAttribute(6)>
        Public value As Byte

        ' Alarm
        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public alarmZoneID As System.UInt32
        <System.Runtime.InteropServices.FieldOffsetAttribute(4)>
        Public alarmDoorID As System.UInt32
        <System.Runtime.InteropServices.FieldOffsetAttribute(8)>
        Public alarmIoDeviceID As System.UInt32
        <System.Runtime.InteropServices.FieldOffsetAttribute(12)>
        Public alarmPort As System.UInt16

        ' Interlock
        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public interlockZoneID As System.UInt32
        <System.Runtime.InteropServices.FieldOffsetAttribute(4)>
        Public interlockDoorID_0 As System.UInt32
        <System.Runtime.InteropServices.FieldOffsetAttribute(8)>
        Public interlockDoorID_1 As System.UInt32
        <System.Runtime.InteropServices.FieldOffsetAttribute(12)>
        Public interlockDoorID_2 As System.UInt32
        <System.Runtime.InteropServices.FieldOffsetAttribute(16)>
        Public interlockDoorID_3 As System.UInt32

        ' RelayAction
        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public relayActionRelayPort As System.UInt16
        <System.Runtime.InteropServices.FieldOffsetAttribute(2)>
        Public relayActionInputPort As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Event
        Public id As System.UInt32
        Public dateTime As System.UInt32
        Public deviceID As System.UInt32

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public userID As Byte()

        Public code As System.UInt16
        Public param As Byte
        Public image As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2EventExtIoDevice
        Public ioDeviceID As System.UInt32
        Public port As System.UInt16
        Public value As Byte
        Public reserved As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2EventExtUnion
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public userID As Byte()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_CARD_DATA_SIZE)>
        Public cardID As Byte()

        Public doorID As System.UInt32
        Public zoneID As System.UInt32
        Public ioDevice As Suprema.BS2EventExtIoDevice
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2EventBlob
        Public eventMask As System.UInt16
        Public id As System.UInt32
        Public info As Suprema.BS2EventExtInfo
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public objectID As Byte() 'BS2EventExtUnion

        Public tnaKey As Byte
        Public jobCode As System.UInt32
        Public imageSize As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_EVENT_MAX_IMAGE_SIZE)>
        Public image As Byte()
        Public reserved As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2EventSmallBlob
        Public eventMask As System.UInt16
        Public id As System.UInt32
        Public info As Suprema.BS2EventExtInfo
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public objectID As Byte() 'BS2EventExtUnion

        Public tnaKey As Byte
        Public jobCode As System.UInt32
        Public imageSize As System.UInt16
        Public imageObj As System.IntPtr
        Public reserved As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2EventSmallBlobEx
        Public eventMask As System.UInt16
        Public id As System.UInt32
        Public info As Suprema.BS2EventExtInfo
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public objectID As Byte() 'BS2EventExtUnion

        Public tnaKey As Byte
        Public jobCode As System.UInt32
        Public imageSize As System.UInt16
        Public imageObj As System.IntPtr
        Public reserved As Byte
	    Public temperature As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2EventExtInfo
        Public dateTime As System.UInt32
        Public deviceID As System.UInt32
        Public code As System.UInt16

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()

    End Structure  

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2UserAccessGroups
        Public numAccessGroups As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ACCESS_GROUP_PER_USER)>
        Public accessGroupID As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit, Size:=1)>
    Public Structure BS2NumOfLevelUnion
        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public numAccessLevels As Byte

        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        Public numFloorLevels As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit, Size:=512)>
    Public Structure BS2LevelUnion
        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ACCESS_LEVEL_PER_ACCESS_GROUP)>
        Public accessLevels As System.UInt32()

        <System.Runtime.InteropServices.FieldOffsetAttribute(0)>
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ACCESS_LEVEL_PER_ACCESS_GROUP)>
        Public floorLevels As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AccessGroup
        Public id As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ACCESS_GROUP_NAME_LEN)>
        Public name As Byte()
        Public numOflevelUnion As Suprema.BS2NumOfLevelUnion
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        Public levelUnion As Suprema.BS2LevelUnion
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DoorSchedule
        Public doorID As System.UInt32
        Public scheduleID As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AccessLevel
        Public id As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ACCESS_LEVEL_NAME_LEN)>
        Public name As Byte()
        Public numDoorSchedules As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ACCESS_LEVEL_ITEMS)>
        Public doorSchedules As Suprema.BS2DoorSchedule()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2TimePeriod
        Public startTime As System.UInt16
        Public endTime As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DaySchedule
        Public numPeriods As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_TIME_PERIODS_PER_DAY)>
        Public periods As Suprema.BS2TimePeriod()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2WeeklySchedule
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_NUM_WEEKDAYS)>
        Public schedule As Suprema.BS2DaySchedule()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DailySchedule
        Public startDate As System.UInt32
        Public numDays As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DAYS_PER_DAILY_SCHEDULE)>
        Public schedule As Suprema.BS2DaySchedule()
    End Structure

    'Discarded
    '    [StructLayout(LayoutKind.Explicit, Size = 2168)]
    '    public struct BS2ScheduleUnion
    '    {   
    '        [FieldOffset(0)]
    '        public BS2DailySchedule daily;
    '
    '        [FieldOffset(0)]
    '        public BS2WeeklySchedule weekly;
    '    }

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2HolidaySchedule
        Public id As System.UInt32
        Public schedule As Suprema.BS2DaySchedule
    End Structure

    'Discarded
    '[StructLayout(LayoutKind.Sequential, Pack = 1)]
    'public struct BS2Schedule
    '{
    '    public UInt32 id;
    '    [MarshalAs(UnmanagedType.ByValArray, SizeConst = BS2Environment.BS2_MAX_SCHEDULE_NAME_LEN)]
    '    public byte[] name;
    '    public byte isDaily;
    '    public byte numHolidaySchedules;
    '    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    '    public byte[] reserved;
    '    public BS2ScheduleUnion scheduleUnion;
    '    [MarshalAs(UnmanagedType.ByValArray, SizeConst = BS2Environment.BS2_MAX_HOLIDAY_GROUPS_PER_SCHEDULE)]
    '    public BS2HolidaySchedule[] holidaySchedules;
    '}

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Holiday
        Public [date] As System.UInt32        
        Public recurrence As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
    End Structure

    Public Structure BS2HolidayGroup
        Public id As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_HOLIDAY_GROUP_NAME_LEN)>
        Public name As Byte()
        Public numHolidays As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_HOLIDAYS_PER_GROUP)>
        Public holidays As Suprema.BS2Holiday()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2BlackList
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_CARD_DATA_SIZE)>
        Public cardID As Byte()
        Public issueCount As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ZoneStatus
        Public id As System.UInt32
        Public status As Byte 'BS2ZoneStatusEnum
        Public disabled As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ApbMember
        Public deviceID As System.UInt32
        Public type As Byte 'BS2APBZoneReaderTypeEnum
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AntiPassbackZone
        Public zoneID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ZONE_NAME_LEN)>
        Public name As Byte()
        Public type As Byte 'BS2APBZoneTypeEnum
        Public numReaders As Byte
        Public numBypassGroups As Byte
        Public disabled As Byte
        Public alarmed As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        Public resetDuration As System.UInt32 'in seconds, 0: no reset
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_APB_ALARM_ACTION)>
        Public alarm As Suprema.BS2Action()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_READERS_PER_APB_ZONE)>
        Public readers As Suprema.BS2ApbMember()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=512)>
        Public reserved2 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_BYPASS_GROUPS_PER_APB_ZONE)>
        Public bypassGroupIDs As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2TimedApbMember
        Public deviceID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2TimedAntiPassbackZone
        Public zoneID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ZONE_NAME_LEN)>
        Public name As Byte()
        Public type As Byte 'BS2TimedAPBZoneTypeEnum
        Public numReaders As Byte
        Public numBypassGroups As Byte
        Public disabled As Byte
        Public alarmed As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        Public resetDuration As System.UInt32 'in seconds, 0: no reset
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_TIMED_APB_ALARM_ACTION)>
        Public alarm As Suprema.BS2Action()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_READERS_PER_TIMED_APB_ZONE)>
        Public readers As Suprema.BS2TimedApbMember()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=320)>
        Public reserved2 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_BYPASS_GROUPS_PER_TIMED_APB_ZONE)>
        Public bypassGroupIDs As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FireSensor
        Public deviceID As System.UInt32
        Public port As Byte
        Public switchType As Byte 'BS2SwitchTypeEnum
        Public duration As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FireAlarmZone
        Public zoneID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ZONE_NAME_LEN)>
        Public name As Byte()
        Public numSensors As Byte
        Public numDoors As Byte
        Public alarmed As Byte
        Public disabled As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_FIRE_SENSORS_PER_FIRE_ALARM_ZONE)>
        Public sensor As Suprema.BS2FireSensor()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_FIRE_ALARM_ACTION)>
        Public alarm As Suprema.BS2Action()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved2 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DOORS_PER_FIRE_ALARM_ZONE)>
        Public doorIDs As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ScheduledLockUnlockZone
        Public zoneID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ZONE_NAME_LEN)>
        Public name As Byte()
        Public lockScheduleID As System.UInt32
        Public unlockScheduleID As System.UInt32
        Public numDoors As Byte
        Public numBypassGroups As Byte
        Public numUnlockGroups As Byte
        Public bidirectionalLock As Byte
        Public disabled As Byte
        Public alarmed As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_SCHEDULED_LOCK_UNLOCK_ALARM_ACTION)>
        Public alarm As Suprema.BS2Action()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved2 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DOORS_IN_SCHEDULED_LOCK_UNLOCK_ZONE)>
        Public doorIDs As System.UInt32()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_BYPASS_GROUPS_IN_SCHEDULED_LOCK_UNLOCK_ZONE)>
        Public bypassGroupIDs As System.UInt32()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_UNLOCK_GROUPS_IN_SCHEDULED_LOCK_UNLOCK_ZONE)>
        Public unlockGroupIDs As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LiftFloors
        Public liftID As System.UInt32
        Public numFloors As System.UInt16
        Public reserved As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=256)>
        Public floorIndices As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LiftLockUnlockZone
        Public zoneID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ZONE_NAME_LEN)>
        Public name As Byte()
        Public unlockScheduleID As System.UInt32
        Public lockScheduleID As System.UInt32
        Public numLifts As Byte
        Public numBypassGroups As Byte
        Public numUnlockGroups As Byte
        Public unused As Byte
        Public disabled As Byte
        Public alarmed As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_LIFT_LOCK_UNLOCK_ALARM_ACTION)>
        Public alarm As Suprema.BS2Action()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved2 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_LIFTS_IN_LIFT_LOCK_UNLOCK_ZONE)>
        Public lifts As Suprema.BS2LiftFloors()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_BYPASS_GROUPS_IN_LIFT_LOCK_UNLOCK_ZONE)>
        Public bypassGroupIDs As System.UInt32()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_UNLOCK_GROUPS_IN_LIFT_LOCK_UNLOCK_ZONE)>
        Public unlockGroupIDs As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DeviceNode
        Public parentDeviceID As System.UInt32
        Public deviceID As System.UInt32
        Public deviceType As System.UInt16
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AuthGroup
        Public id As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_AUTH_GROUP_NAME_LEN)>
        Public name As Byte()        
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved As Byte()        
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FloorSchedule
        Public liftID As System.UInt32
        Public floorIndex As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
        Public scheduleID As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FloorLevel
        Public id As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_FLOOR_LEVEL_NAME_LEN)>
        Public name As Byte()
        Public numFloorSchedules As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_FLOOR_LEVEL_ITEMS)>
        Public floorSchedules As Suprema.BS2FloorSchedule()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FloorStatus
        Public activated As Byte
        Public activateFlags As Byte
        Public deactivateFlags As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LiftFloor
        Public deviceID As System.UInt32
        Public port As Byte
        Public status As Suprema.BS2FloorStatus
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LiftSensor
        Public deviceID As System.UInt32
        Public port As Byte
        Public switchType As Byte
        Public duration As System.UInt16
        Public scheduleID As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LiftAlarm
        Public sensor As Suprema.BS2LiftSensor
        Public action As Suprema.BS2Action
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2Lift
        Public liftID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_LIFT_NAME_LEN)>
        Public name As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DEVICES_ON_LIFT)>
        Public deviceID As System.UInt32()
        Public activateTimeout As System.UInt32
        Public dualAuthTimeout As System.UInt32
        Public numFloors As Byte
        Public numDualAuthApprovalGroups As Byte
        Public dualAuthApprovalType As Byte
        Public tamperOn As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DEVICES_ON_LIFT)>
        Public dualAuthRequired As Byte()
        Public dualAuthScheduleID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_FLOORS_ON_LIFT)>
        Public floor As Suprema.BS2LiftFloor()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DUAL_AUTH_APPROVAL_GROUP_ON_LIFT)>
        Public dualAuthApprovalGroupID As System.UInt32()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ALARMS_ON_LIFT)>
        Public alarm As Suprema.BS2LiftAlarm()
        Public tamper As Suprema.BS2LiftAlarm
        Public alarmFlags As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()

    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LiftStatus
        Public liftID As System.UInt32
        Public numFloors As System.UInt16
        Public alarmFlags As Byte
        Public tamperOn As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_FLOORS_ON_LIFT)>
        Public floors As Suprema.BS2FloorStatus()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure CSP_BS2ScheduleUnion
        Public daily As Suprema.BS2DailySchedule

        Public weekly As Suprema.BS2WeeklySchedule
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure CSP_BS2Schedule
        Public id As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_SCHEDULE_NAME_LEN)>
        Public name As Byte()
        Public isDaily As Byte
        Public numHolidaySchedules As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
        Public scheduleUnion As Suprema.CSP_BS2ScheduleUnion
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_HOLIDAY_GROUPS_PER_SCHEDULE)>
        Public holidaySchedules As Suprema.BS2HolidaySchedule()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure CXX_BS2Schedule
        Public id As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_SCHEDULE_NAME_LEN)>
        Public name As Byte()
        Public isDaily As Byte
        Public numHolidaySchedules As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2168)>       
        Public scheduleUnion As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_HOLIDAY_GROUPS_PER_SCHEDULE)>
        Public holidaySchedules As Suprema.BS2HolidaySchedule()
    End Structure

    Public Class Translator(Of TSource, TOutput)  ': TranslatorBase<TSource, TOutput>
        Public Sub Translate(Of TSource, TOutput)(ByRef src As TSource, ByRef output As TOutput)
            Util.TranslatePrimitive(Of TSource, TOutput)(src, output)
            Try
                Dim type As System.Type = GetType(Translator(Of TSource, TOutput))
                Dim extTranslate As System.Reflection.MethodInfo = type.GetMethod("Translate_", New System.Type() {GetType(TSource).MakeByRefType(), GetType(TOutput).MakeByRefType()})
                If extTranslate IsNot Nothing Then
                    output = CType(extTranslate.Invoke(Me, New Object() {src, output}), TOutput)
                End If

            Finally

            End Try
        End Sub

        Public Function Translate_(ByRef src As Suprema.CSP_BS2Schedule, ByRef output As Suprema.CXX_BS2Schedule) As Suprema.CXX_BS2Schedule
            Util.TranslatePrimitive(Of Suprema.CSP_BS2Schedule, Suprema.CXX_BS2Schedule)(src, output)
            Dim bytes As Byte() = New Byte(-1) {}
            If src.isDaily <> 0 Then
                bytes = Util.StructToBytes(Of Suprema.BS2DailySchedule)(src.scheduleUnion.daily)
            Else
                bytes = Util.StructToBytes(Of Suprema.BS2WeeklySchedule)(src.scheduleUnion.weekly)
            End If
            System.Array.Clear(output.scheduleUnion, 0, output.scheduleUnion.Length)
            System.Array.Copy(bytes, output.scheduleUnion, bytes.Length)

            Return output
        End Function

        Public Function Translate_(ByRef src As Suprema.CXX_BS2Schedule, ByRef output As Suprema.CSP_BS2Schedule) As Suprema.CSP_BS2Schedule
            Util.TranslatePrimitive(Of Suprema.CXX_BS2Schedule, Suprema.CSP_BS2Schedule)(src, output)
            If src.isDaily <> 0 Then
                output.scheduleUnion.daily = Util.BytesToStruct(Of Suprema.BS2DailySchedule)(src.scheduleUnion)
            Else
                output.scheduleUnion.weekly = Util.BytesToStruct(Of Suprema.BS2WeeklySchedule)(src.scheduleUnion)
            End If

            Return output
        End Function
    End Class

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2IntrusionAlarmZone
        Public zoneID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ZONE_NAME_LEN)>
        Public name As Byte()
        Public armDelay As Byte
        Public alarmDelay As Byte
        Public disabled As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=1)>
        Public reserved As Byte()
        Public numReaders As Byte
        Public numInputs As Byte
        Public numOutputs As Byte
        Public numCards As Byte
        Public numDoors As Byte
        Public numGroups As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=10)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2IntrusionAlarmZoneBlob
        Public IntrusionAlarmZone As Suprema.BS2IntrusionAlarmZone
        Public memberObjs As System.IntPtr
        Public inputObjs As System.IntPtr
        Public outputObjs As System.IntPtr
        Public cardObjs As System.IntPtr
        Public doorIDs As System.IntPtr
        Public groupIDs As System.IntPtr

    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AlarmZoneMember
        Public deviceID As System.UInt32
        Public inputType As Byte
        Public operationType As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()

    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AlarmZoneInput
        Public deviceID As System.UInt32
        Public port As Byte
        Public switchType As Byte
        Public duration As System.UInt16
        Public operationType As Byte       
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()

    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AlarmZoneOutput
        Public eventcode As System.UInt16        
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
        Public action As Suprema.BS2Action

    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2WeekTime
        Public year As System.UInt16
        Public month As Byte
        Public ordinal As SByte
        Public weekDay As Byte
        Public hour As Byte
        Public minute As Byte
        Public second As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DstSchedule
        Public startTime As Suprema.BS2WeekTime
        Public endTime As Suprema.BS2WeekTime
        Public timeOffset As System.Int32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DstConfig
        Public numSchedules As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=31)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DST_SCHEDULE)>
        Public schedules As Suprema.BS2DstSchedule()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2EncryptKey
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_ENC_KEY_SIZE)>
        Public key As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved As Byte()
    End Structure
	
	<System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2InterlockZoneInput
        Public deviceID As System.UInt32
        Public port As Byte
        Public switchType As Byte
        Public duration As System.UInt16
        Public operationType As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2InterlockZoneOutput
        Public eventcode As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
        Public action As Suprema.BS2Action
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2InterlockZone
        Public zoneID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ZONE_NAME_LEN)>
        Public name As Byte()
        Public disabled As Byte
        Public numInputs As Byte
        Public numOutputs As Byte
        Public numDoors As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2InterlockZoneBlob
        Public InterlockZone As Suprema.BS2InterlockZone
        Public inputObjs As System.IntPtr
        Public outputObjs As System.IntPtr
        Public doorIDs As System.IntPtr
    End Structure

    '=> [IPv6]
    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=4)>
    Public Structure BS2IPV6Config
        Public useIPV6 As Byte
        Public reserved1 As Byte 	'Not yet apply //useIPV4;
        Public useDhcpV6 As Byte
        Public useDnsV6 As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV6_ADDR_SIZE)>
        Public staticIpAddressV6 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV6_ADDR_SIZE)>
        Public staticGatewayV6 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV6_ADDR_SIZE)>
        Public dnsAddrV6 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV6_ADDR_SIZE)>
        Public serverIpAddressV6 As Byte()
        Public serverPortV6 As System.UInt16
        Public sslServerPortV6 As System.UInt16		
        Public portV6 As System.UInt16
        Public numOfAllocatedAddressV6 As Byte
        Public numOfAllocatedGatewayV6 As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV6_ADDR_SIZE * Suprema.BS2Environment.BS2_MAX_IPV6_ALLOCATED_ADDR)>
        Public allocatedIpAddressV6 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV6_ADDR_SIZE * Suprema.BS2Environment.BS2_MAX_IPV6_ALLOCATED_ADDR)>
        Public allocatedGatewayV6 As Byte()
    End Structure
    '<=

    '=> [IPv6]
    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=4)>
    Public Structure BS2IPv6DeviceInfo
        Public id As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=1)>
        Public reserved As Byte()
        Public bIPv6Mode As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV6_ADDR_SIZE)>
        Public ipv6Address As Byte()
        Public portV6 As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV6_ADDR_SIZE)>	
        Public connectedIPV6 As Byte()	
        Public numOfAllocatedAddressV6 As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_IPV6_ADDR_SIZE * Suprema.BS2Environment.BS2_MAX_IPV6_ALLOCATED_ADDR)>
        Public allocatedIpAddressV6 As Byte() 
    End Structure
    '<=

    'User Small Blob
	<System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=4)>
    Public Structure BS2UserSmallBlob
        Public user As Suprema.BS2User
        Public setting As Suprema.BS2UserSetting
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_NAME_LEN)>
        Public name As Byte()
        Public user_photo_obj As System.IntPtr 'BS2UserPhoto
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_PIN_HASH_SIZE)>
        Public pin As Byte()
        Public cardObjs As System.IntPtr
        Public fingerObjs As System.IntPtr
        Public faceObjs As System.IntPtr        
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ACCESS_GROUP_PER_USER)>
        Public accessGroupId As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=4)>
    Public Structure BS2UserSmallBlobEx
        Public user As Suprema.BS2User
        Public setting As Suprema.BS2UserSetting
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_NAME_LEN)>
        Public name As Byte()
        Public user_photo_obj As System.IntPtr
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_PIN_HASH_SIZE)>
        Public pin As Byte()
        Public cardObjs As System.IntPtr
        Public fingerObjs As System.IntPtr
        Public faceObjs As System.IntPtr
        Public job As Suprema.BS2Job
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_PHRASE_SIZE)>
        Public phrase As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ACCESS_GROUP_PER_USER)>
        Public accessGroupId As System.UInt32()
    End Structure
    '<=

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2UserSettingEx
        Public faceAuthMode As Byte
        Public fingerprintAuthMode As Byte
        Public cardAuthMode As Byte
        Public idAuthMode As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=28)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2TemplateEx
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_FACE_EX_TEMPLATE_SIZE)>
        Public data As Byte()
        Public isIR As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FaceExWarped
        Public faceIndex As Byte
        Public numOfTemplate As Byte
        Public flag As Byte
        Public reserved As Byte
        Public imageLen As System.UInt32
        Public irImageLen As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public unused As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_WARPED_IMAGE_LENGTH)>
        Public imageData As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_WARPED_IR_IMAGE_LENGTH)>
        Public irImageData As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_TEMPLATES_PER_FACE_EX)>
        Public templateEx As Suprema.BS2TemplateEx()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2FaceExUnwarped
        Public faceIndex As Byte
        Public numOfTemplate As Byte
        Public flag As Byte
        Public reserved As Byte
        Public imageLen As System.UInt32
        '[MarshalAs(UnmanagedType.ByValArray, SizeConst = 82808)]
        'public byte[] image;
        'public IntPtr image;
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>         ' + 2.9.6
    Public Structure BS2FaceExTemplateOnly
        Public faceIndex As Byte
        Public numOfTemplate As Byte
        Public flag As Byte
        Public reserved As Byte
        Public imageLen As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2UserFaceExBlob
        Public user As Suprema.BS2User
        Public setting As Suprema.BS2UserSetting
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_NAME_LEN)>
        Public name As Byte()
        Public user_photo_obj As System.IntPtr
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_PIN_HASH_SIZE)>
        Public pin As Byte()
        Public cardObjs As System.IntPtr
        Public fingerObjs As System.IntPtr
        Public faceObjs As System.IntPtr
        Public job As Suprema.BS2Job
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_PHRASE_SIZE)>
        Public phrase As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_ACCESS_GROUP_PER_USER)>
        Public accessGroupId As System.UInt32()

        Public settingEx As Suprema.BS2UserSettingEx
        Public faceExObjs As System.IntPtr
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2BarcodeConfig              ' [+ V2.8]
	    Public useBarcode As Byte
        Public scanTimeout As Byte

        Public bypassData As Byte                 ' [+ V2.8.2.7]
        Public treatAsCSN As Byte                 ' [+ V2.8.2.7]

        Public useVisualBarcode As Byte           ' [+ V2.9.1]
        Public motionSensitivity As Byte          ' [+ V2.9.1]
        Public visualCameraScanTimeout As Byte    ' [+ V2.9.1]

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=9)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
	Public Structure BS2ICExInput                ' [+ V2.8.1]
        Public portIndex As Byte
        Public switchType As Byte
        Public duration As System.UInt16
		
		Public reserved As Byte
		Public supervisedResistor As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
		Public reserved1 As Byte()
		
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=26)>
		Public reserved2 As Byte()
	End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2InputConfigEx          ' [+ V2.8.1]
        Public numInputs As Byte
        Public numSupervised As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=18)>
        Public reserved As Byte()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_INPUT_NUM_EX)>
        Public inputs As Suprema.BS2ICExInput()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=200)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2RACInput                ' [+ V2.8.1]
		Public port As Byte
        Public type As Byte
	    Public mask As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=9)>
		Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2RACRelay                ' [+ V2.8.1]
		Public port As Byte
        Public reserved0 As Byte
        Public disconnEnabled As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=9)>
		Public reserved As Byte()
        
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_RELAY_ACTION_INPUT)>
        Public input As Suprema.BS2RACInput()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2RelayActionConfig      ' [+ V2.8.1]
        Public deviceID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public reserved As Byte()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_RELAY_ACTION)>
        Public relay As Suprema.BS2RACRelay()
        
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=152)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LagacyAuth          ' [+ V2.8]
		Public biometricAuthMask As Byte
        'biomerticOnly: 1;
        'biometricPIN: 1;
        'unused: 6;
		Public cardAuthMask As Byte
        'rdOnly: 1;
        'rdBiometric: 1;
        'rdPIN: 1;
        'rdBiometricOrPIN: 1;
        'rdBiometricPIN: 1;
        'used: 3;
		Public idAuthMask As Byte
		'Biometric: 1;
		'PIN: 1;
		'BiometricOrPIN: 1;
		'BiometricPIN: 1;
		'used: 4;
	End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ExtendedAuth          ' [+ V2.8]
		Public faceAuthMask As System.UInt32
        'faceOnly: 1;
        'faceFingerprint: 1;
        'facePIN: 1;
        'faceFingerprintOrPIN: 1;
        'faceFingerprintPIN: 1;
        'unused: 27;
		Public fingerprintAuthMask As System.UInt32
		'fingerprintOnly: 1;
		'fingerprintFace: 1;
		'fingerprintPIN: 1;
		'fingerprintFaceOrPIN: 1;
		'fingerprintFacePIN: 1;
		'unused: 27;
		Public cardAuthMask As System.UInt32
		'cardOnly: 1;
		'cardFace: 1;
		'cardFingerprint: 1;
		'cardPIN: 1;
		'cardFaceOrFingerprint: 1;
		'cardFaceOrPIN: 1;
		'cardFingerprintOrPIN: 1;
		'cardFaceOrFingerprintOrPIN: 1;
		'cardFaceFingerprint: 1;
		'cardFacePIN: 1;
		'cardFingerprintFace: 1;
		'cardFingerprintPIN: 1;
		'cardFaceOrFingerprintPIN: 1;
		'cardFaceFingerprintOrPIN: 1;
		'cardFingerprintFaceOrPIN: 1;
		'unused: 17;
		Public idAuthMask As System.UInt32
        'idFace: 1;
        'idFingerprint: 1;
        'idPIN: 1;
        'idFaceOrFingerprint: 1;
        'idFaceOrPIN: 1;
        'idFingerprintOrPIN: 1;
        'idFaceOrFingerprintOrPIN: 1;
        'idFaceFingerprint: 1;
        'idFacePIN: 1;
        'idFingerprintFace: 1;
        'idFingerprintPIN: 1;
        'idFaceOrFingerprintPIN: 1;
        'idFaceFingerprintOrPIN: 1;
        'idFingerprintFaceOrPIN: 1;
        'unused: 18;
	End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2AuthSupported
		Public extendedMode As Byte
		Public credentialsMask As Byte
        'card: 1;
        'fingerprint: 1;
        'face: 1;
        'id: 1;
        'pin: 1;
        'reserved: 3;
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
		Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public auth As Byte()
        'public BS2LagacyAuth lagacy;
        'public BS2ExtendedAuth extended;
	End Structure


    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2DeviceCapabilities
	    Public maxUsers As System.UInt32
	    Public maxEventLogs As System.UInt32
	    Public maxImageLogs As System.UInt32
	    Public maxBlacklists As System.UInt32
	    Public maxOperators As System.UInt32
	    Public maxCards As System.UInt32		
''' </>
	    Public maxFaces As System.UInt32		
''' </>
	    Public maxFingerprints As System.UInt32		
''' </>
	    Public maxUserNames As System.UInt32		
''' </>
	    Public maxUserImages As System.UInt32		
''' </>
	    Public maxUserJobs As System.UInt32		
''' </>
	    Public maxUserPhrases As System.UInt32		
''' </>
	    Public maxCardsPerUser As Byte		
''' </>
	    Public maxFacesPerUser As Byte		
''' </>
	    Public maxFingerprintsPerUser As Byte		
''' </>
	    Public maxInputPorts As Byte		
''' </>
	    Public maxOutputPorts As Byte		
''' </>
	    Public maxRelays As Byte			
''' </>
	    Public maxRS485Channels As Byte		
''' </>

	    Public systemSupported As Byte
        'cameraSupported: 1;
        'tamperSupported: 1;
        'wlanSupported: 1;
        'displaySupported: 1;
        'thermalSupported: 1;
        'maskSupported: 1;
        'faceExSupported: 1;
        'unused: 1;
	    'voipExSupported: 1;
	    Public cardSupportedMask As System.UInt32
        'EM: 1;
        'HIDProx: 1;
        'MifareFelica: 1;
        'iClass: 1;
        'ClassicPlus: 1;
        'DesFireEV1: 1;
        'SRSE: 1;
        'SEOS: 1;
        'NFC: 1;
        'BLE: 1;
        'reserved: 21;
        'useCardOperation: 1;
	    Public authSupported As Suprema.BS2AuthSupported
	    Public functionSupported As Byte
        'intelligentPDSupported: 1;
   	    'updateUserSupported: 1;
	    'simulatedUnlockSupported: 1;
        'smartCardByteOrderSupported: 1;
        'treatAsCSNSupported: 1;
        'rtspSupported: 1;
        'lfdSupported: 1;
        'visualQRSupported: 1;

	    Public maxVoipExtensionNumbers As Byte        ' [+V2.8.3]

        Public functionSupported2 As Byte             ' [+V2.9.1]
        'osdpStandardCentralSupported : 1;
        'enableLicenseFuncSupported : 1;
        'keypadBacklightSupported : 1              // [+V2.9.4]
        'uzWirelessLockDoorSupported : 1
        'customSmartCardSupported : 1
        'tomSupported : 1
       	'tomEnrollSupported: 1;
	    'showOsdpResultbyLED: 1;

        Public functionSupported3 As Byte             ' [+ 2.9.6]
  	    'customSmartCardFelicaSupported: 1;
	    'ignoreInputAfterWiegandOut: 1;
	    'setSlaveBaudrateSupported: 1;
        'rtspResolutionChangeSupported: 1;
        'voipResolutionChangeSupported: 1;
        'voipTransportChangeSupported: 1;
        'authMsgUserInfoSupported: 1;
        'scrambleKeyboardModeSupported: 1;

        Public visualFaceTemplateVersion As System.UInt16    ' [+ 2.9.6]

        Public functionSupported4 As Byte             ' [+ 2.9.8]
        'authDenyMaskSupported: 1;

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=430)>
	    Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2VoipConfigExtOutboundProxy
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_URL_SIZE)>
        Public address As Byte()
        Public port As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2ExtensionNumber
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public phoneNumber As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_DESCRIPTION_NAME_LEN)>
        Public description As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2VoipConfigExtVolume
        Public speaker As Byte
        Public mic As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2VoipConfigExt
        Public enabled As Byte
        Public useOutboundProxy As Byte
        Public registrationDuration As System.UInt16

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_URL_SIZE)>
        Public address As Byte()
        Public port As System.UInt16
        Public volume As Suprema.BS2VoipConfigExtVolume

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public id As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public password As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public authorizationCode As Byte()

        Public outboundProxy As Suprema.BS2VoipConfigExtOutboundProxy

        Public exitButton As Byte
        Public reserved1 As Byte
        Public numPhoneBook As Byte
        Public showExtensionNumber As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_VOIP_MAX_PHONEBOOK_EXT)>
        Public phonebook As Suprema.BS2ExtensionNumber()
        Public resolution As Byte                                     ' [+ 2.9.8]
        Public transport As Byte                                      ' [+ 2.9.8]
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=30)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2RtspConfig
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public id As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public password As Byte()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_URL_SIZE)>
        Public address As Byte()
        Public port As System.UInt16

        Public enabled As Byte
        Public reserved As Byte
        Public resolution As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=31)>
        Public reserved2 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardLedAction
        Public use As Byte
        Public readerNumber As Byte
        Public ledNumber As Byte

        Public tempCommand As Byte
        Public tempOnTime As Byte
        Public tempOffTime As Byte
        Public tempOnColor As Byte
        Public tempOffColor As Byte
        Public tempRunTime As System.UInt16

        Public permCommand As Byte
        Public permOnTime As Byte
        Public permOffTime As Byte
        Public permOnColor As Byte
        Public permOffColor As Byte

        Public reserved As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardBuzzerAction
        Public use As Byte
        Public readerNumber As Byte
        Public tone As Byte
        Public onTime As Byte
        Public offTime As Byte
        Public numOfCycle As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardAction
        Public actionType As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public led As Suprema.BS2OsdpStandardLedAction()
        Public buzzer As Suprema.BS2OsdpStandardBuzzerAction
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardActionConfig
        Public version As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_OSDP_STANDARD_ACTION_MAX_COUNT)>
        Public actions As Suprema.BS2OsdpStandardAction()
    End Structure

	<System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
	Public Structure BS2OsdpStandardDeviceNotify
        Public deviceID As System.UInt32
        Public deviceType As System.UInt16
        Public enableOSDP As Byte
        Public connected As Byte
        Public channelInfo As Byte
        Public osdpID As Byte
        Public supremaSearch As Byte
        Public activate As Byte
        Public useSecure As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public vendorCode As Byte()
        Public fwVersion As System.UInt32
        Public modelNumber As Byte
        Public modelVersion As Byte
        Public readInfo As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=5)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardDevice
        Public deviceID As System.UInt32
        Public deviceType As System.UInt16
        Public enableOSDP As Byte
        Public connected As Byte
        Public channelInfo As Byte
        Public osdpID As Byte
        Public supremaSearch As Byte
        Public activate As Byte
        Public useSecure As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public vendorCode As Byte()
        Public fwVersion As System.UInt32
        Public modelNumber As Byte
        Public modelVersion As Byte
        Public readInfo As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=25)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardChannel
        Public baudRate As System.UInt32
        Public channelIndex As Byte
        Public useRegistance As Byte
        Public numOfDevices As Byte
        Public channelType As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_RS485_MAX_SLAVES_PER_CHANNEL)>
        Public slaveDevices As Suprema.BS2OsdpStandardDevice()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2License                                        ' + 2.9.1
        Public index As Byte
        Public hasCapability As Byte
        Public enable As Byte
        Public reserved As Byte
        Public licenseType As System.UInt16
        Public licenseSubType As System.UInt16
        Public enableTime As System.UInt32
        Public expiredTime As System.UInt32
        Public issueNumber As System.UInt32

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_USER_ID_SIZE)>
        Public name As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LicenseConfig                                  ' + 2.9.1
        Public version As Byte
        Public numOfLicense As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_MAX_LICENSE_COUNT)>
        Public license As Suprema.BS2License()

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public reserved1 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LicenseBlob                                    ' + 2.9.1
        Public licenseType As System.UInt16
        Public numOfDevices As System.UInt16
        Public deviceIDObjs As System.IntPtr
        Public licenseLen As System.UInt32
        Public licenseObj As System.IntPtr
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2LicenseResult                                  ' + 2.9.1
        Public deviceID As System.UInt32
        Public status As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardDeviceResult                       ' + 2.9.1
        Public deviceID As System.UInt32
        Public result As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardDeviceSecurityKey                        ' + 2.9.1
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_OSDP_STANDARD_KEY_SIZE)>
        Public key As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardDeviceCapabilityItem
        Public compliance As Byte
        Public count As Byte
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardDeviceCapability
        Public input As Suprema.BS2OsdpStandardDeviceCapabilityItem
        Public output As Suprema.BS2OsdpStandardDeviceCapabilityItem
        Public led As Suprema.BS2OsdpStandardDeviceCapabilityItem
        Public audio As Suprema.BS2OsdpStandardDeviceCapabilityItem
        Public textOutput As Suprema.BS2OsdpStandardDeviceCapabilityItem
        Public reader As Suprema.BS2OsdpStandardDeviceCapabilityItem

        Public recvBufferSize As System.UInt16
        Public largeMsgSize As System.UInt16

        Public osdpVersion As Byte
        Public cardFormat As Byte
        Public timeKeeping As Byte
        Public canCommSecure As Byte

        Public crcSupport As Byte
        Public smartCardSupport As Byte
        Public biometricSupport As Byte
        Public securePinEntrySupport As Byte

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardDeviceAdd
        Public osdpID As Byte
        Public activate As Byte
        Public useSecureSession As Byte
        Public deviceType As Byte
        Public deviceID As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardDeviceUpdate
        Public osdpID As Byte
        Public activate As Byte
        Public useSecureSession As Byte
        Public deviceType As Byte
        Public deviceID As System.UInt32
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardChannelInfo
        Public channelIndex As Byte
        Public channelType As Byte
        Public maxOsdpDevice As Byte
        Public numOsdpAvailableDevice As Byte

        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8)>
        Public deviceIDs As System.UInt32()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardDeviceAvailable
        Public numOfChannel As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_RS485_MAX_CHANNELS_EX)>
        Public channels As Suprema.BS2OsdpStandardChannelInfo()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved1 As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2OsdpStandardConfig
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_RS485_MAX_CHANNELS_EX)>
        Public mode As Byte()
        Public numOfChannels As System.UInt16
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=32)>
        Public reserved1 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=Suprema.BS2Environment.BS2_RS485_MAX_CHANNELS_EX)>
        Public channels As Suprema.BS2OsdpStandardChannel()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2CustomMifareCard
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public primaryKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved1 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=6)>
        Public secondaryKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved2 As Byte()
        Public startBlockIndex As System.UInt16
        Public dataSize As Byte
        Public skipBytes As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4)>
        Public reserved As Byte()
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2CustomDesFireCard
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public primaryKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=16)>
        Public secondaryKey As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public appID As Byte()
        Public fileID As Byte
        Public encryptionType As Byte                 ' 0: DES/3DES, 1: AES
        Public operationMode As Byte                  ' 0: legacy(use picc master key), 1: new mode(use app master, file read, file write key)
        Public dataSize As Byte
        Public skipBytes As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=4)>
        Public reserved As Byte()
        Public desfireAppKey As Suprema.BS2DesFireAppLevelKey
    End Structure

    <System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=1)>
    Public Structure BS2CustomCardConfig
        Public dataType As Byte
        Public useSecondaryKey As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=2)>
        Public reserved1 As Byte()

        Public mifare As Suprema.BS2CustomMifareCard
        Public desfire As Suprema.BS2CustomDesFireCard
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=24)>
        Public reserved2 As Byte()
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=96)>
        Public reserved3 As Byte()

        Public smartCardByteOrder As Byte
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)>
        Public reserved4 As Byte()
        Public formatID As System.UInt32
        <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=8)>
        Public reserved5 As Byte()
    End Structure
End Namespace
