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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.pgMain = New System.Windows.Forms.PropertyGrid()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tsslLED_init = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslLED_config = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslLED_cooling = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslLED_capture = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslLED_reading = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tspbProgress = New System.Windows.Forms.ToolStripProgressBar()
        Me.tsslProgress = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslSplit1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslMain = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslSplit2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslTemperature = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsmiFPSIndicator = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslMemory = New System.Windows.Forms.ToolStripStatusLabel()
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.tsmiFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_LoadSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_SaveSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_RunSequence = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_SaveAllXMLParameters = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiFile_ExploreHere = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_ExploreCampaign = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_OpenLastFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiFile_CreateXML = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_TestWebInterface = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiFile_StoreEXCELStat = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiFile_Exit = New System.Windows.Forms.ToolStripMenuItem()
        Me.CaptureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SeriesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AllReadoutModesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExposureTimeSeriesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GainVariationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiPreset = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiPreset_StandardCapture = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiPreset_FastLive = New System.Windows.Forms.ToolStripMenuItem()
        Me.CenterROIToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveTransmissionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiPreset_SkipCooling = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiPreset_DevTestMWeiss = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiPreset_NoOverhead = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiActions = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiResetLoopStat = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiNewGUID = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiLoad10MicronData = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiClearLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiActions_AllCoolersOff = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiTools_AllQHYDLLs = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiTools_Log = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiTools_Log_Store = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiTools_Log_Clear = New System.Windows.Forms.ToolStripMenuItem()
        Me.zgcMain = New ZedGraph.ZedGraphControl()
        Me.tSetTemp = New System.Windows.Forms.Timer(Me.components)
        Me.tsMain = New System.Windows.Forms.ToolStrip()
        Me.tsbCapture = New System.Windows.Forms.ToolStripButton()
        Me.tsbStopCapture = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbCooling = New System.Windows.Forms.ToolStripButton()
        Me.ilMain = New System.Windows.Forms.ImageList(Me.components)
        Me.tbLogOutput = New System.Windows.Forms.TextBox()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.pgMeta = New System.Windows.Forms.PropertyGrid()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.pgPlotAndText = New System.Windows.Forms.PropertyGrid()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.rtbStatistics = New System.Windows.Forms.RichTextBox()
        Me.sfdMain = New System.Windows.Forms.SaveFileDialog()
        Me.ofdMain = New System.Windows.Forms.OpenFileDialog()
        Me.tStatusUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ssMain.SuspendLayout()
        Me.msMain.SuspendLayout()
        Me.tsMain.SuspendLayout()
        Me.tcMain.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.SuspendLayout()
        '
        'pgMain
        '
        Me.pgMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgMain.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pgMain.Location = New System.Drawing.Point(3, 3)
        Me.pgMain.Name = "pgMain"
        Me.pgMain.Size = New System.Drawing.Size(353, 302)
        Me.pgMain.TabIndex = 0
        Me.pgMain.ToolbarVisible = False
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslLED_init, Me.tsslLED_config, Me.tsslLED_cooling, Me.tsslLED_capture, Me.tsslLED_reading, Me.tspbProgress, Me.tsslProgress, Me.tsslSplit1, Me.tsslMain, Me.tsslSplit2, Me.tsslTemperature, Me.tsmiFPSIndicator, Me.tsslMemory})
        Me.ssMain.Location = New System.Drawing.Point(0, 490)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(943, 24)
        Me.ssMain.TabIndex = 1
        Me.ssMain.Text = "StatusStrip1"
        '
        'tsslLED_init
        '
        Me.tsslLED_init.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.tsslLED_init.Enabled = False
        Me.tsslLED_init.Name = "tsslLED_init"
        Me.tsslLED_init.Size = New System.Drawing.Size(28, 19)
        Me.tsslLED_init.Text = "Init"
        '
        'tsslLED_config
        '
        Me.tsslLED_config.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.tsslLED_config.Enabled = False
        Me.tsslLED_config.Name = "tsslLED_config"
        Me.tsslLED_config.Size = New System.Drawing.Size(47, 19)
        Me.tsslLED_config.Text = "Config"
        '
        'tsslLED_cooling
        '
        Me.tsslLED_cooling.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.tsslLED_cooling.Enabled = False
        Me.tsslLED_cooling.Name = "tsslLED_cooling"
        Me.tsslLED_cooling.Size = New System.Drawing.Size(53, 19)
        Me.tsslLED_cooling.Text = "Cooling"
        '
        'tsslLED_capture
        '
        Me.tsslLED_capture.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.tsslLED_capture.Enabled = False
        Me.tsslLED_capture.Name = "tsslLED_capture"
        Me.tsslLED_capture.Size = New System.Drawing.Size(53, 19)
        Me.tsslLED_capture.Text = "Capture"
        '
        'tsslLED_reading
        '
        Me.tsslLED_reading.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.tsslLED_reading.Enabled = False
        Me.tsslLED_reading.Name = "tsslLED_reading"
        Me.tsslLED_reading.Size = New System.Drawing.Size(63, 19)
        Me.tsslLED_reading.Text = "Read data"
        '
        'tspbProgress
        '
        Me.tspbProgress.ForeColor = System.Drawing.Color.Lime
        Me.tspbProgress.Name = "tspbProgress"
        Me.tspbProgress.Size = New System.Drawing.Size(300, 18)
        Me.tspbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'tsslProgress
        '
        Me.tsslProgress.Name = "tsslProgress"
        Me.tsslProgress.Size = New System.Drawing.Size(22, 19)
        Me.tsslProgress.Text = "---"
        '
        'tsslSplit1
        '
        Me.tsslSplit1.Name = "tsslSplit1"
        Me.tsslSplit1.Size = New System.Drawing.Size(10, 19)
        Me.tsslSplit1.Text = "|"
        '
        'tsslMain
        '
        Me.tsslMain.Name = "tsslMain"
        Me.tsslMain.Size = New System.Drawing.Size(50, 19)
        Me.tsslMain.Text = "--IDLE--"
        '
        'tsslSplit2
        '
        Me.tsslSplit2.Name = "tsslSplit2"
        Me.tsslSplit2.Size = New System.Drawing.Size(10, 19)
        Me.tsslSplit2.Text = "|"
        '
        'tsslTemperature
        '
        Me.tsslTemperature.Name = "tsslTemperature"
        Me.tsslTemperature.Size = New System.Drawing.Size(58, 19)
        Me.tsslTemperature.Text = "T = ??? °C"
        '
        'tsmiFPSIndicator
        '
        Me.tsmiFPSIndicator.Name = "tsmiFPSIndicator"
        Me.tsmiFPSIndicator.Size = New System.Drawing.Size(47, 19)
        Me.tsmiFPSIndicator.Text = "FPS: ???"
        '
        'tsslMemory
        '
        Me.tsslMemory.Name = "tsslMemory"
        Me.tsslMemory.Size = New System.Drawing.Size(73, 19)
        Me.tsslMemory.Text = "Memory: ???"
        '
        'msMain
        '
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFile, Me.CaptureToolStripMenuItem, Me.tsmiPreset, Me.tsmiActions, Me.tsmiTools})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(943, 24)
        Me.msMain.TabIndex = 2
        Me.msMain.Text = "MenuStrip1"
        '
        'tsmiFile
        '
        Me.tsmiFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiFile_LoadSettings, Me.tsmiFile_SaveSettings, Me.tsmiFile_SaveAllXMLParameters, Me.ToolStripMenuItem4, Me.tsmiFile_RunSequence, Me.ToolStripMenuItem6, Me.tsmiFile_ExploreHere, Me.tsmiFile_ExploreCampaign, Me.tsmiFile_OpenLastFile, Me.ToolStripMenuItem1, Me.tsmiFile_CreateXML, Me.tsmiFile_TestWebInterface, Me.tsmiFile_StoreEXCELStat, Me.ToolStripMenuItem5, Me.tsmiFile_Exit})
        Me.tsmiFile.Name = "tsmiFile"
        Me.tsmiFile.Size = New System.Drawing.Size(37, 20)
        Me.tsmiFile.Text = "File"
        '
        'tsmiFile_LoadSettings
        '
        Me.tsmiFile_LoadSettings.Name = "tsmiFile_LoadSettings"
        Me.tsmiFile_LoadSettings.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.tsmiFile_LoadSettings.Size = New System.Drawing.Size(286, 22)
        Me.tsmiFile_LoadSettings.Tag = "Load"
        Me.tsmiFile_LoadSettings.Text = "Load settings (does not expose)"
        '
        'tsmiFile_SaveSettings
        '
        Me.tsmiFile_SaveSettings.Name = "tsmiFile_SaveSettings"
        Me.tsmiFile_SaveSettings.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.tsmiFile_SaveSettings.Size = New System.Drawing.Size(286, 22)
        Me.tsmiFile_SaveSettings.Text = "Save settings"
        '
        'tsmiFile_RunSequence
        '
        Me.tsmiFile_RunSequence.Name = "tsmiFile_RunSequence"
        Me.tsmiFile_RunSequence.Size = New System.Drawing.Size(286, 22)
        Me.tsmiFile_RunSequence.Tag = "Run"
        Me.tsmiFile_RunSequence.Text = "Run XML sequence"
        '
        'tsmiFile_SaveAllXMLParameters
        '
        Me.tsmiFile_SaveAllXMLParameters.Name = "tsmiFile_SaveAllXMLParameters"
        Me.tsmiFile_SaveAllXMLParameters.Size = New System.Drawing.Size(286, 22)
        Me.tsmiFile_SaveAllXMLParameters.Text = "Save all XML parameters"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(283, 6)
        '
        'tsmiFile_ExploreHere
        '
        Me.tsmiFile_ExploreHere.Name = "tsmiFile_ExploreHere"
        Me.tsmiFile_ExploreHere.Size = New System.Drawing.Size(286, 22)
        Me.tsmiFile_ExploreHere.Text = "Explorer @ EXE path"
        '
        'tsmiFile_ExploreCampaign
        '
        Me.tsmiFile_ExploreCampaign.Name = "tsmiFile_ExploreCampaign"
        Me.tsmiFile_ExploreCampaign.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.tsmiFile_ExploreCampaign.Size = New System.Drawing.Size(286, 22)
        Me.tsmiFile_ExploreCampaign.Text = "Explorer @ current campaign"
        '
        'tsmiFile_OpenLastFile
        '
        Me.tsmiFile_OpenLastFile.Name = "tsmiFile_OpenLastFile"
        Me.tsmiFile_OpenLastFile.Size = New System.Drawing.Size(286, 22)
        Me.tsmiFile_OpenLastFile.Text = "Open last stored file"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(283, 6)
        '
        'tsmiFile_CreateXML
        '
        Me.tsmiFile_CreateXML.Name = "tsmiFile_CreateXML"
        Me.tsmiFile_CreateXML.Size = New System.Drawing.Size(286, 22)
        Me.tsmiFile_CreateXML.Text = "Create XML sequence (Inline VB)"
        '
        'tsmiFile_TestWebInterface
        '
        Me.tsmiFile_TestWebInterface.Name = "tsmiFile_TestWebInterface"
        Me.tsmiFile_TestWebInterface.Size = New System.Drawing.Size(286, 22)
        Me.tsmiFile_TestWebInterface.Text = "Test web interface"
        '
        'tsmiFile_StoreEXCELStat
        '
        Me.tsmiFile_StoreEXCELStat.Name = "tsmiFile_StoreEXCELStat"
        Me.tsmiFile_StoreEXCELStat.Size = New System.Drawing.Size(286, 22)
        Me.tsmiFile_StoreEXCELStat.Text = "Store statistics as EXCEL file"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(283, 6)
        '
        'tsmiFile_Exit
        '
        Me.tsmiFile_Exit.Name = "tsmiFile_Exit"
        Me.tsmiFile_Exit.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.tsmiFile_Exit.Size = New System.Drawing.Size(286, 22)
        Me.tsmiFile_Exit.Text = "Exit"
        '
        'CaptureToolStripMenuItem
        '
        Me.CaptureToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SeriesToolStripMenuItem})
        Me.CaptureToolStripMenuItem.Name = "CaptureToolStripMenuItem"
        Me.CaptureToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.CaptureToolStripMenuItem.Text = "Capture"
        '
        'SeriesToolStripMenuItem
        '
        Me.SeriesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AllReadoutModesToolStripMenuItem, Me.ExposureTimeSeriesToolStripMenuItem, Me.GainVariationToolStripMenuItem})
        Me.SeriesToolStripMenuItem.Name = "SeriesToolStripMenuItem"
        Me.SeriesToolStripMenuItem.Size = New System.Drawing.Size(104, 22)
        Me.SeriesToolStripMenuItem.Text = "Series"
        '
        'AllReadoutModesToolStripMenuItem
        '
        Me.AllReadoutModesToolStripMenuItem.Name = "AllReadoutModesToolStripMenuItem"
        Me.AllReadoutModesToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.AllReadoutModesToolStripMenuItem.Text = "All read-out modes"
        '
        'ExposureTimeSeriesToolStripMenuItem
        '
        Me.ExposureTimeSeriesToolStripMenuItem.Name = "ExposureTimeSeriesToolStripMenuItem"
        Me.ExposureTimeSeriesToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.ExposureTimeSeriesToolStripMenuItem.Text = "Exposure time series"
        '
        'GainVariationToolStripMenuItem
        '
        Me.GainVariationToolStripMenuItem.Name = "GainVariationToolStripMenuItem"
        Me.GainVariationToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.GainVariationToolStripMenuItem.Text = "Gain variation"
        '
        'tsmiPreset
        '
        Me.tsmiPreset.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiPreset_StandardCapture, Me.ToolStripMenuItem3, Me.tsmiPreset_FastLive, Me.CenterROIToolStripMenuItem, Me.SaveTransmissionToolStripMenuItem, Me.tsmiPreset_SkipCooling, Me.tsmiPreset_DevTestMWeiss, Me.tsmiPreset_NoOverhead})
        Me.tsmiPreset.Name = "tsmiPreset"
        Me.tsmiPreset.Size = New System.Drawing.Size(56, 20)
        Me.tsmiPreset.Text = "Presets"
        '
        'tsmiPreset_StandardCapture
        '
        Me.tsmiPreset_StandardCapture.Name = "tsmiPreset_StandardCapture"
        Me.tsmiPreset_StandardCapture.Size = New System.Drawing.Size(171, 22)
        Me.tsmiPreset_StandardCapture.Text = "Standard capture"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(168, 6)
        '
        'tsmiPreset_FastLive
        '
        Me.tsmiPreset_FastLive.Name = "tsmiPreset_FastLive"
        Me.tsmiPreset_FastLive.Size = New System.Drawing.Size(171, 22)
        Me.tsmiPreset_FastLive.Text = "Fast live mode"
        '
        'CenterROIToolStripMenuItem
        '
        Me.CenterROIToolStripMenuItem.Name = "CenterROIToolStripMenuItem"
        Me.CenterROIToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.CenterROIToolStripMenuItem.Text = "Center ROI"
        '
        'SaveTransmissionToolStripMenuItem
        '
        Me.SaveTransmissionToolStripMenuItem.Name = "SaveTransmissionToolStripMenuItem"
        Me.SaveTransmissionToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.SaveTransmissionToolStripMenuItem.Text = "Save transmission"
        '
        'tsmiPreset_SkipCooling
        '
        Me.tsmiPreset_SkipCooling.Name = "tsmiPreset_SkipCooling"
        Me.tsmiPreset_SkipCooling.Size = New System.Drawing.Size(171, 22)
        Me.tsmiPreset_SkipCooling.Text = "Skip cooling"
        '
        'tsmiPreset_DevTestMWeiss
        '
        Me.tsmiPreset_DevTestMWeiss.Name = "tsmiPreset_DevTestMWeiss"
        Me.tsmiPreset_DevTestMWeiss.Size = New System.Drawing.Size(171, 22)
        Me.tsmiPreset_DevTestMWeiss.Text = "Dev test (M Weiss)"
        '
        'tsmiPreset_NoOverhead
        '
        Me.tsmiPreset_NoOverhead.Name = "tsmiPreset_NoOverhead"
        Me.tsmiPreset_NoOverhead.Size = New System.Drawing.Size(171, 22)
        Me.tsmiPreset_NoOverhead.Text = "No overhead"
        '
        'tsmiActions
        '
        Me.tsmiActions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiResetLoopStat, Me.tsmiNewGUID, Me.tsmiLoad10MicronData, Me.tsmiClearLog, Me.ToolStripMenuItem2, Me.tsmiActions_AllCoolersOff})
        Me.tsmiActions.Name = "tsmiActions"
        Me.tsmiActions.Size = New System.Drawing.Size(59, 20)
        Me.tsmiActions.Text = "Actions"
        '
        'tsmiResetLoopStat
        '
        Me.tsmiResetLoopStat.Name = "tsmiResetLoopStat"
        Me.tsmiResetLoopStat.Size = New System.Drawing.Size(179, 22)
        Me.tsmiResetLoopStat.Text = "Reset loop statistics"
        '
        'tsmiNewGUID
        '
        Me.tsmiNewGUID.Name = "tsmiNewGUID"
        Me.tsmiNewGUID.Size = New System.Drawing.Size(179, 22)
        Me.tsmiNewGUID.Text = "New GUID"
        '
        'tsmiLoad10MicronData
        '
        Me.tsmiLoad10MicronData.Name = "tsmiLoad10MicronData"
        Me.tsmiLoad10MicronData.Size = New System.Drawing.Size(179, 22)
        Me.tsmiLoad10MicronData.Text = "Load 10Micron data"
        '
        'tsmiClearLog
        '
        Me.tsmiClearLog.Name = "tsmiClearLog"
        Me.tsmiClearLog.Size = New System.Drawing.Size(179, 22)
        Me.tsmiClearLog.Text = "Clear log"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(176, 6)
        '
        'tsmiActions_AllCoolersOff
        '
        Me.tsmiActions_AllCoolersOff.Name = "tsmiActions_AllCoolersOff"
        Me.tsmiActions_AllCoolersOff.Size = New System.Drawing.Size(179, 22)
        Me.tsmiActions_AllCoolersOff.Text = "All coolers off"
        '
        'tsmiTools
        '
        Me.tsmiTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiTools_AllQHYDLLs, Me.tsmiTools_Log})
        Me.tsmiTools.Name = "tsmiTools"
        Me.tsmiTools.Size = New System.Drawing.Size(46, 20)
        Me.tsmiTools.Text = "Tools"
        '
        'tsmiTools_AllQHYDLLs
        '
        Me.tsmiTools_AllQHYDLLs.Name = "tsmiTools_AllQHYDLLs"
        Me.tsmiTools_AllQHYDLLs.Size = New System.Drawing.Size(163, 22)
        Me.tsmiTools_AllQHYDLLs.Text = "Get all QHY DLLs"
        '
        'tsmiTools_Log
        '
        Me.tsmiTools_Log.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiTools_Log_Store, Me.tsmiTools_Log_Clear})
        Me.tsmiTools_Log.Name = "tsmiTools_Log"
        Me.tsmiTools_Log.Size = New System.Drawing.Size(163, 22)
        Me.tsmiTools_Log.Text = "DLL Log"
        '
        'tsmiTools_Log_Store
        '
        Me.tsmiTools_Log_Store.Name = "tsmiTools_Log_Store"
        Me.tsmiTools_Log_Store.Size = New System.Drawing.Size(121, 22)
        Me.tsmiTools_Log_Store.Text = "Store log"
        '
        'tsmiTools_Log_Clear
        '
        Me.tsmiTools_Log_Clear.Name = "tsmiTools_Log_Clear"
        Me.tsmiTools_Log_Clear.Size = New System.Drawing.Size(121, 22)
        Me.tsmiTools_Log_Clear.Text = "Clear"
        '
        'zgcMain
        '
        Me.zgcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgcMain.Location = New System.Drawing.Point(0, 0)
        Me.zgcMain.Name = "zgcMain"
        Me.zgcMain.ScrollGrace = 0R
        Me.zgcMain.ScrollMaxX = 0R
        Me.zgcMain.ScrollMaxY = 0R
        Me.zgcMain.ScrollMaxY2 = 0R
        Me.zgcMain.ScrollMinX = 0R
        Me.zgcMain.ScrollMinY = 0R
        Me.zgcMain.ScrollMinY2 = 0R
        Me.zgcMain.Size = New System.Drawing.Size(572, 208)
        Me.zgcMain.TabIndex = 0
        '
        'tSetTemp
        '
        Me.tSetTemp.Enabled = True
        Me.tSetTemp.Interval = 500
        '
        'tsMain
        '
        Me.tsMain.BackColor = System.Drawing.SystemColors.Control
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbCapture, Me.tsbStopCapture, Me.ToolStripSeparator1, Me.tsbCooling})
        Me.tsMain.Location = New System.Drawing.Point(0, 24)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Size = New System.Drawing.Size(943, 38)
        Me.tsMain.TabIndex = 5
        Me.tsMain.Text = "ToolStrip1"
        '
        'tsbCapture
        '
        Me.tsbCapture.Image = CType(resources.GetObject("tsbCapture.Image"), System.Drawing.Image)
        Me.tsbCapture.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCapture.Name = "tsbCapture"
        Me.tsbCapture.Size = New System.Drawing.Size(53, 35)
        Me.tsbCapture.Text = "Capture"
        Me.tsbCapture.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbStopCapture
        '
        Me.tsbStopCapture.Image = CType(resources.GetObject("tsbStopCapture.Image"), System.Drawing.Image)
        Me.tsbStopCapture.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbStopCapture.Name = "tsbStopCapture"
        Me.tsbStopCapture.Size = New System.Drawing.Size(35, 35)
        Me.tsbStopCapture.Text = "Stop"
        Me.tsbStopCapture.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 38)
        '
        'tsbCooling
        '
        Me.tsbCooling.Image = CType(resources.GetObject("tsbCooling.Image"), System.Drawing.Image)
        Me.tsbCooling.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.tsbCooling.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCooling.Name = "tsbCooling"
        Me.tsbCooling.Size = New System.Drawing.Size(53, 35)
        Me.tsbCooling.Text = "Cooling"
        Me.tsbCooling.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.tsbCooling.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ilMain
        '
        Me.ilMain.ImageStream = CType(resources.GetObject("ilMain.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilMain.TransparentColor = System.Drawing.Color.Transparent
        Me.ilMain.Images.SetKeyName(0, "Capture.png")
        Me.ilMain.Images.SetKeyName(1, "StopCapture.png")
        '
        'tbLogOutput
        '
        Me.tbLogOutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbLogOutput.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLogOutput.Location = New System.Drawing.Point(0, 0)
        Me.tbLogOutput.Multiline = True
        Me.tbLogOutput.Name = "tbLogOutput"
        Me.tbLogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLogOutput.Size = New System.Drawing.Size(367, 85)
        Me.tbLogOutput.TabIndex = 4
        Me.tbLogOutput.WordWrap = False
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.TabPage1)
        Me.tcMain.Controls.Add(Me.TabPage2)
        Me.tcMain.Controls.Add(Me.TabPage3)
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.Font = New System.Drawing.Font("Courier New", 8.25!)
        Me.tcMain.Location = New System.Drawing.Point(0, 0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(367, 335)
        Me.tcMain.TabIndex = 6
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.pgMain)
        Me.TabPage1.Location = New System.Drawing.Point(4, 23)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(359, 308)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Exposure"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.pgMeta)
        Me.TabPage2.Location = New System.Drawing.Point(4, 23)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(359, 308)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Meta data / Advanced"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'pgMeta
        '
        Me.pgMeta.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgMeta.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pgMeta.Location = New System.Drawing.Point(3, 3)
        Me.pgMeta.Name = "pgMeta"
        Me.pgMeta.Size = New System.Drawing.Size(353, 302)
        Me.pgMeta.TabIndex = 1
        Me.pgMeta.ToolbarVisible = False
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.pgPlotAndText)
        Me.TabPage3.Location = New System.Drawing.Point(4, 23)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(359, 308)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Plot / Report"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'pgPlotAndText
        '
        Me.pgPlotAndText.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgPlotAndText.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pgPlotAndText.Location = New System.Drawing.Point(3, 3)
        Me.pgPlotAndText.Name = "pgPlotAndText"
        Me.pgPlotAndText.Size = New System.Drawing.Size(353, 302)
        Me.pgPlotAndText.TabIndex = 2
        Me.pgPlotAndText.ToolbarVisible = False
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(0, 65)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.SplitContainer3)
        Me.scMain.Size = New System.Drawing.Size(943, 424)
        Me.scMain.SplitterDistance = 367
        Me.scMain.TabIndex = 7
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.tcMain)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.tbLogOutput)
        Me.SplitContainer2.Size = New System.Drawing.Size(367, 424)
        Me.SplitContainer2.SplitterDistance = 335
        Me.SplitContainer2.TabIndex = 0
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        Me.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.zgcMain)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.rtbStatistics)
        Me.SplitContainer3.Size = New System.Drawing.Size(572, 424)
        Me.SplitContainer3.SplitterDistance = 208
        Me.SplitContainer3.TabIndex = 0
        '
        'rtbStatistics
        '
        Me.rtbStatistics.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbStatistics.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbStatistics.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbStatistics.Location = New System.Drawing.Point(0, 0)
        Me.rtbStatistics.Name = "rtbStatistics"
        Me.rtbStatistics.Size = New System.Drawing.Size(572, 212)
        Me.rtbStatistics.TabIndex = 0
        Me.rtbStatistics.Text = ""
        Me.rtbStatistics.WordWrap = False
        '
        'ofdMain
        '
        Me.ofdMain.FileName = "OpenFileDialog1"
        '
        'tStatusUpdate
        '
        Me.tStatusUpdate.Enabled = True
        Me.tStatusUpdate.Interval = 250
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(283, 6)
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(943, 514)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.tsMain)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.msMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.msMain
        Me.Name = "MainForm"
        Me.Text = "QHY Capture"
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        Me.tcMain.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.Panel2.PerformLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pgMain As Windows.Forms.PropertyGrid
    Friend WithEvents ssMain As Windows.Forms.StatusStrip
    Friend WithEvents msMain As Windows.Forms.MenuStrip
    Friend WithEvents tsmiFile As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiFile_ExploreHere As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmiFile_Exit As Windows.Forms.ToolStripMenuItem
    Friend WithEvents CaptureToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsslMain As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents zgcMain As ZedGraph.ZedGraphControl
    Friend WithEvents tspbProgress As Windows.Forms.ToolStripProgressBar
    Friend WithEvents tsslProgress As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tSetTemp As Windows.Forms.Timer
    Friend WithEvents tsMain As Windows.Forms.ToolStrip
    Friend WithEvents tsbCapture As Windows.Forms.ToolStripButton
    Friend WithEvents tsbStopCapture As Windows.Forms.ToolStripButton
    Friend WithEvents tsmiPreset As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ilMain As Windows.Forms.ImageList
    Friend WithEvents SeriesToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents AllReadoutModesToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExposureTimeSeriesToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents GainVariationToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiFile_TestWebInterface As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiPreset_FastLive As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tbLogOutput As Windows.Forms.TextBox
    Friend WithEvents CenterROIToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tcMain As Windows.Forms.TabControl
    Friend WithEvents TabPage1 As Windows.Forms.TabPage
    Friend WithEvents TabPage2 As Windows.Forms.TabPage
    Friend WithEvents pgMeta As Windows.Forms.PropertyGrid
    Friend WithEvents scMain As Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer3 As Windows.Forms.SplitContainer
    Friend WithEvents rtbStatistics As Windows.Forms.RichTextBox
    Friend WithEvents tsmiFile_ExploreCampaign As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiActions As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiResetLoopStat As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmiFPSIndicator As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsmiFile_StoreEXCELStat As Windows.Forms.ToolStripMenuItem
    Friend WithEvents sfdMain As Windows.Forms.SaveFileDialog
    Friend WithEvents tsmiNewGUID As Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiFile_RunSequence As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem5 As ToolStripSeparator
    Friend WithEvents tsmiLoad10MicronData As ToolStripMenuItem
    Friend WithEvents ofdMain As OpenFileDialog
    Friend WithEvents tsslMemory As ToolStripStatusLabel
    Friend WithEvents tStatusUpdate As Timer
    Friend WithEvents tsmiFile_OpenLastFile As ToolStripMenuItem
    Friend WithEvents tsmiClearLog As ToolStripMenuItem
    Friend WithEvents tsslLED_capture As ToolStripStatusLabel
    Friend WithEvents tsslLED_reading As ToolStripStatusLabel
    Friend WithEvents tsslSplit1 As ToolStripStatusLabel
    Friend WithEvents tsslSplit2 As ToolStripStatusLabel
    Friend WithEvents SaveTransmissionToolStripMenuItem As ToolStripMenuItem
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
End Class
