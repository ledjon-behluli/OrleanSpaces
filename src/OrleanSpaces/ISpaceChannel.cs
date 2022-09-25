namespace OrleanSpaces;

internal interface ISpaceChannel
{
    bool HasActiveConsumer { get; set; }
}