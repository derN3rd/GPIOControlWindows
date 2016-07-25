Imports System.Net
Imports System.IO

Public Class Form1
    Dim gpioIPbox As Object
    Dim gpioIP As String

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        Dim tempip As String
        If gpioIP Is "" Or IsNothing(gpioIP) Then
            tempip = "0.0.0.0"
        Else
            tempip = gpioIP
        End If
        gpioIPbox = InputBox("Bitte gebe die IP von RPi ein (z.B.: 192.168.2.123)", "GPIO Config", tempip)
        If Not (gpioIPbox Is "") Then
            GPIOControl.My.Settings.gpioIP = gpioIPbox
            GPIOControl.My.Settings.Save()
            gpioIP = gpioIPbox.ToString
        End If
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If Not (GPIOControl.My.Settings.gpioIP Is "" Or IsNothing(GPIOControl.My.Settings.gpioIP)) Then
            gpioIP = GPIOControl.My.Settings.gpioIP
        End If
    End Sub

    Public Function doGpioRequest(device As Integer, status As Integer) As Integer
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create("http://" + gpioIP + "/" + device.ToString + "/" + status.ToString)
            request.Method = "GET"

            request.BeginGetResponse(New AsyncCallback(AddressOf ResponseRecent), request)
            Return 1
        Catch ex As Exception
            Label1.Text = "Oh, da lief was schief"
            Return 0
        End Try
        
    End Function

    Private Sub ResponseRecent(ByVal asynchronousResult As IAsyncResult)
        Try
            Dim webRequest As HttpWebRequest = DirectCast(asynchronousResult.AsyncState, HttpWebRequest)
            Dim webResponse As HttpWebResponse = webRequest.EndGetResponse(asynchronousResult)
            Dim stream As New StreamReader(webResponse.GetResponseStream())
            Dim responseString = stream.ReadToEnd
            Label1.Text = ""
        Catch ex As Exception
            Label1.Text = "Oh, da lief was schief"
        End Try
        

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        doGpioRequest(1, 1)
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        doGpioRequest(1, 0)
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        doGpioRequest(2, 1)
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        doGpioRequest(2, 0)
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        doGpioRequest(3, 1)
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        doGpioRequest(3, 0)
    End Sub
End Class
