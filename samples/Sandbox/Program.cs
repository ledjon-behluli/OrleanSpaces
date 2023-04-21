
using Newtonsoft.Json.Linq;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

long kbAtExecution = GC.GetTotalMemory(false) / 1024;

Span<char> chars = stackalloc char[12];
new IntTuple(1, 1, 1, 1).TryFormat(chars, out _);
//var a = chars.ToString();

Console.ReadKey();
