using System;
using System.Text.RegularExpressions;

public static class MemorySpecValidator
{
    /// <summary>
    /// 전체 형식이 맞는지 검사
    /// TYPE:ADDRESS:LENGTH (&TYPE:ADDRESS:LENGTH ...) 구조
    /// ADDRESS = 16진수 (0~9, A~F)
    /// LENGTH  = 10진수
    /// </summary>
    public static bool IsValidMemorySpec(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        string s = text.Replace(" ", "");

        // TYPE:HEXADDR:LENGTH (&TYPE:HEXADDR:LENGTH)*
        const string pattern =
            @"^(?:[A-Za-z]{1,3}:[0-9A-Fa-f]+:\d+)(?:&[A-Za-z]{1,3}:[0-9A-Fa-f]+:\d+)*$";

        return Regex.IsMatch(s, pattern);
    }

    /// <summary>
    /// & 가 빠진 케이스 감지
    /// 예) B:1000:10W:100A:10
    /// 예) W:100B:100:2
    /// 예) D:0:1B:10:2
    /// 숫자 뒤에 TYPE이 바로 붙으면 & 누락으로 간주
    /// </summary>
    public static bool HasMissingAmpersand(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        string s = text.Replace(" ", "");

        // 숫자 + TYPE: 패턴 → 새 블록인데 &가 없음
        // 예: "10W:" / "100B:" / "1B:"
        const string pattern = @"\d+[A-Za-z]{1,3}:";

        return Regex.IsMatch(s, pattern);
    }

    /// <summary>
    /// 최종 검증: 전체 형식 + &누락 검사
    /// </summary>
    public static bool IsSafeMemorySpec(string text)
    {
        if (!IsValidMemorySpec(text))
            return false;

        if (HasMissingAmpersand(text))
            return false;

        return true;
    }

    /// <summary>
    /// 파싱 (검증 통과한 문자열만 넣어야 함)
    /// </summary>
    public static (string type, int address, int length)[] Parse(string text)
    {
        string s = text.Replace(" ", "");
        string[] blocks = s.Split('&');

        var result = new (string type, int address, int length)[blocks.Length];

        for (int i = 0; i < blocks.Length; i++)
        {
            var parts = blocks[i].Split(':');
            string type = parts[0];
            string addrHex = parts[1];   // HEX 주소
            string lengthText = parts[2];

            int addr = Convert.ToInt32(addrHex, 16);
            int len = int.Parse(lengthText);

            result[i] = (type, addr, len);
        }

        return result;
    }
}


void ApplyClassicGridStyle(DataGridView dgv)
{
    dgv.EnableHeadersVisualStyles = false;

    dgv.GridColor = Color.FromArgb(224, 224, 224);
    dgv.BackgroundColor = Color.White;
    dgv.BorderStyle = BorderStyle.FixedSingle;

    dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
    dgv.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;

    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
}




dataGridView1.EnableHeadersVisualStyles = false;

dataGridView1.GridColor = Color.FromArgb(224, 224, 224); // 핵심
dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
dataGridView1.AdvancedCellBorderStyle.All =
    DataGridViewAdvancedCellBorderStyle.Single;

dataGridView1.BorderStyle = BorderStyle.FixedSingle;
