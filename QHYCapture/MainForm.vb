﻿Option Explicit On
Option Strict On

#Disable Warning CA1416 ' Validate platform compatibility
Partial Public Class MainForm

    Private Const BitsPerByte As Integer = 8

    'Reflect database
    Public ReadOnly DB_config As New cReflectHelp(M.Config)
    'Reflect meta database
    Public ReadOnly DB_meta As New cReflectHelp(M.Meta)
    'Reflect report database
    Public ReadOnly DB_report As New cReflectHelp(M.Report.Config)

    '''<summary>WCF interface.</summary>
    Private WithEvents DB_ServiceContract As cDB_ServiceContract
    '''<summary>Indicate that a property was changed and parameters need to be updated in the camera.</summary>
    Private PropertyChanged As Boolean = False
    '''<summary>RTF report generator.</summary>
    Private RTFGen As New cRTFGen
    '''<summary>Accumulated statistics.</summary>
    Private LoopStat As AstroNET.Statistics.sStatistics
    '''<summary>Monitor for the MIDI events.</summary>
    Private WithEvents MIDI As cMIDIMonitor

    Private WithEvents ZWOASI As New cZWOASI

    Private WithEvents QHYFunction As New cQHYFunction

    Private Const OneCapture As System.UInt32 = CType(1, System.UInt32)
    Private Const FirstCapture As System.UInt32 = CType(1, System.UInt32)

    '''<summary>Class to configure the temperature control of the camera.</summary>
    Private Class cTempConfig
        '''<summary>Time moment since the temperature is within the range.</summary>
        Public InRangeSince As DateTime
        Public Sub New()
            InRangeSince = DateTime.MaxValue
        End Sub
        Public ReadOnly Property InRangeSeconds() As Double
            Get
                If InRangeSince = DateTime.MaxValue Then
                    Return -1
                Else
                    Return (Now - InRangeSince).TotalSeconds
                End If
            End Get
        End Property
        '''<summary>Store a new temperature measurement.</summary>
        '''<param name="CurrentTemp">Measured temperature</param>
        '''<param name="TargetTemp">Requested target temperature</param>
        '''<param name="Tolerance">Tolerance value.</param>
        Public Sub SetTemp(ByVal CurrentTemp As Double, ByVal TargetTemp As Double, ByVal Tolerance As Double)
            If System.Math.Abs(CurrentTemp - TargetTemp) <= Tolerance Then
                If InRangeSince = DateTime.MaxValue Then InRangeSince = DateTime.Now
            Else
                InRangeSince = DateTime.MaxValue
            End If
        End Sub
    End Class
    Private TempConfig As New cTempConfig

    '''<summary>Run a single capture.</summary>
    Private Sub RunCaptureToolStripMenuItem_Click(sender As Object, e As EventArgs)
        QHYCapture(True)
    End Sub

    '''<summary>Command for a QHY capture run.</summary>
    Public Sub QHYCapture(ByVal CloseAtEnd As Boolean)

        Dim EffectiveArea As sRect_UInt
        Dim OverScanArea_invalid As sRect_UInt              'current DLL return 0:0:0:0
        Dim bpp As UInteger = 0

        'Set DLL log path
        QHY.QHY.LogFile = M.Meta.QHYLogFile

        'Wait for start condition
        If M.Meta.StartDateTime >= Now Then
            Do
                Dim TimeToGo As TimeSpan = M.Meta.StartDateTime - Now
                tsslProgress.Text = "Exposure starts @ " & M.Meta.StartDateTime.ValRegIndep & " (" & TimeToGo.ValRegIndep & " to go)"
                DE()
            Loop Until (Now >= M.Meta.StartDateTime) Or (M.DB.StopFlag = True)
        End If
        If (M.DB.StopFlag = True) Then Exit Sub

        'Start
        M.DB.StopFlag = False
        M.DB.RunningFlag = True
        M.DB.Stopper.Start()

        'Try to get a suitable camera and continue if found
        LED_update(tsslLED_init, True)
        If InitQHY(M.Config.CamToUse) = False Then Log("No suitable camera found!")
        LED_update(tsslLED_init, False)
        If M.DB.CamHandle = IntPtr.Zero Then Exit Sub

        LED_update(tsslLED_config, True)

        'Get chip properties & SDK version
        QHY.QHY.GetQHYCCDChipInfo(M.DB.CamHandle, M.Meta.MyChip_Physical.Width, M.Meta.MyChip_Physical.Height, M.Meta.MyChip_Pixel.Width, M.Meta.MyChip_Pixel.Height, M.Meta.MyPixel_Size.Width, M.Meta.MyPixel_Size.Height, bpp)
        QHY.QHY.GetQHYCCDEffectiveArea(M.DB.CamHandle, EffectiveArea.X, EffectiveArea.Y, EffectiveArea.Width, EffectiveArea.Height)
        QHY.QHY.GetQHYCCDOverScanArea(M.DB.CamHandle, OverScanArea_invalid.X, OverScanArea_invalid.Y, OverScanArea_invalid.Width, OverScanArea_invalid.Height)
        QHY.QHY.GetQHYCCDSDKVersion(M.Meta.SDKVersion(0), M.Meta.SDKVersion(1), M.Meta.SDKVersion(2), M.Meta.SDKVersion(3))
        M.DB.Stopper.Stamp("Get chip properties")

        'Get camera firmware version info
        ReDim M.Meta.FWVersion(32)
        Using Pinner As New cIntelIPP.cPinHandler
            Dim BufPtr As IntPtr = Pinner.Pin(M.Meta.FWVersion)
            QHY.QHY.GetQHYCCDFWVersion(M.DB.CamHandle, BufPtr)
        End Using

        'Log chip properties
        If M.Meta.Log_CamProp = True Then
            Log("SDK version: " & M.Meta.SDKVersionString)
            Log("Chip info (bpp: " & bpp.ValRegIndep & ")")
            Log("  Chip  W x H  :" & M.Meta.MyChip_Physical.Width.ValRegIndep & " x " & M.Meta.MyChip_Physical.Height.ValRegIndep & " mm")
            Log("  Image W x H  :" & M.Meta.MyChip_Pixel.Width.ValRegIndep & " x " & M.Meta.MyChip_Pixel.Height.ValRegIndep & " pixel")
            Log("  Pixel W x H  :" & M.Meta.MyPixel_Size.Width.ValRegIndep & " x " & M.Meta.MyPixel_Size.Height.ValRegIndep & " um")
            Log("CCD Effective Area:")
            Log("  Start X : Y  :" & EffectiveArea.X.ValRegIndep & ":" & EffectiveArea.Y.ValRegIndep)
            Log("  Size  W x H  :" & EffectiveArea.Width.ValRegIndep & " x " & EffectiveArea.Height.ValRegIndep)
            Log("CCD Overscan Area:")
            Log("  Start X : Y  :" & OverScanArea_invalid.X.ValRegIndep & ":" & OverScanArea_invalid.Y.ValRegIndep)
            Log("  Size  W x H  :" & OverScanArea_invalid.Width.ValRegIndep & " x " & OverScanArea_invalid.Height.ValRegIndep)
            Log("==============================================================================")
            'Log all control values
            Log("ControlValues:")
            LogControlValues()
            M.DB.Stopper.Stamp("GetQHYCCDParams")
            Log("==============================================================================")
        End If

        RefreshProperties()
        LED_update(tsslLED_config, False)

        'Set properties for color cameras
        If IsColorCamera() = False And M.Meta.ColorStatOffForMono Then
            M.Report.Config.PlotStatisticsColor = False
            M.Config.StatColor = False
        End If
        Dim ChannelToRead As UInteger = 0

        'Prepare buffers
        LoopStat = New AstroNET.Statistics.sStatistics
        Dim PinHandler As cIntelIPP.cPinHandler = Nothing
        Dim CamRawBuffer As Byte() = {}
        Dim CamRawBufferPtr As IntPtr = Nothing
        Dim InfinitBuffer(,) As UInt32 = {}
        M.DB.Stopper.Stamp("Prepare buffers")

        'Select filter
        Dim FilterActive As Integer = -1
        M.Config.FilterWheelHelper = New cFilterWheelHelper(M.Config.FilterOrder)
        If M.Config.Filter <> cFilterWheelHelper.eFilter.Unchanged And M.Config.UseFilterWheel = True Then
            FilterActive = QHYFunction.ActivateFilter(M.DB.CamHandle, M.Config.FilterWheelHelper.FilterSlot(M.Config.Filter), M.Meta.FilterWheelTimeOut, M.Config.FilterWheelHelper.VerboseFilterList)
        End If
        M.DB.Stopper.Stamp("Select filter")

        'Enter capture loop
        Dim EndTimeStamps As New List(Of DateTime)
        Dim TotalCaptureTime As Double = 0
        Dim CaptureInfo_running As New cSingleCaptureInfo
        Dim CaptureInfo_finished As New cSingleCaptureInfo

        M.DB.CurrentExposureIndex = FirstCapture
        Do

            '================================================================================
            ' START EXPOSURE ON FIRST ENTRY
            '================================================================================

            If M.DB.CurrentExposureIndex = FirstCapture Then
                CaptureInfo_running = StartExposure()
            End If

            '================================================================================
            ' WAIT FOR END AND READ BUFFERS
            '================================================================================

            IdleExposureTime(M.Config.ExposureTime)
            If StopNow(False) Then Exit Do

            'Get the buffer size from the DLL - typically too big but does not care ...
            Dim BytesToTransfer_reported As UInteger = QHY.QHY.GetQHYCCDMemLength(M.DB.CamHandle)
            LogVerbose("GetQHYCCDMemLength says: " & BytesToTransfer_reported.ValRegIndep.PadLeft(12) & " byte to transfer.")
            If (CamRawBuffer.Length <> BytesToTransfer_reported) Or (IsNothing(PinHandler) = True) Then
                PinHandler = New cIntelIPP.cPinHandler
                ReDim CamRawBuffer(CInt(BytesToTransfer_reported - 1))
                CamRawBufferPtr = PinHandler.Pin(CamRawBuffer)
            End If
            M.DB.Stopper.Stamp("GetQHYCCDMemLength & pinning")

            'Read image data from camera - ALWAYS WITH OVERSCAN
            Dim Captured_W As UInteger = 0 : Dim Captured_H As UInteger = 0 : Dim BitPerPixel As UInteger = 0
            Dim LiveModePollCount As Integer = 0
            LED_update(tsslLED_reading, True)
            If M.Config.StreamMode = eStreamMode.SingleFrame Then
                CallOK("GetQHYCCDSingleFrame", QHY.QHY.GetQHYCCDSingleFrame(M.DB.CamHandle, Captured_W, Captured_H, BitPerPixel, ChannelToRead, CamRawBufferPtr))
                LED_update(tsslLED_capture, False)
            Else
                Dim LiveModeReady As UInteger = UInteger.MaxValue
                Do
                    LiveModeReady = QHY.QHY.GetQHYCCDLiveFrame(M.DB.CamHandle, Captured_W, Captured_H, BitPerPixel, ChannelToRead, CamRawBufferPtr)
                    LiveModePollCount += 1
                    DE()
                Loop Until (LiveModeReady = QHYCamera.QHY.QHYCCD_ERROR.QHYCCD_SUCCESS) Or M.DB.StopFlag = True
            End If
            LED_update(tsslLED_reading, False)
            CaptureInfo_running.ObsEnd = Now
            EndTimeStamps.Add(CaptureInfo_running.ObsEnd)

            Dim BytesToTransfer_calculated As Long = Captured_W * Captured_H * CInt(BitPerPixel / BitsPerByte)
            LogVerbose("Calculation says       : " & BytesToTransfer_calculated.ValRegIndep.PadLeft(12) & " byte to transfer.")
            LogVerbose("Loaded image with " & Captured_W.ValRegIndep & "x" & Captured_H.ValRegIndep & " pixel @ " & BitPerPixel & " bit resolution, " & ChannelToRead.ValRegIndep & " channels")
            M.DB.Stopper.Stamp("GetQHYCCDSingleFrame (" & LiveModePollCount.ValRegIndep & " x)")

            'Remove overscan - do NOT run if an ROI is set
            Dim SingleStatCalc As New AstroNET.Statistics(M.DB.IPP)
            SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data = ChangeAspectIPP(M.DB.IPP, CamRawBuffer, CInt(Captured_W), CInt(Captured_H))      'only convert flat byte buffer to UInt16 matrix data
            If M.Config.EffectiveAreaOnly = True And M.Config.ROISet = False Then
                'Effective area Y coordinates must be shifted ...
                EffectiveArea.Y = 0
                Dim ReturnCode As cIntelIPP.IppStatus = GetEffectiveArea(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, EffectiveArea)
                If ReturnCode <> cIntelIPP.IppStatus.NoErr Then LogError("Overscan removal FAILED")
            End If
            CaptureInfo_running.NAXIS1 = SingleStatCalc.DataProcessor_UInt16.ImageData(0).NAXIS1
            CaptureInfo_running.NAXIS2 = SingleStatCalc.DataProcessor_UInt16.ImageData(0).NAXIS2
            M.DB.Stopper.Stamp("ChangeAspect")

            'Software binning - if > 1 data type is moved from UInt16 to UInt32
            If M.Config.SoftwareBinning > 1 Then
                SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data = ImageProcessing.BinSum(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, M.Config.SoftwareBinning)
                SingleStatCalc.Reset_UInt16()
            End If

            'Infinit stack (for focus quality estimation)
            If M.Config.StackAll = True Then
                Dim AsUInt32(,) As UInt32 = {}
                M.DB.IPP.Convert(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, AsUInt32)
                M.DB.IPP.Add(AsUInt32, InfinitBuffer)
                M.DB.IPP.Copy(InfinitBuffer, SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data)
                SingleStatCalc.Reset_UInt16()
            End If

            '================================================================================
            'RETRIGGER CAPTURE (done already here to capture while the data are processed)
            '================================================================================

            CaptureInfo_finished = CaptureInfo_running
            If (M.DB.CurrentExposureIndex <= M.Config.CaptureCount) And (M.DB.StopFlag = False) Then
                M.DB.CurrentExposureIndex += OneCapture
                CaptureInfo_running = StartExposure()
            End If

            '================================================================================
            'STATISTICS AND PLOTTING
            '================================================================================

            'Calculate statistics
            Dim SingleStat As New AstroNET.Statistics.sStatistics
            If (M.Config.StatMono = True) Or (M.Config.StatColor = True) Then SingleStat = SingleStatCalc.ImageStatistics()
            SingleStat.MonoStatistics_Int.Width = CaptureInfo_finished.NAXIS1 : SingleStat.MonoStatistics_Int.Height = CaptureInfo_finished.NAXIS2

            LoopStat = AstroNET.Statistics.CombineStatistics(SingleStat.DataMode, SingleStat, LoopStat)
            M.DB.Stopper.Stamp("Statistics - calc")

            'Display statistics
            Dim DisplaySumStat As Boolean = False
            If M.Report.Config.Log_ClearStat = True Then RTFGen.Clear()
            If M.Config.CaptureCount > 1 And M.Report.Config.Log_ClearStat = True Then DisplaySumStat = True
            Dim SingleStatReport As List(Of String) = SingleStat.StatisticsReport(M.Config.StatMono, M.Config.StatColor, M.Report.Config.BayerPatternNames)
            Dim LoopStatReport As List(Of String) = LoopStat.StatisticsReport(M.Config.StatMono, M.Config.StatColor, M.Report.Config.BayerPatternNames)
            If IsNothing(SingleStatReport) = False Then
                RTFGen.AddEntry("Capture #" & CaptureInfo_finished.CaptureIdx.ValRegIndep & " statistics:", Drawing.Color.Black)
                For Idx As Integer = 0 To SingleStatReport.Count - 1
                    Dim Line As String = SingleStatReport(Idx)
                    If DisplaySumStat = True Then Line &= "|" & LoopStatReport(Idx).Substring(SingleStat.MonoStatistics_Int.ReportHeaderLength + 1)
                    RTFGen.AddEntry(Line, Drawing.Color.Black)
                Next Idx
                RTFGen.ForceRefresh()
                DE()
            End If
            M.DB.Stopper.Stamp("Statistics - text")

            '═════════════════════════════════════════════════════════════════════════════
            'Plot histogram
            '═════════════════════════════════════════════════════════════════════════════

            'Set caption
            Dim PlotTitle As New List(Of String)
            PlotTitle.Add(M.DB.UsedCameraId.ToString)
            PlotTitle.Add(CaptureInfo_finished.ExpTime.ToString.Trim & " s")
            PlotTitle.Add("Gain " & CaptureInfo_finished.Gain.ToString.Trim)
            PlotTitle.Add("Filter " & [Enum].GetName(GetType(cFilterWheelHelper.eFilter), CaptureInfo_finished.FilterActive))
            PlotTitle.Add("Temperature " & CaptureInfo_finished.ObsStartTemp.ToString.Trim & " °C")
            M.Report.Plotter.SetCaptions(Join(PlotTitle.ToArray, ", "), "ADU value", "# of pixel")
            M.Report.Plot(M.Config.CaptureCount, SingleStat, LoopStat)

            M.DB.Stopper.Stamp("Statistics - plot")

            '═════════════════════════════════════════════════════════════════════════════
            'Store image
            '═════════════════════════════════════════════════════════════════════════════

            If M.Config.StoreImage = True Then

                Dim Path As String = System.IO.Path.Combine(M.Config.StoragePath, M.Meta.GUID)
                If System.IO.Directory.Exists(Path) = False Then System.IO.Directory.CreateDirectory(Path)

                'Compose all FITS keyword entries and replace placeholders in filename ($EXP$, ...)
                Dim FileNameToWrite As String = M.Config.FileName
                Dim CustomElement As Dictionary(Of eFITSKeywords, Object) = GenerateFITSHeader(CaptureInfo_finished, FileNameToWrite)

                'Generate final filename
                M.DB.LastStoredFile = MakeUnique(System.IO.Path.Combine(Path, FileNameToWrite & "." & M.Meta.FITSExtension))

                'Store file and display if selected
                Select Case SingleStatCalc.DataType
                    Case AstroNET.Statistics.eDataType.UInt16
                        cFITSWriter.Write(M.DB.LastStoredFile, SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, cFITSWriter.eBitPix.Int16, CustomElement)
                    Case AstroNET.Statistics.eDataType.UInt32
                        cFITSWriter.Write(M.DB.LastStoredFile, SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data, cFITSWriter.eBitPix.Int32, CustomElement)
                End Select
                If M.Config.AutoOpenImage = True Then Ato.Utils.StartWithItsEXE(M.DB.LastStoredFile)

            End If
            M.DB.Stopper.Stamp("Store image")

            'Stop conditions
            If M.DB.CurrentExposureIndex > M.Config.CaptureCount Then Exit Do
            If M.DB.StopFlag = True Then Exit Do

        Loop Until 1 = 0

        '================================================================================
        'Stop live mode if used

        StopNow(True)

        'Release buffer handles
        PinHandler = Nothing

        'Close camera if selected 
        If CloseAtEnd = True Then CloseCamera()
        M.DB.Stopper.Stamp("CloseCamera")
        QHY.QHY.StoreCurrentLog()

        '================================================================================
        'Display timing log

        If M.Meta.Log_Timing = True Then
            Log("--------------------------------------------------------------")
            Log("TIMING:")
            LogNoTime(M.DB.Stopper.GetLog)
            Log("--------------------------------------------------------------")
        End If

        'Reset GUI to idle state

        tsslMain.Text = "--IDLE--" : tsslMain.BackColor = ssMain.BackColor
        tsslTemperature.Text = "T = ??? °C" : tsslTemperature.BackColor = ssMain.BackColor
        M.DB.RunningFlag = False

    End Sub

    '''<summary>Remove overscan and dark fields and get only effective area.</summary>
    Private Function GetEffectiveArea(ByRef FullFrame As System.UInt16(,), ByVal EffectiveArea As sRect_UInt) As cIntelIPP.IppStatus
        Dim NoOverscan(CInt(EffectiveArea.Width - 1), CInt(EffectiveArea.Height - 1)) As System.UInt16
        Dim Status_GetROI As cIntelIPP.IppStatus = M.DB.IPP.Copy(FullFrame, NoOverscan, CInt(EffectiveArea.X), CInt(EffectiveArea.Y), CInt(EffectiveArea.Width), CInt(EffectiveArea.Height))
        If Status_GetROI <> cIntelIPP.IppStatus.NoErr Then Return Status_GetROI
        Return M.DB.IPP.Copy(NoOverscan, FullFrame, 0, 0, NoOverscan.GetUpperBound(0) + 1, NoOverscan.GetUpperBound(1) + 1)
    End Function


    '''<returns>TRUE if camera hardware is stopped and exit can be performed.</returns>
    Private Function StopNow(ByVal Force As Boolean) As Boolean
        If M.DB.StopFlag Or Force = True Then
            CallOK("CancelQHYCCDExposing", QHY.QHY.CancelQHYCCDExposing(M.DB.CamHandle))
            CallOK("CancelQHYCCDExposingAndReadout", QHY.QHY.CancelQHYCCDExposingAndReadout(M.DB.CamHandle))
            If M.Config.StreamMode = eStreamMode.LiveFrame Then CallOK("StopQHYCCDLive", QHY.QHY.StopQHYCCDLive(M.DB.CamHandle))
            Return True
        End If
        Return False
    End Function

    Private Function MakeUnique(ByVal FullPath As String) As String
        If System.IO.File.Exists(FullPath) = False Then
            'FullPath OK
        Else
            Dim Dir As String = IO.Path.GetDirectoryName(FullPath)
            Dim FileName As String = IO.Path.GetFileNameWithoutExtension(FullPath)
            Dim FileExt As String = IO.Path.GetExtension(FullPath)
            Dim FileIdx As Integer = 1
            Do
                FullPath = System.IO.Path.Combine(Dir, FileName & "(" & FileIdx.ToString.Trim & ")" & FileExt)
                If System.IO.File.Exists(FullPath) = False Then Exit Do
                FileIdx += 1
            Loop Until 1 = 0
        End If
        Return FullPath
    End Function

    '''<summary>Keep the GUI alive during exposure.</summary>
    '''<param name="ExposureTime">Expected time for this capture.</param>
    Private Sub IdleExposureTime(ByVal ExposureTime As Double)
        If ExposureTime > 0.1 Then
            Dim ExpStart As DateTime = Now
            Dim TimePassed As Double = Double.NaN
            tspbProgress.Maximum = CInt(ExposureTime * 10)
            Do
                ReadTemperature()
                System.Threading.Thread.Sleep(100)
                TimePassed = (Now - ExpStart).TotalSeconds
                If TimePassed < ExposureTime Then
                    Dim TimeToGo As Double = ExposureTime - TimePassed
                    Dim ProgBarVal As Integer = CInt(TimePassed * 10)
                    If ProgBarVal <= tspbProgress.Maximum Then tspbProgress.Value = ProgBarVal
                    Dim TimeFormat As String = CStr(IIf(ExposureTime < 10, "0.0", "0"))
                    tsslProgress.Text = Format(TimePassed, TimeFormat).Trim & "/" & ExposureTime.ToString.Trim & " seconds exposed"
                End If
                DE()
            Loop Until (TimePassed >= ExposureTime) Or (M.DB.StopFlag = True)
        End If
        tspbProgress.Value = 0
        tsslProgress.Text = "-- not exposing right now --"
    End Sub

    '''<summary>Calculate the total time for the running exposure and the estimated ETA.</summary>
    Private Function TimeToGo() As TimeSpan
        Try
            Return TimeSpan.FromSeconds((M.Config.CaptureCount - M.DB.CurrentExposureIndex) * M.Config.ExposureTime)
        Catch ex As Exception
            Return TimeSpan.Zero
        End Try
    End Function

    '''<summary>Calculate the total time for the running exposure and the estimated ETA.</summary>
    Private Function ETA() As DateTime
        Return Now.Add(TimeToGo)
    End Function

    '''<summary>Adjust the size of the requested ROI to the chip properties.</summary>
    '''<param name="NewCenter">New center of the ROI to set.</param>
    '''<param name="NewWidth">New width of the ROI.</param>
    '''<param name="NewHeight">New height of the ROI.</param>
    '''<param name="Chip_Pixel">Size of the CCD ship [pixel].</param>
    Private Function AdjustAndCorrectROI(ByVal NewCenter As Point, ByVal NewWidth As Integer, ByVal NewHeight As Integer) As System.Drawing.Rectangle
        M.Config.ROI = New Rectangle(NewCenter.X - ((NewWidth - 1) \ 2), NewCenter.Y - ((NewHeight - 1) \ 2), NewWidth, NewHeight)
        Return AdjustAndCorrectROI()
    End Function

    '''<summary>Adjust the size of the requested ROI to the chip properties and return the correct value.</summary>
    Private Function AdjustAndCorrectROI() As System.Drawing.Rectangle

        Dim ROIForCapture As New System.Drawing.Rectangle(M.Config.ROI.X, M.Config.ROI.Y, M.Config.ROI.Width, M.Config.ROI.Height)

        '1.) Auto-ROI for value 0
        If ROIForCapture.Width <= 0 Then ROIForCapture.Width = CInt(M.Meta.Chip_Pixel.Width)
        If ROIForCapture.Height <= 0 Then ROIForCapture.Height = CInt(M.Meta.Chip_Pixel.Height)

        '2.) ROI size must be even
        If ROIForCapture.Width Mod 2 <> 0 Then ROIForCapture.Width += 1
        If ROIForCapture.Height Mod 2 <> 0 Then ROIForCapture.Height += 1

        '3.) Ensure ROI fits the chip
        If ROIForCapture.Width > M.Meta.Chip_Pixel.Width Then ROIForCapture.Width = CInt(M.Meta.Chip_Pixel.Width)
        If ROIForCapture.Height > M.Meta.Chip_Pixel.Height Then ROIForCapture.Height = CInt(M.Meta.Chip_Pixel.Height)
        If ROIForCapture.Width < 1 Then ROIForCapture.Width = 1
        If ROIForCapture.Height < 1 Then ROIForCapture.Height = 1

        '4.) Ensure ROI is within the chip
        If ROIForCapture.X < 0 Then ROIForCapture.X = 0
        If ROIForCapture.Y < 0 Then ROIForCapture.Y = 0
        If ROIForCapture.X + ROIForCapture.Width > M.Meta.Chip_Pixel.Width Then ROIForCapture.X = CInt(M.Meta.Chip_Pixel.Width - ROIForCapture.Width)
        If ROIForCapture.Y + ROIForCapture.Height > M.Meta.Chip_Pixel.Height Then ROIForCapture.Height = CInt(M.Meta.Chip_Pixel.Height - ROIForCapture.Height)

        '5.) Return ROI
        Return ROIForCapture

    End Function

    '===============================================================================================
    ' Image and signal processing
    '===============================================================================================

    'Re-orient the image data in the buffer ("first in X direction" becomes "first in Y direction")
    Private Function ChangeAspectIPP(ByRef IntelIPP As cIntelIPP, ByRef RawBuffer() As Byte, ByVal TargetWidth As Integer, ByVal TargetHeight As Integer) As UInt16(,)
        Dim RetVal(TargetWidth - 1, TargetHeight - 1) As UInt16
        IntelIPP.Transpose(RawBuffer, RetVal)
        Return RetVal
    End Function

    '===============================================================================================
    ' Logging and error handling
    '===============================================================================================

    Private Function CallOK(ByVal ErrorCode As UInteger) As Boolean
        Return CallOK(String.Empty, ErrorCode)
    End Function

    Private Function CallOK(ByVal Action As String, ByVal ErrorCode As UInteger) As Boolean
        If ErrorCode = 0 Then
            Return True
        Else
            LogError("QHY ERROR on <" & Action & ">: <0x" & Hex(ErrorCode) & ">")
            Return False
        End If
    End Function

    Private Sub LogTime(ByVal Text As String, ByVal DurationMS As Long)
        Log(Text & " took " & DurationMS.ValRegIndep & " ms")
    End Sub

    Private Sub LogTiming(ByVal Text As String, ByVal Ticker As System.Diagnostics.Stopwatch)
        Log(Text & ": " & Ticker.ElapsedMilliseconds.ValRegIndep & " ms")
    End Sub

    Private Sub LogError(ByVal Entries As List(Of String))
        For Each Entry As String In Entries
            LogError(Entry)
        Next
    End Sub

    Private Sub LogError(ByVal Text As String)
        Log("########### " & Text & " ###########")
    End Sub

    Private Sub LogVerbose(ByVal Text As String)
        If M.Meta.Log_Verbose = True Then Log(Text)
    End Sub

    Private Sub LogVerbose(ByVal Text As List(Of String))
        For Each Line As String In Text
            LogVerbose(Line)
        Next Line
    End Sub

    Private Sub Log(ByVal Text As List(Of String))
        For Each Line As String In Text
            Line = Now.ForLogging & "|" & Line
            If M.DB.Log_Generic.Length = 0 Then
                M.DB.Log_Generic.Append(Text)
            Else
                M.DB.Log_Generic.Append(System.Environment.NewLine & Line)
            End If
        Next Line
        DisplayLog()
    End Sub

    '''<summary>Append list of text but do not add time (e.g. as this is a time-stamp log).</summary>
    '''<param name="Text"></param>
    Private Sub LogNoTime(ByVal Text As List(Of String))
        For Each Line As String In Text
            If M.DB.Log_Generic.Length = 0 Then
                M.DB.Log_Generic.Append(Text)
            Else
                M.DB.Log_Generic.Append(System.Environment.NewLine & Line)
            End If
        Next Line
        DisplayLog()
    End Sub

    Private Sub LogStart(ByVal Text As String)
        Log(Text)
    End Sub

    Private Sub Log(ByVal Text As String)
        Text = Now.ForLogging & "|" & Text
        If M.DB.Log_Generic.Length = 0 Then
            M.DB.Log_Generic.Append(Text)
        Else
            M.DB.Log_Generic.Append(System.Environment.NewLine & Text)
        End If
        DisplayLog()
    End Sub

    Private Sub DisplayLog()
        With tbLogOutput
            .Text = M.DB.Log_Generic.ToString
            If tbLogOutput.Text.Length > 0 Then
                .SelectionStart = .Text.Length - 1
                .SelectionLength = 0
            End If
            .ScrollToCaret()
        End With
        DE()
    End Sub

    Private Sub EndAction()
        Log("=========================================")
        tsslMain.Text = "--IDLE--"
        DE()
    End Sub

    Private Sub DE()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub tsmiFile_ExploreHere_Click(sender As Object, e As EventArgs) Handles tsmiFile_ExploreHere.Click
        Ato.Utils.StartWithItsEXE(M.DB.EXEPath)
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        tsmiNewGUID_Click(Nothing, Nothing)

        Me.Text = GetBuildDateTime.GetMainformTitle

        'Load IPP
        cFITSWriter.UseIPPForWriting = False
        Dim IPPLoadError As String = String.Empty
        Dim IPPPathToUse As String = cIntelIPP.SearchDLLToUse(cIntelIPP.PossiblePaths(M.DB.EXEPath).ToArray, IPPLoadError)
        If String.IsNullOrEmpty(IPPPathToUse) = True Then
            Log("Intel IPP not installed!")
        Else
            If String.IsNullOrEmpty(IPPLoadError) = False Then
                Log("Intel IPP found in <" & IPPPathToUse & "> but could not be loaded: <" & IPPLoadError & ">")
            Else
                M.DB.IPP = New cIntelIPP(IPPPathToUse)
                cFITSWriter.UseIPPForWriting = True
                cFITSWriter.IPPPath = M.DB.IPP.IPPPath
                Dim TestResult As String = M.DB.IPP.TestIPP
                If String.IsNullOrEmpty(TestResult) = False Then
                    Log("Error calling Intel IPP: <" & TestResult & ">")
                End If
            End If
        End If

        'Set FITS viewer
        Dim FileName As String = "FITSWork4.exe"
        Dim Locations As List(Of String) = Everything.GetExactMatch(FileName, Everything.GetSearchResult(FileName))
        If Locations.Count > 0 Then M.Meta.FITSViewer = Locations(0)

        'Start WCF
        'netsh http add urlacl url=http://+:1250/ user=DESKTOP-I7\albusmw
        DB_ServiceContract = New cDB_ServiceContract(M.DB, M.Meta)
        If M.Meta.WebInterfacePort <> "0" Then
            Dim WebServiceAdr As String = "http://localhost:" & M.Meta.WebInterfacePort & "/"
            'M.DB.SetupWCF = New ServiceModel.Web.WebServiceHost(GetType(cDB_ServiceContract), New Uri(WebServiceAdr))
            'M.DB.serviceBehavior = M.DB.SetupWCF.Description.Behaviors.Find(Of ServiceModel.Description.ServiceDebugBehavior)
            'M.DB.serviceBehavior.HttpHelpPageEnabled = True
            'M.DB.serviceBehavior.IncludeExceptionDetailInFaults = True
            Try
                ' M.DB.SetupWCF.Open()
            Catch ex As Exception
                Log("Error creating WCF interface: <" & ex.Message & ">")
            End Try
        End If

        'Other objects
        M.Report.Plotter = New cZEDGraph(zgcMain)
        RefreshProperties()

        'Set toolstrip icons
        tsbCapture.Image = ilMain.Images.Item("Capture.png")
        tsbStopCapture.Image = ilMain.Images.Item("StopCapture.png")

        'RTF statistics
        RTFGen.AttachToControl(rtbStatistics)
        RTFGen.RTFInit("Courier New", 8)

        'MIDI monitor
        MIDI = New cMIDIMonitor
        If MIDI.MIDIDeviceCount > 0 Then MIDI.SelectMidiDevice(0)

        'Load DSC if running on DSC PC
        Select Case Environment.MachineName
            Case "NUCDSCALBUSMW"
                tsmiPreset_DSC_Click(Nothing, Nothing)
        End Select

        'Show DB
        RefreshProperties()

        'Set position
        Me.Width = 1600
        Me.Height = 900

    End Sub

    Private Sub tsmiFile_Exit_Click(sender As Object, e As EventArgs) Handles tsmiFile_Exit.Click
        End
    End Sub

    Private Sub tSetTemp_Tick(sender As Object, e As EventArgs) Handles tSetTemp.Tick
        If (M.DB.CamHandle <> IntPtr.Zero) And TargetTempResonable() Then
            QHY.QHY.ControlQHYCCDTemp(M.DB.CamHandle, M.Config.Temp_Target)
        End If
    End Sub

    Private Sub tsbCapture_Click(sender As Object, e As EventArgs) Handles tsbCapture.Click
        If CType(sender, System.Windows.Forms.ToolStripButton).Enabled = True Then QHYCapture(True)
        M.DB.StopFlag = False
    End Sub

    Private Sub tsbStopCapture_Click(sender As Object, e As EventArgs) Handles tsbStopCapture.Click
        M.DB.StopFlag = True
    End Sub

    Private Sub AllReadoutModesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllReadoutModesToolStripMenuItem.Click
        M.DB.StopFlag = False
        For Each Mode As eReadOutMode In [Enum].GetValues(GetType(eReadOutMode))
            M.Config.ReadOutModeEnum = Mode
            RefreshProperties()
            QHYCapture(False)
            If M.DB.StopFlag = True Then Exit For
        Next Mode
        CloseCamera()
    End Sub

    Private Sub ExposureTimeSeriesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExposureTimeSeriesToolStripMenuItem.Click
        M.DB.StopFlag = False
        'For lust of all exposure times
        Dim AllExposureTimes As New List(Of Double)
        For Exp As Integer = 0 To 2
            For Base As Double = 1 To 9
                AllExposureTimes.Add(Base * (10 ^ Exp))
            Next Base
        Next Exp
        AllExposureTimes.Sort()
        'Run series
        For Each Exposure As Double In AllExposureTimes
            M.Config.ExposureTime = Exposure
            RefreshProperties()
            QHYCapture(False)
            If M.DB.StopFlag = True Then Exit For
        Next Exposure
        CloseCamera()
    End Sub

    Private Sub GainVariationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GainVariationToolStripMenuItem.Click
        M.DB.StopFlag = False
        For Gain As Double = 0 To 200 Step 1
            M.Config.Gain = Gain
            For Exp As Integer = 1 To 60 Step 1
                M.Config.ExposureTime = Exp
                RefreshProperties()
                QHYCapture(False)
            Next Exp
            If M.DB.StopFlag = True Then Exit For
        Next Gain
        CloseCamera()
    End Sub

    Private Sub tsmiFile_TestWebInterface_Click(sender As Object, e As EventArgs) Handles tsmiFile_TestWebInterface.Click
        'Test call for the web interface
        System.Diagnostics.Process.Start("http//localhost:1250/GetParameterList")
    End Sub

    Private Sub DB_ServiceContract_ValueChanged() Handles DB_ServiceContract.ValueChanged
        RefreshProperties()
    End Sub

    Private Sub DB_ServiceContract_StartExposure() Handles DB_ServiceContract.StartExposure
        tsbCapture_Click(tsbCapture, Nothing)
    End Sub

    Private Sub tsmiPreset_FastLive_Click(sender As Object, e As EventArgs) Handles tsmiPreset_FastLive.Click
        With M.Config
            .StreamMode = eStreamMode.LiveFrame
            .ExposureTime = 0.01
            .Gain = 20
            .CaptureCount = Int32.MaxValue
            .StoreImage = False
            .DDR_RAM = False
            .ConfigAlways = False
            .Filter = cFilterWheelHelper.eFilter.Unchanged
            .USBTraffic = 0
            .Temp_Target = -100
            .Temp_Tolerance = 1000
            .Temp_StableTime = 0
        End With
        With M.Meta
            .Load10MicronDataAlways = False
        End With
        With M.Report.Config
            .Log_ClearStat = True
            .PlotStatisticsColor = False
            .PlotStatisticsMono = False
            .PlotMeanStatistics = False
            .PlotSingleStatistics = False
        End With
        RefreshProperties()
    End Sub

    Private Sub tsmiPreset_Click(sender As Object, e As EventArgs) Handles tsmiPreset_CenterROI.Click
        Dim ROISize As Integer = CInt(InputBox("Size", "ROI size", "100")) : ROISize = ROISize \ 2
        If ROISize > 0 Then
            With M.Config
                .ROI = New Drawing.Rectangle((9600 \ 2) - ROISize, (6422 \ 2) - ROISize, 2 * ROISize, 2 * ROISize)
            End With
        End If
        RefreshProperties()
    End Sub

    '''<summary>Refresh all property grid displays.</summary>
    Private Sub RefreshProperties()
        pgMain.SelectedObject = M.Config
        pgMeta.SelectedObject = M.Meta
        pgPlotAndText.SelectedObject = M.Report.Config
        DE()
    End Sub

    Private Sub TestSeriesToolStripMenuItem_Click(sender As Object, e As EventArgs)
        M.DB.StopFlag = False
        M.Config.StoreImage = True
        M.Config.AutoOpenImage = False
        M.Config.ExposureTime = 60
        For Each ReadOutMode As eReadOutMode In [Enum].GetValues(GetType(eReadOutMode))
            If ReadOutMode <> eReadOutMode.Invalid Then
                M.Config.ReadOutModeEnum = ReadOutMode
                For Each Filter As cFilterWheelHelper.eFilter In New cFilterWheelHelper.eFilter() {cFilterWheelHelper.eFilter.H_alpha}
                    M.Config.Filter = Filter
                    For Gain As Double = 0 To 200 Step 1
                        M.Config.Gain = Gain
                        Load10MicronData()
                        RefreshProperties()
                        QHYCapture(False)
                        If M.DB.StopFlag = True Then Exit For
                    Next Gain
                    If M.DB.StopFlag = True Then Exit For
                Next Filter
                If M.DB.StopFlag = True Then Exit For
            End If
        Next ReadOutMode
        CloseCamera()
    End Sub

    Private Sub tsmiFile_ExploreCampaign_Click(sender As Object, e As EventArgs) Handles tsmiFile_ExploreCampaign.Click
        Try
            Dim FolderToOpen As String = System.IO.Path.Combine(M.Config.StoragePath, M.Meta.GUID)
            If System.IO.Directory.Exists(FolderToOpen) = True Then Ato.Utils.StartWithItsEXE(FolderToOpen)
        Catch ex As Exception
            'No nothing ...
        End Try
    End Sub

    Private Sub tsmiActions_ResetLoopStat_Click(sender As Object, e As EventArgs) Handles tsmiActions_ResetLoopStat.Click
        LoopStat = New AstroNET.Statistics.sStatistics
        LoopStat.Count = 0
    End Sub

    Private Sub pgMain_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles pgMain.PropertyValueChanged
        PropertyChanged = True
        pgMain.SelectedObject = M.Config
    End Sub

    Private Sub tsmiFile_StoreEXCELStat_Click(sender As Object, e As EventArgs) Handles tsmiFile_StoreEXCELStat.Click

        Dim AddHisto As Boolean = True

        With sfdMain
            .Filter = "EXCEL file (*.xlsx)|*.xlsx"
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With

        Using workbook As New ClosedXML.Excel.XLWorkbook

            '1.) Histogram
            If AddHisto = True Then
                Dim XY As New List(Of Object())
                For Each Key As Long In LoopStat.MonochromHistogram_Int.Keys
                    Dim Values As New List(Of Object)
                    Values.Add(Key)
                    Values.Add(LoopStat.MonochromHistogram_Int(Key))
                    If LoopStat.BayerHistograms_Int(0, 0).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(0, 0)(Key)) Else Values.Add(String.Empty)
                    If LoopStat.BayerHistograms_Int(0, 1).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(0, 1)(Key)) Else Values.Add(String.Empty)
                    If LoopStat.BayerHistograms_Int(1, 0).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(1, 0)(Key)) Else Values.Add(String.Empty)
                    If LoopStat.BayerHistograms_Int(1, 1).ContainsKey(Key) Then Values.Add(LoopStat.BayerHistograms_Int(1, 1)(Key)) Else Values.Add(String.Empty)
                    XY.Add(Values.ToArray)
                Next Key
                Dim worksheet As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Histogram")
                worksheet.Cell(1, 1).InsertData(New List(Of String)({"Pixel value", "Count Mono", "Count Bayer_0_0", "Count Bayer_0_1", "Count Bayer_1_0", "Count Bayer_1_1"}), True)
                worksheet.Cell(2, 1).InsertData(XY)
                For Each col As ClosedXML.Excel.IXLColumn In worksheet.ColumnsUsed
                    col.AdjustToContents()
                Next col
            End If

            '2.) Histo density
            Dim HistDens As New List(Of Object())
            For Each Key As UInteger In LoopStat.MonoStatistics_Int.HistXDist.Keys
                HistDens.Add(New Object() {Key, LoopStat.MonoStatistics_Int.HistXDist(Key)})
            Next Key
            Dim worksheet2 As ClosedXML.Excel.IXLWorksheet = workbook.Worksheets.Add("Histogram Density")
            worksheet2.Cell(1, 1).InsertData(New List(Of String)({"Step size", "Count"}), True)
            worksheet2.Cell(2, 1).InsertData(HistDens)
            For Each col As ClosedXML.Excel.IXLColumn In worksheet2.ColumnsUsed
                col.AdjustToContents()
            Next col

            '4) Save and open
            Dim FileToGenerate As String = IO.Path.Combine(M.Config.StoragePath, sfdMain.FileName)
            workbook.SaveAs(FileToGenerate)

        End Using

    End Sub

    Private Sub tsmiNewGUID_Click(sender As Object, e As EventArgs) Handles tsmiNewGUID.Click
        M.Meta.GUID = Now.ForFileSystem
        RefreshProperties()
    End Sub

    Private Sub tsmiFile_RunSequence_Click(sender As Object, e As EventArgs) Handles tsmiFile_RunSequence.Click, tsmiFile_LoadSettings.Click
        'Load / run the passed XML config file
        With ofdMain
            .Filter = "XML definitions (*.qhycapture.xml)|*.qhycapture.xml"
            .FileName = String.Empty
            .Multiselect = False
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With
        Dim Run As Boolean = CBool(IIf(CType(sender, ToolStripMenuItem).Tag.ToString.Trim.ToUpper = "RUN", True, False))
        LogError(RunXMLSequence(ofdMain.FileName, Run))
    End Sub

    Private Function TabFormat(ByVal Text As Integer) As String
        Return Text.ValRegIndep.PadLeft(10)
    End Function

    Private Sub tStatusUpdate_Tick(sender As Object, e As EventArgs) Handles tStatusUpdate.Tick
        tsslMemory.Text = "Memory " & Format(GetMyMemSize, "0.0") & " MByte"
        tsslETA.Text = "ETA " & Format(ETA() & " ( total capture time: " & TimeToGo.ToString & ")")
    End Sub

    '''<summary>Get the memory consumption [MByte] of this EXE.</summary>
    '''<remarks>This functions needs a significant time !!!!!</remarks>
    Private Function GetMyMemSize() As Double
        Return Process.GetCurrentProcess.PrivateMemorySize64 / 1048576
    End Function

    Private Sub tsmiFile_OpenLastFile_Click(sender As Object, e As EventArgs) Handles tsmiFile_OpenLastFile.Click
        If System.IO.File.Exists(M.DB.LastStoredFile) Then
            If String.IsNullOrEmpty(M.Meta.FITSViewer) = True Then
                Ato.Utils.StartWithItsEXE(M.DB.LastStoredFile)
            Else
                Process.Start(M.Meta.FITSViewer, M.DB.LastStoredFile)
            End If
        End If
    End Sub

    Private Sub tsmiPreset_SaveTransmission_Click(sender As Object, e As EventArgs) Handles tsmiPreset_SaveTransmission.Click
        With M.Config
            .StreamMode = eStreamMode.LiveFrame
            .CaptureCount = 1000000
            .USBTraffic = 25
            .ExposureTime = 0.001
            .StoreImage = False
            .ConfigAlways = True
        End With
        With M.Report.Config
            .PlotSingleStatistics = True
            .PlotMeanStatistics = True
        End With
        RefreshProperties()
    End Sub

    Private Sub tsmiClearLog_Click(sender As Object, e As EventArgs) Handles tsmiClearLog.Click
        M.DB.Log_Generic.Clear()
        DisplayLog()
    End Sub

    Private Sub tsbCooling_Click(sender As Object, e As EventArgs) Handles tsbCooling.Click
        If InitQHY(M.Config.CamToUse) = True Then
            Dim Cooling As New frmCooling
            Cooling.Show()
        Else
            Log("No suitable camera found!")
        End If
    End Sub

    Private Sub tsmiFile_SaveAllXMLParameters_Click(sender As Object, e As EventArgs) Handles tsmiFile_SaveAllXMLParameters.Click
        'Write all available properties
        Dim FileOut As New List(Of String)
        FileOut.AddRange(DB_config.GetAllPropertyNames(True))
        FileOut.AddRange(DB_meta.GetAllPropertyNames(True))
        FileOut.AddRange(DB_report.GetAllPropertyNames(True))
        Dim XMLXMLParameterFile As String = System.IO.Path.Combine(M.DB.EXEPath, "AllXMLParameters.txt")
        System.IO.File.WriteAllLines(XMLXMLParameterFile, FileOut)
        Ato.Utils.StartWithItsEXE(XMLXMLParameterFile)
    End Sub

    Private Sub tsmiFile_CreateXML_Click(sender As Object, e As EventArgs) Handles tsmiFile_CreateXML.Click
        Dim XMLGeneration As New frmXMLGeneration
        XMLGeneration.Show()
    End Sub

    Private Sub tsmiPreset_SkipCooling_Click(sender As Object, e As EventArgs) Handles tsmiPreset_SkipCooling.Click
        'Build the settings XML in memory
        Dim MemStream As New System.IO.MemoryStream
        Dim SettingDoc As New System.Xml.XmlDocument
        Using XMLContent As System.Xml.XmlWriter = SettingDoc.CreateNavigator.AppendChild
            XMLContent.WriteStartDocument()
            XMLContent.WriteStartElement("sequence")
            XMLContent.WriteStartElement("exp")
            XMLContent.WriteAttributeString("Temp_Target", "-100")
            XMLContent.WriteAttributeString("Temp_Tolerance", "1000")
            XMLContent.WriteAttributeString("Temp_StableTime", "0")
            XMLContent.WriteEndElement()
            XMLContent.WriteEndElement()
            XMLContent.WriteEndDocument()
        End Using
        'Load it
        LogError(RunXMLSequence(SettingDoc, False))
    End Sub

    Private Sub tcMain_KeyUp(sender As Object, e As KeyEventArgs) Handles tcMain.KeyUp
        'Display property name
        If e.KeyCode = Keys.F2 Then
            Dim PropName As String = String.Empty
            Select Case tcMain.SelectedIndex
                Case 0
                    PropName = pgMain.SelectedGridItem.PropertyDescriptor.Name
                Case 1
                    PropName = pgMeta.SelectedGridItem.PropertyDescriptor.Name
                Case 2
                    PropName = pgPlotAndText.SelectedGridItem.PropertyDescriptor.Name
            End Select
            If MsgBox(PropName & System.Environment.NewLine & "Copy to clipboard?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question Or MsgBoxStyle.DefaultButton1) = MsgBoxResult.Yes Then
                Clipboard.Clear()
                Clipboard.SetText(PropName)
            End If
        End If
    End Sub

    Private Sub tsmiTools_AllQHYDLLs_Click(sender As Object, e As EventArgs) Handles tsmiTools_AllQHYDLLs.Click
        'Get all qhyccd.dll version and display them
        Dim AllDLLs As List(Of String) = Everything.GetSearchResult("qhyccd.dll")
        Dim DifferentDLLs As New Dictionary(Of String, String)
        Dim RTFScanReport As New List(Of String)
        For Each DLLFile As String In AllDLLs
            Dim Hash As String = FileHash.MD5(DLLFile)
            If String.IsNullOrEmpty(Hash) = False Then
                Dim VersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(DLLFile)
                Dim VersionString As String = VersionInfo.FileVersion : If IsNothing(VersionString) = True Then VersionString = "???"
                VersionString = VersionString.Replace(",", ".").Replace(" ", String.Empty).PadRight(12)
                Dim InfoLine As String = VersionString & " of " & System.IO.File.GetCreationTime(DLLFile) & " -> " & DLLFile
                If DifferentDLLs.ContainsKey(Hash) = False Then
                    DifferentDLLs.Add(Hash, DLLFile)
                    RTFScanReport.Add("*** " & InfoLine)
                Else
                    RTFScanReport.Add("    " & InfoLine)
                End If
            End If
        Next DLLFile
        RTFScanReport.Add(DifferentDLLs.Count.ValRegIndep & " different verions found.")
        'Display info
        Dim Info As New cRTFTextBox
        Info.Init("Different qhyccd.dll versions", 1200, 400)
        Info.ShowText(RTFScanReport)
    End Sub

    Private Sub tsmiPreset_DevTestMWeiss_Click(sender As Object, e As EventArgs) Handles tsmiPreset_DevTestMWeiss.Click
        ''Build the settings XML in memory
        Dim MemStream As New System.IO.MemoryStream
        Dim SettingDoc As New System.Xml.XmlDocument
        Using XMLContent As System.Xml.XmlWriter = SettingDoc.CreateNavigator.AppendChild
            XMLContent.WriteStartDocument()
            XMLContent.WriteStartElement("sequence")
            XMLContent.WriteStartElement("exp")
            XMLContent.WriteAttributeString("Temp_Target", "--100")
            XMLContent.WriteAttributeString("Temp_Tolerance", "1000")
            XMLContent.WriteAttributeString("Temp_StableTime", "0")
            XMLContent.WriteAttributeString("StoreImage", "False")
            XMLContent.WriteAttributeString("ExposureTypeEnum", "eExposureType.Test")
            XMLContent.WriteAttributeString("Log_CamProp", "True")
            XMLContent.WriteAttributeString("Log_Timing", "True")
            XMLContent.WriteAttributeString("Log_Verbose", "True")
            XMLContent.WriteAttributeString("Load10MicronDataAlways", "False")
            XMLContent.WriteEndElement()
            XMLContent.WriteEndElement()
            XMLContent.WriteEndDocument()
        End Using
        'Load it
        LogError(RunXMLSequence(SettingDoc, False))
    End Sub

    Private Sub tsmiPreset_NoOverhead_Click(sender As Object, e As EventArgs) Handles tsmiPreset_NoOverhead.Click
        With M.Config
            .Temp_Target = -100
            .Temp_Tolerance = 1000
            .Temp_StableTime = 0
            .ExposureTime = 0.001
            .StoreImage = False
            .ConfigAlways = False
        End With
        With M.Meta
            .Load10MicronDataAlways = False
        End With
        RefreshProperties()
    End Sub

    Private Sub tsmiTools_Log_Store_Click(sender As Object, e As EventArgs) Handles tsmiTools_Log_Store.Click
        Dim LogFile As String = System.IO.Path.Combine(M.DB.EXEPath, "QHYDLLCalls.log")
        System.IO.File.WriteAllLines(LogFile, QHY.QHY.GetCallLog.ToArray)
        Ato.Utils.StartWithItsEXE(LogFile)
    End Sub

    Private Sub tsmiTools_Log_Clear_Click(sender As Object, e As EventArgs) Handles tsmiTools_Log_Clear.Click
        If MsgBox("Are you sure to clean the log?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Question) = MsgBoxResult.Ok Then QHY.QHY.CallLog.Clear()
    End Sub

    Private Sub AllCoolersOffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiActions_AllCoolersOff.Click
        If CallOK(QHY.QHY.InitQHYCCDResource) = True Then
            Dim CameraCount As UInteger = QHY.QHY.ScanQHYCCD
            If CameraCount > 0 Then
                Log(CameraCount.ValRegIndep & " cameras found")
                For Idx As Integer = 0 To CInt(CameraCount - 1)
                    Dim CurrentCamID As New System.Text.StringBuilder(0)
                    If CallOK(QHY.QHY.GetQHYCCDId(Idx, CurrentCamID)) = True Then
                        Log("  Open <" & CurrentCamID.ToString & ">")
                        M.DB.CamHandle = QHY.QHY.OpenQHYCCD(CurrentCamID)
                        If M.DB.CamHandle <> IntPtr.Zero Then
                            QHY.QHY.CancelQHYCCDExposingAndReadout(M.DB.CamHandle)
                            QHY.QHY.ControlQHYCCDTemp(M.DB.CamHandle, 40.0)
                            Log("    Target temperature set to 40°C")
                        End If
                    End If
                Next Idx
            End If
            QHY.QHY.ReleaseQHYCCDResource()
            M.DB.CamHandle = IntPtr.Zero
            Log("All coolers deactivated")
        End If
    End Sub

    Private Sub tsmiPreset_StandardCapture_Click(sender As Object, e As EventArgs) Handles tsmiPreset_StandardCapture.Click
        With M.Config
            .StatColor = False
            .Temp_Target = -20.0
            .Temp_Tolerance = 0.2
            .Temp_TimeOutAndOK = 600.0
            .ExposureTypeEnum = eExposureType.Light
        End With
        M.Meta.Load10MicronDataAlways = True
        M.Meta.ObjectName = InputBox("Object", "Object", M.Meta.ObjectName)
        RefreshProperties()
    End Sub

    '===================================================================================================

    '''<summary>Handle data entered via a MIDI input device.</summary>
    Private Sub MIDI_Increment(Channel As Integer, Value As Integer) Handles MIDI.Increment
        Select Case Channel
            Case 1
                M.Config.Gain += Value
            Case 2
                M.Meta.WhiteBalance_Red += Value
            Case 3
                M.Meta.WhiteBalance_Green += Value
            Case 4
                M.Meta.WhiteBalance_Blue += Value
            Case 5
                M.Meta.Contrast += Value / 100
            Case 6
                M.Meta.Brightness += Value / 100
        End Select
        RefreshProperties()
    End Sub

    Private Sub MIDI_Reset(Channel As Integer) Handles MIDI.Reset
        Select Case Channel
            Case 1
                M.Config.Gain = 0
            Case 2
                M.Meta.WhiteBalance_Red = 128
            Case 3
                M.Meta.WhiteBalance_Green = 128
            Case 4
                M.Meta.WhiteBalance_Blue = 128
            Case 5
                M.Meta.Contrast = 0.0
            Case 6
                M.Meta.Brightness = 0.0
        End Select
        RefreshProperties()
    End Sub

    Private Sub tsmiFile_SaveSettings_Click(sender As Object, e As EventArgs) Handles tsmiFile_SaveSettings.Click
        'Get the file name to save
        With sfdMain
            .Filter = "XML definitions (*.qhycapture.xml)|*.qhycapture.xml"
            .FileName = M.Meta.GUID & ".qhycapture.xml"
            If .ShowDialog <> DialogResult.OK Then Exit Sub
        End With
        'Create the XML file to save
        Dim FileOut As New System.Xml.XmlDocument
        Dim Sequence As System.Xml.XmlNode = FileOut.AppendChild(FileOut.CreateNode(System.Xml.XmlNodeType.Element, "sequence", String.Empty))
        Dim Exp As System.Xml.XmlNode = Sequence.AppendChild(FileOut.CreateNode(System.Xml.XmlNodeType.Element, "exp", String.Empty))
        PropGridToXML(Exp, M.DB)
        PropGridToXML(Exp, M.Meta)
        FileOut.Save(sfdMain.FileName)
        Ato.Utils.StartWithItsEXE(sfdMain.FileName)
    End Sub

    Private Sub tsmiFile_OpenINI_Click(sender As Object, e As EventArgs)
        If System.IO.File.Exists(M.DB.MyINI) Then
            Ato.Utils.StartWithItsEXE(M.DB.MyINI)
        Else
            MsgBox("INI file <" & M.DB.MyINI & "> Not found!", vbCritical Or MsgBoxStyle.OkOnly, "File Not found")
        End If
    End Sub

    Private Sub tsmiPreset_DSC_Click(sender As Object, e As EventArgs) Handles tsmiPreset_DSC.Click
        'Build the settings XML in memory
        Dim MemStream As New System.IO.MemoryStream
        Dim SettingDoc As New System.Xml.XmlDocument
        Using XMLContent As System.Xml.XmlWriter = SettingDoc.CreateNavigator.AppendChild
            With XMLContent
                .WriteStartDocument()
                .WriteStartElement("sequence")
                .WriteStartElement("exp")
                .WriteAttributeString("Load10MicronDataAlways", "false")
                .WriteAttributeString("IP_PWI4_URL", "http://localhost:8220")
                .WriteAttributeString("SiteLongitude", "-70:51:11.8'")
                .WriteAttributeString("SiteLatitude", "-30:31:34.7'")
                .WriteAttributeString("Origin", "Deep Sky Chile")
                .WriteAttributeString("Telescope", "DeltaRho350")
                .WriteAttributeString("TelescopeAperture", "350.0")
                .WriteAttributeString("TelescopeFocalLength", "1050.0")
                .WriteAttributeString("FilterOrder", "L-R-G-B-S-H-O")
                .WriteAttributeString("ExposureTypeEnum", "Light")
                .WriteAttributeString("ExposureTime", "180")
                .WriteAttributeString("CaptureCount", "30")
                .WriteAttributeString("Temp_Target", "-15")
                .WriteAttributeString("Temp_Tolerance", "0.5")
                .WriteAttributeString("Gain", "56")
                .WriteEndElement()
                .WriteEndElement()
                .WriteEndDocument()
            End With
        End Using
        'Load it
        LogError(RunXMLSequence(SettingDoc, False))
    End Sub

    Private Sub tsmiPreset_Test_JustOnce_Click(sender As Object, e As EventArgs) Handles tsmiPreset_Test_JustOnce.Click
        With M.Config
            .Temp_Target = 30
            .Temp_Tolerance = 1000
            .Temp_StableTime = 0
            .ExposureTime = 1
            .CaptureCount = 1
        End With
    End Sub

    Private Sub QHYFunction_Log(Text As String) Handles QHYFunction.Log
        Log(Text)
    End Sub

    Private Sub QHYFunction_LogError(Text As String) Handles QHYFunction.LogError
        LogError(Text)
    End Sub

    Private Sub QHYFunction_LogVerbose(Text As String) Handles QHYFunction.LogVerbose
        LogVerbose(Text)
    End Sub

    Private Sub pgMain_SelectedGridItemChanged(sender As Object, e As SelectedGridItemChangedEventArgs) Handles pgMain.SelectedGridItemChanged
        Try
            tbItemName.Text = "Item name: " & pgMain.SelectedGridItem.PropertyDescriptor.Name
        Catch ex As Exception
            tbItemName.Text = "Item name: ???"
        End Try
    End Sub

    Private Sub LoadPWI4Data()
        If String.IsNullOrEmpty(M.Meta.IP_PWI4_URL) = False Then
            Dim CurrentStatus As String = Download.GetResponse(M.Meta.IP_PWI4_URL & "/status")
            M.DB.PWI4.ProcessStatus(CurrentStatus)
            M.Meta.TelescopeFocusAsSet = CType(M.DB.PWI4.GetValue(ePWI4.focuser__position), Integer)
            M.Meta.Tel_RA = CType(M.DB.PWI4.GetValue(ePWI4.mount__ra_j2000_hours), String).ValRegIndep * (360 / 24)
            M.Meta.Tel_DEC = CType(M.DB.PWI4.GetValue(ePWI4.mount__dec_j2000_degs), String).ValRegIndep
            M.Meta.TelescopeAltitude = CType(M.DB.PWI4.GetValue(ePWI4.mount__altitude_degs), String).ValRegIndep.ToDegMinSec(":")
            M.Meta.TelescopeAzimuth = CType(M.DB.PWI4.GetValue(ePWI4.mount__azimuth_degs), String).ValRegIndep.ToDegMinSec(":")
            M.Meta.SiteLatitude = CType(M.DB.PWI4.GetValue(ePWI4.site__latitude_degs), String).ValRegIndep.ToDegMinSec(":")
            M.Meta.SiteLongitude = CType(M.DB.PWI4.GetValue(ePWI4.site__longitude_degs), String).ValRegIndep.ToDegMinSec(":")
            M.Meta.SiteHeight = CType(M.DB.PWI4.GetValue(ePWI4.site__height_meters), String)
            RefreshProperties()
        End If
    End Sub

    Private Sub tsmiActions_Mount_10Micron_Click(sender As Object, e As EventArgs) Handles tsmiActions_Mount_10Micron.Click
        Load10MicronData()
    End Sub

    Private Sub tsmiActions_Mount_PWI4_Click(sender As Object, e As EventArgs) Handles tsmiActions_Mount_PWI4.Click
        LoadPWI4Data()
    End Sub

End Class
#Enable Warning CA1416 ' Validate platform compatibility