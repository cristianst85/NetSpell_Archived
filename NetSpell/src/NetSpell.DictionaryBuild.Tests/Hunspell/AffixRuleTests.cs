using NetSpell.DictionaryBuild.Hunspell;
using NUnit.Framework;

namespace NetSpell.DictionaryBuild.Tests.Hunspell
{
    [TestFixture]
    public class AffixRuleTests
    {
        [TestCase("SFX t 0 u eț", "SFX", 't', null, "u", "eț", null)]
        [TestCase("PFX e 0 ne .", "PFX", 'e', null, "ne", null, null)]
        [TestCase("SFX U a o a", "SFX", 'U', "a", "o", "a", null)]
        [TestCase("SFX U a ei [^cgk]a", "SFX", 'U', "a", "ei", "[^cgk]a", null)]
        [TestCase("SFX Q   0     u      [^eu]", "SFX", 'Q', null, "u", "[^eu]", null)]
        [TestCase("SFX l   0     ul      [^eu]           s. m. sg. art.", "SFX", 'l', null, "ul", "[^eu]", "s. m. sg. art.")]
        [TestCase("SFX I esc ii [^i]esc #prefect simplu", "SFX", 'I', "esc", "ii", "[^i]esc", "#prefect simplu")]
        public void TryParse(string line, string affixClassString, char affixRuleFlag, string strippingCharacters, string affix, string condition, string morphologicalDescription)
        {
            var parseResult = AffixRule.TryParse(line, out AffixRule affixRule, out AffixClass affixClass);
            
            Assert.AreEqual(true, parseResult, "parseResult");
            Assert.IsNotNull(affixRule, "Assert affixRule is not null.");
            Assert.IsNotNull(affixClass, "Assert affixClass is not null.");
            Assert.AreEqual(affixClassString, affixClass.ToString(), "affixClass");
            Assert.AreEqual(affixRuleFlag, affixRule.Flag, "flag");
            Assert.AreEqual(strippingCharacters, affixRule.StrippingCharacters, "strippingCharacters");
            Assert.AreEqual(affix, affixRule.Affix, "affix");
            Assert.AreEqual(condition, affixRule.Condition, "condition");
            Assert.AreEqual(morphologicalDescription, affixRule.MorphologicalDescription, "morphologicalDescription");
        }

        [TestCase("SFX t 0 u eț", "SFX", 't', null, "u", "eț", null)]
        [TestCase("PFX e 0 ne .", "PFX", 'e', null, "ne", null, null)]
        [TestCase("SFX U a o a", "SFX", 'U', "a", "o", "a", null)]
        [TestCase("SFX U a ei [^cgk]a", "SFX", 'U', "a", "ei", "[^cgk]a", null)]
        [TestCase("SFX Q   0     u      [^eu]", "SFX", 'Q', null, "u", "[^eu]", null)]
        [TestCase("SFX l   0     ul      [^eu]           s. m. sg. art.", "SFX", 'l', null, "ul", "[^eu]", "s. m. sg. art.")]
        [TestCase("SFX I esc ii [^i]esc #prefect simplu", "SFX", 'I', "esc", "ii", "[^i]esc", "#prefect simplu")]
        public void Parse(string line, string affixClassString, char affixRuleFlag, string strippingCharacters, string affix, string condition, string morphologicalDescription)
        {
            var affixRule = AffixRule.Parse(line, out AffixClass affixClass);
            
            Assert.IsNotNull(affixRule, "Assert affixRule is not null.");
            Assert.IsNotNull(affixClass, "Assert affixClass is not null.");
            Assert.AreEqual(affixClassString, affixClass.ToString(), "affixClass");
            Assert.AreEqual(affixRuleFlag, affixRule.Flag, "flag");
            Assert.AreEqual(strippingCharacters, affixRule.StrippingCharacters, "strippingCharacters");
            Assert.AreEqual(affix, affixRule.Affix, "affix");
            Assert.AreEqual(condition, affixRule.Condition, "condition");
            Assert.AreEqual(morphologicalDescription, affixRule.MorphologicalDescription, "morphologicalDescription");
        }

        [TestCase('o', null, "auto", null, null, "o 0 auto .")]
        [TestCase('t', null, "u", "eț", null, "t 0 u eț")]
        [TestCase('e', null, "ne", null, null, "e 0 ne .")]
        [TestCase('B', "ez", "ează", "[^i]ez", null, "B ez ează [^i]ez")]
        [TestCase('q', null, "ul", null, "adj. m. sg. art.", "q 0 ul . adj. m. sg. art.")]
        public void ToString(char affixClassFlag, string strippingCharacters, string affix, string condition, string morphologicalDescription, string expectedLineString)
        {
            var affixRule = new AffixRule(affixClassFlag, strippingCharacters, affix, condition, morphologicalDescription);
            
            Assert.AreEqual(expectedLineString, affixRule.ToString());
        }

        [TestCase('o', null, "auto", null, null, "o 0 auto .")]
        [TestCase('t', null, "u", "eț", null, "t 0 u eț")]
        [TestCase('e', null, "ne", null, null, "e 0 ne .")]
        [TestCase('B', "ez", "ează", "[^i]ez", null, "B ez ează [^i]ez")]
        [TestCase('q', null, "ul", null, "adj. m. sg. art.", "q 0 ul .")]
        public void ToNetSpellString(char affixClassFlag, string strippingCharacters, string affix, string condition, string morphologicalDescription, string expectedLineString)
        {
            var affixRule = new AffixRule(affixClassFlag, strippingCharacters, affix, condition, morphologicalDescription);
            
            Assert.AreEqual(expectedLineString, affixRule.ToNetSpellString());
        }

        [TestCase("PFX", 'o', null, "auto", null, null, "PFX o 0 auto .")]
        [TestCase("SFX", 't', null, "u", "eț", null, "SFX t 0 u eț")]
        [TestCase("SFX", 'e', null, "ne", null, null, "SFX e 0 ne .")]
        [TestCase("SFX", 'B', "ez", "ează", "[^i]ez", null, "SFX B ez ează [^i]ez")]
        [TestCase("SFX", 'q', null, "ul", null, "adj. m. sg. art.", "SFX q 0 ul . adj. m. sg. art.")]
        public void ToString(string affixClass, char affixClassFlag, string strippingCharacters, string affix, string condition, string morphologicalDescription, string expectedLineString)
        {
            var affixRule = new AffixRule(affixClassFlag, strippingCharacters, affix, condition, morphologicalDescription);
            
            Assert.AreEqual(expectedLineString, affixRule.ToString(AffixClass.FromString(affixClass)));
        }
    }
}
