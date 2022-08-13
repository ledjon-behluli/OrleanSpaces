using System;
using System.Runtime.CompilerServices;

namespace OrleanSpaces
{
    public struct SpaceTemplate : ITuple
    {
        private readonly object[] _items;

        public int Length => _items.Length;
        public object this[int index] => _items[index];

        private SpaceTemplate(params object[] items)
        {
            if (items.Length == 0)
            {
                throw new OrleanSpacesException($"At least one object must be present to construct a {nameof(SpaceTemplate)}.");
            }

            _items = items;
        }

        public static SpaceTemplate Create(object item) => new SpaceTemplate(item);

        public static SpaceTemplate Create(ITuple tuple)
        {
            if (tuple is null)
            {
                throw new ArgumentNullException(nameof(tuple));
            }

            var items = new object[tuple.Length];

            for (int i = 0; i < tuple.Length; i++)
            {
                items[i] = tuple[i];
            }

            return new SpaceTemplate(items);
        }

        public static implicit operator SpaceTuple(SpaceTemplate template) => SpaceTuple.Create(template._items);
    }
}
