Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text

' Get more free themes at ThemesVB.NET

Module Theme
    ' Get more free themes at ThemesVB.NET
    Public ReadOnly Property GlobalFont(B As FontStyle, S As Integer) As Font
        Get
            Return New Font("Segoe UI", S, B)
        End Get
    End Property

    Public Function GetCheckMark() As String
        Return "iVBORw0KGgoAAAANSUhEUgAAABMAAAAQCAYAAAD0xERiAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAEySURBVDhPY/hPRUBdw/79+/efVHz77bf/X37+wRAn2bDff/7+91l+83/YmtsYBpJs2ITjz/8rTbrwP2Dlrf9XXn5FkSPJsD13P/y3nHsVbNjyy28w5Ik27NWXX//TNt8DG1S19zFWNRiGvfzy8//ccy9RxEB4wvFnYIMMZl7+//brLwx5EEYx7MP33/9dF18Ha1py8RVcHBR7mlMvgsVXX8X0Hgwz/P379z8yLtz5AKxJdcpFcBj9+v3nf/CqW2Cx5E13UdSiYwzDvv36/d9/BUSzzvRL/0t2PQSzQd57+vEHilp0jGEYCJ9+8hnuGhiee+4Vhjp0jNUwEN566/1/m/mQZJC/48H/zz9+YVWHjHEaBsKgwAZ59eH771jl0TFew0D48osvWMWxYYKGEY///gcAqiuA6kEmfEMAAAAASUVORK5CYII="
    End Function

End Module

Module Helpers
    ' Get more free themes at ThemesVB.NET
    Public Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
    End Enum

    Public Function FullRectangle(S As Size, Subtract As Boolean) As Rectangle
        If Subtract Then
            Return New Rectangle(0, 0, S.Width - 1, S.Height - 1)
        Else
            Return New Rectangle(0, 0, S.Width, S.Height)
        End If
    End Function

    Public Function GreyColor(G As UInteger) As Color
        Return Color.FromArgb(G, G, G)
    End Function

    Public Sub CenterString(G As Graphics, T As String, F As Font, C As Color, R As Rectangle)
        Dim TS As SizeF = G.MeasureString(T, F)

        Using B As New SolidBrush(C)
            G.DrawString(T, F, B, New Point(R.Width / 2 - (TS.Width / 2), R.Height / 2 - (TS.Height / 2)))
        End Using
    End Sub
    ' Get more free themes at ThemesVB.NET
    Public Sub FillRoundRect(G As Graphics, R As Rectangle, Curve As Integer, C As Color)

        Using B As New SolidBrush(C)
            G.FillPie(B, R.X, R.Y, Curve, Curve, 180, 90)
            G.FillPie(B, R.X + R.Width - Curve, R.Y, Curve, Curve, 270, 90)
            G.FillPie(B, R.X, R.Y + R.Height - Curve, Curve, Curve, 90, 90)
            G.FillPie(B, R.X + R.Width - Curve, R.Y + R.Height - Curve, Curve, Curve, 0, 90)
            G.FillRectangle(B, CInt(R.X + Curve / 2), R.Y, R.Width - Curve, CInt(Curve / 2))
            G.FillRectangle(B, R.X, CInt(R.Y + Curve / 2), R.Width, R.Height - Curve)
            G.FillRectangle(B, CInt(R.X + Curve / 2), CInt(R.Y + R.Height - Curve / 2), R.Width - Curve, CInt(Curve / 2))
        End Using

    End Sub

    Public Sub DrawRoundRect(G As Graphics, R As Rectangle, Curve As Integer, C As Color)

        Using P As New Pen(C)
            G.DrawArc(P, R.X, R.Y, Curve, Curve, 180, 90)
            G.DrawLine(P, CInt(R.X + Curve / 2), R.Y, CInt(R.X + R.Width - Curve / 2), R.Y)
            G.DrawArc(P, R.X + R.Width - Curve, R.Y, Curve, Curve, 270, 90)
            G.DrawLine(P, R.X, CInt(R.Y + Curve / 2), R.X, CInt(R.Y + R.Height - Curve / 2))
            G.DrawLine(P, CInt(R.X + R.Width), CInt(R.Y + Curve / 2), CInt(R.X + R.Width), CInt(R.Y + R.Height - Curve / 2))
            G.DrawLine(P, CInt(R.X + Curve / 2), CInt(R.Y + R.Height), CInt(R.X + R.Width - Curve / 2), CInt(R.Y + R.Height))
            G.DrawArc(P, R.X, R.Y + R.Height - Curve, Curve, Curve, 90, 90)
            G.DrawArc(P, R.X + R.Width - Curve, R.Y + R.Height - Curve, Curve, Curve, 0, 90)
        End Using

    End Sub

    Public Sub CenterStringTab(G As Graphics, text As String, font As Font, brush As Brush, rect As Rectangle, Optional shadow As Boolean = False, Optional yOffset As Integer = 0)

        Dim textSize As SizeF = G.MeasureString(text, font)
        Dim textX As Integer = rect.X + (rect.Width / 2) - (textSize.Width / 2)
        Dim textY As Integer = rect.Y + (rect.Height / 2) - (textSize.Height / 2) + yOffset

        If shadow Then G.DrawString(text, font, Brushes.Black, textX + 1, textY + 1)
        G.DrawString(text, font, brush, textX, textY + 1)

    End Sub

End Module


<DefaultEvent("CheckedChanged")>
Class FirefoxRadioButton
    Inherits Control

#Region " Public "
    Public Event CheckedChanged(sender As Object, e As EventArgs)
#End Region

#Region " Private "
    Private State As MouseState
    Private ETC As Color = Nothing
    Private G As Graphics

    Private _EnabledCalc As Boolean
    Private _Checked As Boolean
    Private _Bold As Boolean
#End Region

#Region " Properties "

    Public Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property Enabled As Boolean
        Get
            Return EnabledCalc
        End Get
        Set(value As Boolean)
            _EnabledCalc = value
            Invalidate()
        End Set
    End Property

    <DisplayName("Enabled")>
    Public Property EnabledCalc As Boolean
        Get
            Return _EnabledCalc
        End Get
        Set(value As Boolean)
            Enabled = value
            Invalidate()
        End Set
    End Property

    Public Property Bold As Boolean
        Get
            Return _Bold
        End Get
        Set(value As Boolean)
            _Bold = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Control "

    Sub New()
        DoubleBuffered = True
        ForeColor = Color.FromArgb(66, 78, 90)
        Font = GlobalFont(FontStyle.Regular, 10)
        Size = New Size(160, 27)
        Enabled = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Parent.BackColor)

        If Enabled Then
            ETC = Color.FromArgb(66, 78, 90)

            Select Case State

                Case MouseState.Over, MouseState.Down

                    Using P As New Pen(Color.FromArgb(34, 146, 208))
                        G.DrawEllipse(P, New Rectangle(2, 2, 22, 22))
                    End Using

                Case Else

                    Using P As New Pen(GreyColor(190))
                        G.DrawEllipse(P, New Rectangle(2, 2, 22, 22))
                    End Using

            End Select

            If Checked Then

                Using B As New SolidBrush(Color.FromArgb(34, 146, 208))
                    G.FillEllipse(B, New Rectangle(7, 7, 12, 12))
                End Using

            End If

        Else
            ETC = GreyColor(170)

            Using P As New Pen(GreyColor(210))
                G.DrawEllipse(P, New Rectangle(2, 2, 22, 22))
            End Using

            If Checked Then

                Using B As New SolidBrush(Color.FromArgb(34, 146, 208))
                    G.FillEllipse(B, New Rectangle(7, 7, 12, 12))
                End Using

            End If

        End If

        Using B As New SolidBrush(ETC)

            If Bold Then
                G.DrawString(Text, GlobalFont(FontStyle.Bold, 10), B, New Point(32, 4))
            Else
                G.DrawString(Text, GlobalFont(FontStyle.Regular, 10), B, New Point(32, 4))
            End If

        End Using


    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        If Enabled Then

            If Not Checked Then

                For Each C As Control In Parent.Controls
                    If TypeOf C Is FirefoxRadioButton Then
                        DirectCast(C, FirefoxRadioButton).Checked = False
                    End If
                Next

            End If

            Checked = True
            RaiseEvent CheckedChanged(Me, e)
        End If

        State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

End Class

<DefaultEvent("CheckedChanged")>
Class FirefoxCheckBox
    Inherits Control

#Region " Public "
    Public Event CheckedChanged(sender As Object, e As EventArgs)
#End Region

#Region " Private "
    Private State As MouseState
    Private ETC As Color = Nothing
    Private G As Graphics

    Private _EnabledCalc As Boolean
    Private _Checked As Boolean
    Private _Bold As Boolean
#End Region

#Region " Properties "

    Public Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property Enabled As Boolean
        Get
            Return EnabledCalc
        End Get
        Set(value As Boolean)
            _EnabledCalc = value
            Invalidate()
        End Set
    End Property

    <DisplayName("Enabled")>
    Public Property EnabledCalc As Boolean
        Get
            Return _EnabledCalc
        End Get
        Set(value As Boolean)
            Enabled = value
            Invalidate()
        End Set
    End Property

    Public Property Bold As Boolean
        Get
            Return _Bold
        End Get
        Set(value As Boolean)
            _Bold = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Control "

    Sub New()
        DoubleBuffered = True
        ForeColor = Color.FromArgb(66, 78, 90)
        Font = GlobalFont(FontStyle.Regular, 10)
        Size = New Size(160, 27)
        Enabled = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Parent.BackColor)

        If Enabled Then
            ETC = Color.FromArgb(66, 78, 90)

            Select Case State

                Case MouseState.Over, MouseState.Down
                    DrawRoundRect(G, New Rectangle(3, 3, 20, 20), 3, Color.FromArgb(44, 156, 218))

                Case Else
                    DrawRoundRect(G, New Rectangle(3, 3, 20, 20), 3, GreyColor(200))

            End Select

            If Checked Then

                Using I As Image = Image.FromStream(New IO.MemoryStream(Convert.FromBase64String(GetCheckMark)))
                    G.DrawImage(I, New Point(4, 5))
                End Using

            End If

        Else

            ETC = GreyColor(170)
            DrawRoundRect(G, New Rectangle(3, 3, 20, 20), 3, GreyColor(220))

            If Checked Then

                Using I As Image = Image.FromStream(New IO.MemoryStream(Convert.FromBase64String(GetCheckMark)))
                    G.DrawImage(I, New Point(4, 5))
                End Using

            End If

        End If


        Using B As New SolidBrush(ETC)

            If Bold Then
                G.DrawString(Text, GlobalFont(FontStyle.Bold, 10), B, New Point(32, 4))
            Else
                G.DrawString(Text, GlobalFont(FontStyle.Regular, 10), B, New Point(32, 4))
            End If

        End Using


    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        If Enabled Then
            Checked = Not Checked
            RaiseEvent CheckedChanged(Me, e)
        End If

        State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

End Class

Class H1X
    Inherits Label

#Region " Private "
    Private G As Graphics
#End Region

#Region " Control "

    Sub New()
        DoubleBuffered = True
        AutoSize = False
        Font = New Font("Segoe UI Semibold", 20)
        ForeColor = Color.FromArgb(76, 88, 100)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        Using P As New Pen(GreyColor(200))
            G.DrawLine(P, New Point(0, 50), New Point(Width, 50))
        End Using

    End Sub

#End Region

End Class

Class H2X
    Inherits Label

#Region " Control "

    Sub New()
        Font = GlobalFont(FontStyle.Bold, 10)
        ForeColor = Color.FromArgb(76, 88, 100)
        BackColor = Color.White
    End Sub

#End Region

End Class

Class ButtonX
    Inherits Control

#Region " Private "
    Private State As MouseState
    Private ETC As Color = Nothing
    Private G As Graphics

    Private _EnabledCalc As Boolean
#End Region

#Region " Properties "

    Public Shadows Property Enabled As Boolean
        Get
            Return EnabledCalc
        End Get
        Set(value As Boolean)
            _EnabledCalc = value
            Invalidate()
        End Set
    End Property

    <DisplayName("Enabled")>
    Public Property EnabledCalc As Boolean
        Get
            Return _EnabledCalc
        End Get
        Set(value As Boolean)
            Enabled = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Control "

    Sub New()
        DoubleBuffered = True
        Enabled = True
        ForeColor = Color.FromArgb(56, 68, 80)
        Font = GlobalFont(FontStyle.Regular, 10)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Parent.BackColor)

        If Enabled Then
            ETC = Color.FromArgb(56, 68, 80)

            Select Case State

                Case MouseState.None

                    Using B As New SolidBrush(GreyColor(245))
                        G.FillRectangle(B, New Rectangle(1, 1, Width - 2, Height - 2))
                    End Using

                    DrawRoundRect(G, FullRectangle(Size, True), 2, GreyColor(193))

                Case MouseState.Over

                    Using B As New SolidBrush(GreyColor(232))
                        G.FillRectangle(B, New Rectangle(1, 1, Width - 2, Height - 2))
                    End Using

                    DrawRoundRect(G, FullRectangle(Size, True), 2, GreyColor(193))

                Case Else

                    Using B As New SolidBrush(GreyColor(212))
                        G.FillRectangle(B, New Rectangle(1, 1, Width - 2, Height - 2))
                    End Using

                    DrawRoundRect(G, FullRectangle(Size, True), 2, GreyColor(193))

            End Select

        Else
            ETC = GreyColor(170)

            Using B As New SolidBrush(GreyColor(245))
                G.FillRectangle(B, New Rectangle(1, 1, Width - 2, Height - 2))
            End Using

            DrawRoundRect(G, FullRectangle(Size, True), 2, GreyColor(223))

        End If

        CenterString(G, Text, GlobalFont(FontStyle.Regular, 10), ETC, FullRectangle(Size, False))

    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

End Class

Class RedirectX
    Inherits Control

#Region " Private "
    Private State As MouseState
    Private G As Graphics

    Private FC As Color = Nothing
    Private FF As Font = Nothing
#End Region

#Region " Control "

    Sub New()
        DoubleBuffered = True
        Cursor = Cursors.Hand
        BackColor = Color.White
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        Select Case State

            Case MouseState.Over
                FC = Color.FromArgb(23, 140, 229)
                FF = GlobalFont(FontStyle.Underline, 10)

            Case MouseState.Down
                FC = Color.FromArgb(255, 149, 0)
                FF = GlobalFont(FontStyle.Regular, 10)

            Case Else
                FC = Color.FromArgb(0, 149, 221)
                FF = GlobalFont(FontStyle.Regular, 10)

        End Select

        Using B As New SolidBrush(FC)
            G.DrawString(Text, FF, B, New Point(0, 0))
        End Using

    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

End Class

Class SubTabControlX
    Inherits TabControl

#Region " Private "
    Private G As Graphics
    Private TabRect As Rectangle
#End Region

#Region " Control "

    Sub New()
        DoubleBuffered = True
        Alignment = TabAlignment.Top
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        SetStyle(ControlStyles.UserPaint, True)
        ItemSize = New Size(100, 40)
        SizeMode = TabSizeMode.Fixed
    End Sub

    Protected Overrides Sub OnControlAdded(e As ControlEventArgs)
        MyBase.OnControlAdded(e)
        Try
            For i As Integer = 0 To TabPages.Count - 1
                TabPages(i).BackColor = Color.White
                TabPages(i).ForeColor = Color.FromArgb(66, 79, 90)
                TabPages(i).Font = GlobalFont(FontStyle.Regular, 10)
            Next
        Catch
        End Try
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Parent.BackColor)

        For i As Integer = 0 To TabPages.Count - 1

            TabRect = GetTabRect(i)

            If GetTabRect(i).Contains(Me.PointToClient(Cursor.Position)) And Not SelectedIndex = i Then

                Using B As New SolidBrush(GreyColor(240))
                    G.FillRectangle(B, New Rectangle(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2, GetTabRect(i).Width, GetTabRect(i).Height + 1))
                End Using

            ElseIf SelectedIndex = i Then

                Using B As New SolidBrush(GreyColor(240))
                    G.FillRectangle(B, New Rectangle(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2, GetTabRect(i).Width, GetTabRect(i).Height + 1))
                End Using

                Using P As New Pen(Color.FromArgb(255, 149, 0), 4)
                    G.DrawLine(P, New Point(TabRect.X - 2, TabRect.Y + ItemSize.Height - 2), New Point(TabRect.X + TabRect.Width - 2, TabRect.Y + ItemSize.Height - 2))
                End Using

            ElseIf Not SelectedIndex = i Then
                G.FillRectangle(Brushes.White, GetTabRect(i))
            End If

            Using B As New SolidBrush(Color.FromArgb(56, 69, 80))
                CenterStringTab(G, TabPages(i).Text, GlobalFont(FontStyle.Regular, 10), B, GetTabRect(i))
            End Using

        Next

        Using P As New Pen(GreyColor(200))
            G.DrawLine(P, New Point(0, ItemSize.Height + 2), New Point(Width, ItemSize.Height + 2))
        End Using

    End Sub

#End Region

End Class

Class MainTabControlX
    Inherits TabControl

#Region " Private "
    Private G As Graphics
    Private TabRect As Rectangle
    Private FC As Color = Nothing
#End Region

#Region " Control "

    Sub New()
        DoubleBuffered = True
        ItemSize = New Size(43, 152)
        Alignment = TabAlignment.Left
        SizeMode = TabSizeMode.Fixed
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        SetStyle(ControlStyles.UserPaint, True)
    End Sub

    Protected Overrides Sub OnControlAdded(e As ControlEventArgs)
        MyBase.OnControlAdded(e)
        Try
            For i As Integer = 0 To TabPages.Count - 1
                TabPages(i).BackColor = Color.White
                TabPages(i).ForeColor = Color.FromArgb(66, 79, 90)
                TabPages(i).Font = GlobalFont(FontStyle.Regular, 10)
            Next
        Catch
        End Try
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Color.FromArgb(66, 79, 90))

        For i As Integer = 0 To TabPages.Count - 1

            TabRect = GetTabRect(i)

            If SelectedIndex = i Then

                Using B As New SolidBrush(Color.FromArgb(52, 63, 72))
                    G.FillRectangle(B, TabRect)
                End Using

                FC = GreyColor(245)

                Using B As New SolidBrush(Color.FromArgb(255, 175, 54))
                    G.FillRectangle(B, New Rectangle(TabRect.Location.X - 3, TabRect.Location.Y + 1, 15, TabRect.Height - 2))
                End Using

            Else
                FC = GreyColor(192)

                Using B As New SolidBrush(Color.FromArgb(66, 79, 90))
                    G.FillRectangle(B, TabRect)
                End Using

            End If

            Using B As New SolidBrush(FC)
                G.DrawString(TabPages(i).Text, GlobalFont(FontStyle.Regular, 10), B, New Point(TabRect.X + 15, TabRect.Y + 12))
            End Using

            If Not IsNothing(ImageList) Then
                If Not TabPages(i).ImageIndex < 0 Then
                    G.DrawImage(ImageList.Images(TabPages(i).ImageIndex), New Rectangle(TabRect.X + 19, TabRect.Y + ((TabRect.Height / 2) - 10), 18, 18))
                End If
            End If

        Next

    End Sub

#End Region

End Class

<DefaultEvent("TextChanged")>
Class TextboxX
    Inherits Control
    ' Get more free themes at ThemesVB.NET
#Region " Private "
    Private WithEvents TB As New TextBox
    Private G As Graphics
    Private State As MouseState
    Private IsDown As Boolean

    Private _EnabledCalc As Boolean
    Private _allowpassword As Boolean = False
    Private _maxChars As Integer = 32767
    Private _textAlignment As HorizontalAlignment
    Private _multiLine As Boolean = False
    Private _readOnly As Boolean = False
#End Region

#Region " Properties "

    Public Shadows Property Enabled As Boolean
        Get
            Return EnabledCalc
        End Get
        Set(value As Boolean)
            TB.Enabled = value
            _EnabledCalc = value
            Invalidate()
        End Set
    End Property

    <DisplayName("Enabled")>
    Public Property EnabledCalc As Boolean
        Get
            Return _EnabledCalc
        End Get
        Set(value As Boolean)
            Enabled = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _allowpassword
        End Get
        Set(ByVal value As Boolean)
            TB.UseSystemPasswordChar = UseSystemPasswordChar
            _allowpassword = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property MaxLength() As Integer
        Get
            Return _maxChars
        End Get
        Set(ByVal value As Integer)
            _maxChars = value
            TB.MaxLength = MaxLength
            Invalidate()
        End Set
    End Property

    Public Shadows Property TextAlign() As HorizontalAlignment
        Get
            Return _textAlignment
        End Get
        Set(ByVal value As HorizontalAlignment)
            _textAlignment = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property MultiLine() As Boolean
        Get
            Return _multiLine
        End Get
        Set(ByVal value As Boolean)
            _multiLine = value
            TB.Multiline = value
            OnResize(EventArgs.Empty)
            Invalidate()
        End Set
    End Property

    Public Shadows Property [ReadOnly]() As Boolean
        Get
            Return _readOnly
        End Get
        Set(ByVal value As Boolean)
            _readOnly = value
            If TB IsNot Nothing Then
                TB.ReadOnly = value
            End If
        End Set
    End Property

#End Region

#Region " Control "

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnBackColorChanged(ByVal e As EventArgs)
        MyBase.OnBackColorChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnForeColorChanged(ByVal e As EventArgs)
        MyBase.OnForeColorChanged(e)
        TB.ForeColor = ForeColor
        Invalidate()
    End Sub

    Protected Overrides Sub OnFontChanged(ByVal e As EventArgs)
        MyBase.OnFontChanged(e)
        TB.Font = Font
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        MyBase.OnGotFocus(e)
        TB.Focus()
    End Sub

    Private Sub TextChangeTb() Handles TB.TextChanged
        Text = TB.Text
    End Sub

    Private Sub TextChng() Handles MyBase.TextChanged
        TB.Text = Text
    End Sub

    Public Sub NewTextBox()
        With TB
            .Text = String.Empty
            .BackColor = Color.White
            .ForeColor = Color.FromArgb(66, 78, 90)
            .TextAlign = HorizontalAlignment.Left
            .BorderStyle = BorderStyle.None
            .Location = New Point(3, 3)
            .Font = GlobalFont(FontStyle.Regular, 10)
            .Size = New Size(Width - 3, Height - 3)
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With
    End Sub

    Sub New()
        MyBase.New()
        NewTextBox()
        Controls.Add(TB)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        TextAlign = HorizontalAlignment.Left
        ForeColor = Color.FromArgb(66, 78, 90)
        Font = GlobalFont(FontStyle.Regular, 10)
        Size = New Size(130, 29)
        Enabled = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Parent.BackColor)

        If Enabled Then

            TB.ForeColor = Color.FromArgb(66, 78, 90)

            If State = MouseState.Down Then
                DrawRoundRect(G, FullRectangle(Size, True), 3, Color.FromArgb(44, 156, 218))
            Else
                DrawRoundRect(G, FullRectangle(Size, True), 3, GreyColor(200))
            End If

        Else
            TB.ForeColor = GreyColor(170)
            DrawRoundRect(G, FullRectangle(Size, True), 3, GreyColor(230))
        End If

        TB.TextAlign = TextAlign
        TB.UseSystemPasswordChar = UseSystemPasswordChar

    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If Not MultiLine Then
            Dim tbheight As Integer = TB.Height
            TB.Location = New Point(10, CType(((Height / 2) - (tbheight / 2) - 0), Integer))
            TB.Size = New Size(Width - 20, tbheight)
        Else
            TB.Location = New Point(10, 10)
            TB.Size = New Size(Width - 20, Height - 20)
        End If
    End Sub

    Protected Overrides Sub OnEnter(e As EventArgs)
        MyBase.OnEnter(e)
        State = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)
        MyBase.OnLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

End Class

Class NumericUpDownX
    Inherits Control

#Region " Private "
    Private G As Graphics
    Private _Value As Integer
    Private _Min As Integer
    Private _Max As Integer
    Private Loc As Point
    Private Down As Boolean
    Private _EnabledCalc As Boolean
    Private ETC As Color = Nothing
#End Region

#Region " Properties "

    Public Shadows Property Enabled As Boolean
        Get
            Return EnabledCalc
        End Get
        Set(value As Boolean)
            _EnabledCalc = value
            Invalidate()
        End Set
    End Property

    <DisplayName("Enabled")>
    Public Property EnabledCalc As Boolean
        Get
            Return _EnabledCalc
        End Get
        Set(value As Boolean)
            Enabled = value
            Invalidate()
        End Set
    End Property

    Public Property Value As Integer
        Get
            Return _Value
        End Get
        Set(v As Integer)

            If v <= _Max And v >= Minimum Then
                _Value = v
            End If

            Invalidate()

        End Set
    End Property

    Public Property Minimum As Integer
        Get
            Return _Min
        End Get
        Set(v As Integer)

            If v < Maximum Then
                _Min = v
            End If

            If Value < Minimum Then
                Value = Minimum
            End If

            Invalidate()
        End Set
    End Property

    Public Property Maximum As Integer
        Get
            Return _Max
        End Get
        Set(v As Integer)

            If v > Minimum Then
                _Max = v
            End If

            If Value > Maximum Then
                Value = Maximum
            End If

            Invalidate()
        End Set
    End Property

#End Region

#Region " Control "

    Sub New()
        DoubleBuffered = True
        Value = 0
        Minimum = 0
        Maximum = 100
        Cursor = Cursors.IBeam
        BackColor = Color.White
        ForeColor = Color.FromArgb(66, 78, 90)
        Font = GlobalFont(FontStyle.Regular, 10)
        Enabled = True
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        Loc.X = e.X
        Loc.Y = e.Y
        Invalidate()

        If Loc.X < Width - 23 Then
            Cursor = Cursors.IBeam
        Else
            Cursor = Cursors.Default
        End If

    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Height = 30
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If Enabled Then

            If Loc.X > Width - 21 AndAlso Loc.X < Width - 3 Then
                If Loc.Y < 15 Then
                    If (Value + 1) <= Maximum Then
                        Value += 1
                    End If
                Else
                    If (Value - 1) >= Minimum Then
                        Value -= 1
                    End If
                End If
            Else
                Down = Not Down
                Focus()
            End If

        End If

        Invalidate()
    End Sub

    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        MyBase.OnKeyPress(e)
        Try
            If Down Then
                Value = Value & e.KeyChar.ToString
            End If

            If Value > Maximum Then
                Value = Maximum
            End If

        Catch
        End Try
    End Sub
    ' Get more free themes at ThemesVB.NET
    Protected Overrides Sub OnKeyup(ByVal e As System.Windows.Forms.KeyEventArgs)
        MyBase.OnKeyUp(e)

        If e.KeyCode = Keys.Up Then

            If (Value + 1) <= Maximum Then
                Value += 1
            End If

            Invalidate()

        ElseIf e.KeyCode = Keys.Down Then

            If (Value - 1) >= Minimum Then
                Value -= 1
            End If

        ElseIf e.KeyCode = Keys.Back Then
            Dim BC As String = Value.ToString()
            BC = BC.Remove(Convert.ToInt32(BC.Length - 1))

            If (BC.Length = 0) Then
                BC = "0"
            End If

            Value = Convert.ToInt32(BC)

        End If

        Invalidate()

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Parent.BackColor)

        If Enabled Then

            ETC = Color.FromArgb(66, 78, 90)

            Using P As New Pen(GreyColor(190))
                DrawRoundRect(G, FullRectangle(Size, True), 2, GreyColor(190))
                G.DrawLine(P, New Point(Width - 24, 13.5F), New Point(Width - 5, 13.5F))
            End Using

            DrawRoundRect(G, New Rectangle(Width - 24, 4, 19, 21), 3, GreyColor(200))

        Else
            ETC = GreyColor(170)

            Using P As New Pen(GreyColor(230))
                DrawRoundRect(G, FullRectangle(Size, True), 2, GreyColor(190))
                G.DrawLine(P, New Point(Width - 24, 13.5F), New Point(Width - 5, 13.5F))
            End Using

            DrawRoundRect(G, New Rectangle(Width - 24, 4, 19, 21), 3, GreyColor(220))

        End If
        ' Get more free themes at ThemesVB.NET
        Using B As New SolidBrush(ETC)
            G.DrawString("t", New Font("Marlett", 8, FontStyle.Bold), B, New Point(Width - 22, 5))
            G.DrawString("u", New Font("Marlett", 8, FontStyle.Bold), B, New Point(Width - 22, 13))
            CenterString(G, Value, New Font("Segoe UI", 10), ETC, New Rectangle(Width / 2 - 10, 0, Width - 5, Height))
        End Using

    End Sub

#End Region

End Class