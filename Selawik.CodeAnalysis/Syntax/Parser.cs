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

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection.Metadata.Ecma335;
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
            var builder = ImmutableArray.CreateBuilder<StatementSyntax>();

            while (current.Kind != TokenKind.EndOfFileToken)
            {
                var x = ParseExpression();
                var semi = MatchToken(TokenKind.SemicolonToken);
                builder.Add(new StatementSyntax(x, semi, syntaxTree));
            }

            var eof = MatchToken(TokenKind.EndOfFileToken);

            return new CompilationUnitSyntax(ns, builder.ToImmutable(), eof, syntaxTree);
        }

        ExpressionSyntax ParseExpression(Int32 parentPrecedence = 0, Boolean? rightAssociative = null)
        {
            // This function is 'always' trying to parse out a binary expression.
            // However, if it ends up we aren't actually parsing one out, it will
            // bail out accordingly.

            ExpressionSyntax left;

            // Check if we're parsing a unary operator. If we are, we want to consume it
            // and apply it to the next expression.
            var unaryOperatorPrecedence = SyntaxFacts.UnaryOperatorPrecedence(current.Kind);
            if (unaryOperatorPrecedence != 0)
            {
                var operatorToken = NextToken();
                var operand = ParseExpression(unaryOperatorPrecedence);
                left = new UnarySyntax(operatorToken, operand, syntaxTree);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = SyntaxFacts.BinaryOperatorPrecedence(current.Kind);
                // We may have gotten here, where current isn't actually a binary operator (e.g. 'f(-5)').
                // In that case, we should bail, and simply return what we've got.

                // In the case that we are parsing another operator (e.g. '2 * 3 + 4'), we understandably must check for precedence.
                // We're parsing left to right, so if we are already in a nested expression, then we need to decide where this newly
                // parsed out term goes; to the parent expression (2 * 3) or this one (3 + 4).
                // So, if the parent precedence is equal or higher, we also bail and return the term we've got.
                // As a slight caveat, some operators are right-associative. In that case, if the precedences are equal, we do
                // still want to eagerly parse them out as we go.
                if (precedence == 0 || (rightAssociative == true ? precedence < parentPrecedence : precedence <= parentPrecedence))
                    break;

                var operatorToken = NextToken();

                // If we're still going, then we have the higher precedence (or we're right associative), so we can parse
                // another expression out and form a binary expression from that. Since that makes us the "parent expression"
                // now, we need to pass in our precedence, and whether we're right associative.
                var right = ParseExpression(precedence, SyntaxFacts.IsRightAssociative(operatorToken.Kind));
                left = new BinarySyntax(left, operatorToken, right, syntaxTree);
            }

            return left;
        }

        ExpressionSyntax ParsePrimaryExpression() => current.Kind switch
        {
            TokenKind.TrueKeyword => ParseBoolean(),
            TokenKind.FalseKeyword => ParseBoolean(),
            TokenKind.StringToken => ParseString(),
            TokenKind.NumberToken => ParseNumber(),
            TokenKind.VarKeyword => ParseDeclaration(),
            TokenKind.UsingKeyword => ParseUsing(),
        };

        DeclarationSyntax ParseDeclaration()
        {
            var type = ParseDottedName(true);
            var name = MatchToken(TokenKind.IdentifierToken);
            MatchToken(TokenKind.EqualsToken);
            var expr = ParseExpression();

            return new DeclarationSyntax(type, name, expr, syntaxTree);
        }

        LiteralSyntax ParseBoolean() => new LiteralSyntax(
            current.Kind == TokenKind.TrueKeyword ? MatchToken(TokenKind.TrueKeyword) : MatchToken(TokenKind.FalseKeyword), syntaxTree);

        LiteralSyntax ParseString() => new LiteralSyntax(MatchToken(TokenKind.StringToken), syntaxTree);
        LiteralSyntax ParseNumber() => new LiteralSyntax(MatchToken(TokenKind.NumberToken), syntaxTree);

        UsingSyntax ParseUsing()
        {
            var @using = MatchToken(TokenKind.UsingKeyword);
            var ns = ParseDottedName();

            return new UsingSyntax(@using, ns, syntaxTree);
        }

        NamespaceDirectiveSyntax ParseNamespaceDirective()
        {
            var keyword = MatchToken(TokenKind.NamespaceKeyword);
            var ns = ParseDottedName();
            var semi = MatchToken(TokenKind.SemicolonToken);
            return new NamespaceDirectiveSyntax(keyword, ns, semi, syntaxTree);
        }

        SeparatedSyntaxList<SyntaxToken> ParseDottedName(bool allowVar = false)
        {
            var builder = ImmutableArray.CreateBuilder<SyntaxNode>();
            if (allowVar && current.Kind == TokenKind.VarKeyword)
            {
                builder.Add(NextToken());
                return new SeparatedSyntaxList<SyntaxToken>(builder.ToImmutable());
            }

            do
            {
                builder.Add(MatchToken(TokenKind.IdentifierToken));
            } while (current.Kind == TokenKind.DotToken);

            return new SeparatedSyntaxList<SyntaxToken>(builder.ToImmutable());

        }
    }
}
