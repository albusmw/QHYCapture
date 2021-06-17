Option Explicit On
Option Strict On
Imports System.Runtime.InteropServices

Public Class M
    '''<summary>DB that holds all relevant information.</summary>
    Public Shared WithEvents DB As New cDB
    '''<summary>DB that holds meta information.</summary>
    Public Shared WithEvents Meta As New cDB_meta
    '''<summary>DB that holds report information.</summary>
    Public Shared WithEvents Report As New cAstroStatDisp
End Class

'''<summary>Characteristics data of one single capture.</summary>
Public Class cSingleCaptureInfo
    '''<summary>Running capture index.</summary>
    Public CaptureIdx As UInt32 = 0
    '''<summary>Temperature [°C] at start of exposure.</summary>
    Public ObsStartTemp As Double = Double.NaN
    '''<summary>Selected filter.</summary>
    Public FilterActive As eFilter = eFilter.Invalid
    '''<summary>Telescope focus position.</summary>
    Public TelescopeFocus As Double = Double.NaN
    '''<summary>Time at observation start.</summary>
    Public ObsStart As DateTime = Now
    '''<summary>Time at observation end.</summary>
    Public ObsEnd As DateTime = DateTime.MinValue
    '''<summary>Selected QHY read-out mode.</summary>
    Public CamReadOutMode As New Text.StringBuilder
    '''<summary>Exposure time [s].</summary>
    Public ExpTime As Double = Double.NaN
    '''<summary>Selected gain value.</summary>
    Public Gain As Double = Double.NaN
    '''<summary>Selected offset value.</summary>
    Public Offset As Double = Double.NaN
    '''<summary>Selected brightness value.</summary>
    Public Brightness As Double = Double.NaN
    '''<summary>Number of pixel in X direction (bigger axis).</summary>
    Public NAXIS1 As Integer = 0
    '''<summary>Number of pixel in Y direction (bigger axis).</summary>
    Public NAXIS2 As Integer = 0
End Class

'''<summary>Size description.</summary>
Public Structure sSize_UInt
    Dim Width As UInteger
    Dim Height As UInteger
End Structure

'''<summary>Size description.</summary>
Public Structure sSize_Dbl
    Dim Width As Double
    Dim Height As Double
End Structure

'''<summary>Rectangle description.</summary>
Public Structure sRect_UInt
    Dim X As UInteger
    Dim Y As UInteger
    Dim Width As UInteger
    Dim Height As UInteger
End Structure

'''<summary>Read-out resolution.</summary>
Public Enum eReadResolution
    <ComponentModel.Description("8-bit")>
    Res8Bit = 0
    <ComponentModel.Description("16-bit")>
    Res16Bit = 16
End Enum

'''<summary>Mode for X axis scaling mode.</summary>
Public Enum eXAxisScalingMode
    <ComponentModel.Description("Automatic")>
    Auto
    <ComponentModel.Description("Full 16 bit range")>
    FullRange16Bit
    <ComponentModel.Description("Leave as is")>
    LeaveAsIs
End Enum

'''<summary>Filter as to be send as ASCII string.</summary>
Public Enum eFilter As Byte
    <ComponentModel.Description("Unchanged")>
    Invalid = 0
    <ComponentModel.Description("Light")>
    L = 1
    <ComponentModel.Description("Red")>
    R = 2
    <ComponentModel.Description("Green")>
    G = 3
    <ComponentModel.Description("Blue")>
    B = 4
    <ComponentModel.Description("H alpha")>
    H_alpha = 5
    <ComponentModel.Description("S-II")>
    S_II = 6
    <ComponentModel.Description("O-III")>
    O_III = 7
    <ComponentModel.Description("Empty")>
    Empty = 9
End Enum

'''<summary>Available stream modes.</summary>
Public Enum eStreamMode As UInteger
    <ComponentModel.Description("Single frame")>
    SingleFrame = 0
    <ComponentModel.Description("Live")>
    LiveFrame = 1
    <ComponentModel.Description("--INVALID--")>
    Invalid = UInteger.MaxValue
End Enum

'''<summary>Available readout modes.</summary>
Public Enum eReadOutMode As UInteger
    <ComponentModel.Description("Photographic")>
    Photographic = 0
    <ComponentModel.Description("High Gain")>
    HighGain = 1
    <ComponentModel.Description("Extend Fullwell")>
    ExtendFullwell = 2
    <ComponentModel.Description("--INVALID--")>
    Invalid = UInteger.MaxValue
End Enum

'''<summary>Available exposure types modes.</summary>
Public Enum eExposureType As UInteger
    <ComponentModel.Description("Undefined")>
    Undefined = 0
    <ComponentModel.Description("Light")>
    Light = 1
    <ComponentModel.Description("Dark")>
    Dark = 2
    <ComponentModel.Description("Flat")>
    Flat = 3
    <ComponentModel.Description("Bias")>
    Bias = 4
    <ComponentModel.Description("DarkFlat")>
    DarkFlat = 5
    <ComponentModel.Description("Test")>
    Test = 99
    <ComponentModel.Description("Custom)")>
    Custom = 100
    <ComponentModel.Description("Invalid")>
    Invalid = UInteger.MaxValue
End Enum

'''<summary>Database holding relevant information.</summary>
Public Class cDB

    '''<summary>Handle to the camera.</summary>
    Public CamHandle As IntPtr = IntPtr.Zero
    '''<summary>Currently used camera ID.</summary>
    Public UsedCameraId As System.Text.StringBuilder
    '''<summary>Currently used read-out mode.</summary>
    Public UsedReadMode As eReadOutMode = eReadOutMode.Invalid
    '''<summary>Currently used stream mode.</summary>
    Public UsedStreamMode As eStreamMode = eStreamMode.Invalid
    '''<summary>Used to call BeginQHYLiveMode only once.</summary>
    Public LiveModeInitiated As Boolean = False
    '''<summary>Used to call BeginQHYLiveMode only once.</summary>
    Public LastStoredFile As String = String.Empty

    Public Stopper As New cStopper

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property EXEPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property MyINI As String = System.IO.Path.Combine(New String() {EXEPath, "Config.INI"})

    <ComponentModel.Browsable(False)>
    Public Log_Generic As New Text.StringBuilder

    '''<summary>INI access object.</summary>
    Public INI As New Ato.cINI_IO

    '''<summary>Flag that indicates the sequence is running.</summary>
    <ComponentModel.Browsable(False)>
    Public Property RunningFlag As Boolean = False

    '''<summary>Flag that indicates to halt the capture sequence.</summary>
    <ComponentModel.Browsable(False)>
    Public Property StopFlag As Boolean = False

    'WCF
    Public SetupWCF As ServiceModel.Web.WebServiceHost
    Public serviceBehavior As ServiceModel.Description.ServiceDebugBehavior

    '''<summary>Intel IPP access.</summary>
    Public IPP As cIntelIPP

    Const Cat1 As String = "1. Imaging hardware"
    Const Cat2_Exposure As String = "2. Exposure"
    Const Cat3 As String = "3. Image storage"
    Const Cat4 As String = "4. Statistics calculation"
    Const Cat5 As String = "5. Debug and logging"
    Const Cat6 As String = "6. Exposure - Advanced"
    Const Cat7 As String = "7. Focus star search"
    Const Indent As String = "  "
    Const NotSet As String = "-----"

    '''<summary>Camera to search for.</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "1. Camera to search")>
    <ComponentModel.Description("Search string for the camera - string must occure in the CameraID. Use * to use first found camera.")>
    <ComponentModel.DefaultValue("*")>
    Public Property CamToUse As String = "600M"

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "2. Read-out mode")>
    <ComponentModel.Description("Photographic or high-gain.")>
    <ComponentModel.DefaultValue(GetType(eReadOutMode), "Photographic")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.EnumDesciptionConverter))>
    Public Property ReadOutModeEnum As eReadOutMode = eReadOutMode.Photographic

    <ComponentModel.Browsable(False)>
    Public ReadOnly Property ReadOutModeString As String = [Enum].GetName(GetType(eReadOutMode), ReadOutModeEnum)

    '''<summary>Single frame or live mode.</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "3. Stream mode")>
    <ComponentModel.Description("Single frame or live mode.")>
    <ComponentModel.DefaultValue(GetType(eStreamMode), "SingleFrame")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.EnumDesciptionConverter))>
    Public Property StreamMode As eStreamMode = eStreamMode.SingleFrame

    '''<summary>Target temperature to cool to; enter <-100 for do-not-use</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "4.1. Target Temp")>
    <ComponentModel.Description("Target temperature to cool to; enter <= -100 for do-not-use")>
    <ComponentModel.DefaultValue(-10.0)>
    Public Property Temp_Target As Double = -10.0

    '''<summary>Tolerable temperature error [°C]</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "4.2. Target Temp - tolerance")>
    <ComponentModel.Description("Tolerable temperature error [°C]")>
    <ComponentModel.DefaultValue(0.2)>
    Public Property Temp_Tolerance As Double = 0.2

    '''<summary>Time [s] to be within tolerance to mark cooling as done.</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "4.3. Target Temp - stable time")>
    <ComponentModel.Description("Time [s] to be within tolerance to mark cooling as done")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.DoublePropertyConverter_s))>
    <ComponentModel.DefaultValue(60.0)>
    Public Property Temp_StableTime As Double = 60.0

    '''<summary>Time [s] after which the cooling is finished even if the target temperature is NOT reached.</summary>
    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "4.4. Cooling time-out")>
    <ComponentModel.Description("Time [s] after which the cooling is finished even if the target temperature is NOT reached")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.DoublePropertyConverter_s))>
    <ComponentModel.DefaultValue(600.0)>
    Public Property Temp_TimeOutAndOK As Double = 600.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "5.1. Binning - Hardware")>
    <ComponentModel.Description("Hardware Binning (NxN)")>
    <ComponentModel.DefaultValue(1)>
    Public Property HardwareBinning As Integer = 1

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "5.2. Binning - Software")>
    <ComponentModel.Description("Software Binning (NxN)")>
    <ComponentModel.DefaultValue(1)>
    Public Property SoftwareBinning As Integer = 1

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "6. Read resolution")>
    <ComponentModel.Description("Bit per pixel resolution")>
    <ComponentModel.DefaultValue(eReadResolution.Res16Bit)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.EnumDesciptionConverter))>
    Public Property ReadResolution As eReadResolution = eReadResolution.Res16Bit

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "7. ROI")>
    <ComponentModel.Description("ROI (without binning)")>
    <ComponentModel.DefaultValue(GetType(Drawing.Rectangle), "0, 0, 0, 0")>
    Public Property ROI As New Drawing.Rectangle(0, 0, 0, 0)

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "8. USB traffic")>
    <ComponentModel.Description("USB traffic - 0 is fastest, maximum value depends on the camera")>
    <ComponentModel.DefaultValue(20.0)>
    Public Property USBTraffic As Double = 20.0

    <ComponentModel.Category(Cat1)>
    <ComponentModel.DisplayName(Indent & "9. DDR RAM")>
    <ComponentModel.Description("Use DDR RAM?")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property DDR_RAM As Boolean = True

    '===================================================================================================

    '''<summary>Number of exposured to take with identical settings.</summary>
    <ComponentModel.Category(Cat2_Exposure)>
    <ComponentModel.DisplayName(Indent & "1.1. # of captures")>
    <ComponentModel.Description("Number of exposured to take with identical settings.")>
    <ComponentModel.DefaultValue(1)>
    Public Property CaptureCount As Integer = 1

    '''<summary>Light, Bias, Dark, Flat, Tricolor or TestOnly.</summary>
    <ComponentModel.Category(Cat2_Exposure)>
    <ComponentModel.DisplayName(Indent & "1.2. Exposure type")>
    <ComponentModel.Description("Light, Bias, Dark, Flat, Tricolor or TestOnly.")>
    <ComponentModel.DefaultValue(GetType(eExposureType), "Undefined")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.EnumDesciptionConverter))>
    Public Property ExposureTypeEnum As eExposureType = eExposureType.Undefined

    '''<summary>Custom exposure type if ExposureTypeEnum is set to Custom.</summary>
    <ComponentModel.Category(Cat2_Exposure)>
    <ComponentModel.DisplayName(Indent & "1.3. Custom exposure type")>
    <ComponentModel.Description("Custom exposure type if ExposureTypeEnum is set to Custom.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property ExposureTypeCustom As String = NotSet

    '''<summary>String representation of the exposure type enum.</summary>
    <ComponentModel.Browsable(False)>
    Public ReadOnly Property ExposureTypeString As String
        Get
            If ExposureTypeEnum = eExposureType.Custom Then Return ExposureTypeCustom Else Return [Enum].GetName(GetType(eExposureType), ExposureTypeEnum)
        End Get
    End Property

    <ComponentModel.Category(Cat2_Exposure)>
    <ComponentModel.DisplayName(Indent & "2.1. Filter wheel")>
    <ComponentModel.Description("Configure if a real filter wheel should be controlled or if filter is just used in meta data, ....")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property UseFilterWheel As Boolean = True

    '''<summaryFilter slot to select</summary>
    <ComponentModel.Category(Cat2_Exposure)>
    <ComponentModel.DisplayName(Indent & "2.2. Filter slot")>
    <ComponentModel.Description("Filter slot to select")>
    <ComponentModel.DefaultValue(eFilter.Invalid)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.EnumDesciptionConverter))>
    Public Property FilterSlot As eFilter = eFilter.Invalid

    '''<summary>Exposure time [s].</summary>
    <ComponentModel.Category(Cat2_Exposure)>
    <ComponentModel.DisplayName(Indent & "3. Exposure time [s]")>
    <ComponentModel.Description("Exposure time [s] for each exposure")>
    <ComponentModel.DefaultValue(1.0)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.DoublePropertyConverter_s))>
    Public Property ExposureTime As Double = 1.0

    '''<summaryGain to set</summary>
    <ComponentModel.Category(Cat2_Exposure)>
    <ComponentModel.DisplayName(Indent & "4. Gain")>
    <ComponentModel.Description("Gain to set")>
    <ComponentModel.DefaultValue(26.0)>
    Public Property Gain As Double = 26.0

    '''<summary>Offset to set</summary>
    <ComponentModel.Category(Cat2_Exposure)>
    <ComponentModel.DisplayName(Indent & "5. Offset")>
    <ComponentModel.Description("Offset to set")>
    <ComponentModel.DefaultValue(50.0)>
    Public Property Offset As Double = 50.0

    '''<summary>Remove the overscan area in the stored data and file</summary>
    <ComponentModel.Category(Cat2_Exposure)>
    <ComponentModel.DisplayName(Indent & "6. Remove overscan")>
    <ComponentModel.Description("Remove the overscan area in the stored data and file")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property RemoveOverscan As Boolean = True

    '''<summary>Write all exposure data to the camera on each exposure start (takes some ms ...)</summary>
    <ComponentModel.Category(Cat2_Exposure)>
    <ComponentModel.DisplayName(Indent & "7. Config for each capture")>
    <ComponentModel.Description("Write all exposure data to the camera on each exposure start (takes some ms ...)")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property ConfigAlways As Boolean = True

    <ComponentModel.Category(Cat2_Exposure)>
    <ComponentModel.DisplayName(Indent & "8. Close cam on each xml exp")>
    <ComponentModel.Description("Close and re-open the camera after each exposure XML entry")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property CloseCam As Boolean = False

    '===================================================================================================

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "1. Store captured image")>
    <ComponentModel.Description("Store the captured image on harddisc")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property StoreImage As Boolean = True

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "2. Storage path")>
    <ComponentModel.Description("Root path to store images")>
    <ComponentModel.DefaultValue("C:\DATA_IO\QHYCapture")>
    Public Property StoragePath As String = "C:\DATA_IO\QHYCapture"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "3. File name start")>
    <ComponentModel.Description("File name start to use")>
    <ComponentModel.DefaultValue("QHY600_$FILT$_$EXP$_$GAIN$_$OFFS$_$IDX$_$CNT$_$RMODE$")>
    Public Property FileName As String = "QHY600_$FILT$_$EXP$_$GAIN$_$OFFS$_$IDX$_$CNT$_$RMODE$"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "4. FITS extenstion")>
    <ComponentModel.Description("Extension to use for FITS files")>
    <ComponentModel.DefaultValue("fits")>
    Public Property FITSExtension As String = "fits"

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "5. Open image automatically?")>
    <ComponentModel.Description("Automaticall open a stored FITS file with the default editor")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property AutoOpenImage As Boolean = False

    <ComponentModel.Category(Cat3)>
    <ComponentModel.DisplayName(Indent & "6. Show live image")>
    <ComponentModel.Description("Show a live image?")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property ShowLiveImage As Boolean = False

    '===================================================================================================

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName(Indent & "1. Calculate mono statistics")>
    <ComponentModel.Description("Clear statistics log on every measurement")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property StatMono As Boolean = True

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName(Indent & "2. Calculate color statistics")>
    <ComponentModel.Description("Clear statistics log on every measurement")>
    <ComponentModel.DefaultValue(True)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property StatColor As Boolean = True

    <ComponentModel.Category(Cat4)>
    <ComponentModel.DisplayName(Indent & "3. Infinit stack mode")>
    <ComponentModel.Description("Stack all images (use for focus or drift analysis)")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property StackAll As Boolean = False

    '===================================================================================================

    '''<summary>Property indicating if the ROI is set.</summary>
    <ComponentModel.Browsable(False)>
    Public ReadOnly Property ROISet() As Boolean
        Get
            If ROI.X = 0 And ROI.Y = 0 Then
                If ROI.Width = 0 And ROI.Height = 0 Then Return False                                                       'auto-ROI
                If ROI.Width = M.Meta.Chip_Pixel.Width And ROI.Height = M.Meta.Chip_Pixel.Height Then Return False          'ROI set from the chip dimensions
            End If
            Return True
        End Get
    End Property

End Class

'''<summary>Database holding meta data information.</summary>
Public Class cDB_meta

    Const Cat1_SiteAndMount As String = "1. Site and mount"
    Const Cat2_ObjectAndInstrument As String = "2. Object and instrument"
    Const Cat3_Logging As String = "3. Software logging"
    Const Cat4_CamProperties As String = "4. Camera properties"
    Const Cat5_CamAdvanced As String = "5. Advanced camera settings"
    Const Cat6_Focus As String = "6. Focus support"
    Const Cat7_MiscSettings As String = "7. Misc settings"
    Const Indent As String = "  "
    Const NotSet As String = "-----"

    '''<summary>Automatically oad mount data via LAN?.</summary>
    <ComponentModel.Category(Cat1_SiteAndMount)>
    <ComponentModel.DisplayName(Indent & "1.1. Use 10Micron LAN?")>
    <ComponentModel.Description("Automatically oad mount data via LAN?")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    <ComponentModel.DefaultValue(True)>
    Public Property Load10MicronDataAlways As Boolean = True

    '''<summary>IP of the 10Micron mount</summary>
    <ComponentModel.Category(Cat1_SiteAndMount)>
    <ComponentModel.DisplayName(Indent & "1.2. 10Micron LAN IP")>
    <ComponentModel.Description("IP of the 10Micron mount.")>
    Public Property IP_10Micron As String = "192.168.10.119"

    '''<summary>LAN port of the 10Micron mount</summary>
    <ComponentModel.Category(Cat1_SiteAndMount)>
    <ComponentModel.DisplayName(Indent & "1.3. 10Micron LAN Port")>
    <ComponentModel.Description("LAN port of the 10Micron mount.")>
    <ComponentModel.DefaultValue(3490)>
    Public Property IP_10Micron_Port As Integer = 3490

    '''<summary>Time-out time for connecting the mount.</summary>
    <ComponentModel.Category(Cat1_SiteAndMount)>
    <ComponentModel.DisplayName(Indent & "1.4. 10Micron LAN Time-out")>
    <ComponentModel.Description("Time-out time for connecting the mount.")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.DoublePropertyConverter_s))>
    <ComponentModel.DefaultValue(2)>
    Public Property IP_10Micron_TimeOut As Integer = 2

    '''<summary>Longitude of the site.</summary>
    <ComponentModel.Category(Cat1_SiteAndMount)>
    <ComponentModel.DisplayName(Indent & "2.1. Site - Longitude")>
    <ComponentModel.Description("Longitude of the site.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property SiteLongitude As String = NotSet

    '''<summary>Latitude of the site.</summary>
    <ComponentModel.Category(Cat1_SiteAndMount)>
    <ComponentModel.DisplayName(Indent & "2.2. Site - Latitude")>
    <ComponentModel.Description("Latitude of the site.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property SiteLatitude As String = NotSet

    '''<summary>Height of the site.</summary>
    <ComponentModel.Category(Cat1_SiteAndMount)>
    <ComponentModel.DisplayName(Indent & "2.3. Site - Height")>
    <ComponentModel.Description("Height of the site.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property SiteHeight As String = NotSet

    '''<summary>Configured mount right ascension.</summary>
    <ComponentModel.Category(Cat1_SiteAndMount)>
    <ComponentModel.DisplayName(Indent & "3.1. Mount - RA")>
    <ComponentModel.Description("Configured mount right ascension.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeRightAscension As String = NotSet

    '''<summary>Configured mount declination.</summary>
    <ComponentModel.Category(Cat1_SiteAndMount)>
    <ComponentModel.DisplayName(Indent & "3.2. Mount - DEC")>
    <ComponentModel.Description("Configured mount declination.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeDeclination As String = NotSet

    '''<summary>Current mount altitude.</summary>
    <ComponentModel.Category(Cat1_SiteAndMount)>
    <ComponentModel.DisplayName(Indent & "4.1. Mount - Altitude")>
    <ComponentModel.Description("Current mount altitude.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeAltitude As String = NotSet

    '''<summary>Current mount azimuth.</summary>
    <ComponentModel.Category(Cat1_SiteAndMount)>
    <ComponentModel.DisplayName(Indent & "4.2. Mount - Azimuth")>
    <ComponentModel.Description("Current mount azimuth.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property TelescopeAzimuth As String = NotSet

    '================================================================================

    '''<summary>Name of the object (NGC1234, ...).</summary>
    <ComponentModel.Category(Cat2_ObjectAndInstrument)>
    <ComponentModel.DisplayName(Indent & "1. Objectame")>
    <ComponentModel.Description("Name of the object (NGC1234, ...).")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property ObjectName As String = NotSet

    '''<summary>Sequence GUID.</summary>
    <ComponentModel.Category(Cat2_ObjectAndInstrument)>
    <ComponentModel.DisplayName(Indent & "2. GUID of the sequence")>
    <ComponentModel.Description("Sequence GUID.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public Property GUID As String = NotSet

    <ComponentModel.Category(Cat2_ObjectAndInstrument)>
    <ComponentModel.DisplayName(Indent & "3. Author")>
    <ComponentModel.Description("Author to add to the meta data.")>
    <ComponentModel.DefaultValue("Martin Weiss")>
    Public Property Author As String = "Martin Weiss"

    <ComponentModel.Category(Cat2_ObjectAndInstrument)>
    <ComponentModel.DisplayName(Indent & "4. Origin")>
    <ComponentModel.Description("Origin to add to the meta data.")>
    <ComponentModel.DefaultValue("Sternwarte Holzkirchen")>
    Public Property Origin As String = "Sternwarte Holzkirchen"

    <ComponentModel.Category(Cat2_ObjectAndInstrument)>
    <ComponentModel.DisplayName(Indent & "5. Telescope used")>
    <ComponentModel.Description("Telescope name to add to the meta data.")>
    <ComponentModel.DefaultValue("CDK 12.5 with reducer")>
    Public Property Telescope As String = "CDK 12.5 with reducer"

    <ComponentModel.Category(Cat2_ObjectAndInstrument)>
    <ComponentModel.DisplayName(Indent & "5.1. Telescope aperture [mm]")>
    <ComponentModel.Description("Telescope aperture to add to the meta data.")>
    <ComponentModel.DefaultValue(317.0)>
    Public Property TelescopeAperture As Double = 317.0

    '''<summary>Telescope focal length [mm].</summary>
    <ComponentModel.Category(Cat2_ObjectAndInstrument)>
    <ComponentModel.DisplayName(Indent & "5.2. Telescope focal length [mm]")>
    <ComponentModel.Description("Telescope focal length to add to the meta data.")>
    <ComponentModel.DefaultValue(1676.4)>
    Public Property TelescopeFocalLength As Double = 1676.4

    '''<summary>Telescope focal length [mm].</summary>
    <ComponentModel.Category(Cat2_ObjectAndInstrument)>
    <ComponentModel.DisplayName(Indent & "5.3. Telescope focus position")>
    <ComponentModel.Description("Focuser position reported by the telescope.")>
    <ComponentModel.DefaultValue(Double.NaN)>
    Public Property TelescopeFocus As Double = Double.NaN

    '===================================================================================================

    '''<summary>Log all supported camera properties with name and range in the begin - useful e.g. to see the value range for certain settings.</summary>
    <ComponentModel.Category(Cat3_Logging)>
    <ComponentModel.DisplayName(Indent & "1. Log camera properties")>
    <ComponentModel.Description("Log all supported camera properties with name and range in the begin - useful e.g. to see the value range for certain settings.")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    <ComponentModel.DefaultValue(False)>
    Public Property Log_CamProp As Boolean = False

    '''<summary>Display a detailed timing log in the end.</summary>
    <ComponentModel.Category(Cat3_Logging)>
    <ComponentModel.DisplayName(Indent & "2. Log timing")>
    <ComponentModel.Description("Display a detailed timing log in the end.")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    <ComponentModel.DefaultValue(False)>
    Public Property Log_Timing As Boolean = False

    '''<summary>Log special settings.</summary>
    <ComponentModel.Category(Cat3_Logging)>
    <ComponentModel.DisplayName(Indent & "3. Log verbose")>
    <ComponentModel.Description("Log special settings.")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    <ComponentModel.DefaultValue(False)>
    Public Property Log_Verbose As Boolean = False

    '''<summary>This folder contains the IPP (Intel Performance Primitives) that are used to speed-up all calculation processes.</summary>
    <ComponentModel.Category(Cat3_Logging)>
    <ComponentModel.DisplayName(Indent & "4. Intel IPP path")>
    <ComponentModel.Description("This folder contains the IPP (Intel Performance Primitives) that are used to speed-up all calculation processes.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public ReadOnly Property Log_IntelIPPPath As String
        Get
            If IsNothing(M.DB.IPP) = True Then
                Return "--- (nothing)"
            Else
                Return M.DB.IPP.IPPPath
            End If
        End Get
    End Property

    '''<summary>This file contains all DLL calls.</summary>
    <ComponentModel.Category(Cat3_Logging)>
    <ComponentModel.DisplayName(Indent & "5. QHY DLL log path")>
    <ComponentModel.Description("This file contains all DLL calls.")>
    <ComponentModel.DefaultValue(NotSet)>
    Public ReadOnly Property QHYLogFile As String
        Get
            Return System.IO.Path.Combine(M.DB.EXEPath, "QHYLog_" & GUID & ".log")
        End Get
    End Property

    '===================================================================================================

    '''<summary>Chip physical size [mm].</summary>
    <ComponentModel.Category(Cat4_CamProperties)>
    <ComponentModel.DisplayName(Indent & "1. Chip physical size [mm]")>
    <ComponentModel.Description("Chip physical size [mm].")>
    Public ReadOnly Property Chip_Physical As sSize_Dbl
        Get
            Return MyChip_Physical
        End Get
    End Property
    Public MyChip_Physical As New sSize_Dbl

    '''<summary>Chip size [pixel].</summary>
    <ComponentModel.Category(Cat4_CamProperties)>
    <ComponentModel.DisplayName(Indent & "2. Chip size [pixel]")>
    <ComponentModel.Description("Chip size [pixel].")>
    Public ReadOnly Property Chip_Pixel As sSize_UInt
        Get
            Return MyChip_Pixel
        End Get
    End Property
    Public MyChip_Pixel As New sSize_UInt

    '''<summary>Chip pixel size [um].</summary>
    <ComponentModel.Category(Cat4_CamProperties)>
    <ComponentModel.DisplayName(Indent & "3. Chip pixel size [um]")>
    <ComponentModel.Description("Chip pixel size [um].")>
    Public ReadOnly Property Pixel_Size As sSize_Dbl
        Get
            Return MyPixel_Size
        End Get
    End Property
    Public MyPixel_Size As New sSize_Dbl

    '''<summary>SDK version.</summary>
    <ComponentModel.Category(Cat4_CamProperties)>
    <ComponentModel.DisplayName(Indent & "4. SDK version")>
    <ComponentModel.Description("SDK version.")>
    Public ReadOnly Property SDKVersionString As String
        Get
            Dim RetVal As New List(Of String)
            For Each Entry As UInteger In SDKVersion
                RetVal.Add(Entry.ValRegIndep)
            Next Entry
            Return Join(RetVal.ToArray, ".")
        End Get
    End Property
    Public SDKVersion(3) As UInteger

    '''<summary>Firmware version.</summary>
    <ComponentModel.Category(Cat4_CamProperties)>
    <ComponentModel.DisplayName(Indent & "5. Firmware version")>
    <ComponentModel.Description("Firmware version.")>
    Public ReadOnly Property FWVersionString As String
        Get
            Dim Year As Integer = (FWVersion(0) \ 16) + &H10
            Dim Month As Integer = FWVersion(0) - ((FWVersion(0) \ 16) * 16)
            Dim Day As Integer = FWVersion(1)
            Return Format(Year, "00") & "-" & Format(Month, "00") & "-" & Format(Day, "00")
        End Get
    End Property
    Public FWVersion As Byte() = {0, 0, 0, 0}

    '===================================================================================================

    <ComponentModel.Category(Cat5_CamAdvanced)>
    <ComponentModel.DisplayName(Indent & "1. Brightness")>
    <ComponentModel.Description("Brightness control value of the camera - leave at 0.0 for optimum file output results")>
    <ComponentModel.DefaultValue(0.0)>
    Public Property Brightness As Double = 0.0

    <ComponentModel.Category(Cat5_CamAdvanced)>
    <ComponentModel.DisplayName(Indent & "2. Contrast")>
    <ComponentModel.Description("Contrast control value of the camera - leave at 0.0 for optimum file output results")>
    <ComponentModel.DefaultValue(0.0)>
    Public Property Contrast As Double = 0.0

    <ComponentModel.Category(Cat5_CamAdvanced)>
    <ComponentModel.DisplayName(Indent & "3. Gamma")>
    <ComponentModel.Description("Gamma control value of the camera - leave at 1.0 for optimum file output results")>
    <ComponentModel.DefaultValue(1.0)>
    Public Property Gamma As Double = 1.0

    <ComponentModel.Category(Cat5_CamAdvanced)>
    <ComponentModel.DisplayName(Indent & "4.1. White balance - Red")>
    <ComponentModel.Description("White balance RED control value of the camera - leave at 128.0 for optimum file output results")>
    <ComponentModel.DefaultValue(128.0)>
    Public Property WhiteBalance_Red As Double = 128.0

    <ComponentModel.Category(Cat5_CamAdvanced)>
    <ComponentModel.DisplayName(Indent & "4.2. White balance - Green")>
    <ComponentModel.Description("White balance GREEN control value of the camera - leave at 128.0 for optimum file output results")>
    <ComponentModel.DefaultValue(128.0)>
    Public Property WhiteBalance_Green As Double = 128.0

    <ComponentModel.Category(Cat5_CamAdvanced)>
    <ComponentModel.DisplayName(Indent & "4.3. White balance - Blue")>
    <ComponentModel.Description("White balance BLUE control value of the camera - leave at 128.0 for optimum file output results")>
    <ComponentModel.DefaultValue(128.0)>
    Public Property WhiteBalance_Blue As Double = 128.0

    '===================================================================================================

    <ComponentModel.Category(Cat6_Focus)>
    <ComponentModel.DisplayName(Indent & "1. Auto-star search for auto-focus")>
    <ComponentModel.Description("Auto-select ROI and center on found maximum")>
    <ComponentModel.DefaultValue(False)>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    Public Property StarSearch As Boolean = False

    <ComponentModel.Category(Cat6_Focus)>
    <ComponentModel.DisplayName(Indent & "2. Auto-star search ROI size")>
    <ComponentModel.Description("Size [pixel] of the ROI during start search")>
    <ComponentModel.DefaultValue(5)>
    Public Property StarSearch_Blur As Integer = 5

    <ComponentModel.Category(Cat6_Focus)>
    <ComponentModel.DisplayName(Indent & "3. Auto-star search ROI size")>
    <ComponentModel.Description("Size [pixel] of the ROI during start search")>
    <ComponentModel.DefaultValue(41)>
    Public Property StarSearch_ROI As Integer = 41

    '===================================================================================================

    '''<summary>Time [s] after which the filter wheel movement is stoped and the software goes on even if the filter is NOT in place.</summary>
    <ComponentModel.Category(Cat7_MiscSettings)>
    <ComponentModel.DisplayName(Indent & "1. Filter wheel time-out")>
    <ComponentModel.Description("Time [s] after which the filter wheel movement is stoped and the software goes on even if the filter is NOT in place.")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.DoublePropertyConverter_s))>
    <ComponentModel.DefaultValue(15.0)>
    Public Property FilterWheelTimeOut As Double = 15.0

    '''<summary>Auto-switch color statistics OFF for mono cameras?</summary>
    <ComponentModel.Category(Cat7_MiscSettings)>
    <ComponentModel.DisplayName(Indent & "2. Color stat OFF for mono")>
    <ComponentModel.Description("Auto-switch color statistics OFF for mono cameras?")>
    <ComponentModel.TypeConverter(GetType(ComponentModelEx.BooleanPropertyConverter_YesNo))>
    <ComponentModel.DefaultValue(True)>
    Public ReadOnly Property ColorStatOffForMono As Boolean
        Get
            Return MyColorStatOffForMono
        End Get
    End Property
    Public MyColorStatOffForMono As Boolean = True

End Class
