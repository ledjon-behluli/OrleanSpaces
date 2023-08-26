using BenchmarkDotNet.Running;

//BenchmarkRunner.Run<SpaceTupleBenchmarks>();
//BenchmarkRunner.Run<BoolTupleBenchmarks>();
//BenchmarkRunner.Run<ByteTupleBenchmarks>();
//BenchmarkRunner.Run<CharTupleBenchmarks>();
//BenchmarkRunner.Run<DateTimeOffsetTupleBenchmarks>();
//BenchmarkRunner.Run<DateTimeTupleBenchmarks>();
//BenchmarkRunner.Run<DecimalTupleBenchmarks>();
//BenchmarkRunner.Run<DoubleTupleBenchmarks>();
//BenchmarkRunner.Run<FloatTupleBenchmarks>();
//BenchmarkRunner.Run<GuidTupleBenchmarks>();
//BenchmarkRunner.Run<TimeSpanTupleBenchmarks>();
//BenchmarkRunner.Run<HugeTupleBenchmarks>();
//BenchmarkRunner.Run<IntTupleBenchmarks>();
//BenchmarkRunner.Run<LongTupleBenchmarks>();
//BenchmarkRunner.Run<ShortTupleBenchmarks>();
//BenchmarkRunner.Run<TimeSpanTupleBenchmarks>();
//BenchmarkRunner.Run<SpaceTemplateBenchmarks>();

BenchmarkRunner.Run<TupleCollectionBenchmarks>();

Console.ReadKey();
