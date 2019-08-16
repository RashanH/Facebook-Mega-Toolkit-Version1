Imports System.Net
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports MSHTML

Public Class MainForm
    Public MyUid As String

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim autoshell = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Internet Explorer\Main", True)
        autoshell.SetValue("Display Inline Images", "yes")
        autoshell.Close()
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Application.EnableVisualStyles()
        '  System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 2")
        Dim autoshell = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Internet Explorer\Main", True)
        autoshell.SetValue("Display Inline Images", "no")
        autoshell.Close()

        ListView1.View = View.Details
        ListView1.FullRowSelect = True
        ListView1.SmallImageList = ImageList1
        ListView1.Columns.Add("", 36, HorizontalAlignment.Center)          'UID
        ListView1.Columns.Add("Name", 169, HorizontalAlignment.Left) 'Add column 2
        ListView1.Columns.Add("Action", 85, HorizontalAlignment.Center) 'Add column 3
        ListView1.Columns.Add("", 75, HorizontalAlignment.Center)
        ListView1.Columns.Add("", 30, HorizontalAlignment.Center)

        Me.ListView1.Font = New System.Drawing.Font("Segoe UI", 8, FontStyle.Regular)

    End Sub

    Private Sub ListView1_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles ListView1.ColumnWidthChanged
        'Static FireMe As Boolean = True
        'If FireMe = True Then
        '    FireMe = False
        '    ListView1.Columns(0).Width = 0
        '    ListView1.Columns(1).Width = 180
        '    ListView1.Columns(2).Width = 85
        '    ListView1.Columns(3).Width = 75
        '    ListView1.Columns(4).Width = 55
        '    FireMe = True
        'End If
    End Sub

    Private Sub ListView1_ColumnWidthChanging(sender As Object, e As ColumnWidthChangingEventArgs) Handles ListView1.ColumnWidthChanging
        '  e.Cancel = True
    End Sub



    Private Sub ListView1_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles ListView1.DrawColumnHeader
        e.DrawDefault = True  ' let System draw this element
    End Sub

    Private Sub ListView1_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles ListView1.DrawItem
     
        '  e.DrawDefault = True

        If Not (e.State And ListViewItemStates.Selected) = 0 Then

            ' Draw the background for a selected item.
            e.Graphics.FillRectangle(Brushes.White, e.Bounds)
            e.DrawFocusRectangle()
            ' e.Graphics.DrawImage(
        End If
        e.Item.ForeColor = Color.White


      
    End Sub

    Private Sub ListView1_DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs) Handles ListView1.DrawSubItem
        Try
            If Not e.Item.Selected = True Then
                e.DrawDefault = True
            Else
                '      e.DrawText(TextFormatFlags.VerticalCenter)

                '   e.DrawDefault = True

                '    e.DrawText(TextFormatFlags.VerticalCenter)
                e.DrawText(TextFormatFlags.VerticalCenter)
                e.Graphics.DrawImage(Me.ListView1.SmallImageList.Images(e.Item.Text), Me.ListView1.GetItemRect(e.ItemIndex, ItemBoundsPortion.Icon))


                '  e.DrawText(TextFormatFlags.NoPadding & TextFormatFlags.VerticalCenter)
               
            End If
        Catch ex As Exception

        End Try
        '  e.DrawDefault = True




    End Sub



    Private Sub ButtonX1_Click(sender As Object, e As EventArgs) Handles ButtonX1.Click
        WebBrowser1.Navigate("https://mbasic.facebook.com/friends/center/friends/?ppk=0")
        ButtonX1.Visible = False
        lblFriendlistCount.Visible = True
        H2X1.Visible = True
        ProgressBar1.Visible = True

        ButtonX2.Visible = False
        RichTextBox1.Visible = False
        FirefoxRadioButton1.Visible = False
        FirefoxRadioButton2.Visible = False
        NumericUpDownX1.Visible = False
        Label2.Visible = False
        'disabele controls
    End Sub


    Public ii As Integer = 0

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        If WebBrowser1.DocumentText.Contains("Who can see your friends list?") Then 'ON FRIENDLIST PAge
            If WebBrowser1.DocumentText.Contains("uid=") Then

                Dim pattern1 As String = "uid=(.*?)&"
                Dim pattern2 As String = "acted"">(.*?)</a"
                Dim pattern3 As String = "src\s*=\s*""([^""]+)"""
                For Each i As Match In Regex.Matches(WebBrowser1.DocumentText, pattern1)
                    ListBox1.Items.Add(i.Groups(1).Value)

               
                Next


              
                For Each i As Match In Regex.Matches(WebBrowser1.DocumentText, pattern2, RegexOptions.Singleline + RegexOptions.IgnoreCase)
                    lstNames.Items.Add(i.Groups(1).Value.Replace(vbCrLf, " ").Replace("        ", " "))
                Next

                Dim myRegex As New Regex(pattern3, RegexOptions.None)
                For Each myMatch As Match In myRegex.Matches(WebBrowser1.DocumentText)
                    If myMatch.Success Then
                        If myMatch.Value.ToString.Contains("p50x50") Then
                            Dim x = myMatch.Value.Replace("src=", "").Replace("""", "")
                            lstDPs.Items.Add(x)
                        End If
                    End If
                Next

                ii = ii + 1
                WebBrowser1.Navigate("https://mbasic.facebook.com/friends/center/friends/?ppk=" & ii)
            Else
                'Friend list extraction complete

                If Not BackgroundWorker2.IsBusy Then
                    BackgroundWorker2.RunWorkerAsync()
                End If

                ProgressBar1.Visible = False
                SubTabControlX2.SelectedIndex = 1
                If FirefoxRadioButton1.Checked = True Then
                    WebBrowser2.Navigate("http://mbasic.facebook.com/me")
                Else

                    ListBox2.Items.AddRange(Split(RichTextBox1.Text, vbLf))
                    lblPostIDsCount.Text = ListBox2.Items.Count
                End If
                ListBox2.Items.Clear()
                ButtonX2.Visible = False
                RichTextBox1.Visible = False
                FirefoxRadioButton1.Visible = False
                FirefoxRadioButton2.Visible = False
                NumericUpDownX1.Visible = False
                Label2.Visible = False
                lblPostIDsCount.Visible = True
                H2X2.Visible = True
            End If


        End If
        lblFriendlistCount.Text = ListBox1.Items.Count
    End Sub


    Private Sub FirefoxRadioButton1_CheckedChanged(sender As Object, e As EventArgs)
        If FirefoxRadioButton1.Checked = True Then
            NumericUpDownX1.Enabled = True
        Else
            NumericUpDownX1.Enabled = False
        End If
    End Sub

    Private Sub RichTextBox1_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs)

    End Sub



    Private Sub WebBrowser2_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser2.DocumentCompleted
        Dim pattern As String = "/(.*?)/allactivity"                 'MOVE To login page
        For Each i As Match In Regex.Matches(WebBrowser2.DocumentText, pattern)
            MyUid = i.Groups(1).Value
        Next
        If WebBrowser2.DocumentText.Contains("m-timeline-cover-section") Then 'sure user on profile page
            If FirefoxRadioButton1.Checked = True Then

                Dim pattern2 As String = "ft_id=(.*?)&"
                For Each i2 As Match In Regex.Matches(WebBrowser2.DocumentText, pattern2)
                    ListBox2.Items.Add(i2.Groups(1).Value)
                Next
                If Not NumericUpDownX1.Value >= ListBox2.Items.Count Then
                    While ListBox2.Items.Count > NumericUpDownX1.Value
                        ListBox2.Items.RemoveAt(ListBox2.Items.Count - 1)
                    End While
                    'finished
                    SubTabControlX2.SelectedIndex = 2
                    lblPostIDsCount.Text = ListBox2.Items.Count
                    H2X3.Text = "likes obtained from " & lblPostIDsCount.Text & " posts" & "..." & vbCrLf & " Please wait..."
                    If Not ListBox2.Items.Count <= 0 Then
                        CurrentPost = 0
                        WebBrowser3.Navigate("https://m.facebook.com/ufi/reaction/profile/browser/fetch/?limit=999999999999999999&total_count=999999999999999999&ft_ent_identifier=" & ListBox2.Items(CurrentPost).ToString, Nothing, Nothing, "User-Agent: Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/1.0.154.53 Safari/525.19")
                    End If
                Else
                    'click see more
                    'If WebBrowser2.DocumentText.Contains("Show more") Then
                    For Each element As HtmlElement In WebBrowser2.Document.GetElementsByTagName("a")
                        If element.OuterHtml.Contains("Show more") Then
                            element.InvokeMember("click")
                            Return
                        End If
                        lblPostIDsCount.Text = ListBox2.Items.Count
                    Next
                    If Not WebBrowser2.DocumentText.Contains("Show more") Then
                        'finished with skipping
                        SubTabControlX2.SelectedIndex = 2
                        lblPostIDsCount.Text = ListBox2.Items.Count
                        H2X3.Text = "likes obtained from " & lblPostIDsCount.Text & " posts" & "..." & vbCrLf & " Please wait..."
                        If Not ListBox2.Items.Count <= 0 Then
                            CurrentPost = 0
                            WebBrowser3.Navigate("https://m.facebook.com/ufi/reaction/profile/browser/fetch/?limit=999999999999999999&total_count=999999999999999999&ft_ent_identifier=" & ListBox2.Items(CurrentPost).ToString, Nothing, Nothing, "User-Agent: Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/1.0.154.53 Safari/525.19")
                        End If
                    End If
                    '   Else
                    'finished()
                    'SubTabControlX2.SelectedIndex = 2
                    'H2X3.Text = "likes obtained from " & lblPostIDsCount.Text & " posts" & "..." & vbCrLf & " Please wait..."

                    '  End If

                End If
                lblPostIDsCount.Text = ListBox2.Items.Count
            Else
                'ID's by richtextbox
                ' ListBox2.Items.AddRange(Split(RichTextBox1.Text, vbLf)) 'OVERRIDED BY BUTTON
                ' lblPostIDsCount.Text=ListBox2.Items.Count 
            End If
        Else

        End If


    End Sub


    Public CurrentPost As Integer = 0
    Private Sub WebBrowser3_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser3.DocumentCompleted

        'For Each element As HtmlElement In WebBrowser3.Document.GetElementsByTagName("a")

        '    If element.OuterHtml.Contains("shown_ids") Then
        '        '   element.InvokeMember("shown_ids")
        '        Dim res As String = element.OuterHtml.ToString
        '        Dim sp() As String = res.Split("&amp;")
        '        MsgBox(sp(1))
        '        '    res = element.OuterHtml.Replace("<a href=""/ufi/reaction/profile/browser/fetch/?limit=10&amp;shown_ids=", "")
        '        '  res = res.Replace("""><span>See More</span></a>", "") 'LANGUAGE PROBLEM
        '        ' MsgBox(res)
        '        ListBox3.Items.AddRange(Split(res, "%2C"))
        '        lblPostLikersCount.Text = ListBox3.Items.Count

        '    End If
        'Next

        'If Val(lblPostIDsCount.Text) > CurrentPost + 1 Then
        '    CurrentPost = CurrentPost + 1
        '    WebBrowser3.Navigate("https://mbasic.facebook.com/ufi/reaction/profile/browser/fetch/?limit=999999999999999999&total_count=999999999999999999&ft_ent_identifier=" & ListBox2.Items(CurrentPost).ToString)
        '    '  ElseIf Val(lblPostIDsCount.Text) < CurrentPost + 1 Then
        '    'overflow
        'End If

        ' '' '' '' ''Dim theElementCollection As HtmlElementCollection = Nothing
        ' '' '' '' ''theElementCollection = WebBrowser3.Document.GetElementsByTagName("a")
        ' '' '' '' ''While WebBrowser3.DocumentText.Contains("See More")


        ' '' '' '' ''    For Each curElement As HtmlElement In theElementCollection
        ' '' '' '' ''        If curElement.OuterHtml.Contains("See Moretruyx") Then
        ' '' '' '' ''            curElement.InvokeMember("click")
        ' '' '' '' ''        End If
        ' '' '' '' ''    Next
        ' '' '' '' ''End While
        ' '' '' '' ''MsgBox("done")
        '    ' MsgBox(WebBrowser3.DocumentText.Contains("pam uiBoxLightblue uiMorePagerPrimary"))
        'Loop 'Until WebBrowser3.DocumentText.Contains("See More") = False
        'MsgBox("done")



        'Dim script As String = "$(document).ready(function() {$('input[name=btnSub]').trigger('click'); });"
        'ScriptManager.RegisterStartupScript(Me, [GetType](), "ServerControlScript", script, True)

        ' RichTextBox2.Text = WebBrowser3.Document.Body.InnerHtml
        CurrentPost = CurrentPost + 1
        H2X3.Text = "likes obtained from " & lblPostIDsCount.Text & " posts" & "..." & vbCrLf & " Please wait..."

        If Not WebBrowser3.DocumentText.Contains("The page you requested cannot be displayed right now") Then

            Dim pattern As String = "profile_browser"",""id"":(.*?),""sc"
            For Each i As Match In Regex.Matches(WebBrowser3.Document.Body.InnerHtml, pattern)
                ListBox3.Items.Add(i.Groups(1).Value)
                lblPostLikersCount.Text = ListBox3.Items.Count
            Next
            If Val(lblPostIDsCount.Text) >= CurrentPost + 1 Then
                WebBrowser3.Navigate("https://m.facebook.com/ufi/reaction/profile/browser/fetch/?limit=999999999999999999&total_count=999999999999999999&ft_ent_identifier=" & ListBox2.Items(CurrentPost).ToString, Nothing, Nothing, "User-Agent: Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/1.0.154.53 Safari/525.19")

            Else
                'finish
                'validate
                SubTabControlX2.SelectedIndex = 4 'swith tabs

                BackgroundWorker1.RunWorkerAsync()
                While BackgroundWorker1.IsBusy
                    Application.DoEvents()
                End While


               

                lblst1.Text = lblFriendlistCount.Text
                Label5.Text = "All likers" & vbCrLf & "(for " & lblPostIDsCount.Text & " post(s)) :"
                lblst2.Text = ListBox3.Items.Count
                lblst3.Text = lblPostLikersCount.Text

                Dim collectionOfListViewItems = ListView1.Items.Cast(Of ListViewItem)()
                Dim ToUnfriend = (From c In collectionOfListViewItems Where c.SubItems(2).Text = "UNFRIEND").Count()
                lblst4.Text = ToUnfriend
                lblst5.Text = Val(ListBox1.Items.Count - ToUnfriend)

            End If
        Else
            '  MsgBox("post not available")  'error
            If Val(lblPostIDsCount.Text) >= CurrentPost + 1 Then
                WebBrowser3.Navigate("https://m.facebook.com/ufi/reaction/profile/browser/fetch/?limit=999999999999999999&total_count=999999999999999999&ft_ent_identifier=" & ListBox2.Items(CurrentPost).ToString, Nothing, Nothing, "User-Agent: Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/1.0.154.53 Safari/525.19")

            Else
                'finish
                'validate
              
                SubTabControlX2.SelectedIndex = 4 'swith tabs

                'collect profile pics
                BackgroundWorker1.RunWorkerAsync()


            End If
        End If

    End Sub
    Private Sub filldps()
        Dim sleeper As New Threading.ManualResetEvent(False)
        Do
            sleeper.WaitOne(250)
            Me.Invoke(Sub()
                          Try
                              For i = 0 To ListView1.Items.Count
                                  ListView1.Items(i).Tag = New String(ListBox1.Items(i).ToString)
                                  ListView1.Items(i).ImageIndex = ImageList1.Images.IndexOfKey(ListView1.Items(i).Tag.ToString)
                              Next
                          Catch ex As Exception
                              ' l_timer.Enabled = False
                          End Try
                      End Sub)
        Loop
    End Sub
    Private Sub ButtonX5_Click(sender As Object, e As EventArgs) Handles ButtonX5.Click
        FirefoxCheckBox1.Visible = False
        ButtonX5.Visible = False
        SubTabControlX1.SelectedIndex = 1
        logrefresh = True
        WebBrowser5.Navigate("https://m.facebook.com/login")
        WaitForPageLoad()
    End Sub
    Public logrefresh As Boolean = False
    Private Sub WebBrowser5_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser5.DocumentCompleted
        If WebBrowser5.DocumentText.Contains("Share with") Then 'logged
            WebBrowser5.Visible = False
            Loading.Visible = False
            GroupBox2.Visible = True
            GroupBox2.Enabled = True
            PictureBox8.Visible = False
            ' ElseIf WebBrowser5.DocumentText.Contains("Save Your Password for Next Time") Then
            'inject
            'Dim headElement As HtmlElement = WebBrowser5.Document.GetElementsByTagName("head")(0)
            'Dim scriptElement As HtmlElement = WebBrowser5.Document.CreateElement("script")
            'Dim element As IHTMLScriptElement = CType(scriptElement.DomElement, IHTMLScriptElement)
            'element.text = "function sayHello() { " & lblinjectcode.text & " }"
            'headElement.AppendChild(scriptElement)
            'WebBrowser5.Document.InvokeScript("sayHello")

        ElseIf WebBrowser5.Document.Url.AbsoluteUri.ToString = "https://m.facebook.com/login" Then
            If logrefresh = True Then
                logrefresh = False
                WebBrowser5.Refresh()
            End If

        End If

    End Sub



    Private Property pageready As Boolean = False

#Region "Page Loading Functions"
    Private Sub WaitForPageLoad()
        AddHandler WebBrowser5.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)
        While Not pageready
            WebBrowser5.Visible = False
            Loading.Visible = True
            Application.DoEvents()
        End While
        pageready = False
    End Sub

    Private Sub PageWaiter(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
        If WebBrowser5.ReadyState = WebBrowserReadyState.Complete Then
            pageready = True
            WebBrowser5.Visible = True
            Loading.Visible = False
            RemoveHandler WebBrowser5.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)
        End If
    End Sub

#End Region


    Public unfriendIndex As Integer = 0
    Private Sub ButtonX8_Click(sender As Object, e As EventArgs) Handles ButtonX8.Click

        Dim toUnfriendCount = 0
        For Each item As ListViewItem In ListView1.Items
            If item.SubItems(2).Text = "UNFRIEND" Then
                toUnfriendCount += 1
            End If
        Next
        If Not toUnfriendCount = 0 Then
            Dim result As Integer = MessageBox.Show("You are about to unfriend " & toUnfriendCount & " friends." & vbCrLf & "Are you sure?", "Confirmation", MessageBoxButtons.YesNo)
            If result = DialogResult.Yes Then

                For Each item As ListViewItem In ListView1.Items
                    If item.SubItems(2).Text = "UNFRIEND" Then
                        ListBox8.Items.Add(item.Text)
                    End If
                Next


                WebBrowser7.Navigate("https://mbasic.facebook.com/removefriend.php?friend_id=" & ListBox8.Items(unfriendIndex).ToString)
                lblFinCount.Text = "1/" & toUnfriendCount
                Label10.Text = toUnfriendCount
                lblfinName.Text = "Resolving..."
                SubTabControlX2.SelectedIndex = 5
            End If
        Else
            MsgBox("Select friends to unfriend")
        End If


    End Sub


    ' Private ImgList As New ImageList With {.ImageSize = New Size(24, 24)}




    Private Sub ButtonX7_Click(sender As Object, e As EventArgs) Handles ButtonX7.Click
        For i As Integer = 0 To ListView1.Items.Count - 1
            If ListView1.Items(i).SubItems(1).Text.ToLower.Contains(TextboxX1.Text.ToLower) Then
                SetItemFocus(ListView1, ListView1.Items(i))
                Return
            End If
        Next



    End Sub


    Public Function SetItemFocus(ByRef ctlListview As ListView, ByVal oLVItem As ListViewItem, Optional ByVal iVisibleIndex As Integer = 3) As Boolean
        Try

            Dim iItemsPerPage As Integer
            Dim iCurrentTopIndex As Integer
            Dim iLVItemIndex As Integer = oLVItem.Index

            With ctlListview

                ' Clear all selections and highlight and focus the desired item.
                .SelectedItems.Clear()
                .Items(iLVItemIndex).Selected = True
                .FocusedItem = oLVItem

                ' Determine if desired index + number of items in view will exceed total items in the control
                iCurrentTopIndex = .TopItem.Index
                iItemsPerPage = (.ClientSize.Height \ .GetItemRect(0).Height) - 1

                ' Do we even need to scroll? Not if the selected track is already in view
                If (iCurrentTopIndex >= iLVItemIndex) Or (iLVItemIndex > iCurrentTopIndex + iItemsPerPage) Then

                    ' Is iCurrentTopIndex above or below target index?
                    If iCurrentTopIndex >= iLVItemIndex Then ' Going UP

                        If iLVItemIndex > 3 Then
                            ' Drops the highlighted item down a few so it's not hidden
                            ' behind the Column header.
                            .Items((iLVItemIndex - iVisibleIndex)).EnsureVisible()
                        Else
                            oLVItem.EnsureVisible()
                        End If
                    Else ' Going DOWN
                        ' Are there sufficient items to set to the topindex
                        If (iLVItemIndex + iItemsPerPage) > .Items.Count - 1 Then
                            ' Can't be set to the top as the control has insufficient
                            ' items, so just scroll to the end of listview
                            .Items(.Items.Count - 1).EnsureVisible()
                        Else
                            ' Since a listview always moves the item just into view,
                            ' have it instead move to the top by faking item we want to 'EnsureVisible'
                            ' the item iItemsPerPage -1(or -3) below the actual index of interest.
                            If iLVItemIndex > 3 Then
                                .Items((iLVItemIndex + iItemsPerPage) - iVisibleIndex).EnsureVisible()
                            Else
                                .Items((iLVItemIndex + iItemsPerPage) - 1).EnsureVisible()
                            End If
                        End If
                    End If
                End If
            End With

            SetItemFocus = True

        Catch oError As Exception
        End Try
    End Function

    Private Sub ButtonX2_Click(sender As Object, e As EventArgs) Handles ButtonX2.Click
        SubTabControlX2.SelectedIndex = 2
    End Sub



    Private Sub WebBrowser7_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser7.DocumentCompleted

        '  MsgBox(ListBox8.Items(1).ToString)
        If WebBrowser7.DocumentText.ToLower.Contains("confirm") Then 'unfriend page
            For Each htmls As HtmlElement In WebBrowser7.Document.All
                If htmls.GetAttribute("value").ToLower = "confirm" Then
                    htmls.InvokeMember("click")
                End If
            Next
            '     ElseIf WebBrowser7.DocumentText.Contains("The page you requested cannot be displayed right now. It may be temporarily unavailable,") Then

        Else
            If unfriendIndex = 200 Then
                SubTabControlX1.SelectedIndex = 6
                MsgBox("Free version of Facebook mega tools only support 200 unfriends at once")
                AboutForm.ShowDialog()
                WebBrowser7.Stop()
                Exit Sub
            End If

            If unfriendIndex < ListBox8.Items.Count - 1 Then
                unfriendIndex = unfriendIndex + 1
                lblFinCount.Text = unfriendIndex + 1 & "/" & Label10.Text
                For Each item As ListViewItem In ListView1.Items
                    If item.Text = ListBox8.Items(unfriendIndex).ToString Then
                        lblfinName.Text = item.SubItems(1).Text
                    End If
                Next
                WebBrowser7.Navigate("https://mbasic.facebook.com/removefriend.php?friend_id=" & ListBox8.Items(unfriendIndex).ToString)

            Else
                'FINISH
                'MainTabControlX1.SelectedIndex = 0
                SubTabControlX1.SelectedIndex = 6
                AboutForm.ShowDialog()
            End If


        End If


    End Sub

    Private Sub ButtonX9_Click(sender As Object, e As EventArgs) Handles ButtonX9.Click

        For i As Integer = 0 To ListView1.Items.Count - 1
            ListView1.Items(i).SubItems(2).Text = "KEEP"
        Next
    End Sub

    Private Sub TabPage10_Click(sender As Object, e As EventArgs) Handles TabPage10.Click

    End Sub


    Private Sub FirefoxCheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles FirefoxCheckBox1.CheckedChanged
        If FirefoxCheckBox1.Checked = True Then
            ButtonX5.Visible = True
        Else
            ButtonX5.Visible = False
        End If
    End Sub


    Private Function lvColumnNumber(ByVal e As System.Windows.Forms.MouseEventArgs) As Integer
        Dim colend As Integer = 0
        Dim colstart As Integer = 0
        Dim x As Integer

        For x = 0 To (ListView1.Columns.Count - 1)
            colend = colend + ListView1.Columns(x).Width
            If colstart <= e.X And e.X <= colend Then
                Return x + 1
            End If
            colstart = colstart + ListView1.Columns(x).Width
        Next
    End Function

    Private Sub ListView1_MouseMove(sender As Object, e As MouseEventArgs) Handles ListView1.MouseMove
        If lvColumnNumber(e) = 4 Or lvColumnNumber(e) = 5 Then
            Windows.Forms.Cursor.Current = Cursors.Hand
        Else
            Windows.Forms.Cursor.Current = Cursors.Default
        End If
    End Sub


    Private Sub ListView1_Click(sender As Object, e As EventArgs) Handles ListView1.Click
        Dim mousePosition As Point = ListView1.PointToClient(Control.MousePosition)
        Dim hit As ListViewHitTestInfo = ListView1.HitTest(mousePosition)
        Dim columnindex As Integer = hit.Item.SubItems.IndexOf(hit.SubItem)

        If columnindex = 3 Then
            If ListView1.SelectedItems.Count > 0 Then
                If ListView1.SelectedItems(0).SubItems(2).Text = "UNFRIEND" Then
                    ListView1.SelectedItems(0).SubItems(2).Text = "KEEP"
                Else
                    ListView1.SelectedItems(0).SubItems(2).Text = "UNFRIEND"
                End If


                If ListView1.SelectedItems(0).SubItems(2).Text = "UNFRIEND" Then
                    '  ListView1.SelectedItems(0).UseItemStyleForSubItems = False
                    ListView1.SelectedItems(0).SubItems(2).ForeColor = Color.Red

                Else
                    '  ListView1.SelectedItems(0).UseItemStyleForSubItems = False
                    ListView1.SelectedItems(0).SubItems(2).ForeColor = Color.Green
                End If

                Try
                    Dim collectionOfListViewItems = ListView1.Items.Cast(Of ListViewItem)()
                    Dim ToUnfriend = (From c In collectionOfListViewItems Where c.SubItems(2).Text = "UNFRIEND").Count()
                    lblst4.Text = ToUnfriend
                    lblst5.Text = Val(ListBox1.Items.Count - ToUnfriend)

                Catch ex As Exception

                End Try


            End If
        ElseIf columnindex = 4 Then
            If ListView1.SelectedItems.Count > 0 Then
                Dim webAddress As String = "http://www.facebook.com/profile.php?id=" & ListView1.SelectedItems(0).Text
                Process.Start(webAddress)
            End If
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub


    Private Sub TabPage2_Click(sender As Object, e As EventArgs) Handles TabPage2.Click

    End Sub

    Private Sub SubTabControlX2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SubTabControlX2.SelectedIndexChanged
        If SubTabControlX2.SelectedIndex = 4 Then
            headerlbl.Text = "Scan Summary"
        Else
            headerlbl.Text = ""
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim webAddress As String = "http://rashanhasaranga.com/facebook-mega-tools-download"
        Process.Start(webAddress)
    End Sub

    Private Sub PictureBox10_Click(sender As Object, e As EventArgs) Handles PictureBox10.Click
        Dim webAddress As String = "http://rashanhasaranga.com/facebook-mega-tools-download"
        Process.Start(webAddress)
    End Sub

    Private Sub PictureBox15_Click(sender As Object, e As EventArgs) Handles PictureBox15.Click
        Dim webAddress As String = "http://rashanhasaranga.com"
        Process.Start(webAddress)
    End Sub

    Private Sub PictureBox18_Click(sender As Object, e As EventArgs) Handles PictureBox18.Click
        Dim webAddress As String = "https://twitter.com/RashanHasaranga"
        Process.Start(webAddress)
    End Sub

    Private Sub PictureBox20_Click(sender As Object, e As EventArgs) Handles PictureBox20.Click
        Dim webAddress As String = "https://www.instagram.com/rashan.hasaranga/"
        Process.Start(webAddress)
    End Sub

    Private Sub PictureBox16_Click(sender As Object, e As EventArgs) Handles PictureBox16.Click
        Dim webAddress As String = "https://rashanhasaranga.com"
        Process.Start(webAddress)
    End Sub

    Private Sub PictureBox21_Click(sender As Object, e As EventArgs) Handles PictureBox21.Click
        Dim webAddress As String = "https://github.com/rashanh"
        Process.Start(webAddress)
    End Sub

    Private Sub PictureBox22_Click(sender As Object, e As EventArgs) Handles PictureBox22.Click
        Dim webAddress As String = "https://www.facebook.com/100008164230307"
        Process.Start(webAddress)
    End Sub

    Private Sub ButtonX3_Click(sender As Object, e As EventArgs) Handles ButtonX3.Click
        AboutForm.ShowDialog()
    End Sub


    Private Sub Label23_Click(sender As Object, e As EventArgs) Handles Label23.Click
        AboutForm.ShowDialog()
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Dim webAddress As String = "http://rashandonate.ddns.net"
        Process.Start(webAddress)
    End Sub

   


    Private Sub BackgroundWorker1_DoWork_1(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        'finish
        'validate
        ListBox5.Items.Clear()
        ListBox6.Items.Clear()
        ListBox5.Items.AddRange(RichTextBox3.Lines)
        ListBox6.Items.AddRange(RichTextBox4.Lines)


        Dim items(ListBox5.Items.Count - 1) As Object
        ListBox5.Items.CopyTo(items, 0)
        ListBox5.Items.Clear()
        ListBox5.Items.AddRange(items.AsEnumerable().Distinct().ToArray())

        Dim items2(ListBox6.Items.Count - 1) As Object
        ListBox6.Items.CopyTo(items2, 0)
        ListBox6.Items.Clear()
        ListBox6.Items.AddRange(items2.AsEnumerable().Distinct().ToArray())


        Dim SubTabControlX2a As Action = Function() SubTabControlX2.SelectedIndex = 4
        SubTabControlX2.Invoke(SubTabControlX2a)

        '   SubTabControlX2.SelectedIndex = 4 'swith tabs

        


        'remove duplicates
        Dim items3(ListBox3.Items.Count - 1) As Object
        ListBox3.Items.CopyTo(items3, 0)
        ListBox3.Items.Clear()
        ListBox3.Items.AddRange(items3.AsEnumerable().Distinct().ToArray())
    
        '  lblst3.Text = lblPostLikersCount.Text

        'remove approved from all friends
        Dim itemsx() As String = ListBox3.Items.Cast(Of String).ToArray
        Dim itemsx2() As String = ListBox1.Items.Cast(Of String).ToArray
        ListBox7.Items.Clear()
        ListBox7.Items.AddRange(itemsx2.Except(itemsx).ToArray)

        'move must unfriend to list7
        ListBox7.Items.AddRange(ListBox6.Items)

        'finally remove exclutions from list 7
        Dim itemsx3() As String = ListBox5.Items.Cast(Of String).ToArray
        Dim itemsx4() As String = ListBox7.Items.Cast(Of String).ToArray
        ListBox7.Items.Clear()
        ListBox7.Items.AddRange(itemsx4.Except(itemsx3).ToArray)


    
        Dim i As Integer = 0
        Do While (ListBox7.Items.Count) - 1 >= i
            If ListBox7.Items(i) = String.Empty Then
                ListBox7.Items.Remove(ListBox7.Items(i))
                i -= 1
            End If
            i += 1
        Loop
        'remove dulpicates from list7
        Dim items4(ListBox7.Items.Count - 1) As Object
        ListBox7.Items.CopyTo(items4, 0)
        ListBox7.Items.Clear()
        ListBox7.Items.AddRange(items4.AsEnumerable().Distinct().ToArray())

        'filllistview()
        'Application.DoEvents()
        Dim filllistviewthread As New Threading.Thread(AddressOf filllistview)
        With filllistviewthread
            '.Name = "someName"
            .IsBackground = True
        End With
        filllistviewthread.Start()


        'fill profile pics
        Dim filldpthread As New Threading.Thread(AddressOf filldps)
        With filldpthread
            '.Name = "someName"
            .IsBackground = True
        End With

        filldpthread.Start()


        BackgroundWorker1.Dispose()
    End Sub

    Private Sub filllistview()
        '   Dim sleeper As New Threading.ManualResetEvent(False)

        ' sleeper.WaitOne(500)
        Me.Invoke(Sub()
                      'profile pic
                      'While BackgroundWorker2.IsBusy
                      '    Application.DoEvents()
                      'End While

                      If ListBox1.InvokeRequired Then
                          ListBox1.Invoke(New DelegateStandardPattern(AddressOf filllistview))
                          Return
                      End If


                      For i = 0 To ListBox1.Items.Count - 1
                          'Dim subItems As String() = {lstNames.Items(i).ToString, "KEEP", "Change?", "View"}
                          'ListView1.Items.Add(ListBox1.Items(i).ToString, ListBox1.Items(i).ToString).SubItems.AddRange(subItems)  'use SubItems

                          Dim itm As ListViewItem = ListView1.Items.Add(ListBox1.Items(i).ToString)
                          itm.UseItemStyleForSubItems = False
                          itm.ForeColor = Color.Blue
                          'itm.ImageKey = ListBox1.Items(i).ToString
                          itm.SubItems.Add(lstNames.Items(i).ToString)
                          itm.SubItems.Add("KEEP").ForeColor = Color.Chocolate
                          itm.SubItems.Add("Change?")
                          itm.SubItems.Add("⏏")


                      Next


                      For i1 As Integer = 0 To ListView1.Items.Count - 1
                          For x As Integer = 0 To ListBox7.Items.Count - 1
                              If ListView1.Items(i1).Text = ListBox7.Items(x).ToString Then
                                  ListView1.Items(i1).SubItems(2).Text = "UNFRIEND"
                              End If
                      Next x, i1

                      For i2 As Integer = 0 To ListView1.Items.Count - 1
                          If ListView1.Items(i2).SubItems(2).Text = "UNFRIEND" Then
                              '  ListView1.Items(i2).UseItemStyleForSubItems = True
                              ListView1.Items(i2).SubItems(2).ForeColor = Color.Red

                          Else
                              ' ListView1.Items(i2).UseItemStyleForSubItems = True
                              ListView1.Items(i2).SubItems(2).ForeColor = Color.Green
                          End If
                      Next

                      For i7 As Integer = 0 To ListView1.Items.Count - 1
                          ListView1.Items(i7).SubItems(3).ForeColor = Color.Blue
                          ListView1.Items(i7).SubItems(4).ForeColor = Color.Blue
                      Next

                  End Sub)

    End Sub

    Public Delegate Sub DelegateStandardPattern()
    'Private Sub filllistview()

    '    'profile pic
    '    'While BackgroundWorker2.IsBusy
    '    '    Application.DoEvents()
    '    'End While

    '    If ListBox1.InvokeRequired Then
    '        ListBox1.Invoke(New DelegateStandardPattern(AddressOf filllistview))
    '        Return
    '    End If


    '    For i = 0 To ListBox1.Items.Count - 1
    '        'Dim subItems As String() = {lstNames.Items(i).ToString, "KEEP", "Change?", "View"}
    '        'ListView1.Items.Add(ListBox1.Items(i).ToString, ListBox1.Items(i).ToString).SubItems.AddRange(subItems)  'use SubItems

    '        Dim itm As ListViewItem = ListView1.Items.Add(ListBox1.Items(i).ToString)
    '        itm.UseItemStyleForSubItems = False
    '        itm.ForeColor = Color.Blue
    '        'itm.ImageKey = ListBox1.Items(i).ToString
    '        itm.SubItems.Add(lstNames.Items(i).ToString)
    '        itm.SubItems.Add("KEEP").ForeColor = Color.Chocolate
    '        itm.SubItems.Add("Change?")
    '        itm.SubItems.Add("▶")


    '    Next


    '    For i1 As Integer = 0 To ListView1.Items.Count - 1
    '        For x As Integer = 0 To ListBox7.Items.Count - 1
    '            If ListView1.Items(i1).Text = ListBox7.Items(x).ToString Then
    '                ListView1.Items(i1).SubItems(2).Text = "UNFRIEND"
    '            End If
    '    Next x, i1

    '    For i2 As Integer = 0 To ListView1.Items.Count - 1
    '        If ListView1.Items(i2).SubItems(2).Text = "UNFRIEND" Then
    '            '  ListView1.Items(i2).UseItemStyleForSubItems = True
    '            ListView1.Items(i2).SubItems(2).ForeColor = Color.Red

    '        Else
    '            ' ListView1.Items(i2).UseItemStyleForSubItems = True
    '            ListView1.Items(i2).SubItems(2).ForeColor = Color.Green
    '        End If
    '    Next

    '    For i7 As Integer = 0 To ListView1.Items.Count - 1
    '        ListView1.Items(i7).SubItems(3).ForeColor = Color.Blue
    '        ListView1.Items(i7).SubItems(4).ForeColor = Color.Blue
    '    Next




    'End Sub

    Private Sub BackgroundWorker2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork
        lblst4.Text = ListBox7.Items.Count
        lblst5.Text = Val(lblst1.Text) - Val(lblst4.Text)
        While ImageList1.Images.Count < ListBox1.Items.Count
            ImageList1.Images.Add(ListBox1.Items(ImageList1.Images.Count).ToString, Bitmap.FromStream(System.Net.HttpWebRequest.Create("http://graph.facebook.com/" & ListBox1.Items(ImageList1.Images.Count).ToString & "/picture?type=square").GetResponse.GetResponseStream))
        End While



    End Sub

    Private Sub BackgroundWorker3_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        For i = 0 To ListView1.Items.Count - 1
            ListView1.Items(i).Tag = New String(ListBox1.Items(i).ToString)
            ListView1.Items(i).ImageIndex = ImageList1.Images.IndexOfKey(ListView1.Items(i).Tag.ToString)

        Next
    End Sub

    Private Sub ButtonX6_Click(sender As Object, e As EventArgs) Handles ButtonX6.Click
        MainTabControlX1.SelectedIndex = 1
        SubTabControlX2.SelectedIndex = 0
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        frm_tos.ShowDialog()
    End Sub
End Class
