using System;

public interface IPool
{
    int Count { get; } //available elements count
    void Expand(int count, Action<object> onElementCreate);
    object Get();
    void Release(object data);
}