using BenchmarkDotNet.Running;

//BenchmarkRunner.Run<TypeCheckerBenchmarks>();
//BenchmarkRunner.Run<SpaceUnitBenchmarks>();
//BenchmarkRunner.Run<SpaceTupleBenchmarks>();
//BenchmarkRunner.Run<SpaceTemplateBenchmarks>();

BenchmarkRunner.Run<IntTupleBenchmarks>();

Console.ReadKey();