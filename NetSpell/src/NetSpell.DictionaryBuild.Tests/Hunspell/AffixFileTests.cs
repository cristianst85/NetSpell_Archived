using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using NUnit.Framework;

namespace NetSpell.DictionaryBuild.Hunspell.Tests {

    [TestFixture]
    public class AffixFileTests {

        [TestCase(@"..\..\Data\Dics\de_DE.aff", "ISO8859-1")]
        [TestCase(@"..\..\Data\Dics\ro_RO.aff", "UTF-8")]
        [TestCase(@"..\..\Data\Dics\ru_RU.aff", "KOI8-R")]
        public void GetEncoding(String filePath, String expectedEncoding) {
            String encoding = AffixFile.GetEncoding(filePath);
            Assert.AreEqual(expectedEncoding, encoding);
        }

        [Test]
        public void LoadRomanianSampleFile() {
            AffixFile affixFile = AffixFile.Load(@"..\..\Data\Samples\ro_RO.aff", "UTF-8");
            Assert.AreEqual(Path.GetFullPath(@"..\..\Data\Samples\ro_RO.aff"), affixFile.Path);
            Assert.AreEqual("UTF-8", affixFile.Encoding);
            Assert.AreEqual("iaăâșțîertolncusmpdbgfzvhjxkwyqACDM", affixFile.TryCharacters);

            ICollection<Affix> expectedPrefixesCollection = new Collection<Affix>() {
                new Affix(AffixClass.Prefix, 'o', true, new Collection<AffixRule>() { 
                    new AffixRule('o', null, "auto", null) 
                }),
                new Affix(AffixClass.Prefix, 'e', true, new Collection<AffixRule>() { 
                    new AffixRule('e', null, "ne", null)
                })
            };

            ICollection<Affix> expectedSuffixesCollection = new Collection<Affix>() {
                new Affix(AffixClass.Suffix, 't', true, new Collection<AffixRule>() { 
                    new AffixRule('t', null, "u", "eț"),
                    new AffixRule('t', null, "ul", "eț"),
                    new AffixRule('t', null, "ului", "eț")
                }),
                new Affix(AffixClass.Suffix, 'e', true, new Collection<AffixRule>() {
                    new AffixRule('e', null, "le", "e"),
                    new AffixRule('e', null, "lui", "e")
                })
            };

            IList<KeyValuePair<String, String>> expectedReplacementsCollection = new List<KeyValuePair<String, String>>() {
               new KeyValuePair<String, String>("ce", "che"),
                new KeyValuePair<String, String>("ci", "chi"),
            };

            CollectionAssert.AreEqual(expectedPrefixesCollection, affixFile.Prefixes);
            CollectionAssert.AreEqual(expectedSuffixesCollection, affixFile.Suffixes);
            CollectionAssert.AreEqual(expectedReplacementsCollection, affixFile.Replacements);
        }

        [TestCase(@"..\..\Data\Dics\de_DE.aff")]
        [TestCase(@"..\..\Data\Dics\ro_RO.aff")]
        [TestCase(@"..\..\Data\Dics\ru_RU.aff")]
        public void LoadFullFile(String filePath) {
            AffixFile affixFile = null;
            Assert.DoesNotThrow(
                () => {  affixFile = AffixFile.Load(filePath, encoding: AffixFile.GetEncoding(filePath)); }
            );

            Console.WriteLine(String.Format("affixFile.Prefixes.Count: {0}", affixFile.Prefixes.Count));
            Console.WriteLine(String.Format("affixFile.Suffixes.Count: {0}", affixFile.Suffixes.Count));
            Console.WriteLine(String.Format("affixFile.Replacements.Count: {0}", affixFile.Replacements.Count));
        }
    }
}