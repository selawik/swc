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

using System.Collections.Generic;

namespace Selawik.CodeAnalysis.Syntax
{
    internal class UnarySyntax : ExpressionSyntax
    {
        public UnarySyntax(SyntaxToken operatorToken, ExpressionSyntax operand, SyntaxTree syntaxTree) : base(syntaxTree)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Operand { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OperatorToken;
            yield return Operand;
        }
    }
}