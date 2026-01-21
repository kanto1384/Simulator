private void FitGridColumns(DataGridView grid)
{
    if (grid.Columns.Count == 0) return;

    grid.SuspendLayout();
    try
    {
        // ✅ 사용자 수동 조절 허용
        grid.AllowUserToResizeColumns = true;

        // ✅ "계산용"으로 잠깐 자동 사이즈 켠 다음
        // (각 컬럼 AutoSizeMode를 쓰는 경우가 많아서)
        foreach (DataGridViewColumn c in grid.Columns)
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

        // preferred width 계산
        grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        const int maxWide = 380;
        const int maxMid = 260;
        const int maxNarrow = 180;

        foreach (DataGridViewColumn c in grid.Columns)
        {
            int max = maxMid;
            var name = c.Name ?? "";
            var header = c.HeaderText ?? "";

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

            // ✅ 계산된 폭을 최대폭으로 제한
            if (c.Width > max) c.Width = max;
            c.MinimumWidth = 80;

            // ✅ 여기서 AutoSize를 끄면 "수동 조절"이 살아남
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Resizable = DataGridViewTriState.True;
        }

        // (선택) 긴 셀은 툴팁으로 전체 보기
        grid.ShowCellToolTips = true;
    }
    finally
    {
        grid.ResumeLayout();
    }
}
