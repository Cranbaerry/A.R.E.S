Public Class Form1
    Dim PubLog As String
    Dim PriLog As String
    Dim LogFol As String
    Dim PubLogParsed As String
    Dim PriLogParsed As String
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
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog).Replace("Time detected:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse.txt").Replace("Avatar ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse2.txt").Replace("Avatar Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse.txt").Replace("Avatar Description:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse2.txt").Replace("Avatar Author ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse.txt").Replace("Avatar Author Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse2.txt").Replace("Avatar Asset URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse.txt").Replace("Avatar Image URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse2.txt").Replace("Avatar Thumbnail Image URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse.txt").Replace("Avatar Release Status:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "parse2.txt").Replace("Avatar Version:", ""), False)
        My.Computer.FileSystem.DeleteFile(PubLog & "parse2.txt")
        My.Computer.FileSystem.RenameFile(PubLog & "parse.txt", "ParsedPubLog.txt")
        PubLogParsed = (LogFol & "ParsedPubLog.txt")
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog).Replace("Time detected:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse.txt").Replace("Avatar ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse2.txt").Replace("Avatar Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse.txt").Replace("Avatar Description:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse2.txt").Replace("Avatar Author ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse.txt").Replace("Avatar Author Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse2.txt").Replace("Avatar Asset URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse.txt").Replace("Avatar Image URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse2.txt").Replace("Avatar Thumbnail Image URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse.txt").Replace("Avatar Release Status:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "parse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "parse2.txt").Replace("Avatar Version:", ""), False)
        My.Computer.FileSystem.DeleteFile(PriLog & "parse2.txt")
        My.Computer.FileSystem.RenameFile(PriLog & "parse.txt", "ParsedPriLog.txt")
        PriLogParsed = (LogFol & "ParsedPriLog.txt")
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
End Class
