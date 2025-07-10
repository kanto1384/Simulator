using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// … 여기에 CBR_PRD_REG_AWL_EIF_MARK_DETECT_IN, C_IN_EQP 클래스가 정의되어 있어야 합니다

class Program
{
    static void Main()
    {
        // 1) 인스턴스 생성 (필요에 따라 실제 데이터를 채워도 되고, 그냥 타입 정보만 뽑아도 됩니다)
        var root = new CBR_PRD_REG_AWL_EIF_MARK_DETECT_IN();

        // 2) "IN_EQP" 필드(혹은 프로퍼티) 정보 가져오기
        //    (필드인 경우 GetField, 프로퍼티인 경우 GetProperty)
        var listField = typeof(CBR_PRD_REG_AWL_EIF_MARK_DETECT_IN)
                            .GetField("IN_EQP", BindingFlags.Public | BindingFlags.Instance);
        // var listProp = typeof(...).GetProperty("IN_EQP", ...); // 프로퍼티일 땐 이렇게

        // 3) 실제 리스트 객체 꺼내기
        var list = (IList)listField.GetValue(root);

        // 4) 리스트 요소의 타입 얻기
        var elementType = listField.FieldType.GetGenericArguments()[0];

        // 5) 요소 타입의 public 인스턴스 프로퍼티만 골라내기
        var props = elementType
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(p => p.Name);

        // 6) 출력
        Console.WriteLine($"IN_EQP 요소({elementType.Name}) 의 프로퍼티:");
        foreach (var name in props)
            Console.WriteLine(" - " + name);
    }
}
