using OrleanSpaces.Tuples.Specialized;

public readonly struct IntTemplateCache
{
#pragma warning disable OSA002
    private static readonly IntTemplate template_1 = new(null);
#pragma warning restore OSA002

    public static ref readonly IntTemplate Template_1 => ref template_1;
}