Public Class Form1
    Public QueOfNewCmds As New ArrayList


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Sub GetNewCommands()
        Static NextPollTime As DateTime = DateTime.Now
        Const NextPollTimeIncrement As Double = 7  'in seconds
        Dim CmdList As System.Collections.Generic.List(Of Marathon.LoggerHost.Utility.CommandInfo) = New System.Collections.Generic.List(Of Marathon.LoggerHost.Utility.CommandInfo)

        Try
            If NextPollTime <= DateTime.Now Then
                'Debug.Print(NextPollTime.ToString & "'  " & Now.ToString)
                Dim CmdTimer As DateTime = Date.Now
                CmdList = Marathon.LoggerHost.QueueController.GetNextCommandBatch
                If Not IsNothing(CmdList) Then
                    'dqLog.WriteProcessLog("clsPool-ProcessLoop", "CMD, TimeOf`Day.TotalSeconds= ," & Now.TimeOfDay.TotalSeconds)
                    For Each tmpObj As Marathon.LoggerHost.Utility.CommandInfo In CmdList
                        If Not IsNothing(tmpObj) Then
                            If tmpObj.Result.Success Then
                                If Not IsNothing(tmpObj.TargetLogger) Then
                                    'add to cue
                                    QueOfNewCmds.Add(tmpObj)
                                    Marathon.LoggerHost.QueueController.CommandReceived(tmpObj.CommandId) 'acknowledge command
                                    Debug.Print(tmpObj.Command.ToString & ", " & tmpObj.TargetLogger.Settings.IPAddress)
                                End If
                            Else
                                'dqLog.WriteProcessLog("clsPool-ProcessLoop", "CMD - Obj isNothing, TimeOfDay.TotalSeconds= ," & Now.TimeOfDay.TotalSeconds)
                                Debug.Print("clsPool-ProcessLoop", "CMD - Obj isNothing, TimeOfDay.TotalSeconds= ," & Now.TimeOfDay.TotalSeconds)
                            End If
                        End If
                    Next
                End If
                Dim tmpTS As TimeSpan = TimeSpan.FromSeconds(NextPollTimeIncrement)
                NextPollTime = DateTime.Now + tmpTS
                'Debug.Print("NextPollTime= " & NextPollTime.ToString)
            End If
        Catch ex As Exception
            MsgBox("Error getting Commands")
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GetNewCommands()

    End Sub
End Class
