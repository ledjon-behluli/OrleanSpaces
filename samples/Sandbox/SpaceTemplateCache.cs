using OrleanSpaces.Tuples;

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA002
    private static readonly SpaceTemplate template_1 = new(null);
#pragma warning restore OSA002

    public static ref readonly SpaceTemplate Template_1 => ref template_1;
}