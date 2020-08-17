﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace Marimo.ParserCombinator.Core
{
    public class SequenceParser<T1, T2> : IParser<(T1, T2)>
    {
        IParser<T1> parser1 { get; }
        IParser<T2> parser2 { get; }
        public SequenceParser(IParser<T1> parser1, IParser<T2> parser2)
        {
            this.parser1 = parser1;
            this.parser2 = parser2;
        }
        public async Task<(bool isSuccess, Cursol cursol, (T1, T2) parsed)> ParseAsync(Cursol cursol)
        {
            (T1, T2) returnValue = default;
            var helper = new SequenceHelper(cursol);

            return
                await helper.ParseAsync(parser1, value => returnValue.Item1 = value) &&
                await helper.ParseAsync(parser2, value => returnValue.Item2 = value)
                ? (true, helper.Current, returnValue)
                : (false, cursol, default);
        }
    }
    public class SequenceParser<T1, T2, T3> : IParser<(T1, T2, T3)>
    {
        IParser<T1> parser1 { get; }
        IParser<T2> parser2 { get; }
        IParser<T3> parser3 { get; }
        public SequenceParser(IParser<T1> parser1, IParser<T2> parser2, IParser<T3> parser3)
        {
            this.parser1 = parser1;
            this.parser2 = parser2;
            this.parser3 = parser3;
        }
        public async Task<(bool isSuccess, Cursol cursol, (T1, T2, T3) parsed)> ParseAsync(Cursol cursol)
        {
            (T1, T2, T3) returnValue = default;
            var helper = new SequenceHelper(cursol);

            return
                await helper.ParseAsync(parser1, value => returnValue.Item1 = value) &&
                await helper.ParseAsync(parser2, value => returnValue.Item2 = value) &&
                await helper.ParseAsync(parser3, value => returnValue.Item3 = value)
                ? (true, helper.Current, returnValue)
                : (false, cursol, default);
        }
    }
    class SequenceHelper
    {
        public Cursol Current { get; private set; }

        public SequenceHelper(Cursol current) => Current = current;
        public async Task<bool> ParseAsync<T>(IParser<T> parser, Action<T> setter)
        {
            var result = await parser.ParseAsync(Current);
            if (!result.isSuccess)
            {
                return result.isSuccess;
            }
            Current = result.cursol;
            setter(result.parsed);
            return result.isSuccess;
        }
    }
}