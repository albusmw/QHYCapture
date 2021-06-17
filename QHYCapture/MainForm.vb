Option Explicit On
Option Strict On
Imports System.Windows.Forms

Partial Public Class MainForm

    '''<summary>WCF interface.</summary>
    Private WithEvents DB_ServiceContract As cDB_ServiceContract
    '''<summary>Indicate that a property was changed and parameters need to be updated in the camera.</summary>
    Private PropertyChanged As Boolean = False
    '''<summary>RTF report generator.</summary>
    Private RTFGen As New Ato.cRTFGenerator
    '''<summary>Focus window.</summary>
    Private FocusWindow As cImgForm = Nothing
    '''<summary>Accumulated statistics.</summary>
    Private LoopStat As AstroNET.Statistics.sStatistics
    '''<summary>Monitor for the MIDI events.</summary>
    Private WithEvents MIDI As cMIDIMonitor

    Private WithEvents ZWOASI As New cZWOASI

    '''<summary>Run a single capture.</summary>
    Private Sub RunCaptureToolStripMenuItem_Click(sender As Object, e As EventArgs)
        QHYCapture(True)
    End Sub

    '''<summary>Command for a QHY capture run.</summary>
    Public Sub QHYCapture(ByVal CloseAtEnd As Boolean)

        Const BitsPerByte As Integer = 8
        Dim EffArea As sRect_UInt
        Dim OverArea As sRect_UInt
        Dim bpp As UInteger = 0

        'Set DLL log path
        QHY.QHY.LogFile = M.Meta.QHYLogFile

        'Start
        M.DB.StopFlag = False
        M.DB.RunningFlag = True
        M.DB.Stopper.Start()

        'Try to get a suitable camera and continue if found
        LED_update(tsslLED_init, True)
        If InitQHY(M.DB.CamToUse) = False Then Log("No suitable camera found!")
        LED_update(tsslLED_init, False)
        If M.DB.CamHandle = IntPtr.Zero Then Exit Sub

        LED_update(tsslLED_config, True)

        'Get chip properties & SDK version
        QHY.QHY.GetQHYCCDChipInfo(M.DB.CamHandle, M.Meta.MyChip_Physical.Width, M.Meta.MyChip_Physical.Height, M.Meta.MyChip_Pixel.Width, M.Meta.MyChip_Pixel.Height, M.Meta.MyPixel_Size.Width, M.Meta.MyPixel_Size.Height, bpp)
        QHY.QHY.GetQHYCCDEffectiveArea(M.DB.CamHandle, EffArea.X, EffArea.Y, EffArea.Width, EffArea.Height)
        QHY.QHY.GetQHYCCDOverScanArea(M.DB.CamHandle, OverArea.X, OverArea.Y, OverArea.Width, OverArea.Height)
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
            Log("  Start X : Y  :" & EffArea.X.ValRegIndep & ":" & EffArea.Y.ValRegIndep)
            Log("  Size  W x H  :" & EffArea.Width.ValRegIndep & " x " & EffArea.Height.ValRegIndep)
            Log("CCD Overscan Area:")
            Log("  Start X : Y  :" & OverArea.X.ValRegIndep & ":" & OverArea.Y.ValRegIndep)
            Log("  Size  W x H  :" & OverArea.Width.ValRegIndep & " x " & OverArea.Height.ValRegIndep)
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
            M.Report.Prop.PlotStatisticsColor = False
            M.DB.StatColor = False
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
        Dim FilterActive As eFilter = eFilter.Invalid
        If M.DB.FilterSlot <> eFilter.Invalid And M.DB.UseFilterWheel = True Then
            FilterActive = ActiveFilter(M.DB.CamHandle, M.DB.FilterSlot, M.Meta.FilterWheelTimeOut)
        End If
        M.DB.Stopper.Stamp("Select filter")

        'Enter capture loop
        Dim EndTimeStamps As New List(Of DateTime)
        Dim TotalCaptureTime As Double = 0
        Dim RunningCaptureInfo As New cSingleCaptureInfo

        For CaptureLoopCount As UInt32 = 1 To CUInt(M.DB.CaptureCount)

            '================================================================================
            ' START EXPOSURE ON FIRST ENTRY
            '================================================================================

            If CaptureLoopCount = 1 Then
                RunningCaptureInfo = StartExposure(CaptureLoopCount, FilterActive)
            End If

            '================================================================================
            ' WAIT FOR END AND READ BUFFERS
            '================================================================================

            IdleExposureTime(CaptureLoopCount, M.DB.ExposureTime)
            If StopNow(False) Then Exit For

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
            If M.DB.StreamMode = eStreamMode.SingleFrame Then
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
            RunningCaptureInfo.ObsEnd = Now
            EndTimeStamps.Add(RunningCaptureInfo.ObsEnd)

            Dim BytesToTransfer_calculated As Long = Captured_W * Captured_H * CInt(BitPerPixel / BitsPerByte)
            LogVerbose("Calculation says       : " & BytesToTransfer_calculated.ValRegIndep.PadLeft(12) & " byte to transfer.")
            LogVerbose("Loaded image with " & Captured_W.ValRegIndep & "x" & Captured_H.ValRegIndep & " pixel @ " & BitPerPixel & " bit resolution, " & ChannelToRead.ValRegIndep & " channels")
            M.DB.Stopper.Stamp("GetQHYCCDSingleFrame (" & LiveModePollCount.ValRegIndep & " x)")

            'Remove overscan - do NOT run if an ROI is set
            Dim SingleStatCalc As New AstroNET.Statistics(M.DB.IPP)
            SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data = ChangeAspectIPP(M.DB.IPP, CamRawBuffer, CInt(Captured_W), CInt(Captured_H))      'only convert flat byte buffer to UInt16 matrix data
            If M.DB.RemoveOverscan = True And M.DB.ROISet = False Then
                Dim NoOverscan(CInt(EffArea.Width - 1), CInt(EffArea.Height - 1)) As UInt16
                Dim Status_GetROI As cIntelIPP.IppStatus = M.DB.IPP.Copy(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, NoOverscan, CInt(EffArea.X), CInt(EffArea.Y), CInt(EffArea.Width), CInt(EffArea.Height))
                Dim Status_SetData As cIntelIPP.IppStatus = M.DB.IPP.Copy(NoOverscan, SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, 0, 0, NoOverscan.GetUpperBound(0) + 1, NoOverscan.GetUpperBound(1) + 1)
                If Status_GetROI <> cIntelIPP.IppStatus.NoErr Or Status_SetData <> cIntelIPP.IppStatus.NoErr Then
                    LogError("Overscan removal FAILED")
                End If
            End If
            RunningCaptureInfo.NAXIS1 = SingleStatCalc.DataProcessor_UInt16.ImageData(0).NAXIS1
            RunningCaptureInfo.NAXIS2 = SingleStatCalc.DataProcessor_UInt16.ImageData(0).NAXIS2
            M.DB.Stopper.Stamp("ChangeAspect")

            'Software binning - if > 1 data type is moved from UInt16 to UInt32
            If M.DB.SoftwareBinning > 1 Then
                SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data = ImageProcessing.Binning(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, M.DB.SoftwareBinning)
                SingleStatCalc.Reset_UInt16()
            End If

            'Infinit stack (for focus quality estimation)
            If M.DB.StackAll = True Then
                Dim AsUInt32(,) As UInt32 = {}
                M.DB.IPP.Convert(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, AsUInt32)
                M.DB.IPP.Add(AsUInt32, InfinitBuffer)
                M.DB.IPP.Copy(InfinitBuffer, SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data)
                SingleStatCalc.Reset_UInt16()
            End If

            If M.Meta.StarSearch = True Then
                Dim ROICenter As Point = ImageProcessing.BrightStarDetect(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, M.Meta.StarSearch_Blur, M.Meta.StarSearch_Blur)
                M.DB.ROI = AdjustAndCorrectROI(ROICenter, M.Meta.StarSearch_ROI, M.Meta.StarSearch_ROI)
                CallOK("SetQHYCCDResolution", QHY.QHY.SetQHYCCDResolution(M.DB.CamHandle, CUInt(M.DB.ROI.X), CUInt(M.DB.ROI.Y), CUInt(M.DB.ROI.Width \ M.DB.HardwareBinning), CUInt(M.DB.ROI.Height \ M.DB.HardwareBinning)))
                M.Meta.StarSearch = False
            End If

            '================================================================================
            'RETRIGGER CAPTURE
            '================================================================================

            Dim LastCaptureInfo As cSingleCaptureInfo = RunningCaptureInfo
            If (CaptureLoopCount < M.DB.CaptureCount) And (M.DB.StopFlag = False) Then
                RunningCaptureInfo = StartExposure(CUInt(CaptureLoopCount + 1), FilterActive)
            End If

            '================================================================================
            'STATISTICS AND PLOTTING
            '================================================================================

            'FPS calculation
            If EndTimeStamps.Count > 2 Then
                Dim ThisDuration As Double = (EndTimeStamps(EndTimeStamps.Count - 1) - EndTimeStamps(EndTimeStamps.Count - 2)).TotalSeconds
                TotalCaptureTime += ThisDuration
                tsmiFPSIndicator.Text = Format(1 / ThisDuration, "0.0") & " FPS, mean: " & Format(CaptureLoopCount / TotalCaptureTime, "0.0") & " FPS"
            End If

            'Calculate statistics
            Dim SingleStat As New AstroNET.Statistics.sStatistics
            If (M.DB.StatMono = True) Or (M.DB.StatColor = True) Then SingleStat = SingleStatCalc.ImageStatistics(SingleStatCalc.DataFixFloat)
            SingleStat.MonoStatistics_Int.Width = LastCaptureInfo.NAXIS1 : SingleStat.MonoStatistics_Int.Height = LastCaptureInfo.NAXIS2

            LoopStat = AstroNET.Statistics.CombineStatistics(SingleStat.DataMode, SingleStat, LoopStat)
            M.DB.Stopper.Stamp("Statistics - calc")

            'Display statistics
            Dim DisplaySumStat As Boolean = False
            If M.Report.Prop.Log_ClearStat = True Then RTFGen.Clear()
            If M.DB.CaptureCount > 1 And M.Report.Prop.Log_ClearStat = True Then DisplaySumStat = True
            Dim SingleStatReport As List(Of String) = SingleStat.StatisticsReport(M.DB.StatMono, M.DB.StatColor, M.Report.Prop.BayerPatternNames)
            Dim LoopStatReport As List(Of String) = LoopStat.StatisticsReport(M.DB.StatMono, M.DB.StatColor, M.Report.Prop.BayerPatternNames)
            If IsNothing(SingleStatReport) = False Then
                RTFGen.AddEntry("Capture #" & LastCaptureInfo.CaptureIdx.ValRegIndep & " statistics:", Drawing.Color.Black, True, True)
                For Idx As Integer = 0 To SingleStatReport.Count - 1
                    Dim Line As String = SingleStatReport(Idx)
                    If DisplaySumStat = True Then Line &= "|" & LoopStatReport(Idx).Substring(AstroNET.Statistics.sSingleChannelStatistics_Int.ReportHeaderLength + 1)
                    RTFGen.AddEntry(Line, Drawing.Color.Black, True, False)
                Next Idx
                RTFGen.ForceRefresh()
                DE()
            End If
            M.DB.Stopper.Stamp("Statistics - text")

            '================================================================================
            'Plot histogram

            'Set caption
            Dim PlotTitle As New List(Of String)
            PlotTitle.Add(M.DB.UsedCameraId.ToString)
            PlotTitle.Add(RunningCaptureInfo.ExpTime.ToString.Trim & " s")
            PlotTitle.Add("Gain " & RunningCaptureInfo.Gain.ToString.Trim)
            PlotTitle.Add("Filter " & [Enum].GetName(GetType(eFilter), RunningCaptureInfo.FilterActive))
            PlotTitle.Add("Temperature " & RunningCaptureInfo.ObsStartTemp.ToString.Trim & " °C")
            M.Report.Plotter.SetCaptions(Join(PlotTitle.ToArray, ", "), "ADU value", "# of pixel")
            M.Report.Plot(M.DB.CaptureCount, SingleStat, LoopStat)

            M.DB.Stopper.Stamp("Statistics - plot")

            '================================================================================
            'Display focus image if required
            If M.DB.ShowLiveImage = True Then
                Dim NewWindowRequired As Boolean = False
                If IsNothing(FocusWindow) = True Then
                    NewWindowRequired = True
                Else
                    If FocusWindow.Hoster.IsDisposed = True Then NewWindowRequired = True
                End If
                If NewWindowRequired = True Then
                    FocusWindow = New cImgForm
                    FocusWindow.Show '("Focus Window <" & SingleStatCalc.Dimensions & ">")
                End If
                Select Case SingleStatCalc.DataMode
                    Case AstroNET.Statistics.eDataMode.UInt16
                        FocusWindow.ShowData(SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, SingleStat.MonoStatistics_Int.Min.Key, SingleStat.MonoStatistics_Int.Max.Key)
                    Case AstroNET.Statistics.eDataMode.UInt32
                        FocusWindow.ShowData(SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data, SingleStat.MonoStatistics_Int.Min.Key, SingleStat.MonoStatistics_Int.Max.Key)
                End Select
                M.DB.Stopper.Stamp("Focus window")
            End If

            '================================================================================
            'Store image

            If M.DB.StoreImage = True Then

                Dim Path As String = System.IO.Path.Combine(M.DB.StoragePath, M.Meta.GUID)
                If System.IO.Directory.Exists(Path) = False Then System.IO.Directory.CreateDirectory(Path)

                'Compose all FITS keyword entries and replace placeholders in filename ($EXP$, ...)
                Dim FileNameToWrite As String = M.DB.FileName
                Dim CustomElement As Dictionary(Of eFITSKeywords, Object) = GenerateFITSHeader(LastCaptureInfo, FileNameToWrite)

                'Generate final filename
                M.DB.LastStoredFile = MakeUnique(System.IO.Path.Combine(Path, FileNameToWrite & "." & M.DB.FITSExtension))

                'Store file and display if selected
                Select Case SingleStatCalc.DataMode
                    Case AstroNET.Statistics.eDataMode.UInt16
                        cFITSWriter.Write(M.DB.LastStoredFile, SingleStatCalc.DataProcessor_UInt16.ImageData(0).Data, cFITSWriter.eBitPix.Int16, CustomElement)
                    Case AstroNET.Statistics.eDataMode.UInt32
                        cFITSWriter.Write(M.DB.LastStoredFile, SingleStatCalc.DataProcessor_UInt32.ImageData(0).Data, cFITSWriter.eBitPix.Int32, CustomElement)
                End Select
                If M.DB.AutoOpenImage = True Then System.Diagnostics.Process.Start(M.DB.LastStoredFile)

            End If
            M.DB.Stopper.Stamp("Store image")

            If M.DB.StopFlag = True Then Exit For

        Next CaptureLoopCount

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

    '''<summary></summary>
    '''<returns>TRUE if camera hardware is stopped and exit can be performed.</returns>
    Private Function StopNow(ByVal Force As Boolean) As Boolean
        If M.DB.StopFlag Or Force = True Then
            CallOK("CancelQHYCCDExposing", QHY.QHY.CancelQHYCCDExposing(M.DB.CamHandle))
            CallOK("CancelQHYCCDExposingAndReadout", QHY.QHY.CancelQHYCCDExposingAndReadout(M.DB.CamHandle))
            If M.DB.StreamMode = eStreamMode.LiveFrame Then CallOK("StopQHYCCDLive", QHY.QHY.StopQHYCCDLive(M.DB.CamHandle))
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
    '''<param name="CaptureLoopCount">Index of current capture running.</param>
    '''<param name="ExposureTime">Expected time for this capture.</param>
    Private Sub IdleExposureTime(ByVal CaptureLoopCount As UInt32, ByVal ExposureTime As Double)
        If ExposureTime > 0.1 Then
            Dim ExpStart As DateTime = Now
            Dim TimePassed As Double = Double.NaN
            tspbProgress.Maximum = CInt(ExposureTime * 10)
            Do
                CCDTempOK()
                System.Threading.Thread.Sleep(100)
                TimePassed = (Now - ExpStart).TotalSeconds
                If TimePassed < ExposureTime Then
                    Dim TimeToGo As Double = ExposureTime - TimePassed
                    Dim ProgBarVal As Integer = CInt(TimePassed * 10)
                    If ProgBarVal <= tspbProgress.Maximum Then tspbProgress.Value = ProgBarVal
                    Dim TimeFormat As String = CStr(IIf(ExposureTime < 10, "0.0", "0"))
                    tsslProgress.Text = Format(TimePassed, TimeFormat).Trim & "/" & ExposureTime.ToString.Trim & " seconds exposed"
                    tsslProgress.Text &= ", ETA: " & Format(CalcTotalTimeAndETA(CaptureLoopCount, ExposureTime).AddSeconds(TimeToGo))
                End If
                DE()
            Loop Until (TimePassed >= ExposureTime) Or (M.DB.StopFlag = True)
        End If
        tspbProgress.Value = 0
        tsslProgress.Text = "---"
    End Sub

    '''<summary>Calculate the total time for the running exposure and the estimated ETA.</summary>
    Private Function CalcTotalTimeAndETA(ByVal CaptureLoopCount As UInt32, ByVal ExposureTime As Double) As DateTime
        Dim TimeToGo As TimeSpan = TimeSpan.FromSeconds((M.DB.CaptureCount - CaptureLoopCount) * ExposureTime)
        Return Now.Add(TimeToGo)
    End Function

    '''<summary>Adjust the size of the requested ROI to the chip properties.</summary>
    '''<param name="NewCenter">New center of the ROI to set.</param>
    '''<param name="NewWidth">New width of the ROI.</param>
    '''<param name="NewHeight">New height of the ROI.</param>
    '''<param name="Chip_Pixel">Size of the CCD ship [pixel].</param>
    Private Function AdjustAndCorrectROI(ByVal NewCenter As Point, ByVal NewWidth As Integer, ByVal NewHeight As Integer) As System.Drawing.Rectangle
        M.DB.ROI = New Rectangle(NewCenter.X - ((NewWidth - 1) \ 2), NewCenter.Y - ((NewHeight - 1) \ 2), NewWidth, NewHeight)
        Return AdjustAndCorrectROI()
    End Function

    '''<summary>Adjust the size of the requested ROI to the chip properties and return the correct value.</summary>
    Private Function AdjustAndCorrectROI() As System.Drawing.Rectangle

        Dim ROIForCapture As New System.Drawing.Rectangle(M.DB.ROI.X, M.DB.ROI.Y, M.DB.ROI.Width, M.DB.ROI.Height)

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

    Private Function CallOK(ByVal ErrorCode As ZWO.ASICameraDll.ASI_ERROR_CODE) As Boolean
        If ErrorCode = ZWO.ASICameraDll.ASI_ERROR_CODE.ASI_SUCCESS Then
            Return True
        Else
            LogError("ZWO ASI ERROR: <" & ErrorCode.ToString.Trim & "> #####")
            Return False
        End If
    End Function

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
        Diagnostics.Process.Start(M.DB.EXEPath)
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        tsmiNewGUID_Click(Nothing, Nothing)

        'Get build data
        Dim BuildDate As String = String.Empty
        Dim AllResources As String() = System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceNames
        For Each Entry As String In AllResources
            If Entry.EndsWith(".BuildDate.txt") Then
                BuildDate = " (Build of " & (New System.IO.StreamReader(System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream(Entry)).ReadToEnd.Trim).Replace(",", ".") & ")"
                Exit For
            End If
        Next Entry

        Me.Text &= BuildDate

        'Load INI
        M.DB.INI.Load(M.DB.MyINI)

        'Load IPP
        Dim IPPLoadError As String = String.Empty
        Dim IPPPathToUse As String = cIntelIPP.SearchDLLToUse(cIntelIPP.PossiblePaths(M.DB.EXEPath).ToArray, IPPLoadError)
        If String.IsNullOrEmpty(IPPLoadError) = True Then
            M.DB.IPP = New cIntelIPP(IPPPathToUse)
            cFITSWriter.UseIPPForWriting = True
        Else
            cFITSWriter.UseIPPForWriting = False
        End If
        cFITSWriter.IPPPath = M.DB.IPP.IPPPath

        'Start WCF
        'netsh http add urlacl url=http://+:1250/ user=DESKTOP-I7\albusmw
        DB_ServiceContract = New cDB_ServiceContract(M.DB, M.Meta)
        Dim WebServicePort As String = M.DB.INI.Get("Connections", "WebInterfacePort", "1250")
        If WebServicePort <> "0" Then
            Dim WebServiceAdr As String = "http://localhost:" & WebServicePort & "/"
            M.DB.SetupWCF = New ServiceModel.Web.WebServiceHost(GetType(cDB_ServiceContract), New Uri(WebServiceAdr))
            M.DB.serviceBehavior = M.DB.SetupWCF.Description.Behaviors.Find(Of ServiceModel.Description.ServiceDebugBehavior)
            M.DB.serviceBehavior.HttpHelpPageEnabled = True
            M.DB.serviceBehavior.IncludeExceptionDetailInFaults = True
            Try
                M.DB.SetupWCF.Open()
            Catch ex As Exception
                Log("Error creating WCF interface: <" & ex.Message & ">")
            End Try
        End If

        'Other objects
        M.Report.Plotter = New cZEDGraphService(zgcMain)
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
            QHY.QHY.ControlQHYCCDTemp(M.DB.CamHandle, M.DB.Temp_Target)
        End If
    End Sub

    Private Sub tsbCapture_Click(sender As Object, e As EventArgs) Handles tsbCapture.Click
        If CType(sender, System.Windows.Forms.ToolStripButton).Enabled = True Then QHYCapture(True)
    End Sub

    Private Sub tsbStopCapture_Click(sender As Object, e As EventArgs) Handles tsbStopCapture.Click
        M.DB.StopFlag = True
    End Sub

    Private Sub AllReadoutModesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllReadoutModesToolStripMenuItem.Click
        M.DB.StopFlag = False
        For Each Mode As eReadOutMode In [Enum].GetValues(GetType(eReadOutMode))
            M.DB.ReadOutModeEnum = Mode
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
            M.DB.ExposureTime = Exposure
            RefreshProperties()
            QHYCapture(False)
            If M.DB.StopFlag = True Then Exit For
        Next Exposure
        CloseCamera()
    End Sub

    Private Sub GainVariationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GainVariationToolStripMenuItem.Click
        M.DB.StopFlag = False
        For Gain As Double = 0 To 200 Step 1
            M.DB.Gain = Gain
            For Exp As Integer = 1 To 60 Step 1
                M.DB.ExposureTime = Exp
                RefreshProperties()
                QHYCapture(False)
            Next Exp
            If M.DB.StopFlag = True Then Exit For
        Next Gain
        CloseCamera()
    End Sub

    Private Sub tsmiFile_TestWebInterface_Click(sender As Object, e As EventArgs) Handles tsmiFile_TestWebInterface.Click
        'Test call for the web interface
        System.Diagnostics.Process.Start("http://localhost:1250/GetParameterList")
    End Sub

    Private Sub DB_ServiceContract_ValueChanged() Handles DB_ServiceContract.ValueChanged
        RefreshProperties()
    End Sub

    Private Sub DB_ServiceContract_StartExposure() Handles DB_ServiceContract.StartExposure
        tsbCapture_Click(tsbCapture, Nothing)
    End Sub

    Private Sub tsmiPreset_FastLive_Click(sender As Object, e As EventArgs) Handles tsmiPreset_FastLive.Click
        Dim ROISize As Integer = 100
        With M.DB
            .StreamMode = eStreamMode.LiveFrame
            .ExposureTime = 0.01
            .Gain = 20
            .CaptureCount = Int32.MaxValue
            .StoreImage = False
            .DDR_RAM = False
            .ConfigAlways = False
            .FilterSlot = eFilter.Invalid
            .ShowLiveImage = True
            .USBTraffic = 0
            .Temp_Target = -100
            .Temp_Tolerance = 1000
            .Temp_StableTime = 0
        End With
        With M.Meta
            .Load10MicronDataAlways = False
        End With
        With M.Report.Prop
            .Log_ClearStat = True
            .PlotStatisticsColor = False
            .PlotStatisticsMono = False
            .PlotMeanStatistics = False
            .PlotSingleStatistics = False
        End With
        RefreshProperties()
    End Sub

    Private Sub FilterSelectionToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim Stopper As New cStopper
        If InitQHY(M.DB.CamToUse) = False Then
            Log("No suitable camera found!")
        Else
            'Filter test
        End If
        CloseCamera()
    End Sub

    Private Sub CenterROIToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CenterROIToolStripMenuItem.Click
        Dim ROISize As Integer = CInt(InputBox("Size:", "ROI size", "100")) : ROISize = ROISize \ 2
        If ROISize > 0 Then
            With M.DB
                .ROI = New Drawing.Rectangle((9600 \ 2) - ROISize, (6422 \ 2) - ROISize, 2 * ROISize, 2 * ROISize)
            End With
        End If
        RefreshProperties()
    End Sub

    '''<summary>Refresh all property grid displays.</summary>
    Private Sub RefreshProperties()
        pgMain.SelectedObject = M.DB
        pgMeta.SelectedObject = M.Meta
        pgPlotAndText.SelectedObject = M.Report.Prop
        DE()
    End Sub

    Private Sub TestSeriesToolStripMenuItem_Click(sender As Object, e As EventArgs)
        M.DB.StopFlag = False
        M.DB.StoreImage = True
        M.DB.AutoOpenImage = False
        M.DB.ExposureTime = 60
        For Each ReadOutMode As eReadOutMode In [Enum].GetValues(GetType(eReadOutMode))
            If ReadOutMode <> eReadOutMode.Invalid Then
                M.DB.ReadOutModeEnum = ReadOutMode
                For Each Filter As eFilter In New eFilter() {eFilter.H_alpha}
                    M.DB.FilterSlot = Filter
                    For Gain As Double = 0 To 200 Step 1
                        M.DB.Gain = Gain
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
        Dim FolderToOpen As String = System.IO.Path.Combine(M.DB.StoragePath, M.Meta.GUID)
        If System.IO.Directory.Exists(FolderToOpen) = True Then System.Diagnostics.Process.Start(FolderToOpen)
    End Sub

    Private Sub tsmiResetLoopStat_Click(sender As Object, e As EventArgs) Handles tsmiResetLoopStat.Click
        LoopStat = New AstroNET.Statistics.sStatistics
        LoopStat.Count = 0
    End Sub

    Private Sub pgMain_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles pgMain.PropertyValueChanged
        PropertyChanged = True
        pgMain.SelectedObject = M.DB
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
            Dim FileToGenerate As String = IO.Path.Combine(M.DB.StoragePath, sfdMain.FileName)
            workbook.SaveAs(FileToGenerate)

        End Using

    End Sub

    Private Sub tsmiNewGUID_Click(sender As Object, e As EventArgs) Handles tsmiNewGUID.Click
        M.Meta.GUID = Now.ForFileSystem
        RefreshProperties()
    End Sub

    Private Sub tsmiLoad10MicronData_Click(sender As Object, e As EventArgs) Handles tsmiLoad10MicronData.Click
        Load10MicronData()
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
        Log(RunXMLSequence(ofdMain.FileName, Run))
    End Sub

    Private Function TabFormat(ByVal Text As Integer) As String
        Return Text.ValRegIndep.PadLeft(10)
    End Function

    Private Sub FITSWriterWithKeywordsToolStripMenuItem_Click(sender As Object, e As EventArgs)
        cFITSWriter.WriteTestFile_Float32("C:\TEMP\FITSHeader.fits")
    End Sub

    Private Sub tStatusUpdate_Tick(sender As Object, e As EventArgs) Handles tStatusUpdate.Tick
        tsslMemory.Text = "Memory: " & Format(GetMyMemSize, "0.0") & " MByte"
    End Sub

    '''<summary>Get the memory consumption [MByte] of this EXE.</summary>
    '''<remarks>This functions needs a significant time !!!!!</remarks>
    Private Function GetMyMemSize() As Double
        Return Process.GetCurrentProcess.PrivateMemorySize64 / 1048576
    End Function

    Private Sub tsmiFile_OpenLastFile_Click(sender As Object, e As EventArgs) Handles tsmiFile_OpenLastFile.Click
        If System.IO.File.Exists(M.DB.LastStoredFile) Then Process.Start(M.DB.LastStoredFile)
    End Sub

    Private Sub SaveTransmissionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveTransmissionToolStripMenuItem.Click
        With M.DB
            .StreamMode = eStreamMode.LiveFrame
            .CaptureCount = 1000000
            .USBTraffic = 25
            .ExposureTime = 0.001
            .ShowLiveImage = False
            .StoreImage = False
            .ConfigAlways = True
        End With
        With M.Report.Prop
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
        If InitQHY(M.DB.CamToUse) = True Then
            Dim Cooling As New frmCooling
            Cooling.Show()
        Else
            Log("No suitable camera found!")
        End If
    End Sub

    Private Sub tsmiFile_SaveAllXMLParameters_Click(sender As Object, e As EventArgs) Handles tsmiFile_SaveAllXMLParameters.Click
        Dim FileOut As New List(Of String)
        FileOut.AddRange(GetAllPropertyNames(M.DB.GetType))
        FileOut.AddRange(GetAllPropertyNames(M.Meta.GetType))
        Dim XMLXMLParameterFile As String = System.IO.Path.Combine(M.DB.EXEPath, "AllXMLParameters.txt")
        System.IO.File.WriteAllLines(XMLXMLParameterFile, FileOut)
        Process.Start(XMLXMLParameterFile)
    End Sub

    Private Sub tsmiFile_CreateXML_Click(sender As Object, e As EventArgs) Handles tsmiFile_CreateXML.Click
        Dim XMLGeneration As New frmXMLGeneration
        XMLGeneration.Show()
    End Sub

    Private Sub tsmiPreset_SkipCooling_Click(sender As Object, e As EventArgs) Handles tsmiPreset_SkipCooling.Click
        With M.DB
            .Temp_Target = -100
            .Temp_Tolerance = 1000
            .Temp_StableTime = 0
        End With
        RefreshProperties()
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
                Clipboard.Clear() :
                Clipboard.SetText(PropName)
            End If
        End If
    End Sub

    Private Sub tsmiTools_AllQHYDLLs_Click(sender As Object, e As EventArgs) Handles tsmiTools_AllQHYDLLs.Click
        'Get all qhyccd.dll version and display them
        Dim AllDLLs As List(Of String) = Everything.GetSearchResult("qhyccd.dll")
        Dim DifferentDLLs As New Dictionary(Of String, String)
        Dim RTFScanReport As New Ato.cRTFGenerator : RTFScanReport.RTFInit("Courier New", 8)
        For Each DLLFile As String In AllDLLs
            Dim Hash As String = FileHash.MD5(DLLFile)
            If String.IsNullOrEmpty(Hash) = False Then
                Dim VersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(DLLFile)
                Dim VersionString As String = VersionInfo.FileVersion : If IsNothing(VersionString) = True Then VersionString = "???"
                VersionString = VersionString.Replace(",", ".").Replace(" ", String.Empty).PadRight(12)
                Dim InfoLine As String = VersionString & " of " & System.IO.File.GetCreationTime(DLLFile) & " -> " & DLLFile
                If DifferentDLLs.ContainsKey(Hash) = False Then
                    DifferentDLLs.Add(Hash, DLLFile)
                    RTFScanReport.AddEntry("*** " & InfoLine, Color.Black)
                Else
                    RTFScanReport.AddEntry("    " & InfoLine, Color.Black)
                End If
            End If
        Next DLLFile
        RTFScanReport.AddEntry(DifferentDLLs.Count.ValRegIndep & " different verions found.", Color.Black)
        'Display info
        Dim Info As New cRTFTextBox
        Info.Init("Different qhyccd.dll versions", 1200, 400)
        Info.ShowText(RTFScanReport.GetRTFText)
    End Sub

    Private Sub tsmiPreset_DevTestMWeiss_Click(sender As Object, e As EventArgs) Handles tsmiPreset_DevTestMWeiss.Click
        With M.DB
            .Temp_Target = -100
            .Temp_Tolerance = 1000
            .Temp_StableTime = 0
            .StoreImage = False
            .ExposureTypeEnum = eExposureType.Test
        End With
        With M.Meta
            .Log_CamProp = True
            .Log_Timing = True
            .Log_Verbose = True
            .Load10MicronDataAlways = False
        End With
        RefreshProperties()
    End Sub

    Private Sub tsmiPreset_NoOverhead_Click(sender As Object, e As EventArgs) Handles tsmiPreset_NoOverhead.Click
        With M.DB
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
        Process.Start(LogFile)
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
        M.DB.StatColor = False
        M.DB.Temp_Target = -20.0
        M.DB.Temp_Tolerance = 0.2
        M.DB.Temp_TimeOutAndOK = 600.0
        M.DB.ExposureTypeEnum = eExposureType.Light
        M.Meta.Load10MicronDataAlways = True
        M.Meta.ObjectName = InputBox("Object:", "Object", M.Meta.ObjectName)
        RefreshProperties()
    End Sub

    '===================================================================================================

    '''<summary>Handle data entered via a MIDI input device.</summary>
    Private Sub MIDI_Increment(Channel As Integer, Value As Integer) Handles MIDI.Increment
        Select Case Channel
            Case 1
                M.DB.Gain += Value
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
                M.DB.Gain = 0
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
        Dim FileOut As New Xml.XmlDocument
        Dim Sequence As Xml.XmlNode = FileOut.AppendChild(FileOut.CreateNode(Xml.XmlNodeType.Element, "sequence", String.Empty))
        Dim Exp As Xml.XmlNode = Sequence.AppendChild(FileOut.CreateNode(Xml.XmlNodeType.Element, "exp", String.Empty))
        PropGridToXML(Exp, M.DB)
        PropGridToXML(Exp, M.Meta)
        FileOut.Save(sfdMain.FileName)
        Process.Start(sfdMain.FileName)
    End Sub

End Class