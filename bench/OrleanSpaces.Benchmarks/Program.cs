using BenchmarkDotNet.Running;

//BenchmarkRunner.Run<TypeCheckerBenchmarks>();
//BenchmarkRunner.Run<SpaceUnitBenchmarks>();
//BenchmarkRunner.Run<SpaceTupleBenchmarks>();
//BenchmarkRunner.Run<SpaceTemplateBenchmarks>();
//BenchmarkRunner.Run<DateTimeTupleBenchmarks>();
//BenchmarkRunner.Run<DateTimeOffsetTupleBenchmarks>();
//BenchmarkRunner.Run<TimeSpanTupleBenchmarks>();
//BenchmarkRunner.Run<CharTupleBenchmarks>();
//BenchmarkRunner.Run<EnumTupleBenchmarks>();
//BenchmarkRunner.Run<BoolTupleBenchmarks>();
//BenchmarkRunner.Run<DecimalTupleBenchmarks>();
//BenchmarkRunner.Run<Int16TupleBenchmarks>();

BenchmarkRunner.Run<TestBench>();

Console.ReadKey();