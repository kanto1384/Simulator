for (int row = 2; row <= rowCount; row++) // 헤더 제외
{
    string[] cells = new string[3];
    bool contains725 = false;

    for (int col = 2; col <= 4; col++) // B~D = 2~4
    {
        var cell = usedRange.Cells[row, col] as Excel.Range;
        string value = cell?.Text?.ToString() ?? "";
        cells[col - 2] = value;

        if (value.Contains("7.2.5"))
        {
            contains725 = true;
        }

        if (cell != null) Marshal.ReleaseComObject(cell);
    }

    if (contains725)
    {
        string joined = string.Join(" | ", cells);
        matchedValues.Add(joined);
    }
}

foreach (var val in matchedValues)
{
    // B~D 열 데이터를 구분자로 나눈 배열
    var tokens = val.Split('|').Select(x => x.Trim()).ToArray();

    bool hasO = tokens.Any(t => t == "O");
    bool hasX = tokens.Any(t => t == "X");

    if (hasO) countO++;
    else if (hasX) countX++;
    else countNull++;
}


