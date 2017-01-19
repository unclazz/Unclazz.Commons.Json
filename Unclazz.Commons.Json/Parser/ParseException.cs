using System;
namespace Unclazz.Commons.Json.Parser
{

	/// <summary>
	/// パース処理中に発生したエラーを表す例外オブジェクトです。
	/// </summary>
	public sealed class ParseException : Exception
	{
        private readonly string message;
        /// <summary>
		/// パース処理の入力オブジェクト
		/// </summary>
        public Input Input { get; private set; }
		/// <summary>
		/// 例外メッセージ
		/// </summary>
        public override string Message
        {
            get
            {
                return message;
            }
        }
		private readonly Exception cause;

		public ParseException(Input input)
		{
			Input = input;
            message = MakeMessage(input, "error has occurred.");
		}
		public ParseException(Input input, string message)
		{
			Input = input;
            message = MakeMessage(input, message);
		}
		public ParseException(Input input, string message, Exception cause)
		{
			Input = input;
            message = MakeMessage(input, message);
			this.cause = cause;
		}
		public override Exception GetBaseException()
		{
			return cause;
		}
		private string MakeMessage(Input input, string message)
		{
			return string.Format("at line {0}, column {1}. {2}",
				input.LineNumber, input.ColumnNumber, message);
		}
	}
}

