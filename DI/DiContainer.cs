using System.Collections.Generic;
using System.Linq;

public interface IResolver
{
    void Resolve(object obj);
}

public interface IBinder
{
    void AddToBind(BindingConstructionModel constructionModel);
}

public interface IPoolHandler
{
    void BindPool(IPoolManager pool);
    void InitPools();
    void DisposePools();
}

public class DiContainer : IResolver, IBinder, IPoolHandler
{
    private readonly Stack<DiContext> _containersStack = new Stack<DiContext>();
    private readonly IInjectionStrategy _injectionStrategy = new DefaultInjectionStrategy();
    private readonly DependencyResolver _resolver;
    private readonly DependencyBuilder _builder;
    
    private DiContext _current => _containersStack.Peek();

    public DiContainer()
    {
        _resolver = new DependencyResolver(_injectionStrategy);
        _builder = new DependencyBuilder(_injectionStrategy);
    }

    public void AddContext()
    {
        _containersStack.Push(new DiContext(new DefaultInjectionStrategy()));
    }

    public void RemoveContext()
    {
        _containersStack.Pop().Dispose();
    }

    public void Resolve(object obj)
    {
        var bindings = GetContextBindings();
        _resolver.Resolve(obj, bindings);
    }

    //Resolving dependencies for top DIContainer
    public void ResolveDependencies()
    {
        var toResolve = _builder.Build(_current.ConstructionData, _current.Bindings);
        var bindings = GetContextBindings();
        foreach (var obj in toResolve)
        {
            _resolver.Resolve(obj, bindings);
        }
        _current.ConstructionData.Clear();
    }

    private List<BindingModel> GetContextBindings()
    {
        return _containersStack
            .ToArray()
            .SelectMany(container => container.Bindings)
            .ToList();
    }

    public void AddToBind(BindingConstructionModel constructionModel)
    {
        _current.AddToBind(constructionModel);
    }

    public void BindPool(IPoolManager pool)
    {
        _current.BindPool(pool);
    }

    public void InitPools()
    {
        _current.InitPools();
    }

    public void DisposePools()
    {
        _current.DisposePools();
    }
}