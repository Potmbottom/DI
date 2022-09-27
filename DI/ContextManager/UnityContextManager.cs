using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class UnityContextModel : ContextModel
{
    public string Path;
}

public class UnityContextManager : ContextManager
{
    private Module _currentModule;
    
    public UnityContextManager(UnityContainer context, UnityContextModel[] modules) : base(context, modules) { }

    public void Create(string param, Transform root)
    {
        base.Create(param);

        if (_currentModule != null)
        {
            Object.Destroy(_currentModule.gameObject);
        }

        var model = (UnityContextModel) Model;
        if(model.Path == null) return;
        
        var factory = new Factory<Module>(Context, new FactoryFromPrefab<Module>(model.Path));
        _currentModule = factory.Create();
        ((UnityContainer) Context).ResolveDependenciesForGameObject(_currentModule.gameObject);
        
        _currentModule.transform.SetParent(root, false);
        _currentModule.Init();
    }
}