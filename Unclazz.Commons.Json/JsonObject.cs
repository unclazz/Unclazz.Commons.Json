using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Unclazz.Commons.Json.Parser;

namespace Unclazz.Commons.Json
{
	/// <summary>
	/// <see cref="IJsonObject"/>の抽象実装クラスでありユーティリティ・クラスです.
	/// <para>このクラスはJSONのノードを表わす実装クラスの親クラスとして各種の共通処理を実装しています.
	/// ライブラリの利用者はこのクラスが提供する静的メソッドを通じてJSONのパースや、
	/// JSONノードを表わす実装クラスのインスタンスの生成を行うことができます.</para>
	/// </summary>
	public abstract class JsonObject : IJsonObject
	{
		private static readonly JsonParser parser = new JsonParser();
		private static readonly Regex re = new Regex("\"|\\\\|/|\b|\f|\n|\r|\t");
		private static readonly StringJsonObject emptyString = new StringJsonObject(string.Empty);
		private static readonly IJsonObject trueValue = new BooleanJsonObject(true);
		private static readonly IJsonObject falseValue = new BooleanJsonObject(false);
		private static readonly IJsonObject zero = new NumberJsonObject(0);
		private static readonly IJsonObject emptyArray = new ArrayJsonObject(new List<IJsonObject>());

		internal static string Quotes(string val)
		{
			return new StringBuilder()
				.Append('"')
				.Append(re.Replace(val, ReplaceControls))
				.Append('"').ToString();
		}

		static string ReplaceControls(Match m)
		{
			char ch = m.Value[0];
			switch (ch)
			{
				case '\a': return "\\a";
				case '\b': return "\\b";
				case '\t': return "\\t";
				case '\n': return "\\n";
				case '\v': return "\\v";
				case '\f': return "\\f";
				case '\r': return "\\r";
				default: return "\\" + ch;
			}
		}

		/// <summary>
		/// 文字列からJSONを読み取ります.
		/// </summary>
		/// <returns>読み取り結果.</returns>
		/// <param name="json">文字列.</param>
		public static IJsonObject FromString(string json)
		{
			return parser.Parse(Input.FromString(json));
		}
		/// <summary>
		/// ファイルからJSONを読み取ります.
		/// </summary>
		/// <returns>読み取り結果.</returns>
		/// <param name="path">ファイルのパス.</param>
		/// <param name="enc">ファイルのエンコーディング.</param>
		public static IJsonObject FromFile(string path, Encoding enc)
		{
			return parser.Parse(Input.FromFile(path, enc));
		}
		/// <summary>
		/// ファイルからJSONを読み取ります.
		/// </summary>
		/// <returns>読み取り結果.</returns>
		/// <param name="path">ファイルのパス.</param>
		public static IJsonObject FromFile(string path)
		{
			return parser.Parse(Input.FromFile(path));
		}
        /// <summary>
        /// ストリームからJSONを読み取ります.
        /// </summary>
        /// <returns>読み取り結果.</returns>
        /// <param name="stream">ストリーム.</param>
        /// <param name="enc">エンコーディング.</param>
		public static IJsonObject FromStream(Stream stream, Encoding enc)
		{
			return parser.Parse(Input.FromStream(stream, enc));
		}
		/// <summary>
		/// <code>Object</code>を表わす<see cref="IJsonObject"/>を組み立てるためのビルダーを生成します.
		/// </summary>
		public static JsonObjectBuilder Builder()
		{
			return JsonObjectBuilder.GetInstance();
		}
		/// <summary>
		/// <code>Object</code>型のJSONノードを組み立てるためのビルダーを生成します.
		/// </summary>
		/// <param name="proto">プロパティの初期値を提供する<see cref="IJsonObject"/>.</param>
		public static JsonObjectBuilder Builder(IJsonObject proto)
		{
			return JsonObjectBuilder.GetInstance(proto);
		}
		/// <summary>
		/// <code>String</code>型のJSONノードを生成します.
		/// </summary>
		/// <param name="val">文字列.</param>
		/// <exception cref="ArgumentNullException">引数に<code>null</code>が指定された場合</exception>
		public static IJsonObject Of(string val)
		{
			if (val == null)
			{
                throw new ArgumentNullException(nameof(val));
			}
			return val.Length == 0 ? emptyString : new StringJsonObject(val);
		}
		/// <summary>
		/// <code>Number</code>型のJSONノードを生成します.
		/// </summary>
		/// <param name="val">数値.</param>
		public static IJsonObject Of(double val)
		{
			return val.CompareTo(0) == 0 ? zero : new NumberJsonObject(val);
		}
		/// <summary>
		/// <code>Boolean</code>型のJSONノードを生成します.
		/// </summary>
		/// <param name="val">ブール値.</param>
		public static IJsonObject Of(bool val)
		{
			return val ? trueValue : falseValue;
		}
		/// <summary>
		/// <code>null</code>型のJSONノードを生成します.
		/// </summary>
		/// <returns>JSONノード.</returns>
		public static IJsonObject OfNull()
		{
			return NullJsonObject.Instance;
		}
		/// <summary>
		/// <code>Array</code>型のJSONノードを生成します.
		/// </summary>
		/// <param name="items">配列の要素となるJSONノード.</param>
		public static IJsonObject Of(IEnumerable<IJsonObject> items)
		{
			if (items == null)
			{
                throw new ArgumentNullException(nameof(items));
			}
			IList<IJsonObject> l = items.ToList().AsReadOnly();
			return l.Count == 0 ? emptyArray : new ArrayJsonObject(l);
		}
		/// <summary>
		/// <code>Array</code>型のJSONノードを生成します.
		/// </summary>
		/// <param name="items">配列の要素となるJSONノード.</param>
		public static IJsonObject Of(params IJsonObject[] items)
		{
			if (items == null)
			{
                throw new ArgumentNullException(nameof(items));
			}
			IList<IJsonObject> l = items.ToList().AsReadOnly();
			return l.Count == 0 ? emptyArray : new ArrayJsonObject(l);
		}
		/// <summary>
		/// <code>Array</code>型のJSONノードを生成します.
		/// </summary>
		/// <param name="items">配列の要素となる文字列.</param>
		public static IJsonObject Of(IEnumerable<string> items)
		{
			if (items == null)
			{
                throw new ArgumentNullException(nameof(items));
			}
			return Of(items.Select(Of));
		}
		/// <summary>
		/// <code>Array</code>型のJSONノードを生成します.
		/// </summary>
		/// <param name="items">配列の要素となる文字列.</param>
		public static IJsonObject Of(params string[] items)
		{
			if (items == null)
			{
                throw new ArgumentNullException(nameof(items));
			}
			return Of(items.Select(Of));
		}
		/// <summary>
		/// <code>Array</code>型のJSONノードを生成します.
		/// </summary>
		/// <param name="items">配列の要素となるブール値.</param>
		public static IJsonObject Of(IEnumerable<bool> items)
		{
			if (items == null)
			{
                throw new ArgumentNullException(nameof(items));
			}
			return Of(items.Select(Of));
		}
		/// <summary>
		/// <code>Array</code>型のJSONノードを生成します.
		/// </summary>
		/// <param name="items">配列の要素となるブール値.</param>
		public static IJsonObject Of(params bool[] items)
		{
			if (items == null)
			{
                throw new ArgumentNullException(nameof(items));
			}
			return Of(items.Select(Of));
		}
		/// <summary>
		/// <code>Array</code>型のJSONノードを生成します.
		/// </summary>
		/// <param name="items">配列の要素となる数値.</param>
		public static IJsonObject Of(IEnumerable<double> items)
		{
			if (items == null)
			{
                throw new ArgumentNullException(nameof(items));
			}
			return Of(items.Select(Of));
		}
		/// <summary>
		/// <code>Array</code>型のJSONノードを生成します.
		/// </summary>
		/// <param name="items">配列の要素となる数値.</param>
		public static IJsonObject Of(params double[] items)
		{
			if (items == null)
			{
                throw new ArgumentNullException(nameof(items));
			}
			return Of(items.Select(Of));
		}

		/// <summary>
		/// このJSONノードが表わす<code>Number</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <exception cref="System.ApplicationException">このノードが別の型を表している場合.</exception>
		public virtual double AsNumber()
		{
			throw new ApplicationException("json node does not represent Number value.");
		}
		/// <summary>
		/// このJSONノードが表わす<code>Number</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <param name="fallback">このノードが別の型を表している場合に返される値.</param>
		public virtual double AsNumber(double fallback)
		{
			return fallback;
		}
		/// <summary>
		/// このJSONノードが表わす<code>String</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <exception cref="System.ApplicationException">このノードが別の型を表している場合.</exception>
		public virtual string AsString()
		{
			throw new ApplicationException("json node does not represent String value.");
		}
		/// <summary>
		/// このJSONノードが表わす<code>String</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <param name="fallback">このノードが別の型を表している場合に返される値.</param>
		public virtual string AsString(string fallback)
		{
			return fallback;
		}
		/// <summary>
		/// このJSONノードが表わす<code>Boolean</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <exception cref="System.ApplicationException">このノードが別の型を表している場合.</exception>
		public virtual bool AsBoolean()
		{
			throw new ApplicationException("json node does not represent Boolean value.");
		}
		/// <summary>
		/// このJSONノードが表わす<code>Boolean</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <param name="fallback">このノードが別の型を表している場合に返される値.</param>
		public virtual bool AsBoolean(bool fallback)
		{
			return fallback;
		}
		/// <summary>
		/// このJSONノードが表わす<code>Array</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <exception cref="System.ApplicationException">このノードが別の型を表している場合.</exception>
		public virtual IList<IJsonObject> AsArray()
		{
			throw new ApplicationException("json node does not represent Array value.");
		}
		/// <summary>
		/// このJSONノードが表わす<code>Array</code>型の値を返します.
		/// </summary>
		/// <returns>ノードが表わす値.</returns>
		/// <param name="fallback">このノードが別の型を表している場合に返される値.</param>
		public virtual IList<IJsonObject> AsArray(IList<IJsonObject> fallback)
		{
			return fallback;
		}
		/// <summary>
		/// このJSONノードが引数で指定された列挙型インスタンスに対応するものかチェックします.
		/// </summary>
		/// <returns>引数で指定された列挙型インスタンスに対応するものだった場合<c>true</c>.</returns>
		/// <param name="type">Type.</param>
		public bool TypeIs(JsonObjectType type)
		{
			return Type == type;
		}
		/// <summary>
		/// このJSONノードが指定された名前のプロパティを持つかどうかチェックします.
		/// </summary>
		/// <returns>指定された名前のプロパティを持つ場合<c>true</c>.</returns>
		/// <param name="name">Name.</param>
		public bool HasProperty(string name)
		{
			return TypeIs(JsonObjectType.Object) && PropertyNames.Any((arg) => arg.Equals(name));
		}
		/// <summary>
		/// このJSONノードが持つプロパティのうち指定された名前のプロパティを返します.
		/// 指定された名前のプロパティが存在しない場合は例外をスローします.
		/// </summary>
		/// <returns>プロパティ値.</returns>
		/// <param name="name">プロパティ名.</param>
		public IJsonObject GetProperty(string name)
		{
			IJsonProperty r = Properties.FirstOrDefault((arg) => arg.Name.Equals(name));
			if (r == null)
			{
				throw new ApplicationException(
					string.Format("json node has not property \"{0}\".", name));
			}
			return r.Value;
		}
		/// <summary>
		/// このJSONノードが持つプロパティのうち指定された名前のプロパティを返します.
		/// 指定された名前のプロパティが存在しない場合は引数で指定されたデフォルト値を返します.
		/// </summary>
		/// <returns>プロパティ値.</returns>
		/// <param name="name">プロパティ名.</param>
		/// <param name="fallback">デフォルト値.</param>
		public IJsonObject GetProperty(string name, IJsonObject fallback)
		{
			return HasProperty(name) ? GetProperty(name) : fallback;
		}
		/// <summary>
		/// このJSONノードの整形されたリテラル表現を返します.
		/// </summary>
		/// <param name="opts">整形オプション.</param>
		public string Format(IJsonFormatOptions opts)
		{
			if (opts.Indent)
			{
				StringBuilder buff = new StringBuilder();
				FormatHelper(this, opts, buff, 0);
				return buff.ToString().TrimStart();
			}
			else {
				return ToString();
			}
		}
		private static void FormatHelper(IJsonObject json, 
		                                 IJsonFormatOptions opts,
		                                 StringBuilder buff,
		                                 int depth)
		{
			if (json.Type != JsonObjectType.Array && !json.TypeIs(JsonObjectType.Object))
			{
				buff.Append(json.ToString());
				return;
			}

			if (json.Type == JsonObjectType.Array)
			{
				buff.Append('[');
				bool empty = true;
				foreach (IJsonObject item in json.AsArray())
				{
					if (empty)
					{
						empty = false;
					}else{
						buff.Append(',');
					}
					FormatHelperNewLine(opts, buff);
					FormatHelperIndent(opts, buff, depth + 1);
					FormatHelper(item, opts, buff, depth + 1);
				}
				if (!empty)
				{
					FormatHelperNewLine(opts, buff);
					FormatHelperIndent(opts, buff, depth);
				}
				buff.Append(']');
			}
			else if (json.Type == JsonObjectType.Object)
			{
				buff.Append('{');
				bool empty = true;
				foreach (IJsonProperty p in json.Properties)
				{
					if (empty)
					{
						empty = false;
					}
					else {
						buff.Append(',');
					}
					FormatHelperNewLine(opts, buff);
					FormatHelperIndent(opts, buff, depth + 1);
					buff.Append(Quotes(p.Name)).Append(' ').Append(':').Append(' ');
					FormatHelper(p.Value, opts, buff, depth + 1);
				}
				if (!empty)
				{
					FormatHelperNewLine(opts, buff);
					FormatHelperIndent(opts, buff, depth);
				}
				buff.Append('}');
			}
		}
		private static void FormatHelperIndent(IJsonFormatOptions opts, StringBuilder buff, int depth)
		{
			if (!opts.Indent || depth == 0)
			{
				return;
			}
			if (opts.SoftTabs)
			{
				for (int i = 0; i < opts.TabWidth * depth; i++)
				{
					buff.Append(' ');
				}
			}
			else {
				for (int i = 0; i < depth; i++)
				{
					buff.Append('\t');
				}
			}
		}
		private static void FormatHelperNewLine(IJsonFormatOptions opts, StringBuilder buff)
		{
			if (!opts.Indent)
			{
				return;
			}
			buff.Append(opts.NewLine);
		}
		/// <summary>
		/// このJSONノードのデータ型を示す列挙型にアクセスします.
		/// </summary>
		/// <value>データ型を示す列挙型にアクセス.</value>
		public JsonObjectType Type { get; private set; }
		/// <summary>
		/// このJSONノードの持つプロパティを要素とする<code>IEnumerable&lt;T></code>にアクセスします.
		/// </summary>
		/// <value>プロパティを要素とする<code>IEnumerable&lt;T></code>.</value>
		public virtual IEnumerable<IJsonProperty> Properties
		{
			get
			{
				return Enumerable.Empty<IJsonProperty>();
			}
		}
		/// <summary>
		/// このJSONノードの持つプロパティ名を要素とする<code>IEnumerable&lt;T></code>にアクセスします.
		/// </summary>
		/// <value>プロパティ名を要素とする<code>IEnumerable&lt;T></code>.</value>
		public IEnumerable<string> PropertyNames
		{
			get
			{
				return Properties.Select((arg) => arg.Name);
			}
		}
		/// <summary>
		/// このJSONノードの持つプロパティ値を要素とする<code>IEnumerable&lt;T></code>にアクセスします.
		/// </summary>
		/// <value>プロパティ値を要素とする<code>IEnumerable&lt;T></code>.</value>
		public IEnumerable<IJsonObject> PropertyValues
		{
			get
			{
				return Properties.Select((arg) => arg.Value);
			}
		}
		/// <summary>
		/// このJSONノードが持つプロパティのうち指定された名前のプロパティを返します.
		/// 指定された名前のプロパティが存在しない場合は例外をスローします.
		/// </summary>
		/// <param name="name">プロパティ名.</param>
		public IJsonObject this[string name]
		{
			get
			{
                return GetProperty(name);
			}
		}

		internal JsonObject(JsonObjectType type)
		{
			Type = type;
		}
	}
}

