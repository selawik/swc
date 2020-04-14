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
using System.Text;
using Selawik.CodeAnalysis.Syntax;
using Selawik.CodeAnalysis.Text;

namespace Selawik.CodeAnalysis
{
    public class Lexer
    {
        readonly SourceText text;
        readonly SyntaxTree syntaxTree;
        Int32 start, position;
        TokenKind kind;
        Object? value;

        public Lexer(SyntaxTree syntaxTree)
        {
            this.syntaxTree = syntaxTree;
            text = syntaxTree.Text;
        }

        public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

        public SyntaxToken Lex()
        {
            start = position;
            kind = TokenKind.BadToken;
            value = null;

            switch (Current)
            {
                case '\0': kind = TokenKind.EndOfFileToken; break;
                case '.': kind = TokenKind.DotToken; position++; break;
                case ';': kind = TokenKind.SemicolonToken; position++; break;
                case '+': kind = TokenKind.PlusToken; position++; break;
                case '-': kind = TokenKind.MinusToken; position++; break;
                case '*': kind = TokenKind.StarToken; position++; break;
                case '/': kind = TokenKind.SlashToken; position++; break;
                case '(': kind = TokenKind.OpenParenthesisToken; position++; break;
                case ')': kind = TokenKind.CloseParenthesisToken; position++; break;
                case '{': kind = TokenKind.OpenBraceToken; position++; break;
                case '}': kind = TokenKind.CloseBraceToken; position++; break;
                case ':': kind = TokenKind.ColonToken; position++; break;
                case ',': kind = TokenKind.CommaToken; position++; break;
                case '~': kind = TokenKind.TildeToken; position++; break;
                case '^': kind = TokenKind.HatToken; position++; break;
                case '&': LexDoubleOperator('&', TokenKind.AmpersandToken, TokenKind.AmpersandAmpersandToken); break;
                case '|': LexDoubleOperator('|', TokenKind.PipeToken, TokenKind.PipePipeToken); break;
                case '=': LexDoubleOperator('=', TokenKind.EqualsToken, TokenKind.EqualsEqualsToken); break;
                case '!': LexDoubleOperator('=', TokenKind.BangToken, TokenKind.BangEqualsToken); break;
                case '<': LexDoubleOperator('=', TokenKind.LessToken, TokenKind.LessEqualsToken); break;
                case '>': LexDoubleOperator('=', TokenKind.GreaterToken, TokenKind.GreaterEqualsToken); break;
                case '"': LexString(); break;
                case var c when c >= '0' && c <= '9': LexNumber(); break;
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    LexWhiteSpace(); break;

                default:
                    if (Char.IsLetter(Current))
                    {
                        LexIdentifierOrKeyword();
                    }
                    else if (Char.IsWhiteSpace(Current))
                    {
                        LexWhiteSpace();
                    }
                    else
                    {
                        Diagnostics.ReportBadCharacter(position, Current);
                        position++;
                    }
                    break;

            }

            var length = position - start;
            var sourceText = SyntaxFacts.GetText(kind) ?? text.ToString(start, length);

            return new SyntaxToken(syntaxTree, kind, start, sourceText, value);
        }

        private void LexDoubleOperator(Char expecting, TokenKind single, TokenKind @double)
        {
            position++;

            if (Current != expecting)
            {
                kind = single;
            }
            else
            {
                kind = @double;
                position++;
            }
        }

        private void LexString()
        {
            // Skip the current quote
            position++;

            var sb = new StringBuilder();
            var done = false;

            while (!done)
            {
                switch (Current)
                {
                    case '\0':
                    case '\r':
                    case '\n':
                        Diagnostics.ReportUnterminatedString(new TextSpan(start, 1));
                        done = true;
                        break;
                    case '"':
                        if (Lookahead == '"')
                        {
                            sb.Append(Current);
                            position += 2;
                        }
                        else
                        {
                            position++;
                            done = true;
                        }
                        break;
                    default:
                        sb.Append(Current);
                        position++;
                        break;
                }
            }

            kind = TokenKind.StringToken;
            value = sb.ToString();
        }

        private void LexWhiteSpace()
        {
            while (Char.IsWhiteSpace(Current))
                position++;

            kind = TokenKind.WhitespaceToken;
        }

        private void LexNumber()
        {
            while (Char.IsDigit(Current))
                position++;

            var length = position - start;
            var source = text.ToString(start, length);

            // TODO: Read dobule
            if (!Int32.TryParse(source, out var parsed))
            {
                Diagnostics.ReportInvalidNumber(new TextSpan(start, length), text);
            }


            value = parsed;
            kind = TokenKind.NumberToken;
        }

        private void LexIdentifierOrKeyword()
        {
            while (Char.IsLetter(Current))
                position++;

            var length = position - start;
            var source = text.ToString(start, length);
            kind = SyntaxFacts.GetKeywordKind(source);
        }

        Char Current => Peek(0);
        Char Lookahead => Peek(1);

        Char Peek(Int32 offset)
        {
            var index = position + offset;

            if (index >= text.Length)
                return '\0';

            return text[index];
        }
    }
}
