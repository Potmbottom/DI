using UnityEngine;

public class FieldAndMethodA
{
    [Inject] public FieldAndMethodB b;

    public FieldAndMethodC C;
    
    public FieldAndMethodA(FieldAndMethodC c)
    {
        C = c;
    }
}

public class FieldAndMethodB
{
}

public class FieldAndMethodC
{
    [Inject] public FieldAndMethodD d;
    
}

public class FieldAndMethodD
{

}

public class FieldAndMethodX
{
    public FieldAndMethodX(FieldAndMethodA a)
    {
        Debug.Log("FieldAndMethod injection success.");
    }
}