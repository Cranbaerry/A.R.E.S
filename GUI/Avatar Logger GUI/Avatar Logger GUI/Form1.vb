Public Class Form1
    Dim PubLog As String
    Dim PriLog As String
    Dim LogFol As String
    Dim PubLogPharsed As String
    Dim PriLogPharsed As String
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
        My.Computer.FileSystem.WriteAllText(PubLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PubLog).Replace("Time detected:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "pharse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "pharse.txt").Replace("Avatar ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "pharse2.txt").Replace("Avatar Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "pharse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "pharse.txt").Replace("Avatar Description:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "pharse2.txt").Replace("Avatar Author ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "pharse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "pharse.txt").Replace("Avatar Author Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "pharse2.txt").Replace("Avatar Asset URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "pharse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "pharse.txt").Replace("Avatar Image URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "pharse2.txt").Replace("Avatar Thumbnail Image URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "pharse2.txt", My.Computer.FileSystem.ReadAllText(PubLog & "pharse.txt").Replace("Avatar Release Status:", ""), False)
        My.Computer.FileSystem.WriteAllText(PubLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PubLog & "pharse2.txt").Replace("Avatar Version:", ""), False)
        My.Computer.FileSystem.DeleteFile(PubLog & "pharse2.txt")
        My.Computer.FileSystem.RenameFile(PubLog & "pharse.txt", "PharsedPubLog.txt")
        PubLogPharsed = (LogFol & "PharsedPubLog.txt")
        My.Computer.FileSystem.WriteAllText(PriLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PriLog).Replace("Time detected:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "pharse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "pharse.txt").Replace("Avatar ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "pharse2.txt").Replace("Avatar Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "pharse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "pharse.txt").Replace("Avatar Description:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "pharse2.txt").Replace("Avatar Author ID:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "pharse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "pharse.txt").Replace("Avatar Author Name:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "pharse2.txt").Replace("Avatar Asset URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "pharse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "pharse.txt").Replace("Avatar Image URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "pharse2.txt").Replace("Avatar Thumbnail Image URL:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "pharse2.txt", My.Computer.FileSystem.ReadAllText(PriLog & "pharse.txt").Replace("Avatar Release Status:", ""), False)
        My.Computer.FileSystem.WriteAllText(PriLog & "pharse.txt", My.Computer.FileSystem.ReadAllText(PriLog & "pharse2.txt").Replace("Avatar Version:", ""), False)
        My.Computer.FileSystem.DeleteFile(PriLog & "pharse2.txt")
        My.Computer.FileSystem.RenameFile(PriLog & "pharse.txt", "PharsedPriLog.txt")
        PriLogPharsed = (LogFol & "PharsedPriLog.txt")
        MessageBox.Show("Logs Pharsed!")
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
