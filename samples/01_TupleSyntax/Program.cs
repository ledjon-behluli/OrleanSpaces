using OrleanSpaces.Primitives;

SpaceTupleSyntax();
SpaceTemplateSyntax();
SpaceTupleEquality();
SpaceTemplateEquality();

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

static void SpaceTupleSyntax()
{
    Console.WriteLine("\nThis section illustrates SpaceTuple creation");
    Console.WriteLine("---------------- BEGIN -------------------");

    Console.WriteLine($"Space tuple with one field (Object): {SpaceTuple.Create(1)}");
    Console.WriteLine($"Space tuple with two fields (Tuple): {SpaceTuple.Create(Tuple.Create(1, "a"))}");
    Console.WriteLine($"Space tuple with two fields (ValueTuple): {SpaceTuple.Create((1, "a"))}");
    Console.WriteLine($"Space tuple with three fields (ValueTuple): {SpaceTuple.Create(ValueTuple.Create(1, "a", 1.5f))}");

    Console.WriteLine("---------------- END -------------------\n");
}

static void SpaceTemplateSyntax()
{
    Console.WriteLine("\nThis section illustrates SpaceTemplate creation");
    Console.WriteLine("---------------- BEGIN -------------------");

    Console.WriteLine($"Space template with one field (Object): {SpaceTemplate.Create(1)}");
    Console.WriteLine($"Space template with two fields (Tuple): {SpaceTemplate.Create(Tuple.Create(1, "a"))}");
    Console.WriteLine($"Space template with two fields (ValueTuple): {SpaceTemplate.Create((1, "a"))}");
    Console.WriteLine($"Space template with two fields (SpaceTuple): {SpaceTemplate.Create(SpaceTuple.Create((1, "a")))}");
    Console.WriteLine($"Space template with three fields (ValueTuple): {SpaceTemplate.Create(ValueTuple.Create(1, "a", 1.5f))}");
    Console.WriteLine($"Space template with NULL placeholder field: {SpaceTemplate.Create((1, "a", UnitField.Null, 1.5f))}");
    Console.WriteLine($"Space template with NULL placeholder and \"NULL\" string: {SpaceTemplate.Create((UnitField.Null, "NULL"))}");

    Console.WriteLine("---------------- END -------------------\n");
}

static void SpaceTupleEquality()
{
    Console.WriteLine("\nThis section illustrates SpaceTuples equality");
    Console.WriteLine("---------------- BEGIN -------------------");

    Console.WriteLine($"These are equal (fields, types, values, indices - match): {SpaceTuple.Create(1)}, {SpaceTuple.Create(1)}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {SpaceTuple.Create((1, "a"))}, {SpaceTuple.Create((1, "a"))}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {SpaceTuple.Create((1, "a", 1.5f))}, {SpaceTuple.Create((1, "a", 1.5f))}");

    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {SpaceTuple.Create(1)}, {SpaceTuple.Create((1, "a"))}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {SpaceTuple.Create(("a", 1))}, {SpaceTuple.Create((1, "a"))}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {SpaceTuple.Create((1, "b"))}, {SpaceTuple.Create((1, "a"))}");

    Console.WriteLine("---------------- END -------------------\n");
}

static void SpaceTemplateEquality()
{
    Console.WriteLine("\nThis section illustrates SpaceTemplate equality");
    Console.WriteLine("---------------- BEGIN -------------------");

    Console.WriteLine($"These are equal (fields, types, values, indices - match): {SpaceTemplate.Create(1)}, {SpaceTemplate.Create(1)}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {SpaceTemplate.Create((1, "a"))}, {SpaceTemplate.Create((1, "a"))}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {SpaceTemplate.Create((1, "a", typeof(int)))}, {SpaceTemplate.Create((1, "a", typeof(int)))}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {SpaceTemplate.Create((1, "a", typeof(int), UnitField.Null))}, {SpaceTemplate.Create((1, "a", typeof(int), UnitField.Null))}");

    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {SpaceTemplate.Create(1)}, {SpaceTemplate.Create((1, "a"))}");
    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {SpaceTemplate.Create(1)}, {SpaceTemplate.Create((1, typeof(string)))}");
    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {SpaceTemplate.Create(1)}, {SpaceTemplate.Create((1, UnitField.Null))}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {SpaceTemplate.Create(("a", 1))}, {SpaceTemplate.Create((1, "a"))}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {SpaceTemplate.Create(("a", typeof(int)))}, {SpaceTemplate.Create((typeof(int), "a"))}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {SpaceTemplate.Create(("a", UnitField.Null))}, {SpaceTemplate.Create((UnitField.Null, "a"))}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {SpaceTemplate.Create((1, "b"))}, {SpaceTemplate.Create((1, "a"))}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {SpaceTemplate.Create((1, typeof(int)))}, {SpaceTemplate.Create((1, typeof(string)))}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {SpaceTemplate.Create((1, typeof(int)))}, {SpaceTemplate.Create((1, UnitField.Null))}");

    Console.WriteLine("---------------- END -------------------\n");
}