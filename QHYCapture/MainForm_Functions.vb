﻿Option Explicit On
Option Strict On
Imports System.Reflection
Imports System.Windows.Forms

Partial Public Class MainForm
#Disable Warning CA1416 ' Validate platform compatibility

    Private Delegate Sub InvokeDelegate()

    Public Class cReflectHelp
        Public ReadOnly Type As Type = Nothing
        Public ReadOnly Properties As New List(Of String)
        Public Sub New(ByVal ElementToLoad As Object)
            Type = ElementToLoad.GetType
            Properties = GetAllPropertyNames(Type, False)
        End Sub
        '''<summary>Get a list of all available property names.</summary>
        '''<param name="TypeToReflect">Type to get parameter names from.</param>
        Public Function GetAllPropertyNames(ByVal AddDescription As Boolean) As List(Of String)
            Return GetAllPropertyNames(Type, False)
        End Function
        '''<summary>Get a list of all available property names.</summary>
        '''<param name="TypeToReflect">Type to get parameter names from.</param>
        '''<param name="AddDescription">TRUE to add description, ...</param>
        Public Shared Function GetAllPropertyNames(ByRef TypeToReflect As Type, ByVal AddDescription As Boolean) As List(Of String)
            Dim Desc_Type As Type = GetType(System.ComponentModel.DescriptionAttribute)
            Dim RetVal As New List(Of String)
            For Each SingleProperty As Reflection.PropertyInfo In TypeToReflect.GetProperties()
                Dim PropertyName As String = SingleProperty.Name
                If AddDescription = True Then
                    Dim DescriptionAtr As Attribute = SingleProperty.GetCustomAttribute(Desc_Type)
                    If IsNothing(DescriptionAtr) = False Then
                        PropertyName &= " (" & CType(DescriptionAtr, System.ComponentModel.DescriptionAttribute).Description & ")"
                    End If
                End If
                RetVal.Add(PropertyName)
            Next SingleProperty
            Return RetVal
        End Function
    End Class



    '''<summary>Execute an XML file sequence.</summary>
    '''<param name="SpecFile">File to load specifications from.</param>
    '''<param name="RunExposure">TRUE to run exposure sequence, FALSE for only load parameters.</param>
    '''<returns>List of errors during sequence execution.</returns>
    Private Function RunXMLSequence(ByVal SpecFile As String, ByVal RunExposure As Boolean) As List(Of String)
        Dim RetVal As New List(Of String)
        'Move over all exposure specifications in the file
        Dim SpecDoc As New Xml.XmlDocument
        Try
            SpecDoc.Load(SpecFile)
        Catch ex As Exception
            Return New List(Of String)({"XML error: [" & ex.Message & "]"})
        End Try
        Return RunXMLSequence(SpecDoc, RunExposure)
    End Function

    '''<summary>Execute an XML document sequence.</summary>
    '''<param name="SpecFile">XML document to load specifications from.</param>
    '''<param name="RunExposure">TRUE to run exposure sequence, FALSE for only load parameters.</param>
    '''<returns>List of errors during sequence execution.</returns>
    Private Function RunXMLSequence(ByVal SpecDoc As Xml.XmlDocument, ByVal RunExposure As Boolean) As List(Of String)
        Dim RetVal As New List(Of String)
        Dim BoolTrue As New List(Of String)({"TRUE", "YES", "1"})
        Dim BindFlagsSet As Reflection.BindingFlags = Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.SetProperty
        For Each ExpNode As Xml.XmlNode In SpecDoc.SelectNodes("/sequence/exp")
            'Load all attributes from the file
            For Each ExpAttrib As Xml.XmlAttribute In ExpNode.Attributes
                Dim PropName As String = ExpAttrib.Name
                Dim PropType As Type = Nothing
                Dim PropValue As Object = Nothing
                'Get property type and value
                Try
                    If DB_config.Properties.Contains(PropName) Then PropType = DB_config.Type.GetProperty(PropName).PropertyType
                    If DB_meta.Properties.Contains(PropName) Then PropType = DB_meta.Type.GetProperty(PropName).PropertyType
                    If DB_report.Properties.Contains(PropName) Then PropType = DB_report.Type.GetProperty(PropName).PropertyType
                    Select Case PropType
                        Case GetType(Int32)
                            PropValue = CType(ExpAttrib.Value, Int32)
                        Case GetType(UInt32)
                            PropValue = CType(ExpAttrib.Value, UInt32)
                        Case GetType(Double)
                            PropValue = Val(ExpAttrib.Value.Replace(",", "."))
                        Case GetType(String)
                            PropValue = ExpAttrib.Value
                        Case GetType(Boolean)
                            If BoolTrue.Contains(ExpAttrib.Value.ToUpper) Then PropValue = True Else PropValue = False
                        Case GetType(eExposureType)
                            Dim ParsedValue As eExposureType = eExposureType.Invalid
                            Dim Parsed As Boolean = [Enum].TryParse(Of eExposureType)(ExpAttrib.Value, ParsedValue)
                            If Parsed = True Then
                                PropValue = ParsedValue
                            Else
                                Throw New Exception("eExposureType can not be parsed")
                            End If
                        Case GetType(eReadOutMode)
                            Dim ParsedValue As eReadOutMode = eReadOutMode.Invalid
                            Dim Parsed As Boolean = [Enum].TryParse(Of eReadOutMode)(ExpAttrib.Value, ParsedValue)
                            If Parsed = True Then
                                PropValue = ParsedValue
                            Else
                                Throw New Exception("eReadOutMode can not be parsed")
                            End If
                        Case Else
                            'Dim X As Type = Type.GetType(PropTypeName)
                            PropValue = [Enum].Parse(PropType, ExpAttrib.Value)
                    End Select
                Catch ex As Exception
                    RetVal.Add("Error processing property <" & PropName & ">: " & ex.Message)
                End Try
                If IsNothing(PropValue) = False Then
                    Try
                        If DB_config.Properties.Contains(PropName) Then
                            DB_config.Type.InvokeMember(PropName, BindFlagsSet, Type.DefaultBinder, M.Config, New Object() {PropValue})
                        Else
                            If DB_meta.Properties.Contains(PropName) Then
                                DB_meta.Type.InvokeMember(PropName, BindFlagsSet, Type.DefaultBinder, M.Meta, New Object() {PropValue})
                            Else
                                If DB_report.Properties.Contains(PropName) Then
                                    DB_report.Type.InvokeMember(PropName, BindFlagsSet, Type.DefaultBinder, M.Report.Config, New Object() {PropValue})
                                Else
                                    RetVal.Add("Error processing property <" & PropName & ">: Property is not defined")
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        RetVal.Add("Failed setting property <" & PropName & ">: " & ex.Message)
                    End Try
                Else
                    RetVal.Add("Error processing property <" & PropName & ">: Not value specified")
                End If
            Next ExpAttrib
            RefreshProperties()
            'Start exposure if specified
            If M.Config.CaptureCount > 0 And RunExposure Then
                QHYCapture(M.Config.CloseCam)
            End If
            If M.DB.StopFlag = True Then Exit For
        Next ExpNode
        If RunExposure Then CloseCamera()
        Return RetVal
    End Function

    '''<summary>Get the configuration of each property.</summary>
    Private Function PropGridToXML(ByVal Node As Xml.XmlNode, ByVal DBObject As Object) As List(Of Xml.XmlNode)
        Dim RetVal As New List(Of Xml.XmlNode)
        For Each Prop As Reflection.PropertyInfo In DBObject.GetType.GetProperties()
            Dim PropName As String = Prop.Name
            Dim PropDispName As String = Prop.Name
            Dim PropVal As Object = Prop.GetValue(DBObject)
            Dim PropDesc As String = String.Empty
            Dim PropBrowsable As Boolean = True
            Dim DispNameAttr As Object() = Prop.GetCustomAttributes(GetType(System.ComponentModel.DisplayNameAttribute), True) : If DispNameAttr.Length > 0 Then PropDispName = CType(DispNameAttr(0), System.ComponentModel.DisplayNameAttribute).DisplayName.Trim
            Dim DescAttr As Object() = Prop.GetCustomAttributes(GetType(System.ComponentModel.DescriptionAttribute), True) : If DescAttr.Length > 0 Then PropDesc = CType(DescAttr(0), System.ComponentModel.DescriptionAttribute).Description
            Dim BrowsAttr As Object() = Prop.GetCustomAttributes(GetType(System.ComponentModel.BrowsableAttribute), True) : If BrowsAttr.Length > 0 Then PropBrowsable = CType(BrowsAttr(0), System.ComponentModel.BrowsableAttribute).Browsable
            'Only add attributes that are browsable
            If PropBrowsable = True And Prop.CanWrite = True Then
                Dim PropNode As Xml.XmlElement = Node.OwnerDocument.CreateElement(PropName)
                Try
                    PropNode.InnerText = CStr(PropVal)
                Catch ex As Exception
                    PropNode.InnerText = "!!!CONVERSION_ERROR!!!"
                End Try
                PropNode.Attributes.Append(Node.OwnerDocument.CreateAttribute("DisplayedName")) : PropNode.Attributes.GetNamedItem("DisplayedName").Value = PropDispName
                If PropDesc.Length > 0 Then
                    PropNode.Attributes.Append(Node.OwnerDocument.CreateAttribute("Description")) : PropNode.Attributes.GetNamedItem("Description").Value = PropDesc
                End If
                Node.AppendChild(PropNode)
            End If
        Next Prop
        Return RetVal
    End Function

    '''<summary>Start the exposure.</summary>
    Private Function StartExposure() As cSingleCaptureInfo

        Dim SingleCaptureData As New cSingleCaptureInfo

        'Set exposure parameters (first time / on property change / always if configured)
        LED_update(tsslLED_config, True)
        If (M.DB.CurrentExposureIndex = FirstCapture) Or (M.Config.ConfigAlways = True) Or PropertyChanged = True Then
            M.Config.ROI = AdjustAndCorrectROI()
            SetExpParameters()
        End If
        LED_update(tsslLED_config, False)
        M.DB.Stopper.Stamp("Set exposure parameters")

        'Cancel any running exposure
        If M.Config.StreamMode = eStreamMode.SingleFrame Then
            CallOK("CancelQHYCCDExposing", QHY.QHY.CancelQHYCCDExposing(M.DB.CamHandle))
            CallOK("CancelQHYCCDExposingAndReadout", QHY.QHY.CancelQHYCCDExposingAndReadout(M.DB.CamHandle))
            M.DB.Stopper.Stamp("Cancel exposure")
        End If

        'Ensure the temperature is set correct
        SetTemperature()

        'Load parameters from the mount
        If M.Meta.Load10MicronDataAlways = True Then Load10MicronData()
        LoadPWI4Data()

        'Load all parameter from the camera
        With SingleCaptureData
            .CaptureIdx = M.DB.CurrentExposureIndex
            .FilterActive = M.Config.Filter
            .CamReadOutMode = New Text.StringBuilder : QHY.QHY.GetQHYCCDReadModeName(M.DB.CamHandle, M.Config.ReadOutModeEnum, .CamReadOutMode)
            .ExpTime = QHY.QHY.GetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_EXPOSURE) / 1000000
            .Gain = QHY.QHY.GetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_GAIN)
            .Offset = QHY.QHY.GetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_OFFSET)
            .Brightness = QHY.QHY.GetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_BRIGHTNESS)
            .ObsStartTemp = QHY.QHY.GetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_CURTEMP)
            .TelescopeFocus = M.Meta.TelescopeFocusAsSet
        End With

        'Start exposure (single or live frame mode)
        tsslMain.Text = "Taking capture " & M.DB.CurrentExposureIndex.ValRegIndep & "/" & M.Config.CaptureCount.ValRegIndep
        LED_update(tsslLED_capture, True)
        M.DB.Stopper.Start()
        If M.Config.StreamMode = eStreamMode.SingleFrame Then
            CallOK("ExpQHYCCDSingleFrame", QHY.QHY.ExpQHYCCDSingleFrame(M.DB.CamHandle))
            M.DB.Stopper.Stamp("ExpQHYCCDSingleFrame")
        Else
            If M.DB.LiveModeInitiated = False Then
                CallOK("BeginQHYCCDLive", QHY.QHY.BeginQHYCCDLive(M.DB.CamHandle))
                M.DB.LiveModeInitiated = True
            End If
            M.DB.Stopper.Stamp("BeginQHYCCDLive")
        End If

        Return SingleCaptureData

    End Function

    '''<summary>Log all control values.</summary>
    Private Sub LogControlValues()

        'Display all properties available
        For Each CONTROL_ID As QHYCamera.QHY.CONTROL_ID In [Enum].GetValues(GetType(QHYCamera.QHY.CONTROL_ID))                          'Move over all Control ID's
            If QHY.QHY.IsQHYCCDControlAvailable(M.DB.CamHandle, CONTROL_ID) <> QHYCamera.QHY.QHYCCD_ERROR.QHYCCD_SUCCESS Then           'If control is available
                Log("  " & CONTROL_ID.ToString.Trim.PadRight(40) & ": NOT AVAILABLE")
            Else
                Dim Min As Double = Double.NaN
                Dim Max As Double = Double.NaN
                Dim Stepping As Double = Double.NaN
                Dim CurrentValue As Double = QHY.QHY.GetQHYCCDParam(M.DB.CamHandle, CONTROL_ID)
                If QHY.QHY.GetQHYCCDParamMinMaxStep(M.DB.CamHandle, CONTROL_ID, Min, Max, Stepping) = QHYCamera.QHY.QHYCCD_ERROR.QHYCCD_SUCCESS Then
                    Log("  " & CONTROL_ID.ToString.Trim.PadRight(40) & ": " & Min.ValRegIndep & " ... <" & Stepping.ValRegIndep & "> ... " & Max.ValRegIndep & ", current: " & CurrentValue.ValRegIndep)
                Else
                    Select Case CurrentValue
                        Case UInteger.MaxValue
                            Log("  " & CONTROL_ID.ToString.Trim.PadRight(40) & ": BOOL, current: TRUE")
                        Case 0
                            Log("  " & CONTROL_ID.ToString.Trim.PadRight(40) & ": BOOL, current: FALSE")
                        Case Else
                            Log("  " & CONTROL_ID.ToString.Trim.PadRight(40) & ": Discret, current: " & CurrentValue.ValRegIndep)
                    End Select
                End If
            End If
        Next CONTROL_ID

    End Sub

    Private Function IsColorCamera() As Boolean
        Dim CanQueryColorCam As QHYCamera.QHY.QHYCCD_ERROR = QHY.QHY.IsQHYCCDControlAvailable(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CAM_COLOR)
        Select Case CanQueryColorCam
            Case QHYCamera.QHY.QHYCCD_ERROR.QHYCCD_ERROR
                Return False
            Case QHYCamera.QHY.QHYCCD_ERROR.QHYCCD_ERROR_NO_DEVICE
                Return False
            Case Else
                Select Case CType(CanQueryColorCam, QHYCamera.QHY.BAYER_ID)
                    Case QHYCamera.QHY.BAYER_ID.BAYER_BG, QHYCamera.QHY.BAYER_ID.BAYER_GB, QHYCamera.QHY.BAYER_ID.BAYER_GR, QHYCamera.QHY.BAYER_ID.BAYER_RG
                        Return True
                    Case Else
                        Return False
                End Select
        End Select
    End Function

    '''<summary>Init the camera with the passed handle.</summary>
    Private Function InitQHY(ByVal CamIDToSearch As String) As Boolean

        'Init if not yet done
        If M.DB.CamHandle = IntPtr.Zero Or M.DB.UsedReadMode = eReadOutMode.Invalid Or M.DB.UsedStreamMode = eStreamMode.Invalid Then

            If CallOK(QHY.QHY.InitQHYCCDResource) = True Then                                                                 'Init DLL itself
                M.DB.Stopper.Stamp("InitQHYCCDResource")
                Dim CameraCount As UInteger = QHY.QHY.ScanQHYCCD                                                              'Scan for connected cameras
                M.DB.Stopper.Stamp("ScanQHYCCD")
                If CameraCount > 0 Then                                                                                             'If there is a camera found

                    Dim CamScanReport As New List(Of String)

                    'Get all cameras
                    CamScanReport.Add("Found " & CameraCount.ValRegIndep & " cameras:")
                    Dim AllCameras As New Dictionary(Of Integer, System.Text.StringBuilder)
                    For Idx As Integer = 0 To CInt(CameraCount - 1)
                        Dim CurrentCamID As New System.Text.StringBuilder(0)                                                        'Prepare camera ID holder
                        If CallOK(QHY.QHY.GetQHYCCDId(Idx, CurrentCamID)) = True Then                                         'Fetch camera ID
                            AllCameras.Add(Idx, CurrentCamID)
                            CamScanReport.Add("  Camera #" & (Idx + 1).ValRegIndep & ": <" & CurrentCamID.ToString & ">")
                        Else
                            CamScanReport.Add("  Camera #" & (Idx + 1).ValRegIndep & ": <<?????>>")
                        End If
                    Next Idx

                    'Find correct camera
                    M.DB.UsedCameraId = New System.Text.StringBuilder
                    For Each CamIdx As Integer In AllCameras.Keys
                        If AllCameras(CamIdx).ToString.Contains(CamIDToSearch) = True Or CamIDToSearch = "*" Then
                            M.DB.UsedCameraId = New System.Text.StringBuilder(AllCameras(CamIdx).ToString)
                            Exit For
                        End If
                    Next CamIdx

                    'Exit if camera is not correct
                    If M.DB.UsedCameraId.Length = 0 Then
                        Log(CamScanReport)
                        Return False
                    Else
                        LogVerbose(CamScanReport)
                    End If

                    'Open found camera
                    LogVerbose("Found QHY camera to use: <" & M.DB.UsedCameraId.ToString & ">")                                       'Display fetched camera ID
                    M.DB.CamHandle = QHY.QHY.OpenQHYCCD(M.DB.UsedCameraId)                                                        'Open the camera
                    If M.DB.CamHandle <> IntPtr.Zero Then

                        'Stop any running exposures
                        QHY.QHY.CancelQHYCCDExposingAndReadout(M.DB.CamHandle)

                        'Get all supported read-out modes
                        Dim ReadoutModesCount As UInteger = 0
                        CallOK(QHY.QHY.GetQHYCCDNumberOfReadModes(M.DB.CamHandle, ReadoutModesCount))
                        Dim AllReadOutModes As New List(Of String)
                        For ReadoutMode As UInteger = 0 To CUInt(ReadoutModesCount - 1)
                            Dim ReadoutModeName As New Text.StringBuilder
                            Dim ResX As UInteger = 0 : Dim ResY As UInteger = 0
                            CallOK(QHY.QHY.GetQHYCCDReadModeName(M.DB.CamHandle, ReadoutMode, ReadoutModeName))
                            CallOK(QHY.QHY.GetQHYCCDReadModeResolution(M.DB.CamHandle, ReadoutMode, ResX, ResY))
                            AllReadOutModes.Add(ReadoutMode.ValRegIndep & ": " & ReadoutModeName.ToString & " (" & ResX.ValRegIndep & "x" & ResY.ValRegIndep & ")")
                        Next ReadoutMode

                        If M.Meta.Log_CamProp Then
                            Log("Available read-out modes:")
                            Log(AllReadOutModes)
                        End If

                        'Run the start-up init sequence
                        Log("Init QHY camera  <" & M.DB.UsedCameraId.ToString & "> ...")
                        If CallOK(QHY.QHY.SetQHYCCDReadMode(M.DB.CamHandle, M.Config.ReadOutModeEnum)) = True Then
                            If CallOK(QHY.QHY.SetQHYCCDStreamMode(M.DB.CamHandle, M.Config.StreamMode)) = True Then                   'Set single capture mode
                                If CallOK(QHY.QHY.InitQHYCCD(M.DB.CamHandle)) = True Then                                       'Init the camera with the selected mode, ...
                                    'Camera was opened correct
                                    M.DB.UsedReadMode = M.Config.ReadOutModeEnum
                                    M.DB.UsedStreamMode = M.Config.StreamMode
                                    M.DB.LiveModeInitiated = False
                                Else
                                    LogError("InitQHYCCD FAILED!")
                                    M.DB.CamHandle = IntPtr.Zero
                                End If
                            Else
                                LogError("SetQHYCCDStreamMode FAILED!")
                                M.DB.CamHandle = IntPtr.Zero
                            End If
                        Else
                            LogError("SetQHYCCDReadMode to <" & M.Config.ReadOutModeEnum & "> FAILED!")
                        End If
                    Else
                        LogError("OpenQHYCCD FAILED!")
                        M.DB.CamHandle = IntPtr.Zero
                        Return False
                    End If
                Else
                    LogError("Init DLL OK but no camera found!")
                    M.DB.CamHandle = IntPtr.Zero
                    Return False
                End If
            Else
                LogError("Init QHY did fail!")
            End If
        End If

        'Everything OK
        M.DB.Stopper.Stamp("InitQHY")
        Return True

    End Function

    '''<summary>Returns true if reasonable temperature settings are configured.</summary>
    Private Function TargetTempResonable() As Boolean
        If M.Config.Temp_Target <= -50.0 Then Return False
        If M.Config.Temp_Target >= 50.0 Then Return False
        If M.Config.Temp_Tolerance >= 100.0 Then Return False
        Return True
    End Function

    '''<summary>Set the requested temperature.</summary>
    Private Sub SetTemperature()
        If TargetTempResonable() = False Then Exit Sub
        LED_update(tsslLED_cooling, True)
        Dim TimeOutT As New Diagnostics.Stopwatch : TimeOutT.Reset() : TimeOutT.Start()
        If M.Config.Temp_Target > -50 Then
            Do
                Dim CurrentTemp As Double = ReadTemperature()
                TempConfig.SetTemp(CurrentTemp, M.Config.Temp_Target, M.Config.Temp_Tolerance)
                If TempConfig.InRangeSeconds > -1 Then tsslTemperature.Text &= ", " & TempConfig.InRangeSeconds.ValRegIndep("0.0") & " s within tolerance"
                System.Threading.Thread.Sleep(500)
                DE()
            Loop Until (TimeOutT.ElapsedMilliseconds > M.Config.Temp_TimeOutAndOK * 1000) Or (M.DB.StopFlag = True) Or TempConfig.InRangeSeconds >= M.Config.Temp_StableTime
        End If
        M.DB.Stopper.Stamp("Set temperature")
        LED_update(tsslLED_cooling, False)
    End Sub

    '''<summary>Get and display temperature and cooler power from the QHY CCD.</summary>
    Private Function ReadTemperature() As Double
        Dim RetVal As Double = QHY.QHY.GetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_CURTEMP)
        Dim CurrentPWM As Double = QHY.QHY.GetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_CURPWM)
        tsslTemperature.Text = "T = " & RetVal.ValRegIndep & " °C (-> " & M.Config.Temp_Target.ValRegIndep & " °C, cooler @ " & CurrentPWM.ValRegIndep & " %)"
        If System.Math.Abs(RetVal - M.Config.Temp_Target) <= M.Config.Temp_Tolerance Then
            tsslTemperature.BackColor = Color.Green
        Else
            tsslTemperature.BackColor = Color.Red
        End If
        Return RetVal
    End Function

    ''<summary>Set the exposure parameters</summary>
    Private Sub SetExpParameters()
        CallOK("SetQHYCCDBinMode", QHY.QHY.SetQHYCCDBinMode(M.DB.CamHandle, CUInt(M.Config.HardwareBinning), CUInt(M.Config.HardwareBinning)))
        CallOK("SetQHYCCDResolution", QHY.QHY.SetQHYCCDResolution(M.DB.CamHandle, CUInt(M.Config.ROI.X), CUInt(M.Config.ROI.Y), CUInt(M.Config.ROI.Width \ M.Config.HardwareBinning), CUInt(M.Config.ROI.Height \ M.Config.HardwareBinning)))
        CallOK("CONTROL_TRANSFERBIT", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_TRANSFERBIT, M.Config.ReadResolution))
        CallOK("CONTROL_GAIN", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_GAIN, M.Config.Gain))
        CallOK("CONTROL_OFFSET", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_OFFSET, M.Config.Offset))
        CallOK("CONTROL_USBTRAFFIC", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_USBTRAFFIC, M.Config.USBTraffic))
        CallOK("CONTROL_DDR", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_DDR, CInt(IIf(M.Config.DDR_RAM = True, 1.0, 0.0))))
        CallOK("CONTROL_EXPOSURE", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_EXPOSURE, M.Config.ExposureTime * 1000000))
        CallOK("CONTROL_BRIGHTNESS", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_BRIGHTNESS, M.Meta.Brightness))
        CallOK("CONTROL_CONTRAST", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_CONTRAST, M.Meta.Contrast))
        CallOK("CONTROL_WBR", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_WBR, M.Meta.WhiteBalance_Red))
        CallOK("CONTROL_WBG", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_WBG, M.Meta.WhiteBalance_Green))
        CallOK("CONTROL_WBB", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_WBB, M.Meta.WhiteBalance_Blue))
        CallOK("CONTROL_GAMMA", QHY.QHY.SetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_GAMMA, M.Meta.Gamma))
        PropertyChanged = False
    End Sub

    '''<summary>Close the camera connection.</summary>
    Private Sub CloseCamera()
        If M.DB.CamHandle <> IntPtr.Zero Then
            Log("Closing camera ...")
            QHY.QHY.CancelQHYCCDExposingAndReadout(M.DB.CamHandle)
            QHY.QHY.CloseQHYCCD(M.DB.CamHandle)
            QHY.QHY.ReleaseQHYCCDResource()
            M.DB.CamHandle = IntPtr.Zero
        End If
        LED_update(tsslLED_cooling, False)
        LED_update(tsslLED_capture, False)
        LED_update(tsslLED_reading, False)
    End Sub

    '''<summary>Load the data from the 10Micron mount.</summary>
    Private Sub Load10MicronData()
        Try
            Dim Client10Micron As New Net.Sockets.TcpClient
            If Client10Micron.ConnectAsync(M.Meta.IP_10Micron, M.Meta.IP_10Micron_Port).Wait(2000) = True Then
                Dim Stream10Micron As Net.Sockets.NetworkStream = Client10Micron.GetStream
                c10Micron.SendCommand(Stream10Micron, c10Micron.SetCommand.SetUltraHighPrecision)
                M.Meta.SiteLatitude = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.SiteLatitude)
                M.Meta.SiteLongitude = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.SiteLongitude)
                M.Meta.Tel_RA = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeRightAscension).ParseRA * (360 / 24)
                M.Meta.Tel_DEC = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeDeclination).ParseDegree
                M.Meta.TelescopeAltitude = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeAltitude)
                M.Meta.TelescopeAzimuth = c10Micron.GetAnswer(Stream10Micron, c10Micron.GetCommand.TelescopeAzimuth)
                RefreshProperties()
            Else
                LogError("Could not connect to 10Micro @ <" & M.Meta.IP_10Micron & ":" & M.Meta.IP_10Micron_Port.ValRegIndep & " within " & M.Meta.IP_10Micron_TimeOut.ValRegIndep & " seconds")
            End If
        Catch ex As Exception
            LogError("Could not load 10Micro data: <" & ex.Message & ">")
        End Try
    End Sub

    '''<summary>Calculate all entries from the FITS header.</summary>
    '''<param name="SingleCaptureData">Capture configuration.</param>
    '''<param name="FileNameToWrite">File name with replacement parameters to use.</param>
    Private Function GenerateFITSHeader(ByVal SingleCaptureData As cSingleCaptureInfo, ByRef FileNameToWrite As String) As Dictionary(Of eFITSKeywords, Object)

        Dim CustomElement As New Dictionary(Of eFITSKeywords, Object)

        'Precalculation
        Dim PLATESZ1 As Double = (M.Meta.Pixel_Size.Width * SingleCaptureData.NAXIS1) / 1000                           '[mm]
        Dim PLATESZ2 As Double = (M.Meta.Pixel_Size.Height * SingleCaptureData.NAXIS2) / 1000                          '[mm]
        Dim FOV1 As Double = 2 * Math.Atan(PLATESZ1 / (2 * M.Meta.TelescopeFocalLength)) * (180 / Math.PI)
        Dim FOV2 As Double = 2 * Math.Atan(PLATESZ2 / (2 * M.Meta.TelescopeFocalLength)) * (180 / Math.PI)
        Dim FilterName As String = M.Config.FilterWheelHelper.FilterNameLong(SingleCaptureData.FilterActive)

        CustomElement.Add(eFITSKeywords.OBS_ID, (M.Meta.GUID))

        'Object and telescope pointing data
        CustomElement.Add(eFITSKeywords.OBJECT, M.Meta.ObjectName)
        CustomElement.Add(eFITSKeywords.RA_NOM, M.Meta.Tel_RAString)
        CustomElement.Add(eFITSKeywords.DEC_NOM, M.Meta.Tel_DECString)
        CustomElement.Add(eFITSKeywords.RA, M.Meta.Tel_RA)
        CustomElement.Add(eFITSKeywords.DEC, M.Meta.Tel_DEC)
        CustomElement.Add(eFITSKeywords.ALTITUDE, M.Meta.TelescopeAltitude)
        CustomElement.Add(eFITSKeywords.AZIMUTH, M.Meta.TelescopeAzimuth)

        'Origin (person and site) information
        CustomElement.Add(eFITSKeywords.AUTHOR, M.Meta.Author)
        CustomElement.Add(eFITSKeywords.ORIGIN, M.Meta.Origin)
        CustomElement.Add(eFITSKeywords.SITELAT, M.Meta.SiteLatitude)
        CustomElement.Add(eFITSKeywords.SITELONG, M.Meta.SiteLongitude)
        CustomElement.Add(eFITSKeywords.PROGRAM, Me.Text)

        'Telescope and camera properties
        CustomElement.Add(eFITSKeywords.TELESCOP, M.Meta.Telescope)
        CustomElement.Add(eFITSKeywords.TELAPER, M.Meta.TelescopeAperture / 1000.0)
        CustomElement.Add(eFITSKeywords.TELFOC, M.Meta.TelescopeFocalLength / 1000.0)
        CustomElement.Add(eFITSKeywords.INSTRUME, M.DB.UsedCameraId.ToString)
        CustomElement.Add(eFITSKeywords.PIXSIZE1, M.Meta.Pixel_Size.Width)
        CustomElement.Add(eFITSKeywords.PIXSIZE2, M.Meta.Pixel_Size.Height)
        CustomElement.Add(eFITSKeywords.PLATESZ1, PLATESZ1 / 10)                                                    'calculated from the image data as ROI may be set ...
        CustomElement.Add(eFITSKeywords.PLATESZ2, PLATESZ2 / 10)                                                    'calculated from the image data as ROI may be set ...
        CustomElement.Add(eFITSKeywords.FOV1, FOV1)
        CustomElement.Add(eFITSKeywords.FOV2, FOV2)
        CustomElement.Add(eFITSKeywords.COLORTYP, "0")                                                           '<- check
        CustomElement.Add(eFITSKeywords.FILTER, FilterName)

        CustomElement.Add(eFITSKeywords.DATE_OBS, SingleCaptureData.ObsStart.Date)
        CustomElement.Add(eFITSKeywords.DATE_END, SingleCaptureData.ObsEnd.Date)
        CustomElement.Add(eFITSKeywords.TIME_OBS, SingleCaptureData.ObsStart.TimeOfDay)
        CustomElement.Add(eFITSKeywords.TIME_END, SingleCaptureData.ObsEnd.TimeOfDay)

        CustomElement.Add(eFITSKeywords.CRPIX1, 0.5 * (SingleCaptureData.NAXIS1 + 1))
        CustomElement.Add(eFITSKeywords.CRPIX2, 0.5 * (SingleCaptureData.NAXIS2 + 1))

        CustomElement.Add(eFITSKeywords.IMAGETYP, M.Config.ExposureTypeString)
        CustomElement.Add(eFITSKeywords.EXPTIME, SingleCaptureData.ExpTime)
        CustomElement.Add(eFITSKeywords.GAIN, SingleCaptureData.Gain)
        CustomElement.Add(eFITSKeywords.OFFSET, SingleCaptureData.Offset)
        CustomElement.Add(eFITSKeywords.BRIGHTNESS, SingleCaptureData.Brightness)
        CustomElement.Add(eFITSKeywords.SET_TEMP, M.Config.Temp_Target)
        CustomElement.Add(eFITSKeywords.CCD_TEMP, SingleCaptureData.ObsStartTemp)
        CustomElement.Add(eFITSKeywords.FOCUS, SingleCaptureData.TelescopeFocus)

        CustomElement.Add(eFITSKeywords.QHY_MODE, SingleCaptureData.CamReadOutMode.ToString)

        'Create FITS file name
        FileNameToWrite = FileNameToWrite.Replace("$IDX$", Format(SingleCaptureData.CaptureIdx, "000"))
        FileNameToWrite = FileNameToWrite.Replace("$CNT$", Format(M.Config.CaptureCount, "000"))
        FileNameToWrite = FileNameToWrite.Replace("$EXP$", SingleCaptureData.ExpTime.ValRegIndep("000"))
        FileNameToWrite = FileNameToWrite.Replace("$GAIN$", SingleCaptureData.Gain.ValRegIndep("000"))
        FileNameToWrite = FileNameToWrite.Replace("$OFFS$", SingleCaptureData.Offset.ValRegIndep("000"))
        FileNameToWrite = FileNameToWrite.Replace("$FILT$", FilterName)
        FileNameToWrite = FileNameToWrite.Replace("$RMODE$", [Enum].GetName(GetType(eReadOutMode), M.Config.ReadOutModeEnum))

        Return CustomElement

    End Function

    '''<summary>Add a certain FITS header card.</summary>
    Private Sub AddFITSHeaderCard(ByRef Container As List(Of String()), ByVal Keyword As eFITSKeywords, ByVal VAlue As String)
        If String.IsNullOrEmpty(VAlue) = False Then
            Dim FITSKey As New cFITSKey
            Container.Add(New String() {FITSKey(Keyword)(0), VAlue, FITSKey.Comment(Keyword)})
        End If
    End Sub

    '''<summary>Active or deactive the capture LED.</summary>
    Private Sub LED_update(ByRef LED As ToolStripStatusLabel, ByVal Status As Boolean)
        tsslLED_init.Enabled = False : tsslLED_init.BackColor = System.Drawing.SystemColors.Control
        tsslLED_config.Enabled = False : tsslLED_config.BackColor = System.Drawing.SystemColors.Control
        tsslLED_cooling.Enabled = False : tsslLED_cooling.BackColor = System.Drawing.SystemColors.Control
        tsslLED_capture.Enabled = False : tsslLED_capture.BackColor = System.Drawing.SystemColors.Control
        tsslLED_reading.Enabled = False : tsslLED_reading.BackColor = System.Drawing.SystemColors.Control
        LED.Enabled = Status
        LED.BackColor = CType(IIf(Status, Color.Red, System.Drawing.SystemColors.Control), Color)
        LED.Invalidate()
        ssMain.Update()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub InvokeMethod()
        tsslLED_capture.Invalidate()
        ssMain.Update()
        System.Windows.Forms.Application.DoEvents()
    End Sub

#Enable Warning CA1416 ' Validate platform compatibility
End Class
