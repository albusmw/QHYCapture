Option Explicit On
Option Strict On

'''<summary>Form to realize a timed cooling.</summary>
Public Class frmCooling

    Private Sub frmCooling_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Cooling for " & M.DB.UsedCameraId.ToString
    End Sub

    Private Sub tQuery_Tick(sender As Object, e As EventArgs) Handles tQuery.Tick

        'Get a timed cooling if configured
        Dim TimedCool As Double = GetTimedTemp()
        If Double.IsNaN(TimedCool) = False Then M.DB.Temp_Target = TimedCool

        'Get current camera parameters
        Dim CurrentTemp As Double = QHY.QHY.GetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_CURTEMP)
        Dim TargetTemp As Double = QHY.QHY.GetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_COOLER)
        Dim CurrentPWM As Double = QHY.QHY.GetQHYCCDParam(M.DB.CamHandle, QHYCamera.QHY.CONTROL_ID.CONTROL_CURPWM)

        tbT_measured.Text = CurrentTemp.ValRegIndep("#0.0")
        tbT_set.Text = TargetTemp.ValRegIndep("#0.0")
        tbCoolerPWM.Text = CurrentPWM.ValRegIndep("#0")
        Dim Delta As Double = CurrentTemp - TargetTemp
        Dim DeltaDisp As Integer = CInt(Math.Abs(Delta * 10))
        If DeltaDisp > 100 Then DeltaDisp = 100
        pbDelta.Value = DeltaDisp
        If Delta > 0 Then
            pbDelta.ForeColor = Color.Red
        Else
            pbDelta.ForeColor = Color.Blue
        End If
    End Sub

    Private Sub btnSetTemp_Click(sender As Object, e As EventArgs) Handles btnSetTemp.Click
        Dim NewTemp As Double
        If Double.TryParse(InputBox("New target: ", "New temperature", M.DB.Temp_Target.ValRegIndep), NewTemp) = True Then M.DB.Temp_Target = NewTemp
    End Sub

    '''<summary>Get a configured timed cooling.</summary>
    Private Function GetTimedTemp() As Double

        Dim RetVal As Double = Double.NaN
        Dim Forma As IFormatProvider = Nothing
        Dim MinutesOnly As String = "mm"

        '0.) Parse times
        Try
            lbTimings.Text = Now.ValRegIndep & " -> " & DateTime.Parse(tbStartTime.Text).ValRegIndep & " -> " & DateTime.Parse(tbStartTime.Text).AddMinutes(Val(tbStartTime2.Text.Replace(",", "."))).ValRegIndep
        Catch ex As Exception
            lbTimings.Text = "Timing can not be parsed"
        End Try

        '1.) Initial cooling
        If cbStartTime.Checked = True Then
            Dim StartTime As DateTime = Nothing
            Dim TimedCool As Double = Double.NaN
            If DateTime.TryParse(tbStartTime.Text, StartTime) = True Then
                If Now > StartTime Then
                    If Double.TryParse(tbTimedCool.Text, TimedCool) = True Then
                        RetVal = TimedCool
                    End If
                End If
            End If
            '2.) Final cooling
            If cbStartTime2.Checked = True Then
                Dim TimeSpan2 As Double = Double.NaN
                Dim StartTime2 As DateTime = Nothing
                Dim TimedCool2 As Double = Double.NaN
                TimeSpan2 = Val(tbStartTime2.Text.Replace(",", "."))
                If Now > StartTime.AddMinutes(TimeSpan2) Then
                    If Double.TryParse(tbTimedCool2.Text, TimedCool2) = True Then
                        RetVal = TimedCool2
                    End If
                End If
            End If
        End If

        Return RetVal
    End Function

End Class