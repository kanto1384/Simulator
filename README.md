using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

public class PopupBrowserForm : Form
{
    public WebView2 WebView { get; } = new WebView2();

    public PopupBrowserForm()
    {
        Text = "Remote Portal - Popup";
        Width = 1100;
        Height = 800;

        WebView.Dock = DockStyle.Fill;
        Controls.Add(WebView);
    }

    public async Task EnsureInitializedAsync(CoreWebView2Environment env)
    {
        // WebView2가 같은 Environment를 쓰면 쿠키/세션이 공유돼서 로그인 상태 유지가 잘 됨
        await WebView.EnsureCoreWebView2Async(env);

        // 팝업이 또 팝업을 띄우는 경우도 있으니 재귀로 처리하고 싶으면:
        WebView.CoreWebView2.NewWindowRequested += (s, e) =>
        {
            // 간단히: 같은 로직으로 또 새창 열기
            e.Handled = true;
            var child = new PopupBrowserForm();
            child.Show(this);
            _ = child.EnsureInitializedAsync(env).ContinueWith(_ =>
            {
                if (child.WebView.CoreWebView2 != null)
                    e.NewWindow = child.WebView.CoreWebView2;
                if (!string.IsNullOrWhiteSpace(e.Uri))
                    child.WebView.CoreWebView2.Navigate(e.Uri);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        };
    }
}




private async void CoreWebView2_NewWindowRequested(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
{
    // 팝업(새창) 요청을 우리가 처리
    e.Handled = true;

    // 새 폼 + 새 WebView2 만들기
    var popup = new PopupBrowserForm();

    // 폼 Show 먼저 (핸들 생성)
    popup.Show(this);

    // 새 WebView2가 준비되면 그걸 새 창 대상(NewWindow)으로 연결
    await popup.EnsureInitializedAsync(_web.CoreWebView2.Environment);

    // 이게 핵심: "새창"을 새 WebView2로 연결
    e.NewWindow = popup.WebView.CoreWebView2;

    // 혹시 URI가 같이 넘어오는 경우도 있어서 안전하게 보강
    if (!string.IsNullOrWhiteSpace(e.Uri))
    {
        popup.WebView.CoreWebView2.Navigate(e.Uri);
    }
}
