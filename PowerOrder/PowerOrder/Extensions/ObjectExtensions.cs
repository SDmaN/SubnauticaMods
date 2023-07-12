using System;
using System.Linq;
using System.Reflection;

namespace PowerOrder.Extensions;

internal static class ObjectExtensions
{
    public static T GetField<T>(this object o, string fieldName)
    {
        if (o is null)
        {
            throw new ArgumentNullException(nameof(o));
        }

        if (fieldName is null)
        {
            throw new ArgumentNullException(nameof(fieldName));
        }

        var field = FindMember<FieldInfo>(o.GetType(), fieldName);

        return field is not null ? (T)field.GetValue(o) : default;
    }

    public static void SetField<T>(this object o, string fieldName, T value)
    {
        if (o is null)
        {
            throw new ArgumentNullException(nameof(o));
        }

        if (fieldName is null)
        {
            throw new ArgumentNullException(nameof(fieldName));
        }

        var field = FindMember<FieldInfo>(o.GetType(), fieldName);
        field?.SetValue(o, value);
    }

    private static TMember FindMember<TMember>(Type t, string memberName)
        where TMember : MemberInfo
    {
        var members = t.GetMember(memberName, BindingFlags.Instance | BindingFlags.NonPublic);
        var found = members.FirstOrDefault(x => x is TMember);

        return found is not null
            ? (TMember)found
            : t.BaseType is null
            ? default
            : FindMember<TMember>(t.BaseType, memberName);
    }
}
