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

            return default;
        }


        Char Peek(Int32 offset)
        {
            var index = position + offset;

            if (index >= text.Length)
                return '\0';

            return text[index];
        }
    }
}
