<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImageForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pbeMain = New PictureBoxEx()
        Me.tbFocus = New System.Windows.Forms.TextBox()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        CType(Me.pbeMain, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbeMain
        '
        Me.pbeMain.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.pbeMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbeMain.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor
        Me.pbeMain.Location = New System.Drawing.Point(0, 0)
        Me.pbeMain.Name = "pbeMain"
        Me.pbeMain.Size = New System.Drawing.Size(523, 596)
        Me.pbeMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbeMain.TabIndex = 0
        Me.pbeMain.TabStop = False
        '
        'tbFocus
        '
        Me.tbFocus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbFocus.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbFocus.Location = New System.Drawing.Point(0, 0)
        Me.tbFocus.Multiline = True
        Me.tbFocus.Name = "tbFocus"
        Me.tbFocus.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbFocus.Size = New System.Drawing.Size(291, 596)
        Me.tbFocus.TabIndex = 1
        Me.tbFocus.WordWrap = False
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(12, 12)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.pbeMain)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.tbFocus)
        Me.scMain.Size = New System.Drawing.Size(818, 596)
        Me.scMain.SplitterDistance = 523
        Me.scMain.TabIndex = 2
        '
        'frmImageForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(842, 620)
        Me.Controls.Add(Me.scMain)
        Me.Name = "frmImageForm"
        Me.Text = "Image form"
        CType(Me.pbeMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.Panel2.PerformLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pbeMain As PictureBoxEx
    Friend WithEvents tbFocus As TextBox
    Friend WithEvents scMain As SplitContainer
End Class
