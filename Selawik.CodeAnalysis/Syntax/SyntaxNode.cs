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
using System.IO;
using System.Linq;
using Selawik.CodeAnalysis.Text;

namespace Selawik.CodeAnalysis.Syntax
{
    public abstract class SyntaxNode
    {
        protected SyntaxNode(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        public SyntaxTree SyntaxTree { get; }

        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();

        public virtual TextSpan Span => TextSpan.FromBounds(GetChildren().First().Span.Start, GetChildren().Last().Span.End);

        public override String ToString()
        {
            var sr = new StringWriter();
            PrettyPrint(sr, this);
            return sr.ToString();
        }

        public void WriteTo(TextWriter writer)
        {
            PrettyPrint(writer, this);
        }

        public SyntaxToken GetLastToken()
        {
            if (this is SyntaxToken token)
                return token;

            return GetChildren().Last().GetLastToken();
        }

        static void PrettyPrint(TextWriter writer, SyntaxNode node, String indent = "", Boolean isLast = true)
        {
            var toConsole = writer == Console.Out;
            var marker = isLast ? "└──" : "├──";

            if (toConsole)
                Console.ForegroundColor = ConsoleColor.DarkGray;

            writer.Write(indent);
            writer.Write(marker);

            if (toConsole)
                Console.ForegroundColor = node is SyntaxToken ? ConsoleColor.Blue : ConsoleColor.Cyan;

            writer.Write(node.Kind);

            if (node is SyntaxToken { Value: { } } t)
            {
                writer.Write(" ");
                writer.Write(t.Value);
            }

            if (toConsole)
                Console.ResetColor();

            writer.WriteLine();

            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(writer, child, indent, child == lastChild);
        }
    }
}
