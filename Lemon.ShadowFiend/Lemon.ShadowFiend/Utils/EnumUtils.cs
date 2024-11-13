using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Lemon.ShadowFiend.Utils;

public class EnumUtils
{
    public static IEnumerable<T?> GetAllMembers<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>() where T : Enum
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
        return fields.Select(f=>(T)f.GetValue(null)!);
    }
}