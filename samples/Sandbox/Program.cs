using OrleanSpaces.Tuples.Typed;

StringTuple tuple = new("12", "34", "56");

StringTemplate template1 = new("12", "34", "56");
template1.Matches(tuple);

Console.ReadKey();