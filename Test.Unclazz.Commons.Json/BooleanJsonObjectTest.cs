﻿using NUnit.Framework;
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
		public void IsObjectExactly_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(true);

			// Act
			var r = json.IsObjectExactly();

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void IsNull_ReturnsFalse()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(true);

			// Act
			var r = json.IsNull();

			// Assert
			Assert.That(r, Is.False);
		}

		[Test()]
		public void ArrayValue_ThrowsException()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(true);

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() =>
			{
				json.ArrayValue();
			});
		}

		[Test()]
		public void ArrayValue1_ReturnsFallback()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(true);

			// Act
			// Assert
			Assert.That(json.ArrayValue(JsonObject.Of("foo", "bar").ArrayValue()).Count, Is.EqualTo(2));
		}

		[Test()]
		public void StringValue_ThrowsException()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(true);
			IJsonObject json1 = JsonObject.Of(true);

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() =>
			{
				json0.StringValue();
			});
			Assert.Throws<ApplicationException>(() =>
			{
				json1.StringValue();
			});
		}

		[Test()]
		public void StringValue1_ReturnsFallbackValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(true);
			IJsonObject json1 = JsonObject.Of(false);

			// Act
			// Assert
			Assert.That(json0.StringValue("foo"), Is.EqualTo("foo"));
			Assert.That(json1.StringValue("bar"), Is.EqualTo("bar"));
		}

		[Test()]
		public void BooleanValue_ReturnsValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(true);
			IJsonObject json1 = JsonObject.Of(false);

			// Act
			// Assert
			Assert.That(json0.BooleanValue(), Is.True);
			Assert.That(json1.BooleanValue(), Is.False);
		}

		[Test()]
		public void BooleanValue1_IgnoresFallbackValue()
		{
			// Arrange
			IJsonObject json = JsonObject.Of(false);

			// Act
			// Assert
			Assert.That(json.BooleanValue(true), Is.False);
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
		public void NumberValue_ThrowsException()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(true);
			IJsonObject json1 = JsonObject.Of(false);

			// Act
			// Assert
			Assert.Throws<ApplicationException>(() => json0.NumberValue());
			Assert.Throws<ApplicationException>(() => json1.NumberValue());
		}

		[Test()]
		public void NumberValue_ReturnsFallbackValue()
		{
			// Arrange
			IJsonObject json0 = JsonObject.Of(false);
			IJsonObject json1 = JsonObject.Of(true);

			// Act
			// Assert
			Assert.That(json0.NumberValue(1), Is.EqualTo(1));
			Assert.That(json1.NumberValue(2), Is.EqualTo(2));
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

