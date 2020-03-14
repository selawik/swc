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

namespace Selawik.CodeAnalysis.Syntax
{
    public static class SyntaxFacts
    {
        public static String? GetText(SyntaxKind kind) => kind switch
        {
            SyntaxKind.PlusToken => "+",
            SyntaxKind.MinusToken => "-",
            SyntaxKind.StarToken => "*",
            SyntaxKind.SlashToken => "/",
            SyntaxKind.BangToken => "!",
            SyntaxKind.EqualsToken => "=",
            SyntaxKind.TildeToken => "~",
            SyntaxKind.LessToken => "<",
            SyntaxKind.LessEqualsToken => "<=",
            SyntaxKind.GreaterToken => ">",
            SyntaxKind.GreaterEqualsToken => ">=",
            SyntaxKind.AmpersandToken => "&",
            SyntaxKind.AmpersandAmpersandToken => "&&",
            SyntaxKind.PipeToken => "|",
            SyntaxKind.PipePipeToken => "||",
            SyntaxKind.HatToken => "^",
            SyntaxKind.EqualsEqualsToken => "==",
            SyntaxKind.BangEqualsToken => "!=",
            SyntaxKind.OpenParenthesisToken => "(",
            SyntaxKind.CloseParenthesisToken => ")",
            SyntaxKind.OpenBraceToken => "{",
            SyntaxKind.CloseBraceToken => "}",
            SyntaxKind.ColonToken => ":",
            SyntaxKind.CommaToken => ",",
            SyntaxKind.BreakKeyword => "break",
            SyntaxKind.ContinueKeyword => "continue",
            SyntaxKind.ElseKeyword => "else",
            SyntaxKind.FalseKeyword => "false",
            SyntaxKind.ForKeyword => "for",
            SyntaxKind.FunctionKeyword => "function",
            SyntaxKind.IfKeyword => "if",
            SyntaxKind.LetKeyword => "let",
            SyntaxKind.ReturnKeyword => "return",
            SyntaxKind.TrueKeyword => "true",
            SyntaxKind.VarKeyword => "var",
            SyntaxKind.WhileKeyword => "while",
            SyntaxKind.DoKeyword => "do",
            _ => null,
        };

        public static SyntaxKind GetKeywordKind(string text) => text switch
        {
            "break" => SyntaxKind.BreakKeyword,
            "continue" => SyntaxKind.ContinueKeyword,
            "else" => SyntaxKind.ElseKeyword,
            "false" => SyntaxKind.FalseKeyword,
            "for" => SyntaxKind.ForKeyword,
            "function" => SyntaxKind.FunctionKeyword,
            "if" => SyntaxKind.IfKeyword,
            "let" => SyntaxKind.LetKeyword,
            "return" => SyntaxKind.ReturnKeyword,
            "true" => SyntaxKind.TrueKeyword,
            "var" => SyntaxKind.VarKeyword,
            "while" => SyntaxKind.WhileKeyword,
            "do" => SyntaxKind.DoKeyword,
            _ => SyntaxKind.IdentifierToken,
        };

    }
}
