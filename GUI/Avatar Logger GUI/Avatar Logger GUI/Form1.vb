Imports System.IO
Public Class Form1
    Dim PubLog As String
    Dim PriLog As String
    Dim LogFol As String
    Dim PubLogParsed As String
    Dim PriLogParsed As String
    Dim CurrentLine As Integer = 2
    Dim FoundLine As Integer = 0
    Dim NextLine As Integer = 0
    Dim NextButton As Integer = 0
    Dim LabelLog As Integer = 1
    Public Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = "Open File Dialog"
        fd.InitialDirectory = "C:\"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        If fd.ShowDialog() = DialogResult.OK Then
            PubLog = fd.FileName
            My.Settings.PublicLog = PubLog
            MessageBox.Show("(SAVED)You selected: " + PubLog)
        End If
    End Sub

    Public Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = "Open File Dialog"
        fd.InitialDirectory = "C:\"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        If fd.ShowDialog() = DialogResult.OK Then
            PriLog = fd.FileName
            My.Settings.PrivateLog = PriLog
            MessageBox.Show("(SAVED)You selected: " + PriLog)
        End If
    End Sub

    Public Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog).Replace("Time Detected:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse.txt").Replace("Avatar ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse2.txt").Replace("Avatar Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse.txt").Replace("Avatar Description:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse2.txt").Replace("Author ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse.txt").Replace("Author Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse2.txt").Replace("Asset URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse.txt").Replace("Image URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse2.txt").Replace("Thumbnail URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse.txt").Replace("Release Status:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse2.txt").Replace("Version:", ""), False)
        My.Computer.FileSystem.DeleteFile(PubLog & "parse2.txt")
        My.Computer.FileSystem.RenameFile(PubLog & "parse.txt", "ParsedPubLog.txt")
        PubLogParsed = (LogFol & "\ParsedPubLog.txt")
        My.Settings.ParsedPublic = PubLogParsed
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog).Replace("Time Detected:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse.txt").Replace("Avatar ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse2.txt").Replace("Avatar Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse.txt").Replace("Avatar Description:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse2.txt").Replace("Author ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse.txt").Replace("Author Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse2.txt").Replace("Asset URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse.txt").Replace("Image URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse2.txt").Replace("Thumbnail URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse.txt").Replace("Release Status:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse2.txt").Replace("Version:", ""), False)
        My.Computer.FileSystem.DeleteFile(PriLog & "parse2.txt")
        My.Computer.FileSystem.RenameFile(PriLog & "parse.txt", "ParsedPriLog.txt")
        PriLogParsed = (LogFol & "\ParsedPriLog.txt")
        My.Settings.ParsedPrivate = PriLogParsed
        MessageBox.Show("Logs parsed!")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim Fol As FolderBrowserDialog = New FolderBrowserDialog()
        Fol.Description = "Select a folder"
        If (Fol.ShowDialog() = DialogResult.OK) Then
            LogFol = Fol.SelectedPath
            My.Settings.AvatarFolder = LogFol
            MessageBox.Show("(SAVED)You selected: " + Fol.SelectedPath)
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click 'Search Button
        Dim Searched As String = TextBox1.Text
        If PriLog = "" Then
            MessageBox.Show("Assign a private avatar log!")
            Exit Sub
        ElseIf PubLog = "" Then
            MessageBox.Show("Assign a public avatar log!")
            Exit Sub
        ElseIf LogFol = "" Then
            MessageBox.Show("Assign an avatar log folder!")
            Exit Sub
        ElseIf PriLogParsed = "" Then
            MessageBox.Show("Private log not parsed!")
            Exit Sub
        ElseIf PubLogParsed = "" Then
            MessageBox.Show("Public log not parsed!")
        Exit Sub
        End If
        If Searched = "" Then
            MessageBox.Show("Please enter the search field")
            Exit Sub
        End If
        Dim allLines As List(Of String) = New List(Of String)
        Dim ParsedFile As String = PubLogParsed
        If NextButton = 1 Then
            NextButton = 0
            NextLine = 0
        Else
            FoundLine = 0
            NextLine = 0
        End If

        If RadioButton1.Checked Then 'Avatar Name
            CurrentLine = 4
        ElseIf RadioButton2.Checked Then 'Avatar ID
            CurrentLine = 3
        ElseIf RadioButton3.Checked Then 'Avatar Author
            CurrentLine = 7
        End If

        If RadioButton4.Checked Then
            ParsedFile = PubLogParsed
        ElseIf RadioButton5.Checked Then
            ParsedFile = PriLogParsed
        End If
            Dim reader As New System.IO.StreamReader(ParsedFile)
            Do While Not reader.EndOfStream
                allLines.Add(reader.ReadLine())
            Loop
        reader.Close()
        Dim LineCount = File.ReadAllLines(ParsedFile).Length
        ListBox1.Items.Clear()
        For i = 1 To (LineCount \ 12)
            If ReadLine(CurrentLine, allLines) = Searched Then
                Dim NextLineSkip As Integer = 0
                If RadioButton2.Checked Then
                Else
                    ListBox1.Items.Add(Searched)
                End If
                If FoundLine = 0 Then
                    FoundLine = CurrentLine
                    NextLineSkip = 1
                End If
                If NextLineSkip = 0 And NextLine = 0 And CurrentLine > FoundLine Then
                    NextLine = CurrentLine
                End If
                CurrentLine = CurrentLine + 12
            Else
                CurrentLine = CurrentLine + 12
            End If
        Next
        CurrentLine = FoundLine

        If FoundLine = "0" Then
            TextBox2.Text = ""
            TextBox3.Text = ""
            TextBox4.Text = ""
            TextBox5.Text = ""
            TextBox6.Text = ""
            TextBox7.Text = ""
            TextBox8.Text = ""
            TextBox9.Text = ""
            TextBox10.Text = ""
            TextBox14.Text = ""
            Label6.ForeColor = Color.White
            TextBox15.Text = ""
            TextBox10.Text = ""
            TextBox9.Text = ""
            TextBox8.Text = ""
            WebBrowser1.Navigate("")
            Label15.Text = "0/0"
            LabelLog = 1
            MessageBox.Show("Search term not found")
            Exit Sub
        Else
        End If
        Dim TimeDetected As String
        Dim AvatarID As String
        Dim AvatarName As String
        Dim AvatarDecryption As String
        Dim AuthorID As String
        Dim AuthorName As String
        Dim Thumbnail As String
        Dim AssetURL As String
        Dim AssetImage As String
        Dim ReleaseType As String
        Dim AvatarVer As String

        If RadioButton1.Checked Then 'Avatar Name
            TimeDetected = ReadLine((CurrentLine - 2), allLines)
            AvatarID = ReadLine((CurrentLine - 1), allLines)
            AvatarName = ReadLine((CurrentLine), allLines)
            AvatarDecryption = ReadLine((CurrentLine + 1), allLines)
            AuthorID = ReadLine((CurrentLine + 2), allLines)
            AuthorName = ReadLine((CurrentLine + 3), allLines)
            AssetURL = ReadLine((CurrentLine + 4), allLines)
            AssetImage = ReadLine((CurrentLine + 5), allLines)
            Thumbnail = ReadLine((CurrentLine + 6), allLines)
            ReleaseType = ReadLine((CurrentLine + 7), allLines)
            AvatarVer = ReadLine((CurrentLine + 8), allLines)

        ElseIf RadioButton2.Checked Then 'Avatar ID
            TimeDetected = ReadLine((CurrentLine - 1), allLines)
            AvatarID = ReadLine((CurrentLine), allLines)
            AvatarName = ReadLine((CurrentLine + 1), allLines)
            AvatarDecryption = ReadLine((CurrentLine + 2), allLines)
            AuthorID = ReadLine((CurrentLine + 3), allLines)
            AuthorName = ReadLine((CurrentLine + 4), allLines)
            AssetURL = ReadLine((CurrentLine + 5), allLines)
            AssetImage = ReadLine((CurrentLine + 6), allLines)
            Thumbnail = ReadLine((CurrentLine + 7), allLines)
            ReleaseType = ReadLine((CurrentLine + 8), allLines)
            AvatarVer = ReadLine((CurrentLine + 9), allLines)

        ElseIf RadioButton3.Checked Then 'Avatar Author
            TimeDetected = ReadLine((CurrentLine - 5), allLines)
            AvatarID = ReadLine((CurrentLine - 4), allLines)
            AvatarName = ReadLine((CurrentLine - 3), allLines)
            AvatarDecryption = ReadLine((CurrentLine - 2), allLines)
            AuthorID = ReadLine((CurrentLine - 1), allLines)
            AuthorName = ReadLine((CurrentLine), allLines)
            AssetURL = ReadLine((CurrentLine + 1), allLines)
            AssetImage = ReadLine((CurrentLine + 2), allLines)
            Thumbnail = ReadLine((CurrentLine + 3), allLines)
            ReleaseType = ReadLine((CurrentLine + 4), allLines)
            AvatarVer = ReadLine((CurrentLine + 5), allLines)

        End If

        TextBox2.Text = TimeDetected
        TextBox3.Text = AvatarID
        TextBox4.Text = AvatarName
        TextBox5.Text = AvatarDecryption
        TextBox6.Text = AuthorID
        TextBox7.Text = AuthorName
        TextBox14.Text = AssetURL
        TextBox15.Text = AssetImage
        TextBox10.Text = Thumbnail
        TextBox9.Text = ReleaseType
        TextBox8.Text = AvatarVer
        Label6.ForeColor = Color.Blue
        WebBrowser1.Navigate(Thumbnail)
        Label15.Text = (LabelLog & "/" & ListBox1.Items.Count.ToString())
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PriLogParsed = My.Settings.ParsedPrivate
        PubLogParsed = My.Settings.ParsedPublic
        LogFol = My.Settings.AvatarFolder
        PubLog = My.Settings.PublicLog
        PriLog = My.Settings.PrivateLog
        RadioButton4.Checked = True
        RadioButton1.Checked = True
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button5_Click(sender, e)
        End If
    End Sub

    Public Function ReadLine(lineNumber As Integer, lines As List(Of String)) As String
        Return lines(lineNumber - 1)
    End Function

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        Process.Start(TextBox14.Text)
    End Sub
    'Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
    '    WebBrowser1.Size = WebBrowser1.Document.Body.ScrollRectangle.Size
    'End Sub
    Private Sub Label13_Click(sender As Object, e As EventArgs) Handles Label13.Click
        Process.Start("https://github.com/LargestBoi/AvatarLogger-GUI")
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        FoundLine = NextLine
        NextButton = 1
        If LabelLog >= ListBox1.Items.Count() Then
            LabelLog = 0
        End If
        Label15.Text = (LabelLog & "/" & ListBox1.Items.Count.ToString())
        LabelLog = LabelLog + 1
        Button5_Click(sender, e)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        My.Computer.FileSystem.DeleteFile(LogFol & "\ParsedPriLog.txt")
        My.Computer.FileSystem.DeleteFile(LogFol & "\ParsedPubLog.txt")
        MessageBox.Show("Parse reset!")
    End Sub
End Class
