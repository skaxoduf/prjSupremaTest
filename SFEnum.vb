Imports System

Namespace Suprema
    <Flags>
    Public Enum BS2ErrorCode
        BS_SDK_SUCCESS = 1
        BS_SDK_DURESS_SUCCESS = 2
        BS_SDK_FIRST_AUTH_SUCCESS = 3
        BS_SDK_SECOND_AUTH_SUCCESS = 4
        BS_SDK_DUAL_AUTH_SUCCESS = 5
        BS_SDK_WIEGAND_BYPASS_SUCCESS = 11
        BS_SDK_ANONYMOUS_SUCCESS = 12

        ' Driver errors
        BS_SDK_ERROR_FROM_DEVICE_DRIVER = -1

        ' Communication errors
        BS_SDK_ERROR_CANNOT_OPEN_SOCKET = -101
        BS_SDK_ERROR_CANNOT_CONNECT_SOCKET = -102
        BS_SDK_ERROR_CANNOT_LISTEN_SOCKET = -103
        BS_SDK_ERROR_CANNOT_ACCEPT_SOCKET = -104
        BS_SDK_ERROR_CANNOT_READ_SOCKET = -105
        BS_SDK_ERROR_CANNOT_WRITE_SOCKET = -106
        BS_SDK_ERROR_SOCKET_IS_NOT_CONNECTED = -107
        BS_SDK_ERROR_SOCKET_IS_NOT_OPEN = -108
        BS_SDK_ERROR_SOCKET_IS_NOT_LISTENED = -109
        BS_SDK_ERROR_SOCKET_IN_PROGRESS = -110
        '=> [IPv6]
        BS_SDK_ERROR_IPV4_IS_NOT_ENABLE = -111
        BS_SDK_ERROR_IPV6_IS_NOT_ENABLE = -112
        BS_SDK_ERROR_NOT_SUPPORTED_SPECIFIED_DEVICE_INFO = -113
        BS_SDK_ERROR_NOT_ENOUGTH_BUFFER = -114
        BS_SDK_ERROR_NOT_SUPPORTED_IPV6 = -115
        BS_SDK_ERROR_INVALID_ADDRESS = -116
        '<=

        ' Packet errors
        BS_SDK_ERROR_INVALID_PARAM = -200
        BS_SDK_ERROR_INVALID_PACKET = -201
        BS_SDK_ERROR_INVALID_DEVICE_ID = -202
        BS_SDK_ERROR_INVALID_DEVICE_TYPE = -203
        BS_SDK_ERROR_PACKET_CHECKSUM = -204
        BS_SDK_ERROR_PACKET_INDEX = -205
        BS_SDK_ERROR_PACKET_COMMAND = -206
        BS_SDK_ERROR_PACKET_SEQUENCE = -207
        BS_SDK_ERROR_NO_PACKET = -209
        BS_SDK_ERROR_INVALID_CODE_SIGN = -210

        'Fingerprint errors
        BS_SDK_ERROR_EXTRACTION_FAIL = -300
        BS_SDK_ERROR_VERIFY_FAIL = -301
        BS_SDK_ERROR_IDENTIFY_FAIL = -302
        BS_SDK_ERROR_IDENTIFY_TIMEOUT = -303
        BS_SDK_ERROR_FINGERPRINT_CAPTURE_FAIL = -304
        BS_SDK_ERROR_FINGERPRINT_SCAN_TIMEOUT = -305
        BS_SDK_ERROR_FINGERPRINT_SCAN_CANCELLED = -306
        BS_SDK_ERROR_NOT_SAME_FINGERPRINT = -307
        BS_SDK_ERROR_EXTRACTION_LOW_QUALITY = -308
        BS_SDK_ERROR_CAPTURE_LOW_QUALITY = -309
        BS_SDK_ERROR_CANNOT_FIND_FINGERPRINT = -310
        BS_SDK_ERROR_NO_FINGER_DETECTED = BS_SDK_ERROR_FINGERPRINT_CAPTURE_FAIL
        BS_SDK_ERROR_FAKE_FINGER_DETECTED = -311
        BS_SDK_ERROR_FAKE_FINGER_TRY_AGAIN = -312
        BS_SDK_ERROR_FAKE_FINGER_SENSOR_ERROR = -313
        BS_SDK_ERROR_CANNOT_FIND_FACE = -314
        BS_SDK_ERROR_FACE_CAPTURE_FAIL = -315
        BS_SDK_ERROR_FACE_SCAN_TIMEOUT = -316
        BS_SDK_ERROR_FACE_SCAN_CANCELLED = -317
        BS_SDK_ERROR_FACE_SCAN_FAILED = -318
        BS_SDK_ERROR_NO_FACE_DETECTED = BS_SDK_ERROR_FACE_CAPTURE_FAIL
        BS_SDK_ERROR_UNMASKED_FACE_DETECTED = -319
        BS_SDK_ERROR_FAKE_FACE_DETECTED = -320
        BS_SDK_ERROR_CANNOT_ESTIMATE = -321
        BS_SDK_ERROR_NORMALIZE_FACE = -322
        BS_SDK_ERROR_SMALL_DETECTION = -323
        BS_SDK_ERROR_LARGE_DETECTION = -324
        BS_SDK_ERROR_BIASED_DETECTION = -325
        BS_SDK_ERROR_ROTATED_FACE = -326
        BS_SDK_ERROR_OVERLAPPED_FACE = -327
        BS_SDK_ERROR_UNOPENED_EYES = -328
        BS_SDK_ERROR_NOT_LOOKING_FRONT = -329
        BS_SDK_ERROR_OCCLUDED_MOUTH = -330
        BS_SDK_ERROR_MATCH_FAIL = -331
        BS_SDK_ERROR_INCOMPATIBLE_FACE = -332     ' [+V2.8.3]


        'File I/O errors
        BS_SDK_ERROR_CANNOT_OPEN_DIR = -400
        BS_SDK_ERROR_CANNOT_OPEN_FILE = -401
        BS_SDK_ERROR_CANNOT_WRITE_FILE = -402
        BS_SDK_ERROR_CANNOT_SEEK_FILE = -403
        BS_SDK_ERROR_CANNOT_READ_FILE = -404
        BS_SDK_ERROR_CANNOT_GET_STAT = -405
        BS_SDK_ERROR_CANNOT_GET_SYSINFO = -406
        BS_SDK_ERROR_DATA_MISMATCH = -407
        BS_SDK_ERROR_ALREADY_OPEN_DIR = -408

        ' I/O errors
        BS_SDK_ERROR_INVALID_RELAY = -500
        BS_SDK_ERROR_CANNOT_WRITE_IO_PACKET = -501
        BS_SDK_ERROR_CANNOT_READ_IO_PACKET = -502
        BS_SDK_ERROR_CANNOT_READ_INPUT = -503
        BS_SDK_ERROR_READ_INPUT_TIMEOUT = -504
        BS_SDK_ERROR_CANNOT_ENABLE_INPUT = -505
        BS_SDK_ERROR_CANNOT_SET_INPUT_DURATION = -506
        BS_SDK_ERROR_INVALID_PORT = -507
        BS_SDK_ERROR_INVALID_INTERPHONE_TYPE = -508
        BS_SDK_ERROR_INVALID_LCD_PARAM = -510
        BS_SDK_ERROR_CANNOT_WRITE_LCD_PACKET = -511
        BS_SDK_ERROR_CANNOT_READ_LCD_PACKET = -512
        BS_SDK_ERROR_INVALID_LCD_PACKET = -513
        BS_SDK_ERROR_INPUT_QUEUE_FULL = -520
        BS_SDK_ERROR_WIEGAND_QUEUE_FULL = -521
        BS_SDK_ERROR_MISC_INPUT_QUEUE_FULL = -522
        BS_SDK_ERROR_WIEGAND_DATA_QUEUE_FULL = -523
        BS_SDK_ERROR_WIEGAND_DATA_QUEUE_EMPTY = -524

        'Util errors
        BS_SDK_ERROR_NOT_SUPPORTED = -600
        BS_SDK_ERROR_TIMEOUT = -601
        BS_SDK_ERROR_CANNOT_SET_TIME = -602

        'Database errors
        BS_SDK_ERROR_INVALID_DATA_FILE = -700
        BS_SDK_ERROR_TOO_LARGE_DATA_FOR_SLOT = -701
        BS_SDK_ERROR_INVALID_SLOT_NO = -702
        BS_SDK_ERROR_INVALID_SLOT_DATA = -703
        BS_SDK_ERROR_CANNOT_INIT_DB = -704
        BS_SDK_ERROR_DUPLICATE_ID = -705
        BS_SDK_ERROR_USER_FULL = -706
        BS_SDK_ERROR_DUPLICATE_TEMPLATE = -707
        BS_SDK_ERROR_FINGERPRINT_FULL = -708
        BS_SDK_ERROR_DUPLICATE_CARD = -709
        BS_SDK_ERROR_CARD_FULL = -710
        BS_SDK_ERROR_NO_VALID_HDR_FILE = -711
        BS_SDK_ERROR_INVALID_LOG_FILE = -712
        BS_SDK_ERROR_CANNOT_FIND_USER = -714
        BS_SDK_ERROR_ACCESS_LEVEL_FULL = -715
        BS_SDK_ERROR_INVALID_USER_ID = -716
        BS_SDK_ERROR_BLACKLIST_FULL = -717
        BS_SDK_ERROR_USER_NAME_FULL = -718
        BS_SDK_ERROR_USER_IMAGE_FULL = -719
        BS_SDK_ERROR_USER_IMAGE_SIZE_TOO_BIG = -720
        BS_SDK_ERROR_SLOT_DATA_CHECKSUM = -721
        BS_SDK_ERROR_CANNOT_UPDATE_FINGERPRINT = -722
        BS_SDK_ERROR_TEMPLATE_FORMAT_MISMATCH = -723
        BS_SDK_ERROR_NO_ADMIN_USER = -724
        BS_SDK_ERROR_CANNOT_FIND_LOG = -725
        BS_SDK_ERROR_DOOR_SCHEDULE_FULL = -726
        BS_SDK_ERROR_DB_SLOT_FULL = -727
        BS_SDK_ERROR_ACCESS_GROUP_FULL = -728
        BS_SDK_ERROR_FLOOR_LEVEL_FULL = -729
        BS_SDK_ERROR_ACCESS_SCHEDULE_FULL = -730
        BS_SDK_ERROR_HOLIDAY_GROUP_FULL = -731
        BS_SDK_ERROR_HOLIDAY_FULL = -732
        BS_SDK_ERROR_TIME_PERIOD_FULL = -733
        BS_SDK_ERROR_NO_CREDENTIAL = -734
        BS_SDK_ERROR_NO_BIOMETRIC_CREDENTIAL = -735
        BS_SDK_ERROR_NO_CARD_CREDENTIAL = -736
        BS_SDK_ERROR_NO_PIN_CREDENTIAL = -737
        BS_SDK_ERROR_NO_BIOMETRIC_PIN_CREDENTIAL = -738
        BS_SDK_ERROR_NO_USER_NAME = -739
        BS_SDK_ERROR_NO_USER_IMAGE = -740
        BS_SDK_ERROR_READER_FULL = -741
        BS_SDK_ERROR_CACHE_MISSED = -742
        BS_SDK_ERROR_OPERATOR_FULL = -743
        BS_SDK_ERROR_INVALID_LINK_ID = -744
        BS_SDK_ERROR_TIMER_CANCELED = -745
        BS_SDK_ERROR_USER_JOB_FULL = -746
        BS_SDK_ERROR_CANNOT_UPDATE_FACE = -747
        BS_SDK_ERROR_FACE_FULL = -748
        BS_SDK_ERROR_FLOOR_SCHEDULE_FULL = -749
        BS_SDK_ERROR_CANNOT_FIND_AUTH_GROUP = -750
        BS_SDK_ERROR_AUTH_GROUP_FULL = -751
        BS_SDK_ERROR_USER_PHRASE_FULL = -752
        BS_SDK_ERROR_DST_SCHEDULE_FULL = -753
        BS_SDK_ERROR_CANNOT_FIND_DST_SCHEDULE = -754
        BS_SDK_ERROR_INVALID_SCHEDULE = -755
        BS_SDK_ERROR_CANNOT_FIND_OPERATOR = -756
        BS_SDK_ERROR_DUPLICATE_FINGERPRINT = -757
        BS_SDK_ERROR_DUPLICATE_FACE = -758
        BS_SDK_ERROR_NO_FACE_CREDENTIAL = -759
        BS_SDK_ERROR_NO_FINGERPRINT_CREDENTIAL = -760
        BS_SDK_ERROR_NO_FACE_PIN_CREDENTIAL = -761
        BS_SDK_ERROR_NO_FINGERPRINT_PIN_CREDENTIAL = -762
        BS_SDK_ERROR_USER_IMAGE_EX_FULL = -763

        'Config errors
        BS_SDK_ERROR_INVALID_CONFIG = -800
        BS_SDK_ERROR_CANNOT_OPEN_CONFIG_FILE = -801
        BS_SDK_ERROR_CANNOT_READ_CONFIG_FILE = -802
        BS_SDK_ERROR_INVALID_CONFIG_FILE = -803
        BS_SDK_ERROR_INVALID_CONFIG_DATA = -804
        BS_SDK_ERROR_CANNOT_WRITE_CONFIG_FILE = -805
        BS_SDK_ERROR_INVALID_CONFIG_INDEX = -806

        'Device errors
        BS_SDK_ERROR_CANNOT_SCAN_FINGER = -900
        BS_SDK_ERROR_CANNOT_SCAN_CARD = -901
        BS_SDK_ERROR_CANNOT_OPEN_RTC = -902
        BS_SDK_ERROR_CANNOT_SET_RTC = -903
        BS_SDK_ERROR_CANNOT_GET_RTC = -904
        BS_SDK_ERROR_CANNOT_SET_LED = -905
        BS_SDK_ERROR_CANNOT_OPEN_DEVICE_DRIVER = -906
        BS_SDK_ERROR_CANNOT_FIND_DEVICE = -907
        BS_SDK_ERROR_CANNOT_SCAN_FACE = -908
        BS_SDK_ERROR_SLAVE_FULL = -910
        BS_SDK_ERROR_CANNOT_ADD_DEVICE = -911
        BS_SDK_ERROR_SLAVE_NOT_READY = -912     ' SetSlaveBaudrate

        'Door errors
        BS_SDK_ERROR_CANNOT_FIND_DOOR = -1000
        BS_SDK_ERROR_DOOR_FULL = -1001
        BS_SDK_ERROR_CANNOT_LOCK_DOOR = -1002
        BS_SDK_ERROR_CANNOT_UNLOCK_DOOR = -1003
        BS_SDK_ERROR_CANNOT_RELEASE_DOOR = -1004
        BS_SDK_ERROR_CANNOT_FIND_LIFT = -1005
        BS_SDK_ERROR_LIFT_FULL = -1006

        'Access control errors
        BS_SDK_ERROR_ACCESS_RULE_VIOLATION = -1100
        BS_SDK_ERROR_DISABLED = -1101
        BS_SDK_ERROR_NOT_YET_VALID = -1102
        BS_SDK_ERROR_EXPIRED = -1103
        BS_SDK_ERROR_BLACKLIST = -1104
        BS_SDK_ERROR_CANNOT_FIND_ACCESS_GROUP = -1105
        BS_SDK_ERROR_CANNOT_FIND_ACCESS_LEVEL = -1106
        BS_SDK_ERROR_CANNOT_FIND_ACCESS_SCHEDULE = -1107
        BS_SDK_ERROR_CANNOT_FIND_HOLIDAY_GROUP = -1108
        BS_SDK_ERROR_CANNOT_FIND_BLACKLIST = -1109
        BS_SDK_ERROR_AUTH_TIMEOUT = -1110
        BS_SDK_ERROR_DUAL_AUTH_TIMEOUT = -1111
        BS_SDK_ERROR_INVALID_AUTH_MODE = -1112
        BS_SDK_ERROR_AUTH_UNEXPECTED_USER = -1113
        BS_SDK_ERROR_AUTH_UNEXPECTED_CREDENTIAL = -1114
        BS_SDK_ERROR_DUAL_AUTH_FAIL = -1115
        BS_SDK_ERROR_BIOMETRIC_AUTH_REQUIRED = -1116
        BS_SDK_ERROR_CARD_AUTH_REQUIRED = -1117
        BS_SDK_ERROR_PIN_AUTH_REQUIRED = -1118
        BS_SDK_ERROR_BIOMETRIC_OR_PIN_AUTH_REQUIRED = -1119
        BS_SDK_ERROR_TNA_CODE_REQUIRED = -1120
        BS_SDK_ERROR_AUTH_SERVER_MATCH_REFUSAL = -1121
        BS_SDK_ERROR_CANNOT_FIND_FLOOR_LEVEL = -1122
        BS_SDK_ERROR_AUTH_FAIL = -1123
        BS_SDK_ERROR_AUTH_GROUP_REQUIRED = -1124
        BS_SDK_ERROR_IDENTIFICATION_REQUIRED = -1125
        BS_SDK_ERROR_ANTI_TAILGATE_VIOLATION = -1126
        BS_SDK_ERROR_HIGH_TEMPERATURE_VIOLATION = -1127
        BS_SDK_ERROR_CANNOT_MEASURE_TEMPERATURE = -1128
        BS_SDK_ERROR_UNMASKED_FACE_VIOLATION = -1129

        ' Required (Fingerprint/Face/PIN/Mask/Thermal ...)
        BS_SDK_MASK_CHECK_REQUIRED = -1130
        BS_SDK_THERMAL_CHECK_REQUIRED = -1131
        BS_SDK_FACE_AUTH_REQUIRED = -1132
        BS_SDK_FINGERPRINT_AUTH_REQUIRED = -1133
        BS_SDK_FACE_OR_PIN_AUTH_REQUIRED = -1134
        BS_SDK_FINGERPRINT_OR_PIN_AUTH_REQUIRED = -1135

        'Zone errors
        BS_SDK_ERROR_CANNOT_FIND_ZONE = -1200
        <Obsolete>
        BS_SDK_ERROR_ZONE_FULL = -1201
        BS_SDK_ERROR_SET_ZONE = -1201
        BS_SDK_ERROR_HARD_APB_VIOLATION = -1202
        BS_SDK_ERROR_SOFT_APB_VIOLATION = -1203
        BS_SDK_ERROR_HARD_TIMED_APB_VIOLATION = -1204
        BS_SDK_ERROR_SOFT_TIMED_APB_VIOLATION = -1205
        BS_SDK_ERROR_SCHEDULED_LOCK_VIOLATION = -1206
        <Obsolete>
        BS_SDK_ERROR_SCHEDULED_UNLOCK_VIOLATION = -1207
        BS_SDK_ERROR_INTRUSION_ALARM_VIOLATION = -1207
        <Obsolete>
        BS_SDK_ERROR_SET_FIRE_ALARM = -1208
        BS_SDK_ERROR_APB_ZONE_FULL = -1208
        BS_SDK_ERROR_TIMED_APB_ZONE_FULL = -1209
        BS_SDK_ERROR_FIRE_ALARM_ZONE_FULL = -1210
        BS_SDK_ERROR_SCHEDULED_LOCK_UNLOCK_ZONE_FULL = -1211
        BS_SDK_ERROR_INACTIVE_ZONE = -1212
        BS_SDK_ERROR_INTRUSION_ALARM_ZONE_FULL = -1213
        BS_SDK_ERROR_CANNOT_ARM = -1214
        BS_SDK_ERROR_CANNOT_DISARM = -1215
        BS_SDK_ERROR_CANNOT_FIND_ARM_CARD = -1216
        BS_SDK_ERROR_HARD_ENTRANCE_LIMIT_COUNT_VIOLATION = -1217
        BS_SDK_ERROR_SOFT_ENTRANCE_LIMIT_COUNT_VIOLATION = -1218
        BS_SDK_ERROR_HARD_ENTRANCE_LIMIT_TIME_VIOLATION = -1219
        BS_SDK_ERROR_SOFT_ENTRANCE_LIMIT_TIME_VIOLATION = -1220
        BS_SDK_ERROR_INTERLOCK_ZONE_DOOR_VIOLATION = -1221
        BS_SDK_ERROR_INTERLOCK_ZONE_INPUT_VIOLATION = -1222
        BS_SDK_ERROR_INTERLOCK_ZONE_FULL = -1223
        BS_SDK_ERROR_AUTH_LIMIT_SCHEDULE_VIOLATION = -1224
        BS_SDK_ERROR_AUTH_LIMIT_COUNT_VIOLATION = -1225
        BS_SDK_ERROR_AUTH_LIMIT_USER_VIOLATION = -1226
        BS_SDK_ERROR_SOFT_AUTH_LIMIT_VIOLATION = -1227
        BS_SDK_ERROR_HARD_AUTH_LIMIT_VIOLATION = -1228

        BS_SDK_ERROR_LIFT_LOCK_UNLOCK_ZONE_FULL = -1229
        BS_SDK_ERROR_LIFT_LOCK_VIOLATION = -1230
        'Card errors
        BS_SDK_ERROR_CARD_IO = -1300
        BS_SDK_ERROR_CARD_INIT_FAIL = -1301
        BS_SDK_ERROR_CARD_NOT_ACTIVATED = -1302
        BS_SDK_ERROR_CARD_CANNOT_READ_DATA = -1303
        BS_SDK_ERROR_CARD_CIS_CRC = -1304
        BS_SDK_ERROR_CARD_CANNOT_WRITE_DATA = -1305
        BS_SDK_ERROR_CARD_READ_TIMEOUT = -1306
        BS_SDK_ERROR_CARD_READ_CANCELLED = -1307
        BS_SDK_ERROR_CARD_CANNOT_SEND_DATA = -1308
        BS_SDK_ERROR_CANNOT_FIND_CARD = -1310

        ' Operation
        BS_SDK_ERROR_INVALID_PASSWORD = -1400

        ' System
        BS_SDK_ERROR_CAMERA_INIT_FAIL = -1500
        BS_SDK_ERROR_JPEG_ENCODER_INIT_FAIL = -1501
        BS_SDK_ERROR_CANNOT_ENCODE_JPEG = -1502
        BS_SDK_ERROR_JPEG_ENCODER_NOT_INITIALIZED = -1503
        BS_SDK_ERROR_JPEG_ENCODER_DEINIT_FAIL = -1504
        BS_SDK_ERROR_CAMERA_CAPTURE_FAIL = -1505
        BS_SDK_ERROR_CANNOT_DETECT_FACE = -1506

        'ETC.
        BS_SDK_ERROR_FILE_IO = -2000
        BS_SDK_ERROR_ALLOC_MEM = -2002
        BS_SDK_ERROR_CANNOT_UPGRADE = -2003
        BS_SDK_ERROR_DEVICE_LOCKED = -2004
        BS_SDK_ERROR_CANNOT_SEND_TO_SERVER = -2005
        BS_SDK_ERROR_CANNOT_UPGRADE_MEMORY = -2006
        BS_SDK_ERROR_UPGRADE_NOT_SUPPORTED = -2007

        'SSL
        BS_SDK_ERROR_SSL_INIT = -3000
        BS_SDK_ERROR_SSL_NOT_SUPPORTED = -3001
        BS_SDK_ERROR_SSL_CANNOT_CONNECT = -3002
        BS_SDK_ERROR_SSL_ALREADY_CONNECTED = -3003
        BS_SDK_ERROR_SSL_INVALID_CERT = -3004
        BS_SDK_ERROR_SSL_VERIFY_CERT = -3005
        BS_SDK_ERROR_SSL_INVALID_KEY = -3006
        BS_SDK_ERROR_SSL_VERIFY_KEY = -3007

        ' Mobile access
        BS_SDK_ERROR_MOBILE_PORTAL = -3100

        ' OSDP
        BS_SDK_ERROR_NOT_OSDP_STANDARD_CHANNEL = -4001
        BS_SDK_ERROR_ALREADY_FULL_SLAVES = -4002
        BS_SDK_ERROR_DUPLICATE_OSDP_ID = -4003
        BS_SDK_ERROR_FAIL_ADD_OSDP_DEVICE = -4004
        BS_SDK_ERROR_FAIL_UPDATE_OSDP_DEVICE = -4005
        BS_SDK_ERROR_INVALID_OSDP_DEVICE_ID = -4006
        BS_SDK_ERROR_FAIL_MASTER_SET_KEY = -4007
        BS_SDK_ERROR_FAIL_SLAVE_SET_KEY = -4008
        BS_SDK_ERROR_DISCONNECT_SLAVE_DEVICE = -4009

        ' license
        BS_SDK_ERROR_NO_LICENSE = -4010
        BS_SDK_ERROR_LICENSE_CRC = -4011
        BS_SDK_ERROR_LICENSE_FILE_NOT_VALID = -4012
        BS_SDK_ERROR_LICENSE_PAYLOAD_LENGTH = -4013
        BS_SDK_ERROR_LICENSE_PARRING_JSON = -4014
        BS_SDK_ERROR_LICENSE_JSON_FORMAT = -4015
        BS_SDK_ERROR_LICENSE_ENABLE_PARTIAL = -4016
        BS_SDK_ERROR_LICENSE_NO_MATCH_DEVICE = -4017

        BS_SDK_ERROR_NULL_POINTER = -10000
        BS_SDK_ERROR_UNINITIALIZED = -10001
        BS_SDK_ERROR_CANNOT_RUN_SERVICE = -10002
        BS_SDK_ERROR_CANCELED = -10003
        BS_SDK_ERROR_EXIST = -10004
        BS_SDK_ERROR_ENCRYPT = -10005
        BS_SDK_ERROR_DECRYPT = -10006
        BS_SDK_ERROR_DEVICE_BUSY = -10007
        BS_SDK_ERROR_INTERNAL = -10008
        BS_SDK_ERROR_INVALID_FILE_FORMAT = -10009
        BS_SDK_ERROR_INVALID_SCHEDULE_ID = -10010
        BS_SDK_ERROR_UNKNOWN_FINGER_TEMPLATE = -10011
    End Enum

    <Flags>
    Public Enum BS2ConnectionModeEnum
        SERVER_TO_DEVICE = 0
        DEVICE_TO_SERVER = 1
    End Enum

    <Flags>
    Public Enum BS2RS485ModeEnum
        DISABLED = 0
        MASTER = 1
        SLAVE = 2
        STANDALONE = 3
    End Enum

    <Flags>
    Public Enum BS2CardTypeEnum
        UNKNOWN = &H0
        CSN = &H01
        SECURE = &H02
        ACCESS = &H03

        CSN_MOBILE = &H04
        WIEGAND_MOBILE = &H05
        QR = &H06
        SECURE_QR = &H07

        WIEGAND = &H0A
        CONFIG_CARD = &H0B
        CUSTOM_SMART = &H0D
    End Enum

    <Flags>
    Public Enum BS2CardDataTypeEnum
        BINARY = 0
        ASCII = 1
        UTF16 = 2
        BCD = 3
    End Enum

    <Flags>
    Public Enum BS2CardByteOrderEnum
        MSB = 0
        LSB = 1
    End Enum

    <Flags>
    Public Enum BS2CardModelEnum
        OMPW = 0 ' mifare card
        OIPW = 1 ' iclass card
        OEPW = 2 ' em card
        OHPW = 3 ' hid card
        ODPW = 4 ' mifare AND em card
        OAPW = 5 ' ALL card
        ODSPW = 6  ' SAM socket model

        PMPW = 20
        PIPW = 21
        PEPW = 22
        PHPW = 23
        PDPW = 24
        PAPW = 25
        PDSPW = 26

        ALL = 30    ' All Binary Model
    End Enum

    <Flags>
    Public Enum BS2UserFlagEnum
        NONE = &H00
        ''' <Sameas=""server=""/>
        CREATED = &H01
        ''' <Useris=""created=""on=""device=""/>
        UPDATED = &H02
        ''' <Useris=""updated=""on=""device=""/>
        DELETED = &H04
        ''' <Useris=""deleted=""from=""device=""/>
        DISABLED = &H80
    End Enum

    <Flags>
    Public Enum BS2UserOperatorEnum
        NONE = 0
        ADMIN = 1
        CONFIG = 2
        USER = 3
    End Enum

    <Flags>
    Public Enum BS2UserSecurityLevelEnum
        [DEFAULT] = 0
        LOWER = 1
        LOW = 2
        NORMAL = 3
        HIGH = 4
        HIGHER = 5
    End Enum

    <Flags>
    Public Enum BS2FaceEnrollThreshold
        THRESHOLD_0 = 0
        THRESHOLD_1 = 1
        THRESHOLD_2 = 2
        THRESHOLD_3 = 3
        THRESHOLD_4 = 4
        THRESHOLD_5 = 5
        THRESHOLD_6 = 6
        THRESHOLD_7 = 7
        THRESHOLD_8 = 8
        THRESHOLD_9 = 9

        THRESHOLD_DEFAULT = THRESHOLD_4
    End Enum

    <Flags>
    Public Enum BS2FingerAuthModeEnum
        NONE = 255
        ''' <Authenticationmode=""is=""not=""defined=""/>
        PROHIBITED = 254
        ''' <Authenticationmode=""is=""prohibited=""/>

        BIOMETRIC_ONLY = 0
        BIOMETRIC_PIN = 1
        NUM_OF_BIOMETRIC_AUTH_MODE = 2
    End Enum

    <Flags>
    Public Enum BS2FaceAuthModeEnum
        NONE = 255
        ''' <Authenticationmode=""is=""not=""defined=""/>
        PROHIBITED = 254
        ''' <Authenticationmode=""is=""prohibited=""/>

        BIOMETRIC_ONLY = BS2FingerAuthModeEnum.BIOMETRIC_ONLY
        BIOMETRIC_PIN = BS2FingerAuthModeEnum.BIOMETRIC_PIN
        NUM_OF_BIOMETRIC_AUTH_MODE = BS2FingerAuthModeEnum.NUM_OF_BIOMETRIC_AUTH_MODE
    End Enum

    <Flags>
    Public Enum BS2CardAuthModeEnum
        NONE = 255
        ''' <Authenticationmode=""is=""not=""defined=""/>
        PROHIBITED = 254
        ''' <Authenticationmode=""is=""prohibited=""/>

        CARD_ONLY = BS2FaceAuthModeEnum.NUM_OF_BIOMETRIC_AUTH_MODE     ' 2
        CARD_BIOMETRIC = 3
        CARD_PIN = 4
        CARD_BIOMETRIC_OR_PIN = 5
        CARD_BIOMETRIC_PIN = 6
        NUM_OF_CARD_AUTH_MODE = 7
    End Enum

    <Flags>
    Public Enum BS2IDAuthModeEnum
        NONE = 255
        ''' <Authenticationmode=""is=""not=""defined=""/>
        PROHIBITED = 254
        ''' <Authenticationmode=""is=""prohibited=""/>

        ID_BIOMETRIC = BS2CardAuthModeEnum.NUM_OF_CARD_AUTH_MODE       ' 7
        ID_PIN = 8
        ID_BIOMETRIC_OR_PIN = 9
        ID_BIOMETRIC_PIN = 10
        NUM_OF_ID_AUTH_MODE = 11
    End Enum

    ' F2 support
    <Flags>
    Public Enum BS2ExtFaceAuthModeEnum
        NONE = 255
        ''' <Authenticationmode=""is=""not=""defined=""/>
        PROHIBITED = 254
        ''' <Authenticationmode=""is=""prohibited=""/>

        EXT_FACE_ONLY = BS2IDAuthModeEnum.NUM_OF_ID_AUTH_MODE    ' 11
        EXT_FACE_FINGERPRINT = 12
        EXT_FACE_PIN = 13
        EXT_FACE_FINGERPRINT_OR_PIN = 14
        EXT_FACE_FINGERPRINT_PIN = 15
        NUM_OF_EXT_FACE_AUTH_MODE = 16
    End Enum

    <Flags>
    Public Enum BS2ExtFingerprintAuthModeEnum
        NONE = 255
        ''' <Authenticationmode=""is=""not=""defined=""/>
        PROHIBITED = 254
        ''' <Authenticationmode=""is=""prohibited=""/>

        EXT_FINGERPRINT_ONLY = BS2ExtFaceAuthModeEnum.NUM_OF_EXT_FACE_AUTH_MODE        ' 16
        EXT_FINGERPRINT_FACE = 17
        EXT_FINGERPRINT_PIN = 18
        EXT_FINGERPRINT_FACE_OR_PIN = 19
        EXT_FINGERPRINT_FACE_PIN = 20
        NUM_OF_EXT_FINGERPRINT_AUTH_MODE = 21
    End Enum

    <Flags>
    Public Enum BS2ExtCardAuthModeEnum
        NONE = 255
        ''' <Authenticationmode=""is=""not=""defined=""/>
        PROHIBITED = 254
        ''' <Authenticationmode=""is=""prohibited=""/>

        EXT_CARD_ONLY = BS2ExtFingerprintAuthModeEnum.NUM_OF_EXT_FINGERPRINT_AUTH_MODE     ' 21
        EXT_CARD_FACE = 22
        EXT_CARD_FINGERPRINT = 23
        EXT_CARD_PIN = 24
        EXT_CARD_FACE_OR_FINGERPRINT = 25
        EXT_CARD_FACE_OR_PIN = 26
        EXT_CARD_FINGERPRINT_OR_PIN = 27
        EXT_CARD_FACE_OR_FINGERPRINT_OR_PIN = 28
        EXT_CARD_FACE_FINGERPRINT = 29
        EXT_CARD_FACE_PIN = 30
        EXT_CARD_FINGERPRINT_FACE = 31
        EXT_CARD_FINGERPRINT_PIN = 32
        EXT_CARD_FACE_OR_FINGERPRINT_PIN = 33
        EXT_CARD_FACE_FINGERPRINT_OR_PIN = 34
        EXT_CARD_FINGERPRINT_FACE_OR_PIN = 35
        NUM_OF_EXT_CARD_AUTH_MODE = 36
    End Enum

    <Flags>
    Public Enum BS2ExtIDAuthModeEnum
        NONE = 255
        ''' <Authenticationmode=""is=""not=""defined=""/>
        PROHIBITED = 254
        ''' <Authenticationmode=""is=""prohibited=""/>

        EXT_ID_FACE = BS2ExtCardAuthModeEnum.NUM_OF_EXT_CARD_AUTH_MODE     ' 36
        EXT_ID_FINGERPRINT = 37
        EXT_ID_PIN = 38
        EXT_ID_FACE_OR_FINGERPRINT = 39
        EXT_ID_FACE_OR_PIN = 40
        EXT_ID_FINGERPRINT_OR_PIN = 41
        EXT_ID_FACE_OR_FINGERPRINT_OR_PIN = 42
        EXT_ID_FACE_FINGERPRINT = 43
        EXT_ID_FACE_PIN = 44
        EXT_ID_FINGERPRINT_FACE = 45
        EXT_ID_FINGERPRINT_PIN = 46
        EXT_ID_FACE_OR_FINGERPRINT_PIN = 47
        EXT_ID_FACE_FINGERPRINT_OR_PIN = 48
        EXT_ID_FINGERPRINT_FACE_OR_PIN = 49
        NUM_OF_EXT_ID_AUTH_MODE = 50
    End Enum

    <Flags>
    Public Enum BS2FingerprintFlagEnum
        NORMAL = 0
        DURESS = 1
    End Enum

    <Flags>
    Public Enum BS2FingerprintQualityEnum
        QUALITY_LOW = 20
        QUALITY_STANDARD = 40
        QUALITY_HIGH = 60
        QUALITY_HIGHEST = 80
    End Enum

    <Flags>
    Public Enum BS2FingerprintSecurityEnum
        NORMAL = 0
        HIGH = 1
        HIGHEST = 2
    End Enum

    <Flags>
    Public Enum BS2FingerprintSensitivityEnum
        SENSITIVE0 = 0
        SENSITIVE1 = 1
        SENSITIVE2 = 2
        SENSITIVE3 = 3
        SENSITIVE4 = 4
        SENSITIVE5 = 5
        SENSITIVE6 = 6
        SENSITIVE7 = 7
    End Enum

    <Flags>
    Public Enum BS2FingerprintFastModeEnum
        AUTO = 0
        NORMAL = 1
        FASTER = 2
        FASTEST = 3
    End Enum

    <Flags>
    Public Enum BS2FingerprintTemplateFormatEnum
        FORMAT_SUPREMA = 0
        FORMAT_ISO = 1
        FORMAT_ANSI = 2
    End Enum

    <Flags>
    Public Enum BS2FingerprintSensorModeEnum
        ALWAYS_ON = 0
        PROXIMITY = 1
    End Enum

    <Flags>
    Public Enum BS2DeviceTypeEnum
        UNKNOWN = &H00

        BIOENTRY_PLUS = &H01
        BIOENTRY_W = &H02
        BIOLITE_NET = &H03
        XPASS = &H04
        XPASS_S2 = &H05
        SECURE_IO_2 = &H06
        DOOR_MODULE_20 = &H07
        BIOSTATION_2 = &H08
        BIOSTATION_A2 = &H09
        FACESTATION_2 = &H0A
        IO_DEVICE = &H0B
        BIOSTATION_L2 = &H0C
        BIOENTRY_W2 = &H0D
        'CORE_STATION  = 0x0E,		// Deprecated 2.6.0
        CORESTATION_40 = &H0E
        OUTPUT_MODULE = &H0F
        INPUT_MODULE = &H10
        BIOENTRY_P2 = &H11
        BIOLITE_N2 = &H12
        XPASS2 = &H13
        XPASS_S3 = &H14
        BIOENTRY_R2 = &H15
        XPASS_D2 = &H16
        DOOR_MODULE_21 = &H17
        XPASS_D2_KEYPAD = &H18
        FACELITE = &H19
        XPASS2_KEYPAD = &H1A
        XPASS_D2_REV = &H1B     ' [+2.7]
        XPASS_D2_KEYPAD_REV = &H1C ' [+2.7]
        FACESTATION_F2_FP = &H1D   ' FSF2 support
        FACESTATION_F2 = &H1E     ' FSF2 support
        XSTATION_2_QR = &H1F     ' [+2.8]
        XSTATION_2 = &H20     ' [+2.8]
        IM_120 = &H21     ' [+2.8.1]
        XSTATION_2_FP = &H22     ' [+2.8.1]
        BIOSTATION_3 = &H23     ' [+2.8.3]
        THIRD_OSDP_DEVICE = &H24   ' [+2.9.1]
        THIRD_OSDP_IO_DEVICE = &H25   ' [+2.9.1]
        BIOSTATION_2A = &H26     ' [+2.9.4]
        BIOENTRY_W3 = &H2A     ' [+2.9.6]


        TYPE_MAX = BIOENTRY_W3
        'UNKNOWN         = 0xFF,
    End Enum

    <Flags>
    Public Enum BS2TCPBasebandEnum
        BASE_10MB = 0
        BASE_100MB = 1
    End Enum

    <Flags>
    Public Enum BS2LanguageEnum
        KOREAN = 0
        ENGLISH = 1
        CUSTOM = 2
    End Enum

    <Flags>
    Public Enum BS2BackgroundEnum
        LOGO = 0
        NOTICE = 1
        SLIDE = 2
        PDF = 3
    End Enum

    <Flags>
    Public Enum BS2BGThemeEnum
        THEME1 = 0
        THEME2 = 1
        THEME3 = 2
        THEME4 = 3
    End Enum

    <Flags>
    Public Enum BS2DateFormatEnum
        YYYYMMDD = 0
        MMDDYYYY = 1
        DDMMYYYY = 2
    End Enum

    <Flags>
    Public Enum BS2TimeFormatEnum
        HOUR12 = 0
        HOUR24 = 1
    End Enum

    <Flags>
    Public Enum BS2HomeFormationEnum
        INTERPHONE = 0
        SHORTCUT1 = 1
        SHORTCUT2 = 2
        SHORTCUT3 = 3
        SHORTCUT4 = 4
    End Enum

    <Flags>
    Public Enum BS2ShortCutHomeEnum
        MENU = 0
        TNA = 1
        LANGUAGE = 2
        ID = 3
        FINGERPRINT = 4
        INTERPHONE = 5
        ARM = 6
        VOLUME = 7
    End Enum

    <Flags>
    Public Enum BS2TNAModeEnum
        UNUSED = 0
        USER = 1
        SCHEDULE = 2
        LAST_CHOICE = 3
        FIXED = 4
    End Enum

    <Flags>
    Public Enum BS2TNAKeyEnum
        UNSPECIFIED = 0
        KEY1 = 1
        KEY2 = 2
        KEY3 = 3
        KEY4 = 4
        KEY5 = 5
        KEY6 = 6
        KEY7 = 7
        KEY8 = 8
        KEY9 = 9
        KEY10 = 10
        KEY11 = 11
        KEY12 = 12
        KEY13 = 13
        KEY14 = 14
        KEY15 = 15
        KEY16 = 16
    End Enum

    <Flags>
    Public Enum BS2WiegandInOutEnum
        [IN] = 0
        OUT = 1
        INOUT = 2      ' FISSDK-147  missing I/O mode
    End Enum

    <Flags>
    Public Enum BS2WiegandParityEnum
        None = 0
        Odd = 1
        Even = 2
    End Enum

    <Flags>
    Public Enum BS2ScheduleIDEnum
        NEVER = 0
        ALWAYS = 1
    End Enum

    <Flags>
    Public Enum BS2SoundIndexEnum
        WELCOME = 0
        AUTH_SUCCESS = 1
        AUTH_FAIL = 2
        ALARM_1 = 3    ' FISSDK-95  Add missing sound index
        ALARM_2 = 4    ' FISSDK-95  Add missing sound index
    End Enum

    <Flags>
    Public Enum BS2ResourceTypeEnum
        BS2_RESOURCE_TYPE_UI = 0
        BS2_RESOURCE_TYPE_NOTICE = 1
        BS2_RESOURCE_TYPE_IMAGE = 2
        BS2_RESOURCE_TYPE_SLIDE = 3
        BS2_RESOURCE_TYPE_SOUND = 4
    End Enum

    <Flags>
    Public Enum BS2SwitchTypeEnum
        NORMAL_OPEN = 0
        NORMAL_CLOSE = 1
    End Enum

    <Flags>
    Public Enum BS2DoorFlagEnum
        NONE = 0
        SCHEDULE = 1
        EMERGENCY = 2
        [OPERATOR] = 4
        ALL = &HFF
    End Enum

    <Flags>
    Public Enum BS2DoorAlarmFlagEnum
        NONE = 0
        FORCED_OPEN = 1
        HELD_OPEN = 2
        APB = 4
    End Enum

    <Flags>
    Public Enum BS2DualAuthDeviceEnum
        NO_DEVICE = 0
        ENTRY_DEVICE_ONLY = 1
        EXIT_DEVICE_ONLY = 2
        BOTH_DEVICE = 3
    End Enum

    <Flags>
    Public Enum BS2LedColorEnum
        OFF = 0
        RED = 1
        YELLOW = 2
        GREEN = 3
        CYAN = 4
        BLUE = 5
        MAGENTA = 6
        WHITE = 7
    End Enum

    <Flags>
    Public Enum BS2BuzzerToneEnum
        OFF = 0
        LOW = 1
        MIDDLE = 2
        HIGH = 3
    End Enum

    <Flags>
    Public Enum BS2LiftActionTypeEnum
        ACTIVATE_FLOORS = 0
        DEACTIVATE_FLOORS = 1
        RELEASE_FLOORS = 2
    End Enum

    <Flags>
    Public Enum BS2ActionTypeEnum
        NONE = 0
        LOCK_DEVICE = 1
        UNLOCK_DEVICE = 2
        REBOOT_DEVICE = 3
        RELEASE_ALARM = 4
        GENERAL_INPUT = 5
        RELAY = 6
        TTL = 7
        SOUND = 8
        DISPLAY = 9
        BUZZER = 10
        LED = 11
        FIRE_ALARM_INPUT = 12
        AUTH_SUCCESS = 13
        AUTH_FAIL = 14
        LIFT = 15
    End Enum

    <Flags>
    Public Enum BS2TriggerTypeEnum
        NONE = 0
        [EVENT] = 1
        INPUT = 2
        SCHEDULE = 3
    End Enum

    <Flags>
    Public Enum BS2DualAuthApprovalEnum
        NONE = 0
        LAST = 1
    End Enum

    <Flags>
    Public Enum BS2FaceDetectionLevelEnum
        NONE = 0
        NORMAL = 1
        STRICT = 2
    End Enum

    <Flags>
    Public Enum BS2GlobalAPBFailActionTypeEnum
        NONE = 0
        SOFT = 1
        HARD = 2
    End Enum

    <Flags>
    Public Enum BS2WlanOperationModeEnum
        MANAGED = 0
        ADHOC = 1
    End Enum

    <Flags>
    Public Enum BS2WlanAuthTypeEnum
        OPEN = 0
        [SHARED] = 1
        WPA_PSK = 2
        WPA2_PSK = 3
    End Enum

    <Flags>
    Public Enum BS2WlanEncryptionTypeEnum
        NONE = 0
        WEP = 1
        TKIP_AES = 2
        AES = 3
        TKIP = 4
    End Enum

    <Flags>
    Public Enum BS2ZoneTypeEnum
        APB = 0
        TIMED_APB = 1
        FIRE_ALARM = 2
        SCHEDULED_LOCK_UNLOCK = 3
    End Enum

    <Flags>
    Public Enum BS2ZoneStatusEnum
        NORMAL = 0
        ALARM = 1
        FORCED_LOCKED = 2
        FORCED_UNLOCKED = 4
        ARM = 8
        DISARM = NORMAL
    End Enum

    <Flags>
    Public Enum BS2APBZoneTypeEnum
        HARD = 0
        SOFT = 1
    End Enum

    <Flags>
    Public Enum BS2APBZoneReaderTypeEnum
        NONE = -1
        ENTRY = 0
        [EXIT] = 1
    End Enum

    <Flags>
    Public Enum BS2TimedAPBZoneTypeEnum
        HARD = 0
        SOFT = 1
    End Enum

    <Flags>
    Public Enum BS2SslMethodMaskEnum As UInteger
        NONE = 0
        SSL2 = &H1
        SSL3 = &H2
        TLS1 = &H4
        TLS1_1 = &H8
        TLS1_2 = &H10
        ALL = &HFFFFFFFFUI
    End Enum

    <Flags>
    Public Enum BS2LiftAlarmFlagEnum
        NONE = 0
        FIRST = &H1
        SECOND = &H2
        TAMPER = &H4
    End Enum

    <Flags>
    Public Enum BS2FloorFlagEnum
        NONE = 0
        SCHEDULE = &H1
        EMERGENCY = &H2
        [OPERATOR] = &H4
        ACTION = &H8       ' [+ 2.6.4]
    End Enum

    <Flags>
    Public Enum BS2ConfigMaskEnum As UInteger
        NONE = 0
        FACTORY = &H0001                ' Factory configuration
        SYSTEM = &H0002                 ' System configuration
        IP = &H0004                     ' TCP/IP configuration
        RS485 = &H0008                  ' RS485 configuration
        WLAN = &H0010                   ' Wireless LAN configuration
        AUTH = &H0020                   ' Authentication configuration
        CARD = &H0040                   ' Card configuration
        FINGERPRINT = &H0080            ' Fingerprint configuration
        FACE = &H0100                   ' Face configuration
        TRIGGER_ACTION = &H0200         ' Trigger Action configuration
        DISPLAY = &H0400                ' Display configuration
        SOUND = &H0800                  ' Sound configuration
        STATUS = &H1000                 ' Status Signal(LED, Buzzer) configuration
        WIEGAND = &H2000                ' Wiegand configuration
        USB = &H4000                    ' USB configuration
        TNA = &H8000                    ' Time and Attendance configuration (@deprecated)
        VIDEOPHONE = &H10000            ' Videophone configuration
        INTERPHONE = &H20000            ' Interphone configuration
        VOIP = &H40000                  ' Voice over IP configuration
        INPUT = &H80000                 ' Input(Supervised input) configuration
        WIEGAND_IO = &H100000            ' Wiegand IO Device configuration
        TNA_EXT = &H200000              ' Time and Attendance configuration
        IP_EXT = &H400000               ' DNS and Server url configuration
        [EVENT] = &H800000                ' Event configuration
        CARD_1x = &H1000000             ' Card_1x configuration
        WIEGAND_MULTI = &H2000000       ' Wiegand Multi configuration
        SYSTEM_EXT = &H4000000       ' Extended System configuration
        DST = &H8000000           '< Daylight Saving configuration
        ALL = &HFFFFFFFFUI                ' 4 bytes
    End Enum

    <Flags>
    Public Enum BS2UserMaskEnum As UInteger
        ID_ONLY = 0                     ' fill only user id in BS2User
        DATA = &H0001                   ' BS2User
        SETTING = &H0002                ' BS2UserSetting
        NAME = &H0004                   ' BS2_USER_NAME
        PHOTO = &H0008                  ' BS2UserPhoto
        PIN = &H0010                    ' BS2_HASH256
        CARD = &H0020                   ' BS2CSNCard
        FINGER = &H0040                 ' BS2FingerTemplate
        FACE = &H0080                   ' BS2FaceTemplate
        ACCESS_GROUP = &H0100           ' BS2_ACCESS_GROUP_ID
        JOB = &H0200                    ' BS2Job
        PHRASE = &H0400                 ' BS2_USER_PHRASE
        FACE_EX = &H0800                ' BS2FaceExWarped, BS2FaceExUnwarped
        SETTING_EX = &H1000             ' BS2UserSettingEx
        ALL = &HFFFF                    ' 4 bytes
    End Enum

    <Flags>
    Public Enum BS2UserInfoMaskEnum      ' [+V2.8.3]
        PHRASE = &H01
        JOB_CODE = &H02
        NAME = &H04
        PHOTO = &H08
        PIN = &H10
        CARD = &H20
        FINGER = &H40
        FACE = &H80
    End Enum

    <Flags>
    Public Enum BS2EventCodeEnum
        ALL = &H0000
        MASK = &HFF00

        VERIFY_SUCCESS = &H1000
        VERIFY_SUCCESS_ID_PIN = &H1001
        VERIFY_SUCCESS_ID_FINGER = &H1002
        VERIFY_SUCCESS_ID_FINGER_PIN = &H1003
        VERIFY_SUCCESS_ID_FACE = &H1004
        VERIFY_SUCCESS_ID_FACE_PIN = &H1005
        VERIFY_SUCCESS_CARD = &H1006
        VERIFY_SUCCESS_CARD_PIN = &H1007
        VERIFY_SUCCESS_CARD_FINGER = &H1008
        VERIFY_SUCCESS_CARD_FINGER_PIN = &H1009
        VERIFY_SUCCESS_CARD_FACE = &H100A
        VERIFY_SUCCESS_CARD_FACE_PIN = &H100B
        VERIFY_SUCCESS_AOC = &H100C
        VERIFY_SUCCESS_AOC_PIN = &H100D
        VERIFY_SUCCESS_AOC_FINGER = &H100E
        VERIFY_SUCCESS_AOC_FINGER_PIN = &H100F
        VERIFY_SUCCESS_CARD_FACE_FINGER = &H1010
        VERIFY_SUCCESS_CARD_FINGER_FACE = &H1011
        VERIFY_SUCCESS_ID_FACE_FINGER = &H1012
        VERIFY_SUCCESS_ID_FINGER_FACE = &H1013
        VERIFY_SUCCESS_MOBLIE_CARD = &H1016
        VERIFY_SUCCESS_MOBILE_CARD_PIN = &H1017
        VERIFY_SUCCESS_MOBILE_CARD_FINGER = &H1018
        VERIFY_SUCCESS_MOBILE_CARD_FINGER_PIN = &H1019
        VERIFY_SUCCESS_MOBILE_CARD_FACE = &H101A
        VERIFY_SUCCESS_MOBILE_CARD_FACE_PIN = &H101B
        VERIFY_SUCCESS_MOBILE_CARD_FACE_FINGER = &H1020
        VERIFY_SUCCESS_MOBILE_CARD_FINGER_FACE = &H1021
        VERIFY_SUCCESS_QR = &H1025
        VERIFY_SUCCESS_QR_PIN = &H1026
        VERIFY_SUCCESS_QR_FINGER = &H1027
        VERIFY_SUCCESS_QR_FINGER_PIN = &H1028
        VERIFY_SUCCESS_QR_FACE = &H1029
        VERIFY_SUCCESS_QR_FACE_PIN = &H102A
        VERIFY_SUCCESS_QR_FACE_FINGER = &H102B
        VERIFY_SUCCESS_QR_FINGER_FACE = &H102C

        VERIFY_FAIL = &H1100
        VERIFY_FAIL_ID = &H1101
        VERIFY_FAIL_CARD = &H1102
        VERIFY_FAIL_PIN = &H1103
        VERIFY_FAIL_FINGER = &H1104
        VERIFY_FAIL_FACE = &H1105
        VERIFY_FAIL_AOC_PIN = &H1106
        VERIFY_FAIL_AOC_FINGER = &H1107
        VERIFY_FAIL_CREDENTIAL_MOBILE_CARD = &H1108
        VERIFY_FAIL_CREDENTIAL_QR = &H110C
        VERIFY_FAIL_NON_NUMERIC_QR = &H1109
        VERIFY_FAIL_NON_PRINTABLE_QR = &H110A
        VERIFY_FAIL_TOO_LONG_QR = &H110B

        VERIFY_DURESS = &H1200
        VERIFY_DURESS_ID_PIN = &H1201
        VERIFY_DURESS_ID_FINGER = &H1202
        VERIFY_DURESS_ID_FINGER_PIN = &H1203
        VERIFY_DURESS_ID_FACE = &H1204
        VERIFY_DURESS_ID_FACE_PIN = &H1205
        VERIFY_DURESS_CARD = &H1206
        VERIFY_DURESS_CARD_PIN = &H1207
        VERIFY_DURESS_CARD_FINGER = &H1208
        VERIFY_DURESS_CARD_FINGER_PIN = &H1209
        VERIFY_DURESS_CARD_FACCE = &H120A
        VERIFY_DURESS_CARD_FACE_PIN = &H120B
        VERIFY_DURESS_AOC = &H120C
        VERIFY_DURESS_AOC_PIN = &H120D
        VERIFY_DURESS_AOC_FINGER = &H120E
        VERIFY_DURESS_AOC_FINGER_PIN = &H120F
        VERIFY_DURESS_CARD_FACE_FINGER = &H1210
        VERIFY_DURESS_CARD_FINGER_FACE = &H1211
        VERIFY_DURESS_ID_FACE_FINGER = &H1212
        VERIFY_DURESS_ID_FINGER_FACE = &H1213
        VERIFY_DURESS_MOBLIE_CARD = &H1216
        VERIFY_DURESS_MOBILE_CARD_PIN = &H1217
        VERIFY_DURESS_MOBILE_CARD_FINGER = &H1218
        VERIFY_DURESS_MOBILE_CARD_FINGER_PIN = &H1219
        VERIFY_DURESS_MOBILE_CARD_FACE = &H121A
        VERIFY_DURESS_MOBILE_CARD_FACE_PIN = &H121B
        VERIFY_DURESS_MOBILE_CARD_FACE_FINGER = &H1220
        VERIFY_DURESS_MOBILE_CARD_FINGER_FACE = &H1221
        VERIFY_DURESS_QR = &H1225
        VERIFY_DURESS_QR_PIN = &H1226
        VERIFY_DURESS_QR_FINGER = &H1227
        VERIFY_DURESS_QR_FINGER_PIN = &H1228
        VERIFY_DURESS_QR_FACE = &H1229
        VERIFY_DURESS_QR_FACE_PIN = &H122A
        VERIFY_DURESS_QR_FACE_FINGER = &H122B
        VERIFY_DURESS_QR_FINGER_FACE = &H122C

        IDENTIFY_SUCCESS = &H1300
        IDENTIFY_SUCCESS_FINGER = &H1301
        IDENTIFY_SUCCESS_FINGER_PIN = &H1302
        IDENTIFY_SUCCESS_FACE = &H1303
        IDENTIFY_SUCCESS_FACE_PIN = &H1304
        IDENTIFY_SUCCESS_FACE_FINGER = &H1305
        IDENTIFY_SUCCESS_FACE_FINGER_PIN = &H1306
        IDENTIFY_SUCCESS_FINGER_FACE = &H1307
        IDENTIFY_SUCCESS_FINGER_FACE_PIN = &H1308

        IDENTIFY_FAIL = &H1400
        IDENTIFY_FAIL_ID = &H1401
        IDENTIFY_FAIL_CARD = &H1402
        IDENTIFY_FAIL_PIN = &H1403
        IDENTIFY_FAIL_FINGER = &H1404
        IDENTIFY_FAIL_FACE = &H1405
        IDENTIFY_FAIL_AOC_PIN = &H1406
        IDENTIFY_FAIL_AOC_FINGER = &H1407
        IDENTIFY_FAIL_CREDENTIAL_MOBILE_CARD = &H1408
        IDENTIFY_FAIL_CREDENTIAL_QR = &H140C
        IDENTIFY_FAIL_NON_NUMERIC_QR = &H1409
        IDENTIFY_FAIL_NON_PRINTABLE_QR = &H140A
        IDENTIFY_FAIL_TOO_LONG_QR = &H140B

        IDENTIFY_DURESS = &H1500
        IDENTIFY_DURESS_FINGER = &H1501
        IDENTIFY_DURESS_FINGER_PIN = &H1502
        IDENTIFY_DURESS_FACE = &H1503
        IDENTIFY_DURESS_FACE_PIN = &H1504
        IDENTIFY_DURESS_FACE_FINGER = &H1505
        IDENTIFY_DURESS_FACE_FINGER_PIN = &H1506
        IDENTIFY_DURESS_FINGER_FACE = &H1507
        IDENTIFY_DURESS_FINGER_FACE_PIN = &H1508

        DUAL_AUTH_SUCCESS = &H1600

        DUAL_AUTH_FAIL = &H1700
        DUAL_AUTH_FAIL_TIMEOUT = &H1701
        DUAL_AUTH_FAIL_ACCESS_GROUP = &H1702

        AUTH_FAILED = &H1800
        AUTH_FAILED_INVALID_AUTH_MODE = &H1801
        AUTH_FAILED_INVALID_CREDENTIAL = &H1802
        AUTH_FAILED_TIMEOUT = &H1803
        AUTH_FAILED_MATCHING_REFUSAL = &H1804

        ACCESS_DENIED = &H1900
        ACCESS_DENIED_ACCESS_GROUP = &H1901
        ACCESS_DENIED_DISABLED = &H1902
        ACCESS_DENIED_EXPIRED = &H1903
        ACCESS_DENIED_ON_BLACKLIST = &H1904
        ACCESS_DENIED_APB = &H1905
        ACCESS_DENIED_TIMED_APB = &H1906
        ACCESS_DENIED_SCHEDULED_LOCK = &H1907

        ACCESS_EXCUSED_APB = &H1908
        ACCESS_EXCUSED_TIMED_APB = &H1909

        ACCESS_DENIED_FACE_DETECTION = &H190A
        ACCESS_DENIED_CAMERA_CAPTURE = &H190B
        ACCESS_DENIED_FAKE_FINGER = &H190C
        ACCESS_DENIED_DEVICE_ZONE_ENTRANCE_LIMIT = &H190D
        ACCESS_DENIED_INTRUSION_ALARM = &H190E
        ACCESS_DENIED_INTERLOCK = &H190F
        ACCESS_EXCUSED_AUTH_LIMIT = &H1910         ' [+V2.8.3]
        ACCESS_DENIED_AUTH_LIMIT = &H1911          ' [+V2.8.3]
        ACCESS_DENIED_ANTI_TAILGATE = &H1912       ' [+V2.8.3]
        ACCESS_DENIED_HIGH_TEMPERATURE = &H1913    ' [+V2.8.3]
        ACCESS_DENIED_NO_TEMPERATURE = &H1914      ' [+V2.8.3]
        ACCESS_DENIED_UNMASKED_FACE = &H1915       ' [+V2.8.3]

        USER_ENROLL_SUCCESS = &H2000
        USER_ENROLL_FAIL = &H2100
        USER_ENROLL_FAIL_INVALID_FACE = &H2101
        USER_ENROLL_FAIL_MISMATCHED_FORMAT = &H2102       ' [+V2.8.3]
        USER_ENROLL_FAIL_FULL_CREDENTIAL = &H2103         ' [+V2.8.3]
        USER_ENROLL_FAIL_INVALID_USER = &H2104            ' [+V2.8.3]
        USER_ENROLL_FAIL_INTERNAL_ERROR = &H2109          ' [+V2.8.3]
        USER_UPDATE_SUCCESS = &H2200
        USER_UPDATE_FAIL = &H2300
        USER_DELETE_SUCCESS = &H2400
        USER_DELETE_FAIL = &H2500
        USER_DELETE_ALL_SUCCESS = &H2600
        USER_ISSUE_AOC_SUCCESS = &H2700
        USER_DUPLICATE_CREDENTIAL = &H2800
        USER_UPDATE_PARTIAL_SUCCESS = &H2900               ' [+V2.8.3]
        USER_UPDATE_PARTIAL_FAIL = &H2A00                  ' [+V2.8.3]
        USER_UPDATE_PARTIAL_FAIL_INVALID_FACE = &H2A01
        USER_UPDATE_PARTIAL_FAIL_MISMATCHED_FORMAT = &H2A02 ' [+V2.8.3]
        USER_UPDATE_PARTIAL_FAIL_FULL_CREDENTIAL = &H2A03  ' [+V2.8.3]
        USER_UPDATE_PARTIAL_FAIL_INVALID_USER = &H2A04     ' [+V2.8.3]
        USER_UPDATE_PARTIAL_FAIL_INTERNAL_ERROR = &H2A09   ' [+V2.8.3]

        DEVICE_SYSTEM_RESET = &H3000
        DEVICE_SYSTEM_STARTED = &H3100
        DEVICE_TIME_SET = &H3200
        DEVICE_TIMEZONE_SET = &H3201
        DEVICE_DST_SET = &H3202
        DEVICE_LINK_CONNECTED = &H3300
        DEVICE_LINK_DISCONNECTED = &H3400
        DEVICE_DHCP_SUCCESS = &H3500
        DEVICE_ADMIN_MENU = &H3600
        DEVICE_UI_LOCKED = &H3700
        DEVICE_UI_UNLOCKED = &H3800
        DEVICE_COMM_LOCKED = &H3900
        DEVICE_COMM_UNLOCKED = &H3A00
        DEVICE_TCP_CONNECTED = &H3B00
        DEVICE_RTSP_CONNECTED = &H3B10
        DEVICE_TCP_DISCONNECTED = &H3C00
        DEVICE_RTSP_DISCONNECTED = &H3C10
        DEVICE_RS485_CONNECTED = &H3D00
        DEVICE_RS485_DISCONNECTED = &H3E00
        DEVICE_INPUT_DETECTED = &H3F00
        DEVICE_TAMPER_ON = &H4000
        DEVICE_TAMPER_OFF = &H4100
        DEVICE_EVENT_LOG_CLEARED = &H4200
        DEVICE_FIRMWARE_UPGRADED = &H4300
        DEVICE_RESOURCE_UPGRADED = &H4400
        DEVICE_CONFIG_RESET = &H4500
        DEVICE_DATABASE_RESET = &H4501
        DEVICE_FACTORY_RESET = &H4502
        DEVICE_CONFIG_RESET_EX = &H4503
        DEVICE_FACTORY_RESET_WITHOUT_ETHERNET = &H4504

        SUPERVISED_INPUT_SHORT = &H4600
        SUPERVISED_INPUT_OPEN = &H4700

        DEVICE_AC_FAIL = &H4800
        DEVICE_AC_SUCCESS = &H4900
        EXIT_BUTTON = &H4A00
        SIMULATED_EXIT_BUTTON = &H4A01
        OPERATOR_OPEN = &H4B00
        VOIP_OPEN = &H4C00

        DOOR_UNLOCKED = &H5000
        DOOR_UNLOCKED_BY_BUTTON = &H5001           ' [+V2.8.3]
        DOOR_UNLOCKED_BY_OPERATOR = &H5002         ' [+V2.8.3]
        DOOR_UNLOCKED_BY_SIMULATED_BUTTON = &H5003 ' [+V2.8.3]
        DOOR_UNLOCKED_BY_VOIP = &H5004             ' [+V2.8.3]
        DOOR_LOCKED = &H5100
        DOOR_OPENED = &H5200
        DOOR_CLOSED = &H5300
        DOOR_FORCED_OPEN = &H5400
        DOOR_HELD_OPEN = &H5500
        DOOR_FORCED_OPEN_ALARM = &H5600
        DOOR_FORCED_OPEN_ALARM_CLEAR = &H5700
        DOOR_HELD_OPEN_ALARM = &H5800
        DOOR_HELD_OPEN_ALARM_CLEAR = &H5900
        DOOR_APB_ALARM = &H5A00
        DOOR_APB_ALARM_CLEAR = &H5B00
        DOOR_RELEASE_NONE = &H5C00
        DOOR_RELEASE_SCHEDULE = &H5C01
        DOOR_RELEASE_EMERGENCY = &H5C02
        DOOR_RELEASE_OPERATOR = &H5C04
        DOOR_LOCK_NONE = &H5D00
        DOOR_LOCK_SCHEDULE = &H5D01
        DOOR_LOCK_EMERGENCY = &H5D02
        DOOR_LOCK_OPERATOR = &H5D04
        DOOR_UNLOCK_NONE = &H5E00
        DOOR_UNLOCK_SCHEDULE = &H5E01
        DOOR_UNLOCK_EMERGENCY = &H5E02
        DOOR_UNLOCK_OPERATOR = &H5E04

        ZONE_APB_VIOLATION = &H6000
        ZONE_APB_VIOLATION_HARD = &H6001
        ZONE_APB_VIOLATION_SOFT = &H6002
        ZONE_APB_ALARM = &H6100
        ZONE_APB_ALARM_CLEAR = &H6200

        ZONE_TIMED_APB_VIOLATION = &H6300
        ZONE_TIMED_APB_VIOLATION_HARD = &H6301
        ZONE_TIMED_APB_VIOLATION_SOFT = &H6302
        ZONE_TIMED_APB_ALARM = &H6400
        ZONE_TIMED_APB_ALARM_CLEAR = &H6500

        ZONE_FIRE_ALARM_INPUT = &H6600
        ZONE_FIRE_ALARM = &H6700
        ZONE_FIRE_ALARM_CLEAR = &H6800

        ZONE_SCHEDULED_LOCK_VIOLATION = &H6900
        ZONE_SCHEDULED_LOCK_START = &H6A00
        ZONE_SCHEDULED_LOCK_END = &H6B00
        ZONE_SCHEDULED_UNLOCK_START = &H6C00
        ZONE_SCHEDULED_UNLOCK_END = &H6D00
        ZONE_SCHEDULED_LOCK_ALARM = &H6E00
        ZONE_SCHEDULED_LOCK_ALARM_CLEAR = &H6F00

        LIFT_FLOOR_ACTIVATED = &H7000
        LIFT_FLOOR_DEACTIVATED = &H7100
        LIFT_FLOOR_RELEASE = &H7200
        LIFT_FLOOR_RELEASE_SCHEDULE = &H7201
        LIFT_FLOOR_RELEASE_EMERGENCY = &H7202
        LIFT_FLOOR_RELEASE_OPERATOR = &H7204
        LIFT_FLOOR_ACTIVATE = &H7300
        LIFT_FLOOR_ACTIVATE_SCHEDULE = &H7301
        LIFT_FLOOR_ACTIVATE_EMERGENCY = &H7302
        LIFT_FLOOR_ACTIVATE_OPERATOR = &H7304
        LIFT_FLOOR_DEACTIVATE = &H7400
        LIFT_FLOOR_DEACTIVATE_SCHEDULE = &H7401
        LIFT_FLOOR_DEACTIVATE_EMERGENCY = &H7402
        LIFT_FLOOR_DEACTIVATE_OPERATOR = &H7404

        LIFT_ALARM_INPUT = &H7500
        LIFT_ALARM = &H7600
        LIFT_ALARM_CLEAR = &H7700
        LIFT_ALL_FLOOR_ACTIVATED = &H7800
        LIFT_ALL_FLOOR_DEACTIVATED = &H7900

        GLOBAL_APB_EXCUSED = &H8000

        ZONE_ENTRANCE_LIMIT_COUNT_VIOLATION = &H8100
        ZONE_ENTRANCE_LIMIT_COUNT_VIOLATION_HARD = &H8101
        ZONE_ENTRANCE_LIMIT_COUNT_VIOLATION_SOFT = &H8102
        ZONE_ENTRANCE_LIMIT_TIME_VIOLATION_HARD = &H8103
        ZONE_ENTRANCE_LIMIT_TIME_VIOLATION_SOFT = &H8104
        ZONE_ENTRANCE_LIMIT_ALARM = &H8200
        ZONE_ENTRANCE_LIMIT_ALARM_CLEAR = &H8300

        ZONE_INTRUSION_ALARM_VIOLATION = &H9000
        ZONE_INTRUSION_ALARM_ARM_GRANTED = &H9100
        ZONE_INTRUSION_ALARM_ARM_SUCCESS = &H9200
        ZONE_INTRUSION_ALARM_ARM_FAIL = &H9300
        ZONE_INTRUSION_ALARM_DISARM_GRANTED = &H9400
        ZONE_INTRUSION_ALARM_DISARM_SUCCESS = &H9500
        ZONE_INTRUSION_ALARM_DISARM_FAIL = &H9600
        ZONE_INTRUSION_ALARM_INPUT = &H9700
        ZONE_INTRUSION_ALARM = &H9800
        ZONE_INTRUSION_ALARM_CLEAR = &H9900
        ZONE_INTRUSION_ALARM_ARM_DENIED = &H9A00
        ZONE_INTRUSION_ALARM_DISARM_DENIED = &H9B00

        ZONE_INTERLOCK_VIOLATION = &HA000
        ZONE_INTERLOCK_ALARM = &HA100
        ZONE_INTERLOCK_ALARM_DOOR_OPEN_DENIED = &HA200
        ZONE_INTERLOCK_ALARM_INDOOR_DENIED = &HA300
        ZONE_INTERLOCK_ALARM_CLEAR = &HA400
        ZONE_AUTH_LIMIT_VIOLATION = &HA500
        GLOBAL_AUTH_LIMIT_EXCUSED = &HA600

        ' Relay Action (Linakge & Latching)
        RELAY_ACTION_ON = &HC300
        RELAY_ACTION_OFF = &HC400
        RELAY_ACTION_KEEP = &HC500
    End Enum

    <Flags>
    Public Enum BS2SubEventCodeEnum
        SUB_EVENT_MASK = &H00FF

        VERIFY_ID_PIN = &H01
        VERIFY_ID_FINGER = &H02
        VERIFY_ID_FINGER_PIN = &H03
        VERIFY_ID_FACE = &H04
        VERIFY_ID_FACE_PIN = &H05
        VERIFY_CARD = &H06
        VERIFY_CARD_PIN = &H07
        VERIFY_CARD_FINGER = &H08
        VERIFY_CARD_FINGER_PIN = &H09
        VERIFY_CARD_FACE = &H0A
        VERIFY_CARD_FACE_PIN = &H0B
        VERIFY_AOC = &H0C
        VERIFY_AOC_PIN = &H0D
        VERIFY_AOC_FINGER = &H0E
        VERIFY_AOC_FINGER_PIN = &H0F
        VERIFY_CARD_FACE_FINGER = &H10
        VERIFY_CARD_FINGER_FACE = &H11
        VERIFY_ID_FACE_FINGER = &H12
        VERIFY_ID_FINGER_FACE = &H13

        VERIFY_MOBLIE_CARD = &H16
        VERIFY_MOBILE_CARD_PIN = &H17
        VERIFY_MOBILE_CARD_FINGER = &H18
        VERIFY_MOBILE_CARD_FINGER_PIN = &H19
        VERIFY_MOBILE_CARD_FACE = &H1A
        VERIFY_MOBILE_CARD_FACE_PIN = &H1B
        VERIFY_MOBILE_CARD_FACE_FINGER = &H20
        VERIFY_MOBILE_CARD_FINGER_FACE = &H21

        VERIFY_QR = &H25
        VERIFY_QR_PIN = &H26
        VERIFY_QR_FINGER = &H27
        VERIFY_QR_FINGER_PIN = &H28
        VERIFY_QR_FACE = &H29
        VERIFY_QR_FACE_PIN = &H2A
        VERIFY_QR_FACE_FINGER = &H2B
        VERIFY_QR_FINGER_FACE = &H2C

        ' Identified authentication mode
        IDENTIFY_FINGER = &H01
        IDENTIFY_FINGER_PIN = &H02
        IDENTIFY_FACE = &H03
        IDENTIFY_FACE_PIN = &H04
        IDENTIFY_FACE_FINGER = &H05
        IDENTIFY_FACE_FINGER_PIN = &H06
        IDENTIFY_FINGER_FACE = &H07
        IDENTIFY_FINGER_FACE_PIN = &H08

        ' Reason to be failed
        ENROLL_FAIL_INVALID_FACE = &H01
        UPDATE_FAIL_INVALID_FACE = &H01
        ENROLL_FAIL_MISMATCHED_FORMAT = &H02       ' [+V2.8.3]
        UPDATE_FAIL_MISMATCHED_FORMAT = &H02       ' [+V2.8.3]
        ENROLL_FAIL_FULL_CREDENTIAL = &H03         ' [+V2.8.3]
        UPDATE_FAIL_FULL_CREDENTIAL = &H03         ' [+V2.8.3]
        ENROLL_FAIL_INVALID_USER = &H04            ' [+V2.8.3]
        UPDATE_FAIL_INVALID_USER = &H04            ' [+V2.8.3]
        ENROLL_FAIL_INTERNAL_ERROR = &H09          ' [+V2.8.3]
        UPDATE_FAIL_INTERNAL_ERROR = &H09          ' [+V2.8.3]

        ' Reason to be failed
        DUAL_AUTH_FAIL_TIMEOUT = &H01
        DUAL_AUTH_FAIL_ACCESS_GROUP = &H02

        ' Bypass mode - soft violation
        BYPASS_NO_VIOLATION = &H00
        BYPASS_THERMAL_VIOLATION = &H01
        BYPASS_MASK_VIOLATION = &H02
        BYPASS_MASK_THERMAL_VIOLATION = &H03

        ' Reason to be failed
        HIGH_TEMPERATURE = &H00
        NO_TEMPERATURE = &H01
        UNMASKED_FACE = &H02

        ' Failed credential
        CREDENTIAL_ID = &H01
        CREDENTIAL_CARD = &H02
        CREDENTIAL_PIN = &H03
        CREDENTIAL_FINGER = &H04
        CREDENTIAL_FACE = &H05
        CREDENTIAL_AOC_PIN = &H06
        CREDENTIAL_AOC_FINGER = &H07
        CREDENTIAL_MOBILE_CARD = &H08
        'CREDENTIAL_QR = 0x09,             // [+ V2.8.2.7]
        CREDENTIAL_QR = &H0C
        NON_NUMERIC_QR = &H09
        NON_PRINTABLE_QR = &H0A
        TOO_LONG_QR = &H0B

        ' Reason to be failed
        AUTH_FAIL_INVALID_AUTH_MODE = &H01
        AUTH_FAIL_INVALID_CREDENTIAL = &H02
        AUTH_FAIL_TIMEOUT = &H03
        AUTH_FAIL_MATCHING_REFUSAL = &H04

        ' Reason to be denied
        ACCESS_DENIED_ACCESS_GROUP = &H01
        ACCESS_DENIED_DISABLED = &H02
        ACCESS_DENIED_EXPIRED = &H03
        ACCESS_DENIED_ON_BLACKLIST = &H04
        ACCESS_DENIED_APB = &H05
        ACCESS_DENIED_TIMED_APB = &H06
        ACCESS_DENIED_SCHEDULED_LOCK = &H07
        ACCESS_DENIED_FORCED_LOCK = &H07        ' Deprecated in V2.4.0.
        ACCESS_EXCUSED_APB = &H08
        ACCESS_EXCUSED_TIMED_APB = &H09
        ACCESS_DENIED_FACE_DETECTION = &H0A
        ACCESS_DENIED_CAMERA_CAPTURE = &H0B
        ACCESS_DENIED_FAKE_FINGER = &H0C
        ACCESS_DENIED_DEVICE_ZONE_ENTRANCE_LIMIT = &H0D
        ACCESS_DENIED_INTRUSION_ALARM = &H0E
        ACCESS_DENIED_INTERLOCK = &H0F
        ACCESS_EXCUSED_AUTH_LIMIT = &H10
        ACCESS_DENIED_AUTH_LIMIT = &H11
        ACCESS_DENIED_ANTI_TAILGATE = &H12
        ACCESS_DENIED_HIGH_TEMPERATURE = &H13
        ACCESS_DENIED_NO_TEMPERATURE = &H14
        ACCESS_DENIED_UNMASKED_FACE = &H15

        ' Door flag type
        DOOR_FLAG_SCHEDULE = BS2DoorFlagEnum.SCHEDULE
        DOOR_FLAG_OPERATOR = BS2DoorFlagEnum.OPERATOR
        DOOR_FLAG_EMERGENCY = BS2DoorFlagEnum.EMERGENCY

        ' Floor flag type
        FLOOR_FLAG_SCHEDULE = BS2FloorFlagEnum.SCHEDULE
        FLOOR_FLAG_OPERATOR = BS2FloorFlagEnum.OPERATOR
        FLOOR_FLAG_ACTION = BS2FloorFlagEnum.ACTION
        FLOOR_FLAG_EMERGENCY = BS2FloorFlagEnum.EMERGENCY

        ' Antipassback violation type
        ZONE_HARD_APB = &H01
        ZONE_SOFT_APB = &H02

        ' Device Zone Entrance limit violation type
        DEVICE_ZONE_HARD_ENTRANCE_LIMIT_COUNT = &H01
        DEVICE_ZONE_SOFT_ENTRANCE_LIMIT_COUNT = &H02
        DEVICE_ZONE_HARD_ENTRANCE_LIMIT_TIME = &H03
        DEVICE_ZONE_SOFT_ENTRANCE_LIMIT_TIME = &H04

        ' InterlockZone violation type
        INTERLOCKZONE_DOOR_OPEN = &H01
        INTERLOCK_INPUT_DETECT = &H02

        ' Authentication Limit violation type
        ZONE_HARD_AUTH_LIMIT = &H01
        ZONE_SOFT_AUTH_LIMIT = &H02
        ZONE_SCHEDULE_AUTH_LIMIT = &H03
        ZONE_COUNT_AUTH_LIMIT = &H04
        ZONE_USER_AUTH_LIMIT = &H05
    End Enum

    <Flags>
    Public Enum BS2EventMaskEnum As UShort
        NONE = 0
        INFO = &H0001
        USER_ID = &H0002
        CARD_ID = &H0004
        DOOR_ID = &H0008
        ZONE_ID = &H0010
        IODEVICE = &H0020
        TNA_KEY = &H0040
        JOB_CODE = &H0080
        IMAGE = &H100
        TEMPERATURE = &H200
        QR_DATA = &H400
        ALL = &HFFFF
    End Enum

    <Flags>
    Public Enum BS2_CRED_KEY_REQ
        BS2_CRED_KEY_REQ_COMM = &H0
        BS2_CRED_KEY_REQ_DATA = &H1
    End Enum

#Region "DEVICE_ZONE_SUPPORTED"
    <Flags>
    Public Enum BS2_DEVICE_ZONE_NODE_TYPE
        BS2_DEVICE_ZONE_NODE_TYPE_MASTER = &H01
        BS2_DEVICE_ZONE_NODE_TYPE_MEMBER = &H02
    End Enum

    Public Enum BS2_DEVICE_ZONE_TYPE
#Region "ENTRNACE_LIMIT"
        BS2_DEVICE_ZONE_TYPE_ENTRANCE_LIMIT = &H03
#End Region
#Region "FIRE_ALRAM"
        BS2_DEVICE_ZONE_TYPE_FIRE_ALARM = &H05
#End Region
    End Enum

#Region "ENTRNACE_LIMIT"
    Public Enum BS2_DEVICE_ZONE_ENTRANCE_LIMIT_TYPE
        BS2_DEVICE_ZONE_ENTRANCE_LIMIT_SOFT = &H01
        BS2_DEVICE_ZONE_ENTRANCE_LIMIT_HARD = &H02
    End Enum

    Public Enum BS2_DEVICE_ZONE_ENTRANCE_LIMIT_DISCONNECTED_ACTION_TYPE
        BS2_DEVICE_ZONE_ENTRANCE_LIMIT_DISCONNECTED_ACTION_SOFT = &H01
        BS2_DEVICE_ZONE_ENTRANCE_LIMIT_DISCONNECTED_ACTION_HARD = &H02
    End Enum
#End Region

#Region "FIRE_ALRAM"
    Public Enum BS2_DEVICE_ZONE_ALARMED_STATUS_TYPE
        BS2_DEVICE_ZONE_ALARMED_DISALARM = &H00
        BS2_DEVICE_ZONE_ALARMED_ALARM = &H01
        BS2_DEVICE_ZONE_ALARMED_SELF = &H02
    End Enum
#End Region
#End Region

    <Flags>
    Public Enum BS2OperationTypeEnum
        INTERLOCK_ZONE_INPUT_SENSOR_OPERATION_MASK_NONE = &H00
        INTERLOCK_ZONE_INPUT_SENSOR_OPERATION_MASK_ENRTY = &H01
        INTERLOCK_ZONE_INPUT_SENSOR_OPERATION_MASK_EXIT = &H02
        INTERLOCK_ZONE_INPUT_SENSOR_OPERATION_MASK_ALL = &HFF
    End Enum

#Region "DEBUG"
    Friend Module Constants
        Public Const DEBUG_MODULE_KEEP_ALIVE As UInteger = &H1 << 0
        Public Const DEBUG_MODULE_SOCKET_MANAGER As UInteger = &H1 << 1
        Public Const DEBUG_MODULE_SOCKET_HANDLER As UInteger = &H1 << 2
        Public Const DEBUG_MODULE_DEVICE As UInteger = &H1 << 3
        Public Const DEBUG_MODULE_DEVICE_MANAGER As UInteger = &H1 << 4
        Public Const DEBUG_MODULE_EVENT_DISPATCHER As UInteger = &H1 << 5
        Public Const DEBUG_MODULE_API As UInteger = &H1 << 6
        Public Const DEBUG_MODULE_ALL As UInteger = &HfffffffFUI
        Public Const DEBUG_LOG_FATAL As UInteger = &H1 << 0
        Public Const DEBUG_LOG_ERROR As UInteger = &H1 << 1
        Public Const DEBUG_LOG_WARN As UInteger = &H1 << 2
        Public Const DEBUG_LOG_INFO As UInteger = &H1 << 3
        'public const UInt32 DEBUG_LOG_TRACE                 = (0x1 << 4);
        Public Const DEBUG_LOG_TRACE As UInteger = &H1 << 8
        Public Const DEBUG_LOG_OPERATION_ALL As UInteger = &H000000fF
        Public Const DEBUG_LOG_ALL As UInteger = &HfffffffFUI
    End Module
#End Region

    <Flags>
    Public Enum BS2ParityTypeEnum
        BS2_WIEGAND_PARITY_NONE = 0
        BS2_WIEGAND_PARITY_ODD = 1
        BS2_WIEGAND_PARITY_EVEN = 2
    End Enum

    <Flags>
    Public Enum BS2WiegandFormatEnum As UShort
        BS2_WIEGAND_H10301_26 = 0
        BS2_WIEGAND_H10302_37 = 1
        BS2_WIEGAND_H10304_37 = 2
        BS2_WIEGAND_C1000_35 = 3
        BS2_WIEGAND_C1000_48 = 4
    End Enum

    <Flags>
    Public Enum BS2WiegandModeEnum
        BS2_WIEGAND_IN_ONLY = 0
        BS2_WIEGAND_OUT_ONLY = 1
        BS2_WIEGAND_IN_OUT = 2
    End Enum

    <Flags>
    Public Enum BS2AuthModeEnum
        BS2_AUTH_MODE_NONE = 255
        BS2_AUTH_MODE_PROHIBITED = 254

        BS2_AUTH_MODE_BIOMETRIC_ONLY = 0
        BS2_AUTH_MODE_BIOMETRIC_PIN = 1

        BS2_AUTH_MODE_CARD_ONLY = 2
        BS2_AUTH_MODE_CARD_BIOMETRIC = 3
        BS2_AUTH_MODE_CARD_PIN = 4
        BS2_AUTH_MODE_CARD_BIOMETRIC_OR_PIN = 5
        BS2_AUTH_MODE_CARD_BIOMETRIC_PIN = 6

        BS2_AUTH_MODE_ID_BIOMETRIC = 7
        BS2_AUTH_MODE_ID_PIN = 8
        BS2_AUTH_MODE_ID_BIOMETRIC_OR_PIN = 9
        BS2_AUTH_MODE_ID_BIOMETRIC_PIN = 10

        BS2_NUM_OF_AUTH_MODE
    End Enum

    <Flags>
    Public Enum BS2DesfireCardEncryption
        BS2_DESFIRECARD_ENCRYPTION_DES_3DES = 0
        BS2_DESFIRECARD_ENCRYPTION_AES = 1
    End Enum

    <Flags>
    Public Enum BS2DesfireCardOperation
        BS2_DESFIRECARD_OPERATION_MODE_LEGACY = 0
        BS2_DESFIRECARD_OPERATION_MODE_APPLEVELKEY = 1
    End Enum

    '=> [IPv6]
    Public Enum BS2SpecifiedDeviceInfo As UInteger
        BS2_SPECIFIED_DEVICE_INFO_SIMPLE = 1
        BS2_SPECIFIED_DEVICE_INFO_SIMPLE_EX = 2
        BS2_SPECIFIED_DEVICE_INFO_IPV6 = 3
    End Enum
    '<=

    ' Log image field (BS2Event.image) -> 1 byte
    ' BS2Event.image 00000000
    '---------------------------------
    '   Image        00000001
    '   DST          00000010
    '   Half         00000100
    '   Hour         01111000
    '   Negative     10000000
    Public Enum BS2EventImageBitPos As Byte
        BS2_IMAGEFIELD_POS_IMAGE = &H01
        BS2_IMAGEFIELD_POS_DST = &H02
        BS2_IMAGEFIELD_POS_HALF = &H78
        BS2_IMAGEFIELD_POS_HOUR = &H80
    End Enum

    <Flags>
    Public Enum BS2SystemConfigCardOperationMask As UInteger
        CARD_OPERATION_MASK_DEFAULT = &HFFFFFFFFUI
        CARD_OPERATION_USE = &H80000000UI
        CARD_OPERATION_MASK_CUSTOM_DESFIRE_EV1 = &H00000800
        CARD_OPERATION_MASK_CUSTOM_CLASSIC_PLUS = &H00000400
        CARD_OPERATION_MASK_BLE = &H00000200
        CARD_OPERATION_MASK_NFC = &H00000100
        CARD_OPERATION_MASK_SEOS = &H00000080
        CARD_OPERATION_MASK_SR_SE = &H00000040
        CARD_OPERATION_MASK_DESFIRE_EV1 = &H00000020
        CARD_OPERATION_MASK_CLASSIC_PLUS = &H00000010
        CARD_OPERATION_MASK_ICLASS = &H00000008
        CARD_OPERATION_MASK_MIFARE_FELICA = &H00000004
        CARD_OPERATION_MASK_HIDPROX = &H00000002
        CARD_OPERATION_MASK_EM = &H00000001
    End Enum

    <Flags>
    Public Enum BS2SupportedInfoMask As UInteger
        BS2_SUPPORT_RS485EX = &H00000001
        BS2_SUPPORT_CARDEX = &H00000002
        BS2_SUPPORT_DST = &H00000004
        BS2_SUPPORT_DESFIREEX = &H00000008
        BS2_SUPPORT_FACE_EX = &H00000010       ' F2 support
        BS2_SUPPORT_QR = &H00000020            ' [+2.8]

        BS2_SUPPORT_FINGER_SCAN = &H00010000
        BS2_SUPPORT_FACE_SCAN = &H00020000
        BS2_SUPPORT_FACE_EX_SCAN = &H00040000
        BS2_SUPPORT_QR_SCAN = &H00080000       ' [+2.8]

        BS2_SUPPORT_ALL = BS2_SUPPORT_RS485EX Or BS2_SUPPORT_CARDEX Or BS2_SUPPORT_DST Or BS2_SUPPORT_DESFIREEX Or BS2_SUPPORT_FACE_EX Or BS2_SUPPORT_QR Or BS2_SUPPORT_FINGER_SCAN Or BS2_SUPPORT_FACE_SCAN Or BS2_SUPPORT_FACE_EX_SCAN Or BS2_SUPPORT_QR_SCAN
    End Enum

    <Flags>
    Public Enum BS2DeviceStatus
        NORMAL
        LOCKED
        RTC_ERROR
        WAITING_INPUT
        WAITING_DHCP
        SCAN_FINGER
        SCAN_CARD
        SUCCESS
        FAIL
        DURESS
        PROCESS_CONFIG_CARD
        SUCCESS_CONFIG_CARD
        SCAN_FACE
        RESERVED3
        RESERVED4

        NUM_OF_STATUS
    End Enum

    <Flags>
    Public Enum BS2CapabilitySystemSupport
        SYSTEM_SUPPORT_CAMERA = &H01
        SYSTEM_SUPPORT_TAMPER = &H02
        SYSTEM_SUPPORT_WLAN = &H04
        SYSTEM_SUPPORT_DISPLAY = &H08
        SYSTEM_SUPPORT_THERMAL = &H10
        SYSTEM_SUPPORT_MASK = &H20
        SYSTEM_SUPPORT_FACEEX = &H40
        SYSTEM_SUPPORT_VOIPEX = &H80         ' [+V2.8.3]
    End Enum

    <Flags>
    Public Enum BS2CapabilityCardSupport As UInteger
        CARD_SUPPORT_EM = &H00000001
        CARD_SUPPORT_HIDPROX = &H00000002
        CARD_SUPPORT_MIFAREFELICA = &H00000004
        CARD_SUPPORT_ICLASS = &H00000008
        CARD_SUPPORT_CLASSICPLUS = &H00000010
        CARD_SUPPORT_DESFIREEV1 = &H00000020
        CARD_SUPPORT_SRSE = &H00000040
        CARD_SUPPORT_SEOS = &H00000080
        CARD_SUPPORT_NFC = &H00000100
        CARD_SUPPORT_BLE = &H00000200
        CARD_SUPPORT_CUSTOMCLASSICPLUS = &H00000400   ' [V2.9.6]
        CARD_SUPPORT_CUSTOMDESFIREEV1 = &H00000800   ' [V2.9.6]
        CARD_SUPPORT_TOM_NFC = &H00001000   ' [V2.9.6]
        CARD_SUPPORT_TOM_BLE = &H00002000   ' [V2.9.6]
        CARD_SUPPORT_CUSTOMFELICA = &H00004000
        CARD_SUPPORT_USECARDOPERATION = &H80000000UI
    End Enum

    <Flags>
    Public Enum BS2CapabilityFunctionSupport
        FUNCTION_SUPPORT_INTELLIGENTPD = &H01
        FUNCTION_SUPPORT_UPDATEUSER = &H02     ' [V2.8.3]
        FUNCTION_SUPPORT_SIMULATEDUNLOCK = &H04     ' [V2.8.3]
        FUNCTION_SUPPORT_SMARTCARDBYTEORDER = &H08
        FUNCTION_SUPPORT_TREATASCSN = &H10
        FUNCTION_SUPPORT_RTSP = &H20     ' [V2.8.3]
        FUNCTION_SUPPORT_LFD = &H40     ' [V2.8.3]
        FUNCTION_SUPPORT_VISUALQR = &H80     ' [V2.8.3]
    End Enum

    <Flags>
    Public Enum BS2CapabilityFunctionSupport2
        FUNCTION2_SUPPORT_OSDPSTANDARDCENTRAL = &H01  ' [V2.9.1]
        FUNCTION2_SUPPORT_ENABLELICENSE = &H02  ' [V2.9.1]
        FUNCTION2_SUPPORT_KEYPADBACKLIGHT = &H04  ' [V2.9.4]
        FUNCTION2_SUPPORT_UZWIRELESSLOCKDOOR = &H08  ' [V2.9.4]
        FUNCTION2_SUPPORT_CUSTOMSMARTCARD = &H10  ' [V2.9.4]
        FUNCTION2_SUPPORT_TOM = &H20  ' [V2.9.4]
        FUNCTION2_SUPPORT_TOMENROLL = &H40  ' [V2.9.6]
        FUNCTION2_SUPPORT_SHOWOSDPRESULTBYLED = &H80  ' [V2.9.6]
    End Enum

    <Flags>
    Public Enum BS2CapabilityFunctionSupport3
        FUNCTION3_SUPPORT_CUSTOMSMARTCARDFELICA = &H01  ' [V2.9.6]
        FUNCTION3_SUPPORT_IGNOREINPUTAFTERWIEGANDOUT = &H02  ' [V2.9.6]
        FUNCTION3_SUPPORT_SETSLAVEBAUDRATE = &H04  ' [V2.9.6]
        FUNCTION3_SUPPORT_RTSP_RESOLUTIONCHANGE = &H08  ' [V2.9.8]
        FUNCTION3_SUPPORT_VOIP_RESOLUTIONCHANGE = &H10  ' [V2.9.8]
        FUNCTION3_SUPPORT_VOIP_TRANSPORTCHANGE = &H20  ' [V2.9.8]
        FUNCTION3_SUPPORT_AUTHMSG_USERINFO = &H40  ' [V2.9.8]
        FUNCTION3_SUPPORT_SCRAMBLEKEYBOARDMODE = &H80  ' [V2.9.8]
    End Enum

    <Flags>
    Public Enum BS2CapabilityFunctionSupport4
        FUNCTION4_SUPPORT_AUTHDENYMASK = &H01  ' [V2.9.8]
    End Enum

    <Flags>
    Public Enum BS2SupervisedResistor
        SUPERVISED_RESISTOR_1K = 0
        SUPERVISED_RESISTOR_2_2K = 1
        SUPERVISED_RESISTOR_4_7K = 2
        SUPERVISED_RESISTOR_10K = 3

        SUPERVISED_RESISTOR_UNUSED = 254
    End Enum

    <Flags>
    Public Enum BS2RelayActionInputType
        RELAY_ACTION_INPUT_TYPE_NONE = &H00
        RELAY_ACTION_INPUT_TYPE_LINKAGE = &H01
        RELAY_ACTION_INPUT_TYPE_LATCHING = &H02
        RELAY_ACTION_INPUT_TYPE_RELEASE = &H03
    End Enum

    <Flags>
    Public Enum BS2RelayActionInputMask
        RELAY_ACTION_INPUT_MASK_NONE = &H00
        RELAY_ACTION_INPUT_MASK_ALARM = &H01
        RELAY_ACTION_INPUT_MASK_FAULT = &H02
    End Enum

    <Flags>
    Public Enum BS2MotionSensitivity                ' + 2.9.1
        LOW = 0
        NORMAL = 1
        HIGH = 2
    End Enum

    <Flags>
    Public Enum BS2LicenseStatus                    ' + 2.9.1
        NOT_SUPPORTED = 0
        DISABLE = 1
        ENABLE = 2
        EXPIRED = 3
    End Enum

    <Flags>
    Public Enum BS2LicenseType                      ' + 2.9.1
        NONE = &H0000
        VISUAL_QR_MASK = &H0001
        MAX_MASK = VISUAL_QR_MASK
    End Enum

    <Flags>
    Public Enum BS2LicenseSubType                   ' + 2.9.1
        NONE = 0
        VISUAL_QR_CODE_CORP = 1
    End Enum

    <Flags>
    Public Enum BS2OsdpStandardActionType           ' + 2.9.1
        COUNT = 3

        NONE = 0
        SUCCESS = 1
        FAIL = 2
        WAIT_INPUT = 3
    End Enum

    <Flags>
    Public Enum BS2OsdpStandardLEDCommand           ' + 2.9.1
        NOP = 0
        CANCEL = 1
        [SET] = 2
    End Enum

    <Flags>
    Public Enum BS2OsdpStandardColor                ' + 2.9.1
        BLACK = 0
        RED
        GREEN
        AMBER
        BLUE
        MAGENTA
        CYAN
        WHITE
    End Enum

    <Flags>
    Public Enum BS2OsdpStandardTone                 ' + 2.9.1
        NONE = 0
        OFF = 1
        [ON] = 2
    End Enum

    <Flags>
    Public Enum BS2FaceExFlag
        NONE = &H00
        WARPED = &H01
        TEMPLATE_ONLY = &H20
        ALL = &HFF
    End Enum

    <Flags>
    Public Enum BS2StopFlag
        NONE = &H00
        ON_DOOR_CLOSED = &H01
        BY_CMD_RUN_ACTION = &H02
    End Enum
End Namespace
