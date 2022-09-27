using System.Linq;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private UnityContextModel[] _installers;
    [SerializeField] private Transform _root;

    //Execute very first
    public void Awake()
    {
        //Create container and add root context
        var context = new UnityContainer();
        var moduleFactory = new UnityContextManager(context, _installers);
        
        //Bind factory to 0 context
        context.AddContext();
        context.BindInstance(moduleFactory);
        context.ResolveDependencies();
        
        //Create first module
        moduleFactory.Create(_installers.FirstOrDefault().Key, _root);
    }
}