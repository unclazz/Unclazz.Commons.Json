using NUnit.Framework;
using System;
using Unclazz.Commons.Json.Parser;
using Unclazz.Commons.Json;
using System.Linq;

namespace Test.Unclazz.Commons.Json
{
	[TestFixture()]
	public class NumberJsonObjectTest
	{
		
		[Test()]
		public void ToString_ReturnsNumberLiteral()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(0.5);
			IJsonObject json1 = JsonObject.Of(1.0);

			// Act
			string r0 = json0.ToString();
			string r1 = json1.ToString();

			// Assert
			Assert.That(r0, Is.EqualTo("0.5"));
			Assert.That(r1, Is.EqualTo("1"));
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeObject_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(0);

			// Act
			var r = json.TypeIs(JsonObjectType.Object);

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeNull_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(0);

			// Act
			var r = json.TypeIs(JsonObjectType.Null);

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void AsArray_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(1);

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
			IJsonObject json = JsonObject.Of(1);

			// Act
			// Assert
			Assert.That(json.AsArray(JsonObject.Of("foo", "bar").AsArray()).Count, Is.EqualTo(2));
		}

		[Test()]
		public void AsString_ThrowsException()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(0);
			IJsonObject json1 = JsonObject.Of(1);

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
			IJsonObject json0 = JsonObject.Of(1);
			IJsonObject json1 = JsonObject.Of(0);

			// Act
			// Assert
			Assert.That(json0.AsString("foo"), Is.EqualTo("foo"));
			Assert.That(json1.AsString("bar"), Is.EqualTo("bar"));
		}

		[Test()]
		public void AsNumber_ReturnsValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(0.5);
			IJsonObject json1 = JsonObject.Of(-1.0);

			// Act
			// Assert
			Assert.That(json0.AsNumber(), Is.EqualTo(0.5));
			Assert.That(json1.AsNumber(), Is.EqualTo(-1));
		}

		[Test()]
		public void AsNumber_IgnoresFallbackValues()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(0.5);
			IJsonObject json1 = JsonObject.Of(-1.0);

			// Act
			// Assert
			Assert.That(json0.AsNumber(1), Is.EqualTo(0.5));
			Assert.That(json1.AsNumber(2), Is.EqualTo(-1));
		}

		[Test()]
		public void AsBoolean_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(1);

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
			IJsonObject json = JsonObject.Of(0);

			// Act
			// Assert
			Assert.That(json.AsBoolean(true), Is.True);
		}

		[Test()]
		public void Properties_ReturnsEmptyEnumeralbe()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(1);

			// Act
			// Assert
			Assert.That(json.Properties.Count(), Is.EqualTo(0));
		}

		[Test()]
		public void HasProperty_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(1);

			// Act
			// Assert
			Assert.That(json.HasProperty("foo"), Is.False);
		}

		[Test()]
		public void GetProperty_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(1);

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() =>
			{
				json.GetProperty("foo");
			});
		}

		[Test()]
		public void Equals_ComparesBasedOnValueWrappedByJsonObject()
		{
			Assert.That(JsonObject.Of(1).Equals(JsonObject.Of(1.0)), Is.True);
			Assert.That(JsonObject.Of(0).Equals(JsonObject.Of(0.0)), Is.True);
			Assert.That(JsonObject.Of(0).Equals(JsonObject.Of(0.1)), Is.False);
		}
	}
}

