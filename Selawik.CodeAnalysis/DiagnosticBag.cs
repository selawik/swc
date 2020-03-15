using System;
using System.Collections;
using System.Collections.Generic;
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

        void Report(TextSpan span, String message)
        {
            var diagnostic = new Diagnostic(span, message);
            diagnostics.Add(diagnostic);
        }
    }

}
