using NUnit.Framework;
using System.Linq;
using Unclazz.Commons.Json.Parser;
using Unclazz.Commons.Json;

namespace Test.Unclazz.Commons.Json
{
	[TestFixture]
	public class ParserTest
	{

		[Test]
		public void Parse_WhenApplyToEmptyString_ThrowsException()
		{
			// Arrange
			var p = new JsonParser();

			// Act
			// Assert
			Assert.Throws<ParseException>(() =>
			{
				p.Parse(Input.FromString(""));
			});
			Assert.Throws<ParseException>(() =>
			{
				p.Parse(Input.FromString(" "));
			});
		}

		[Test]
		public void Parse_WhenApplyToBooleanLiteral_ReturnsBooleanJsonObject()
		{
			// Arrange
			var p = new JsonParser();

			// Act
			var r0 = p.Parse(Input.FromString("true"));
			var r1 = p.Parse(Input.FromString(" true "));
			var r2 = p.Parse(Input.FromString("false"));
			var r3 = p.Parse(Input.FromString(" false "));

			// Assert
			Assert.That(r0.AsBoolean(), Is.True);
			Assert.That(r1.AsBoolean(), Is.True);
			Assert.That(r2.AsBoolean(), Is.False);
			Assert.That(r3.AsBoolean(), Is.False);
		}

		[Test]
		public void Parse_WhenApplyToNullLiteral_ReturnsNullJsonObject()
		{
			// Arrange
			var p = new JsonParser();

			// Act
			var r0 = p.Parse(Input.FromString("null"));
			var r1 = p.Parse(Input.FromString(" null "));

			// Assert
			Assert.That(r0.TypeIs(JsonObjectType.Null), Is.True);
			Assert.That(r1.TypeIs(JsonObjectType.Null), Is.True);
		}

		[Test]
		public void Parse_WhenApplyToNumberLiteral_ReturnsNumberJsonObject()
		{
			// Arrange
			var p = new JsonParser();

			// Act
			var r0 = p.Parse(Input.FromString("0.0"));
			var r1 = p.Parse(Input.FromString(" 0.0 "));
			var r2 = p.Parse(Input.FromString(" -0.1 "));
			var r3 = p.Parse(Input.FromString(" -0.1e+1 "));

			// Assert
			Assert.That(r0.AsNumber(), Is.EqualTo(0.0));
			Assert.That(r1.AsNumber(), Is.EqualTo(0.0));
			Assert.That(r2.AsNumber(), Is.EqualTo(-0.1));
			Assert.That(r3.AsNumber(), Is.EqualTo(-0.1e+1));
		}

		[Test]
		public void Parse_WhenApplyToArrayLiteral_ReturnsArrayJsonObject()
		{
			// Arrange
			var p = new JsonParser();

			// Act
			IJsonObject r0 = p.Parse(Input.FromString("[0.0, true]"));
			IJsonObject r1 = p.Parse(Input.FromString(" [0.0 ,true] "));
			IJsonObject r2 = p.Parse(Input.FromString("[ null,'foo' ]"));
			IJsonObject r3 = p.Parse(Input.FromString(" [{} , \"foo\"] "));

			// Assert
			Assert.That(r0.AsArray().Count, Is.EqualTo(2));
			Assert.That(r0.AsArray()[0].AsNumber(), Is.EqualTo(0.0));
			Assert.That(r0.AsArray()[1].AsBoolean(), Is.True);
			Assert.That(r1.AsArray().Count, Is.EqualTo(2));
			Assert.That(r2.AsArray().Count, Is.EqualTo(2));
			Assert.That(r3.AsArray().Count, Is.EqualTo(2));
			Assert.That(r3.AsArray()[0].TypeIs(JsonObjectType.Object), Is.True);
			Assert.That(r3.AsArray()[1].AsString(), Is.EqualTo("foo"));
		}

		[Test]
		public void Parse_WhenApplyToObjectLiteral_ReturnsObjectJsonObject()
		{
			// Arrange
			var p = new JsonParser();

			// Act
			IJsonObject r0 = p.Parse(Input.FromString("{}"));
			IJsonObject r1 = p.Parse(Input.FromString(" {} "));
			IJsonObject r2 = p.Parse(Input.FromString("{ foo:\"bar\" }"));
			IJsonObject r3 = p.Parse(Input.FromString("{'foo' : \"bar\", baz: true}"));
			IJsonObject r4 = p.Parse(Input.FromString(" {\"foo\" : 'bar' ,baz:true} "));
			IJsonObject r5 = p.Parse(Input.FromString(" {\"foo\": {\"bar\": {\"baz\": null}}} "));

			// Assert
			Assert.That(r0.Properties.Count(), Is.EqualTo(0));
			Assert.That(r1.Properties.Count(), Is.EqualTo(0));
			Assert.That(r2.Properties.Count(), Is.EqualTo(1));
			Assert.That(r3.Properties.Count(), Is.EqualTo(2));
			Assert.That(r4.Properties.Count(), Is.EqualTo(2));
			Assert.That(r4.GetProperty("foo").AsString(), Is.EqualTo("bar"));
			Assert.That(r4.GetProperty("baz").AsBoolean(), Is.True);
			Assert.That(r5.Properties.Count(), Is.EqualTo(1));
			Assert.That(r5.GetProperty("foo")
			            .GetProperty("bar")
			            .GetProperty("baz")
			            .TypeIs(JsonObjectType.Null), Is.True);
		}
	}
}