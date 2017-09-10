using System;
namespace Unclazz.Commons.Json.Parser
{

	/// <summary>
	/// パース処理中に発生したエラーを表す例外オブジェクトです。
	/// </summary>
	public sealed class ParseException : Exception
	{
        /// <summary>
		/// パース処理の入力オブジェクト
		/// </summary>
        public Input Input { get; private set; }
        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="input">例外発生時の<see cref="Input"/>インスタンス.</param>
		public ParseException(Input input) : base(MakeMessage(input, null))
		{
			Input = input;
		}
        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="input">例外発生時の<see cref="Input"/>インスタンス.</param>
        /// <param name="message">メッセージ.</param>
		public ParseException(Input input, string message) : base(MakeMessage(input, message))
		{
			Input = input;
            message = MakeMessage(input, message);
		}
        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="input">例外発生時の<see cref="Input"/>インスタンス.</param>
        /// <param name="message">メッセージ.</param>
        /// <param name="cause">原因となった例外.</param>
		public ParseException(Input input, string message, Exception cause) : base(MakeMessage(input, message), cause)
		{
            Input = input;
		}
		static string MakeMessage(Input input, string message)
		{
			return string.Format("at line {0}, column {1}. {2}",
				input.LineNumber, input.ColumnNumber, message ?? "unexpected error has occurred.");
		}
	}
}

