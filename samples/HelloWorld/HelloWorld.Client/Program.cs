using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using OrleanSpaces.Clients;
using OrleanSpaces.Core.Primitives;

/*
var client = new ClientBuilder()
    .UseLocalhostClustering()
    .UseTupleSpace()
    .Build();

await client.Connect();

Console.WriteLine("Connected to tuple space.");

var spaceClient = client.ServiceProvider.GetRequiredService<ISpaceClient>();
*/

SpaceTupleSyntax();
SpaceTemplateSyntax();
SpaceTupleEqualities();

// This section describes how to interact with the tuple space
// ---------------- BEGIN -------------------

//int count = await spaceClient.CountAsync();
//Console.WriteLine($"Total tuples in space: {count}");

// ----------------- END --------------------

Console.WriteLine("\n\n Press any key to terminate...\n\n");
Console.ReadLine();

//await client.Close();

static void SpaceTupleSyntax()
{
    Console.WriteLine("\nThis section describes how SpaceTuple works");
    Console.WriteLine("---------------- BEGIN -------------------");

    Console.WriteLine($"Space tuple with one field (Object): {SpaceTuple.Create(1)}");
    Console.WriteLine($"Space tuple with two fields (Tuple): {SpaceTuple.Create(Tuple.Create(1, "a"))}");
    Console.WriteLine($"Space tuple with two fields (ValueTuple): {SpaceTuple.Create((1, "a"))}");
    Console.WriteLine($"Space tuple with three fields (ValueTuple): {SpaceTuple.Create(ValueTuple.Create(1, "a", 1.5f))}");

    Console.WriteLine("---------------- END -------------------\n");
}

static void SpaceTemplateSyntax()
{
    Console.WriteLine("\nThis section describes how SpaceTemplate works");
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

static void SpaceTupleEqualities()
{
    Console.WriteLine("\nThis section describes SpaceTuples equalities");
    Console.WriteLine("---------------- BEGIN -------------------");

    Console.WriteLine($"These are equal (fields, types, values, indices - match): {SpaceTuple.Create(1)}, {SpaceTuple.Create(1)}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {SpaceTuple.Create((1, "a"))}, {SpaceTuple.Create((1, "a"))}");
    Console.WriteLine($"These are equal (fields, types, values, indices - match): {SpaceTuple.Create((1, "a", 1.5f))}, {SpaceTuple.Create((1, "a", 1.5f))}");

    Console.WriteLine($"These are not equal (types, values, indices match - fields don't): {SpaceTuple.Create(1)}, {SpaceTuple.Create((1, "a"))}");
    Console.WriteLine($"These are not equal (types, values, fields match - indices don't): {SpaceTuple.Create(("a", 1))}, {SpaceTuple.Create((1, "a"))}");
    Console.WriteLine($"These are not equal (types, fields, indices match - values don't): {SpaceTuple.Create((1, "b"))}, {SpaceTuple.Create((1, "a"))}");

    Console.WriteLine("---------------- END -------------------\n");
}