﻿using NUnit.Framework;
using System;
using Unclazz.Commons.Json.Parser;
using Unclazz.Commons.Json;

namespace Test.Unclazz.Commons.Json
{
	[TestFixture()]
	public class JsonObjectTest
	{
		[Test()]
		public void FromString_WhenApplyToValidJsonLiteral_ReturnsSuccessfuly()
		{
			// Arrange 
			// Act
			IJsonObject r0 = JsonObject.FromString("{}");
			IJsonObject r1 = JsonObject.FromString("[]");
			IJsonObject r2 = JsonObject.FromString("true");
			IJsonObject r3 = JsonObject.FromString("null");
			IJsonObject r4 = JsonObject.FromString("''");
			IJsonObject r5 = JsonObject.FromString("\"\"");
			IJsonObject r6 = JsonObject.FromString("0.0");

			// Assert
			Assert.That(r0.TypeIs(JsonObjectType.Object), Is.True);
			Assert.That(r1.TypeIs(JsonObjectType.Array), Is.True);
			Assert.That(r2.TypeIs(JsonObjectType.Boolean), Is.True);
			Assert.That(r3.TypeIs(JsonObjectType.Null), Is.True);
			Assert.That(r4.TypeIs(JsonObjectType.String), Is.True);
			Assert.That(r5.TypeIs(JsonObjectType.String), Is.True);
			Assert.That(r6.TypeIs(JsonObjectType.Number), Is.True);
		}
		[Test()]
		public void Format_ReturnsFormattedLiteral()
		{
			// Arrange
			IJsonObject j0 = JsonObject.FromString("{foo: 'fooval', bar :[], baz: { baz2: [0,1,2, {}] }}");
			IJsonObject j1 = JsonObject.Builder().Build();
			IJsonObject j2 = JsonObject.OfNull();
			IJsonFormatOptions opts = JsonFormatOptions.Builder().Indent(true).NewLine("\n").Build();

			// Act
			string s0 = j0.Format(opts);
			string s1 = j1.Format(opts);
			string s2 = j2.Format(opts);

			// Assert
			Assert.AreEqual("{\n" +
			                "\t\"foo\" : \"fooval\",\n" +
			                "\t\"bar\" : [],\n" +
			                "\t\"baz\" : {\n" +
							"\t\t\"baz2\" : [\n" +
			                "\t\t\t0,\n" +
			                "\t\t\t1,\n" +
			                "\t\t\t2,\n" +
			                "\t\t\t{}\n" +
							"\t\t]\n" +
							"\t}\n" +
			                "}", s0);
			Assert.AreEqual("{}", s1);
			Assert.AreEqual("null", s2);
		}

		[Test()]
		public void Equals_ComparesBasedOnValueWrappedByJsonObject()
		{
			Assert.True(JsonObject.FromString("{foo:123,bar:456}")
			            .Equals(JsonObject.FromString("{foo:123,bar:456}")));
			Assert.True(JsonObject.FromString("{foo:123,bar:456}")
			            .Equals(JsonObject.FromString("{bar:456,foo:123}")));
			Assert.False(JsonObject.FromString("{foo:123,bar:456}")
			            .Equals(JsonObject.FromString("{foo:456,bar:123}")));
		}
	}
}

