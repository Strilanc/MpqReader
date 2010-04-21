Imports Strilbrary.Threading
Imports Strilbrary.Values
Imports Strilbrary.Streams
Imports Strilbrary.Collections

Public Class FrmReader
    Private curArchive As MPQ.Archive
    Private listFile As New MPQ.ListFile()
    Private ReadOnly ref As New InvokedCallQueue(Me, initiallyStarted:=True)

    Private Function LoadInternalListFile() As Task
        Return ImportListFile(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                              IO.Path.DirectorySeparatorChar +
                              "MpqReader" +
                              IO.Path.DirectorySeparatorChar +
                              "listfile.txt")
    End Function
    Private Function ImportListFile(ByVal path As String) As Task
        Return ThreadPooledFunc(
            Function()
                Dim filenames = New List(Of String)
                Using sr = New IO.StreamReader(New IO.FileStream(path, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read))
                    Do Until sr.EndOfStream
                        filenames.Add(sr.ReadLine)
                    Loop
                End Using

                Return ref.QueueAction(Sub() UpdateInternalListFile(filenames, saveResult:=False))
            End Function).Unwrap
    End Function
    Private Function SaveInternalListFile() As Task
        Dim filenames = listFile.IncludedStrings.ToArray()

        Return ThreadPooledAction(
            Sub()
                Try
                    Dim path = IO.Path.Combine(IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                               "MpqReader"),
                                                               "listfile.txt")
                    If Not IO.Directory.Exists(IO.Path.GetDirectoryName(path)) Then IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(path))
                    Using w = New IO.StreamWriter(New IO.FileStream(path, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None))
                        For Each key In filenames
                            w.WriteLine(key)
                        Next key
                    End Using
                    Log("Internal list file saved.")
                Catch ex As Exception
                    Log(ex.ToString)
                End Try
            End Sub)
    End Function
    Private Sub UpdateInternalListFile(ByVal newEntries As IList(Of String), ByVal saveResult As Boolean)
        Dim n = listFile.IncludeRangeCountAdded(newEntries)
        Log("Added {0} new entries to internal list file, totalling {1}.".Frmt(n, listFile.Count))
        If n > 0 AndAlso saveResult Then
            SaveInternalListFile()
        End If
    End Sub

    Public Sub LoadArchive(ByVal path As String)
        Log("Loading archive at {0}...".frmt(IO.Path.GetFileName(path)))
        gridFileTable.Rows.Clear()
        gridHashTable.Rows.Clear()
        curArchive = MPQ.Archive.FromFile(path)

        Try
            listFile.IncludeArchiveListFile(curArchive)
        Catch e As Exception
        End Try
        Log("Archive loaded.")

        ExploreTables()
    End Sub
    Private Sub ExploreTables()
        Dim archive = curArchive
        tabBlockTable.Show()
        tabHashTable.Show()
        gridFileTable.Rows.Clear()
        gridHashTable.Rows.Clear()

        Log("Exploring file archive...")
        Dim nameMap As New Dictionary(Of UInteger, List(Of String))
        Dim hid = 0
        For Each entry In archive.Hashtable.Entries
            If archive IsNot curArchive Then Return
            Dim status = "{0} (Invalid)".Frmt(entry.BlockIndex)
            Dim name = "[0x{0}]".Frmt(entry.FileKey)
            If listFile.Contains(entry.FileKey) Then
                name = listFile(entry.FileKey)
            Else
                Try
                    Using f = archive.OpenFileInBlock(entry.BlockIndex)
                        name += ".{0}".Frmt(If(TryDetectFileType(f), "?"))
                    End Using
                Catch ex As Exception
                    name += " (error)"
                End Try
            End If
            If entry.FileKey = ULong.MaxValue Then name = "[Empty]"
            If Not entry.Invalid OrElse entry.BlockIndex.EnumValueIsDefined Then
                status = entry.BlockIndex.ToString
                If Not nameMap.ContainsKey(entry.BlockIndex) Then nameMap(entry.BlockIndex) = New List(Of String)
                nameMap(entry.BlockIndex).Add(name)
            End If
            hid += 1
            gridHashTable.Rows.Add(hid, name, status, entry.FileKey, entry.Language)
        Next entry

        Log("Exploring hash table...")
        For i = 0 To curArchive.BlockTable.Size - 1
            If archive IsNot curArchive Then Return
            Dim block = curArchive.BlockTable.TryGetBlock(CUInt(i))
            Contract.Assume(block IsNot Nothing)
            Dim name As String
            Dim names = If(nameMap.ContainsKey(CUInt(i)), nameMap(CUInt(i)), New List(Of String))
            If Not names.Any Then
                name = "[not in hash table]"
            ElseIf names.Count = 1 Then
                name = names.First
            Else
                name = "[multiple]: " + String.Join(", ", names.ToArray)
            End If

            gridFileTable.Rows.Add(i, name, block.Offset, block.Length, block.FileSize, block.Properties.EnumFlagsToString)
        Next i

        Log("Archive explored.")
    End Sub

    Private Sub txtArchive_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles txtArchive.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim args = CType(e.Data.GetData(DataFormats.FileDrop), String())
            txtArchive.Text = args(0)
        End If
    End Sub
    Private Sub txtArchive_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles txtArchive.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub txtArchive_TextChanged() Handles txtArchive.TextChanged
        curArchive = Nothing
        tabBlockTable.Hide()
        tabHashTable.Hide()

        If txtArchive.Text = "" Then
            lblArchiveResult.Text = "Required"
            ToolTip1.SetToolTip(lblArchiveResult, "Required")
        ElseIf IO.File.Exists(txtArchive.Text) Then
            Try
                lblArchiveResult.Text = "Loading..."
                LoadArchive(txtArchive.Text)
                lblArchiveResult.Text = "Loaded"
                ToolTip1.SetToolTip(lblArchiveResult, "Loaded")
                numArchivedFileIndex.Maximum = curArchive.BlockTable.Size - 1
            Catch ex As Exception
                lblArchiveResult.Text = "Error"
                ToolTip1.SetToolTip(lblArchiveResult, ex.ToString())
            End Try
        Else
            lblArchiveResult.Text = "File not Found"
            ToolTip1.SetToolTip(lblArchiveResult, "File not Found")
        End If

        txtArchivedFileName.Enabled = curArchive IsNot Nothing
        btnExtractAll.Enabled = curArchive IsNot Nothing
        btnExtractFileIndex.Enabled = curArchive IsNot Nothing
        numArchivedFileIndex.Enabled = curArchive IsNot Nothing
        btnExtractFileName.Enabled = curArchive IsNot Nothing AndAlso curArchive.hashTable.contains(txtArchivedFileName.Text)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AddHandler Strilbrary.Exceptions.UnexpectedException,
            Sub(exception, context) Log("Unexpected Exception ({0}): {1}".Frmt(context, e))
        ToolTip1.SetToolTip(txtArchive, "The MPQ Archive to open." + vbNewLine + "Type the path, browse, or drag and drop the file.")
        txtArchive.Text = ""
        txtArchive_TextChanged()
        LoadInternalListFile()
    End Sub

    Private Sub btnArchiveBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnArchiveBrowse.Click
        Dim d As New OpenFileDialog()
        d.InitialDirectory = My.Settings.lastArchiveFolder
        d.FileName = ""
        d.ShowDialog()
        If d.FileName <> "" Then
            My.Settings.lastArchiveFolder = IO.Path.GetDirectoryName(d.FileName)
            If txtArchive.Text = d.FileName Then
                txtArchive_TextChanged()
            Else
                txtArchive.Text = d.FileName
            End If
            My.Settings.Save()
        End If
    End Sub
    Private Sub btnListFileBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnListfileBrowse.Click
        Dim d As New OpenFileDialog()
        d.InitialDirectory = My.Settings.lastListFileFolder
        d.FileName = ""
        Select Case d.ShowDialog()
            Case Windows.Forms.DialogResult.OK
                My.Settings.lastListFileFolder = IO.Path.GetDirectoryName(d.FileName)
                My.Settings.Save()
                ImportListFile(d.FileName)
                My.Settings.Save()
        End Select
    End Sub

    Private Sub txtArchivedFileName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtArchivedFileName.TextChanged
        btnExtractFileName.Enabled = curArchive IsNot Nothing AndAlso curArchive.hashTable.contains(txtArchivedFileName.Text)
    End Sub

    Private Sub btnExtractFileName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtractFileName.Click
        Dim d As New SaveFileDialog()
        d.InitialDirectory = My.Settings.lastSaveFolder
        d.OverwritePrompt = True
        d.FileName = txtArchivedFileName.Text
        Select Case d.ShowDialog()
            Case Windows.Forms.DialogResult.OK
                My.Settings.lastSaveFolder = IO.Path.GetDirectoryName(d.FileName)
                Try
                    Using file = curArchive.OpenFileByName(txtArchivedFileName.Text).AsStream
                        file.WriteToFileSystem(d.FileName)
                    End Using
                Catch ex As Exception
                    MsgBox(ex.ToString, MsgBoxStyle.OkOnly, "Error")
                End Try
                My.Settings.Save()
        End Select
    End Sub
    Private Sub btnExtractFileIndex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtractFileIndex.Click
        Dim d As New SaveFileDialog()
        d.InitialDirectory = My.Settings.lastSaveFolder
        d.OverwritePrompt = True
        d.FileName = ""
        Select Case d.ShowDialog()
            Case Windows.Forms.DialogResult.OK
                My.Settings.lastSaveFolder = IO.Path.GetDirectoryName(d.FileName)
                Try
                    Using file = curArchive.OpenFileInBlock(CUInt(numArchivedFileIndex.Value)).AsStream
                        file.WriteToFileSystem(d.FileName)
                    End Using
                Catch ex As Exception
                    Log(ex.ToString)
                End Try
                My.Settings.Save()
        End Select
    End Sub

    Private Sub btnExtractAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtractAll.Click
        Dim d As New FolderBrowserDialog
        d.SelectedPath = My.Settings.lastSaveFolder
        Select Case d.ShowDialog()
            Case Windows.Forms.DialogResult.OK
                My.Settings.lastSaveFolder = d.SelectedPath
                Try
                    curArchive.ExtractAll(d.SelectedPath, listFile)
                Catch ex As Exception
                    Log(ex.ToString)
                End Try
                My.Settings.Save()
        End Select
    End Sub

    Private Sub btnSearchMapForListFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchMapForListFiles.Click
        Dim d As New OpenFileDialog()
        d.InitialDirectory = My.Settings.lastArchiveFolder
        d.FileName = ""
        Select Case d.ShowDialog()
            Case Windows.Forms.DialogResult.OK
                My.Settings.lastArchiveFolder = IO.Path.GetDirectoryName(d.FileName)
                My.Settings.Save()
                Try
                    Call MPQ.Archive.FromFile(d.FileName).AsyncSearchForFilenames(AddressOf Log).QueueContinueWithAction(ref,
                        Sub(ret) UpdateInternalListFile(ret.ToList, saveResult:=True))
                Catch ex As Exception
                    Log(ex.ToString)
                End Try
                My.Settings.Save()
        End Select
    End Sub

    Private Sub Log(ByVal message As String)
        ref.QueueAction(Sub()
                            txtLog.Text = message + Environment.NewLine + txtLog.Text
                        End Sub)
    End Sub

    Private Sub btnRepack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRepack.Click
        Dim d As New SaveFileDialog
        d.InitialDirectory = IO.Path.GetDirectoryName(txtArchive.Text)
        d.FileName = IO.Path.GetFileName(txtArchive.Text)
        Select Case d.ShowDialog()
            Case Windows.Forms.DialogResult.OK
                Try
                    Using lockSource As New IO.FileStream(txtArchive.Text, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                        Using f As New IO.BufferedStream(New IO.FileStream(d.FileName, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None))
                            curArchive.WriteToStream(f)
                        End Using
                    End Using
                Catch ex As Exception
                    Log(ex.ToString)
                End Try
        End Select
    End Sub
End Class
