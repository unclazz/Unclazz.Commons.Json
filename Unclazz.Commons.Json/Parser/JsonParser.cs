using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Unclazz.Commons.Json.Parser
{
	/// <summary>
	/// <see cref="Input"/>からJSON形式データを読み取るパーサーです。
	/// このパーサーはシングルクォーテーションで囲われた<c>String</c>型リテラルや
	/// シングルもしくはダブルクオーテーションが欠落したプロパティ名の使用を許容します。
	/// 加えてまたパーサーはJSONを構成するトークンの間の任意の場所における
	/// 行コメントおよびブロックコメントの使用を許容します。
	/// </summary>
	public sealed class JsonParser
	{
		private static readonly char WhiteSpace = ' ';
		private static readonly Regex NumberPattern = 
			new Regex("^-?(0|[1-9][0-9]*)(\\.[0-9]+)?([eE][+-]?[0-9]+)?");
		private static readonly Regex BooleanPattern = new Regex("^(true|false)");

		/// <summary>
		/// JSON形式データを読み取ります。
		/// </summary>
		/// <param name="input">入力データ</param>
		/// <exception cref="ParseException">読み取り中に構文エラーや予期せぬエラーが発生した場合</exception>
		public IJsonObject Parse(Input input)
		{
			using (input)
			{
				input.SkipWhitespace();
				return ParseNode(input);
			}
		}

		IJsonObject ParseNode(Input input)
		{
			char curr = input.Current;
			if (curr == '{')
			{
				return ParseObjectNode(input);
			}
			else if (curr == '[')
			{
				return ParseArrayNode(input);
			}
			else if (curr == '"' || curr == '\'')
			{
				return ParseStringNode(input);
			}
			else if (curr == 't' || curr == 'f')
			{
				return ParseBooleanNode(input);
			}
			else if (curr == 'n')
			{
				return ParseNullNode(input);
			}
			else if (curr == '-' || ('0' <= curr && curr <= '9'))
			{
				return ParseNumberNode(input);
			}
			throw new ParseException(input, "unknown token.");
		}

		string ParseQuotedString(Input input)
		{
			StringBuilder buff = new StringBuilder();
			char quote = input.Current;

			while (!input.EndOfFile)
			{
				char c1 = input.GoNext();
				if (c1 == quote)
				{
					input.GoNext();
					return buff.ToString();
				}
                // エスケープシーケンスの始まりに該当するかチェック
                if (c1 == '\\')
                {
                    // 該当する場合
                    // エスケープシーケンスを読み取りバッファに追加する
                    ParseEscapeSequenceAndAppendtoBuff(input, buff);
                }
                else
                {
                    // 該当しない場合
                    // その文字をそのままバッファに追加する
                    buff.Append(c1);
                }
            }
			throw new ParseException(input, "syntax error. unclosed quoted string.");
		}
        void ParseEscapeSequenceAndAppendtoBuff(Input input, StringBuilder buff)
        {
            // 次の文字を読取る
            var curr = input.GoNext();
            // Unicodeエスケープシーケンスに該当するかチェック
            if (curr == 'u')
            {
                // 該当する場合
                // シーケンスを読み取ってバッファに追加
                buff.Append(ParseEscapedUnicode(input));
            }
            else
            {
                // 該当しない場合
                // シーケンスの照応する文字をバッファに追加
                buff.Append(ResolveSimpleEscapeSequece(input));
            }
        }
        char ResolveSimpleEscapeSequece(Input input)
        {
            var x = input.Current;
            switch (x) // options: ", /, \, b, f, n, r and t. except u.
            {
                case '"':
                case '/':
                case '\\':
                    return x;
                case 'b':
                    return '\b';
                case 'f':
                    return '\f';
                case 'n':
                    return '\n';
                case 'r':
                    return '\r';
                case 't':
                    return '\t';
                default:
                    throw new ParseException(input, string.Format(
                        "syntax error. unknown escape sequence \"\\{0}\".", x));
            }
        }
        string ParseEscapedUnicode(Input input)
        {
            // 1つ目の\uXXXX（のXXXX）を読み取る
            var mayHighSurrogate = Parse4HexDigits(input);
            // 読み取り結果が「上位サロゲート」に該当するかチェック
            if (IsHighSurrogate(mayHighSurrogate))
            {
                // 該当する場合
                // 2つ目の\uXXXXを読み取る
                input.GoNext(); input.Check('\\');
                input.GoNext(); input.Check('u');
                // XXXXは「下位サロゲート」（であるはず）
                var lowSurrogate = Parse4HexDigits(input);
                // 上位サロゲートと下位サロゲートからUnicodeのコードポイントを計算
                var pt = (mayHighSurrogate - 0xD800) * 0x400 + (lowSurrogate - 0xDC00) + 0x10000;
                // 文字列化して返す（charの範囲をオーバーしているからstringで表現される）
                return char.ConvertFromUtf32(pt);
            }
            else
            {
                // 該当しない場合
                // そのまま独立した文字として返す
                return char.ConvertFromUtf32(mayHighSurrogate);
            }
        }
        int Parse4HexDigits(Input input)
        {
            // 読み取り結果を順次アサインする変数
            var tmp = 0;
            // ループで4文字だけ読み取る
            for (var i = 0; i < 4; i++)
            {
                // 前回ループでアサインされた数値に16を乗じて桁上げする
                tmp *= 16;
                // 次の1文字読み取る。16進数であるはず
                var hexDigit = input.GoNext();
                // 文字の範囲ごとに適切な計算を行って整数化する
                if ('0' <= hexDigit && hexDigit <= '9')
                {
                    // 0～9であればコードポイントを減ずるだけでそのまま照応する整数になる
                    tmp += hexDigit - '0';
                }
                else if ('A' <= hexDigit && hexDigit <= 'F')
                {
                    // A～FであればAのコードポイントを減じて、10を加えると照応する整数になる
                    tmp += hexDigit - 'A' + 10;
                }
                else if ('a' <= hexDigit && hexDigit <= 'f')
                {
                    // a～fであればaのコードポイントを減じて、10を加えると照応する整数になる
                    tmp += hexDigit - 'a' + 10;
                }
                else
                {
                    throw new ParseException(input, "syntax error. invalid character as hex-digit.");
                }
            }
            return tmp;
        }
        bool IsHighSurrogate(int i)
        {
            return 0xD800 <= i && i <= 0xDBFF;
        }
		string ParseIdentifierString(Input input)
		{
			StringBuilder buff = new StringBuilder();
			while (!input.EndOfFile)
			{
				if (input.Current <= WhiteSpace || input.Current == ':')
				{
					break;
				}
				buff.Append(input.Current);
				input.GoNext();
			}
			return buff.ToString();
		}
		IJsonObject ParseStringNode(Input input)
		{
			return JsonObject.Of(ParseQuotedString(input));
		}
		IJsonObject ParseBooleanNode(Input input)
		{
			string r = input.ClipToken(BooleanPattern);
			return JsonObject.Of(r.Equals("true"));
		}
		IJsonObject ParseNullNode(Input input)
		{
			input.GoNext("null");
			return JsonObject.OfNull();
		}
		IJsonObject ParseNumberNode(Input input)
		{
			string r = input.ClipToken(NumberPattern);
			return JsonObject.Of(double.Parse(r));
		}
		IJsonObject ParseArrayNode(Input input)
		{
			input.Check('[');
			input.GoNext();
			IList<IJsonObject> buff = new List<IJsonObject>();

			input.SkipWhitespace();
			if (input.Current == ']')
			{
				input.GoNext();
				return JsonObject.Of(buff);
			}

			while (!input.EndOfFile)
			{
				buff.Add(ParseNode(input));

				input.SkipWhitespace();
				if (input.Current == ']')
				{
					input.GoNext();
					return JsonObject.Of(buff);
				}
				input.Check(',');
				input.GoNext();
				input.SkipWhitespace();
			}
			throw new ParseException(input, "unclosed Array literal.");
		}
		IJsonObject ParseObjectNode(Input input)
		{
			input.Check('{');
			input.GoNext();
			var b = JsonObject.Builder();

			input.SkipWhitespace();
			if (input.Current == '}')
			{
				input.GoNext();
				return b.Build();
			}

			while (!input.EndOfFile)
			{
				string propName;
				if (input.Current == '"' || input.Current == '\'')
				{
					propName = ParseQuotedString(input);
				}
				else 
				{
					propName = ParseIdentifierString(input);
				}

				input.SkipWhitespace();
				input.Check(':');
				input.GoNext();

				input.SkipWhitespace();
				b.Append(propName, ParseNode(input));

				input.SkipWhitespace();
				if (input.Current == '}')
				{
					input.GoNext();
					return b.Build();
				}
				input.Check(',');
				input.GoNext();
				input.SkipWhitespace();
			}
			throw new ParseException(input, "unclosed Object literal.");
		}
	}
}

