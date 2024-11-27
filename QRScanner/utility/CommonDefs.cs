using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QRScanner.utility
{
    /*
     * This file includes common definitions provided by Zebra, along with two custom classes 
     * designed to manage operation codes (opcodes) and status codes efficiently.
     */

    /// <summary>
    /// Scanner Types
    /// </summary>
    public enum ScannerType
    {
        /// <summary>
        /// All Scanner Types mentioned below
        /// </summary>
        All = 1,
        /// <summary>
        /// Symbol Native API (SNAPI) with Imaging Interface
        /// </summary>
        Snapi = 2,
        /// <summary>
        /// Simple Serial Interface (SSI) over RS-232
        /// </summary>
        SsiRs232 = 3,
        /// <summary>
        /// IBM Hand-held USB
        /// </summary>
        IbmHandheldUsb = 6,
        /// <summary>
        /// WincorNixdorf Mode B
        /// </summary>
        NixdorfModeB = 7,
        /// <summary>
        /// USB Keyboard HID
        /// </summary>
        UsbKeyboardHid = 8,
        /// <summary>
        /// IBM Table-top USB
        /// </summary>
        IbmTableTopUsb = 9,
        /// <summary>
        /// Simple Serial Interface (SSI) over Bluetooth Classic
        /// </summary>
        SsiBluetoothClassic = 11,
        /// <summary>
        /// OPOS (IBM Hand-held)
        /// </summary>
        Opos = 13
    }

    /// <summary>
    /// Event Types
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// Barcode event Type
        /// </summary>
        Barcode = 1,

        /// <summary>
        /// Image event Type
        /// </summary>
        Image = 2,

        /// <summary>
        /// Video event Type
        /// </summary>
        Video = 4,

        /// <summary>
        /// RMD event Type
        /// </summary>
        Rmd = 8,

        /// <summary>
        /// PNP event Type
        /// </summary>
        Pnp = 16,

        /// <summary>
        /// Other event Types
        /// </summary>
        Other = 32,
    }

    /// <summary>
    /// Available Values For 'Status' 
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Status success
        /// </summary>
        Success = 0,

        /// <summary>
        /// Status locked
        /// </summary>
        Locked = 10
    }

    /// <summary>
    /// Barcode Event Type
    /// </summary>
    public enum DecodeEventType
    {
        /// <summary>
        /// Successful decode
        /// </summary>
        ScannerDecodeGood = 1
    }

    /// <summary>
    /// Video Event Type
    /// </summary>
    public enum VideoEventType
    {
        /// <summary>
        /// Complete video frame captured
        /// </summary>
        VideoFrameComplete = 1
    }


    /// <summary>
    /// Image Event Type
    /// </summary>
    public enum ImageEventType
    {
        /// <summary>
        /// Image captured
        /// </summary>
        ImageComplete = 1,

        /// <summary>
        /// Image error or status
        /// </summary>
        ImageTranStatus = 2,
    }

    /// <summary>
    /// Device Notification 
    /// </summary>
    public enum DeviceNotification
    {
        /// <summary>
        /// Scanner attached
        /// </summary>
        ScannerAttached = 0,

        /// <summary>
        /// Scanner detached
        /// </summary>
        ScannerDetached = 1
    }

    /// <summary>
    /// Scanner Notification
    /// </summary>
    public enum ScannerNotification
    {
        /// <summary>
        /// Barcode mode
        /// </summary>
        BarcodeMode = 1,

        /// <summary>
        /// Image mode
        /// </summary>
        ImageMode = 2,

        /// <summary>
        /// Video mode
        /// </summary>
        VideoMode = 3,

        /// <summary>
        /// Device enabled
        /// </summary>
        DeviceEnabled = 13,

        /// <summary>
        /// Device disabled
        /// </summary>
        DeviceDisabled = 14
    }

    /// <summary>
    /// Firmware Download Events
    /// </summary>
    public enum FirmwareDownloadEvent
    {
        /// <summary>
        /// Triggered when flash download session starts 
        /// </summary>
        ScannerUfSessionStart = 11,

        /// <summary>
        /// Triggered when component download starts 
        /// </summary>
        ScannerUfDownloadStart = 12,

        /// <summary>
        /// Triggered when block(s) Of flash completed 
        /// </summary>
        ScannerUfDownloadProgress = 13,

        /// <summary>
        /// Triggered when component download ends 
        /// </summary>
        ScannerUfDownloadEnd = 14,

        /// <summary>
        /// Triggered when flash download session ends 
        /// </summary>
        ScannerUFSessionEnd = 15
    }

    /// <summary>
    /// File Types
    /// Please refer scanner PRGs for more information on scanner parameters.   
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Jpeg file
        /// </summary>
        JpegFileSelection = 1,

        /// <summary>
        /// Bmp file
        /// </summary>
        BmpFileSelection = 3,

        /// <summary>
        /// Tiff file
        /// </summary>
        TiffFileSelection = 4
    }

    /// <summary>
    /// Video View Finder 
    /// </summary>
    public enum VideoViewFinder
    {
        /// <summary>
        ///  Video View Finder On  
        /// </summary>
        On = 1,

        /// <summary>
        /// Video View Finder Off
        /// </summary>
        Off = 0
    }

    /// <summary>
    /// Param Persistance
    /// </summary>
    public enum ParamPersistance
    {
        /// <summary>
        /// Parameters Persistance On
        /// </summary>
        ParamPersistanceOn = 1,

        /// <summary>
        /// Parameters Persistance On
        /// </summary>
        ParamPersistanceOff = 0
    }

    /// <summary>
    /// Beep Codes
    /// Please refer scanner PRGs for more information on beep codes.   
    /// </summary>
    public enum BeepCode
    {
        OneShortHigh = 0x00,
        TwoShortHigh = 0x01,
        ThreeShortHigh = 0x02,
        FourShortHigh = 0x03,
        FiveShortHigh = 0x04,

        OneShortLow = 0x05,
        TwoShortLow = 0x06,
        ThreeShortLow = 0x07,
        FourShortLow = 0x08,
        FiveShortLow = 0x09,

        OneLongHigh = 0x0a,
        TwoLongHigh = 0x0b,
        ThreeLongHigh = 0x0c,
        FourLongHigh = 0x0d,
        FiveLongHigh = 0x0e,

        OneLongLow = 0x0f,
        TwoLongLow = 0x10,
        ThreeLongLow = 0x11,
        FourLongLow = 0x12,
        FiveLongLow = 0x13,

        FastHighLowHighLow = 0x14,
        SlowHighLowHighLow = 0x15,
        HighLow = 0x16,
        LowHigh = 0x17,
        HighLowHigh = 0x18,
        LowHighLow = 0x19
    }

    /// <summary>
    /// LED Codes
    /// Please refer scanner PRGs for more information on LED codes.   
    /// </summary>
    public enum LEDCode
    {
        /// <summary>
        /// Green  Led On
        /// </summary>
        Led1On = 0x2B,

        /// <summary>
        /// Yellow  Led On
        /// </summary>
        Led2On = 0x2D,

        /// <summary>
        /// Red  Led On 
        /// </summary>
        Led3On = 0x2F,

        /// <summary>
        /// Green  Led Off 
        /// </summary>
        Led1Off = 0x2A,

        /// <summary>
        /// Yellow  Led Off 
        /// </summary>
        Led2Off = 0x2E,

        /// <summary>
        /// Red  Led Off
        /// </summary>
        Led3Off = 0x30
    }

    /// <summary>
    /// CoreScanner Opcodes
    /// Please refer Scanner SDK for Windows Developer Guide for more information on opcodes.   
    /// </summary>
    public enum Opcode
    {
        /// <summary>
        /// Gets the version of CoreScanner
        /// </summary>
        GetVersion = 1000,

        /// <summary>
        /// Register for API events
        /// </summary>
        RegisterForEvents = 1001,

        /// <summary>
        /// Unregister for API events
        /// </summary>
        UnregisterForEvents = 1002,

        /// <summary>
        /// Get Bluetooth scanner pairing bar code
        /// </summary>
        GetPairingBarcode = 1005,

        /// <summary>
        /// Claim a specific device
        /// </summary>
        ClaimDevice = 1500,

        /// <summary>
        /// Release a specific device
        /// </summary>
        ReleaseDevice = 1501,

        /// <summary>
        /// Abort MacroPDF of a specified scanner
        /// </summary>
        AbortMacroPdf = 2000,

        /// <summary>
        /// Abort firmware update process of a specified scanner, while in progress
        /// </summary>
        AbortUpdateFirmware = 2001,

        /// <summary>
        /// Turn Aim off
        /// </summary>
        DeviceAimOff = 2002,

        /// <summary>
        /// Turn Aim on
        /// </summary>
        DeviceAimOn = 2003,

        /// <summary>
        /// Flush MacroPDF of a specified scanner
        /// </summary>
        FlushMacroPdf = 2005,

        /// <summary>
        /// Pull the trigger of a specified scanner
        /// </summary>
        DevicePullTrigger = 2011,

        /// <summary>
        /// Release the trigger of a specified scanner
        /// </summary>
        DeviceReleaseTrigger = 2012,

        /// <summary>
        /// Disable scanning on a specified scanner
        /// </summary>
        DeviceScanDisable = 2013,

        /// <summary>
        /// Enable scanning on a specified scanner
        /// </summary>
        DeviceScanEnable = 2014,

        /// <summary>
        /// Set parameters to default values of a specified scanner
        /// </summary>
        SetParameterDefaults = 2015,

        /// <summary>
        /// Set parameters of a specified scanner
        /// </summary>
        DeviceSetParameters = 2016,

        /// <summary>
        /// Set and persist parameters of a specified scanner
        /// </summary>
        SetParameterPersistance = 2017,

        /// <summary>
        /// Reboot a specified scanner
        /// </summary>
        RebootScanner = 2019,

        /// <summary>
        /// Disconnect the specified Bluetooth scanner
        /// </summary>
        DisconnectBluetoothScanner = 2023,

        /// <summary>
        /// Change a specified scanner to snapshot mode 
        /// </summary>
        DeviceCaptureImage = 3000,

        /// <summary>
        /// Change a specified scanner to decode mode 
        /// </summary>
        DeviceCaptureBarcode = 3500,

        /// <summary>
        /// Change a specified scanner to video mode 
        /// </summary>
        DeviceCaptureVideo = 4000,

        /// <summary>
        /// Get all the attributes of a specified scanner
        /// </summary>
        RsmAttrGetAll = 5000,

        /// <summary>
        /// Get the attribute values(s) of specified scanner
        /// </summary>
        RsmAttrGet = 5001,

        /// <summary>
        /// Get the next attribute to a given attribute of specified scanner
        /// </summary>
        RsmAttrGetNext = 5002,

        /// <summary>
        /// Set the attribute values(s) of specified scanner
        /// </summary>
        RsmAttrSet = 5004,

        /// <summary>
        /// Store and persist the attribute values(s) of specified scanner
        /// </summary>
        RsmAttrStore = 5005,

        /// <summary>
        /// Get the topology of the connected devices
        /// </summary>
        GetDeviceTopology = 5006,

        /// <summary>
        /// Remove all Symbol device entries from registry
        /// </summary>
        UninstallSymbolDevices = 5010,

        /// <summary>
        /// Start (flashing) the updated firmware
        /// </summary>
        StartNewFirmware = 5014,

        /// <summary>
        /// Update the firmware to a specified scanner
        /// </summary>
        UpdateFirmware = 5016,

        /// <summary>
        /// Update the firmware to a specified scanner using a scanner plug-in
        /// </summary>
        UpdateFirmwareFromPlugin = 5017,

        /// <summary>
        /// Update good scan tone of the scanner with specified wav file
        /// </summary>
        UpdateDecodeTone = 5050,

        /// <summary>
        /// Erase good scan tone of the scanner
        /// </summary>
        EraseDecodeTone = 5051,

        /// <summary>
        /// Perform an action involving scanner beeper/LEDs
        /// </summary>
        SetAction = 6000,

        /// <summary>
        /// Set the serial port settings of a NIXDORF Mode-B scanner
        /// </summary>
        DeviceSetSerialPortSettings = 6101,

        /// <summary>
        /// Switch the USB host mode of a specified scanner
        /// </summary>
        DeviceSwitchHostMode = 6200,

        /// <summary>
        /// Switch CDC devices
        /// </summary>
        SwitchCdcDevices = 6201,

        /// <summary>
        /// Enable/Disable keyboard emulation mode
        /// </summary>
        KeyboardEmulatorEnable = 6300,

        /// <summary>
        /// Set the locale for keyboard emulation mode
        /// </summary>
        KeyboardEmulatorSetLocale = 6301,

        /// <summary>
        /// Get current configuration of the HID keyboard emulator
        /// </summary>
        KeyboardEmulatorGetConfig = 6302,

        /// <summary>
        ///  Configure Driver ADF
        /// </summary>
        ConfigureDADF = 6400,

        /// <summary>
        /// Reset Driver ADF
        /// </summary>
        ResetDADF = 6401,

        /// <summary>
        /// Measure the weight on the scanner's platter and get the value
        /// </summary>
        ScaleReadWeight = 7000,

        /// <summary>
        ///  Zero the scale
        /// </summary>
        ScaleZeroScale = 7002,

        /// <summary>
        /// Reset the scale
        /// </summary>
        ScaleSystemReset = 7015,
    }

    /// <summary>
    /// Code Types
    /// Please refer Scanner SDK for Windows Developer Guide for more information on code types.   
    /// </summary>
    public enum CodeType
    {
        Unknown = 0x00,
        Code39 = 0x01,
        Codabar = 0x02,
        Code128 = 0x03,
        Discrete2of5 = 0x04,
        Iata = 0x05,
        Interleaved2of5 = 0x06,
        Code93 = 0x07,
        UpcA = 0x08,
        UpcE0 = 0x09,
        Ean8 = 0x0A,
        Ean13 = 0x0B,
        Code11 = 0x0C,
        Code49 = 0x0D,
        Msi = 0x0E,
        Ean128 = 0x0F,
        UpcE1 = 0x10,
        Pdf417 = 0x11,
        Code16k = 0x12,
        Code39FullAscii = 0x13,
        UpcD = 0x14,
        Code39Trioptic = 0x15,
        Bookland = 0x16,
        UpcaWCode128 = 0x17, // For Upc-A W/Code 128 Supplemental
        Jan13WCode128 = 0x78, // For Ean/Jan-13 W/Code 128 Supplemental
        Nw7 = 0x18,
        Isbt128 = 0x19,
        MicroPdf = 0x1a,
        Datamatrix = 0x1b,
        Qrcode = 0x1c,
        MicroPdfCca = 0x1d,
        PostnetUs = 0x1e,
        PlanetCode = 0x1f,
        Code32 = 0x20,
        Isbt128Con = 0x21,
        JapanPostal = 0x22,
        AusPostal = 0x23,
        DutchPostal = 0x24,
        Maxicode = 0x25,
        CanadinPostal = 0x26,
        UkPostal = 0x27,
        MacroPdf = 0x28,
        MacroQrCode = 0x29,
        MicroQrCode = 0x2c,
        Aztec = 0x2d,
        AztecRune = 0x2e,
        Distance = 0x2f,
        Rss14 = 0x30,
        RssLimited = 0x31,
        RssExpanded = 0x32,
        Parameter = 0x33,
        Usps4c = 0x34,
        UpuFicsPostal = 0x35,
        Issn = 0x36,
        Scanlet = 0x37,
        Cuecode = 0x38,
        Matrix2of5 = 0x39,
        Upca_2 = 0x48,
        Upce0_2 = 0x49,
        Ean8_2 = 0x4a,
        Ean13_2 = 0x4b,
        Upce1_2 = 0x50,
        CcaEan128 = 0x51,
        CcaEan13 = 0x52,
        CcaEan8 = 0x53,
        CcaRssExpanded = 0x54,
        CcaRssLimited = 0x55,
        CcaRss14 = 0x56,
        CcaUpca = 0x57,
        CcaUpce = 0x58,
        CccEan128 = 0x59,
        Tlc39 = 0x5a,
        CcbEan128 = 0x61,
        CcbEan13 = 0x62,
        CcbEan8 = 0x63,
        CcbRssExpanded = 0x64,
        CcbRssLimited = 0x65,
        CcbRss14 = 0x66,
        CcbUpca = 0x67,
        CcbUpce = 0x68,
        SignatureCapture = 0x69,
        Moa = 0x6a,
        Pdf417Parameter = 0x70,
        Chinese2of5 = 0x72,
        Korean3Of5 = 0x73,
        DatamatrixParam = 0x74,
        CodeZ = 0x75,
        Upca_5 = 0x88,
        Upce0_5 = 0x89,
        Ean8_5 = 0x8a,
        Ean13_5 = 0x8b,
        Upce1_5 = 0x90,
        MacroMicroPdf = 0x9a,
        OcrB = 0xa0,
        OcrA = 0xa1,
        ParsedDriverLicense = 0xb1,
        ParsedUid = 0xb2,
        ParsedNdc = 0xb3,
        DatabarCoupon = 0xb4,
        ParsedXml = 0xb6,
        HanXinCode = 0xb7,
        Calibration = 0xc0,
        Gs1Datamatrix = 0xc1,
        Gs1Qr = 0xc2,
        Mainmark = 0xc3,
        Dotcode = 0xc4,
        GridMatrix = 0xc8,
    }

    /// <summary>
    /// RSM Data Types
    /// </summary>
    public enum RSMDataType
    {
        /// <summary>
        /// Byte  Unsigned char
        /// </summary>
        B,

        /// <summary>
        /// Char  Signed byte
        /// </summary>
        C,

        /// <summary>
        /// Bit flags
        /// </summary>
        F,

        /// <summary>
        /// Word  Short unsigned integer (16 Bits)
        /// </summary>
        W,

        /// <summary>
        /// Sword  Short signed integer (16 Bits)
        /// </summary>
        I,

        /// <summary>
        /// Dword  Long unsigned integer (32 Bits)
        /// </summary>
        D,

        /// <summary>
        /// Sdword  Long signed integer (32 Bits)
        /// </summary>
        L,

        /// <summary>
        /// Array
        /// </summary>
        A,

        /// <summary>
        /// String
        /// </summary>
        S,

        /// <summary>
        /// Action
        /// </summary>
        X
    }

    /// <summary>
    /// Common constants definition class
    /// </summary>
    public static class Constant
    {
        /// Triggered when update error or status
        public const int ScannerUfStatus = 16;

        /// Maximum number of scanners to be connected
        public const int MaxNumDevices = 255;

        /// Video View Finder paaram number
        public const ushort VideoViewFinderParamNum = 0x0144;

        /// Image Filter Type param number
        public const ushort ImageFilterTypeParamNum = 0x0130;  // These values may change with the scanner   

        /// Maximum buffer size
        public const int MaxBuffSize = 1024;

        /// Maximum number of bytes per parameter    
        public const int MaxParamLength = 2;

        /// Maximum number of bytes for serial number
        public const uint MaxSerialNumberLength = 255;

        /// Wave file buffer Size (Default file size Is 10kb)
        public const int WavFileMaxSize = 10240;

    }

    /// <summary>
    /// A static class providing constant integer values for all supported operation codes (opcodes).
    /// These opcodes represent specific commands that can be executed on Zebra scanners.
    /// </summary>
    /// <remarks>
    /// The class is categorized based on the functionality of the commands, such as:
    /// <list type="bullet">
    /// <item>Scanner SDK Commands</item>
    /// <item>Scanner Access Control Commands</item>
    /// <item>Scanner Common Commands</item>
    /// <item>Scanner Operation Mode Commands</item>
    /// <item>Scanner Management Commands</item>
    /// <item>Real-Time Alerts (RTA) Configuration Commands</item>
    /// <item>Serial Scanner Commands</item>
    /// <item>Keyboard Emulator Commands</item>
    /// <item>Scale Commands</item>
    /// </list>
    /// </remarks>
    public static class OpcodesHandler
    {
        // Scanner SDK Commands
        public const int GET_VERSION = 1000;
        public const int REGISTER_FOR_EVENTS = 1001;
        public const int UNREGISTER_FOR_EVENTS = 1002;

        // Scanner Access Control Commands
        public const int CLAIM_DEVICE = 1500;
        public const int RELEASE_DEVICE = 1501;

        // Scanner Common Commands
        public const int ABORT_MACROPDF = 2000;
        public const int ABORT_UPDATE_FIRMWARE = 2001;
        public const int AIM_OFF = 2002;
        public const int AIM_ON = 2003;
        public const int FLUSH_MACROPDF = 2005;
        public const int DEVICE_PULL_TRIGGER = 2011;
        public const int DEVICE_RELEASE_TRIGGER = 2012;
        public const int SCAN_DISABLE = 2013;
        public const int SCAN_ENABLE = 2014;
        public const int SET_PARAMETER_DEFAULTS = 2015;
        public const int DEVICE_SET_PARAMETERS = 2016;
        public const int SET_PARAMETER_PERSISTANCE = 2017;
        public const int REBOOT_SCANNER = 2019;

        // Scanner Operation Mode Commands
        public const int DEVICE_CAPTURE_IMAGE = 3000;
        public const int DEVICE_CAPTURE_BARCODE = 3500;
        public const int DEVICE_CAPTURE_VIDEO = 4000;

        // Scanner Management Commands
        public const int ATTR_GETALL = 5000;
        public const int ATTR_GET = 5001;
        public const int ATTR_GETNEXT = 5002;
        public const int ATTR_SET = 5004;
        public const int ATTR_STORE = 5005;
        public const int GET_DEVICE_TOPOLOGY = 5006;
        public const int START_NEW_FIRMWARE = 5014;
        public const int UPDATE_FIRMWARE = 5016;
        public const int UPDATE_FIRMWARE_FROM_PLUGIN = 5017;
        public const int UPDATE_DECODE_TONE = 5050;
        public const int ERASE_DECODE_TONE = 5051;

        // RTA Configuration Commands (Real-Time Alerts)
        public const int GET_SUPPORTED_RTA_EVENTS = 5500;
        public const int REGISTER_RTA_EVENTS = 5501;
        public const int UNREGISTER_RTA_EVENTS = 5502;
        public const int GET_RTA_ALERT_STATUS = 5503;
        public const int SET_RTA_ALERT_STATUS = 5504;
        public const int RTA_SUSPEND = 5505;
        public const int RTA_STATE = 5506;

        // Scanner Action Commands
        public const int SET_ACTION = 6000;

        // Serial Scanner Commands
        public const int DEVICE_SET_SERIAL_PORT_SETTINGS = 6101;

        // Other Commands
        public const int DEVICE_SWITCH_HOST_MODE = 6200;

        // Keyboard Emulator Commands
        public const int KEYBOARD_EMULATOR_ENABLE = 6300;
        public const int KEYBOARD_EMULATOR_SET_LOCALE = 6301;
        public const int KEYBOARD_EMULATOR_GET_CONFIG = 6302;

        // Scale Commands
        public const int SCALE_READ_WEIGHT = 7000;
        public const int SCALE_ZERO_SCALE = 7002;
        public const int SCALE_SYSTEM_RESET = 7015;
    }

    /// <summary>
    /// A static class for managing and interpreting status codes returned by the Zebra CoreScanner API.
    /// </summary>
    /// <remarks>
    /// The class provides:
    /// <list type="bullet">
    /// <item>A dictionary mapping status codes to their human-readable descriptions.</item>
    /// <item>A method to handle status codes and return their descriptions.</item>
    /// </list>
    /// </remarks>
    public static class StatusHandler
    {
        private static readonly Dictionary<int, string> StatusMessages = new Dictionary<int, string>
        {
            { 0, "Generic success." },
            { 10, "Device is locked by another application." },
            { 100, "Invalid application handle. Reserved parameter. Value is zero." },
            { 101, "Required Comm Lib is unavailable to support the requested Type." },
            { 102, "Null buffer pointer." },
            { 103, "Invalid buffer pointer." },
            { 104, "Incorrect buffer size." },
            { 105, "Requested Type IDs are duplicated." },
            { 106, "Incorrect value for number of Types." },
            { 107, "Invalid argument." },
            { 108, "Invalid scanner ID." },
            { 109, "Incorrect value for number of Event IDs." },
            { 110, "Event IDs are duplicated." },
            { 111, "Invalid value for Event ID." },
            { 112, "Required device is unavailable." },
            { 113, "Opcode is invalid." },
            { 114, "Invalid value for Type." },
            { 115, "Opcode does not support asynchronous method." },
            { 116, "Device does not support the Opcode." },
            { 117, "Operation failed in device." },
            { 118, "Request failed in CoreScanner." },
            { 119, "Operation not supported for auxiliary scanners." },
            { 120, "Device busy. Applications should retry command." },
            { 200, "CoreScanner is already opened." },
            { 201, "CoreScanner is already closed." },
            { 202, "CoreScanner is closed." },
            { 300, "Malformed inXML." },
            { 301, "XML Reader could not be instantiated." },
            { 302, "Input for XML Reader could not be set." },
            { 303, "XML Reader property could not be set." },
            { 304, "XML Writer could not be instantiated." },
            { 305, "Output for XML Writer could not be set." },
            { 306, "XML Writer property could not be set." },
            { 307, "Cannot read element from XML input." },
            { 308, "Arguments in inXML are not valid." },
            { 309, "Write to XML output string failed." },
            { 310, "InXML exceed length." },
            { 311, "Buffer length for type exceeded." },
            { 400, "Null pointer." },
            { 401, "Cannot add a duplicate client." },
            { 500, "Invalid firmware file." },
            { 501, "Firmware Update failed in scanner." },
            { 502, "Failed to read DAT file." },
            { 503, "Firmware Update is in progress." },
            { 504, "Firmware update is already aborted." },
            { 505, "Firmware Update aborted." },
            { 506, "Scanner is disconnected while updating firmware." },
            { 600, "The software component is already resident in the scanner." }
        };

        /// <summary>
        /// Handles the status code and retrieves its description.
        /// </summary>
        /// <param name="status">The status code returned by the scanner.</param>
        /// <returns>A formatted string describing the status code.</returns>
        public static string HandleStatus(int status)
        {
            if (StatusMessages.TryGetValue(status, out string message))
            {
                string fullMessage = $"Status {status}: {message}";
                return fullMessage;
            }
            else
            {
                string unknownMessage = $"Unknown status code: {status}";
                return unknownMessage;
            }
        }
    }
}

