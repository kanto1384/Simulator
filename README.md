public async Task EnsureInitializedAsync(CoreWebView2Environment env)
{
    await WebView.EnsureCoreWebView2Async(env);

    // JS에서 "CLICKED" 메시지 보내면 팝업 닫기
    WebView.CoreWebView2.WebMessageReceived += (s, e) =>
    {
        if (e.TryGetWebMessageAsString() == "CLOSE_POPUP")
        {
            BeginInvoke(new Action(() => Close()));
        }
    };

    // 페이지 로드되면 '접속하기' 버튼 클릭을 감지해서 메시지 보내기
    WebView.CoreWebView2.NavigationCompleted += async (s, e) =>
    {
        const string js = """
        (() => {
          const btn = document.querySelector('#pop_wrapper #pop_container #pop_btn a.btn1_2');
          if (!btn) return;

          if (btn.dataset.__closeHook === '1') return;
          btn.dataset.__closeHook = '1';

          btn.addEventListener('click', () => {
            // send_value()는 원래대로 실행되고,
            // 우리는 "닫아!"만 C#에 알림
            window.chrome?.webview?.postMessage('CLOSE_POPUP');
          }, true);
        })();
        """;

        try { await WebView.CoreWebView2.ExecuteScriptAsync(js); } catch { }
    };
}
