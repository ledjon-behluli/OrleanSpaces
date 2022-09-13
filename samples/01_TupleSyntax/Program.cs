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

    Console.WriteLine($"Space tuple with one field (Int32): {new SpaceTuple(1)}");
    Console.WriteLine($"Space tuple with two fields (ValueTuple): {new SpaceTuple((1, "a"))}");
    Console.WriteLine($"Space tuple with three fields (ValueTuple): {new SpaceTuple(ValueTuple.Create(1, "a", 1.5f))}");

    Console.WriteLine("---------------- END -------------------\n");
}

static void SpaceTemplateSyntax()
{
    Console.WriteLine("\nThis section illustrates SpaceTemplate creation");
    Console.WriteLine("---------------- BEGIN -------------------");

    Console.WriteLine($"Space template with one field (Int32): {new SpaceTemplate(1)}");
    Console.WriteLine($"Space template with two fields (ValueTuple): {new SpaceTemplate((1, "a"))}");
    Console.WriteLine($"Space template with two fields (SpaceTuple): {(SpaceTemplate)new SpaceTuple((1, "a"))}");
    Console.WriteLine($"Space template with three fields (ValueTuple): {new SpaceTemplate(ValueTuple.Create(1, "a", 1.5f))}");
    Console.WriteLine($"Space template with NULL placeholder field: {new SpaceTemplate((1, "a", SpaceUnit.Null, 1.5f))}");
    Console.WriteLine($"Space template with NULL placeholder and \"NULL\" string: {new SpaceTemplate((SpaceUnit.Null, "NULL"))}");

    Console.WriteLine("---------------- END -------------------\n");
}

static void SpaceTupleEquality()
{
    Console.WriteLine("\nThis section illustrates SpaceTuples equality");
    Console.WriteLine("---------------- BEGIN -------------------");

    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTuple(1)}, {new SpaceTuple(1)}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTuple((1, "a"))}, {new SpaceTuple((1, "a"))}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTuple((1, "a", 1.5f))}, {new SpaceTuple((1, "a", 1.5f))}");

    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {new SpaceTuple(1)}, {new SpaceTuple((1, "a"))}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {new SpaceTuple(("a", 1))}, {new SpaceTuple((1, "a"))}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {new SpaceTuple((1, "b"))}, {new SpaceTuple((1, "a"))}");

    Console.WriteLine("---------------- END -------------------\n");
}

static void SpaceTemplateEquality()
{
    Console.WriteLine("\nThis section illustrates SpaceTemplate equality");
    Console.WriteLine("---------------- BEGIN -------------------");
    
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTemplate(1)}, {new SpaceTemplate(1)}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTemplate((1, "a"))}, {new SpaceTemplate((1, "a"))}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTemplate((1, "a", typeof(int)))}, {new SpaceTemplate((1, "a", typeof(int)))}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTemplate((1, "a", typeof(int), SpaceUnit.Null))}, {new SpaceTemplate((1, "a", typeof(int), SpaceUnit.Null))}");

    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {new SpaceTemplate(1)}, {new SpaceTemplate((1, "a"))}");
    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {new SpaceTemplate(1)}, {new SpaceTemplate((1, typeof(string)))}");
    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {new SpaceTemplate(1)}, {new SpaceTemplate((1, SpaceUnit.Null))}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {new SpaceTemplate(("a", 1))}, {new SpaceTemplate((1, "a"))}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {new SpaceTemplate(("a", typeof(int)))}, {new SpaceTemplate((typeof(int), "a"))}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {new SpaceTemplate(("a", SpaceUnit.Null))}, {new SpaceTemplate((SpaceUnit.Null, "a"))}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {new SpaceTemplate((1, "b"))}, {new SpaceTemplate((1, "a"))}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {new SpaceTemplate((1, typeof(int)))}, {new SpaceTemplate((1, typeof(string)))}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {new SpaceTemplate((1, typeof(int)))}, {new SpaceTemplate((1, SpaceUnit.Null))}");

    Console.WriteLine("---------------- END -------------------\n");
}