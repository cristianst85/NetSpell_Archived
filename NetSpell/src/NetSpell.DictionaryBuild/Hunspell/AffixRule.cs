// AffixRule.cs
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

    public class AffixRule : IEquatable<AffixRule> {
        /// <summary>
        /// Name of the affix class.
        /// </summary>
        public char Flag { get; private set; }
        /// <summary>
        /// Characters to strip from the beginning (at prefix rules)
        /// or end (at suffix rules) of the word.
        /// </summary>
        public String StrippingCharacters { get; private set; }
        /// <summary>
        /// Affix.
        /// </summary>
        public String Affix { get; private set; }
        /// <summary>
        /// Condition.
        /// </summary>
        public String Condition { get; private set; }
        /// <summary>
        /// Custom morphological description.
        /// </summary>
        public String MorphologicalDescription { get; private set; }

        public AffixRule(char flag, String strippingCharacters, String affix, String condition) {
            this.Flag = flag;
            this.StrippingCharacters = strippingCharacters;
            this.Affix = affix;
            this.Condition = condition;
        }

        public AffixRule(char flag, String strippingCharacters, String affix, String condition, String morphologicalDescription) {
            this.Flag = flag;
            this.StrippingCharacters = strippingCharacters;
            this.Affix = affix;
            this.Condition = condition;
            this.MorphologicalDescription = morphologicalDescription;
        }

        /// <summary>
        /// Parse a line of text to an AffixRule object.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="affixClass"></param>
        /// <returns></returns>
        public static AffixRule Parse(string line, out AffixClass affixClass) {
            AffixRule affixRule;
            Exception exception;

            if (internalParse(line, out affixRule, out affixClass, out exception)) {
                return affixRule;
            }
            else {
                throw new Exception(String.Format("Invalid affix rule syntax. {0}", exception.Message));
            }
        }

        /// <summary>
        /// Try to parse a line of text to an AffixRule object.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="affixRule"></param>
        /// <param name="affixClass"></param>
        /// <returns></returns>
        public static bool TryParse(string line, out AffixRule affixRule, out AffixClass affixClass) {
            Exception exception = null;
            return internalParse(line, out affixRule, out affixClass, out exception);
        }

        private static bool internalParse(string line, out AffixRule affixRule, out AffixClass affixClass, out Exception exception) {
            if (line == null) {
                throw new ArgumentNullException("line");
            }

            affixRule = null;
            affixClass = null;
            exception = null;

            String[] tokens = line.Split(new char[] { ' ' }, 6, StringSplitOptions.RemoveEmptyEntries);

            if (!(tokens.Length == 5 || tokens.Length == 6)) {
                exception = new Exception("Invalid number of tokens found.");
                return false;
            }

            String affixClassString = tokens[0];
            String classFlag = tokens[1];
            String strippingCharacters = tokens[2];
            String affix = tokens[3];
            String condition = tokens[4];
            String morphologicalDescription = null;

            if (tokens.Length == 6) {
                morphologicalDescription = tokens[5];
            }

            if (!(affixClassString.Equals(AffixClass.Prefix.ToString()) || affixClassString.Equals(AffixClass.Suffix.ToString()))) {
                exception = new Exception("Invalid affix class.");
                return false;
            }

            if (classFlag.Length != 1) {
                exception = new Exception("Invalid flag length.");
                return false;
            }

            if (strippingCharacters.Equals("0")) {
                strippingCharacters = null;
            }

            if (condition.Equals(".")) {
                condition = null;
            }

            affixClass = AffixClass.FromString(affixClassString);
            affixRule = new AffixRule(char.Parse(classFlag), strippingCharacters, affix, condition, morphologicalDescription);

            return true;
        }

        public override string ToString() {
            return String.Format("{0} {1} {2} {3} {4}", this.Flag, this.StrippingCharacters == null ? "0" : this.StrippingCharacters, this.Affix, this.Condition == null ? "." : this.Condition, this.MorphologicalDescription).TrimEnd();
        }

        public string ToNetSpellString() {
            return String.Format("{0} {1} {2} {3}", this.Flag, this.StrippingCharacters == null ? "0" : this.StrippingCharacters, this.Affix, this.Condition == null ? "." : this.Condition).TrimEnd();
        }

        public string ToString(AffixClass affixClass) {
            return String.Format("{0} {1} {2} {3} {4} {5}", affixClass.ToString(), this.Flag, this.StrippingCharacters == null ? "0" : this.StrippingCharacters, this.Affix, this.Condition == null ? "." : this.Condition, this.MorphologicalDescription).TrimEnd();
        }

        public bool Equals(AffixRule other) {
            if (other == null) {
                return false;
            }
            else {
                return
                    this.Flag == other.Flag &&
                    this.StrippingCharacters == other.StrippingCharacters &&
                    this.Affix == other.Affix &&
                    this.Condition == other.Condition &&
                    this.MorphologicalDescription == other.MorphologicalDescription;
            }
        }
    }
}