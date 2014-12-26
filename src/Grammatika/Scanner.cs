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
    using System.IO;
    using System.Text;
    using System.Threading;

    public sealed class Scanner
    {
        private readonly SourceReader _reader;

        public Scanner(Stream stream)
            : this(new StreamReader(stream))
        {
        }

        public Scanner(TextReader reader) : this(new SourceReader(reader))
        {
        }

        public Scanner(SourceReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            _reader = reader;
        }

        public IEnumerable<Token> Scan()
        {
            while (_reader.Peek() != char.MaxValue)
            {
                char c = _reader.Peek();
                if (char.IsWhiteSpace(c))
                {
                    _reader.Read();
                }
                else if (char.IsLetterOrDigit(c) || c == '_')
                {
                    SourceLocation from = _reader.Location.Clone();
                    StringBuilder text = new StringBuilder(1024);
                    while (char.IsLetterOrDigit(c))
                    {
                        text.Append(c);
                        _reader.Read();
                        c = _reader.Peek();
                    }
                    SourceLocation to = _reader.Location.Clone();

                    var s = text.ToString();
                    if (s == "language")
                    {
                        yield return new Token(TokenKind.LanguageKeyword, from, to, text.ToString());
                    }
                    else if (s == "syntax")
                    {
                        yield return new Token(TokenKind.SyntaxKeyword, from, to, text.ToString());
                    }
                    else
                    {
                        yield return new Token(TokenKind.Identifier, from, to, text.ToString());
                    }
                }
                else
                {
                    TokenKind kind = TokenKind.Error;
                    object value = c;
                    SourceLocation from = _reader.Location.Clone();
                    switch (c)
                    {
                        case '{':
                            kind = TokenKind.BeginKeyword;
                            _reader.Read();
                            break;
                        case '}':
                            kind = TokenKind.EndKeyword;
                            _reader.Read();
                            break;
                        case '=':
                            kind = TokenKind.Assignment;
                            _reader.Read();
                            break;
                        case ';':
                            kind = TokenKind.EndOfStatement;
                            _reader.Read();
                            break;
                        case '"':
                            kind = TokenKind.TextLiteral;
                            _reader.Read();
                            StringBuilder builder = new StringBuilder(1024);
                            c = _reader.Read();
                            while (_reader.Peek() != '"')
                            {
                                c = _reader.Read();
                                builder.Append(c);
                            }
                            c = _reader.Read();
                            break;
                        default:
                            _reader.Read();
                            break;
                    }
                    SourceLocation to = _reader.Location.Clone();
                    yield return new Token(kind, @from, to, value);
                }
            }
        }
    }
}
