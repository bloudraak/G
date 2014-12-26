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
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Reflection;
    using Microsoft.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class GeneratorTests
    {
        [Test]
        public void Generate()
        {
            // Arrange
            var language = new LanguageDefinition {Name = "Sample"};
            language.Rules.Add(new RuleDefinition {Name = "Main", Text = "abc"});
            var target = new Generator();

            // Act
            string source;
            using (var writer = new StringWriter())
            {
                target.Generate(language, writer);
                source = writer.ToString();
                File.WriteAllText("GeneratorTests.Generate.cs", source);
            }

            // Assert
            var assembly = CompileGeneratedSource(source);
            ExecuteGeneratedCode(assembly, string.Format("{0}.Parser", language.Name));
        }

        private static Assembly CompileGeneratedSource(string source)
        {
            var provider = new CSharpCodeProvider();
            var parameters = new CompilerParameters {GenerateInMemory = true};
            var results = provider.CompileAssemblyFromSource(parameters, source);

            var message = GetErrorMessages(results);
            Assert.IsFalse(results.Errors.HasErrors, message);
            Assert.IsFalse(results.Errors.HasWarnings, message);

            return results.CompiledAssembly;
        }

        private static string GetErrorMessages(CompilerResults results)
        {
            StringWriter writer = new StringWriter();
            foreach (var error in results.Errors)
            {
                writer.WriteLine(error);
            }
            var message = writer.ToString();
            return message;
        }

        private static void ExecuteGeneratedCode(Assembly assembly, string typeName)
        {
            object parser = assembly.CreateInstance(typeName);
            object[] args = {"language"};
            var type = parser.GetType();
            object result = type.InvokeMember("ParseText", BindingFlags.InvokeMethod, null, parser, args);
            Assert.IsNull(result, "The generated parser returned <null>");
        }
    }
}
