using System;
using ACTUtlTypeLib;

namespace MxComponentSample
{
    class Program
    {
        static void Main(string[] args)
        {
            ActUtlType plc = new ActUtlType();

            // 논리 스테이션 번호 설정 (필요에 따라 수정)
            plc.ActLogicalStationNumber = 1;

            // PLC 연결
            int result = plc.Open();
            Console.WriteLine($"Open result: {result}");

            if (result == 0)
            {
                Console.WriteLine("성공적으로 PLC에 연결되었습니다.");
                // 연결 해제
                plc.Close();
            }
            else
            {
                Console.WriteLine("PLC 연결 실패");
            }
        }
    }
}
