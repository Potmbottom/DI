using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class UnityPoolManager<T> : PoolManager<T> where T : MonoBehaviour
{
    private readonly Transform _root;

    public UnityPoolManager(Func<object> getFunc, int initSize) : base(getFunc, initSize)
    {
        _root = Object.FindObjectOfType<UnityPoolHandler>().transform;
    }

    protected override void OnExpand(object data)
    {
        var component = ((T)data).gameObject.AddComponent<PoolGameObject>();
        BackToPoolRoot(component);
    }

    public override T Get()
    {
        var data = base.Get();
        var component = data.gameObject.GetComponent<PoolGameObject>();
        component.OnSpawned(this);
        data.gameObject.SetActive(true);
        return data;
    }

    public override void Release(object data)
    {
        var component = (PoolGameObject) data;
        base.Release(component.gameObject.GetComponent<T>());
        component.OnDespawned();
        BackToPoolRoot(component);
    }
    
    private void BackToPoolRoot(PoolGameObject data)
    {
        if(_root == null) return; 
        data.transform.SetParent(_root, false);
        data.gameObject.SetActive(false);
    }

    protected override void OnDispose(T data)
    {
        Object.Destroy(data.gameObject);
    }
}