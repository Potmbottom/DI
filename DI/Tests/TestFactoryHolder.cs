using System;
using UnityEngine;

public class TestFactoryHolder : MonoBehaviour
{
    
    [Inject]
    public void SetDependency(IFactory<TestControl> fact)
    {
        try
        {
            fact.Create();
        }
        catch (Exception e)
        {
            Debug.LogError("Factory inject failed.");
            return;
        }
        
        Debug.LogError("Factory inject success.");
    }
}