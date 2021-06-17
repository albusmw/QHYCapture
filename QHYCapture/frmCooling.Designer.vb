<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCooling
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tQuery = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbT_set = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbT_measured = New System.Windows.Forms.TextBox()
        Me.btnSetTemp = New System.Windows.Forms.Button()
        Me.tbCoolerPWM = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.gbTimedControl = New System.Windows.Forms.GroupBox()
        Me.cbStartTime = New System.Windows.Forms.CheckBox()
        Me.tbStartTime = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tbTimedCool = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cbStartTime2 = New System.Windows.Forms.CheckBox()
        Me.tbStartTime2 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tbTimedCool2 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.tbTimedCoolSpeed2 = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.pbDelta = New System.Windows.Forms.ToolStripProgressBar()
        Me.tsslMain = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tbTimedCoolSpeed = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lbTimings = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.gbTimedControl.SuspendLayout()
        Me.ssMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tQuery
        '
        Me.tQuery.Enabled = True
        Me.tQuery.Interval = 250
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "T requested"
        '
        'tbT_set
        '
        Me.tbT_set.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbT_set.Location = New System.Drawing.Point(82, 17)
        Me.tbT_set.Name = "tbT_set"
        Me.tbT_set.ReadOnly = True
        Me.tbT_set.Size = New System.Drawing.Size(69, 20)
        Me.tbT_set.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "T measured"
        '
        'tbT_measured
        '
        Me.tbT_measured.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbT_measured.Location = New System.Drawing.Point(82, 43)
        Me.tbT_measured.Name = "tbT_measured"
        Me.tbT_measured.ReadOnly = True
        Me.tbT_measured.Size = New System.Drawing.Size(69, 20)
        Me.tbT_measured.TabIndex = 3
        '
        'btnSetTemp
        '
        Me.btnSetTemp.Location = New System.Drawing.Point(157, 12)
        Me.btnSetTemp.Name = "btnSetTemp"
        Me.btnSetTemp.Size = New System.Drawing.Size(57, 29)
        Me.btnSetTemp.TabIndex = 4
        Me.btnSetTemp.Text = "Change"
        Me.btnSetTemp.UseVisualStyleBackColor = True
        '
        'tbCoolerPWM
        '
        Me.tbCoolerPWM.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbCoolerPWM.Location = New System.Drawing.Point(82, 69)
        Me.tbCoolerPWM.Name = "tbCoolerPWM"
        Me.tbCoolerPWM.ReadOnly = True
        Me.tbCoolerPWM.Size = New System.Drawing.Size(69, 20)
        Me.tbCoolerPWM.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Cooler @"
        '
        'gbTimedControl
        '
        Me.gbTimedControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbTimedControl.Controls.Add(Me.Label12)
        Me.gbTimedControl.Controls.Add(Me.lbTimings)
        Me.gbTimedControl.Controls.Add(Me.Label11)
        Me.gbTimedControl.Controls.Add(Me.Label10)
        Me.gbTimedControl.Controls.Add(Me.tbTimedCoolSpeed)
        Me.gbTimedControl.Controls.Add(Me.Label9)
        Me.gbTimedControl.Controls.Add(Me.tbTimedCoolSpeed2)
        Me.gbTimedControl.Controls.Add(Me.Label8)
        Me.gbTimedControl.Controls.Add(Me.Label7)
        Me.gbTimedControl.Controls.Add(Me.tbTimedCool2)
        Me.gbTimedControl.Controls.Add(Me.Label6)
        Me.gbTimedControl.Controls.Add(Me.tbStartTime2)
        Me.gbTimedControl.Controls.Add(Me.cbStartTime2)
        Me.gbTimedControl.Controls.Add(Me.Label5)
        Me.gbTimedControl.Controls.Add(Me.tbTimedCool)
        Me.gbTimedControl.Controls.Add(Me.Label4)
        Me.gbTimedControl.Controls.Add(Me.tbStartTime)
        Me.gbTimedControl.Controls.Add(Me.cbStartTime)
        Me.gbTimedControl.Location = New System.Drawing.Point(15, 95)
        Me.gbTimedControl.Name = "gbTimedControl"
        Me.gbTimedControl.Size = New System.Drawing.Size(441, 127)
        Me.gbTimedControl.TabIndex = 9
        Me.gbTimedControl.TabStop = False
        Me.gbTimedControl.Text = "Timed control"
        '
        'cbStartTime
        '
        Me.cbStartTime.AutoSize = True
        Me.cbStartTime.Location = New System.Drawing.Point(12, 18)
        Me.cbStartTime.Name = "cbStartTime"
        Me.cbStartTime.Size = New System.Drawing.Size(36, 17)
        Me.cbStartTime.TabIndex = 0
        Me.cbStartTime.Text = "At"
        Me.cbStartTime.UseVisualStyleBackColor = True
        '
        'tbStartTime
        '
        Me.tbStartTime.Location = New System.Drawing.Point(67, 15)
        Me.tbStartTime.Name = "tbStartTime"
        Me.tbStartTime.Size = New System.Drawing.Size(47, 20)
        Me.tbStartTime.TabIndex = 1
        Me.tbStartTime.Text = "22:00"
        Me.tbStartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(139, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "cool to"
        '
        'tbTimedCool
        '
        Me.tbTimedCool.Location = New System.Drawing.Point(184, 15)
        Me.tbTimedCool.Name = "tbTimedCool"
        Me.tbTimedCool.Size = New System.Drawing.Size(43, 20)
        Me.tbTimedCool.TabIndex = 4
        Me.tbTimedCool.Text = "0"
        Me.tbTimedCool.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(233, 18)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(21, 13)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "° C"
        '
        'cbStartTime2
        '
        Me.cbStartTime2.AutoSize = True
        Me.cbStartTime2.Location = New System.Drawing.Point(12, 43)
        Me.cbStartTime2.Name = "cbStartTime2"
        Me.cbStartTime2.Size = New System.Drawing.Size(48, 17)
        Me.cbStartTime2.TabIndex = 6
        Me.cbStartTime2.Text = "After"
        Me.cbStartTime2.UseVisualStyleBackColor = True
        '
        'tbStartTime2
        '
        Me.tbStartTime2.Location = New System.Drawing.Point(67, 41)
        Me.tbStartTime2.Name = "tbStartTime2"
        Me.tbStartTime2.Size = New System.Drawing.Size(47, 20)
        Me.tbStartTime2.TabIndex = 7
        Me.tbStartTime2.Text = "10"
        Me.tbStartTime2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(120, 44)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "min cool to"
        '
        'tbTimedCool2
        '
        Me.tbTimedCool2.Location = New System.Drawing.Point(184, 41)
        Me.tbTimedCool2.Name = "tbTimedCool2"
        Me.tbTimedCool2.Size = New System.Drawing.Size(43, 20)
        Me.tbTimedCool2.TabIndex = 9
        Me.tbTimedCool2.Text = "-10"
        Me.tbTimedCool2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(233, 44)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(21, 13)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "° C"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(260, 44)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(34, 13)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "within"
        '
        'tbTimedCoolSpeed2
        '
        Me.tbTimedCoolSpeed2.Enabled = False
        Me.tbTimedCoolSpeed2.Location = New System.Drawing.Point(300, 40)
        Me.tbTimedCoolSpeed2.Name = "tbTimedCoolSpeed2"
        Me.tbTimedCoolSpeed2.Size = New System.Drawing.Size(43, 20)
        Me.tbTimedCoolSpeed2.TabIndex = 12
        Me.tbTimedCoolSpeed2.Text = "0"
        Me.tbTimedCoolSpeed2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(349, 44)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(23, 13)
        Me.Label9.TabIndex = 13
        Me.Label9.Text = "min"
        '
        'ssMain
        '
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.pbDelta, Me.tsslMain})
        Me.ssMain.Location = New System.Drawing.Point(0, 235)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(468, 22)
        Me.ssMain.TabIndex = 10
        Me.ssMain.Text = "StatusStrip1"
        '
        'pbDelta
        '
        Me.pbDelta.Name = "pbDelta"
        Me.pbDelta.Size = New System.Drawing.Size(100, 16)
        Me.pbDelta.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'tsslMain
        '
        Me.tsslMain.Name = "tsslMain"
        Me.tsslMain.Size = New System.Drawing.Size(22, 17)
        Me.tsslMain.Text = "---"
        '
        'tbTimedCoolSpeed
        '
        Me.tbTimedCoolSpeed.Enabled = False
        Me.tbTimedCoolSpeed.Location = New System.Drawing.Point(300, 16)
        Me.tbTimedCoolSpeed.Name = "tbTimedCoolSpeed"
        Me.tbTimedCoolSpeed.Size = New System.Drawing.Size(43, 20)
        Me.tbTimedCoolSpeed.TabIndex = 14
        Me.tbTimedCoolSpeed.Text = "0"
        Me.tbTimedCoolSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(260, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(34, 13)
        Me.Label10.TabIndex = 15
        Me.Label10.Text = "within"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(349, 19)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(23, 13)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "min"
        '
        'lbTimings
        '
        Me.lbTimings.AutoSize = True
        Me.lbTimings.Location = New System.Drawing.Point(32, 96)
        Me.lbTimings.Name = "lbTimings"
        Me.lbTimings.Size = New System.Drawing.Size(16, 13)
        Me.lbTimings.TabIndex = 17
        Me.lbTimings.Text = "---"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(9, 72)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(91, 13)
        Me.Label12.TabIndex = 18
        Me.Label12.Text = "Timing sequence:"
        '
        'frmCooling
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(468, 257)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.gbTimedControl)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbCoolerPWM)
        Me.Controls.Add(Me.btnSetTemp)
        Me.Controls.Add(Me.tbT_measured)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbT_set)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmCooling"
        Me.Text = "Cooling"
        Me.gbTimedControl.ResumeLayout(False)
        Me.gbTimedControl.PerformLayout()
        Me.ssMain.ResumeLayout(False)
        Me.ssMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tQuery As Timer
    Friend WithEvents Label1 As Label
    Friend WithEvents tbT_set As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents tbT_measured As TextBox
    Friend WithEvents btnSetTemp As Button
    Friend WithEvents tbCoolerPWM As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents gbTimedControl As GroupBox
    Friend WithEvents Label9 As Label
    Friend WithEvents tbTimedCoolSpeed2 As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents tbTimedCool2 As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents tbStartTime2 As TextBox
    Friend WithEvents cbStartTime2 As CheckBox
    Friend WithEvents Label5 As Label
    Friend WithEvents tbTimedCool As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents tbStartTime As TextBox
    Friend WithEvents cbStartTime As CheckBox
    Friend WithEvents ssMain As StatusStrip
    Friend WithEvents pbDelta As ToolStripProgressBar
    Friend WithEvents tsslMain As ToolStripStatusLabel
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents tbTimedCoolSpeed As TextBox
    Friend WithEvents lbTimings As Label
    Friend WithEvents Label12 As Label
End Class
