// === 하이브리드 매칭: Success는 최신 Start(LIFO), End는 "Success 달린 최신" 우선 ===
private List<CombinedRow> BuildMergedHybrid(IEnumerable<LogRow> ordered)
{
    var result = new List<CombinedRow>();
    // 변수(BR_별)로 "열린 세션"을 스택처럼 관리하지만, 검색은 선호 규칙으로
    var open = new Dictionary<string, List<CombinedRow>>(StringComparer.OrdinalIgnoreCase);

    foreach (var r in ordered)
    {
        if (string.IsNullOrEmpty(r.Var) || !r.Var.StartsWith("BR_", StringComparison.OrdinalIgnoreCase))
            continue;

        if (!open.TryGetValue(r.Var, out var stack))
        {
            stack = new List<CombinedRow>();
            open[r.Var] = stack;
        }

        switch (r.Kind)
        {
            case ActionKind.Start:
                stack.Add(new CombinedRow
                {
                    Var = r.Var,
                    StartTs = r.Timestamp,
                    Status = "Unknown",
                    SourceFile = r.SourceFile,
                    StartIdx = (int)r.LineIndex,
                    EndIdx = -1
                });
                break;

            case ActionKind.Success:
            case ActionKind.Fail:
            {
                // 뒤에서부터(가장 최근) Success 미부착 세션을 찾는다
                CombinedRow target = null;
                for (int i = stack.Count - 1; i >= 0; i--)
                {
                    if (!stack[i].SuccessTs.HasValue)
                    {
                        target = stack[i];
                        break;
                    }
                }
                if (target == null && stack.Count > 0)
                    target = stack[stack.Count - 1]; // 전부 Success가 이미 있다면 마지막에 덮어씀(로그 잡음 방어)

                if (target != null)
                {
                    target.SuccessTs = r.Timestamp;
                    target.Status = (r.Kind == ActionKind.Fail) ? "NG" : "Success";
                }
                else
                {
                    // 고아 Success → 단독 세션
                    result.Add(new CombinedRow
                    {
                        Var = r.Var,
                        StartTs = r.Timestamp,
                        SuccessTs = r.Timestamp,
                        Status = (r.Kind == ActionKind.Fail) ? "NG" : "Success",
                        SourceFile = r.SourceFile,
                        StartIdx = (int)r.LineIndex,
                        EndIdx = -1
                    });
                }
                break;
            }

            case ActionKind.End:
            {
                // 1순위: Success가 이미 있는 가장 최근 세션
                CombinedRow target = null;
                for (int i = stack.Count - 1; i >= 0; i--)
                {
                    if (stack[i].SuccessTs.HasValue) { target = stack[i]; break; }
                }
                // 2순위: 그냥 가장 최근 세션
                if (target == null && stack.Count > 0)
                    target = stack[stack.Count - 1];

                if (target != null)
                {
                    target.EndTs = r.Timestamp;
                    target.EndIdx = (int)r.LineIndex;
                    target.DurationSeconds = Math.Max(0, Math.Round((target.EndTs.Value - target.StartTs).TotalSeconds, 3));
                    if (target.Status == "Unknown" && target.SuccessTs.HasValue) target.Status = "Success";
                    result.Add(target);
                    stack.Remove(target);
                }
                else
                {
                    // 고아 End → 0초 세션
                    result.Add(new CombinedRow
                    {
                        Var = r.Var,
                        StartTs = r.Timestamp,
                        EndTs = r.Timestamp,
                        Status = "Unknown",
                        SourceFile = r.SourceFile,
                        StartIdx = (int)r.LineIndex,
                        EndIdx = (int)r.LineIndex,
                        DurationSeconds = 0
                    });
                }
                break;
            }
        }
    }

    // 파일 끝에서 아직 열린 세션 정리
    foreach (var kv in open)
    {
        foreach (var cur in kv.Value)
        {
            if (!cur.EndTs.HasValue)
            {
                cur.EndTs = cur.SuccessTs ?? cur.StartTs;
                cur.DurationSeconds = Math.Max(0, Math.Round((cur.EndTs.Value - cur.StartTs).TotalSeconds, 3));
            }
            if (cur.Status == "Unknown" && cur.SuccessTs.HasValue) cur.Status = "Success";
            result.Add(cur);
        }
    }

    return result.OrderBy(x => x.StartTs).ToList();
}




mergedRows = BuildMergedHybrid(forMerged.OrderBy(r => r.Timestamp).ThenBy(r => r.LineIndex));




using System.Text.RegularExpressions;

public static bool HasMissingAmpersand(string text)
{
    if (string.IsNullOrWhiteSpace(text))
        return false;

    string s = text.Replace(" ", "");

    // ① [정상블록][정상블록]이 그대로 붙어있는 경우
    //    예: "B:1000:10W:100A:10"
    //
    // ② 주소 안에 새 디바이스 블록이 끼어든 경우
    //    예: "W:100B:100:2"  (원래는 "W:1000&B:100:2" 여야 함)
    const string pattern = 
        @"(?:[A-Za-z]{1,3}:[0-9A-Fa-f]+:\d+(?=[A-Za-z]{1,3}:[0-9A-Fa-f]+:\d+))" // ①
        + @"|(?:[0-9A-Fa-f]+[A-Za-z]{1,3}:\d+:\d+)";                           // ②

    return Regex.IsMatch(s, pattern);
}


public static bool IsValidMemorySpec(string text)
{
    if (string.IsNullOrWhiteSpace(text))
        return false;

    string s = text.Replace(" ", "");

    // TYPE:숫자:숫자 (&TYPE:숫자:숫자)* 전체 형식
    const string pattern =
        @"^(?:[A-Za-z]{1,3}:\d+:\d+)(?:&[A-Za-z]{1,3}:\d+:\d+)*$";

    return Regex.IsMatch(s, pattern);
}
