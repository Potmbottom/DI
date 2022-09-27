using System.Runtime.Serialization;

public class DefaultObjectBuilder<T> : IObjectBuilder
{
    public object Build()
    {
        return FormatterServices.GetUninitializedObject(typeof(T));
    }
}