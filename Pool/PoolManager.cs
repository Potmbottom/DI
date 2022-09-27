using System;

public interface IPoolManager
{
   void Release(object data);
   void Init();
   void Dispose();
}

public interface IPoolManager<out T> : IPoolManager
{
   T Get();
}

public class PoolManager<T> : IPoolManager<T>
{
   private readonly IPool _pool;
   private int _initSize;

   public PoolManager(Func<object> getFunc, int initSize)
   {
      _pool = new Pool(getFunc);
      _initSize = initSize;
   }

   public void Init()
   {
      _pool.Expand(_initSize, OnExpand);
   }

   public void Dispose()
   {
      while (_pool.Count > 0)
      {
         OnDispose((T)_pool.Get());
      }
   }

   public virtual T Get()
   {
      if (_pool.Count <= 0)
      {
         _pool.Expand(_initSize, OnExpand);
      }

      var item = _pool.Get();
      return (T)item;
   }

   public virtual void Release(object data)
   {
      _pool.Release(data);
   }

   protected virtual void OnExpand(object data){ }
   protected virtual void OnDispose(T data){ }
}

