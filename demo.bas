Attribute VB_Name = "Module1"
Dim t621 As Boolean
Dim t622 As Boolean
Dim t623 As Boolean
Dim t631 As Boolean
Dim t632 As Boolean
Dim t641 As Boolean
Dim t642 As Boolean
Dim t643 As Boolean
Dim t651 As Boolean
Dim t652 As Boolean
Sub Reset()
Attribute Reset.VB_ProcData.VB_Invoke_Func = " \n14"
    Dim G93 As Integer
    Worksheets("Test Result Summary").Columns("J:BG").ColumnWidth = 10.13
    Worksheets("Test Result Summary").Range("J1:BG50").Clear
    
    G93 = 11
    For i = 1 To 6
            Worksheets("6.21").Cells(3, G93).MergeArea.ClearContents
            Worksheets("6.22").Cells(3, G93).MergeArea.ClearContents
            Worksheets("6.23").Cells(3, G93).MergeArea.ClearContents
            Worksheets("6.31").Cells(3, G93).MergeArea.ClearContents
            Worksheets("6.32").Cells(3, G93).MergeArea.ClearContents
            Worksheets("6.41").Cells(3, G93).MergeArea.ClearContents
            Worksheets("6.42").Cells(3, G93).MergeArea.ClearContents
            Worksheets("6.43").Cells(3, G93).MergeArea.ClearContents
            Worksheets("6.51").Cells(3, G93).MergeArea.ClearContents
            G93 = G93 + 1
    Next i
    
    Worksheets("6.21").Cells(56, 3).MergeArea.ClearContents
    Worksheets("6.21").Cells(58, 3).MergeArea.ClearContents
    Worksheets("6.21").Cells(61, 3).MergeArea.ClearContents
    Worksheets("6.21").Cells(63, 3).MergeArea.ClearContents
    
   
   
    G93 = 67
        For i = 0 To 4
            Worksheets("6.22").Cells(G93, 13 + i).MergeArea.ClearContents
            Worksheets("6.22").Cells(G93 + 1, 13 + i).MergeArea.ClearContents
        Next i
    
    G93 = 68
    For j = 1 To 2
        For i = 1 To 7
            Worksheets("6.23").Cells(G93, 3).MergeArea.ClearContents
            G93 = G93 + 1
        Next i
        G93 = 76
    Next j
    MsgBox "All data Clear", vbOKOnly + vbInformation, "Clear"
End Sub
