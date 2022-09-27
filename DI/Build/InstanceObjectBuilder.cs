public class InstanceObjectBuilder<T> : IObjectBuilder
{
    private readonly T _instance;
    
    public InstanceObjectBuilder(T instance)
    {
        _instance = instance;
    }
    
    public object Build()
    {
        return _instance;
    }
}