using System;

namespace Unclazz.Commons.Json.Sample
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			// 1. Read JSON
			// JsonObject.FromXxxx methods read JSON from String, File and Stream.
			IJsonObject j1 = JsonObject.FromString("{foo: 123, bar: true, baz: \"hello\"}");
			// IJsonObject#GetProperty() accesses JSON object's property. 
			// NOTE: GetPropery() and XxxxValue() methods have another version 
			// that can be specified fallback value.
			Console.WriteLine("j1 = {0}", j1);
			Console.WriteLine("foo = {0}", j1.GetProperty("foo").NumberValue());
			Console.WriteLine("bar = {0}", j1.GetProperty("bar").BooleanValue());
			Console.WriteLine("baz = {0}", j1.GetProperty("baz").StringValue());
			Console.WriteLine("baa exists? = {0}", j1.HasProperty("baa"));

			// 2. Build JSON
			// JsonObject.Builder() returns new builder instance.
			IJsonObject j2 = JsonObject
				.Builder()
				.Append("foo", "hello")
				.Append("bar", "hello", "bonjour", "こんにちは")
				.Append("baz", (b) => {
					// Lambda's argument is new builder 
					// instance for nested object.
					b.Append("bazProp1", 1);
					b.Append("bazProp2", 2);
				})
				.Build();
			// The builder can be initialized with existed JSON.
			IJsonObject j3 = JsonObject
				.Builder(j2).Append("baa", 123).Build();
			Console.WriteLine("j2 = {0}", j2);
			Console.WriteLine("j3 = {0}", j3);

			// 3. Format JSON
			IJsonFormatOptions opts = JsonFormatOptions
				.Builder()
				.Indent(true).SoftTabs(true).TabWidth(2)
				.Build();
			Console.WriteLine("j3' = {0}", j3.Format(opts));
		}
	}
}
