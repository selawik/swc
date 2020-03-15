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
        static readonly Dictionary<SyntaxKind, String> to = new Dictionary<SyntaxKind, String>
        {
            { SyntaxKind.PlusToken, "+" },
            { SyntaxKind.MinusToken, "-" },
            { SyntaxKind.StarToken, "*" },
            { SyntaxKind.SlashToken, "/" },
            { SyntaxKind.BangToken, "!" },
            { SyntaxKind.EqualsToken, "=" },
            { SyntaxKind.TildeToken, "~" },
            { SyntaxKind.LessToken, "<" },
            { SyntaxKind.LessEqualsToken, "<=" },
            { SyntaxKind.GreaterToken, ">" },
            { SyntaxKind.GreaterEqualsToken, ">=" },
            { SyntaxKind.AmpersandToken, "&" },
            { SyntaxKind.AmpersandAmpersandToken, "&&" },
            { SyntaxKind.PipeToken, "|" },
            { SyntaxKind.PipePipeToken, "||" },
            { SyntaxKind.HatToken, "^" },
            { SyntaxKind.EqualsEqualsToken, "==" },
            { SyntaxKind.BangEqualsToken, "!=" },
            { SyntaxKind.OpenParenthesisToken, "(" },
            { SyntaxKind.CloseParenthesisToken, ")" },
            { SyntaxKind.OpenBraceToken, "{" },
            { SyntaxKind.CloseBraceToken, "}" },
            { SyntaxKind.ColonToken, ":" },
            { SyntaxKind.CommaToken, "," },
            { SyntaxKind.BreakKeyword, "break" },
            { SyntaxKind.ContinueKeyword, "continue" },
            { SyntaxKind.ElseKeyword, "else" },
            { SyntaxKind.FalseKeyword, "false" },
            { SyntaxKind.ForKeyword, "for" },
            { SyntaxKind.IfKeyword, "if" },
            { SyntaxKind.LetKeyword, "let" },
            { SyntaxKind.ReturnKeyword, "return" },
            { SyntaxKind.TrueKeyword, "true" },
            { SyntaxKind.VarKeyword, "var" },
            { SyntaxKind.WhileKeyword, "while" },
            { SyntaxKind.DoKeyword, "do" },
            { SyntaxKind.NamespaceKeyword, "namespace" },
            { SyntaxKind.ClassKeyword, "class" },
            { SyntaxKind.StructKeyword, "struct" },
            { SyntaxKind.InterfaceKeyword, "interface" },
            { SyntaxKind.AsyncKeyword, "async" },
            { SyntaxKind.GetKeyword, "get" },
            { SyntaxKind.SetKeyword, "set" },
            { SyntaxKind.SwitchKeyword, "switch" },
            { SyntaxKind.CaseKeyword, "case" },
            { SyntaxKind.WhenKeyword, "when" },
        };

        static readonly Dictionary<String, SyntaxKind> from = to.ToDictionary(k => k.Value, k => k.Key);

        public static String? GetText(SyntaxKind kind) => to.TryGetValue(kind, out var result) ? result : null;

        public static SyntaxKind GetKeywordKind(String text) => from.TryGetValue(text, out var kind) ? kind : SyntaxKind.IdentifierToken;
    }
}
