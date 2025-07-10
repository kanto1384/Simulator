using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExampleApp
{
    // 모델 정의
    public class A
    {
        public string ss    { get; set; }
        public decimal Price{ get; set; }
        public List<Property> Props { get; set; }
    }

    public class Property
    {
        public string Name { get; set; }
    }

    public class B
    {
        public List<A> BList { get; set; } = new List<A>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            // B 인스턴스 생성 및 A 객체 2개 추가
            var bInstance = new B
            {
                BList = new List<A>
                {
                    new A { ss = "first",  Price = 10m, Props = new List<Property>{ new Property{Name="X"} } },
                    new A { ss = "second", Price = 20m, Props = new List<Property>{ new Property{Name="Y"} } }
                }
            };

            // BList 프로퍼티에서 요소 타입(A) 추출
            var listProp    = typeof(B).GetProperty("BList");
            var elementType = listProp.PropertyType.GetGenericArguments()[0];

            // A 타입의 public 인스턴스 프로퍼티 이름만 골라 출력
            var names = elementType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name);

            foreach (var name in names)
                Console.WriteLine(name);
        }
    }
}
