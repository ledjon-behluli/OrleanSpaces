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

int b = 1;
DateTime dateTime;
DateTimeOffset dateTimeOffset;

SpaceTuple _ = new(1, 1.5f, 1.3d, "a", 'a', true, typeof(int), SpaceUnit.Null, b);


#endregion