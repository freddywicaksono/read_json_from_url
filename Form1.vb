Imports System.Text
Imports System.Xml
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports System.IO

Public Class Form1
    Private responseContent As String = ""
    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim postdata As String
        postdata = "username=" & txtUsername.Text & "&password=" & txtPassword.Text
        txtResponse.Text = WRequest("YOUR_JSON_URL", "POST", postdata)
        GetData()
    End Sub

    Private Sub GetData()
        Dim jsonResulttodict = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(txtResponse.Text)
        Dim wStatus = jsonResulttodict.Item("status")
        Dim wnama As String
        If (wStatus = 1) Then
            wnama = jsonResulttodict.Item("nama")
            txtData.Text = wnama
        Else
            txtData.Text = "Not Valid"
        End If

    End Sub

    Function WRequest(ByVal URL As String, ByVal method As String, ByVal POSTdata As String) As String
        Dim responseData As String = ""
        Try
            Dim cookieJar As New Net.CookieContainer()
            Dim hwrequest As Net.HttpWebRequest = Net.WebRequest.Create(URL)
            hwrequest.CookieContainer = cookieJar
            hwrequest.Accept = "*/*"
            hwrequest.AllowAutoRedirect = True
            hwrequest.UserAgent = "http_requester/0.1"
            hwrequest.Timeout = 60000
            hwrequest.Method = method
            If hwrequest.Method = "POST" Then
                hwrequest.ContentType = "application/x-www-form-urlencoded"
                Dim encoding As New ASCIIEncoding() 'Use UTF8Encoding for XML requests
                Dim postByteArray() As Byte = encoding.GetBytes(POSTdata)
                hwrequest.ContentLength = postByteArray.Length
                Dim postStream As IO.Stream = hwrequest.GetRequestStream()
                postStream.Write(postByteArray, 0, postByteArray.Length)
                postStream.Close()
            End If
            Dim hwresponse As Net.HttpWebResponse = hwrequest.GetResponse()
            If hwresponse.StatusCode = Net.HttpStatusCode.OK Then
                Dim responseStream As IO.StreamReader = _
                  New IO.StreamReader(hwresponse.GetResponseStream())
                responseData = responseStream.ReadToEnd()
            End If
            hwresponse.Close()
        Catch e As Exception
            responseData = "An error occurred: " & e.Message
        End Try
        Return responseData
    End Function
End Class
