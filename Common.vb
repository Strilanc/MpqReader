﻿Imports Mpq
Imports Strilbrary.Threading
Imports Strilbrary.Streams
Imports Strilbrary.Values
Imports Strilbrary.Collections

Friend Module Common
    <Extension()>
    Public Function IncludeRangeCountAdded(ByVal this As MPQ.ListFile, ByVal filenames As IEnumerable(Of String)) As Integer
        Contract.Requires(this IsNot Nothing)
        Contract.Requires(filenames IsNot Nothing)
        Contract.Ensures(Contract.Result(Of Integer)() >= 0)
        Dim result = 0
        For Each filename In filenames
            If Not this.Contains(filename) Then
                result += 1
                this.Include(filename)
            End If
        Next filename
        Return result
    End Function

    Public Function AsyncReadListfile(ByVal path As String, ByVal logger As Action(Of String)) As Task(Of MPQ.ListFile)
        Contract.Requires(path IsNot Nothing)
        Contract.Ensures(Contract.Result(Of Task(Of MPQ.ListFile))() IsNot Nothing)
        Return ThreadPooledFunc(
            Function()
                Try
                    Dim result = New MPQ.ListFile
                    Using sr = New IO.StreamReader(New IO.FileStream(path, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read))
                        Do Until sr.EndOfStream
                            result.Include(sr.ReadLine)
                        Loop
                    End Using
                    Return result
                Catch ex As Exception
                    logger(ex.ToString)
                    Return Nothing
                End Try
            End Function)
    End Function

    <Extension()>
    Public Function AsyncSaveListFile(ByVal this As MPQ.ListFile, ByVal logger As Action(Of String)) As Task
        Contract.Requires(this IsNot Nothing)
        Contract.Ensures(Contract.Result(Of Task)() IsNot Nothing)
        Dim filenames = this.IncludedStrings.ToArray()

        Return ThreadPooledAction(
            Sub()
                Try
                    Dim path = IO.Path.Combine(IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                               "MpqReader"),
                                                               "listfile.txt")
                    If Not IO.Directory.Exists(path) Then IO.Directory.CreateDirectory(path)
                    Using w = New IO.StreamWriter(New IO.FileStream(path, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None))
                        For Each key In filenames
                            w.WriteLine(key)
                        Next key
                    End Using
                Catch ex As Exception
                    logger(ex.ToString)
                End Try
            End Sub)
    End Function

    <Extension()> <Pure()>
    Public Function AsyncSearchForFilenames(ByVal archive As MPQ.Archive,
                                            ByVal logger As Action(Of String)) As Task(Of IEnumerable(Of String))
        Return ThreadPooledFunc(
            Function()
                Dim result = New HashSet(Of String)

                Dim adder = Sub(this As HashSet(Of String), items As IEnumerable(Of String))
                                For Each item In items
                                    If Not this.Contains(item) Then
                                        this.Add(item)
                                        logger("Found filename: {0}".Frmt(item))
                                    End If
                                Next item
                            End Sub

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
                    logger("Searching {0}...".Frmt(filename))
                    adder(result, archive.SearchForFilenames_FileLines(filename))
                Next filename

                'Search script
                For Each filename In {"war3map.j",
                                      "scripts\war3map.j",
                                      "blizzard.j",
                                      "common.j",
                                      "scripts\blizzard.j",
                                      "scripts\common.j"}
                    logger("Searching {0}...".Frmt(filename))
                    adder(result, archive.SearchForFilenames_Script(filename))
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
                    logger("Searching {0}...".Frmt(filename))
                    adder(result, archive.SearchForFilenames_ObjectData(filename))
                Next filename

                Return result.AsEnumerable
            End Function)
    End Function
    <Pure()> <Extension()>
    Private Function SearchForFilenames_FileLines(ByVal archive As MPQ.Archive,
                                                  ByVal filename As String) As HashSet(Of String)
        Contract.Requires(archive IsNot Nothing)
        Contract.Requires(filename IsNot Nothing)
        Contract.Ensures(Contract.Result(Of HashSet(Of String))() IsNot Nothing)

        Dim result = New HashSet(Of String)
        If Not archive.hashTable.contains(filename) Then Return result

        Using reader = New IO.StreamReader(archive.OpenFileByName(filename).AsStream)
            Do Until reader.EndOfStream
                result.IncludeRange(archive.SearchForFilenames_Filename(reader.ReadLine()))
            Loop
        End Using

        Return result
    End Function
    <Pure()> <Extension()>
    Private Function SearchForFilenames_Script(ByVal archive As MPQ.Archive, ByVal filename As String) As HashSet(Of String)
        Contract.Requires(archive IsNot Nothing)
        Contract.Requires(filename IsNot Nothing)
        Contract.Ensures(Contract.Result(Of HashSet(Of String))() IsNot Nothing)

        Dim result = New HashSet(Of String)
        If Not archive.hashTable.contains(filename) Then Return result

        Using reader = New IO.StreamReader(New IO.BufferedStream(archive.OpenFileByName(filename).AsStream))
            Do Until reader.EndOfStream
                Dim line = reader.ReadLine()
                Dim quotedString As String = Nothing
                Dim escaped = False
                For Each character In line
                    If quotedString IsNot Nothing Then 'scanning quoted string
                        If escaped Then
                            quotedString += character
                            escaped = False
                        ElseIf character = "\" Then
                            escaped = True
                        ElseIf character = """"c Then
                            result.IncludeRange(archive.SearchForFilenames_Filename(quotedString))
                            quotedString = Nothing
                        Else
                            quotedString += character
                        End If
                    ElseIf character = """"c Then 'scanning code
                        quotedString = ""
                        escaped = False
                    End If
                Next character
            Loop
        End Using

        Return result
    End Function
    <Pure()> <Extension()>
    Private Function SearchForFilenames_ObjectData(ByVal archive As MPQ.Archive, ByVal filename As String) As HashSet(Of String)
        Contract.Requires(archive IsNot Nothing)
        Contract.Requires(filename IsNot Nothing)
        Contract.Ensures(Contract.Result(Of HashSet(Of String))() IsNot Nothing)

        Dim result = New HashSet(Of String)
        If Not archive.hashTable.contains(filename) Then Return result

        Using s = New IO.BufferedStream(archive.OpenFileByName(filename).AsStream)
            Dim curString = New System.Text.StringBuilder
            Do
                Dim c = s.ReadByte()
                If c = -1 Then Exit Do
                If c < 32 Or c >= 128 Then 'invalid character, possibly terminating a string
                    result.IncludeRange(archive.SearchForFilenames_Filename(curString.ToString))
                    curString.Remove(0, curString.Length)
                Else 'valid character, grow the string
                    curString.Append(Chr(c))
                    If curString.Length >= 1024 Then curString.Remove(0, curString.Length \ 2)
                End If
            Loop
        End Using

        Return result
    End Function

    <Extension()>
    Private Sub IncludeRange(Of T)(ByVal this As HashSet(Of T), ByVal items As IEnumerable(Of T))
        Contract.Requires(this IsNot Nothing)
        Contract.Requires(items IsNot Nothing)
        For Each item In items
            this.Add(item)
        Next item
    End Sub

    <Pure()> <Extension()>
    Private Function SearchForFilenames_Filename(ByVal archive As MPQ.Archive, ByVal item As String) As HashSet(Of String)
        Dim result = New HashSet(Of String)

        If archive.hashTable.contains(item) Then
            result.Add(item)
        End If

        'Recurse
        'mdx imports may be referenced as an mdl
        If item.EndsWith(".mdl", StringComparison.InvariantCultureIgnoreCase) Then
            result.IncludeRange(archive.SearchForFilenames_Filename(
                item.Substring(0, item.Length - 4) + ".mdx"))
        End If
        'command button paths implicitely include a disabled invariant path
        If item.StartsWith("ReplaceableTextures\CommandButtons\", StringComparison.InvariantCultureIgnoreCase) Then
            Dim n = "ReplaceableTextures\CommandButtons\".Length
            result.IncludeRange(archive.SearchForFilenames_Filename(
                item.Substring(0, n - 1) + "Disabled\DIS" + item.Substring(n)))
        End If
        'quotes may enclose paths
        If item.Contains(""""c) Then
            result.IncludeRange(archive.SearchForFilenames_Filename(
                item.Substring(item.IndexOf(""""c) + 1)))
        End If
        If item.EndsWith(""""c) Then
            result.IncludeRange(archive.SearchForFilenames_Filename(
                                item.Substring(0, item.Length - 1)))
        End If
        'equal signs may separate names from paths
        If item.IndexOf("="c) >= 0 Then
            result.IncludeRange(archive.SearchForFilenames_Filename(
                item.Substring(item.IndexOf("="c) + 1)))
        End If

        Return result
    End Function

    ''' <summary>
    ''' Extracts all files in the archive.
    ''' </summary>
    <Extension()>
    Public Sub ExtractAll(ByVal archive As MPQ.Archive, ByVal folder As String, ByVal listFile As MPQ.ListFile)
        Contract.Requires(archive IsNot Nothing)
        Contract.Requires(folder IsNot Nothing)
        Contract.Requires(listFile IsNot Nothing)

        listFile.IncludeArchiveListFile(archive)
        For Each h As MPQ.HashEntry In archive.Hashtable.Entries
            If h.BlockIndex = Hashtable.BlockIndex.DeletedFile Then Continue For
            If h.BlockIndex = Hashtable.BlockIndex.NoFile Then Continue For
            Dim mpqFilename = ""
            Dim mpqFileStream As IRandomReadableStream = Nothing
            Try
                'Open file
                If listFile.Contains(h.FileKey) Then
                    mpqFilename = listFile(h.FileKey)
                    Contract.Assume(mpqFilename IsNot Nothing)
                    mpqFileStream = archive.OpenFileByName(mpqFilename)
                Else
                    mpqFileStream = archive.OpenFileInBlock(h.BlockIndex)
                    Dim ext = TryDetectFileType(mpqFileStream)
                    mpqFileStream.Position = 0
                    mpqFilename = "Unknown{0}".Frmt(h.FileKey) + If(ext IsNot Nothing, "." + ext, "")
                End If

                'Create sub directories as necessary
                Dim subfolders = mpqFilename.Split("\"c)
                Dim curFolder = folder
                For Each subFolder In subfolders.SkipLast(1)
                    curFolder = IO.Path.Combine(curFolder, subFolder)
                    If Not IO.Directory.Exists(curFolder) Then IO.Directory.CreateDirectory(curFolder)
                Next subFolder

                'Write to file
                Dim buffer(0 To 511) As Byte
                mpqFileStream.AsStream.WriteToFileSystem(IO.Path.Combine(curFolder, mpqFilename.Split("\"c).Last))
                Debug.Print("Extracted {0}".Frmt(mpqFilename))
            Catch e As Exception
                Debug.Print("Exception hit in {0}: {1}".Frmt(mpqFilename, e))
            End Try
            If mpqFileStream IsNot Nothing Then mpqFileStream.Dispose()
        Next h
    End Sub

    Public Function TryDetectFileType(ByVal stream As IRandomReadableStream) As String
        If stream.Position <> 0 Then stream.Position = 0
        Dim start = stream.ReadBestEffort(4).ToAsciiChars.AsString
        If start.StartsWith("BLP") Then
            Return "blp"
        ElseIf start.StartsWith("MDLX") Then
            Return "mdx"
        ElseIf start.StartsWith("ID3") Then
            Return "mp3"
        End If

        stream.Position = Math.Max(stream.Length - 26, 0)
        Dim endData = stream.ReadBestEffort(26).ToAsciiChars.AsString
        If endData.SkipLast(1).AsString.EndsWith("TRUEVISION-XFILE.") Then
            Return "tga"
        End If

        Return Nothing
    End Function
End Module
