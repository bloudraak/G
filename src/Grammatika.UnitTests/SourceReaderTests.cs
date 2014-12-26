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
    using NUnit.Framework;

    [TestFixture]
    public class SourceReaderTests
    {
        [Test]
        public void Read_Updated()
        {
            // Arrange
            var reader = new StringReader("0123\n5\n6789");
            var target = new SourceReader(reader);

            SourceLocation expected = new SourceLocation(lineNumber: 1, columnNumber: 2);
            SourceLocation actual;
            
            // Act
            target.Read();
            target.Read();
            actual = target.Location;
            
            // Assert
            Assert.AreEqual(expected, actual, "The source location is incorrect");
        }

        [Test]
        public void Read_AcccountsForLineNumbers()
        {
            // Arrange
            var reader = new StringReader("0123\n5\n6789");
            var target = new SourceReader(reader);

            SourceLocation expected = new SourceLocation(lineNumber: 2, columnNumber: 1);
            SourceLocation actual;

            // Act
            target.Read();
            target.Read();
            target.Read();
            target.Read();
            target.Read();
            actual = target.Location;

            // Assert
            Assert.AreEqual(expected, actual, "The source location is incorrect");
        }

        [Test]
        public void Read_AcccountsForEndOfStream()
        {
            // Arrange
            var reader = new StringReader("01\n5");
            var target = new SourceReader(reader);

            SourceLocation expected = new SourceLocation(lineNumber: 2, columnNumber: 2);
            SourceLocation actual;

            // Act
            target.Read();
            target.Read();
            target.Read();
            target.Read();
            target.Read();
            target.Read();
            target.Read();
            target.Read();
            target.Read();
            actual = target.Location;

            // Assert
            Assert.AreEqual(expected, actual, "The source location is incorrect");
        }
    }
}