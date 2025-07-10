using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExampleApp
{
    // --- 1) 여러분의 모델 클래스들 ---
    public class C_IN_EQP
    {
        public string __EQPTID { get; set; }
        public string EQPTID   { get; set; }
        public string __USERID { get; set; }
        public string USERID   { get; set; }
    }

    public class C_IN_ROLLMAP
    {
        public int    MapId    { get; set; }
        public string MapName  { get; set; }
    }

    public class CBR_PRD_REG_AWL_EIF_MARK_DETECT_IN
    {
        public List<C_IN_EQP>     IN_EQP     { get; set; }
        public List<C_IN_ROLLMAP> IN_ROLLMAP { get; set; }
    }

    // --- 2) 딕셔너리 빌더 유틸리티 ---
    public static class ListDictionaryBuilder
    {
        // 키: 멤버 이름(e.g. "IN_EQP"), 값: 요소 타입의 프로퍼티→값(null) 딕셔너리
        public static Dictionary<string, Dictionary<string, object>> Build(Type rootType)
        {
            var result = new Dictionary<string, Dictionary<string, object>>();

            // rootType의 public 인스턴스 멤버 전부 조사
            MemberInfo[] members = rootType.GetMembers(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < members.Length; i++)
            {
                MemberInfo mi = members[i];
                Type memberType;

                // 필드인지 프로퍼티인지 구분
                if (mi.MemberType == MemberTypes.Field)
                    memberType = ((FieldInfo)mi).FieldType;
                else if (mi.MemberType == MemberTypes.Property)
                    memberType = ((PropertyInfo)mi).PropertyType;
                else
                    continue;

                // 제네릭 List<T> 검사
                if (!memberType.IsGenericType ||
                    memberType.GetGenericTypeDefinition() != typeof(List<>))
                    continue;

                // 요소 타입 T 추출
                Type elementType = memberType.GetGenericArguments()[0];

                // T의 프로퍼티 이름으로 innerDict 초기화
                var innerDict = new Dictionary<string, object>();
                PropertyInfo[] props = elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                for (int j = 0; j < props.Length; j++)
                {
                    innerDict.Add(props[j].Name, null);
                }

                // 결과에 추가
                result.Add(mi.Name, innerDict);
            }

            return result;
        }
    }

    // --- 3) 실제 호출부 ---
    class Program
    {
        static void Main()
        {
            // 반드시 클래스이름.메서드명 으로 호출합니다.
            var map = ListDictionaryBuilder.Build(
                typeof(CBR_PRD_REG_AWL_EIF_MARK_DETECT_IN));

            // 결과 확인
            foreach (var kv in map)
            {
                Console.WriteLine("Member: " + kv.Key);
                foreach (var inner in kv.Value)
                {
                    Console.WriteLine($"  {inner.Key} = {inner.Value}");
                }
            }
        }
    }
}
