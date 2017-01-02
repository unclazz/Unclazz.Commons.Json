namespace Unclazz.Commons.Json
{
	sealed class NumberJsonObject : JsonObject
	{
		private readonly double val;

		internal NumberJsonObject(double val) : base(JsonObjectType.Number)
		{
			this.val = val;
		}

		public override double AsNumber()
		{
			return val;
		}
		public override double AsNumber(double fallback)
		{
			return val;
		}
		public override string ToString()
		{
			return val.ToString();
		}
		public override int GetHashCode()
		{
			return val.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			var other = obj as NumberJsonObject;
			return other != null && this.val.Equals(other.val);
		}
	}
}

