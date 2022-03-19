using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace NetSpell.DictionaryBuild.Hunspell.Tests
{
    [TestFixture]
    public class AffixFileTests
    {
        [TestCase(@"..\..\Data\Dics\de_DE.aff", "ISO8859-1")]
        [TestCase(@"..\..\Data\Dics\ro_RO.aff", "UTF-8")]
        [TestCase(@"..\..\Data\Dics\ru_RU.aff", "KOI8-R")]
        public void GetEncoding(string filePath, string expectedEncoding)
        {
            var fullFilePath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, filePath));
            var encoding = AffixFile.GetEncoding(fullFilePath);

            Assert.AreEqual(expectedEncoding, encoding);
        }

        [Test]
        public void LoadRomanianSampleFile()
        {
            var fullFilePath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\Data\Samples\ro_RO.aff"));
            var affixFile = AffixFile.Load(fullFilePath, "UTF-8");

            Assert.AreEqual(fullFilePath, affixFile.Path);
            Assert.AreEqual("UTF-8", affixFile.Encoding);
            Assert.AreEqual("iaăâșțîertolncusmpdbgfzvhjxkwyqACDM", affixFile.TryCharacters);

            var expectedPrefixesCollection = new Collection<Affix>()
            {
                new Affix(AffixClass.Prefix, 'o', true, new Collection<AffixRule>()
                {
                    new AffixRule('o', null, "auto", null)
                }),
                new Affix(AffixClass.Prefix, 'e', true, new Collection<AffixRule>()
                {
                    new AffixRule('e', null, "ne", null)
                })
            };

            var expectedSuffixesCollection = new Collection<Affix>()
            {
                new Affix(AffixClass.Suffix, 't', true, new Collection<AffixRule>()
                {
                    new AffixRule('t', null, "u", "eț"),
                    new AffixRule('t', null, "ul", "eț"),
                    new AffixRule('t', null, "ului", "eț")
                }),
                new Affix(AffixClass.Suffix, 'e', true, new Collection<AffixRule>()
                {
                    new AffixRule('e', null, "le", "e"),
                    new AffixRule('e', null, "lui", "e")
                })
            };

            var expectedReplacementsCollection = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("ce", "che"),
                new KeyValuePair<string, string>("ci", "chi"),
            };

            CollectionAssert.AreEqual(expectedPrefixesCollection, affixFile.Prefixes);
            CollectionAssert.AreEqual(expectedSuffixesCollection, affixFile.Suffixes);
            CollectionAssert.AreEqual(expectedReplacementsCollection, affixFile.Replacements);
        }

        [TestCase(@"..\..\Data\Dics\de_DE.aff")]
        [TestCase(@"..\..\Data\Dics\ro_RO.aff")]
        [TestCase(@"..\..\Data\Dics\ru_RU.aff")]
        public void LoadFullFile(String filePath)
        {
            var fullFilePath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, filePath));

            AffixFile affixFile = null;

            Assert.DoesNotThrow(() => { affixFile = AffixFile.Load(fullFilePath, encoding: AffixFile.GetEncoding(fullFilePath)); });

            Console.WriteLine(string.Format("affixFile.Prefixes.Count: {0}", affixFile.Prefixes.Count));
            Console.WriteLine(string.Format("affixFile.Suffixes.Count: {0}", affixFile.Suffixes.Count));
            Console.WriteLine(string.Format("affixFile.Replacements.Count: {0}", affixFile.Replacements.Count));
        }
    }
}