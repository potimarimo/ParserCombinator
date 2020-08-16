﻿using Marimo.ParserCombinator;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Marimo.Parser
{
    
    public class JSON
    {
        static Parser<string> Null => new WordParser("null", true);

        static Parser<char> bracketOpen => new CharParser('{');

        static Parser<char> bracketClose => new CharParser('}');

        static Parser<char> doubleQuote => new CharParser('"');

        static Parser<char> collon => new CharParser(':');

        static Parser<string> @string = new WordParser(@"""a""");

        static Parser<char> number => new CharParser('1');


        static Parser<JSONObject> jsonObject =>
            ParserConverter.Create(
                SequenceParser.Create(bracketOpen,
                    SequenceParser.Create(
                        OptionalParser.Create(
                            SequenceParser.Create(
                                @string,
                                SequenceParser.Create(
                                    collon,
                                    number))),
                        bracketClose)),
                tuple => tuple.Item2.Item1.IsPresent ? new JSONObject { Pairs = { ["a"] = new JSONLiteral("1", LiteralType.Number) } } : new JSONObject());

        public static async Task<JSONObject> ParseAsync(string text)
        {
            var (isSuccess, _, parsed) = await jsonObject.ParseAsync(new Cursol(text));
            if(isSuccess)
            {
                return parsed;
            }
            else
            {
                return null;
            }
        }
    }
}
    