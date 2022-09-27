using UnityEngine;

public static class DIExtensions
{
    public static IDataBindingConstructor<T> Bind<T>(this IBinder container)
    {
        var constructor = new DataBindingConstructor<T>();
        constructor.Bind();
        
        container.AddToBind(constructor.Model);
        return constructor;
    }
    
    public static IDataBindingConstructor<T> BindInterfaces<T>(this IBinder container)
    {
        var constructor = new DataBindingConstructor<T>();
        constructor.Bind();
        constructor.WithInterfaces();
        
        container.AddToBind(constructor.Model);
        return constructor;
    }
    
    public static IDataBindingConstructor<T> BindInstance<T>(this IBinder container, T instance)
    {
        var constructor = new DataBindingConstructor<T>();
        constructor.Bind();
        constructor.FromInstance(instance);
        
        container.AddToBind(constructor.Model);
        return constructor;
    }

    public static IFactoryBindingConstructor<T> BindIFactory<T>(this IBinder container) where T : MonoBehaviour
    {
        var constructor = new UnityComponentBindingConstructor<T>((IResolver)container);
        constructor.BindIFactory();
        
        container.AddToBind(constructor.Model);
        return constructor;
    }
}