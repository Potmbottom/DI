using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DefaultInjectionStrategy : IInjectionStrategy
{
    public IEnumerable<MethodBase> GetInjectionMethods(Type type)
    {
        bool IsUnityObject(Type parent)
        {
            while (parent.BaseType != null)
            {
                parent = parent.BaseType;
                if (parent == typeof(MonoBehaviour))
                    return true;
            }

            return false;
        }

        if (IsUnityObject(type))
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(info => info.GetCustomAttribute<InjectAttribute>() != null);
        }

        var ctor = (MethodBase)type.GetConstructors().FirstOrDefault(info => info.GetParameters().Length > 0);
        return ctor == null ? null : new []{ctor};
    }

    public IEnumerable<FieldInfo> GetInjectionFields(Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(info => info.GetCustomAttribute<InjectAttribute>() != null);
    }
}