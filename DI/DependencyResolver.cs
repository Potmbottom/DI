using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DependencyResolver
{
    private readonly IInjectionStrategy _injectionStrategy;

    public DependencyResolver(IInjectionStrategy injectionStrategy)
    {
        _injectionStrategy = injectionStrategy;
    }

    public void Resolve(object obj, List<BindingModel> bindings)
    {
        var type = obj.GetType();
        var methods = _injectionStrategy.GetInjectionMethods(type);
        var fields = _injectionStrategy.GetInjectionFields(type);
        ResolveMethods(methods, bindings, obj, type);
        ResolveFields(fields, bindings, obj, type);
    }

    private void ResolveMethods(IEnumerable<MethodBase> methods, List<BindingModel> bindings, object obj, Type type)
    {
        if (methods == null) return;
        foreach (var method in methods)
        {
            var argumentTypes = method.GetParameters();
            var bindArguments = argumentTypes.Select(type1 => Get(type1.ParameterType, type, obj, bindings)).ToArray();
            method.Invoke(obj, bindArguments);   
        }
    }

    private void ResolveFields(IEnumerable<FieldInfo> fields, List<BindingModel> bindings, object obj, Type type)
    {
        foreach (var field in fields)
        {
            var bindValue = Get(field.FieldType, type, obj, bindings);
            field.SetValue(obj, bindValue);
        }
    }

    private object Get(Type getType, Type contractType, object obj, List<BindingModel> bindings)
    {
        var binding = bindings.FirstOrDefault(model =>
        {
            //duplicate
            if (model.Contract == null) return false;
            //concrete type, but not match
            if (model.Contract != typeof(object) && model.Contract != contractType) return false;
                
            //can inject, looking argument type
            return model.BindingType == getType;
        });
        if (binding != null) return binding.Object;
            
        Debug.LogError($"Cant find dependency {getType} for {obj.GetType()}");
        return null;
    }
}