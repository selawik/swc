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
using Selawik.CodeAnalysis.Text;

namespace Selawik.CodeAnalysis
{
    public sealed class Diagnostic
    {
        public Diagnostic(TextSpan span, String message)
        {
            Span = span;
            Message = message;
        }

        public TextSpan Span { get; }
        public String Message { get; }

        public override String ToString() => $"({Span}) {Message}";
    }

}
