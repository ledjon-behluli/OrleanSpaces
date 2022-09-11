using BenchmarkDotNet.Running;

BenchmarkRunner.Run<SpaceUnitBenchmarks>();
BenchmarkRunner.Run<SpaceTupleBenchmarks>();
BenchmarkRunner.Run<SpaceTemplateBenchmarks>();

Console.ReadKey();