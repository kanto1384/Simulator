using System;
using System.ServiceModel;

// WCF 서비스 계약 인터페이스 정의
[ServiceContract]
public interface IRemoteControlServer
{
    [OperationContract]
    string GetServerName(); // 인자 없는 메서드로 연결 확인
}

class Program
{
    static void Main()
    {
        // NetNamedPipeBinding 설정 (보안 없음)
        var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
        var address = new EndpointAddress("net.pipe://localhost/EIF");

        var factory = new ChannelFactory<IRemoteControlServer>(binding, address);
        IRemoteControlServer proxy = factory.CreateChannel();

        try
        {
            string name = proxy.GetServerName();
            Console.WriteLine("서버 이름: " + name);
        }
        catch (Exception ex)
        {
            Console.WriteLine("오류 발생: " + ex.Message);
        }
        finally
        {
            try
            {
                ((IClientChannel)proxy).Close();
                factory.Close();
            }
            catch { }
        }

        Console.WriteLine("엔터를 눌러 종료합니다.");
        Console.ReadLine();
    }
}
