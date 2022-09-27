using System;
using System.Collections.Generic;
using System.Reflection;

public interface IInjectionStrategy
{
    IEnumerable<MethodBase> GetInjectionMethods(Type type);
    IEnumerable<FieldInfo> GetInjectionFields(Type type);
}