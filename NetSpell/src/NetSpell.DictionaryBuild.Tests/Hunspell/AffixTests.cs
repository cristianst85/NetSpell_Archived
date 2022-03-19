using NetSpell.DictionaryBuild.Hunspell;
using NUnit.Framework;
using System;

namespace NetSpell.DictionaryBuild.Tests.Hunspell
{
    [TestFixture]
    public class AffixTests
    {
        [TestCase("SFX a Y 1", "SFX", 'a', true, 1)]
        [TestCase("SFX b N 2", "SFX", 'b', false, 2)]
        [TestCase("PFX c Y 3", "PFX", 'c', true, 3)]
        [TestCase("PFX d N 4", "PFX", 'd', false, 4)]
        [TestCase("PFX E N 5", "PFX", 'E', false, 5, Description = "Uppercase affix class flag.")]
        public void TryParse(string line, string affixClass, char affixClassFlag, bool isCrossProduct, int expectedNumberOfRules)
        {
            var parseResult = Affix.TryParse(line, out Affix affix, out int numberOfRules);

            Assert.AreEqual(true, parseResult, "parseResult");
            Assert.IsNotNull(affix, "Assert affix is not null.");
            Assert.AreEqual(affixClass, affix.Class.ToString(), "affixClass");
            Assert.AreEqual(affixClassFlag, affix.Flag, "flag");
            Assert.AreEqual(isCrossProduct, affix.IsCrossProduct, "isCrossProduct");
            Assert.AreEqual(expectedNumberOfRules, numberOfRules, "numberOfRules");
            Assert.AreEqual(0, affix.Rules.Count, "affxRulesCount");
        }

        [TestCase("SFX a Y 1", "SFX", 'a', true, 1)]
        [TestCase("SFX b N 2", "SFX", 'b', false, 2)]
        [TestCase("PFX c Y 3", "PFX", 'c', true, 3)]
        [TestCase("PFX d N 4", "PFX", 'd', false, 4)]
        public void Parse(string line, string affixClass, char affixClassFlag, bool isCrossProduct, int expectedNumberOfRules)
        {
            var affix = Affix.Parse(line, out int numberOfRules);

            Assert.IsNotNull(affix, "Assert affix is not null.");
            Assert.AreEqual(affixClass, affix.Class.ToString(), "affixClass");
            Assert.AreEqual(affixClassFlag, affix.Flag, "flag");
            Assert.AreEqual(isCrossProduct, affix.IsCrossProduct, "isCrossProduct");
            Assert.AreEqual(expectedNumberOfRules, numberOfRules, "numberOfRules");
            Assert.AreEqual(0, affix.Rules.Count, "affxRulesCount");
        }

        [TestCase("")]
        [TestCase("abc 1")]
        [TestCase("SFX 1")]
        [TestCase("SFX c 1", Description = "Missing cross product flag.")]
        [TestCase("SFX Y 1", Description = "Missing affix class flag.")]
        [TestCase("SFX c Y", Description = "Missing number of rules.")]
        [TestCase("abc c a x", Description = "Invalid affix class.")]
        [TestCase("SFX c a 1", Description = "Invalid cross product flag.")]
        [TestCase("SFX c a x", Description = "Invalid number of rules.")]
        [TestCase("SFX cc a x", Description = "Invalid affix class flag.")]
        public void ParseThrowsException(string line)
        {
            var numberOfRules = 0;

            Assert.Throws<Exception>(() => Affix.Parse(line, out numberOfRules));
            Assert.AreEqual(0, numberOfRules);
        }

        [TestCase(null)]
        public void ParseThrowsArgumentNullException(string line)
        {
            var numberOfRules = 0;

            Assert.Throws<ArgumentNullException>(() => Affix.Parse(line, out numberOfRules));
            Assert.AreEqual(0, numberOfRules);
        }

        [TestCase("")]
        [TestCase("abc 1")]
        [TestCase("SFX 1")]
        [TestCase("SFX c 1", Description = "Missing cross product flag.")]
        [TestCase("SFX Y 1", Description = "Missing affix class flag.")]
        [TestCase("SFX c Y", Description = "Missing number of rules.")]
        [TestCase("abc c a x", Description = "Invalid affix class.")]
        [TestCase("SFX c a 1", Description = "Invalid cross product flag.")]
        [TestCase("SFX c a x", Description = "Invalid number of rules.")]
        [TestCase("SFX cc a x", Description = "Invalid affix class flag.")]
        public void TryParseInvalidAffixes(string line)
        {
            var parseResult = Affix.TryParse(line, out Affix affix, out int numberOfRules);
            
            Assert.AreEqual(false, parseResult);
            Assert.IsNull(affix, "Assert affix is null.");
            Assert.AreEqual(0, numberOfRules);
        }

        [TestCase("SFX", 'a', true, "SFX a Y 0")]
        [TestCase("PFX", 'b', false, "PFX b N 0")]
        public void ToString(string affixClass, char affixClassFlag, bool isCrossProduct, string expectedLineString)
        {
            var affix = new Affix(affixClass, affixClassFlag, isCrossProduct);
            
            Assert.AreEqual(expectedLineString, affix.ToString());
        }

        [TestCase("SFX", 'a', true, "a Y 0")]
        [TestCase("PFX", 'b', false, "b N 0")]
        public void ToNetSpellString(string affixClass, char affixClassFlag, bool isCrossProduct, string expectedLineString)
        {
            var affix = new Affix(affixClass, affixClassFlag, isCrossProduct);
            
            Assert.AreEqual(expectedLineString, affix.ToNetSpellString());
        }
    }
}
