using System.Collections.Generic;
using UnityEngine;

public class TestInstaller : MonoBehaviour
{
    public TestFactoryHolder TestFactoryHolder;
    
    public void Awake()
    {
        Simple_Injection();
        Interface_Injection();
        FieldAndMethod_Injection();
    }

    private void ContextCreateRemove()
    {

    }

    private void Simple_Injection()
    {
        var container = new DiContainer();
        container.AddContext();
        var context = (IBinder) container;
        
        context.Bind<SimpleA>();
        context.Bind<SimpleB>();
        context.BindInstance(12);
        
        container.ResolveDependencies();
    }

    private void Interface_Injection()
    {
        var container = new DiContainer();
        container.AddContext();
        var context = (IBinder) container;
        
        context.BindInterfaces<InterfaceA>();
        context.BindInterfaces<InterfaceB>();
        context.BindInstance(12);
        
        container.ResolveDependencies();
    }

    private void FieldAndMethod_Injection()
    {
        var container = new DiContainer();
        container.AddContext();
        var context = (IBinder) container;

        context.Bind<FieldAndMethodA>();
        context.Bind<FieldAndMethodB>();
        context.Bind<FieldAndMethodC>();
        context.Bind<FieldAndMethodD>();
        context.Bind<FieldAndMethodX>();
        
        container.ResolveDependencies();
    }

    private void ManyMethods_Injection(DiContainer container)
    {
        container.AddContext();
        var binder = (IBinder) container;
        
        binder.BindInterfaces<InterfaceA>();
        binder.BindInterfaces<InterfaceB>();
        binder.BindInstance(12);
        
        container.ResolveDependencies();
    }

    private void Factory_Injection()
    {
        /*var container = new DIContainer();
        container.BindIFactory<TestControl>().
        container.BindInterfaces<InterfaceB>();
        container.BindInstance(12);
        container.ResolveBindDependencies();*/
    }
}