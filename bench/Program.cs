using BenchmarkDotNet.Running;

BenchmarkRunner.Run<TupleMatcherBenchmarks>();
//BenchmarkRunner.Run<SpaceTupleBenchmarks>();

Console.ReadKey();