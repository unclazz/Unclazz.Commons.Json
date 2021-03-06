﻿namespace Unclazz.Commons.Json
{
	sealed class BooleanJsonObject : JsonObject
	{
		private readonly bool val;

		internal BooleanJsonObject(bool val) : base(JsonObjectType.Boolean)
		{
			this.val = val;
		}
		public override bool AsBoolean()
		{
			return val;
		}
		public override bool AsBoolean(bool fallback)
		{
			return val;
		}
		public override string ToString()
		{
			return val ? "true" : "false";
		}
		public override int GetHashCode()
		{
			return val ? 1 : 0;
		}
		public override bool Equals(object obj)
		{
			var other = obj as BooleanJsonObject;
			return other != null && this.val == other.val;
		}
	}
}

