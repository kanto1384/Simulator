using System;
using System.Linq;
using System.Reflection;

class Program
{
    static void Main()
    {
        // 1) 루트 타입
        var rootType = typeof(CBR_PRD_REG_AWL_EIF_MARK_DETECT_IN);

        // 2) "IN_EQP" 멤버(필드 혹은 프로퍼티) 하나로 잡기
        var member = rootType
            .GetMember("IN_EQP", BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault();

        if (member == null)
            throw new InvalidOperationException("IN_EQP 멤버를 찾을 수 없습니다.");

        // 3) 멤버가 Field인지 Property인지 분기해서 리스트 타입 얻기
        Type listType = member switch
        {
            FieldInfo  fi => fi.FieldType,
            PropertyInfo pi => pi.PropertyType,
            _ => throw new InvalidOperationException("IN_EQP가 필드도, 프로퍼티도 아닙니다.")
        };

        // 4) List<T> 의 T 가져오기
        if (!listType.IsGenericType)
            throw new InvalidOperationException("IN_EQP가 제네릭 리스트가 아닙니다.");

        var elementType = listType.GetGenericArguments()[0];

        // 5) 요소 타입의 프로퍼티 이름만 뽑아 출력
        var names = elementType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name);

        Console.WriteLine($"IN_EQP 요소({elementType.Name}) 프로퍼티:");
        foreach (var n in names)
            Console.WriteLine(" - " + n);
    }
}
