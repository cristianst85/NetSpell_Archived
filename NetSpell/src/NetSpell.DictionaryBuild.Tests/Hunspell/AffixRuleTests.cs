using System;
using NetSpell.DictionaryBuild.Hunspell;
using NUnit.Framework;

namespace NetSpell.DictionaryBuild.Tests.Hunspell {

    [TestFixture]
    public class AffixRuleTests {

        [TestCase("SFX t 0 u eț", "SFX", 't', null, "u", "eț", null)]
        [TestCase("PFX e 0 ne .", "PFX", 'e', null, "ne", null, null)]
        [TestCase("SFX U a o a", "SFX", 'U', "a", "o", "a", null)]
        [TestCase("SFX U a ei [^cgk]a", "SFX", 'U', "a", "ei", "[^cgk]a", null)]
        [TestCase("SFX Q   0     u      [^eu]", "SFX", 'Q', null, "u", "[^eu]", null)]
        [TestCase("SFX l   0     ul      [^eu]           s. m. sg. art.", "SFX", 'l', null, "ul", "[^eu]", "s. m. sg. art.")]
        [TestCase("SFX I esc ii [^i]esc #prefect simplu", "SFX", 'I', "esc", "ii", "[^i]esc", "#prefect simplu")]
        public void TryParse(String line, String affixClassString, char affixRuleFlag, String strippingCharacters, String affix, String condition, String morphologicalDescription) {
            AffixRule affixRule = null;
            AffixClass affixClass = null;
            bool parseResult = AffixRule.TryParse(line, out affixRule, out affixClass);
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
        public void Parse(String line, String affixClassString, char affixRuleFlag, String strippingCharacters, String affix, String condition, String morphologicalDescription) {
            AffixClass affixClass = null;
            AffixRule affixRule = AffixRule.Parse(line, out affixClass);
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
        public void ToString(char affixClassFlag, String strippingCharacters, String affix, String condition, String morphologicalDescription, String expectedLineString) {
            AffixRule affixRule = new AffixRule(affixClassFlag, strippingCharacters, affix, condition, morphologicalDescription);
            Assert.AreEqual(expectedLineString, affixRule.ToString());
        }

        [TestCase('o', null, "auto", null, null, "o 0 auto .")]
        [TestCase('t', null, "u", "eț", null, "t 0 u eț")]
        [TestCase('e', null, "ne", null, null, "e 0 ne .")]
        [TestCase('B', "ez", "ează", "[^i]ez", null, "B ez ează [^i]ez")]
        [TestCase('q', null, "ul", null, "adj. m. sg. art.", "q 0 ul .")]
        public void ToNetSpellString(char affixClassFlag, String strippingCharacters, String affix, String condition, String morphologicalDescription, String expectedLineString) {
            AffixRule affixRule = new AffixRule(affixClassFlag, strippingCharacters, affix, condition, morphologicalDescription);
            Assert.AreEqual(expectedLineString, affixRule.ToNetSpellString());
        }

        [TestCase("PFX", 'o', null, "auto", null, null, "PFX o 0 auto .")]
        [TestCase("SFX", 't', null, "u", "eț", null, "SFX t 0 u eț")]
        [TestCase("SFX", 'e', null, "ne", null, null, "SFX e 0 ne .")]
        [TestCase("SFX", 'B', "ez", "ează", "[^i]ez", null, "SFX B ez ează [^i]ez")]
        [TestCase("SFX", 'q', null, "ul", null, "adj. m. sg. art.", "SFX q 0 ul . adj. m. sg. art.")]
        public void ToString(String affixClass, char affixClassFlag, String strippingCharacters, String affix, String condition, String morphologicalDescription, String expectedLineString) {
            AffixRule affixRule = new AffixRule(affixClassFlag, strippingCharacters, affix, condition, morphologicalDescription);
            Assert.AreEqual(expectedLineString, affixRule.ToString(AffixClass.FromString(affixClass)));
        }
    }
}