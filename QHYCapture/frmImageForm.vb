Option Explicit On
Option Strict On

'''<summary>Form for focus image quality estimation.</summary>
Public Class frmImageForm

    '''<summary>Back color to use.</summary>
    Public Property ImageBackColor As Drawing.Color = Drawing.Color.Black
    '''<summary>Color map to use.</summary>
    Public Property ColorMap As cColorMaps.eMaps = cColorMaps.eMaps.Jet

    '''<summary>Update the content of the focus window.</summary>
    '''<param name="Form">Focus window.</param>
    '''<param name="Data">Data to display.</param>
    '''<param name="MaxData">Maximum in the data in order to normalize correct.</param>
    Public Sub ShowData(ByRef Data(,) As UInt16, ByVal MinData As Long, ByVal MaxData As Long)
        Dim Data_UInt32(Data.GetUpperBound(0), Data.GetUpperBound(1)) As UInt32
        For X As Integer = 0 To Data.GetUpperBound(0)
            For Y As Integer = 0 To Data.GetUpperBound(1)
                Data_UInt32(X, Y) = Data(X, Y)
            Next Y
        Next X
        ShowData(Data_UInt32, MinData, MaxData)
    End Sub

    '''<summary>Update the content of the focus window.</summary>
    '''<param name="Form">Focus window.</param>
    '''<param name="Data">Data to display.</param>
    '''<param name="MaxData">Maximum in the data in order to normalize correct.</param>
    Public Sub ShowData(ByRef Data(,) As UInt32, ByVal MinData As Long, ByVal MaxData As Long)
        Dim OutputImage As New cLockBitmap(Data.GetUpperBound(0), Data.GetUpperBound(1))
        If MaxData = 0 Then MaxData = 1
        OutputImage.LockBits()
        Dim Stride As Integer = OutputImage.BitmapData.Stride
        Dim BytePerPixel As Integer = OutputImage.ColorBytesPerPixel
        Dim YOffset As Integer = 0
        For Y As Integer = 0 To OutputImage.Height - 1
            Dim BaseOffset As Integer = YOffset
            For X As Integer = 0 To OutputImage.Width - 1
                Dim DispVal As Integer = CInt((Data(X, Y) - MinData) * (255 / (MaxData - MinData)))
                Dim Coloring As Drawing.Color = cColorMaps.ColorByMap(DispVal, ColorMap)
                OutputImage.Pixels(BaseOffset) = Coloring.R
                OutputImage.Pixels(BaseOffset + 1) = Coloring.G
                OutputImage.Pixels(BaseOffset + 2) = Coloring.B
                BaseOffset += BytePerPixel
            Next X
            YOffset += Stride
        Next Y
        OutputImage.UnlockBits()
        pbeMain.Image = OutputImage.BitmapToProcess
        Me.Text = "Focus Window <" & (Data.GetUpperBound(0) + 1).ValRegIndep & "x" & (Data.GetUpperBound(1) + 1).ValRegIndep & "> pixel "
        DataAnalysis(Data)
    End Sub

    Public Sub DataAnalysis(ByRef Data(,) As UInt32)
        Dim Pixel As Long = Data.LongLength
        If Pixel <= 10000 Then
            Dim SortedData As New List(Of UInt32)
            Dim Total As UInt64 = 0
            For X As Integer = 0 To Data.GetUpperBound(0)
                For Y As Integer = 0 To Data.GetUpperBound(1)
                    SortedData.Add(Data(X, Y))
                    Total += Data(X, Y)
                Next Y
            Next X
            SortedData.Sort() : SortedData.Reverse()
            Dim SumPoints As New Dictionary(Of Integer, UInt64)
            SumPoints.Add(4, 0)
            SumPoints.Add(9, 0)
            SumPoints.Add(16, 0)
            SumPoints.Add(25, 0)
            SumPoints.Add(36, 0)
            SumPoints.Add(49, 0)
            SumPoints.Add(64, 0)
            SumPoints.Add(81, 0)
            SumPoints.Add(100, 0)
            Dim SumFromMax As UInt64 = 0
            For Idx As Integer = 0 To SortedData.Count - 1
                SumFromMax += SortedData(Idx)
                If SumPoints.ContainsKey(Idx + 1) Then SumPoints(Idx + 1) = SumFromMax
            Next Idx
            Dim OutText As New List(Of String)
            OutText.Add("Total: <" & Total.ValRegIndep & ">")
            For Each Key As Integer In SumPoints.Keys
                Dim Percentage As Double = 100 * (SumPoints(Key) / Total)
                Dim PercentageText As String = Percentage.ValRegIndep("0.00") & " %"
                OutText.Add(("Max <" & Key.ValRegIndep & "> pixel: ").PadRight(20) & SumPoints(Key).ValRegIndep.PadLeft(7) & " = " & PercentageText)
            Next Key
            tbFocus.Text = Join(OutText.ToArray, System.Environment.NewLine)
        Else
            tbFocus.Text = "Too much data ..."
        End If
    End Sub

End Class