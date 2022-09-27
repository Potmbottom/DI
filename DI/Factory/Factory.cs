public class Factory<T> : IFactory<T>
{
    private readonly IResolver _container;
    private readonly IFactory<T> _concreteFactory;
    
    public Factory(IResolver container, IFactory<T> factory)
    {
        _container = container;
        _concreteFactory = factory;
    }

    public T Create()
    {
        var item = _concreteFactory.Create();
        _container.Resolve(item);
        return item;
    }
}