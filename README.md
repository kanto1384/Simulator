public async Task EnsureInitializedAsync(Microsoft.Web.WebView2.Core.CoreWebView2Environment env)
{
    await WebView.EnsureCoreWebView2Async(env);

    // JS -> C# 메시지 받으면 팝업 닫기
    WebView.CoreWebView2.WebMessageReceived += (s, e) =>
    {
        if (e.TryGetWebMessageAsString() == "CLOSE_POPUP")
        {
            BeginInvoke(new Action(Close));
        }
    };

    // 페이지 로드 완료 시 '접속하기' 버튼(#pop_btn > a.btn1_2) 클릭 감지용 JS 주입
    WebView.CoreWebView2.NavigationCompleted += async (s, e) =>
    {
        const string js = """
        (() => {
          const btn = document.querySelector('#pop_btn > a.btn1_2'); // div#pop_btn 다음의 a.btn1_2
          if (!btn) return;

          // 중복 설치 방지
          if (btn.dataset.__closeHook === '1') return;
          btn.dataset.__closeHook = '1';

          // 클릭 시 send_value()는 원래대로 실행되고,
          // 우리는 "닫아" 신호만 C#으로 보냄
          btn.addEventListener('click', () => {
            // 연쇄 클릭 방지
            btn.style.pointerEvents = 'none';
            btn.style.opacity = '0.6';
            btn.style.cursor = 'default';

            // C#에게 팝업 닫으라고 알림
            window.chrome?.webview?.postMessage('CLOSE_POPUP');
          }, true);
        })();
        """;

        try { await WebView.CoreWebView2.ExecuteScriptAsync(js); } catch { }
    };
}
