namespace OrleanSpaces;

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
internal class InternalUseOnlyAttribute : Attribute
{
    public InternalUseOnlyAttribute()
    {
    }
}
