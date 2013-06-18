/*
 Copyright (c) 2003-2006 Niels Kokholm and Peter Sestoft
 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:
 
 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
*/

// BotDetect.C5 example: functional sets 2004-12-21

// Compile with 
//   csc /r:BotDetect.C5.dll Sets.cs 

using System;
using System.Text;
using BotDetect.C5;
using SCG = System.Collections.Generic;

namespace BotDetect {
  // The class of sets with item type T, implemented as a subclass of
  // HashSet<T> but with functional infix operators * + - that compute
  // intersection, union and difference functionally.  That is, they
  // create a new set object instead of modifying an existing one.
  // The hasher is automatically created so that it is appropriate for
  // T.  In particular, this is true when T has the form Set<W> for
  // some W, since Set<W> implements ICollectionValue<W>.

  [Serializable]
  public class Set<T> : HashSet<T> {
    public Set(SCG.IEnumerable<T> enm) : base() {
      AddAll(enm);
    }

    public Set(params T[] elems) : this((SCG.IEnumerable<T>)elems) { }

    // Set union (+), difference (-), and intersection (*):

    public static Set<T> operator +(Set<T> s1, Set<T> s2) {
      if (s1 == null || s2 == null) 
        throw new ArgumentNullException("Set+Set");
      else {
        Set<T> res = new Set<T>(s1);
        res.AddAll(s2);
        return res;
      }
    }

    public static Set<T> operator -(Set<T> s1, Set<T> s2) {
      if (s1 == null || s2 == null) 
        throw new ArgumentNullException("Set-Set");
      else {
        Set<T> res = new Set<T>(s1);
        res.RemoveAll(s2);
        return res;
      }
    }

    public static Set<T> operator *(Set<T> s1, Set<T> s2) {
      if (s1 == null || s2 == null) 
        throw new ArgumentNullException("Set*Set");
      else {
        Set<T> res = new Set<T>(s1);
        res.RetainAll(s2);
        return res;
      }
    }

    // Equality of sets; take care to avoid infinite loops

    public static bool operator ==(Set<T> s1, Set<T> s2) {
      return EqualityComparer<Set<T>>.Default.Equals(s1, s2);
    }

    public static bool operator !=(Set<T> s1, Set<T> s2) {
      return !(s1 == s2);
    }

    public override bool Equals(Object that) {
      return this == (that as Set<T>);
    }

    public override int GetHashCode() {
      return EqualityComparer<Set<T>>.Default.GetHashCode(this);
    }

    // Subset (<=) and superset (>=) relation:

    public static bool operator <=(Set<T> s1, Set<T> s2) {
      if (s1 == null || s2 == null) 
        throw new ArgumentNullException("Set<=Set");
      else
        return s1.ContainsAll(s2);
    }

    public static bool operator >=(Set<T> s1, Set<T> s2) {
      if (s1 == null || s2 == null) 
        throw new ArgumentNullException("Set>=Set");
      else
        return s2.ContainsAll(s1);
    }
    
    public override String ToString() {
      StringBuilder sb = new StringBuilder();
      sb.Append("{");
      bool first = true;
      foreach (T x in this) {
        if (!first)
          sb.Append(",");
        sb.Append(x);
        first = false;
      }
      sb.Append("}");
      return sb.ToString();
    }
  }
}
