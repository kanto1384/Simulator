private void FitGridColumns(DataGridView grid)
{
    if (grid.Columns.Count == 0) return;

    // 화면 깜빡임/느려짐 방지
    grid.SuspendLayout();
    try
    {
        // 컬럼별 최대폭(원하면 조정)
        const int maxWide = 380;   // EquipmentListText 같은 긴 칼럼용
        const int maxMid  = 260;   // 일반 텍스트
        const int maxNarrow = 180; // IP/Host 같은 짧은 칼럼

        // 1) 우선 “내용에 맞게” 계산
        grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        // 2) 너무 넓어진 건 최대폭으로 제한 + 최소폭도 살짝 주기
        foreach (DataGridViewColumn c in grid.Columns)
        {
            // 기본 최소 폭
            c.MinimumWidth = 80;

            int max = maxMid;

            var name = c.Name ?? "";
            var header = c.HeaderText ?? "";

            // 컬럼명/헤더로 분기 (프로젝트의 실제 컬럼명에 맞춰서)
            if (name.Contains("Ip", StringComparison.OrdinalIgnoreCase) ||
                header.Contains("IP", StringComparison.OrdinalIgnoreCase))
                max = maxNarrow;

            else if (name.Contains("Host", StringComparison.OrdinalIgnoreCase) ||
                     header.Contains("Host", StringComparison.OrdinalIgnoreCase))
                max = maxNarrow;

            else if (name.Contains("EquipmentList", StringComparison.OrdinalIgnoreCase) ||
                     header.Contains("장비", StringComparison.OrdinalIgnoreCase) ||
                     header.Contains("목록", StringComparison.OrdinalIgnoreCase))
                max = maxWide;

            // 최대 폭 제한
            if (c.Width > max) c.Width = max;
        }

        // 3) 긴 텍스트는 잘리더라도 툴팁으로 전체 보이게(선택)
        grid.ShowCellToolTips = true;
        grid.CellToolTipTextNeeded -= Grid_CellToolTipTextNeeded;
        grid.CellToolTipTextNeeded += Grid_CellToolTipTextNeeded;
    }
    finally
    {
        grid.ResumeLayout();
    }
}

private void Grid_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
{
    if (sender is not DataGridView g) return;
    if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

    var v = g.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
    if (!string.IsNullOrWhiteSpace(v) && v.Length > 20) // 길 때만
        e.ToolTipText = v;
}
