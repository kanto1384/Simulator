private static List<T> DedupPreferActiveKeepOrder<T>(
    IEnumerable<T> webOrderedRows,
    Func<T, string> keySelector,
    Func<T, bool> isExpiredSelector,
    Func<T, DateTime?> endAtSelector = null!)
{
    var outList = new List<T>();
    var indexByKey = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

    foreach (var row in webOrderedRows)
    {
        var key = keySelector(row);
        if (string.IsNullOrWhiteSpace(key)) continue;

        if (!indexByKey.TryGetValue(key, out var idx))
        {
            // 첫 등장: 웹 순서대로 추가
            indexByKey[key] = outList.Count;
            outList.Add(row);
            continue;
        }

        // 중복: 기존과 새 것을 비교해서 "정상 우선"으로 교체 (자리 유지)
        var cur = outList[idx];
        var curExpired = isExpiredSelector(cur);
        var newExpired = isExpiredSelector(row);

        // 1) 만료 -> 정상으로 업그레이드: 무조건 교체
        if (curExpired && !newExpired)
        {
            outList[idx] = row;
            continue;
        }

        // 2) 둘 다 같은 상태면(둘 다 정상 or 둘 다 만료) 종료일이 더 늦은 걸로 교체(선택)
        if (!curExpired && !newExpired && endAtSelector != null)
        {
            var curEnd = endAtSelector(cur);
            var newEnd = endAtSelector(row);
            if (newEnd.HasValue && (!curEnd.HasValue || newEnd.Value > curEnd.Value))
                outList[idx] = row;
        }
        else if (curExpired && newExpired && endAtSelector != null)
        {
            var curEnd = endAtSelector(cur);
            var newEnd = endAtSelector(row);
            if (newEnd.HasValue && (!curEnd.HasValue || newEnd.Value > curEnd.Value))
                outList[idx] = row;
        }

        // 그 외는 기존 유지 (웹에서 먼저 나온 자리 유지)
    }

    return outList;
}
