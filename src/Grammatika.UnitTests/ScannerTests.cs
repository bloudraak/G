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
    using System.IO;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class ScannerTests
    {
        [Test]
        public void Scan()
        {
            // Arrange
            var stringReader = new StringReader("language Sample{}");
            var sourceReader = new SourceReader(stringReader);
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
            var target = new Scanner(sourceReader);

            // Act
            var tokens = target.Scan().ToArray();

            // Assert
            Assert.AreEqual(-1, stringReader.Peek());
            Assert.AreEqual(expectedTokens.Length, tokens.Length);

            for (int i = 0; i < expectedTokens.Length; i++)
            {
                var expected = expectedTokens[i];
                var actual = tokens[i];

                Assert.AreEqual(expected, actual, "The token at {0} doesn't match", i);
            }
        }
    }
}
