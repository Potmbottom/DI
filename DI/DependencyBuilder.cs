using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DependencyBuilder
{
    private const int CIRCULAR_DEPENDENCY_ERROR = -1;
    
    private readonly List<Type> _ignoreTypes = new List<Type> {typeof(List<>), typeof(Dictionary<,>)};
    private readonly IInjectionStrategy _injectionStrategy;

    public DependencyBuilder(IInjectionStrategy injectionStrategy)
    {
        _injectionStrategy = injectionStrategy;
    }

    public IEnumerable<object> Build(List<BindingConstructionModel> constructionData, List<BindingModel> bindings)
    {
        var needResolve = new List<object>();
        var ordered = GetOrderedBindings(constructionData);
        foreach (var binding in ordered)
        {
            var obj = binding.Getter.Build();
            binding.Aggregate?.Invoke(obj);
            
            var toBind = new List<Type> {binding.BindingType};
            if (binding.Interfaces != null)
                toBind.AddRange(binding.Interfaces);
            bindings.AddRange(toBind.Select(type => new BindingModel(type, binding.Contract, obj)));

            if (!IsNeedResolve(binding)) continue;
            needResolve.Add(obj);
        }

        bool IsNeedResolve(BindingConstructionModel model)
        {
            bool IsIgnored()
            {
                if (model.BindingType.IsPrimitive) return true;
                if (model.BindingType.IsGenericType &&
                    _ignoreTypes.Contains(model.BindingType.GetGenericTypeDefinition())) return true;
                return false;
            }

            bool IsAlreadyResolved()
            {
                return model.Getter.GetType().GetGenericTypeDefinition() == typeof(InstanceObjectBuilder<>);
            }

            return !IsIgnored() && !IsAlreadyResolved();
        }

        return needResolve;
    }

    private IEnumerable<BindingConstructionModel> GetOrderedBindings(IEnumerable<BindingConstructionModel> bindings)
    {
        var dict = new Dictionary<BindingConstructionModel, int>();
        foreach (var model in bindings)
        {
            var methodDepth = model.BindingType.IsPrimitive ? 0 : CalculateDepth(model.BindingType, model.BindingType);

            if (methodDepth == CIRCULAR_DEPENDENCY_ERROR)
                break;

            dict.Add(model, methodDepth);
        }

        return dict.OrderBy(pair => pair.Value)
            .Select(dict1 => dict1.Key).ToList();
    }
    
    //Type root only for debug
    private int CalculateDepth(Type type, Type rootBinding, int depth = 1)
    {
        if (type.IsPrimitive)
        {
            return depth;
        }

        var methods = _injectionStrategy.GetInjectionMethods(type);
        var fields = _injectionStrategy.GetInjectionFields(type);
        
        if (methods != null)
        {
            foreach (var method in methods)
            {
                var parameterTypes = method.GetParameters().Select(argumentInfo => argumentInfo.ParameterType)
                    .ToArray();
                foreach (var paramType in parameterTypes)
                {
                    if (paramType == rootBinding)
                        return CircularDependencyError(rootBinding, type);
                    depth = CalculateDepth(paramType, rootBinding, depth + 1);
                }
            }
        }

        if (fields != null)
        {
            foreach (var field in fields)
            {
                if (field.FieldType == rootBinding)
                    return CircularDependencyError(rootBinding, type);
                depth = CalculateDepth(field.FieldType, rootBinding, depth + 1);
            }
        }
        
        return depth;
    }

    private int CircularDependencyError(Type target, Type param)
    {
        Debug.LogError($"Find circular dependency. Inject target {target} param {param}");
        return CIRCULAR_DEPENDENCY_ERROR;
    }
}