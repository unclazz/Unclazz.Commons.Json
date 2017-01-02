using NUnit.Framework;
using System;
using Unclazz.Commons.Json.Parser;
using Unclazz.Commons.Json;
using System.Linq;

namespace Test.Unclazz.Commons.Json
{
	[TestFixture()]
	public class NullJsonObjectTest
	{
		
		[Test()]
		public void ToString_Always_ReturnsNull()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

			// Act
			string r = json.ToString();

			// Assert
			Assert.That(r, Is.EqualTo("null"));
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeObject_Always_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

			// Act
			var r = json.TypeIs(JsonObjectType.Object);

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeNull_Always_ReturnsTrue()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

			// Act
			var r = json.TypeIs(JsonObjectType.Null);

			// Assert
			Assert.That(r, Is.True);
		}

		[Test()]
		public void AsArray_Always_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() =>
			{
				json.AsArray();
			});
		}

		[Test()]
		public void AsArray1_Always_ReturnsFallback()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

			// Act
			// Assert
			Assert.That(json.AsArray(JsonObject.Of("foo", "bar").AsArray()).Count, Is.EqualTo(2));
		}

		[Test()]
		public void AsString_Always_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() =>
			{
				json.AsString();
			});
		}

		[Test()]
		public void AsString1_Always_ReturnsFallback()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

			// Act
			// Assert
			Assert.That(json.AsString("foo"), Is.EqualTo("foo"));
		}

		[Test()]
		public void AsBoolean_Always_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() =>
			{
				json.AsBoolean();
			});
		}

		[Test()]
		public void AsBoolean1_Always_ReturnsFallback()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

			// Act
			// Assert
			Assert.That(json.AsBoolean(true), Is.True);
		}

		[Test()]
		public void Properties_Always_ReturnsEmptyEnumeralbe()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

			// Act
			// Assert
			Assert.That(json.Properties.Count(), Is.EqualTo(0));
		}

		[Test()]
		public void HasProperty_Always_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

			// Act
			// Assert
			Assert.That(json.HasProperty("foo"), Is.False);
		}

		[Test()]
		public void GetProperty_Always_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.OfNull();

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
			IJsonObject json0 = JsonObject.OfNull();

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() => json0.AsNumber());
		}

		[Test()]
		public void AsNumber_ReturnsFallbackValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.OfNull();

			// Act
			// Assert
			Assert.That(json0.AsNumber(1), Is.EqualTo(1));
		}

		[Test()]
		public void Equals_ComparesBasedOnValueWrappedByJsonObject()
		{
			Assert.That(JsonObject.OfNull().Equals(JsonObject.OfNull()), Is.True);
		}
	}
}

