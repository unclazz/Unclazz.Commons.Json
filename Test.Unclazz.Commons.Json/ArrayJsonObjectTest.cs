using NUnit.Framework;
using System;
using Unclazz.Commons.Json.Parser;
using Unclazz.Commons.Json;
using System.Linq;

namespace Test.Unclazz.Commons.Json
{
	[TestFixture()]
	public class ArrayJsonObjectTest
	{
		
		[Test()]
		public void ToString_ReturnsStringLiteral()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(new string[0]);
			IJsonObject json1 = JsonObject.Of(1,2,3);

			// Act
			string r0 = json0.ToString();
			string r1 = json1.ToString();

			// Assert
			Assert.That(r0, Is.EqualTo("[]"));
			Assert.That(r1, Is.EqualTo("[1,2,3]"));
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeObject_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(new string[0]);

			// Act
			var r = json.TypeIs(JsonObjectType.Object);

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void TypeIs_WhenAppliedToJsonObjectTypeNull_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(new string[0]);

			// Act
			var r = json.TypeIs(JsonObjectType.Null);

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void AsArray_ReturnsValue()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(2,3,4);

			// Act
			// Assert
			Assert.That(json
			            .AsArray()
			            .Select((arg) => arg.AsNumber())
			            .ToArray(),
			            Is.EqualTo(new long[] { 2, 3, 4 }));
		}

		[Test()]
		public void AsArray1_IgnoresFallback()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(2, 3, 4);

			// Act
			// Assert
			Assert.That(json
						.AsArray()
						.Select((arg) => arg.AsNumber())
						.ToArray(),
						Is.EqualTo(new long[] { 2, 3, 4 }));
		}

		[Test()]
		public void AsString_ThrowsException()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(new bool[0]);
			IJsonObject json1 = JsonObject.Of("abc", "def");

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() => json0.AsString());
			Assert.Throws<ApplicationException>(() => json1.AsString());
		}

		[Test()]
		public void AsString1_ReturnsFallback()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(new bool[0]);
			IJsonObject json1 = JsonObject.Of("abc", "def");

			// Act
			// Assert
			Assert.That(json0.AsString("foo"), Is.EqualTo("foo"));
			Assert.That(json1.AsString("bar"), Is.EqualTo("bar"));
		}

		[Test()]
		public void AsBoolean_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.Of("abc", "def");

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
			IJsonObject json = JsonObject.Of("abc", "def");

			// Act
			// Assert
			Assert.That(json.AsBoolean(true), Is.True);
		}

		[Test()]
		public void Properties_ReturnsEmptyEnumeralbe()
		{
			// Arrange
			IJsonObject json = JsonObject.Of("abc", "def");

			// Act
			// Assert
			Assert.That(json.Properties.Count(), Is.EqualTo(0));
		}

		[Test()]
		public void HasProperty_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of("abc", "def");

			// Act
			// Assert
			Assert.That(json.HasProperty("foo"), Is.False);
		}

		[Test()]
		public void GetProperty_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.Of("abc", "def");

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
			IJsonObject json0 = JsonObject.Of(new bool[0]);
			IJsonObject json1 = JsonObject.Of("abc", "def");

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() => json0.AsNumber());
			Assert.Throws<ApplicationException>(() => json1.AsNumber());
		}

		[Test()]
		public void AsNumber_ReturnsFallbackValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(new bool[0]);
			IJsonObject json1 = JsonObject.Of("abc", "def");

			// Act
			// Assert
			Assert.That(json0.AsNumber(1), Is.EqualTo(1));
			Assert.That(json1.AsNumber(2), Is.EqualTo(2));
		}

		[Test()]
		public void Equals_ComparesBasedOnValueWrappedByJsonObject()
		{
			Assert.That(JsonObject.Of(1, 2, 3).Equals(JsonObject.Of(1.0, 2.0, 3.0)), Is.True);
			Assert.That(JsonObject.Of(new string[0]).Equals(JsonObject.Of(new string[0])), Is.True);
			Assert.That(JsonObject.Of(new string[0]).Equals(JsonObject.Of(new bool[0])), Is.True);
			Assert.That(JsonObject.Of(true, false).Equals(JsonObject.Of(new bool[0])), Is.False);
		}
	}
}

