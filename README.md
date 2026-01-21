private void FitGridColumnsLikeExcel(DataGridView grid)
{
    if (grid.Columns.Count == 0) return;

    grid.SuspendLayout();
    try
    {
        foreach (DataGridViewColumn c in grid.Columns)
        {
            // 엑셀 더블클릭과 동일:
            // 헤더 + 모든 셀 중 가장 긴 값 기준
            int preferred =
                c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);

            // 그대로 적용 (여유 패딩 약간)
            c.Width = preferred + 8;
        }
    }
    finally
    {
        grid.ResumeLayout();
    }
}
