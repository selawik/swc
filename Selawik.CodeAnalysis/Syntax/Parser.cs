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

using System.Collections.Immutable;
using System.Runtime.InteropServices.WindowsRuntime;
using Selawik.CodeAnalysis.Text;

namespace Selawik.CodeAnalysis.Syntax
{
    public sealed class Parser
    {
        readonly SourceText text;
        readonly Lexer lexer;
        readonly SyntaxTree syntaxTree;

        public DiagnosticBag Diagnostics { get; }

        SyntaxToken? peek;
        SyntaxToken current;

        public Parser(SyntaxTree syntaxTree)
        {
            lexer = new Lexer(syntaxTree);
            Diagnostics = lexer.Diagnostics;
            this.syntaxTree = syntaxTree;

            current = lexer.Lex();
        }

        SyntaxToken Current => current;

        SyntaxToken Peek => peek ??= lexer.Lex();

        SyntaxToken NextToken()
        {
            var nextToken = peek ?? current;
            current = lexer.Lex();
            peek = null;

            while (current.Kind == TokenKind.WhitespaceToken)
            {
                current = lexer.Lex();
            }

            return nextToken;
        }

        SyntaxToken MatchToken(TokenKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            Diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
            return new SyntaxToken(syntaxTree, kind, Current.Position, null, null);
        }

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            var ns = ParseNamespaceDirective();
            var eof = MatchToken(TokenKind.EndOfFileToken);
            return new CompilationUnitSyntax(ns, eof, syntaxTree);
        }

        NamespaceDirectiveSyntax ParseNamespaceDirective()
        {
            var keyword = MatchToken(TokenKind.NamespaceKeyword);
            var ns = ParseDottedNameList();
            var semi = MatchToken(TokenKind.SemicolonToken);
            return new NamespaceDirectiveSyntax(keyword, ns, semi, syntaxTree);
        }

        SeparatedSyntaxList<SyntaxToken> ParseDottedNameList()
        {
            var nodes = ImmutableArray.CreateBuilder<SyntaxNode>();
            while (current.Kind == TokenKind.IdentifierToken)
            {
                nodes.Add(MatchToken(TokenKind.IdentifierToken));

                if (current.Kind == TokenKind.DotToken)
                    nodes.Add(NextToken());
                else
                    break;
            }

            return new SeparatedSyntaxList<SyntaxToken>(nodes.ToImmutable());
        }
    }
}
