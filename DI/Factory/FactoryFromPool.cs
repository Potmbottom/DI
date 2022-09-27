using UnityEngine;

public class FactoryFromPool<T> : IFactory<T> where T : MonoBehaviour
{
    private readonly IPoolManager<T> _pool;
    
    public FactoryFromPool(int count, IPoolHandler context, IFactory<T> fact)
    {
        _pool = new UnityPoolManager<T>(fact.Create, count);
        context.BindPool(_pool);
    }
    
    public T Create()
    {
        return _pool.Get();
    }
}