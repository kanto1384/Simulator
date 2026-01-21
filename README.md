private void ApplyKoreanHeadersSimple(DataGridView grid)
{
    foreach (DataGridViewColumn col in grid.Columns)
    {
        switch (col.HeaderText)
        {
            case "Ip":
                col.HeaderText = "IP";
                break;

            case "Host":
                col.HeaderText = "호스트";
                break;

            case "EquipmentGroup":
                col.HeaderText = "장비군";
                break;

            case "EquipmentList":
                col.HeaderText = "장비목록";
                break;

            case "Location":
                col.HeaderText = "위치";
                break;

            case "Note":
                col.HeaderText = "비고";
                break;
        }
    }
}
