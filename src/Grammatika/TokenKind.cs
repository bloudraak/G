using System;

namespace Grammatika
{
    public enum TokenKind
    {
        Error,
        LanguageKeyword,
        Identifier,
        BeginKeyword,
        EndKeyword,
        SyntaxKeyword,
        Assignment,
        TextLiteral,
        EndOfStatement
    }
}