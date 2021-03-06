﻿using System.Collections.Generic;

namespace Unclazz.Commons.Json
{
	/// <summary>
	/// JSONを表わすインターフェースです.
	/// <para>インターフェースの提供するプロパティとメソッドを通じてJSONのノードが表わす値にアクセスできます.</para>
	/// <para>このインターフェースの実装はイミュータブルであることが強く推奨されます.
	/// <see cref="JsonObjectBuilder"/>を始めとして、このライブラリが提供するAPIは一般に
	/// <code>IJsonObject</code>インターフェースの実装がイミュータブルであることを想定して実装されています.</para>
	/// </summary>
	public interface IJsonObject
	{
		/// <summary>
		/// このJSONノードのデータ型を示す列挙型にアクセスします.
		/// </summary>
		/// <value>データ型を示す列挙型にアクセス.</value>
		JsonObjectType Type { get; }
		/// <summary>
		/// このJSONノードの持つプロパティを要素とする<code>IEnumerable&lt;T></code>にアクセスします.
		/// </summary>
		/// <value>プロパティを要素とする<code>IEnumerable&lt;T></code>.</value>
		IEnumerable<IJsonProperty> Properties { get; }
		/// <summary>
		/// このJSONノードの持つプロパティ名を要素とする<code>IEnumerable&lt;T></code>にアクセスします.
		/// </summary>
		/// <value>プロパティ名を要素とする<code>IEnumerable&lt;T></code>.</value>
		IEnumerable<string> PropertyNames { get; }
		/// <summary>
		/// このJSONノードの持つプロパティ値を要素とする<code>IEnumerable&lt;T></code>にアクセスします.
		/// </summary>
		/// <value>プロパティ値を要素とする<code>IEnumerable&lt;T></code>.</value>
		IEnumerable<IJsonObject> PropertyValues { get; }
		/// <summary>
		/// このJSONノードが持つプロパティのうち指定された名前のプロパティを返します.
		/// 指定された名前のプロパティが存在しない場合は例外をスローします.
		/// </summary>
		/// <param name="name">プロパティ名.</param>
		IJsonObject this[string name] { get; }

		/// <summary>
		/// このJSONノードが表わす<code>Number</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <exception cref="System.ApplicationException">このノードが別の型を表している場合.</exception>
		double AsNumber();
		/// <summary>
		/// このJSONノードが表わす<code>Number</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <param name="fallback">このノードが別の型を表している場合に返される値.</param>
		double AsNumber(double fallback);
		/// <summary>
		/// このJSONノードが表わす<code>String</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <exception cref="System.ApplicationException">このノードが別の型を表している場合.</exception>
		string AsString();
		/// <summary>
		/// このJSONノードが表わす<code>String</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <param name="fallback">このノードが別の型を表している場合に返される値.</param>
		string AsString(string fallback);
		/// <summary>
		/// このJSONノードが表わす<code>Boolean</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <exception cref="System.ApplicationException">このノードが別の型を表している場合.</exception>
		bool AsBoolean();
		/// <summary>
		/// このJSONノードが表わす<code>Boolean</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <param name="fallback">このノードが別の型を表している場合に返される値.</param>
		bool AsBoolean(bool fallback);
		/// <summary>
		/// このJSONノードが表わす<code>Array</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <exception cref="System.ApplicationException">このノードが別の型を表している場合.</exception>
		IList<IJsonObject> AsArray();
		/// <summary>
		/// このJSONノードが表わす<code>Array</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <param name="fallback">このノードが別の型を表している場合に返される値.</param>
		IList<IJsonObject> AsArray(IList<IJsonObject> fallback);
		/// <summary>
		/// このJSONノードが引数で指定された列挙型インスタンスに対応するものかチェックします.
		/// </summary>
		/// <returns>引数で指定された列挙型インスタンスに対応するものだった場合<c>true</c>.</returns>
		/// <param name="type">Type.</param>
		bool TypeIs(JsonObjectType type);
		/// <summary>
		/// このJSONノードが指定された名前のプロパティを持つかどうかチェックします.
		/// </summary>
		/// <returns>指定された名前のプロパティを持つ場合<c>true</c>.</returns>
		/// <param name="name">Name.</param>
		bool HasProperty(string name);
		/// <summary>
		/// このJSONノードが持つプロパティのうち指定された名前のプロパティを返します.
		/// 指定された名前のプロパティが存在しない場合は例外をスローします.
		/// </summary>
		/// <returns>プロパティ値.</returns>
		/// <param name="name">プロパティ名.</param>
		IJsonObject GetProperty(string name);
		/// <summary>
		/// このJSONノードが持つプロパティのうち指定された名前のプロパティを返します.
		/// 指定された名前のプロパティが存在しない場合は引数で指定されたデフォルト値を返します.
		/// </summary>
		/// <returns>プロパティ値.</returns>
		/// <param name="name">プロパティ名.</param>
		/// <param name="fallback">デフォルト値.</param>
		IJsonObject GetProperty(string name, IJsonObject fallback);
		/// <summary>
		/// このJSONノードのリテラル表現を返します.
		/// </summary>
		/// <returns>リテラル表現.</returns>
		string ToString();
		/// <summary>
		/// このJSONノードの整形されたリテラル表現を返します.
		/// </summary>
		/// <param name="opts">整形オプション.</param>
		string Format(IJsonFormatOptions opts);
	}

	/// <summary>
	/// JSONノードのプロパティを表わすインターフェースです.
	/// </summary>
	public interface IJsonProperty
	{
		/// <summary>
		/// プロパティ名にアクセスします.
		/// </summary>
		/// <value>プロパティ名.</value>
		string Name { get; }
		/// <summary>
		/// プロパティ値いアクセスします.
		/// </summary>
		/// <value>プロパティ値.</value>
		IJsonObject Value { get; }
	}

	/// <summary>
	/// JSONノードのデータ型を表わす列挙型です.
	/// </summary>
	public enum JsonObjectType
	{
        /// <summary>
        /// Null型
        /// </summary>
		Null, 
        /// <summary>
        /// String型
        /// </summary>
        String, 
        /// <summary>
        /// Number型
        /// </summary>
        Number, 
        /// <summary>
        /// Boolean型
        /// </summary>
        Boolean, 
        /// <summary>
        /// Array型
        /// </summary>
        Array, 
        /// <summary>
        /// Object型
        /// </summary>
        Object
	}
}

