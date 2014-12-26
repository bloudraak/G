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
    using NUnit.Framework;

    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void Parse_WithNull_ThrowsArgumentNullException()
        {
            // Arrange
            Exception actual = null;
            var target = new Parser();

            // Act
            try
            {
                target.ParseText(null);
            }
            catch (Exception e)
            {
                actual = e;
            }

            // Assert
            Assert.IsNotNull(actual, "Expected ParseText to throw an exception");
            Assert.IsInstanceOf<ArgumentNullException>(actual);
        }

        [Test]
        public void Parse_WithEmptyLanguage_ReturnsLanguageDefinition()
        {
            // Arrange
            const string source = "language Sample{}";
            var target = new Parser();
            LanguageDefinition actual = null;

            // Act
            actual = target.ParseText(source);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("Sample", actual.Name);
        }

        [Test]
        public void Parse_WithSingleProduction()
        {
            // Arrange
            const string source = "language Sample{ syntax LanguageDefinition=\"abc\"; }";
            var target = new Parser();
            LanguageDefinition actual = null;

            // Act
            actual = target.ParseText(source);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("Sample", actual.Name);
            Assert.AreEqual(1, actual.Rules.Count);
            Assert.AreEqual("LanguageDefinition", actual.Rules[0].Name);
        }
    }
}
