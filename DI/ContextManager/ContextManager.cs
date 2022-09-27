using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ContextModel
{
    public string Key;
    public List<Installer> Installers;
}

public class ContextManager
{
    protected readonly DiContainer Context;
    protected ContextModel Model;
    
    private readonly ContextModel[] _modules;
    private Dictionary<string, int> _hierarchy = new Dictionary<string, int>();
    private int _depth;
    
    public ContextManager(DiContainer context, ContextModel[] modules)
    {
        Context = context;
        _modules = modules;
        _depth = 0;
    }

    public void Create(string param)
    {
        Model = _modules.FirstOrDefault(moduleModel => moduleModel.Key == param);
        if (Model == null)
        {
            Debug.LogError($"Cant find module {param}");
            return;
        }
        ResolveHierarchy(param);
        AddContext(Model.Installers);
    }

    private void ResolveHierarchy(string param)
    {
        if (_hierarchy.ContainsKey(param))
        {
            var pathLength = _depth - _hierarchy[param];
            for (var i = 0; i < pathLength; i++)
            {
                Context.DisposePools();
                Context.RemoveContext();
            }
            _depth = _hierarchy[param];
            _hierarchy = _hierarchy.Where(pair => pair.Value <= _depth)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        else
        {
            _hierarchy.Add(param, _depth);
        }
    }

    private void AddContext(IEnumerable<Installer> installers)
    {
        _depth++;
        Context.AddContext();
        foreach (var installer in installers)
        {
            Context.Resolve(installer);
            installer.Install(Context);
        }
        Context.ResolveDependencies();
        Context.InitPools();
    }
}