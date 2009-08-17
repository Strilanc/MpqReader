Imports Mpq
Imports Strilbrary.Threading
Imports Strilbrary.Threading.Futures
Imports Strilbrary.Threading.Queueing
Imports Strilbrary

Public Class Form1
    Private curArchive As MpqArchive
    Private internalListFile As New Dictionary(Of ULong, String)
    Private ReadOnly ref As New InvokedCallQueue(Me)

    Private Function LoadInternalListFile() As IFuture(Of Exception)
        Return ImportListFile(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                              IO.Path.DirectorySeparatorChar +
                              "MpqReader" +
                              IO.Path.DirectorySeparatorChar +
                              "listfile.txt")
    End Function
    Private Function ImportListFile(ByVal path As String) As IFuture(Of Exception)
        Return ThreadPooledFunc(
            Function()
                Dim d = New Dictionary(Of ULong, String)
                Try
                    Using f = New IO.FileStream(path, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
                        Mpq.Common.readStreamMPQListFile(f, d)
                    End Using

                    Return ref.QueueFunc(
                        Function() As Exception
                            UpdateInternalListFile(d.Values)
                            Return Nothing
                        End Function)
                Catch e As Exception
                    Return e.Futurize
                End Try
            End Function).Defuturize
    End Function
    Private Function SaveInternalListFile() As IFuture(Of Exception)
        Dim keys = internalListFile.Values.ToArray()

        Return ThreadPooledFunc(
            Function()
                Try
                    Dim path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                    path += IO.Path.DirectorySeparatorChar + "MpqReader"
                    If Not IO.Directory.Exists(path) Then  IO.Directory.CreateDirectory(path)
                    path += IO.Path.DirectorySeparatorChar + "listfile.txt"
                    Using w = New IO.StreamWriter(New IO.FileStream(path, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None))
                        For Each key In keys
                            w.WriteLine(key)
                        Next key
                    End Using
                    Log("Internal list file saved.")
                    Return Nothing
                Catch e As Exception
                    Return e
                End Try
            End Function)
    End Function
    Private Function SearchMapArchiveForListFileEntries(ByVal archive As MpqArchive) As IFuture(Of IEnumerable(Of String))
        Return ThreadPooledFunc(
            Function()
                Dim L = New HashSet(Of String)

                Dim TryAdd As Action(Of String, String)
                TryAdd = Sub(filename As String, item As String)
                             If archive.hashTable.contains(item) AndAlso Not L.Contains(item) Then
                                 Log("{0} referenced valid filename ""{1}""".frmt(filename, item))
                                 L.Add(item)
                             End If
                             If item.ToLower Like "*.mdl" Then
                                 Call TryAdd(filename, item.Substring(0, item.Length - 1) + "x")
                             End If
                             If item.ToLower() Like "ReplaceableTextures\CommandButtons\*".ToLower Then
                                 Call TryAdd(filename, "ReplaceableTextures\CommandButtonsDisabled\DIS" + item.Substring("ReplaceableTextures\CommandButtons\".Length))
                             End If
                             If item Like "*""*" Then
                                 TryAdd(filename, item.Substring(item.IndexOf("""") + 1))
                             End If
                             If item Like "*""" Then
                                 TryAdd(filename, item.Substring(0, item.Length - 1))
                             End If
                             If item.IndexOf("="c) >= 0 Then
                                 TryAdd(filename, item.Substring(item.IndexOf("="c) + 1))
                             End If
                         End Sub

                'Search string files
                For Each filename In {"(listfile)",
                                      "Units\CampaignAbilityFunc.txt",
                                      "Units\CampaignUnitFunc.txt",
                                      "Units\CampaignUpgradeFunc.txt",
                                      "Units\ItemFunc.txt",
                                      "Units\ItemAbilityFunc.txt",
                                      "Units\CommonAbilityFunc.txt",
                                      "Units\CommonUnitFunc.txt",
                                      "Units\CommonUpgradeFunc.txt",
                                      "Units\NeutralUnitFunc.txt",
                                      "Units\NeutralUpgradeFunc.txt",
                                      "Units\NeutralAbilityFunc.txt",
                                      "Units\NightElfUnitFunc.txt",
                                      "Units\NightElfUpgradeFunc.txt",
                                      "Units\NightElfAbilityFunc.txt",
                                      "Units\OrcUnitFunc.txt",
                                      "Units\OrcUpgradeFunc.txt",
                                      "Units\OrcAbilityFunc.txt",
                                      "Units\HumanUnitFunc.txt",
                                      "Units\HumanUpgradeFunc.txt",
                                      "Units\HumanAbilityFunc.txt",
                                      "Units\UndeadUnitFunc.txt",
                                      "Units\UndeadUpgradeFunc.txt",
                                      "Units\UndeadAbilityFunc.txt",
                                      "Units\ItemData.slk",
                                      "Units\AbilityData.slk",
                                      "Units\AbilityBuffData.slk",
                                      "Units\UnitAbilities.slk",
                                      "Units\UnitBalance.slk",
                                      "Units\UnitData.slk",
                                      "Units\UnitUI.slk",
                                      "Units\UnitWeapons.slk",
                                      "Units\UpgradeData.slk",
                                      "war3map.wts",
                                      "war3mapSkin.txt"}
                    If Not archive.hashTable.contains(filename) Then  Continue For
                    Log("Searching {0}...".frmt(filename))
                    Try
                        Using s = New IO.StreamReader(archive.OpenFile(filename))
                            Do Until s.EndOfStream
                                TryAdd(filename, s.ReadLine())
                            Loop
                        End Using
                    Catch e As Exception
                        'ignore
                    End Try
                Next filename

                'Search script
                For Each filename In {"war3map.j", "scripts\war3map.j", "blizzard.j", "common.j", "scripts\blizzard.j", "scripts\common.j"}
                    If Not archive.hashTable.contains(filename) Then  Continue For
                    Log("Searching {0}...".frmt(filename))
                    Try
                        Using sr = New IO.StreamReader(New IO.BufferedStream(archive.OpenFile(filename)))
                            Do Until sr.EndOfStream
                                Dim line = sr.ReadLine()
                                Dim ret As String = Nothing
                                Dim escaped = False
                                For i = 0 To line.Length - 1
                                    If ret IsNot Nothing Then
                                        If escaped Then
                                            ret += line(i)
                                            escaped = False
                                        ElseIf line(i) = "\" Then
                                            escaped = True
                                        ElseIf line(i) = """" Then
                                            TryAdd(filename, ret)
                                            ret = Nothing
                                        Else
                                            ret += line(i)
                                        End If
                                    ElseIf line(i) = """" Then
                                        ret = ""
                                        escaped = False
                                    End If
                                Next i
                            Loop
                        End Using
                    Catch e As Exception
                        'ignore
                    End Try
                Next filename

                'Search object data
                For Each filename In {"war3map.w3a",
                                      "war3map.w3h",
                                      "war3map.w3b",
                                      "war3map.w3u",
                                      "war3map.w3i",
                                      "war3map.w3d",
                                      "war3map.w3t",
                                      "war3map.w3q"}
                    If Not archive.hashTable.contains(filename) Then  Continue For
                    Log("Searching {0}...".frmt(filename))
                    Try
                        Using s = New IO.BufferedStream(archive.OpenFile(filename))
                            Dim ret = ""
                            Do
                                Dim c = s.ReadByte()
                                If c = -1 Then  Exit Do
                                If c < 32 Or c >= 128 Then
                                    TryAdd(filename, ret)
                                    ret = ""
                                Else
                                    ret += Chr(c)
                                    If ret.Length >= 1024 Then  ret = ret.Substring(512)
                                End If
                            Loop
                        End Using
                    Catch e As Exception
                        'ignore
                    End Try
                Next filename

                Log("Finished searching map archive.")
                Return L
            End Function)
    End Function
    Private Function UpdateListFile(ByVal oldEntries As Dictionary(Of ULong, String), ByVal newEntries As IEnumerable(Of String)) As Integer
        Dim n = 0
        For Each entry In newEntries
            Dim hash = Mpq.Crypt.HashFileName(entry)
            If oldEntries.ContainsKey(hash) Then Continue For
            oldEntries(hash) = entry
            n += 1
        Next entry
        Return n
    End Function
    Private Sub UpdateInternalListFile(ByVal newEntries As IEnumerable(Of String))
        Dim n = UpdateListFile(internalListFile, newEntries)
        Log("Added {0} new entries to internal list file, totalling {1}.".frmt(n, internalListFile.Count))
        If n > 0 Then
            SaveInternalListFile()
        End If
    End Sub

    Public Sub LoadArchive(ByVal path As String)
        Log("Loading archive at {0}...".frmt(IO.Path.GetFileName(path)))
        gridFileTable.Rows.Clear()
        gridHashTable.Rows.Clear()
        gridArchive.Rows.Clear()
        curArchive = New MpqArchive(path)

        gridArchive.Rows.Add("Filename", IO.Path.GetFileName(path))
        gridArchive.Rows.Add("Path", path)
        gridArchive.Rows.Add("Archive Size", curArchive.archiveSize.ToString + " bytes")
        gridArchive.Rows.Add("File Block Size", curArchive.fileBlockSize.ToString + " bytes")
        gridArchive.Rows.Add("Archive Offset", curArchive.filePosition)
        gridArchive.Rows.Add("File Table Offset", curArchive.fileTablePosition)
        gridArchive.Rows.Add("Hash Table Offset", curArchive.hashTablePosition)
        gridArchive.Rows.Add("File Table Size", curArchive.numFileTableEntries.ToString + " entries")
        gridArchive.Rows.Add("Hash Table Size", curArchive.numHashTableEntries.ToString + " entries")

        Mpq.Common.readMPQListFile(curArchive, internalListFile)
        Dim archive = curArchive

        ThreadPooledAction(
            Sub()
                Log("Exploring file archive...")
                Dim nameMap As New Dictionary(Of UInteger, List(Of String))
                For i = 0 To curArchive.hashTable.hashes.Count - 1
                    Dim i_ = i
                    ref.QueueAction(Sub()
                                        If archive IsNot curArchive Then  Return
                                        Dim entry = curArchive.hashTable.hashes(i_)
                                        Dim status = entry.fileIndex.ToString + " (Invalid)"
                                        Dim name = If(internalListFile.ContainsKey(entry.key), internalListFile(entry.key), "[0x" + entry.key.ToString + "]")
                                        If entry.key = 18446744073709551615UL Then  name = "[Empty]"
                                        If entry.fileIndex < curArchive.fileTable.fileEntries.Count OrElse entry.fileIndex.EnumValueIsDefined Then
                                            status = entry.fileIndex.ToString
                                            If Not nameMap.ContainsKey(entry.fileIndex) Then  nameMap(entry.fileIndex) = New List(Of String)
                                            nameMap(entry.fileIndex).Add(name)
                                        End If
                                        gridHashTable.Rows.Add(name,
                                                           status,
                                                           entry.language,
                                                           entry.key)
                                    End Sub)
                Next i

                Log("Exploring hash table...")
                For i = 0 To curArchive.fileTable.fileEntries.Count - 1
                    Dim i_ = i
                    ref.QueueAction(Sub()
                                        If archive IsNot curArchive Then  Return
                                        Dim entry = curArchive.fileTable.fileEntries(i_)
                                        Dim name As String
                                        Dim names = If(nameMap.ContainsKey(CUInt(i_)), nameMap(CUInt(i_)), New List(Of String))
                                        If Not names.Any Then
                                            name = "[not in hash table]"
                                        ElseIf names.Count = 1 Then
                                            name = names.First
                                        Else
                                            name = "[multiple]: " + String.Join(", ", names.ToArray)
                                        End If

                                        gridFileTable.Rows.Add(name,
                                                               i_,
                                                               entry.actualSize,
                                                               entry.compressedSize,
                                                               entry.filePosition,
                                                               entry.flags.EnumFlagsToString)
                                    End Sub)
                Next i

                Log("Archive loaded.")
            End Sub)
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
        tabArchive.Hide()
        tabFileTable.Hide()
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
                tabArchive.Show()
                tabFileTable.Show()
                tabHashTable.Show()
                numArchivedFileIndex.Maximum = curArchive.fileTable.fileEntries.Count - 1
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
                    Using file As IO.Stream = curArchive.OpenFile(txtArchivedFileName.Text)
                        Mpq.Common.write_stream_to_disk(file, d.FileName)
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
                    Using file As IO.Stream = curArchive.OpenFile(CUInt(numArchivedFileIndex.Value))
                        Mpq.Common.write_stream_to_disk(file, d.FileName)
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
                    Mpq.Common.extractMPQ(d.SelectedPath, curArchive, internalListFile)
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
                    SearchMapArchiveForListFileEntries(New Mpq.MpqArchive(d.FileName)).QueueCallWhenValueReady(ref,
                        Sub(ret) UpdateInternalListFile(ret))
                Catch ex As Exception
                    Log(ex.ToString)
                End Try
                My.Settings.Save()
        End Select
    End Sub

    Private Sub Log(ByVal text As String)
        ref.QueueAction(Sub()
                            txtLog.Text += text + Environment.NewLine
                        End Sub)
    End Sub
End Class
