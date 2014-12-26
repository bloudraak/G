// The MIT License (MIT)
// 
// Copyright (c) 2014 Werner Strydom
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Grammatika
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TokenReader
    {
        private readonly Token[] _tokens;
        private int _position;

        public TokenReader(IEnumerable<Token> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens");
            }
            _tokens = tokens.ToArray();
            _position = 0;
        }

        public Token Current
        {
            get
            {
                if (_position >= _tokens.Length)
                {
                    return null;
                }

                return _tokens[_position];
            }
        }

        public bool MoveNext()
        {
            if (++_position >= _tokens.Length)
            {
                return false;
            }
            return true;
        }
    }
}
