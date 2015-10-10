// AffixFile.cs
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
using System.Collections.ObjectModel;
using System.IO;

namespace NetSpell.DictionaryBuild.Hunspell {
    /// <summary>
    /// Partial implementation of the Hunspell file format.
    /// </summary>
    public class AffixFile {
        public String Path { get; private set; }
        public String Encoding { get; private set; }
        public String TryCharacters { get; set; }
        public ICollection<Affix> Prefixes { get; private set; }
        public ICollection<Affix> Suffixes { get; private set; }
        public IList<KeyValuePair<String, String>> Replacements { get; private set; }

        private AffixFile(String filePath, String encoding, String tryCharacters, ICollection<Affix> prefixes, ICollection<Affix> suffixes, IList<KeyValuePair<String, String>> replacements) {
            this.Path = filePath;
            this.Encoding = encoding;
            this.TryCharacters = tryCharacters;
            this.Prefixes = new Collection<Affix>();
            this.Suffixes = new Collection<Affix>();
            this.Replacements = new List<KeyValuePair<String, String>>();

            foreach (Affix affix in prefixes) {
                if (affix.IsPrefix()) {
                    this.Prefixes.Add(affix);
                }
                else {
                    throw new Exception("Only prefixes are allowed in prefixes collection.");
                }
            }

            foreach (Affix affix in suffixes) {
                if (affix.IsSuffix()) {
                    this.Suffixes.Add(affix);
                }
                else {
                    throw new Exception("Only suffixes are allowed in suffixes collection.");
                }
            }

            foreach (KeyValuePair<String, String> replacement in replacements) {
                this.Replacements.Add(replacement);
            }
        }

        public static String GetEncoding(String filePath) {
            if (!File.Exists(filePath)) {
                throw new FileNotFoundException(String.Format("File {0} was not found.", filePath));
            }

            // Peak into the file and read the encoding defined by SET option.
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("UTF-8"))) {
                    while (sr.Peek() >= 0) {
                        string tempLine = sr.ReadLine().Trim();
                        if (tempLine.StartsWith("SET")) {
                            return tempLine.Split(' ')[1].Trim();
                        }
                    }
                }
            }
            throw new Exception("Cannot detect file encoding.");
        }

        public static AffixFile Load(String filePath, String encoding = "UTF-8") {
            filePath = System.IO.Path.GetFullPath(filePath);

            if (!File.Exists(filePath)) {
                throw new FileNotFoundException(String.Format("File {0} was not found.", filePath));
            }

            String tryCharacters = null;
            ICollection<Affix> prefixes = new Collection<Affix>();
            ICollection<Affix> suffixes = new Collection<Affix>();
            List<KeyValuePair<String, String>> replacements = new  List<KeyValuePair<String, String>>();

            int lineCount = 0;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(encoding))) {
                    while (sr.Peek() >= 0) {
                        string tempLine = sr.ReadLine().Trim();
                        lineCount++;
                        if (tempLine.StartsWith("TRY")) {
                            tryCharacters = tempLine.Substring(4);
                        }
                        else if (tempLine.StartsWith("PFX") || tempLine.StartsWith("SFX")) {
                            AffixClass expectedAffixClass = AffixClass.FromString(tempLine.Substring(0, 3));
                            int expectedRulesCount = 0;
                            int parsedRulesCount = 0;
                            Affix affix = Affix.Parse(tempLine, out expectedRulesCount);
                            while (parsedRulesCount < expectedRulesCount) {
                                string tempRuleLine = sr.ReadLine().Trim();
                                lineCount++;
                                AffixClass affixClass = null;
                                AffixRule affixRule = null;
                                try {
                                    affixRule = AffixRule.Parse(tempRuleLine, out affixClass);
                                }
                                catch (Exception ex) {
                                    throw new Exception(String.Format("{0} at line {1}.", ex.Message.TrimEnd('.'), lineCount));
                                }
                                if (!affixClass.Equals(expectedAffixClass)) {
                                    throw new Exception(String.Format("Invalid affix class at line {2}. Expected {0} but {1} was found.", expectedAffixClass, affixClass, lineCount));
                                }
                                affix.Rules.Add(affixRule);
                                parsedRulesCount++;
                            }
                            // After all rules were parsed add the affix to the corresponding collection.
                            if (affix.IsPrefix()) {
                                prefixes.Add(affix);
                            }
                            else {
                                suffixes.Add(affix);
                            }
                        }
                        else if (tempLine.StartsWith("REP")) {
                            int expectedReplacementsCount = int.Parse(tempLine.Substring(4).Trim());
                            int parsedReplacementsCount = 0;
                            while (parsedReplacementsCount < expectedReplacementsCount) {
                                string tempReplacementLine = sr.ReadLine().Trim();
                                lineCount++;
                                string[] tokens = tempReplacementLine.Split(' ');
                                // Verify replacement option syntax.
                                if (tokens.Length != 3) {
                                    throw new Exception(String.Format("Invalid replacement option syntax at line {0}. Expected {1} tokens but were found {2}.", lineCount, 3, tokens.Length));
                                }
                                if (!tokens[0].Equals("REP")) {
                                    throw new Exception(String.Format("Invalid replacement option syntax at line {0}. Expected first token to be REP but was found {1}.", lineCount, tokens[0]));
                                }
                                replacements.Add(new KeyValuePair<string, string>(tokens[1], tokens[2]));
                                parsedReplacementsCount++;
                            }
                        }
                        else {
                            ; // Ignore anyting else.
                        }
                    }
                }
            }

            AffixFile affixFile = new AffixFile(filePath, encoding, tryCharacters, prefixes, suffixes, replacements);
            return affixFile;
        }
    }
}