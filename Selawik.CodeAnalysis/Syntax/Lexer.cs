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
        Int32 start, position;
        SyntaxKind kind;
        Object? value;

        public Lexer(SourceText text)
        {
            this.text = text;
        }

        public SyntaxToken Lex()
        {
            start = position;
            kind = SyntaxKind.BadToken;
            value = null;

            switch (Current)
            {
                case '\0': kind = SyntaxKind.EndOfFileToken; break;
                case '+': kind = SyntaxKind.PlusToken; position++; break;
                case '-': kind = SyntaxKind.MinusToken; position++; break;
                case '*': kind = SyntaxKind.StarToken; position++; break;
                case '/': kind = SyntaxKind.SlashToken; position++; break;
                case '(': kind = SyntaxKind.OpenParenthesisToken; position++; break;
                case ')': kind = SyntaxKind.CloseParenthesisToken; position++; break;
                case '{': kind = SyntaxKind.OpenBraceToken; position++; break;
                case '}': kind = SyntaxKind.CloseBraceToken; position++; break;
                case ':': kind = SyntaxKind.ColonToken; position++; break;
                case ',': kind = SyntaxKind.CommaToken; position++; break;
                case '~': kind = SyntaxKind.TildeToken; position++; break;
                case '^': kind = SyntaxKind.HatToken; position++; break;
                case '&': LexDoubleOperator('&', SyntaxKind.AmpersandToken, SyntaxKind.AmpersandAmpersandToken); break;
                case '|': LexDoubleOperator('|', SyntaxKind.PipeToken, SyntaxKind.PipePipeToken); break;
                case '=': LexDoubleOperator('=', SyntaxKind.EqualsToken, SyntaxKind.EqualsEqualsToken); break;
                case '!': LexDoubleOperator('=', SyntaxKind.BangToken, SyntaxKind.BangEqualsToken); break;
                case '<': LexDoubleOperator('=', SyntaxKind.LessToken, SyntaxKind.LessEqualsToken); break;
                case '>': LexDoubleOperator('=', SyntaxKind.GreaterToken, SyntaxKind.GreaterEqualsToken); break;
                case '"': LexString(); break;
                case var c when c >= '0' && c <= '9': LexNumber(); break;
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    LexWhiteSpace(); break;
                default:
                    if (Char.IsLetter(Current))
                        LexIdentifierOrKeyword();
                    else if (char.IsWhiteSpace(Current))
                        LexWhiteSpace();
                    else
                    {
                        // TODO: Diagnostics
                        position++;
                    }
                    break;

            }

            var length = position - start;
            var sourceText = SyntaxFacts.GetText(kind) ?? text.ToString(start, length);

            return new SyntaxToken(kind, start, sourceText, value);
        }

        private void LexDoubleOperator(Char expecting, SyntaxKind single, SyntaxKind @double)
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
                        // TODO: Diagnostics
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

            kind = SyntaxKind.StringToken;
            value = sb.ToString();
        }

        private void LexWhiteSpace()
        {
            while (Char.IsWhiteSpace(Current))
                position++;

            kind = SyntaxKind.WhitespaceToken;
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
                // TODO: Diagnostics
            }


            value = parsed;
            kind = SyntaxKind.NumberToken;
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
