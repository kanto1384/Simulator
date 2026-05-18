else if (r.Kind == ActionKind.Fail)
{
    if (q.Count > 0)
    {
        // Start → Fail
        // 열린 Start가 있을 때만 NG 처리
        var cur = q.Dequeue();

        cur.SuccessTs = r.Timestamp;
        cur.SuccessIdx = (int)r.LineIndex;   // JSON / LOT 기준 인덱스
        cur.EndTs = r.Timestamp;
        cur.EndIdx = (int)r.LineIndex;
        cur.Status = "NG";
        cur.DurationSeconds = Math.Max(
            0,
            Math.Round((cur.EndTs.Value - cur.StartTs).TotalSeconds, 3)
        );

        FillLotByIndex(cur);
        result.Add(cur);

        // 바로 뒤 같은 BR End가 오면 후속 End로 보고 무시
        ignoreNextEndAfterFail.Add(r.Var);
    }
    else
    {
        // Fail 단독은 표시하지 않음.
        // 이유: 앞뒤 문맥 없는 Fail은 실제 호출 1건인지 확정 불가.
        // 단독 NG로 만들면 호출 수가 과장될 수 있음.
        ignoreNextEndAfterFail.Add(r.Var);
        continue;
    }
}
