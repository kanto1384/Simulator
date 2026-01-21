private void FitGridColumnsOnce(DataGridView grid)
{
    if (grid.Columns.Count == 0) return;

    // 화면 깜빡임 줄이기
    grid.SuspendLayout();
    try
    {
        // 현재 사용자의 수동 리사이즈 환경을 그대로 유지
        // (AutoSizeMode/AutoSizeColumnsMode 절대 변경하지 않음)

        // 1) 현재 각 컬럼의 AutoSizeMode 백업
        var backup = new Dictionary<DataGridViewColumn, DataGridViewAutoSizeColumnMode>();
        foreach (DataGridViewColumn c in grid.Columns)
            backup[c] = c.AutoSizeMode;

        // 2) 잠깐 AllCells로 바꿔서 "추천 폭" 계산
        foreach (DataGridViewColumn c in grid.Columns)
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

        grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        // 3) 계산된 Width를 최대폭까지만 제한
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

            if (c.Width > max) c.Width = max;
        }

        // 4) 원래 AutoSizeMode로 복원 (⭐ 수동 리사이즈 유지의 핵심)
        foreach (var kv in backup)
            kv.Key.AutoSizeMode = kv.Value;
    }
    finally
    {
        grid.ResumeLayout();
    }
}
