
using OrleanSpaces.Tuples.Typed;

Span<char> chars = stackalloc char[48];
bool result = new IntTuple().TryFormat(chars, out int w);

Span<char> chars1 = stackalloc char[48];
bool result1 = new IntTuple(1).TryFormat(chars, out int w1);

Span<char> chars2 = stackalloc char[48];
bool result2 = new IntTuple(1, 1).TryFormat(chars, out int w2);

//int[] array = new int[4] { 1, 1, 1, 1 }; 

//long memoryBefore = GC.GetTotalMemory(true);

//Test(array);

//long memoryAfter = GC.GetTotalMemory(true);
//long memoryUsed = memoryAfter - memoryBefore;

//memoryUsed = memoryUsed > 0 ? memoryUsed : 0;

//Console.WriteLine($"Memory used: {memoryUsed} bytes");

Console.ReadKey();

//static void Test(int[] array)
//{
//    Span<char> chars = stackalloc char[48];
//    bool result = new IntTuple(array).TryFormat(chars, out int w);
//}