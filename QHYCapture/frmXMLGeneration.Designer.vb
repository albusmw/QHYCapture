<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmXMLGeneration
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
        Me.tbCode = New System.Windows.Forms.TextBox()
        Me.btnBuildFile = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'tbCode
        '
        Me.tbCode.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbCode.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbCode.Location = New System.Drawing.Point(12, 12)
        Me.tbCode.Multiline = True
        Me.tbCode.Name = "tbCode"
        Me.tbCode.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbCode.Size = New System.Drawing.Size(823, 444)
        Me.tbCode.TabIndex = 0
        Me.tbCode.WordWrap = False
        '
        'btnBuildFile
        '
        Me.btnBuildFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBuildFile.Location = New System.Drawing.Point(12, 462)
        Me.btnBuildFile.Name = "btnBuildFile"
        Me.btnBuildFile.Size = New System.Drawing.Size(823, 32)
        Me.btnBuildFile.TabIndex = 1
        Me.btnBuildFile.Text = "Build file"
        Me.btnBuildFile.UseVisualStyleBackColor = True
        '
        'frmXMLGeneration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(847, 506)
        Me.Controls.Add(Me.btnBuildFile)
        Me.Controls.Add(Me.tbCode)
        Me.Name = "frmXMLGeneration"
        Me.Text = "XML Sequence Generation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbCode As TextBox
    Friend WithEvents btnBuildFile As Button
End Class
