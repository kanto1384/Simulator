private void FitGridColumnsOnce(DataGridView grid)
{
    if (grid.Columns.Count == 0) return;

    grid.SuspendLayout();
    try
    {
        const int maxWide = 380;   // 긴 텍스트(장비목록 등)
        const int maxMid = 260;    // 일반
        const int maxNarrow = 180; // IP/Host 같은 짧은

        foreach (DataGridViewColumn c in grid.Columns)
        {
            // ✅ “현재 들어있는 데이터 기준”으로 추천 폭 계산
            int w = c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);

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

            // ✅ clamp + 약간의 여유(헤더/패딩 때문에 6~10px 더 필요할 때가 많음)
            w = Math.Min(w + 10, max);

            // ✅ 폭만 설정 (AutoSizeMode/AutoSizeColumnsMode는 건드리지 않음)
            c.Width = w;
        }
    }
    finally
    {
        grid.ResumeLayout();
    }
}
