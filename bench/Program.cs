using BenchmarkDotNet.Running;

BenchmarkRunner.Run<TupleMatcherBenchmarks>();
BenchmarkRunner.Run<SpaceTupleBenchmarks>();
BenchmarkRunner.Run<SpaceTemplateBenchmarks>();
BenchmarkRunner.Run<SpaceUnitBenchmarks>();

Console.ReadKey();