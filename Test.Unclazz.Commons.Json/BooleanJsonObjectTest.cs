using NUnit.Framework;
using System;
using Unclazz.Commons.Json.Parser;
using Unclazz.Commons.Json;
using System.Linq;

namespace Test.Unclazz.Commons.Json
{
	[TestFixture()]
	public class BooleanJsonObjectTest
	{
		
		[Test()]
		public void ToString_ReturnsLiteral()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(true);
			IJsonObject json1 = JsonObject.Of(false);

			// Act
			string r0 = json0.ToString();
			string r1 = json1.ToString();

			// Assert
			Assert.That(r0, Is.EqualTo("true"));
			Assert.That(r1, Is.EqualTo("false"));
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeObject_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(true);

			// Act
			var r = json.TypeIs(JsonObjectType.Object);

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeNull_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(true);

			// Act
			var r = json.TypeIs(JsonObjectType.Null);

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void AsArray_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(true);

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
			IJsonObject json = JsonObject.Of(true);

			// Act
			// Assert
			Assert.That(json.AsArray(JsonObject.Of("foo", "bar").AsArray()).Count, Is.EqualTo(2));
		}

		[Test()]
		public void AsString_ThrowsException()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(true);
			IJsonObject json1 = JsonObject.Of(true);

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() =>
			{
				json0.AsString();
			});
			Assert.Throws<ApplicationException>(() =>
			{
				json1.AsString();
			});
		}

		[Test()]
		public void AsString1_ReturnsFallbackValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(true);
			IJsonObject json1 = JsonObject.Of(false);

			// Act
			// Assert
			Assert.That(json0.AsString("foo"), Is.EqualTo("foo"));
			Assert.That(json1.AsString("bar"), Is.EqualTo("bar"));
		}

		[Test()]
		public void AsBoolean_ReturnsValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(true);
			IJsonObject json1 = JsonObject.Of(false);

			// Act
			// Assert
			Assert.That(json0.AsBoolean(), Is.True);
			Assert.That(json1.AsBoolean(), Is.False);
		}

		[Test()]
		public void AsBoolean1_IgnoresFallbackValue()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(false);

			// Act
			// Assert
			Assert.That(json.AsBoolean(true), Is.False);
		}

		[Test()]
		public void Properties_ReturnsEmptyEnumeralbe()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(true);

			// Act
			// Assert
			Assert.That(json.Properties.Count(), Is.EqualTo(0));
		}

		[Test()]
		public void HasProperty_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(true);

			// Act
			// Assert
			Assert.That(json.HasProperty("foo"), Is.False);
		}

		[Test()]
		public void GetProperty_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(true);

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
			IJsonObject json0 = JsonObject.Of(true);
			IJsonObject json1 = JsonObject.Of(false);

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() => json0.AsNumber());
			Assert.Throws<ApplicationException>(() => json1.AsNumber());
		}

		[Test()]
		public void AsNumber_ReturnsFallbackValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(false);
			IJsonObject json1 = JsonObject.Of(true);

			// Act
			// Assert
			Assert.That(json0.AsNumber(1), Is.EqualTo(1));
			Assert.That(json1.AsNumber(2), Is.EqualTo(2));
		}

		[Test()]
		public void Equals_ComparesBasedOnValueWrappedByJsonObject()
		{
			Assert.That(JsonObject.Of(true).Equals(JsonObject.Of(true)), Is.True);
			Assert.That(JsonObject.Of(false).Equals(JsonObject.Of(false)), Is.True);
			Assert.That(JsonObject.Of(true).Equals(JsonObject.Of(false)), Is.False);
		}
	}
}

