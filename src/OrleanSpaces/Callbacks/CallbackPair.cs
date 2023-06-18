namespace OrleanSpaces.Callbacks;

internal readonly struct CallbackPair<TTuple, TTemplate>
{
    public readonly TTuple Tuple;
    public readonly TTemplate Template;

    public CallbackPair(TTuple tuple, TTemplate template)
    {
        Tuple = tuple;
        Template = template;
    }
}