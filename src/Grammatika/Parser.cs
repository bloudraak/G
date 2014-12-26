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
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;

    public class Parser
    {
        public LanguageDefinition ParseText(string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var reader = new StringReader(source);
            return Parse(new Scanner(reader));
        }

        public LanguageDefinition Parse(Scanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException("scanner");
            }

            var tokens = scanner.Scan().ToArray();

            var reader = new TokenReader(tokens);

            return ParseLanguageDefinition(reader);
        }

        private static LanguageDefinition ParseLanguageDefinition(TokenReader reader)
        {
            var result = new LanguageDefinition();

            LanguageKeyword(reader);
            result.Name = Identifier(reader);
            BeginKeyword(reader);
            result.Rules.AddRange(RuleDefinitions(reader));
            EndKeyword(reader);

            return result;
        }

        private static List<RuleDefinition> RuleDefinitions(TokenReader reader)
        {
            var rule = new RuleDefinition();
            if (reader.Current.Kind != TokenKind.SyntaxKeyword)
            {
                return new List<RuleDefinition>();
            }

            Syntax(reader);
            rule.Name = Identifier(reader);
            Assigment(reader);
            rule.Text = TextLiteral(reader);
            EndOfStatement(reader);
            return new List<RuleDefinition>(new[] { rule });
        }

        private static void EndOfStatement(TokenReader reader)
        {
            var token = reader.Current;
            var kind = token.Kind;
            if (kind != TokenKind.EndOfStatement)
            {
                throw new ParserException(string.Format("Expected \"language\" but got \"{0}\" instead",
                                                        kind));
            }
            reader.MoveNext();
        }

        private static string TextLiteral(TokenReader reader)
        {
            var token = reader.Current;
            var kind = token.Kind;
            if (kind != TokenKind.TextLiteral)
            {
                throw new ParserException(string.Format("Expected \"language\" but got \"{0}\" instead",
                                                        kind));
            }
            reader.MoveNext();
            return token.Value.ToString();
        }

        private static void Assigment(TokenReader reader)
        {
            var token = reader.Current;
            var kind = token.Kind;
            if (kind != TokenKind.Assignment)
            {
                throw new ParserException(string.Format("Expected \"language\" but got \"{0}\" instead",
                                                        kind));
            }
            reader.MoveNext();
        }

        private static void Syntax(TokenReader reader)
        {
            var token = reader.Current;
            var kind = token.Kind;
            if (kind != TokenKind.SyntaxKeyword)
            {
                throw new ParserException(string.Format("Expected \"language\" but got \"{0}\" instead",
                                                        kind));
            }
            reader.MoveNext();
        }

        private static void LanguageKeyword(TokenReader reader)
        {
            var token = reader.Current;
            var kind = token.Kind;
            if (kind != TokenKind.LanguageKeyword)
            {
                throw new ParserException(string.Format("Expected \"language\" but got \"{0}\" instead",
                                                        kind));
            }
            reader.MoveNext();
        }

        private static string Identifier(TokenReader reader)
        {
            var token = reader.Current;
            var kind = token.Kind;
            if (kind != TokenKind.Identifier)
            {
                throw new ParserException(string.Format("Expected an identifier but got \"{0}\" instead",
                                                        kind));
            }
            reader.MoveNext();
            return token.Value.ToString();
        }

        private static void EndKeyword(TokenReader reader)
        {
            if (reader.Current.Kind != TokenKind.EndKeyword)
            {
                throw new ParserException(string.Format("Expected an \"}}\" but got \"{0}\" instead",
                                                        reader.Current.Kind));
            }
            reader.MoveNext();
        }

        private static void BeginKeyword(TokenReader reader)
        {
            if (reader.Current.Kind != TokenKind.BeginKeyword)
            {
                throw new ParserException(string.Format("Expected an \"{{\" but got \"{0}\" instead",
                                                        reader.Current.Kind));
            }
            reader.MoveNext();
        }
    }
}
