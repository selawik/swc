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
using System.Linq;

namespace Selawik.CodeAnalysis.Syntax
{
    public static class SyntaxFacts
    {
        static readonly Dictionary<TokenKind, String> to = new Dictionary<TokenKind, String>
        {
            { TokenKind.PlusToken, "+" },
            { TokenKind.MinusToken, "-" },
            { TokenKind.StarToken, "*" },
            { TokenKind.SlashToken, "/" },
            { TokenKind.BangToken, "!" },
            { TokenKind.EqualsToken, "=" },
            { TokenKind.TildeToken, "~" },
            { TokenKind.LessToken, "<" },
            { TokenKind.LessEqualsToken, "<=" },
            { TokenKind.GreaterToken, ">" },
            { TokenKind.GreaterEqualsToken, ">=" },
            { TokenKind.AmpersandToken, "&" },
            { TokenKind.AmpersandAmpersandToken, "&&" },
            { TokenKind.PipeToken, "|" },
            { TokenKind.PipePipeToken, "||" },
            { TokenKind.HatToken, "^" },
            { TokenKind.EqualsEqualsToken, "==" },
            { TokenKind.BangEqualsToken, "!=" },
            { TokenKind.OpenParenthesisToken, "(" },
            { TokenKind.CloseParenthesisToken, ")" },
            { TokenKind.OpenBraceToken, "{" },
            { TokenKind.CloseBraceToken, "}" },
            { TokenKind.ColonToken, ":" },
            { TokenKind.CommaToken, "," },
            { TokenKind.BreakKeyword, "break" },
            { TokenKind.ContinueKeyword, "continue" },
            { TokenKind.ElseKeyword, "else" },
            { TokenKind.FalseKeyword, "false" },
            { TokenKind.ForKeyword, "for" },
            { TokenKind.IfKeyword, "if" },
            { TokenKind.LetKeyword, "let" },
            { TokenKind.ReturnKeyword, "return" },
            { TokenKind.TrueKeyword, "true" },
            { TokenKind.VarKeyword, "var" },
            { TokenKind.WhileKeyword, "while" },
            { TokenKind.DoKeyword, "do" },
            { TokenKind.NamespaceKeyword, "namespace" },
            { TokenKind.ClassKeyword, "class" },
            { TokenKind.StructKeyword, "struct" },
            { TokenKind.InterfaceKeyword, "interface" },
            { TokenKind.AsyncKeyword, "async" },
            { TokenKind.GetKeyword, "get" },
            { TokenKind.SetKeyword, "set" },
            { TokenKind.SwitchKeyword, "switch" },
            { TokenKind.CaseKeyword, "case" },
            { TokenKind.WhenKeyword, "when" },
            { TokenKind.UsingKeyword, "using" }
        };

        static readonly Dictionary<String, TokenKind> from = to.ToDictionary(k => k.Value, k => k.Key);

        public static String? GetText(TokenKind kind) => to.TryGetValue(kind, out var result) ? result : null;

        public static TokenKind GetKeywordKind(String text) => from.TryGetValue(text, out var kind) ? kind : TokenKind.IdentifierToken;

        internal static Int32 UnaryOperatorPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                default:
                    return 0;
            }
        }

        internal static Int32 BinaryOperatorPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                default:
                    return 0;
            }
        }

        internal static Boolean? IsRightAssociative(TokenKind kind)
        {
            return false;
        }
    }
}
