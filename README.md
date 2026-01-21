private async Task<List<(string Ip, string Host, string EndAtText, bool IsExpired)>> ExtractIpHostAsync()
{
    //  - 6th td = IP  (index 5)
    //  - 7th td = Host (index 6)
    //  - 15th td = EndAt (index 14)
    //  - expired rows: td style includes #ff0000 (fixed)
    const string js = """
    (() => {
      const rows = Array.from(document.querySelectorAll('#seltable tbody tr'));
      const out = [];
      for (const tr of rows) {
        const tds = tr.querySelectorAll('td');
        if (!tds || tds.length < 15) continue;

        const ip = (tds[5].innerText || '').trim();
        const host = (tds[6].innerText || '').trim();
        const endAt = (tds[14].innerText || '').trim();

        // 만료 판정: style 속성에 #ff0000 포함(고정)
        // (행 전체/일부 td에 걸릴 수 있으니, tr 안의 td style을 전체 스캔)
        let expired = false;
        for (const td of tds) {
          const style = (td.getAttribute('style') || '').toLowerCase();
          if (style.includes('#ff0000')) { expired = true; break; }
        }

        if (!ip && !host) continue;
        out.push({ ip, host, endAt, expired });
      }

      // 중복 제거(같은 ip|host) - expired/정상 섞인 경우는 "정상 우선"으로 남김
      const map = new Map();
      for (const x of out) {
        const key = (x.ip + '|' + x.host).toLowerCase();
        if (!map.has(key)) { map.set(key, x); continue; }
        const cur = map.get(key);
        // 정상(false)이 있으면 정상 우선
        if (cur.expired && !x.expired) map.set(key, x);
      }

      return Array.from(map.values());
    })();
    """;

    var json = await _web.CoreWebView2.ExecuteScriptAsync(js);
    var items = JsonSerializer.Deserialize<List<PortalRowDto>>(json) ?? new();

    return items
        .Select(x => (
            Ip: (x.ip ?? "").Trim(),
            Host: (x.host ?? "").Trim(),
            EndAtText: (x.endAt ?? "").Trim(),
            IsExpired: x.expired
        ))
        .Where(x => !string.IsNullOrWhiteSpace(x.Ip) || !string.IsNullOrWhiteSpace(x.Host))
        .ToList();
}

private sealed class PortalRowDto
{
    public string? ip { get; set; }
    public string? host { get; set; }
    public string? endAt { get; set; }
    public bool expired { get; set; }
}







private static bool TryParseEndAt(string s, out DateTime endAt)
{
    return DateTime.TryParseExact(
        s.Trim(),
        "yyyy-MM-dd HH:mm",
        CultureInfo.InvariantCulture,
        DateTimeStyles.AssumeLocal,
        out endAt
    );
}

private static string ToDDayText(DateTime endAt)
{
    var d = (int)Math.Floor((endAt - DateTime.Now).TotalDays);
    if (d >= 0) return $"D-{d}";
    return $"만료(D+{-d})";
}




private static List<ViewRow> BuildRows(
    List<(string Ip, string Host, string EndAtText, bool IsExpired)> extracted,
    DeviceIndex index)
{
    var rows = new List<ViewRow>(extracted.Count);

    foreach (var (ip, host, endAtText, isExpired) in extracted)
    {
        // ✅ 만료(빨간색)는 스킵(안 보이게)
        if (isExpired) continue;

        // D-day 계산(종료일 없거나 파싱 실패하면 공백)
        string dday = "";
        if (!string.IsNullOrWhiteSpace(endAtText) && TryParseEndAt(endAtText, out var endAt))
            dday = ToDDayText(endAt);

        DeviceItem? item = null;

        if (!string.IsNullOrWhiteSpace(host) && index.ByHost.TryGetValue(host, out var byHost))
            item = byHost;
        else if (!string.IsNullOrWhiteSpace(ip) && index.ByIp.TryGetValue(ip, out var byIp))
            item = byIp;

        if (item is null)
        {
            rows.Add(new ViewRow(ip, host, endAtText, dday, "", "", "", "", "매핑 없음"));
            continue;
        }

        var listText = item.equipmentList is { Count: > 0 }
            ? string.Join(", ", item.equipmentList)
            : "";

        rows.Add(new ViewRow(
            ip,
            host,
            endAtText,
            dday,
            item.equipmentGroup ?? "",
            listText,
            item.location ?? "",
            item.note ?? "",
            "OK"));
    }

    // 정렬: 곧 만료(D-작은값) 위로 올리고 싶으면 여기서 커스텀도 가능
    return rows
        .OrderBy(r => r.Status == "매핑 없음" ? 1 : 0)
        .ThenBy(r => r.EquipmentGroup)
        .ThenBy(r => r.Host)
        .ToList();
}





var extracted = await ExtractIpHostAsync();
_currentRows = BuildRows(extracted, _index);



public sealed record ViewRow(
    string Ip,
    string Host,
    string EndAt,
    string DDay,
    string EquipmentGroup,
    string EquipmentListText,
    string Location,
    string Note,
    string Status);



ContainsIgnoreCase(r.EndAt, q) ||
ContainsIgnoreCase(r.DDay, q) ||
