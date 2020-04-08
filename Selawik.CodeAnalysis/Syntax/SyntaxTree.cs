//  
//  Copyright (C) 2020 Selawik Contributors
//  
//  This file is part of swc, the native Selawik compiler.
//  
//  swc is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  
//  swc is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with swc.  If not, see <https://www.gnu.org/licenses/>.
// 

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Selawik.CodeAnalysis.Text;

namespace Selawik.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        private delegate void ParseHandler(SyntaxTree syntaxTree,
                                   out CompilationUnitSyntax root,
                                   out ImmutableArray<Diagnostic> diagnostics);

        private SyntaxTree(SourceText text, ParseHandler handler)
        {
            Text = text;

            handler(this, out var root, out var diagnostics);

            Diagnostics = diagnostics;
            Root = root;
        }

        public SourceText Text { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public CompilationUnitSyntax Root { get; }

        public static SyntaxTree Load(string fileName)
        {
            var text = File.ReadAllText(fileName);
            var sourceText = SourceText.From(text, fileName);
            return Parse(sourceText);
        }

        private static void Parse(SyntaxTree syntaxTree, out CompilationUnitSyntax root, out ImmutableArray<Diagnostic> diagnostics)
        {
            var parser = new Parser(syntaxTree);
            root = parser.ParseCompilationUnit();
            diagnostics = parser.Diagnostics.ToImmutableArray();
        }

        public static SyntaxTree Parse(string text)
        {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
        }

        public static SyntaxTree Parse(SourceText text)
        {
            return new SyntaxTree(text, Parse);
        }

        public static ImmutableArray<SyntaxToken> ParseTokens(string text)
        {
            var sourceText = SourceText.From(text);
            return ParseTokens(sourceText);
        }

        public static ImmutableArray<SyntaxToken> ParseTokens(string text, out ImmutableArray<Diagnostic> diagnostics)
        {
            var sourceText = SourceText.From(text);
            return ParseTokens(sourceText, out diagnostics);
        }

        public static ImmutableArray<SyntaxToken> ParseTokens(SourceText text)
        {
            return ParseTokens(text, out _);
        }

        public static ImmutableArray<SyntaxToken> ParseTokens(SourceText text, out ImmutableArray<Diagnostic> diagnostics)
        {
            var tokens = new List<SyntaxToken>();

            void ParseTokens(SyntaxTree st, out CompilationUnitSyntax root, out ImmutableArray<Diagnostic> d)
            {
                var l = new Lexer(st);
                while (true)
                {
                    var token = l.Lex();
                    if (token.Kind == SyntaxKind.EndOfFileToken)
                    {
                        root = new CompilationUnitSyntax(st, token);
                        break;
                    }

                    tokens.Add(token);
                }

                d = l.Diagnostics.ToImmutableArray();
            }

            var syntaxTree = new SyntaxTree(text, ParseTokens);
            diagnostics = syntaxTree.Diagnostics;
            return tokens.ToImmutableArray();
        }
    }
}
