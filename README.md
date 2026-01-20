private async void CoreWebView2_NewWindowRequested(
    object? sender,
    Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
{
    // 🔥 이거 없으면 100% 두 개 뜨거나 터짐
    var deferral = e.GetDeferral();

    try
    {
        e.Handled = true;

        // 팝업 Form 먼저 생성 (아직 Navigate ❌)
        var popup = new PopupBrowserForm();

        // WebView2를 "같은 Environment"로 즉시 초기화
        await popup.EnsureInitializedAsync(_web.CoreWebView2.Environment);

        // 🔥 핵심: 엔진에게 "이 WebView가 새창이다"를 먼저 알려줌
        e.NewWindow = popup.WebView.CoreWebView2;

        // 이제 보여줘도 안전
        popup.Show(this);
    }
    finally
    {
        // 🔥 엔진에게 "처리 끝" 신호
        deferral.Complete();
    }
}






public class PopupBrowserForm : Form
{
    public WebView2 WebView { get; } = new WebView2();

    public PopupBrowserForm()
    {
        Text = "Remote Connection";
        Width = 1100;
        Height = 800;
        StartPosition = FormStartPosition.CenterParent;

        WebView.Dock = DockStyle.Fill;
        Controls.Add(WebView);
    }

    public async Task EnsureInitializedAsync(CoreWebView2Environment env)
    {
        await WebView.EnsureCoreWebView2Async(env);
    }
}
