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

namespace Grammatika.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class ScannerTests
    {
        [Test]
        public void Scan_WithEmptyLanguage()
        {
            // Arrange
            const string source = "language Sample{}";
            var expectedTokens = new[]
                                 {
                                     new Token(TokenKind.LanguageKeyword,
                                               new SourceLocation(1, 0),
                                               new SourceLocation(1, 8)),
                                     new Token(TokenKind.Identifier,
                                               new SourceLocation(1, 9),
                                               new SourceLocation(1, 15)),
                                     new Token(TokenKind.BeginKeyword,
                                               new SourceLocation(1, 15),
                                               new SourceLocation(1, 16)),
                                     new Token(TokenKind.EndKeyword,
                                               new SourceLocation(1, 16),
                                               new SourceLocation(1, 17))
                                 };

            Scan(source, expectedTokens);
        }

        [Test]
        public void Scan_WithSingleProduction()
        {
            // Arrange
            const string source = "language Sample{ syntax LanguageDefinition=\"abc\"; }";
            var expectedTokens = new[]
                                 {
                                     new Token(TokenKind.LanguageKeyword,
                                               new SourceLocation(1, 0),
                                               new SourceLocation(1, 8)),
                                     new Token(TokenKind.Identifier,
                                               new SourceLocation(1, 9),
                                               new SourceLocation(1, 15)),
                                     new Token(TokenKind.BeginKeyword,
                                               new SourceLocation(1, 15),
                                               new SourceLocation(1, 16)),
                                     new Token(TokenKind.SyntaxKeyword,
                                               new SourceLocation(1, 17),
                                               new SourceLocation(1, 23)),
                                     new Token(TokenKind.Identifier,
                                               new SourceLocation(1, 24),
                                               new SourceLocation(1, 42)),
                                     new Token(TokenKind.Assignment,
                                               new SourceLocation(1, 42),
                                               new SourceLocation(1, 43)),
                                     new Token(TokenKind.TextLiteral,
                                               new SourceLocation(1, 43),
                                               new SourceLocation(1, 48),
                                               "S"),
                                     new Token(TokenKind.EndOfStatement,
                                               new SourceLocation(1, 48),
                                               new SourceLocation(1, 49)),
                                     new Token(TokenKind.EndKeyword,
                                               new SourceLocation(1, 50),
                                               new SourceLocation(1, 51))
                                 };

            Scan(source, expectedTokens);
        }

        private static void Scan(string source, IReadOnlyList<Token> expectedTokens)
        {
            // Arrange
            var stringReader = new StringReader(source);
            var sourceReader = new SourceReader(stringReader);
            var target = new Scanner(sourceReader);

            // Act
            var tokens = target.Scan().ToArray();

            // Assert
            Assert.AreEqual(-1, stringReader.Peek());
            

            var expectedCount = expectedTokens.Count;
            var actualCount = tokens.Length;
            var count = expectedCount;
            if (actualCount < expectedCount)
            {
                count = actualCount;
            }
            for (int i = 0; i < count; i++)
            {
                var expected = expectedTokens[i];
                var actual = tokens[i];

                Assert.AreEqual(expected, actual, "The token at index {0} doesn't match", i);
            }
            Assert.AreEqual(expectedTokens.Count, tokens.Length);
        }
    }
}
