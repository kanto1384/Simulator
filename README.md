using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// 적당한 네임스페이스/클래스명으로 바꿔서 사용하세요
public static class PropertyPathHelper
{
    public static List<string> GetAllPropertyPaths(Type type, int maxDepth = 5)
    {
        var results = new List<string>();

        void Recurse(Type currentType, string parentName, int depth)
        {
            if (depth > maxDepth) return;

            foreach (var prop in currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var path = parentName == null
                    ? prop.Name
                    : $"{parentName}.{prop.Name}";
                results.Add(path);

                var pType = prop.PropertyType;
                if (pType == typeof(string) || pType.IsPrimitive) 
                    continue;

                if (pType.IsArray)
                {
                    Recurse(pType.GetElementType(), path, depth + 1);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(pType) && pType.IsGenericType)
                {
                    var elemType = pType.GetGenericArguments().First();
                    Recurse(elemType, path, depth + 1);
                }
                else if (pType.IsClass)
                {
                    Recurse(pType, path, depth + 1);
                }
            }
        }

        Recurse(type, null, 0);
        return results;
    }
}
