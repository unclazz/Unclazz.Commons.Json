using NUnit.Framework;
using System;
using Unclazz.Commons.Json;
using System.Linq;

namespace Test.Unclazz.Commons.Json
{
	[TestFixture()]
	public class ObjectJsonObjectTest
	{
		
		[Test()]
		public void ToString_ReturnsStringLiteral()
		{
			// Arrange
			IJsonObject json0 = JsonObject.FromString("{}");
			IJsonObject json1 = JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}");

			// Act
			string r0 = json0.ToString();
			string r1 = json1.ToString();

			// Assert
			Assert.That(r0, Is.EqualTo("{}"));
			Assert.That(r1, Is.EqualTo("{\"a\":\"abc\",\"b\":123,\"c\":true,\"d\":null,\"e\":[],\"f\":{}}"));
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeObject_ReturnsTrue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.FromString("{}");
			IJsonObject json1 = JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}");

			// Act
			var r0 = json0.TypeIs(JsonObjectType.Object);
			var r1 = json1.TypeIs(JsonObjectType.Object);

			// Assert
			Assert.That(r0, Is.True);
			Assert.That(r1, Is.True);
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeNull_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.FromString("{}");

			// Act
			var r = json.TypeIs(JsonObjectType.Null);

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void AsArray_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.FromString("{}");

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() =>
			{
				json.AsArray();
			});
		}

		[Test()]
		public void AsArray1_ReturnsFallback()
		{
			// Arrange
			IJsonObject json = JsonObject.FromString("{}");

			// Act
			// Assert
			Assert.That(json.AsArray(JsonObject.Of("foo", "bar").AsArray()).Count, Is.EqualTo(2));
		}

		[Test()]
		public void AsString_ThrowsException()
		{
			// Arrange
			IJsonObject json0 = JsonObject.FromString("{}");
			IJsonObject json1 = JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}");

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() => json0.AsString());
			Assert.Throws<ApplicationException>(() => json1.AsString());
		}

		[Test()]
		public void AsString1_ReturnsFallbackValues()
		{
			// Arrange
			IJsonObject json0 = JsonObject.FromString("{}");
			IJsonObject json1 = JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}");

			// Act
			// Assert
			Assert.That(json0.AsString("foo"), Is.EqualTo("foo"));
			Assert.That(json1.AsString("bar"), Is.EqualTo("bar"));
		}

		[Test()]
		public void AsBoolean_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.FromString("{}");

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() => json.AsBoolean());
		}

		[Test()]
		public void AsBoolean1_ReturnsFallback()
		{
			// Arrange
			IJsonObject json = JsonObject.FromString("{}");

			// Act
			// Assert
			Assert.That(json.AsBoolean(true), Is.True);
		}

		[Test()]
		public void Properties_ReturnsEnumeralbeOfProperties()
		{
			// Arrange
			IJsonObject json0 = JsonObject.FromString("{}");
			IJsonObject json1 = JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}");

			// Act
			// Assert
			Assert.That(json0.Properties.Count(), Is.EqualTo(0));
			Assert.That(json1.Properties.Count(), Is.EqualTo(6));
		}

		[Test()]
		public void HasProperty_ReturnsTestResult()
		{
			// Arrange
			IJsonObject json0 = JsonObject.FromString("{}");
			IJsonObject json1 = JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}");

			// Act
			// Assert
			Assert.That(json0.HasProperty("a"), Is.False);
			Assert.That(json1.HasProperty("a"), Is.True);
			Assert.That(json1.HasProperty("b"), Is.True);
			Assert.That(json1.HasProperty("c"), Is.True);
			Assert.That(json1.HasProperty("d"), Is.True);
			Assert.That(json1.HasProperty("e"), Is.True);
			Assert.That(json1.HasProperty("f"), Is.True);
			Assert.That(json1.HasProperty("g"), Is.False);
		}

		[Test()]
		public void GetProperty_WhenPropertyDoesNotExist_ThrowsException()
		{
			// Arrange
			IJsonObject json0 = JsonObject.FromString("{}");
			IJsonObject json1 = JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}");

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() => json0.GetProperty("foo"));
			Assert.DoesNotThrow(() => json1.GetProperty("a"));
			Assert.DoesNotThrow(() => json1.GetProperty("b"));
			Assert.DoesNotThrow(() => json1.GetProperty("c"));
			Assert.DoesNotThrow(() => json1.GetProperty("d"));
			Assert.DoesNotThrow(() => json1.GetProperty("e"));
			Assert.DoesNotThrow(() => json1.GetProperty("f"));
			Assert.Throws<ApplicationException>(() => json1.GetProperty("g"));

		}

		[Test()]
		public void GetProperty_WhenPropertyExists_ReturnsTheProperty()
		{
			// Arrange
			IJsonObject json1 = JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}");

			// Act
			// Assert
			Assert.That(json1.GetProperty("a").AsString(), Is.EqualTo("abc"));
			Assert.That(json1.GetProperty("b").AsNumber(), Is.EqualTo(123));
			Assert.That(json1.GetProperty("c").AsBoolean(), Is.EqualTo(true));
			Assert.That(json1.GetProperty("d").TypeIs(JsonObjectType.Null), Is.True);
			Assert.That(json1.GetProperty("e").AsArray().Count, Is.EqualTo(0));
			Assert.That(json1.GetProperty("f").TypeIs(JsonObjectType.Object), Is.True);

		}

		[Test()]
		public void AsNumber_ThrowsException()
		{
			// Arrange
			IJsonObject json0 = JsonObject.FromString("{}");
			IJsonObject json1 = JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}");

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() => json0.AsNumber());
			Assert.Throws<ApplicationException>(() => json1.AsNumber());
		}

		[Test()]
		public void AsNumber_ReturnsFallbackValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.FromString("{}");
			IJsonObject json1 = JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}");

			// Act
			// Assert
			Assert.That(json0.AsNumber(1), Is.EqualTo(1));
			Assert.That(json1.AsNumber(2), Is.EqualTo(2));
		}

		[Test()]
		public void Equals_ComparesBasedOnValueWrappedByJsonObject()
		{
			Assert.AreEqual(JsonObject.FromString("{}"),
							JsonObject.FromString("{}"));
			Assert.AreEqual(JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}"),
					JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}"));
			Assert.AreEqual(JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}"),
					JsonObject.FromString("{a:'abc',c:true,b:123,d:null,e:[],f:{}}"));
			Assert.AreNotEqual(JsonObject.FromString("{a:'abc',b:123,c:true,d:null,e:[],f:{}}"),
					JsonObject.FromString("{a:'abc',c:true,d:null,e:[],f:{}}"));
		}
	}
}

