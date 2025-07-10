using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public static class ListDictionaryBuilder
{
    /// <summary>
    /// rootType에 선언된 List<T> 멤버들 각각에 대해,
    /// T의 프로퍼티 이름을 key, 초기값(null)을 value로 갖는 딕셔너리를 만들어
    /// 멤버 이름→(프로퍼티→값) 딕셔너리 맵을 리턴합니다.
    /// </summary>
    public static Dictionary<string, Dictionary<string, object>>
        Build(Type rootType)
    {
        var result = new Dictionary<string, Dictionary<string, object>>();

        // 1) public 인스턴스 필드/프로퍼티 중에서
        //    List<T> 타입인 멤버만 골라낸다.
        var members = rootType
            .GetMembers(BindingFlags.Public | BindingFlags.Instance)
            .Where(mi =>
            {
                if (mi.MemberType == MemberTypes.Field)
                    return IsListOfT(((FieldInfo)mi).FieldType);
                if (mi.MemberType == MemberTypes.Property)
                    return IsListOfT(((PropertyInfo)mi).PropertyType);
                return false;
            });

        foreach (var mi in members)
        {
            // 2) 멤버가 Field인지 Property인지 분기해서 List<T> 타입 가져오기
            Type listType = (mi.MemberType == MemberTypes.Field)
                ? ((FieldInfo)mi).FieldType
                : ((PropertyInfo)mi).PropertyType;

            // 3) 그 List<T>의 요소 타입 T
            Type elemType = listType.GetGenericArguments()[0];

            // 4) T의 public 인스턴스 프로퍼티 이름으로 딕셔너리 초기화
            var dict = new Dictionary<string, object>();
            foreach (var pi in elemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                dict[pi.Name] = null;
            }

            // 5) 결과 맵에 넣기 (키: “IN_EQP” or “IN_ROLLMAP” 등)
            result[mi.Name] = dict;
        }

        return result;
    }

    // List<T>인지 검사
    private static bool IsListOfT(Type t)
    {
        return t.IsGenericType
            && t.GetGenericTypeDefinition() == typeof(List<>);
    }
}
