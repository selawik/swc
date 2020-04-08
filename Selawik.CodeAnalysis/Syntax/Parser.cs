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

using Selawik.CodeAnalysis.Text;

namespace Selawik.CodeAnalysis.Syntax
{
    sealed class Parser
    {
        readonly SourceText text;
        readonly Lexer lexer;
        readonly SyntaxTree syntaxTree;

        public DiagnosticBag Diagnostics { get; }

        SyntaxToken? peek;
        SyntaxToken current = default!;

        public Parser(SyntaxTree syntaxTree)
        {
            lexer = new Lexer(syntaxTree);
            Diagnostics = lexer.Diagnostics;
            this.syntaxTree = syntaxTree;
        }

        SyntaxToken Current => current;

        SyntaxToken Peek => peek ??= lexer.Lex();

        SyntaxToken NextToken()
        {
            var nextToken = peek ?? current;
            current = lexer.Lex();
            peek = null;
            return nextToken;
        }

        SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            Diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
            return new SyntaxToken(syntaxTree, kind, Current.Position, null, null);
        }

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            var eof = MatchToken(SyntaxKind.EndOfFileToken);
            return new CompilationUnitSyntax(syntaxTree, eof);
        }

    }
}
