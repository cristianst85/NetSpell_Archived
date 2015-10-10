// AffixClass.cs
// 
// The MIT License (MIT)
//
// Copyright (C) 2015, Cristian Stoica.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace NetSpell.DictionaryBuild.Hunspell {

    public class AffixClass : IEquatable<AffixClass> {

        public static AffixClass Prefix {
            get {
                return new AffixClass("PFX");
            }
        }

        public static AffixClass Suffix {
            get {
                return new AffixClass("SFX");
            }
        }

        public String Name { get; private set; }

        private AffixClass(String affixClass) {
            if (affixClass == null) {
                throw new ArgumentNullException("affixClass");
            }
            if (!(affixClass.Equals("PFX") || affixClass.Equals("SFX"))) {
                throw new ArgumentException("Invalid affix class. Only PFX or SFX are allowed.");
            }
            this.Name = affixClass;
        }

        public override string ToString() {
            return this.Name;
        }

        public bool Equals(AffixClass other) {
            if (other == null) {
                return false;
            }
            else {
                return this.Name.Equals(other.Name);
            }
        }

        public static AffixClass FromString(String affixClass) {
            if (affixClass == null) {
                throw new ArgumentNullException("affixClass");
            }
            return new AffixClass(affixClass.ToUpper());
        }
    }
}