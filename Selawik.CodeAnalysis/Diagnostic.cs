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
