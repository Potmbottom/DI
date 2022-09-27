using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class BindingModel
{
    public readonly Type BindingType;
    public readonly Type Contract; //null - duplicate(resolve with aggregators), object - can inject in any type
    public readonly object Object;

    public BindingModel(Type bindingType, Type contract, object obj)
    {
        Object = obj;
        BindingType = bindingType;
        Contract = contract;
    }
}

public class BindingConstructionModel
{
    public Type BindingType;
    public Type[] Interfaces;
    public Type Contract; //null - duplicate(resolve with aggregators), object - can inject in any type
    public IObjectBuilder Getter;
    public Action<object> Aggregate;
}