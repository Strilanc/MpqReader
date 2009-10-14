<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmReader
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tabs = New System.Windows.Forms.TabControl()
        Me.tabControls = New System.Windows.Forms.TabPage()
        Me.btnRepack = New System.Windows.Forms.Button()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.btnSearchMapForListFiles = New System.Windows.Forms.Button()
        Me.lblArchiveResult = New System.Windows.Forms.Label()
        Me.btnExtractAll = New System.Windows.Forms.Button()
        Me.btnExtractFileIndex = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.numArchivedFileIndex = New System.Windows.Forms.NumericUpDown()
        Me.btnExtractFileName = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtArchivedFileName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnArchiveBrowse = New System.Windows.Forms.Button()
        Me.txtArchive = New System.Windows.Forms.TextBox()
        Me.lblListFile = New System.Windows.Forms.Label()
        Me.btnListfileBrowse = New System.Windows.Forms.Button()
        Me.tabHashTable = New System.Windows.Forms.TabPage()
        Me.gridHashTable = New System.Windows.Forms.DataGridView()
        Me.tabBlockTable = New System.Windows.Forms.TabPage()
        Me.gridFileTable = New System.Windows.Forms.DataGridView()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ColHashIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLanguage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colHash = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colPosition = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCompressedSize = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSize = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFlags = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tabs.SuspendLayout()
        Me.tabControls.SuspendLayout()
        CType(Me.numArchivedFileIndex, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabHashTable.SuspendLayout()
        CType(Me.gridHashTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabBlockTable.SuspendLayout()
        CType(Me.gridFileTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabs
        '
        Me.tabs.Controls.Add(Me.tabControls)
        Me.tabs.Controls.Add(Me.tabHashTable)
        Me.tabs.Controls.Add(Me.tabBlockTable)
        Me.tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabs.Location = New System.Drawing.Point(0, 0)
        Me.tabs.Name = "tabs"
        Me.tabs.SelectedIndex = 0
        Me.tabs.Size = New System.Drawing.Size(483, 455)
        Me.tabs.TabIndex = 0
        '
        'tabControls
        '
        Me.tabControls.AutoScroll = True
        Me.tabControls.Controls.Add(Me.btnRepack)
        Me.tabControls.Controls.Add(Me.txtLog)
        Me.tabControls.Controls.Add(Me.btnSearchMapForListFiles)
        Me.tabControls.Controls.Add(Me.lblArchiveResult)
        Me.tabControls.Controls.Add(Me.btnExtractAll)
        Me.tabControls.Controls.Add(Me.btnExtractFileIndex)
        Me.tabControls.Controls.Add(Me.Label4)
        Me.tabControls.Controls.Add(Me.numArchivedFileIndex)
        Me.tabControls.Controls.Add(Me.btnExtractFileName)
        Me.tabControls.Controls.Add(Me.Label3)
        Me.tabControls.Controls.Add(Me.txtArchivedFileName)
        Me.tabControls.Controls.Add(Me.Label2)
        Me.tabControls.Controls.Add(Me.btnArchiveBrowse)
        Me.tabControls.Controls.Add(Me.txtArchive)
        Me.tabControls.Controls.Add(Me.lblListFile)
        Me.tabControls.Controls.Add(Me.btnListfileBrowse)
        Me.tabControls.Location = New System.Drawing.Point(4, 22)
        Me.tabControls.Name = "tabControls"
        Me.tabControls.Padding = New System.Windows.Forms.Padding(3)
        Me.tabControls.Size = New System.Drawing.Size(475, 429)
        Me.tabControls.TabIndex = 3
        Me.tabControls.Text = "Controls"
        Me.tabControls.UseVisualStyleBackColor = True
        '
        'btnRepack
        '
        Me.btnRepack.Location = New System.Drawing.Point(125, 267)
        Me.btnRepack.Name = "btnRepack"
        Me.btnRepack.Size = New System.Drawing.Size(117, 20)
        Me.btnRepack.TabIndex = 17
        Me.btnRepack.Text = "Repack"
        Me.btnRepack.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLog.Location = New System.Drawing.Point(0, 293)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLog.Size = New System.Drawing.Size(475, 136)
        Me.txtLog.TabIndex = 16
        Me.txtLog.WordWrap = False
        '
        'btnSearchMapForListFiles
        '
        Me.btnSearchMapForListFiles.Location = New System.Drawing.Point(11, 45)
        Me.btnSearchMapForListFiles.Name = "btnSearchMapForListFiles"
        Me.btnSearchMapForListFiles.Size = New System.Drawing.Size(174, 20)
        Me.btnSearchMapForListFiles.TabIndex = 15
        Me.btnSearchMapForListFiles.Text = "Search Wc3 Map for Filenames"
        Me.btnSearchMapForListFiles.UseVisualStyleBackColor = True
        '
        'lblArchiveResult
        '
        Me.lblArchiveResult.AutoSize = True
        Me.lblArchiveResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblArchiveResult.Location = New System.Drawing.Point(11, 124)
        Me.lblArchiveResult.Name = "lblArchiveResult"
        Me.lblArchiveResult.Size = New System.Drawing.Size(73, 15)
        Me.lblArchiveResult.TabIndex = 14
        Me.lblArchiveResult.Text = "File not found"
        '
        'btnExtractAll
        '
        Me.btnExtractAll.Enabled = False
        Me.btnExtractAll.Location = New System.Drawing.Point(11, 267)
        Me.btnExtractAll.Name = "btnExtractAll"
        Me.btnExtractAll.Size = New System.Drawing.Size(108, 20)
        Me.btnExtractAll.TabIndex = 12
        Me.btnExtractAll.Text = "Extract All Files"
        Me.btnExtractAll.UseVisualStyleBackColor = True
        '
        'btnExtractFileIndex
        '
        Me.btnExtractFileIndex.Enabled = False
        Me.btnExtractFileIndex.Location = New System.Drawing.Point(101, 239)
        Me.btnExtractFileIndex.Name = "btnExtractFileIndex"
        Me.btnExtractFileIndex.Size = New System.Drawing.Size(97, 20)
        Me.btnExtractFileIndex.TabIndex = 11
        Me.btnExtractFileIndex.Text = "Extract"
        Me.btnExtractFileIndex.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 225)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "File Index"
        '
        'numArchivedFileIndex
        '
        Me.numArchivedFileIndex.Location = New System.Drawing.Point(11, 241)
        Me.numArchivedFileIndex.Name = "numArchivedFileIndex"
        Me.numArchivedFileIndex.Size = New System.Drawing.Size(83, 20)
        Me.numArchivedFileIndex.TabIndex = 9
        '
        'btnExtractFileName
        '
        Me.btnExtractFileName.Enabled = False
        Me.btnExtractFileName.Location = New System.Drawing.Point(371, 201)
        Me.btnExtractFileName.Name = "btnExtractFileName"
        Me.btnExtractFileName.Size = New System.Drawing.Size(97, 20)
        Me.btnExtractFileName.TabIndex = 8
        Me.btnExtractFileName.Text = "Extract"
        Me.btnExtractFileName.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 186)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "File Name"
        '
        'txtArchivedFileName
        '
        Me.txtArchivedFileName.Location = New System.Drawing.Point(11, 202)
        Me.txtArchivedFileName.Name = "txtArchivedFileName"
        Me.txtArchivedFileName.Size = New System.Drawing.Size(353, 20)
        Me.txtArchivedFileName.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Archive"
        '
        'btnArchiveBrowse
        '
        Me.btnArchiveBrowse.Location = New System.Drawing.Point(371, 124)
        Me.btnArchiveBrowse.Name = "btnArchiveBrowse"
        Me.btnArchiveBrowse.Size = New System.Drawing.Size(97, 20)
        Me.btnArchiveBrowse.TabIndex = 4
        Me.btnArchiveBrowse.Text = "Browse"
        Me.btnArchiveBrowse.UseVisualStyleBackColor = True
        '
        'txtArchive
        '
        Me.txtArchive.AllowDrop = True
        Me.txtArchive.Location = New System.Drawing.Point(11, 98)
        Me.txtArchive.Name = "txtArchive"
        Me.txtArchive.Size = New System.Drawing.Size(457, 20)
        Me.txtArchive.TabIndex = 3
        '
        'lblListFile
        '
        Me.lblListFile.AutoSize = True
        Me.lblListFile.Location = New System.Drawing.Point(8, 3)
        Me.lblListFile.Name = "lblListFile"
        Me.lblListFile.Size = New System.Drawing.Size(36, 13)
        Me.lblListFile.TabIndex = 2
        Me.lblListFile.Text = "Listfile"
        '
        'btnListfileBrowse
        '
        Me.btnListfileBrowse.Location = New System.Drawing.Point(11, 19)
        Me.btnListfileBrowse.Name = "btnListfileBrowse"
        Me.btnListfileBrowse.Size = New System.Drawing.Size(174, 20)
        Me.btnListfileBrowse.TabIndex = 1
        Me.btnListfileBrowse.Text = "Import List File"
        Me.btnListfileBrowse.UseVisualStyleBackColor = True
        '
        'tabHashTable
        '
        Me.tabHashTable.Controls.Add(Me.gridHashTable)
        Me.tabHashTable.Location = New System.Drawing.Point(4, 22)
        Me.tabHashTable.Name = "tabHashTable"
        Me.tabHashTable.Padding = New System.Windows.Forms.Padding(3)
        Me.tabHashTable.Size = New System.Drawing.Size(475, 429)
        Me.tabHashTable.TabIndex = 0
        Me.tabHashTable.Text = "Hash Table"
        Me.tabHashTable.UseVisualStyleBackColor = True
        '
        'gridHashTable
        '
        Me.gridHashTable.AllowUserToAddRows = False
        Me.gridHashTable.AllowUserToDeleteRows = False
        Me.gridHashTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridHashTable.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColHashIndex, Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.colLanguage, Me.colHash})
        Me.gridHashTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridHashTable.Location = New System.Drawing.Point(3, 3)
        Me.gridHashTable.Name = "gridHashTable"
        Me.gridHashTable.ReadOnly = True
        Me.gridHashTable.Size = New System.Drawing.Size(469, 423)
        Me.gridHashTable.TabIndex = 2
        '
        'tabBlockTable
        '
        Me.tabBlockTable.Controls.Add(Me.gridFileTable)
        Me.tabBlockTable.Location = New System.Drawing.Point(4, 22)
        Me.tabBlockTable.Name = "tabBlockTable"
        Me.tabBlockTable.Padding = New System.Windows.Forms.Padding(3)
        Me.tabBlockTable.Size = New System.Drawing.Size(475, 429)
        Me.tabBlockTable.TabIndex = 1
        Me.tabBlockTable.Text = "Block Table"
        Me.tabBlockTable.UseVisualStyleBackColor = True
        '
        'gridFileTable
        '
        Me.gridFileTable.AllowUserToAddRows = False
        Me.gridFileTable.AllowUserToDeleteRows = False
        Me.gridFileTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridFileTable.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colIndex, Me.colName, Me.colPosition, Me.colCompressedSize, Me.colSize, Me.colFlags})
        Me.gridFileTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridFileTable.Location = New System.Drawing.Point(3, 3)
        Me.gridFileTable.Name = "gridFileTable"
        Me.gridFileTable.ReadOnly = True
        Me.gridFileTable.Size = New System.Drawing.Size(469, 423)
        Me.gridFileTable.TabIndex = 1
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 100
        Me.ToolTip1.ReshowDelay = 100
        '
        'ColHashIndex
        '
        Me.ColHashIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.ColHashIndex.HeaderText = "ID"
        Me.ColHashIndex.Name = "ColHashIndex"
        Me.ColHashIndex.ReadOnly = True
        Me.ColHashIndex.Width = 43
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn1.HeaderText = "Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 60
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn2.HeaderText = "Block Index"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 88
        '
        'colLanguage
        '
        Me.colLanguage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.colLanguage.HeaderText = "Language"
        Me.colLanguage.Name = "colLanguage"
        Me.colLanguage.ReadOnly = True
        Me.colLanguage.Width = 80
        '
        'colHash
        '
        Me.colHash.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.colHash.HeaderText = "Hashed Filename"
        Me.colHash.Name = "colHash"
        Me.colHash.ReadOnly = True
        Me.colHash.Width = 114
        '
        'colIndex
        '
        Me.colIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.colIndex.HeaderText = "Block Index"
        Me.colIndex.Name = "colIndex"
        Me.colIndex.ReadOnly = True
        Me.colIndex.Width = 88
        '
        'colName
        '
        Me.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.colName.HeaderText = "Name"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        Me.colName.Width = 60
        '
        'colPosition
        '
        Me.colPosition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.colPosition.HeaderText = "Offset"
        Me.colPosition.Name = "colPosition"
        Me.colPosition.ReadOnly = True
        Me.colPosition.Width = 60
        '
        'colCompressedSize
        '
        Me.colCompressedSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.colCompressedSize.HeaderText = "Length"
        Me.colCompressedSize.Name = "colCompressedSize"
        Me.colCompressedSize.ReadOnly = True
        Me.colCompressedSize.Width = 65
        '
        'colSize
        '
        Me.colSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.colSize.HeaderText = "File Size"
        Me.colSize.Name = "colSize"
        Me.colSize.ReadOnly = True
        Me.colSize.Width = 71
        '
        'colFlags
        '
        Me.colFlags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.colFlags.HeaderText = "Properties"
        Me.colFlags.Name = "colFlags"
        Me.colFlags.ReadOnly = True
        Me.colFlags.Width = 79
        '
        'FrmReader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(483, 455)
        Me.Controls.Add(Me.tabs)
        Me.Name = "FrmReader"
        Me.Text = "Mpq Reader"
        Me.tabs.ResumeLayout(False)
        Me.tabControls.ResumeLayout(False)
        Me.tabControls.PerformLayout()
        CType(Me.numArchivedFileIndex, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabHashTable.ResumeLayout(False)
        CType(Me.gridHashTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabBlockTable.ResumeLayout(False)
        CType(Me.gridFileTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabs As System.Windows.Forms.TabControl
    Friend WithEvents tabHashTable As System.Windows.Forms.TabPage

    Friend WithEvents tabBlockTable As System.Windows.Forms.TabPage
    Friend WithEvents gridFileTable As System.Windows.Forms.DataGridView
    Friend WithEvents gridHashTable As System.Windows.Forms.DataGridView




    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colLanguage As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colHash As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colIndex As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSize As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCompressedSize As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPosition As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFlags As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tabControls As System.Windows.Forms.TabPage
    Friend WithEvents lblListFile As System.Windows.Forms.Label
    Friend WithEvents btnListfileBrowse As System.Windows.Forms.Button

    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnArchiveBrowse As System.Windows.Forms.Button
    Friend WithEvents txtArchive As System.Windows.Forms.TextBox
    Friend WithEvents btnExtractAll As System.Windows.Forms.Button
    Friend WithEvents btnExtractFileIndex As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents numArchivedFileIndex As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnExtractFileName As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtArchivedFileName As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

    Friend WithEvents lblArchiveResult As System.Windows.Forms.Label
    Friend WithEvents btnSearchMapForListFiles As System.Windows.Forms.Button
    Friend WithEvents txtLog As System.Windows.Forms.TextBox

    Friend WithEvents btnRepack As System.Windows.Forms.Button
    Friend WithEvents ColHashIndex As System.Windows.Forms.DataGridViewTextBoxColumn




















End Class
