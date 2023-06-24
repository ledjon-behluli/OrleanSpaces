using OrleanSpaces.Tuples;

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

    Console.WriteLine($"Space tuple with one field: {new SpaceTuple(1)}");
    Console.WriteLine($"Space tuple with two fields: {new SpaceTuple(1, "a")}");
    Console.WriteLine($"Space tuple with three fields: {new SpaceTuple(1, "a", 1.5f)}");

    Console.WriteLine("---------------- END -------------------\n");
}

static void SpaceTemplateSyntax()
{
    Console.WriteLine("\nThis section illustrates SpaceTemplate creation");
    Console.WriteLine("---------------- BEGIN -------------------");

    Console.WriteLine($"Space template with one field: {new SpaceTemplate(1)}");
    Console.WriteLine($"Space template with two fields: {new SpaceTemplate(1, "a")}");
    Console.WriteLine($"Space template with three fields: {new SpaceTemplate(1, "a", 1.5f)}");
    Console.WriteLine($"Space template implicit casting from SpaceTuple: {(SpaceTemplate)new SpaceTuple(1, "a")}");
    Console.WriteLine($"Space template with null field: {new SpaceTemplate(1, "a", null, 1.5f)}");
    Console.WriteLine($"Space template with null and \"NULL\" string: {new SpaceTemplate(null, "NULL")}");

    Console.WriteLine("---------------- END -------------------\n");
}

static void SpaceTupleEquality()
{
    Console.WriteLine("\nThis section illustrates SpaceTuples equality");
    Console.WriteLine("---------------- BEGIN -------------------");

    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTuple(1)}, {new SpaceTuple(1)}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTuple(1, "a")}, {new SpaceTuple(1, "a")}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTuple(1, "a", 1.5f)}, {new SpaceTuple(1, "a", 1.5f)}");

    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {new SpaceTuple(1)}, {new SpaceTuple(1, "a")}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {new SpaceTuple("a", 1)}, {new SpaceTuple(1, "a")}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {new SpaceTuple(1, "b")}, {new SpaceTuple(1, "a")}");

    Console.WriteLine("---------------- END -------------------\n");
}

static void SpaceTemplateEquality()
{
    Console.WriteLine("\nThis section illustrates SpaceTemplate equality");
    Console.WriteLine("---------------- BEGIN -------------------");
    
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTemplate(1)}, {new SpaceTemplate(1)}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTemplate(1, "a")}, {new SpaceTemplate(1, "a")}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTemplate(1, "a", typeof(int))}, {new SpaceTemplate(1, "a", typeof(int))}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {new SpaceTemplate(1, "a", typeof(int), null)}, {new SpaceTemplate(1, "a", typeof(int), null)}");

    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {new SpaceTemplate(1)}, {new SpaceTemplate(1, "a")}");
    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {new SpaceTemplate(1)}, {new SpaceTemplate(1, typeof(string))}");
    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {new SpaceTemplate(1)}, {new SpaceTemplate(1, null)}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {new SpaceTemplate("a", 1)}, {new SpaceTemplate(1, "a")}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {new SpaceTemplate("a", typeof(int))}, {new SpaceTemplate(typeof(int), "a")}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {new SpaceTemplate("a", null)}, {new SpaceTemplate(null, "a")}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {new SpaceTemplate(1, "b")}, {new SpaceTemplate(1, "a")}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {new SpaceTemplate(1, typeof(int))}, {new SpaceTemplate(1, typeof(string))}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {new SpaceTemplate(1, typeof(int))}, {new SpaceTemplate(1, null)}");

    Console.WriteLine("---------------- END -------------------\n");
}