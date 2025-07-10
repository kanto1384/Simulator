using System;
using System.Reflection;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // 1) 루트 타입 지정
        Type rootType = typeof(CBR_PRD_REG_AWL_EIF_MARK_DETECT_IN);

        // 2) "IN_EQP" 필드 또는 프로퍼티 정보 조회
        MemberInfo[] members = rootType.GetMember("IN_EQP",
            BindingFlags.Public | BindingFlags.Instance);
        if (members.Length == 0)
        {
            Console.WriteLine("IN_EQP 멤버를 찾을 수 없습니다.");
            return;
        }
        MemberInfo member = members[0];

        // 3) MemberInfo가 Field인지 Property인지 분기하여 List<T> 타입 얻기
        Type listType;
        if (member.MemberType == MemberTypes.Field)
        {
            listType = ((FieldInfo)member).FieldType;
        }
        else if (member.MemberType == MemberTypes.Property)
        {
            listType = ((PropertyInfo)member).PropertyType;
        }
        else
        {
            Console.WriteLine("IN_EQP가 필드도 프로퍼티도 아닙니다.");
            return;
        }

        // 4) 제네릭 List<> 타입인지, 맞다면 T(요소 타입) 추출
        if (!listType.IsGenericType || 
            listType.GetGenericTypeDefinition() != typeof(List<>))
        {
            Console.WriteLine("IN_EQP가 제네릭 List<T>가 아닙니다.");
            return;
        }
        Type elementType = listType.GetGenericArguments()[0];

        // 5) 요소 타입의 public 인스턴스 프로퍼티 이름만 꺼내서 출력
        Console.WriteLine("IN_EQP 리스트 요소 타입: " + elementType.Name);
        foreach (PropertyInfo pi in 
                 elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            Console.WriteLine(" - " + pi.Name);
        }
    }
}
