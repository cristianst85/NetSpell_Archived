// Affix.cs
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
using System.Collections.Generic;
using System.Linq;

namespace NetSpell.DictionaryBuild.Hunspell {

    public class Affix : IEquatable<Affix> {
        /// <summary>
        /// Affix class (prefix or suffx).
        /// </summary>
        public AffixClass Class { get; private set; }
        /// <summary>
        /// Name of the affix class.
        /// </summary>
        public char Flag { get; private set; }
        /// <summary>
        /// Can combine prefixes and suffixes.
        /// </summary>
        public bool IsCrossProduct { get; private set; }
        /// <summary>
        /// Affix rules.
        /// </summary>
        public ICollection<AffixRule> Rules { get; private set; }

        public Affix(String affixClass, char flag, bool isCrossProduct) {
            this.Class = AffixClass.FromString(affixClass);
            this.Flag = flag;
            this.IsCrossProduct = isCrossProduct;
            this.Rules = new List<AffixRule>();
        }

        public Affix(String affixClass, char flag, bool isCrossProduct, ICollection<AffixRule> rules) {
            this.Class = AffixClass.FromString(affixClass);
            this.Flag = flag;
            this.IsCrossProduct = isCrossProduct;
            this.Rules = new List<AffixRule>(rules);
        }

        public Affix(AffixClass affixClass, char flag, bool isCrossProduct, ICollection<AffixRule> rules) {
            this.Class = affixClass;
            this.Flag = flag;
            this.IsCrossProduct = isCrossProduct;
            this.Rules = new List<AffixRule>(rules);
        }

        public bool IsSuffix() {
            return this.Class.Equals(AffixClass.Suffix);
        }

        public bool IsPrefix() {
            return this.Class.Equals(AffixClass.Prefix);
        }

        public override string ToString() {
            return String.Format("{0} {1} {2} {3}", this.Class.ToString(), this.Flag, this.IsCrossProduct ? 'Y' : 'N', this.Rules.Count);
        }

        public bool Equals(Affix other) {
            if (other == null) {
                return false;
            }
            else {
                return
                    this.Class.Equals(other.Class) &&
                    this.Flag == other.Flag &&
                    this.IsCrossProduct == other.IsCrossProduct &&
                    this.Rules.SequenceEqual(other.Rules);
            }
        }

        /// <summary>
        /// Parse a line of text to an Affix object.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="numberOfRules"></param>
        /// <returns></returns>
        public static Affix Parse(string line, out int numberOfRules) {
            Affix affix;

            if (TryParse(line, out affix, out numberOfRules)) {
                return affix;
            }

            throw new Exception("Invalid affix syntax.");
        }

        /// <summary>
        /// Try to parse a line of text to an Affix object.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="numberOfRules"></param>
        /// <returns></returns>
        public static bool TryParse(string line, out Affix affix, out int numberOfRules) {

            if (line == null) {
                throw new ArgumentNullException("line");
            }

            affix = null;
            numberOfRules = 0;

            String[] tokens = line.Split(' ');

            if (tokens.Length != 4) {
                return false;
            }

            String affixClass = tokens[0].ToUpper();
            String classFlag = tokens[1];
            String crossProductFlag = tokens[2];

            if (!(crossProductFlag.Length == 1 && (crossProductFlag.Equals("Y") || crossProductFlag.Equals("N")))) {
                return false;
            }

            if (classFlag.Length != 1) {
                return false;
            }

            bool parseNumberOfRules = int.TryParse(tokens[3], out numberOfRules);

            if (!parseNumberOfRules) {
                return false;
            }

            if (!(affixClass.Equals(AffixClass.Prefix.ToString()) || affixClass.Equals(AffixClass.Suffix.ToString()))) {
                return false;
            }

            affix = new Affix(affixClass, char.Parse(classFlag), crossProductFlag.Equals("Y") ? true : false);

            return true;
        }
    }
}