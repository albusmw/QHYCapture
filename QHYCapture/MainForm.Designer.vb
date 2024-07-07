<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        pgMain = New PropertyGrid()
        ssMain = New StatusStrip()
        tsslLED_init = New ToolStripStatusLabel()
        tsslLED_config = New ToolStripStatusLabel()
        tsslLED_cooling = New ToolStripStatusLabel()
        tsslLED_capture = New ToolStripStatusLabel()
        tsslLED_reading = New ToolStripStatusLabel()
        tspbProgress = New ToolStripProgressBar()
        tsslProgress = New ToolStripStatusLabel()
        tsslSplit1 = New ToolStripStatusLabel()
        tsslMain = New ToolStripStatusLabel()
        tsslSplit2 = New ToolStripStatusLabel()
        tsslTemperature = New ToolStripStatusLabel()
        tsslMemory = New ToolStripStatusLabel()
        ToolStripStatusLabel1 = New ToolStripStatusLabel()
        tsslETA = New ToolStripStatusLabel()
        msMain = New MenuStrip()
        tsmiFile = New ToolStripMenuItem()
        tsmiFile_LoadSettings = New ToolStripMenuItem()
        tsmiFile_RunSequence = New ToolStripMenuItem()
        ToolStripMenuItem6 = New ToolStripSeparator()
        tsmiFile_SaveSettings = New ToolStripMenuItem()
        tsmiFile_SaveAllXMLParameters = New ToolStripMenuItem()
        ToolStripMenuItem4 = New ToolStripSeparator()
        tsmiFile_ExploreHere = New ToolStripMenuItem()
        tsmiFile_ExploreCampaign = New ToolStripMenuItem()
        tsmiFile_OpenLastFile = New ToolStripMenuItem()
        ToolStripMenuItem1 = New ToolStripSeparator()
        tsmiFile_CreateXML = New ToolStripMenuItem()
        tsmiFile_TestWebInterface = New ToolStripMenuItem()
        tsmiFile_StoreEXCELStat = New ToolStripMenuItem()
        ToolStripMenuItem5 = New ToolStripSeparator()
        tsmiFile_Exit = New ToolStripMenuItem()
        CaptureToolStripMenuItem = New ToolStripMenuItem()
        SeriesToolStripMenuItem = New ToolStripMenuItem()
        AllReadoutModesToolStripMenuItem = New ToolStripMenuItem()
        ExposureTimeSeriesToolStripMenuItem = New ToolStripMenuItem()
        GainVariationToolStripMenuItem = New ToolStripMenuItem()
        tsmiPreset = New ToolStripMenuItem()
        tsmiPreset_StandardCapture = New ToolStripMenuItem()
        ToolStripMenuItem3 = New ToolStripSeparator()
        tsmiPreset_FastLive = New ToolStripMenuItem()
        tsmiPreset_CenterROI = New ToolStripMenuItem()
        tsmiPreset_SaveTransmission = New ToolStripMenuItem()
        tsmiPreset_SkipCooling = New ToolStripMenuItem()
        tsmiPreset_DevTestMWeiss = New ToolStripMenuItem()
        tsmiPreset_NoOverhead = New ToolStripMenuItem()
        tsmiPreset_DSC = New ToolStripMenuItem()
        tsmiActions = New ToolStripMenuItem()
        tsmiActions_ResetLoopStat = New ToolStripMenuItem()
        tsmiNewGUID = New ToolStripMenuItem()
        tsmiClearLog = New ToolStripMenuItem()
        ToolStripMenuItem2 = New ToolStripSeparator()
        tsmiActions_Mount = New ToolStripMenuItem()
        tsmiActions_Mount_10Micron = New ToolStripMenuItem()
        tsmiActions_Mount_PWI4 = New ToolStripMenuItem()
        tsmiActions_AllCoolersOff = New ToolStripMenuItem()
        tsmiTools = New ToolStripMenuItem()
        tsmiTools_AllQHYDLLs = New ToolStripMenuItem()
        tsmiTools_Log = New ToolStripMenuItem()
        tsmiTools_Log_Store = New ToolStripMenuItem()
        tsmiTools_Log_Clear = New ToolStripMenuItem()
        zgcMain = New ZedGraph.ZedGraphControl()
        tSetTemp = New Timer(components)
        tsMain = New ToolStrip()
        tsbCapture = New ToolStripButton()
        tsbStopCapture = New ToolStripButton()
        ToolStripSeparator1 = New ToolStripSeparator()
        tsbCooling = New ToolStripButton()
        ilMain = New ImageList(components)
        tbLogOutput = New TextBox()
        tcMain = New TabControl()
        TabPage1 = New TabPage()
        TabPage2 = New TabPage()
        pgMeta = New PropertyGrid()
        TabPage3 = New TabPage()
        pgPlotAndText = New PropertyGrid()
        scMain = New SplitContainer()
        SplitContainer2 = New SplitContainer()
        tbItemName = New TextBox()
        SplitContainer3 = New SplitContainer()
        rtbStatistics = New RichTextBox()
        sfdMain = New SaveFileDialog()
        ofdMain = New OpenFileDialog()
        tStatusUpdate = New Timer(components)
        tsmiPreset_Test = New ToolStripMenuItem()
        tsmiPreset_Test_JustOnce = New ToolStripMenuItem()
        ssMain.SuspendLayout()
        msMain.SuspendLayout()
        tsMain.SuspendLayout()
        tcMain.SuspendLayout()
        TabPage1.SuspendLayout()
        TabPage2.SuspendLayout()
        TabPage3.SuspendLayout()
        CType(scMain, ComponentModel.ISupportInitialize).BeginInit()
        scMain.Panel1.SuspendLayout()
        scMain.Panel2.SuspendLayout()
        scMain.SuspendLayout()
        CType(SplitContainer2, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer2.Panel1.SuspendLayout()
        SplitContainer2.Panel2.SuspendLayout()
        SplitContainer2.SuspendLayout()
        CType(SplitContainer3, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer3.Panel1.SuspendLayout()
        SplitContainer3.Panel2.SuspendLayout()
        SplitContainer3.SuspendLayout()
        SuspendLayout()
        ' 
        ' pgMain
        ' 
        pgMain.Dock = DockStyle.Fill
        pgMain.Font = New Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        pgMain.Location = New Point(4, 3)
        pgMain.Margin = New Padding(4, 3, 4, 3)
        pgMain.Name = "pgMain"
        pgMain.Size = New Size(559, 734)
        pgMain.TabIndex = 0
        pgMain.ToolbarVisible = False
        ' 
        ' ssMain
        ' 
        ssMain.Items.AddRange(New ToolStripItem() {tsslLED_init, tsslLED_config, tsslLED_cooling, tsslLED_capture, tsslLED_reading, tspbProgress, tsslProgress, tsslSplit1, tsslMain, tsslSplit2, tsslTemperature, tsslMemory, ToolStripStatusLabel1, tsslETA})
        ssMain.Location = New Point(0, 1091)
        ssMain.Name = "ssMain"
        ssMain.Padding = New Padding(1, 0, 16, 0)
        ssMain.Size = New Size(1480, 27)
        ssMain.TabIndex = 1
        ssMain.Text = "StatusStrip1"
        ' 
        ' tsslLED_init
        ' 
        tsslLED_init.BorderSides = ToolStripStatusLabelBorderSides.Left Or ToolStripStatusLabelBorderSides.Top Or ToolStripStatusLabelBorderSides.Right Or ToolStripStatusLabelBorderSides.Bottom
        tsslLED_init.Enabled = False
        tsslLED_init.Name = "tsslLED_init"
        tsslLED_init.Size = New Size(28, 22)
        tsslLED_init.Text = "Init"
        ' 
        ' tsslLED_config
        ' 
        tsslLED_config.BorderSides = ToolStripStatusLabelBorderSides.Left Or ToolStripStatusLabelBorderSides.Top Or ToolStripStatusLabelBorderSides.Right Or ToolStripStatusLabelBorderSides.Bottom
        tsslLED_config.Enabled = False
        tsslLED_config.Name = "tsslLED_config"
        tsslLED_config.Size = New Size(47, 22)
        tsslLED_config.Text = "Config"
        ' 
        ' tsslLED_cooling
        ' 
        tsslLED_cooling.BorderSides = ToolStripStatusLabelBorderSides.Left Or ToolStripStatusLabelBorderSides.Top Or ToolStripStatusLabelBorderSides.Right Or ToolStripStatusLabelBorderSides.Bottom
        tsslLED_cooling.Enabled = False
        tsslLED_cooling.Name = "tsslLED_cooling"
        tsslLED_cooling.Size = New Size(53, 22)
        tsslLED_cooling.Text = "Cooling"
        ' 
        ' tsslLED_capture
        ' 
        tsslLED_capture.BorderSides = ToolStripStatusLabelBorderSides.Left Or ToolStripStatusLabelBorderSides.Top Or ToolStripStatusLabelBorderSides.Right Or ToolStripStatusLabelBorderSides.Bottom
        tsslLED_capture.Enabled = False
        tsslLED_capture.Name = "tsslLED_capture"
        tsslLED_capture.Size = New Size(53, 22)
        tsslLED_capture.Text = "Capture"
        ' 
        ' tsslLED_reading
        ' 
        tsslLED_reading.BorderSides = ToolStripStatusLabelBorderSides.Left Or ToolStripStatusLabelBorderSides.Top Or ToolStripStatusLabelBorderSides.Right Or ToolStripStatusLabelBorderSides.Bottom
        tsslLED_reading.Enabled = False
        tsslLED_reading.Name = "tsslLED_reading"
        tsslLED_reading.Size = New Size(63, 22)
        tsslLED_reading.Text = "Read data"
        ' 
        ' tspbProgress
        ' 
        tspbProgress.ForeColor = Color.Lime
        tspbProgress.Name = "tspbProgress"
        tspbProgress.Size = New Size(350, 21)
        tspbProgress.Style = ProgressBarStyle.Continuous
        ' 
        ' tsslProgress
        ' 
        tsslProgress.Name = "tsslProgress"
        tsslProgress.Size = New Size(156, 22)
        tsslProgress.Text = "-- not exposing right now --"
        ' 
        ' tsslSplit1
        ' 
        tsslSplit1.Name = "tsslSplit1"
        tsslSplit1.Size = New Size(10, 22)
        tsslSplit1.Text = "|"
        ' 
        ' tsslMain
        ' 
        tsslMain.Name = "tsslMain"
        tsslMain.Size = New Size(50, 22)
        tsslMain.Text = "--IDLE--"
        ' 
        ' tsslSplit2
        ' 
        tsslSplit2.Name = "tsslSplit2"
        tsslSplit2.Size = New Size(10, 22)
        tsslSplit2.Text = "|"
        ' 
        ' tsslTemperature
        ' 
        tsslTemperature.Name = "tsslTemperature"
        tsslTemperature.Size = New Size(58, 22)
        tsslTemperature.Text = "T = ??? °C"
        ' 
        ' tsslMemory
        ' 
        tsslMemory.Name = "tsslMemory"
        tsslMemory.Size = New Size(73, 22)
        tsslMemory.Text = "Memory: ???"
        ' 
        ' ToolStripStatusLabel1
        ' 
        ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        ToolStripStatusLabel1.Size = New Size(10, 22)
        ToolStripStatusLabel1.Text = "|"
        ' 
        ' tsslETA
        ' 
        tsslETA.Name = "tsslETA"
        tsslETA.Size = New Size(47, 22)
        tsslETA.Text = "ETA: ???"
        ' 
        ' msMain
        ' 
        msMain.Items.AddRange(New ToolStripItem() {tsmiFile, CaptureToolStripMenuItem, tsmiPreset, tsmiActions, tsmiTools})
        msMain.Location = New Point(0, 0)
        msMain.Name = "msMain"
        msMain.Padding = New Padding(7, 2, 0, 2)
        msMain.Size = New Size(1480, 24)
        msMain.TabIndex = 2
        msMain.Text = "MenuStrip1"
        ' 
        ' tsmiFile
        ' 
        tsmiFile.DropDownItems.AddRange(New ToolStripItem() {tsmiFile_LoadSettings, tsmiFile_RunSequence, ToolStripMenuItem6, tsmiFile_SaveSettings, tsmiFile_SaveAllXMLParameters, ToolStripMenuItem4, tsmiFile_ExploreHere, tsmiFile_ExploreCampaign, tsmiFile_OpenLastFile, ToolStripMenuItem1, tsmiFile_CreateXML, tsmiFile_TestWebInterface, tsmiFile_StoreEXCELStat, ToolStripMenuItem5, tsmiFile_Exit})
        tsmiFile.Name = "tsmiFile"
        tsmiFile.Size = New Size(37, 20)
        tsmiFile.Text = "File"
        ' 
        ' tsmiFile_LoadSettings
        ' 
        tsmiFile_LoadSettings.Name = "tsmiFile_LoadSettings"
        tsmiFile_LoadSettings.ShortcutKeys = Keys.Control Or Keys.O
        tsmiFile_LoadSettings.Size = New Size(311, 22)
        tsmiFile_LoadSettings.Tag = "Load"
        tsmiFile_LoadSettings.Text = "Load XML settings (does not expose)"
        ' 
        ' tsmiFile_RunSequence
        ' 
        tsmiFile_RunSequence.Name = "tsmiFile_RunSequence"
        tsmiFile_RunSequence.Size = New Size(311, 22)
        tsmiFile_RunSequence.Tag = "Run"
        tsmiFile_RunSequence.Text = "Run XML settings (does expose)"
        ' 
        ' ToolStripMenuItem6
        ' 
        ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        ToolStripMenuItem6.Size = New Size(308, 6)
        ' 
        ' tsmiFile_SaveSettings
        ' 
        tsmiFile_SaveSettings.Name = "tsmiFile_SaveSettings"
        tsmiFile_SaveSettings.ShortcutKeys = Keys.Control Or Keys.S
        tsmiFile_SaveSettings.Size = New Size(311, 22)
        tsmiFile_SaveSettings.Text = "Save settings"
        ' 
        ' tsmiFile_SaveAllXMLParameters
        ' 
        tsmiFile_SaveAllXMLParameters.Name = "tsmiFile_SaveAllXMLParameters"
        tsmiFile_SaveAllXMLParameters.Size = New Size(311, 22)
        tsmiFile_SaveAllXMLParameters.Text = "Save all XML parameters (for reference)"
        ' 
        ' ToolStripMenuItem4
        ' 
        ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        ToolStripMenuItem4.Size = New Size(308, 6)
        ' 
        ' tsmiFile_ExploreHere
        ' 
        tsmiFile_ExploreHere.Name = "tsmiFile_ExploreHere"
        tsmiFile_ExploreHere.Size = New Size(311, 22)
        tsmiFile_ExploreHere.Text = "Open - EXE path"
        ' 
        ' tsmiFile_ExploreCampaign
        ' 
        tsmiFile_ExploreCampaign.Name = "tsmiFile_ExploreCampaign"
        tsmiFile_ExploreCampaign.ShortcutKeys = Keys.Control Or Keys.E
        tsmiFile_ExploreCampaign.Size = New Size(311, 22)
        tsmiFile_ExploreCampaign.Text = "Open - Current campaign folder"
        ' 
        ' tsmiFile_OpenLastFile
        ' 
        tsmiFile_OpenLastFile.Name = "tsmiFile_OpenLastFile"
        tsmiFile_OpenLastFile.Size = New Size(311, 22)
        tsmiFile_OpenLastFile.Text = "Open - Last stored file"
        ' 
        ' ToolStripMenuItem1
        ' 
        ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        ToolStripMenuItem1.Size = New Size(308, 6)
        ' 
        ' tsmiFile_CreateXML
        ' 
        tsmiFile_CreateXML.Name = "tsmiFile_CreateXML"
        tsmiFile_CreateXML.Size = New Size(311, 22)
        tsmiFile_CreateXML.Text = "Create XML sequence (Inline VB)"
        ' 
        ' tsmiFile_TestWebInterface
        ' 
        tsmiFile_TestWebInterface.Name = "tsmiFile_TestWebInterface"
        tsmiFile_TestWebInterface.Size = New Size(311, 22)
        tsmiFile_TestWebInterface.Text = "Test web interface"
        ' 
        ' tsmiFile_StoreEXCELStat
        ' 
        tsmiFile_StoreEXCELStat.Name = "tsmiFile_StoreEXCELStat"
        tsmiFile_StoreEXCELStat.Size = New Size(311, 22)
        tsmiFile_StoreEXCELStat.Text = "Store statistics as EXCEL file"
        ' 
        ' ToolStripMenuItem5
        ' 
        ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        ToolStripMenuItem5.Size = New Size(308, 6)
        ' 
        ' tsmiFile_Exit
        ' 
        tsmiFile_Exit.Name = "tsmiFile_Exit"
        tsmiFile_Exit.ShortcutKeys = Keys.Control Or Keys.X
        tsmiFile_Exit.Size = New Size(311, 22)
        tsmiFile_Exit.Text = "Exit"
        ' 
        ' CaptureToolStripMenuItem
        ' 
        CaptureToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {SeriesToolStripMenuItem})
        CaptureToolStripMenuItem.Name = "CaptureToolStripMenuItem"
        CaptureToolStripMenuItem.Size = New Size(61, 20)
        CaptureToolStripMenuItem.Text = "Capture"
        ' 
        ' SeriesToolStripMenuItem
        ' 
        SeriesToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {AllReadoutModesToolStripMenuItem, ExposureTimeSeriesToolStripMenuItem, GainVariationToolStripMenuItem})
        SeriesToolStripMenuItem.Name = "SeriesToolStripMenuItem"
        SeriesToolStripMenuItem.Size = New Size(104, 22)
        SeriesToolStripMenuItem.Text = "Series"
        ' 
        ' AllReadoutModesToolStripMenuItem
        ' 
        AllReadoutModesToolStripMenuItem.Name = "AllReadoutModesToolStripMenuItem"
        AllReadoutModesToolStripMenuItem.Size = New Size(181, 22)
        AllReadoutModesToolStripMenuItem.Text = "All read-out modes"
        ' 
        ' ExposureTimeSeriesToolStripMenuItem
        ' 
        ExposureTimeSeriesToolStripMenuItem.Name = "ExposureTimeSeriesToolStripMenuItem"
        ExposureTimeSeriesToolStripMenuItem.Size = New Size(181, 22)
        ExposureTimeSeriesToolStripMenuItem.Text = "Exposure time series"
        ' 
        ' GainVariationToolStripMenuItem
        ' 
        GainVariationToolStripMenuItem.Name = "GainVariationToolStripMenuItem"
        GainVariationToolStripMenuItem.Size = New Size(181, 22)
        GainVariationToolStripMenuItem.Text = "Gain variation"
        ' 
        ' tsmiPreset
        ' 
        tsmiPreset.DropDownItems.AddRange(New ToolStripItem() {tsmiPreset_StandardCapture, ToolStripMenuItem3, tsmiPreset_FastLive, tsmiPreset_CenterROI, tsmiPreset_SaveTransmission, tsmiPreset_SkipCooling, tsmiPreset_DevTestMWeiss, tsmiPreset_NoOverhead, tsmiPreset_DSC, tsmiPreset_Test})
        tsmiPreset.Name = "tsmiPreset"
        tsmiPreset.Size = New Size(56, 20)
        tsmiPreset.Text = "Presets"
        ' 
        ' tsmiPreset_StandardCapture
        ' 
        tsmiPreset_StandardCapture.Name = "tsmiPreset_StandardCapture"
        tsmiPreset_StandardCapture.Size = New Size(237, 22)
        tsmiPreset_StandardCapture.Text = "Standard capture"
        ' 
        ' ToolStripMenuItem3
        ' 
        ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        ToolStripMenuItem3.Size = New Size(234, 6)
        ' 
        ' tsmiPreset_FastLive
        ' 
        tsmiPreset_FastLive.Name = "tsmiPreset_FastLive"
        tsmiPreset_FastLive.Size = New Size(237, 22)
        tsmiPreset_FastLive.Text = "Fast live mode"
        ' 
        ' tsmiPreset_CenterROI
        ' 
        tsmiPreset_CenterROI.Name = "tsmiPreset_CenterROI"
        tsmiPreset_CenterROI.Size = New Size(237, 22)
        tsmiPreset_CenterROI.Text = "Center ROI"
        ' 
        ' tsmiPreset_SaveTransmission
        ' 
        tsmiPreset_SaveTransmission.Name = "tsmiPreset_SaveTransmission"
        tsmiPreset_SaveTransmission.Size = New Size(237, 22)
        tsmiPreset_SaveTransmission.Text = "Save transmission"
        ' 
        ' tsmiPreset_SkipCooling
        ' 
        tsmiPreset_SkipCooling.Name = "tsmiPreset_SkipCooling"
        tsmiPreset_SkipCooling.Size = New Size(237, 22)
        tsmiPreset_SkipCooling.Text = "Skip cooling (in-memory XML)"
        ' 
        ' tsmiPreset_DevTestMWeiss
        ' 
        tsmiPreset_DevTestMWeiss.Name = "tsmiPreset_DevTestMWeiss"
        tsmiPreset_DevTestMWeiss.Size = New Size(237, 22)
        tsmiPreset_DevTestMWeiss.Text = "Dev test (in-memory XML)"
        ' 
        ' tsmiPreset_NoOverhead
        ' 
        tsmiPreset_NoOverhead.Name = "tsmiPreset_NoOverhead"
        tsmiPreset_NoOverhead.Size = New Size(237, 22)
        tsmiPreset_NoOverhead.Text = "No overhead"
        ' 
        ' tsmiPreset_DSC
        ' 
        tsmiPreset_DSC.Name = "tsmiPreset_DSC"
        tsmiPreset_DSC.Size = New Size(237, 22)
        tsmiPreset_DSC.Text = "DSC (in-memory XML)"
        ' 
        ' tsmiActions
        ' 
        tsmiActions.DropDownItems.AddRange(New ToolStripItem() {tsmiActions_ResetLoopStat, tsmiNewGUID, tsmiClearLog, ToolStripMenuItem2, tsmiActions_Mount, tsmiActions_AllCoolersOff})
        tsmiActions.Name = "tsmiActions"
        tsmiActions.Size = New Size(59, 20)
        tsmiActions.Text = "Actions"
        ' 
        ' tsmiActions_ResetLoopStat
        ' 
        tsmiActions_ResetLoopStat.Name = "tsmiActions_ResetLoopStat"
        tsmiActions_ResetLoopStat.Size = New Size(177, 22)
        tsmiActions_ResetLoopStat.Text = "Reset loop statistics"
        ' 
        ' tsmiNewGUID
        ' 
        tsmiNewGUID.Name = "tsmiNewGUID"
        tsmiNewGUID.Size = New Size(177, 22)
        tsmiNewGUID.Text = "New GUID"
        ' 
        ' tsmiClearLog
        ' 
        tsmiClearLog.Name = "tsmiClearLog"
        tsmiClearLog.Size = New Size(177, 22)
        tsmiClearLog.Text = "Clear log"
        ' 
        ' ToolStripMenuItem2
        ' 
        ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        ToolStripMenuItem2.Size = New Size(174, 6)
        ' 
        ' tsmiActions_Mount
        ' 
        tsmiActions_Mount.DropDownItems.AddRange(New ToolStripItem() {tsmiActions_Mount_10Micron, tsmiActions_Mount_PWI4})
        tsmiActions_Mount.Name = "tsmiActions_Mount"
        tsmiActions_Mount.Size = New Size(177, 22)
        tsmiActions_Mount.Text = "Load mount data ..."
        ' 
        ' tsmiActions_Mount_10Micron
        ' 
        tsmiActions_Mount_10Micron.Name = "tsmiActions_Mount_10Micron"
        tsmiActions_Mount_10Micron.Size = New Size(179, 22)
        tsmiActions_Mount_10Micron.Text = "Load 10Micron data"
        ' 
        ' tsmiActions_Mount_PWI4
        ' 
        tsmiActions_Mount_PWI4.Name = "tsmiActions_Mount_PWI4"
        tsmiActions_Mount_PWI4.Size = New Size(179, 22)
        tsmiActions_Mount_PWI4.Text = "Load PWI4 data"
        ' 
        ' tsmiActions_AllCoolersOff
        ' 
        tsmiActions_AllCoolersOff.Name = "tsmiActions_AllCoolersOff"
        tsmiActions_AllCoolersOff.Size = New Size(177, 22)
        tsmiActions_AllCoolersOff.Text = "All coolers off"
        ' 
        ' tsmiTools
        ' 
        tsmiTools.DropDownItems.AddRange(New ToolStripItem() {tsmiTools_AllQHYDLLs, tsmiTools_Log})
        tsmiTools.Name = "tsmiTools"
        tsmiTools.Size = New Size(46, 20)
        tsmiTools.Text = "Tools"
        ' 
        ' tsmiTools_AllQHYDLLs
        ' 
        tsmiTools_AllQHYDLLs.Name = "tsmiTools_AllQHYDLLs"
        tsmiTools_AllQHYDLLs.Size = New Size(163, 22)
        tsmiTools_AllQHYDLLs.Text = "Get all QHY DLLs"
        ' 
        ' tsmiTools_Log
        ' 
        tsmiTools_Log.DropDownItems.AddRange(New ToolStripItem() {tsmiTools_Log_Store, tsmiTools_Log_Clear})
        tsmiTools_Log.Name = "tsmiTools_Log"
        tsmiTools_Log.Size = New Size(163, 22)
        tsmiTools_Log.Text = "DLL Log"
        ' 
        ' tsmiTools_Log_Store
        ' 
        tsmiTools_Log_Store.Name = "tsmiTools_Log_Store"
        tsmiTools_Log_Store.Size = New Size(121, 22)
        tsmiTools_Log_Store.Text = "Store log"
        ' 
        ' tsmiTools_Log_Clear
        ' 
        tsmiTools_Log_Clear.Name = "tsmiTools_Log_Clear"
        tsmiTools_Log_Clear.Size = New Size(121, 22)
        tsmiTools_Log_Clear.Text = "Clear"
        ' 
        ' zgcMain
        ' 
        zgcMain.Dock = DockStyle.Fill
        zgcMain.Location = New Point(0, 0)
        zgcMain.Margin = New Padding(5, 3, 5, 3)
        zgcMain.Name = "zgcMain"
        zgcMain.ScrollGrace = 0R
        zgcMain.ScrollMaxX = 0R
        zgcMain.ScrollMaxY = 0R
        zgcMain.ScrollMaxY2 = 0R
        zgcMain.ScrollMinX = 0R
        zgcMain.ScrollMinY = 0R
        zgcMain.ScrollMinY2 = 0R
        zgcMain.Size = New Size(900, 496)
        zgcMain.TabIndex = 0
        ' 
        ' tSetTemp
        ' 
        tSetTemp.Enabled = True
        tSetTemp.Interval = 500
        ' 
        ' tsMain
        ' 
        tsMain.BackColor = SystemColors.Control
        tsMain.Items.AddRange(New ToolStripItem() {tsbCapture, tsbStopCapture, ToolStripSeparator1, tsbCooling})
        tsMain.Location = New Point(0, 24)
        tsMain.Name = "tsMain"
        tsMain.Size = New Size(1480, 38)
        tsMain.TabIndex = 5
        tsMain.Text = "ToolStrip1"
        ' 
        ' tsbCapture
        ' 
        tsbCapture.Image = CType(resources.GetObject("tsbCapture.Image"), Image)
        tsbCapture.ImageTransparentColor = Color.Magenta
        tsbCapture.Name = "tsbCapture"
        tsbCapture.Size = New Size(53, 35)
        tsbCapture.Text = "Capture"
        tsbCapture.TextImageRelation = TextImageRelation.ImageAboveText
        ' 
        ' tsbStopCapture
        ' 
        tsbStopCapture.Image = CType(resources.GetObject("tsbStopCapture.Image"), Image)
        tsbStopCapture.ImageTransparentColor = Color.Magenta
        tsbStopCapture.Name = "tsbStopCapture"
        tsbStopCapture.Size = New Size(35, 35)
        tsbStopCapture.Text = "Stop"
        tsbStopCapture.TextImageRelation = TextImageRelation.ImageAboveText
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(6, 38)
        ' 
        ' tsbCooling
        ' 
        tsbCooling.Image = CType(resources.GetObject("tsbCooling.Image"), Image)
        tsbCooling.ImageAlign = ContentAlignment.TopCenter
        tsbCooling.ImageTransparentColor = Color.Magenta
        tsbCooling.Name = "tsbCooling"
        tsbCooling.Size = New Size(53, 35)
        tsbCooling.Text = "Cooling"
        tsbCooling.TextAlign = ContentAlignment.BottomCenter
        tsbCooling.TextImageRelation = TextImageRelation.ImageAboveText
        ' 
        ' ilMain
        ' 
        ilMain.ColorDepth = ColorDepth.Depth8Bit
        ilMain.ImageStream = CType(resources.GetObject("ilMain.ImageStream"), ImageListStreamer)
        ilMain.TransparentColor = Color.Transparent
        ilMain.Images.SetKeyName(0, "Capture.png")
        ilMain.Images.SetKeyName(1, "StopCapture.png")
        ' 
        ' tbLogOutput
        ' 
        tbLogOutput.Dock = DockStyle.Fill
        tbLogOutput.Font = New Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tbLogOutput.Location = New Point(0, 0)
        tbLogOutput.Margin = New Padding(4, 3, 4, 3)
        tbLogOutput.Multiline = True
        tbLogOutput.Name = "tbLogOutput"
        tbLogOutput.ScrollBars = ScrollBars.Both
        tbLogOutput.Size = New Size(575, 209)
        tbLogOutput.TabIndex = 4
        tbLogOutput.WordWrap = False
        ' 
        ' tcMain
        ' 
        tcMain.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        tcMain.Controls.Add(TabPage1)
        tcMain.Controls.Add(TabPage2)
        tcMain.Controls.Add(TabPage3)
        tcMain.Font = New Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tcMain.Location = New Point(0, 0)
        tcMain.Margin = New Padding(4, 3, 4, 3)
        tcMain.Name = "tcMain"
        tcMain.SelectedIndex = 0
        tcMain.Size = New Size(575, 766)
        tcMain.TabIndex = 6
        ' 
        ' TabPage1
        ' 
        TabPage1.Controls.Add(pgMain)
        TabPage1.Location = New Point(4, 22)
        TabPage1.Margin = New Padding(4, 3, 4, 3)
        TabPage1.Name = "TabPage1"
        TabPage1.Padding = New Padding(4, 3, 4, 3)
        TabPage1.Size = New Size(567, 740)
        TabPage1.TabIndex = 0
        TabPage1.Text = "Exposure"
        TabPage1.UseVisualStyleBackColor = True
        ' 
        ' TabPage2
        ' 
        TabPage2.Controls.Add(pgMeta)
        TabPage2.Location = New Point(4, 22)
        TabPage2.Margin = New Padding(4, 3, 4, 3)
        TabPage2.Name = "TabPage2"
        TabPage2.Padding = New Padding(4, 3, 4, 3)
        TabPage2.Size = New Size(567, 740)
        TabPage2.TabIndex = 1
        TabPage2.Text = "Meta data / Advanced"
        TabPage2.UseVisualStyleBackColor = True
        ' 
        ' pgMeta
        ' 
        pgMeta.Dock = DockStyle.Fill
        pgMeta.Font = New Font("Lucida Console", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        pgMeta.Location = New Point(4, 3)
        pgMeta.Margin = New Padding(4, 3, 4, 3)
        pgMeta.Name = "pgMeta"
        pgMeta.Size = New Size(559, 734)
        pgMeta.TabIndex = 1
        pgMeta.ToolbarVisible = False
        ' 
        ' TabPage3
        ' 
        TabPage3.Controls.Add(pgPlotAndText)
        TabPage3.Location = New Point(4, 22)
        TabPage3.Margin = New Padding(4, 3, 4, 3)
        TabPage3.Name = "TabPage3"
        TabPage3.Padding = New Padding(4, 3, 4, 3)
        TabPage3.Size = New Size(567, 740)
        TabPage3.TabIndex = 2
        TabPage3.Text = "Plot / Report"
        TabPage3.UseVisualStyleBackColor = True
        ' 
        ' pgPlotAndText
        ' 
        pgPlotAndText.Dock = DockStyle.Fill
        pgPlotAndText.Font = New Font("Lucida Console", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        pgPlotAndText.Location = New Point(4, 3)
        pgPlotAndText.Margin = New Padding(4, 3, 4, 3)
        pgPlotAndText.Name = "pgPlotAndText"
        pgPlotAndText.Size = New Size(559, 734)
        pgPlotAndText.TabIndex = 2
        pgPlotAndText.ToolbarVisible = False
        ' 
        ' scMain
        ' 
        scMain.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        scMain.Location = New Point(0, 75)
        scMain.Margin = New Padding(4, 3, 4, 3)
        scMain.Name = "scMain"
        ' 
        ' scMain.Panel1
        ' 
        scMain.Panel1.Controls.Add(SplitContainer2)
        ' 
        ' scMain.Panel2
        ' 
        scMain.Panel2.Controls.Add(SplitContainer3)
        scMain.Size = New Size(1480, 1014)
        scMain.SplitterDistance = 575
        scMain.SplitterWidth = 5
        scMain.TabIndex = 7
        ' 
        ' SplitContainer2
        ' 
        SplitContainer2.Dock = DockStyle.Fill
        SplitContainer2.Location = New Point(0, 0)
        SplitContainer2.Margin = New Padding(4, 3, 4, 3)
        SplitContainer2.Name = "SplitContainer2"
        SplitContainer2.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer2.Panel1
        ' 
        SplitContainer2.Panel1.Controls.Add(tbItemName)
        SplitContainer2.Panel1.Controls.Add(tcMain)
        ' 
        ' SplitContainer2.Panel2
        ' 
        SplitContainer2.Panel2.Controls.Add(tbLogOutput)
        SplitContainer2.Size = New Size(575, 1014)
        SplitContainer2.SplitterDistance = 800
        SplitContainer2.SplitterWidth = 5
        SplitContainer2.TabIndex = 0
        ' 
        ' tbItemName
        ' 
        tbItemName.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        tbItemName.Location = New Point(8, 769)
        tbItemName.Margin = New Padding(4, 3, 4, 3)
        tbItemName.Name = "tbItemName"
        tbItemName.ReadOnly = True
        tbItemName.Size = New Size(557, 23)
        tbItemName.TabIndex = 7
        ' 
        ' SplitContainer3
        ' 
        SplitContainer3.Dock = DockStyle.Fill
        SplitContainer3.Location = New Point(0, 0)
        SplitContainer3.Margin = New Padding(4, 3, 4, 3)
        SplitContainer3.Name = "SplitContainer3"
        SplitContainer3.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer3.Panel1
        ' 
        SplitContainer3.Panel1.Controls.Add(zgcMain)
        ' 
        ' SplitContainer3.Panel2
        ' 
        SplitContainer3.Panel2.Controls.Add(rtbStatistics)
        SplitContainer3.Size = New Size(900, 1014)
        SplitContainer3.SplitterDistance = 496
        SplitContainer3.SplitterWidth = 5
        SplitContainer3.TabIndex = 0
        ' 
        ' rtbStatistics
        ' 
        rtbStatistics.BorderStyle = BorderStyle.None
        rtbStatistics.Dock = DockStyle.Fill
        rtbStatistics.Font = New Font("Lucida Console", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        rtbStatistics.Location = New Point(0, 0)
        rtbStatistics.Margin = New Padding(4, 3, 4, 3)
        rtbStatistics.Name = "rtbStatistics"
        rtbStatistics.Size = New Size(900, 513)
        rtbStatistics.TabIndex = 0
        rtbStatistics.Text = ""
        rtbStatistics.WordWrap = False
        ' 
        ' ofdMain
        ' 
        ofdMain.FileName = "OpenFileDialog1"
        ' 
        ' tStatusUpdate
        ' 
        tStatusUpdate.Enabled = True
        tStatusUpdate.Interval = 250
        ' 
        ' tsmiPreset_Test
        ' 
        tsmiPreset_Test.DropDownItems.AddRange(New ToolStripItem() {tsmiPreset_Test_JustOnce})
        tsmiPreset_Test.Name = "tsmiPreset_Test"
        tsmiPreset_Test.Size = New Size(237, 22)
        tsmiPreset_Test.Text = "Test settings"
        ' 
        ' tsmiPreset_Test_JustOnce
        ' 
        tsmiPreset_Test_JustOnce.Name = "tsmiPreset_Test_JustOnce"
        tsmiPreset_Test_JustOnce.Size = New Size(188, 22)
        tsmiPreset_Test_JustOnce.Text = "Just capture fast once"
        ' 
        ' MainForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1480, 1118)
        Controls.Add(scMain)
        Controls.Add(tsMain)
        Controls.Add(ssMain)
        Controls.Add(msMain)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MainMenuStrip = msMain
        Margin = New Padding(4, 3, 4, 3)
        Name = "MainForm"
        Text = "QHY Capture"
        ssMain.ResumeLayout(False)
        ssMain.PerformLayout()
        msMain.ResumeLayout(False)
        msMain.PerformLayout()
        tsMain.ResumeLayout(False)
        tsMain.PerformLayout()
        tcMain.ResumeLayout(False)
        TabPage1.ResumeLayout(False)
        TabPage2.ResumeLayout(False)
        TabPage3.ResumeLayout(False)
        scMain.Panel1.ResumeLayout(False)
        scMain.Panel2.ResumeLayout(False)
        CType(scMain, ComponentModel.ISupportInitialize).EndInit()
        scMain.ResumeLayout(False)
        SplitContainer2.Panel1.ResumeLayout(False)
        SplitContainer2.Panel1.PerformLayout()
        SplitContainer2.Panel2.ResumeLayout(False)
        SplitContainer2.Panel2.PerformLayout()
        CType(SplitContainer2, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer2.ResumeLayout(False)
        SplitContainer3.Panel1.ResumeLayout(False)
        SplitContainer3.Panel2.ResumeLayout(False)
        CType(SplitContainer3, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer3.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents pgMain As System.Windows.Forms.PropertyGrid
    Friend WithEvents ssMain As System.Windows.Forms.StatusStrip
    Friend WithEvents msMain As System.Windows.Forms.MenuStrip
    Friend WithEvents tsmiFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiFile_ExploreHere As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmiFile_Exit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CaptureToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsslMain As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents zgcMain As ZedGraph.ZedGraphControl
    Friend WithEvents tspbProgress As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents tsslProgress As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tSetTemp As System.Windows.Forms.Timer
    Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbCapture As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbStopCapture As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsmiPreset As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ilMain As System.Windows.Forms.ImageList
    Friend WithEvents SeriesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AllReadoutModesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExposureTimeSeriesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GainVariationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiFile_TestWebInterface As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiPreset_FastLive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tbLogOutput As System.Windows.Forms.TextBox
    Friend WithEvents tsmiPreset_CenterROI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tcMain As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents pgMeta As System.Windows.Forms.PropertyGrid
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents rtbStatistics As System.Windows.Forms.RichTextBox
    Friend WithEvents tsmiFile_ExploreCampaign As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiActions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiActions_ResetLoopStat As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmiFile_StoreEXCELStat As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sfdMain As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tsmiNewGUID As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiFile_RunSequence As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As ToolStripSeparator
    Friend WithEvents tsmiActions_Mount As ToolStripMenuItem
    Friend WithEvents ofdMain As OpenFileDialog
    Friend WithEvents tsslMemory As ToolStripStatusLabel
    Friend WithEvents tStatusUpdate As Timer
    Friend WithEvents tsmiFile_OpenLastFile As ToolStripMenuItem
    Friend WithEvents tsmiClearLog As ToolStripMenuItem
    Friend WithEvents tsslLED_capture As ToolStripStatusLabel
    Friend WithEvents tsslLED_reading As ToolStripStatusLabel
    Friend WithEvents tsslSplit1 As ToolStripStatusLabel
    Friend WithEvents tsslSplit2 As ToolStripStatusLabel
    Friend WithEvents tsmiPreset_SaveTransmission As ToolStripMenuItem
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents pgPlotAndText As PropertyGrid
    Friend WithEvents tsbCooling As ToolStripButton
    Friend WithEvents tsmiFile_SaveAllXMLParameters As ToolStripMenuItem
    Friend WithEvents tsslLED_cooling As ToolStripStatusLabel
    Friend WithEvents tsmiFile_CreateXML As ToolStripMenuItem
    Friend WithEvents tsslLED_config As ToolStripStatusLabel
    Friend WithEvents tsslLED_init As ToolStripStatusLabel
    Friend WithEvents tsslTemperature As ToolStripStatusLabel
    Friend WithEvents tsmiFile_LoadSettings As ToolStripMenuItem
    Friend WithEvents tsmiPreset_SkipCooling As ToolStripMenuItem
    Friend WithEvents tsmiTools As ToolStripMenuItem
    Friend WithEvents tsmiTools_AllQHYDLLs As ToolStripMenuItem
    Friend WithEvents tsmiPreset_DevTestMWeiss As ToolStripMenuItem
    Friend WithEvents tsmiPreset_NoOverhead As ToolStripMenuItem
    Friend WithEvents tsmiTools_Log As ToolStripMenuItem
    Friend WithEvents tsmiTools_Log_Store As ToolStripMenuItem
    Friend WithEvents tsmiTools_Log_Clear As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents tsmiActions_AllCoolersOff As ToolStripMenuItem
    Friend WithEvents tsmiPreset_StandardCapture As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripSeparator
    Friend WithEvents tsmiFile_SaveSettings As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As ToolStripSeparator
    Friend WithEvents tsmiPreset_DSC As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As ToolStripSeparator
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents tsslETA As ToolStripStatusLabel
    Friend WithEvents tbItemName As TextBox
    Friend WithEvents tsmiActions_Mount_10Micron As ToolStripMenuItem
    Friend WithEvents tsmiActions_Mount_PWI4 As ToolStripMenuItem
    Friend WithEvents tsmiPreset_Test As ToolStripMenuItem
    Friend WithEvents tsmiPreset_Test_JustOnce As ToolStripMenuItem
End Class
