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
    using System.IO;

    public class SourceReader
    {
        private readonly SourceLocation _location;

        private int _position;
        private readonly string _source;
        private readonly int _sourceLength;

        public SourceReader(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            _location = new SourceLocation(1, 0);
            _source = reader.ReadToEnd();
            _sourceLength = _source.Length;
        }

        public SourceLocation Location
        {
            get { return _location; }
        }

        public int Position
        {
            get
            {
                if (_position <= 0)
                {
                    return 0;
                }
                if (_position >= _sourceLength)
                {
                    return _sourceLength + 1;
                }
                return _position;
            }
            set
            {
                if (value <= 0)
                {
                    _position = 0;
                }
                else if (value >= _sourceLength)
                {
                    _position = _sourceLength;
                }
                else
                {
                    _position = value;
                }
            }
        }

        public char Peek(int offset = 0)
        {
            var k = Position + offset;
            if (k >= _sourceLength)
            {
                return char.MaxValue;
            }
            return _source[k];
        }

        public char Read()
        {
            if (Position > _sourceLength)
            {
                return char.MaxValue;
            }

            var result = _source[Position++];
            switch (result)
            {
                case '\n':
                    _location.ColumnNumber = 1;
                    _location.LineNumber++;
                    break;

                default:
                    _location.ColumnNumber++;
                    break;
            }
            return result;
        }

        public void Move(int offset = 1)
        {
            for (int i = 0; i < offset; i++)
            {
                Read();
            }
        }

        public void ConsumeWhile(Func<char, bool> func)
        {
            var c = Peek();
            while (c != char.MaxValue)
            {
                if (!func(c))
                {
                    break;
                }
                c = Read();
            }
        }
    }
}
