Imports System.IO
Public Class Form1
    Dim PubLog As String
    Dim PriLog As String
    Dim LogFol As String
    Dim PubLogParsed As String
    Dim PriLogParsed As String
    Dim CurrentLine As Integer = 2
    Public Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = "Open File Dialog"
        fd.InitialDirectory = "C:\"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        If fd.ShowDialog() = DialogResult.OK Then
            PubLog = fd.FileName
            MessageBox.Show("You selected: " + PubLog)
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
            MessageBox.Show("You selected: " + PriLog)
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
        MessageBox.Show("Logs parsed!")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim Fol As FolderBrowserDialog = New FolderBrowserDialog()
        Fol.Description = "Select a folder"
        If (Fol.ShowDialog() = DialogResult.OK) Then
            LogFol = Fol.SelectedPath
            MessageBox.Show("You selected: " + Fol.SelectedPath)
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click 'Search Button
        Dim Searched As String = TextBox1.Text
        If Searched = "" Then
            MessageBox.Show("Please enter the search field")
            Exit Sub
        End If
        Dim allLines As List(Of String) = New List(Of String)
        Dim ParsedFile As String = PubLogParsed
        Dim Temp As String = "1"

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
            reader.Close() 'Save and close the file
        Dim LineCount = File.ReadAllLines(ParsedFile).Length
        For i = 1 To (LineCount \ 13)
            If ReadLine(CurrentLine, allLines) = Searched Then
                Temp = "1"
                Exit For
            Else
                Temp = "0"
                CurrentLine = CurrentLine + 13
            End If
        Next
        If Temp = "0" Then
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
            Label6.ForeColor = Color.Black
            TextBox15.Text = ""
            TextBox10.Text = ""
            TextBox9.Text = ""
            TextBox8.Text = ""
            WebBrowser1.Navigate("")
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
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PriLogParsed = "F:\SteamLibrary\steamapps\common\VRChat\AvatarLog\ParsedPriLog.txt"
        PubLogParsed = "F:\SteamLibrary\steamapps\common\VRChat\AvatarLog\ParsedPubLog.txt"
        RadioButton4.Checked = True
        RadioButton1.Checked = True
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        WebBrowser1.Size = WebBrowser1.Document.Body.ScrollRectangle.Size
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button5_Click(sender, e)
        End If
    End Sub

    Public Function ReadLine(lineNumber As Integer, lines As List(Of String)) As String 'Function that allows reading lines from a file to work and to keep the ReadLine() command neat and easy the read/write in the code
        Return lines(lineNumber - 1)
    End Function

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        Process.Start(TextBox14.Text)
    End Sub
End Class
