private void BuildLayout()
{
    // ... (기존에 _toolStrip, _split 등 컨트롤 생성하는 코드는 유지)

    // 1) Dock/Height 확실히 지정
    _toolStrip.Dock = DockStyle.Fill;  // TableLayout row 안에서는 Fill로 두는 게 안정적
    _toolStrip.AutoSize = true;

    _split.Dock = DockStyle.Fill;      // 아래 영역 꽉 채우기

    // 2) 루트 레이아웃 (2행)
    var root = new TableLayoutPanel
    {
        Dock = DockStyle.Fill,
        ColumnCount = 1,
        RowCount = 2,
        Margin = Padding.Empty,
        Padding = Padding.Empty
    };

    // Row0: 상단바(자동 높이), Row1: 나머지(전체)
    root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
    root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

    // 3) 배치
    root.Controls.Add(_toolStrip, 0, 0);
    root.Controls.Add(_split, 0, 1);

    // 4) 폼에 root만 올림
    Controls.Clear();
    Controls.Add(root);
}
