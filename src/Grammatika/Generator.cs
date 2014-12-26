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
    using System.CodeDom.Compiler;
    using System.Globalization;
    using System.IO;

    public class Generator
    {
        public void Generate(LanguageDefinition language, TextWriter writer)
        {
            using (var indentedTextWriter = new IndentedTextWriter(writer))
            {
                Generate(language, indentedTextWriter);
            }
        }

        private void Generate(LanguageDefinition language, IndentedTextWriter writer)
        {
            string source = Properties.Resources.CSharpTemplate;

            source = source.Replace("// Copyright", Properties.Resources.Copyright);
            source = source.Replace("XNamespace", language.Name);

            string s = GetTokenKinds(language);
            source = source.Replace("XTokenKind", s);

            s = GeneratePeekingScanMethods(language);
            source = source.Replace("return XScanToken(ref token);", s);
            // Lexer 
            // return XScanToken(ref token);
            s = GenerateScanMethods(language);
            source = source.Replace("// $XScanToken", s);

            writer.Write(source);

            //writer.WriteLine("namespace {0}", language.Name);
            //writer.WriteLine("{");
            //writer.Indent++;
            //writer.WriteLine();
            //writer.WriteLine("using System;");
            //writer.WriteLine("using System.IO;");

            //GenerateTokenKind(language, writer);

            //GenerateToken(language, writer);

            //GenerateLexer(language, writer);
            
            //GenerateSourceReader(writer);
            
            //GenerateParser(writer);

            //writer.Indent--;
            //writer.WriteLine("}");
        }

        private string GeneratePeekingScanMethods(LanguageDefinition language)
        {
            StringWriter stringWriter = new StringWriter();
            IndentedTextWriter writer = new IndentedTextWriter(stringWriter);
            writer.Indent += 2;
            foreach (var rule in language.Rules)
            {
                var text = rule.Text;
                writer.Indent++;
                writer.WriteLine("if(_reader.Peek() == '{0}')", text[0]);
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine("if(Scan{0}(ref token))", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text));
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine("return true;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.Indent--;
                writer.WriteLine("}");

                writer.WriteLine("return false;");
            }
            return stringWriter.ToString();
        }

        private string GenerateScanMethods(LanguageDefinition language)
        {
            StringWriter stringWriter = new StringWriter();
            IndentedTextWriter writer = new IndentedTextWriter(stringWriter);
            writer.Indent += 2;
            foreach (var rule in language.Rules)
            {
                var text = rule.Text;
                writer.WriteLine("private bool Scan{0}(ref Token token)", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text));
                writer.WriteLine("{");
                writer.Indent++;
                for (int i = 0; i < text.Length; i++)
                {
                    writer.WriteLine("if(_reader.Peek({0}) != '{1}')", i, text[i]);
                    writer.WriteLine("{");
                    writer.Indent++;
                    writer.WriteLine("return false;");
                    writer.Indent--; 
                    writer.WriteLine("}");
                }
                writer.WriteLine("if(char.IsLetterOrDigit(_reader.Peek({0})))", text.Length);
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine("return false;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine("_reader.Advance({0});", text.Length);
                writer.WriteLine("token.Kind = TokenKind.{0};", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text));
                writer.WriteLine("token.Value = \"{0}\";", text);
                writer.WriteLine("return true;");
                writer.Indent--;
                writer.WriteLine("}");
            }
            return stringWriter.ToString();
        }

        private string GetTokenKinds(LanguageDefinition language)
        {
            StringWriter stringWriter = new StringWriter();
            IndentedTextWriter writer = new IndentedTextWriter(stringWriter);
            writer.Indent += 2;
            foreach (var rule in language.Rules)
            {
                writer.WriteLine("{0},", rule.Name);
                writer.WriteLine("{0},", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rule.Text));
            }
            return stringWriter.ToString();
        }

        private void GenerateTokenKind(LanguageDefinition language, IndentedTextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Represents the kinds of tokens that exists");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("public enum TokenKind");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("None,");
            foreach (var rule in language.Rules)
            {
                writer.WriteLine("{0},", rule.Name);
                writer.WriteLine("{0},", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rule.Text));
            }
            writer.Indent--;
            writer.WriteLine("}");
        }
        
        private void GenerateToken(LanguageDefinition language, IndentedTextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Represents a token");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("public class Token");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine("public Token()");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("Kind = TokenKind.None;");
            writer.WriteLine("Value = string.Empty; ");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public Token(TokenKind kind, string value = null)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("Kind = kind;");
            writer.WriteLine("Value = value; ");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public TokenKind Kind { get; set; }");
            writer.WriteLine("public string Value { get; set; }");
            writer.Indent--;
            writer.WriteLine("}");
        }

        private static void GenerateParser(IndentedTextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("public class Parser");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("public object ParseText(string source)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("using(StringReader reader = new StringReader(source))");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("return Parse(reader);");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public object Parse(string path)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("using(Stream stream = File.OpenRead(path))");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("return Parse(stream);");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public object Parse(TextReader reader)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("return Parse(new SourceReader(reader));");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public object Parse(SourceReader reader)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("return null;");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public object Parse(Stream stream)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("using(StreamReader reader = new StreamReader(stream))");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("return Parse(reader);");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
        }

        private static void GenerateSourceReader(IndentedTextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Represents a source reader");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("public class SourceReader");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("private string _source;");
            writer.WriteLine("private int _offset;");
            writer.WriteLine();
            writer.WriteLine("public SourceReader(TextReader reader)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("_source = reader.ReadToEnd();");
            writer.WriteLine("_offset = 0;");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public char Peek(int offset = 0)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("if(_offset + offset >= _source.Length)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("return char.MaxValue;");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine("return _source[_offset + offset];");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public void Advance(int offset = 1)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("_offset += offset;");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public char Next()");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("if(_offset >= _source.Length) return char.MaxValue;");
            writer.WriteLine("return _source[_offset++];");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine("}");
        }

        private static void GenerateLexer(LanguageDefinition language, IndentedTextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Represents a lexer");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("public class Lexer");
            writer.WriteLine("{");
            writer.Indent++;
            foreach (var rule in language.Rules)
            {
                writer.WriteLine();
                writer.WriteLine("public Token Scan{0}()", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rule.Text));
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine("return new Token(TokenKind.{0});", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rule.Text));
                writer.Indent--;
                writer.WriteLine("}");
            }
            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}
