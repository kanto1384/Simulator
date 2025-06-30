using System;
using System.ServiceModel;

[ServiceContract]
public interface IElementSession {}  // 이건 마커 인터페이스일 가능성 있음

[ServiceContract]
public interface IElementServer
{
    [OperationContract]
    IElementSession Connect(string clientId); // 세션 생성

    [OperationContract]
    string GetVariableValue(IElementSession session, string tagName); // 세션 기반 요청
}

class Program
{
    static void Main()
    {
        var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
        var address = new EndpointAddress("net.pipe://localhost/EIF");

        var factory = new ChannelFactory<IElementServer>(binding, address);
        var proxy = factory.CreateChannel();

        try
        {
            // 1. 세션 연결
            var session = proxy.Connect("myClient01");
            Console.WriteLine("세션 연결 성공");

            // 2. 세션 기반 값 요청
            var value = proxy.GetVariableValue(session, "Pump1.Status");
            Console.WriteLine($"Pump1 상태: {value}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("오류: " + ex.Message);
        }
        finally
        {
            ((IClientChannel)proxy).Close();
            factory.Close();
        }
    }
}
