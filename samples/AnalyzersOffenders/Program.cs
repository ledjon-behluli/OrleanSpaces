using OrleanSpaces.Tuples;

#region OSA001

SpaceUnit unit1 = new SpaceUnit();
SpaceUnit unit2 = new();
SpaceUnit unit3 = default(SpaceUnit);
SpaceUnit unit4 = default;

SpaceTuple tuple1 = new SpaceTuple();
SpaceTuple tuple2 = new();
SpaceTuple tuple3 = default(SpaceTuple);
SpaceTuple tuple4 = default;

#endregion

#region OSA002

SpaceTuple tuple5 = new SpaceTuple(1, typeof(int), SpaceUnit.Null);

#endregion