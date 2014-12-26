using System;

namespace Grammatika
{
    public class Token
    {
        private readonly TokenKind _kind;
        private readonly SourceLocation _from;
        private readonly SourceLocation _to;
        private object _value;

        public Token(TokenKind kind, SourceLocation from, object value = null)
            : this(kind, @from, @from)
        {
        }

        public Token(TokenKind kind, SourceLocation @from, SourceLocation to, object value = null)
        {
            _kind = kind;
            _from = from.Clone();
            _to= to.Clone();
            _value = value;
        }

        public TokenKind Kind
        {
            get { return _kind; }
        }

        public SourceLocation From
        {
            get { return _from; }
        }

        public SourceLocation To
        {
            get { return _to; }
        }

        public object Value
        {
            get { return _value; }
        }

        public override string ToString()
        {
            return string.Format("Kind: {0}, From: {1}, To: {2}", _kind, _from, _to);
        }

        protected bool Equals(Token other)
        {
            return _kind == other._kind && Equals(_from, other._from) && Equals(_to, other._to);
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
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Token) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) _kind;
                hashCode = (hashCode*397) ^ (_from != null ? _from.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_to != null ? _to.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}