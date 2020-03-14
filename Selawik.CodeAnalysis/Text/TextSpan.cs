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

namespace Selawik.CodeAnalysis.Text
{
    public struct TextSpan : IEquatable<TextSpan>
    {
        public TextSpan(Int32 start, Int32 length)
        {
            Start = start;
            Length = length;
        }

        public Int32 Start { get; }
        public Int32 Length { get; }
        public Int32 End => Start + Length;

        public static TextSpan FromBounds(Int32 start, Int32 end)
        {
            var length = end - start;
            return new TextSpan(start, length);
        }

        public override String ToString() => $"{Start}..{End}";

        public override Boolean Equals(Object? obj) => obj is TextSpan span ? Equals(span) : false;
        public override Int32 GetHashCode() => HashCode.Combine(Start, Length);
        public static Boolean operator ==(TextSpan left, TextSpan right) => left.Equals(right);
        public static Boolean operator !=(TextSpan left, TextSpan right) => !(left == right);
        public Boolean Equals(TextSpan other) => Start == other.Start && Length == other.Length;
    }
}
