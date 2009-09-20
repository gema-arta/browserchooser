﻿Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Net


Public Class frmMain

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure MARGINS
        Public cxLeftWidth As Integer
        Public cxRightWidth As Integer
        Public cyTopHeight As Integer
        Public cyButtomheight As Integer
    End Structure

    <DllImport("dwmapi.dll")> _
    Public Shared Function DwmExtendFrameIntoClientArea(ByVal hWnd As IntPtr, ByRef pMarinset As MARGINS) As Integer
    End Function

    Private Sub LaunchBrowserInfo(ByVal browserNumber As Integer)
        If BrowserConfig.ShowUrl = True And strUrl <> "" Then
            Me.Text = "Open " & BrowserConfig.GetBrowser(browserNumber).Name & " - " & strUrl
        Else
            Me.Text = "Open " & BrowserConfig.GetBrowser(browserNumber).Name
        End If
    End Sub

    Private Sub LaunchBrowserAndClose(ByVal browserNumber As Integer)
        If (Not LaunchBrowser(browserNumber)) Then
            MsgBox("The target browser does not exist in the target location.", MsgBoxStyle.Critical)
        Else
            Me.Close()
        End If
    End Sub


    Private Sub btnInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInfo.Click
        About.ShowDialog()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()

    End Sub

    Private Sub btnApp1_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApp1.MouseHover, btnApp1.MouseEnter
        LaunchBrowserInfo(1)
    End Sub

    Private Sub btnApp1_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApp1.MouseLeave
        If BrowserConfig.ShowUrl = True And strUrl <> "" Then
            Me.Text = DefaultMessage & " - " & strUrl
        Else
            Me.Text = DefaultMessage
        End If
    End Sub

    Private Sub btnApp2_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApp2.MouseHover, btnApp2.MouseEnter
        LaunchBrowserInfo(2)
    End Sub

    Private Sub btnApp2_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApp2.MouseLeave
        If BrowserConfig.ShowUrl = True And strUrl <> "" Then
            Me.Text = DefaultMessage & " - " & strUrl
        Else
            Me.Text = DefaultMessage
        End If
    End Sub

    Private Sub btnApp3_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApp3.MouseHover, btnApp3.MouseEnter
        LaunchBrowserInfo(3)
    End Sub

    Private Sub btnApp3_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApp3.MouseLeave
        If BrowserConfig.ShowUrl = True And strUrl <> "" Then
            Me.Text = DefaultMessage & " - " & strUrl
        Else
            Me.Text = DefaultMessage
        End If
    End Sub

    Private Sub btnApp4_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApp4.MouseHover, btnApp4.MouseEnter
        LaunchBrowserInfo(4)
    End Sub

    Private Sub btnApp4_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApp4.MouseLeave
        If BrowserConfig.ShowUrl = True And strUrl <> "" Then
            Me.Text = DefaultMessage & " - " & strUrl
        Else
            Me.Text = DefaultMessage
        End If
    End Sub

    Private Sub btnApp5_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApp5.MouseHover, btnApp5.MouseEnter
        LaunchBrowserInfo(5)
    End Sub

    Private Sub btnApp5_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApp5.MouseLeave
        If BrowserConfig.ShowUrl = True And strUrl <> "" Then
            Me.Text = DefaultMessage & " - " & strUrl
        Else
            Me.Text = DefaultMessage
        End If
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        On Error Resume Next
        Dim margins As MARGINS = New MARGINS
        margins.cxLeftWidth = -1
        margins.cxRightWidth = -1
        margins.cyTopHeight = -1
        margins.cyButtomheight = -1
        'set all the four value -1 to apply glass effect to the whole window
        'set your own value to make specific part of the window glassy.
        Dim hwnd As IntPtr = Me.Handle
        Dim result As Integer = DwmExtendFrameIntoClientArea(hwnd, margins)

        InitializeMain()
    End Sub

    Private Function SetImage(ByRef BrowserChoice As Browser) As Image
        Select Case BrowserChoice.Image
            Case "Firefox"
                SetImage = My.Resources.Firefox
            Case "Internet Explorer"
                SetImage = My.Resources.InternetExplorer
            Case "Google Chrome"
                SetImage = My.Resources.GoogleChrome
            Case "Opera"
                SetImage = My.Resources.Opera
            Case "Safari"
                SetImage = My.Resources.Safari
            Case Else
                If (Not String.IsNullOrEmpty(BrowserChoice.CustomImagePath)) Then
                    'handles absolute or relative paths, 
                    'Path.Combine(path1, path2): If path2 contains an absolute path, this method returns path2
                    Dim cImage As FileInfo = New FileInfo(Path.Combine(Application.StartupPath, BrowserChoice.CustomImagePath))
                    If (cImage.Exists) Then
                        Try
                            Select Case cImage.Extension.ToUpper
                                Case ".PNG"
                                    SetImage = Bitmap.FromFile(cImage.FullName)
                                Case ".ICO"
                                    SetImage = New Icon(cImage.FullName, New Size(75, 80)).ToBitmap()
                                Case Else
                                    'unexpected file format - set icon to error
                                    SetImage = My.Resources._53
                            End Select
                        Catch ex As Exception
                            'file specified not a valid image format - set icon to error
                            SetImage = My.Resources._53
                        End Try
                    Else
                        'file doesn't exist - set icon to error
                        SetImage = My.Resources._53
                    End If
                Else
                    'custom option chosen but no file specified - set icon to error
                    SetImage = My.Resources._53
                End If
        End Select
    End Function


    Public Sub InitializeMain()
        If (BrowserConfig Is Nothing OrElse BrowserConfig.Browsers.Count = 0) Then
            'Force open Options screen
#If DEBUG Then
            ' this switch allows us to debug Options without having to attach to process due to UAC
            Options.ShowDialog()
#Else
            openOptions()
#End If


        Else

            If (BrowserConfig.GetBrowser(1).IsActive) Then
                Me.Width = (1 * 81) + 112
                btnApp1.Visible = True
                btnApp1.Image = SetImage(BrowserConfig.GetBrowser(1))
            Else
                btnApp1.Visible = False
            End If

            If (BrowserConfig.GetBrowser(2).IsActive) Then
                Me.Width = (2 * 81) + 112
                btnApp2.Visible = True
                btnApp2.Image = SetImage(BrowserConfig.GetBrowser(2))
            Else
                btnApp2.Visible = False
            End If

            If (BrowserConfig.GetBrowser(3).IsActive) Then
                Me.Width = (3 * 81) + 112
                btnApp3.Visible = True
                btnApp3.Image = SetImage(BrowserConfig.GetBrowser(3))
            Else
                btnApp3.Visible = False
            End If

            If (BrowserConfig.GetBrowser(4).IsActive) Then
                Me.Width = (4 * 81) + 112
                btnApp4.Visible = True
                btnApp4.Image = SetImage(BrowserConfig.GetBrowser(4))
            Else
                btnApp4.Visible = False
            End If

            If (BrowserConfig.GetBrowser(5).IsActive) Then
                Me.Width = (5 * 81) + 112
                btnApp5.Visible = True
                btnApp5.Image = SetImage(BrowserConfig.GetBrowser(5))
            Else
                btnApp5.Visible = False
            End If

        End If

        If BrowserConfig.AutoUpdateCheck = True Then
            Dim ts As TimeSpan = New TimeSpan(DateTime.Now.Ticks - BrowserConfig.LastUpdateCheck.Ticks)
            If (ts.Days >= DaysBetweenUpdateCheck) Then
                CheckforUpdate("")
            End If
        End If

        btnOptions.Location = New Point(Me.Width - 33, 12)

        'Load url from parameter
        For i = 0 To My.Application.CommandLineArgs.Count - 1
            strUrl = My.Application.CommandLineArgs.Item(i).ToString
        Next i

        If BrowserConfig.ShowUrl = True And strUrl <> "" Then
            Me.Text = DefaultMessage & " - " & strUrl
        Else
            Me.Text = DefaultMessage
        End If

        'If no URL is passed in, don't display context menu
        If (String.IsNullOrEmpty(strUrl)) Then
            btnApp1.ContextMenuStrip = Nothing
            btnApp2.ContextMenuStrip = Nothing
            btnApp3.ContextMenuStrip = Nothing
            btnApp4.ContextMenuStrip = Nothing
            btnApp5.ContextMenuStrip = Nothing
        End If

        'Set up Tooltips
        If BrowserConfig.ShowUrl = True And strUrl <> "" Then
            btn1TT.SetToolTip(btnApp1, "Open " & strUrl & " in " & BrowserConfig.GetBrowser(1).Name & "." & vbCrLf & "Hotkeys: (1) or (" & BrowserConfig.GetBrowser(1).Name.Substring(0, 1) & ").")
            btn2TT.SetToolTip(btnApp2, "Open " & strUrl & " in " & BrowserConfig.GetBrowser(2).Name & "." & vbCrLf & "Hotkeys: (2) or (" & BrowserConfig.GetBrowser(2).Name.Substring(0, 1) & ").")
            btn3TT.SetToolTip(btnApp3, "Open " & strUrl & " in " & BrowserConfig.GetBrowser(3).Name & "." & vbCrLf & "Hotkeys: (3) or (" & BrowserConfig.GetBrowser(3).Name.Substring(0, 1) & ").")
            btn4TT.SetToolTip(btnApp4, "Open " & strUrl & " in " & BrowserConfig.GetBrowser(4).Name & "." & vbCrLf & "Hotkeys: (4) or (" & BrowserConfig.GetBrowser(4).Name.Substring(0, 1) & ").")
            btn5TT.SetToolTip(btnApp5, "Open " & strUrl & " in " & BrowserConfig.GetBrowser(5).Name & "." & vbCrLf & "Hotkeys: (5) or (" & BrowserConfig.GetBrowser(5).Name.Substring(0, 1) & ").")
        Else
            btn1TT.SetToolTip(btnApp1, "Open " & BrowserConfig.GetBrowser(1).Name & "." & vbCrLf & "Hotkeys: (1) or (" & BrowserConfig.GetBrowser(1).Name.Substring(0, 1) & ").")
            btn2TT.SetToolTip(btnApp2, "Open " & BrowserConfig.GetBrowser(2).Name & "." & vbCrLf & "Hotkeys: (2) or (" & BrowserConfig.GetBrowser(2).Name.Substring(0, 1) & ").")
            btn3TT.SetToolTip(btnApp3, "Open " & BrowserConfig.GetBrowser(3).Name & "." & vbCrLf & "Hotkeys: (3) or (" & BrowserConfig.GetBrowser(3).Name.Substring(0, 1) & ").")
            btn4TT.SetToolTip(btnApp4, "Open " & BrowserConfig.GetBrowser(4).Name & "." & vbCrLf & "Hotkeys: (4) or (" & BrowserConfig.GetBrowser(4).Name.Substring(0, 1) & ").")
            btn5TT.SetToolTip(btnApp5, "Open " & BrowserConfig.GetBrowser(5).Name & "." & vbCrLf & "Hotkeys: (5) or (" & BrowserConfig.GetBrowser(5).Name.Substring(0, 1) & ").")
        End If
    End Sub

    Private Sub btnInfo_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInfo.MouseEnter, btnInfo.MouseHover
        Me.Text = "About"
    End Sub

    Private Sub btnInfo_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInfo.MouseLeave
        Me.Text = DefaultMessage
    End Sub

    Private Sub btnOptions_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOptions.MouseEnter, btnOptions.MouseHover
        Me.Text = "Options"
    End Sub

    Private Sub btnOptions_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOptions.MouseLeave
        Me.Text = DefaultMessage
    End Sub

    Private Sub btnClose_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Text = "Close"
    End Sub

    Private Sub btnClose_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Text = DefaultMessage
    End Sub

    Private Sub btnOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOptions.Click
#If DEBUG Then
        Options.ShowDialog()
#Else
        openOptions()
#End If
    End Sub

    Private Sub openOptions()
        Dim myProcess As New Process
        myProcess.StartInfo.UseShellExecute = True
        myProcess.StartInfo.Verb = "runas"
        myProcess.StartInfo.FileName = Application.ExecutablePath
        myProcess.StartInfo.Arguments = "gooptions"
        myProcess.Start()
        System.Environment.Exit(-1)
    End Sub


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()

    End Sub

    Private Sub btnApp1_Click(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles btnApp1.MouseClick
        If (e.Button = Windows.Forms.MouseButtons.Left) Then
            LaunchBrowserAndClose(1)
        End If
    End Sub

    Private Sub btnApp2_Click(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles btnApp2.MouseClick
        If (e.Button = Windows.Forms.MouseButtons.Left) Then
            LaunchBrowserAndClose(2)
        End If
    End Sub

    Private Sub btnApp3_Click(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles btnApp3.MouseClick
        If (e.Button = Windows.Forms.MouseButtons.Left) Then
            LaunchBrowserAndClose(3)
        End If
    End Sub

    Private Sub btnApp4_Click(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles btnApp4.MouseClick
        If (e.Button = Windows.Forms.MouseButtons.Left) Then
            LaunchBrowserAndClose(4)
        End If
    End Sub

    Private Sub btnApp5_Click(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles btnApp5.MouseClick
        If (e.Button = Windows.Forms.MouseButtons.Left) Then
            LaunchBrowserAndClose(5)
        End If
    End Sub



    Private Sub frmMain_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Dim firstChar As String = e.KeyData.ToString()

        If BrowserConfig.GetBrowser(1).IsActive = True AndAlso (e.KeyCode = Keys.D1 Or BrowserConfig.GetBrowser(1).Name.StartsWith(firstChar, StringComparison.InvariantCultureIgnoreCase)) Then
            LaunchBrowserAndClose(1)
        ElseIf BrowserConfig.GetBrowser(2).IsActive = True AndAlso (e.KeyCode = Keys.D2 Or BrowserConfig.GetBrowser(2).Name.StartsWith(firstChar, StringComparison.InvariantCultureIgnoreCase)) Then
            LaunchBrowserAndClose(2)
        ElseIf BrowserConfig.GetBrowser(3).IsActive = True AndAlso (e.KeyCode = Keys.D3 Or BrowserConfig.GetBrowser(3).Name.StartsWith(firstChar, StringComparison.InvariantCultureIgnoreCase)) Then
            LaunchBrowserAndClose(3)
        ElseIf BrowserConfig.GetBrowser(4).IsActive = True AndAlso (e.KeyCode = Keys.D4 Or BrowserConfig.GetBrowser(4).Name.StartsWith(firstChar, StringComparison.InvariantCultureIgnoreCase)) Then
            LaunchBrowserAndClose(4)
        ElseIf BrowserConfig.GetBrowser(5).IsActive = True AndAlso (e.KeyCode = Keys.D5 Or BrowserConfig.GetBrowser(5).Name.StartsWith(firstChar, StringComparison.InvariantCultureIgnoreCase)) Then
            LaunchBrowserAndClose(5)
        End If
    End Sub

    Public Sub CheckforUpdate(ByVal strMode As String)

        Try
            Dim client As WebClient = New WebClient()

            Dim strWebVersion As String
            'Switch for Portable
            If (PortableMode) Then
                strWebVersion = client.DownloadString("http://www.janolepeek.com/bcport.txt")
            Else
                strWebVersion = client.DownloadString("http://www.janolepeek.com/bclatest.txt")
            End If

            Dim version As System.Version = New System.Version(strWebVersion)
            If version.CompareTo(My.Application.Info.Version) > -1 Then

                If MsgBox("A new version of Browser Checker is available. Would you like to download it now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                    Download.ShowDialog()

                End If
            Else
                If strMode = "verbose" Then
                    MsgBox("You are running the current version of Browser Chooser!", MsgBoxStyle.Information)
                End If
            End If

            BrowserConfig.LastUpdateCheck = DateTime.Now
            Options.SaveConfig()
        Catch ex As Exception
            If strMode = "verbose" Then
                MsgBox("There was an error checking for the latest version." & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical)
            End If
        End Try

    End Sub

    Private Sub AddUrlToAutoOpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddUrlToAutoOpenToolStripMenuItem.Click
        If (Not String.IsNullOrEmpty(strUrl)) Then
            'create a new URI object in order to parse out the server portion
            'given a url like: http://www.google.com/test/browser.aspx?a=3&c=6, uri.Host should equal www.google.com
            Dim uri As UriBuilder = Nothing
            Try
                uri = New UriBuilder(strUrl)
            Catch ex As Exception
                MsgBox(String.Format("Error reading {0} as a valid URL.{1}{2}", strUrl, vbCrLf, ex.Message, MsgBoxStyle.Critical))
            End Try

            'get the browser button/picture box that triggered the context menu
            Dim cms As ContextMenuStrip = Nothing
            Dim browserButton As PictureBox = Nothing
            Try
                cms = DirectCast(sender, ToolStripMenuItem).Owner
                browserButton = DirectCast(cms.SourceControl, PictureBox)
            Catch ex As Exception
                MsgBox(String.Format("Unexpected Context Menu Error!{0}{1}", vbCrLf, ex.Message, MsgBoxStyle.Critical))
            End Try

            If (uri IsNot Nothing AndAlso browserButton IsNot Nothing) Then
                Select Case browserButton.Name
                    Case "btnApp1"
                        BrowserConfig.GetBrowser(1).Urls.Add(uri.Host)
                    Case "btnApp2"
                        BrowserConfig.GetBrowser(2).Urls.Add(uri.Host)
                    Case "btnApp3"
                        BrowserConfig.GetBrowser(3).Urls.Add(uri.Host)
                    Case "btnApp4"
                        BrowserConfig.GetBrowser(4).Urls.Add(uri.Host)
                    Case "btnApp5"
                        BrowserConfig.GetBrowser(5).Urls.Add(uri.Host)
                    Case Else
                        MsgBox(String.Format("Browser Not Configured For Auto Add Url.{0}Name={1}", vbCrLf, browserButton.Name, MsgBoxStyle.Information))
                End Select

                'save additions to the url list
                Options.SaveConfig()

                'not sure if this UAC approach is needed here but added it for consistency
                Process.Start(Application.ExecutablePath, strUrl)
                System.Environment.Exit(-1)

            End If
        End If
    End Sub
End Class

