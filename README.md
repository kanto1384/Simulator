# MxComponent Sample

이 저장소는 Mitsubishi MxComponent V5를 사용하여 PLC에 연결하는 샘플 코드를 제공합니다.

## 콘솔 예제 컴파일 방법

1. Visual Studio에서 C# 콘솔 프로젝트를 생성합니다.
2. 프로젝트 참조에 `ACTUtlTypeLib`(MxComponent 설치 시 제공) COM 라이브러리를 추가합니다.
3. `MxComponentSample/Program.cs` 파일의 코드를 프로젝트에 추가합니다.
4. 논리 스테이션 번호(`ActLogicalStationNumber`)를 환경에 맞게 수정합니다.
5. 프로젝트를 빌드하여 실행합니다.

## Windows Forms 예제 사용법

1. C# Windows Forms 프로젝트를 생성합니다.
2. `ACTUtlTypeLib` COM 라이브러리를 참조로 추가합니다.
3. `MxComponentFormSample` 폴더의 `Program.cs`, `MainForm.cs`, `MainForm.Designer.cs` 파일을 프로젝트에 포함합니다.
4. 폼을 실행하면 IP 주소와 논리 스테이션 번호를 입력하고 Connect 버튼을 눌러 PLC에 연결할 수 있습니다.
5. `B` 또는 `W` 영역의 디바이스를 `B0`, `W0`와 같이 입력합니다.
6. Read 버튼으로 값을 읽을 수 있으며, Write는 `Enable` 버튼을 눌러 활성화 후 사용할 수 있습니다.

**주의**: 코드 실행 전 `MxComponent` 가 올바르게 설치되어 있어야 합니다.


using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EIFClientApp
{
    // Custom delegating handler as part of the pipeline
    public class EIFHandler : DelegatingHandler
    {
        public EIFHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[EIF] Sending request: {request.Method} {request.RequestUri}");
            var response = await base.SendAsync(request, cancellationToken);
            Console.WriteLine($"[EIF] Received response: {(int)response.StatusCode} {response.ReasonPhrase}");
            return response;
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        // Builds an HttpClient pipeline named 'EIF'
        public static HttpClient CreateEIFClient()
        {
            var httpClientHandler = new HttpClientHandler();
            var eifHandler = new EIFHandler(httpClientHandler);
            var client = new HttpClient(eifHandler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
            client.DefaultRequestHeaders.Add("User-Agent", "EIFClient/1.0");
            return client;
        }
    }

    public class MainForm : Form
    {
        private readonly Button sendButton;
        private readonly TextBox urlTextBox;
        private readonly TextBox responseTextBox;

        public MainForm()
        {
            Text = "EIF Client";
            Width = 600;
            Height = 400;

            urlTextBox = new TextBox
            {
                Text = "https://api.github.com/",
                Dock = DockStyle.Top
            };
            Controls.Add(urlTextBox);

            sendButton = new Button
            {
                Text = "Send Request",
                Dock = DockStyle.Top,
                Height = 30
            };
            sendButton.Click += async (s, e) => await SendRequestAsync();
            Controls.Add(sendButton);

            responseTextBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill
            };
            Controls.Add(responseTextBox);
        }

        private async Task SendRequestAsync()
        {
            sendButton.Enabled = false;
            responseTextBox.Clear();
            try
            {
                var client = Program.CreateEIFClient();
                var url = urlTextBox.Text;
                responseTextBox.Text = $"Sending GET {url}...";
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                responseTextBox.Text = content;
            }
            catch (Exception ex)
            {
                responseTextBox.Text = $"[EIF] Error: {ex.Message}";
            }
            finally
            {
                sendButton.Enabled = true;
            }
        }
    }
}




using System;
using System.IO;
using System.IO.Pipes;

namespace NamedPipeExample
{
    class NamedPipeClient
    {
        static void Main(string[] args)
        {
            // 서버에서 지정한 파이프 이름
            const string pipeName = "testpipe";

            try
            {
                using (var client = new NamedPipeClientStream(
                    serverName: ".",      // 로컬 머신을 가리킵니다. 원격 머신을 사용할 경우 머신 이름으로 변경하세요.
                    pipeName: pipeName,
                    direction: PipeDirection.InOut,
                    options: PipeOptions.None))
                {
                    Console.WriteLine($"서버에 연결 시도: {pipeName}...");
                    client.Connect(5000); // 연결 대기 시간 (ms)

                    Console.WriteLine("서버에 연결되었습니다.");

                    using (var writer = new StreamWriter(client))
                    using (var reader = new StreamReader(client))
                    {
                        writer.AutoFlush = true;

                        // 서버에 메시지 보내기
                        string messageToServer = "Hello from client!";
                        Console.WriteLine($"서버로 전송: {messageToServer}");
                        writer.WriteLine(messageToServer);

                        // 서버로부터 응답 읽기
                        string response = reader.ReadLine();
                        Console.WriteLine($"서버 응답: {response}");
                    }
                }
            }
            catch (TimeoutException)
            {
                Console.Error.WriteLine("타임아웃: 서버에 연결할 수 없습니다.");
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine($"IO 예외 발생: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"예외 발생: {ex}");
            }

            Console.WriteLine("클라이언트를 종료합니다. 엔터키를 누르세요.");
            Console.ReadLine();
        }
    }
}
