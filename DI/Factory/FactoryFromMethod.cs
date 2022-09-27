using System;

public class FromMethod<T> : IFactory<T>, IObjectBuilder
{
    private readonly Func<T> _getter;

    public FromMethod(Func<T> getter)
    {
        _getter = getter;
    }
    
    public T Create()
    {
        return _getter.Invoke();
    }

    public object Build()
    {
        return _getter.Invoke();
    }
}