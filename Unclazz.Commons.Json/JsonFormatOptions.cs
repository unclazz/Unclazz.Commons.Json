using System;
namespace Unclazz.Commons.Json
{
	/// <summary>
	/// <see cref="IJsonFormatOptions"/>の実装クラスです.
	/// </summary>
	public sealed class JsonFormatOptions : IJsonFormatOptions
	{
		/// <summary>
		/// ビルダーのインスタンスを返します.
		/// </summary>
		public static JsonFormatOptionsBuilder Builder()
		{
			return new JsonFormatOptionsBuilder();
		}

        /// <summary>
        /// インデントを行うかどうかを示します.
        /// </summary>
        /// <value>インデントを行う場合<c>true</c>.</value>
        public bool Indent { get; private set; }
        /// <summary>
        /// 改行に使用する文字列を示します.
        /// </summary>
        /// <value>文字列.</value>
        public string NewLine { get; private set; }
        /// <summary>
        /// ソフトタブを使用するかどうかを示します.
        /// </summary>
        /// <value>ソフトタブを使用する場合<c>true</c>.</value>
        public bool SoftTabs { get; private set; }
        /// <summary>
        /// タブ幅を示します.
        /// </summary>
        /// <value>タブ幅.</value>
        public int TabWidth { get; private set; }
		internal JsonFormatOptions(bool i, string n, bool s, int t)
		{
			if (t <= 0)
			{
				throw new ArgumentException("tab width must be greater than 0.");
			}
			if (n == null)
			{
				throw new ArgumentException("{0} must not be null.");
			}
			Indent = i;
			NewLine = n;
			SoftTabs = s;
			TabWidth = t;
		}
	}

	/// <summary>
	/// <see cref="IJsonFormatOptions"/>のインスタンスを構築するためのビルダーです.
	/// </summary>
	public sealed class JsonFormatOptionsBuilder
	{
		internal JsonFormatOptionsBuilder()
		{
		}
		bool indent = false;
		string newLine = System.Environment.NewLine;
		bool softTabs = false;
		int tabWidth = 4;
		/// <summary>
		/// インデントを行うかどうかを指定します.
		/// </summary>
		/// <param name="i"><c>true</c>を指定した場合 インデントを行う.</param>
		public JsonFormatOptionsBuilder Indent(bool i)
		{
			indent = i;
			return this;
		}
		/// <summary>
		/// 改行文字として使用する文字列を指定します.
		/// </summary>
		/// <returns>ビルダー.</returns>
		/// <param name="n">改行文字.</param>
		public JsonFormatOptionsBuilder NewLine(string n)
		{
			newLine = n;
			return this;
		}
		/// <summary>
		/// インデントを行うときソフトタブを使用するかどうかを指定します.
		/// </summary>
		/// <returns>ビルダー.</returns>
		/// <param name="s"><c>true</c>を指定した場合 ソフトタブを使用する.</param>
		public JsonFormatOptionsBuilder SoftTabs(bool s)
		{
			softTabs = s;
			return this;
		}
		/// <summary>
		/// ソフトタブを使用するときのタブ幅を指定します.
		/// </summary>
		/// <returns>ビルダー.</returns>
		/// <param name="t">タブ幅.</param>
		public JsonFormatOptionsBuilder TabWidth(int t)
		{
			tabWidth = t;
			return this;
		}
		/// <summary>
		/// <see cref="IJsonFormatOptions"/>のインスタンスを構築します.
		/// </summary>
		public IJsonFormatOptions Build()
		{
			return new JsonFormatOptions(indent, newLine, softTabs, tabWidth);
		}
	}
}

