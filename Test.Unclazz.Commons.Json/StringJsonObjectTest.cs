using NUnit.Framework;
using System;
using Unclazz.Commons.Json.Parser;
using Unclazz.Commons.Json;
using System.Linq;

namespace Test.Unclazz.Commons.Json
{
	[TestFixture()]
	public class StringJsonObjectTest
	{
		
		[Test()]
		public void ToString_ReturnsStringLiteral()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of("");
			IJsonObject json1 = JsonObject.Of("abc");

			// Act
			string r0 = json0.ToString();
			string r1 = json1.ToString();

			// Assert
			Assert.That(r0, Is.EqualTo("\"\""));
			Assert.That(r1, Is.EqualTo("\"abc\""));
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeObject_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of("");

			// Act
			var r = json.TypeIs(JsonObjectType.Object);

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeNull_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of("");

			// Act
			var r = json.TypeIs(JsonObjectType.Null);

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void AsArray_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.Of("");

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
			IJsonObject json = JsonObject.Of("");

			// Act
			// Assert
			Assert.That(json.AsArray(JsonObject.Of("foo", "bar").AsArray()).Count, Is.EqualTo(2));
		}

		[Test()]
		public void AsString_ReturnsValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(string.Empty);
			IJsonObject json1 = JsonObject.Of("abc");

			// Act
			// Assert
			Assert.That(json0.AsString(), Is.EqualTo(string.Empty));
			Assert.That(json1.AsString(), Is.EqualTo("abc"));
		}

		[Test()]
		public void AsString1_IgnoresFallbackValues()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(string.Empty);
			IJsonObject json1 = JsonObject.Of("abc");

			// Act
			// Assert
			Assert.That(json0.AsString("foo"), Is.EqualTo(string.Empty));
			Assert.That(json1.AsString("bar"), Is.EqualTo("abc"));
		}

		[Test()]
		public void AsBoolean_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(string.Empty);

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() =>
			{
				json.AsBoolean();
			});
		}

		[Test()]
		public void AsBoolean1_ReturnsFallback()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(string.Empty);

			// Act
			// Assert
			Assert.That(json.AsBoolean(true), Is.True);
		}

		[Test()]
		public void Properties_ReturnsEmptyEnumeralbe()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(string.Empty);

			// Act
			// Assert
			Assert.That(json.Properties.Count(), Is.EqualTo(0));
		}

		[Test()]
		public void HasProperty_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(string.Empty);

			// Act
			// Assert
			Assert.That(json.HasProperty("foo"), Is.False);
		}

		[Test()]
		public void GetProperty_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(string.Empty);

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() =>
			{
				json.GetProperty("foo");
			});
		}

		[Test()]
		public void AsNumber_ThrowsException()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(string.Empty);
			IJsonObject json1 = JsonObject.Of("abc");

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() => json0.AsNumber());
			Assert.Throws<ApplicationException>(() => json1.AsNumber());
		}

		[Test()]
		public void AsNumber_ReturnsFallbackValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(string.Empty);
			IJsonObject json1 = JsonObject.Of("abc");

			// Act
			// Assert
			Assert.That(json0.AsNumber(1), Is.EqualTo(1));
			Assert.That(json1.AsNumber(2), Is.EqualTo(2));
		}

		[Test()]
		public void Equals_ComparesBasedOnValueWrappedByJsonObject()
		{
			Assert.That(JsonObject.Of("abc").Equals(JsonObject.Of("abc")), Is.True);
			Assert.That(JsonObject.Of(string.Empty).Equals(JsonObject.Of("")), Is.True);
			Assert.That(JsonObject.Of("abc").Equals(JsonObject.Of("abcd")), Is.False);
		}
	}
}

