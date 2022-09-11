using BenchmarkDotNet.Running;
using OrleanSpaces.Primitives;

//BenchmarkRunner.Run<SpaceUnitBenchmarks>();
//BenchmarkRunner.Run<SpaceTupleBenchmarks>();
//BenchmarkRunner.Run<SpaceTemplateBenchmarks>();
BenchmarkRunner.Run<TestRun>();

//SpaceTupleAlloc.Create(1);
//SpaceTupleAlloc.Create("a");
//SpaceTupleAlloc.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));

Console.ReadKey();