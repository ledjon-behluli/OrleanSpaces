using OrleanSpaces.Tuples;

#region OSA001

//SpaceUnit unit1 = new SpaceUnit();
//SpaceUnit unit2 = new();
//SpaceUnit unit3 = default(SpaceUnit);
//SpaceUnit unit4 = default;

//SpaceTuple tuple1 = new SpaceTuple();
//SpaceTuple tuple2 = new();
//SpaceTuple tuple3 = default(SpaceTuple);
//SpaceTuple tuple4 = default;

#endregion

#region OSA002

//int a = 1;
//TestClass b = new();
//TestEnum c = TestEnum.A;

//_ = new SpaceTuple(a, b, c,
//    'a', "a", true, false,
//    (byte)1, (sbyte)1, (short)1, (ushort)1, (int)1, (uint)1,
//    (long)1, (ulong)1, (float)1, (decimal)1, (double)1, typeof(int), typeof(string),
//    DateTime.MinValue, DateTimeOffset.MinValue, TimeSpan.MinValue, Guid.Empty,
//    new TestClass(), new TestStruct(), TestEnum.A,
//    SpaceUnit.Null, new SpaceUnit());

//_ = new SpaceTemplate(a, b, c,
//    'a', "a", true, false,
//    (byte)1, (sbyte)1, (short)1, (ushort)1, (int)1, (uint)1,
//    (long)1, (ulong)1, (float)1, (decimal)1, (double)1, typeof(int), typeof(string),
//    DateTime.MinValue, DateTimeOffset.MinValue, TimeSpan.MinValue, Guid.Empty,
//    new TestClass(), new TestStruct(), TestEnum.A,
//    SpaceUnit.Null, new SpaceUnit());

_ = new SpaceTuple(TestEnum.A, 1, typeof(int));

class TestClass { }
struct TestStruct { }
enum TestEnum { A }

#endregion