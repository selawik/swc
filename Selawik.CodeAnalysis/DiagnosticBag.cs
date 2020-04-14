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
using System.Collections;
using System.Collections.Generic;
using Selawik.CodeAnalysis.Syntax;
using Selawik.CodeAnalysis.Text;

namespace Selawik.CodeAnalysis
{
    public sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        readonly List<Diagnostic> diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => diagnostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void ReportInvalidNumber(TextSpan span, SourceText text)
            => Report(span, $"The number '{text}' isn't valid.");

        public void ReportUnterminatedString(TextSpan span)
            => Report(span, "Unterminated string literal.");

        public void ReportBadCharacter(Int32 position, Char character)
            => Report(new TextSpan(position, 1), $"Bad character input: '{character}'.");

        public void ReportUnexpectedToken(TextSpan span, TokenKind actualKind, TokenKind expectedKind)
            => Report(span, $"Unexpected token <{actualKind}>, expected <{expectedKind}>.");


        void Report(TextSpan span, String message)
        {
            var diagnostic = new Diagnostic(span, message);
            diagnostics.Add(diagnostic);
        }
    }
}
