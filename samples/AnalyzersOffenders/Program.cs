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

//int intVar = 1;
//TestClass classVar = new();
//TestStruct structVar = new();
//TestEnum enumVar = TestEnum.A;

//_ = new SpaceTuple(
//    'a', "a",
//    true, false,
//    SpaceUnit.Null, new SpaceUnit(),
//    (byte)1, (sbyte)1, (short)1, (ushort)1, (int)1, (uint)1, (long)1, (ulong)1, (float)1, (decimal)1, (double)1, typeof(int), typeof(string),
//    DateTime.MinValue, DateTimeOffset.MinValue, TimeSpan.MinValue, Guid.Empty,
//    1, new TestClass(), new TestStruct(), TestEnum.A,
//    intVar, classVar, structVar, enumVar);

//_ = new SpaceTemplate(
//    'a', "a",
//    true, false,
//    SpaceUnit.Null, new SpaceUnit(),
//    (byte)1, (sbyte)1, (short)1, (ushort)1, (int)1, (uint)1, (long)1, (ulong)1, (float)1, (decimal)1, (double)1, typeof(int), typeof(string),
//    DateTime.MinValue, DateTimeOffset.MinValue, TimeSpan.MinValue, Guid.Empty,
//    1, new TestClass(), new TestStruct(), TestEnum.A,
//    intVar, classVar, structVar, enumVar);

//class TestClass { }
//struct TestStruct { }
//enum TestEnum { A }

#endregion

#region OSA003

SpaceTemplate template1 = new();
SpaceTemplate template2 = new(SpaceUnit.Null);
SpaceTemplate template3 = new(SpaceUnit.Null, SpaceUnit.Null);

#endregion
