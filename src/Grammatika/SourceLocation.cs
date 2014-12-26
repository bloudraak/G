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

    public class SourceLocation
    {
        public SourceLocation(int lineNumber, int columnNumber)
        {
            ColumnNumber = columnNumber;
            LineNumber = lineNumber;
        }

        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", LineNumber, ColumnNumber);
        }

        protected bool Equals(SourceLocation other)
        {
            return LineNumber == other.LineNumber && ColumnNumber == other.ColumnNumber;
        }

        public SourceLocation Clone()
        {
            return new SourceLocation(LineNumber, ColumnNumber);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((SourceLocation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (LineNumber*397) ^ ColumnNumber;
            }
        }
    }
}
