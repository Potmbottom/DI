using System.Linq;
using UnityEngine;

public class PoolGameObject : MonoBehaviour, IPoolObject
{
    private IPoolManager _pool;
    private IUnbindableControl[] _unbindable;
    
    private Vector3 _initialPosition;
    private Vector2 _initialAnchorMin;
    private Vector2 _initialAnchorMax;
    private Vector2 _initialPivot;
    
    private void Awake()
    {
        _unbindable = GetComponents<IUnbindableControl>();
        //SaveRectTransformInitState(); 
    }

    public void OnSpawned(IPoolManager pool)
    {
        _pool = pool;
        //ResetRectTransform();
    }
    
    public void OnDespawned()
    {
        _pool = null;
    }

    public virtual void Release()
    {
        var controls = GetComponentsInChildren<IUnbindableControl>(true);
        foreach (var control in controls)
        {
            control.Unbind();
        }
        
        var child = GetComponentsInChildren<PoolGameObject>(true).Where(o => o != this);
        foreach (var poolableObject in child)
        {
            poolableObject.Release();
        }
        
        foreach (var control in _unbindable)
        {
            control.Unbind();
        }
        
        _pool.Release(this);
    }
    
    private void SaveRectTransformInitState()
    {
        if (transform is RectTransform rect)
        {
            _initialPosition = rect.position;
            _initialAnchorMin = rect.anchorMin;
            _initialAnchorMax = rect.anchorMax;
            _initialPivot = rect.pivot;
        }
    }

    private void ResetRectTransform()
    {
        if (transform is RectTransform rect)
        {
            rect.pivot = _initialPivot;
            rect.anchorMin = _initialAnchorMin;
            rect.anchorMax = _initialAnchorMax;
            rect.position = _initialPosition;
        }
    }

}