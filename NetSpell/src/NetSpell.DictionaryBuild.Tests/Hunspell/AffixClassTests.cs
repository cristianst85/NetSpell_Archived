using System;
using NUnit.Framework;

namespace NetSpell.DictionaryBuild.Hunspell.Tests {

    [TestFixture]
    public class AffixClassTests {

        [TestCase("PFX")]
        [TestCase("SFX")]
        [TestCase("pfx")]
        [TestCase("sfx")]
        public void FromString(String affixClassString) {
            AffixClass affixClass = AffixClass.FromString(affixClassString);
            Assert.AreEqual(affixClassString.ToUpper(), affixClass.ToString());
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        [TestCase("ABC", ExpectedException = typeof(ArgumentException))]
        [TestCase("abc", ExpectedException = typeof(ArgumentException))]
        public void FromStringThrowsException(String affixClassString) {
            AffixClass.FromString(affixClassString);
        }
    }
}