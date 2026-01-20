private async void CoreWebView2_NewWindowRequested(
    object? sender,
    Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
{
    // 우리가 새 창을 처리함
    e.Handled = true;

    var popup = new PopupBrowserForm();
    popup.Show(this);

    // 같은 Environment → 쿠키/세션/POST 컨텍스트 유지
    await popup.EnsureInitializedAsync(_web.CoreWebView2.Environment);

    // 🔥 핵심: Navigate 호출 ❌
    // WebView2 엔진이 POST/JS 컨텍스트를 그대로 넘겨줌
    e.NewWindow = popup.WebView.CoreWebView2;
}
