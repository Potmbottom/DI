using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IFactoryBindingConstructor<T>
{
    public IFactoryBindingConstructor<T> FromMethod(Func<T> method);
    public IFactoryBindingConstructor<T> FromFactory(IFactory<T> instance);
    public IFactoryBindingConstructor<T> FromPrefab(string path);
    public IAggregatorConstructor<T> WithPool(int initCount);
}

public interface IDataBindingConstructor<T>
{
    public IDataBindingConstructor<T> Bind();
    public IDataBindingConstructor<T> FromInstance(T instance);
    public IDataBindingConstructor<T> WhenInjected<T1>();
    public IDataBindingConstructor<T> WithInterfaces();
}

public interface IAggregatorConstructor<T>
{
    void AttachToAggregator<TKey>(Dictionary<TKey, IFactory<T>> aggregator, TKey value);
}

public abstract class BindingConstructor<T> : IAggregatorConstructor<T>
{
    public BindingConstructionModel Model { get; protected set; }

    public void AttachToAggregator<TKey>(Dictionary<TKey, IFactory<T>> aggregator, TKey value)
    {
        Model.Contract = null;
        Model.Aggregate = data =>
        {
            aggregator.Add(value, (IFactory<T>)data);
        };
    }
}

public class DataBindingConstructor<T> : BindingConstructor<T>, IDataBindingConstructor<T>
{
    public IDataBindingConstructor<T> Bind()
    {
        Model = new BindingConstructionModel
        {
            BindingType = typeof(T),
            Contract = typeof(object),
            Getter = new DefaultObjectBuilder<T>()
        };

        return this;
    }

    public IDataBindingConstructor<T> FromMethod(Func<T> method)
    {
        Model.Getter = new FromMethod<T>(method);
        return this;
    }
    
    public IDataBindingConstructor<T> FromInstance(T instance)
    {
        Model.Getter = new InstanceObjectBuilder<T>(instance);
        return this;
    }

    public IDataBindingConstructor<T> WhenInjected<T1>()
    {
        Model.Contract = typeof(T1);
        return this;
    }

    public IDataBindingConstructor<T> WithInterfaces()
    {
        Model.Interfaces = typeof(T).GetInterfaces();
        return this;
    }
}

public class UnityComponentBindingConstructor<T> : BindingConstructor<T>, IFactoryBindingConstructor<T>
    where T : MonoBehaviour
{
    private readonly IResolver _container;
    private IFactory<T> _factory;
    
    public UnityComponentBindingConstructor(IResolver container)
    {
        _container = container;
    }
    
    public IFactoryBindingConstructor<T> BindIFactory()
    {
        Model = new BindingConstructionModel();
        
        Model.BindingType = typeof(IFactory<T>);
        Model.Contract = typeof(object);
        return this;
    }

    public IFactoryBindingConstructor<T> FromMethod(Func<T> method)
    {
        _factory = new FromMethod<T>(method);
        Model.Getter = new InstanceObjectBuilder<IFactory<T>>(new Factory<T>(_container, _factory));
        return this;
    }

    public IFactoryBindingConstructor<T> FromFactory(IFactory<T> instance)
    {
        _factory = instance;
        Model.Getter = new InstanceObjectBuilder<IFactory<T>>(new Factory<T>(_container, _factory));
        return this;
    }
    
    public IFactoryBindingConstructor<T> FromPrefab(string path)
    {
        _factory = new FactoryFromPrefab<T>(path);
        Model.Getter = new InstanceObjectBuilder<IFactory<T>>(new Factory<T>(_container, _factory));
        return this;
    }

    public IAggregatorConstructor<T> WithPool(int initCount)
    {
        Model.Getter = new InstanceObjectBuilder<IFactory<T>>(new Factory<T>(_container, new FactoryFromPool<T>
            (initCount, (IPoolHandler)_container, _factory)));
        return this;
    }
}
