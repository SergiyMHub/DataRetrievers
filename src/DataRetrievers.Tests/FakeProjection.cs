namespace DataRetrievers.Tests
{

    public class FakeProjection
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? NullableIntProperty { get; set; }
        public object ObjectProperty { get; set; }
        public bool BoolProperty { get; internal set; }
    }

}

