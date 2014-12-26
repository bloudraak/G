// Copyright
namespace XNamespace
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Represents the kinds of tokens that exists
    /// </summary>
    public enum TokenKind
    {
        None,
        XTokenKind
    }

    /// <summary>
    /// Represents a token
    /// </summary>
    public class Token
    {
        public Token(TokenKind kind = default(TokenKind), string value = null)
        {
            Kind = kind;
            Value = value ?? string.Empty;
        }

        public TokenKind Kind { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Represents a lexer
    /// </summary>
    public class Lexer
    {
        private SourceReader _reader;

        public Lexer(SourceReader reader)
        {
            _reader = reader;
        }

        public bool Scan(ref Token token)
        {
            char c = _reader.Peek();
            if (c == char.MaxValue)
            {
                return false;
            }

            return XScanToken(ref token);
        }
        
        // $XScanToken
    }

    /// <summary>
    /// Represents a source reader
    /// </summary>
    public class SourceReader
    {
        private string _source;
        private int _offset;

        public SourceReader(TextReader reader)
        {
            _source = reader.ReadToEnd();
            _offset = 0;
        }

        public char Peek(int offset = 0)
        {
            if (_offset + offset >= _source.Length)
            {
                return char.MaxValue;
            }
            return _source[_offset + offset];
        }

        public void Advance(int offset = 1)
        {
            _offset += offset;
        }

        public char Next()
        {
            if (_offset >= _source.Length) return char.MaxValue;
            return _source[_offset++];
        }

    }

    public class Parser
    {
        public object ParseText(string source)
        {
            using (StringReader reader = new StringReader(source))
            {
                return Parse(reader);
            }
        }

        public object Parse(string path)
        {
            using (Stream stream = File.OpenRead(path))
            {
                return Parse(stream);
            }
        }

        public object Parse(TextReader reader)
        {
            return Parse(new SourceReader(reader));
        }

        public object Parse(SourceReader reader)
        {
            return null;
        }

        public object Parse(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                return Parse(reader);
            }
        }
    }
}
