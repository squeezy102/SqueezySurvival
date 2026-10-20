using System;
using NUnit.Framework;
using SqueezySurvival.Core.Controls;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Controls
{
    [TestFixture]
    public sealed class MoveModifierBindingsTests
    {
        [TestCase(true, false, false, ItemTransferQuantity.All)]
        [TestCase(false, true, false, ItemTransferQuantity.Half)]
        [TestCase(false, false, true, ItemTransferQuantity.One)]
        public void Default_OneModifierHeld_GivesItsRole(bool shift, bool control, bool alt, ItemTransferQuantity expected)
        {
            Assert.That(MoveModifierBindings.Default.QuantityForHeld(shift, control, alt), Is.EqualTo(expected));
        }

        [Test]
        public void Default_NothingHeld_GivesNoRole()
        {
            Assert.That(MoveModifierBindings.Default.QuantityForHeld(false, false, false), Is.Null);
        }

        [TestCase(true, true, false, ItemTransferQuantity.Half)]
        [TestCase(true, false, true, ItemTransferQuantity.One)]
        [TestCase(false, true, true, ItemTransferQuantity.One)]
        [TestCase(true, true, true, ItemTransferQuantity.One)]
        public void Default_SeveralHeld_TheSmallestMoveWins(bool shift, bool control, bool alt, ItemTransferQuantity expected)
        {
            Assert.That(MoveModifierBindings.Default.QuantityForHeld(shift, control, alt), Is.EqualTo(expected));
        }

        [Test]
        public void Remapped_KeysFollowTheirNewRoles()
        {
            var bindings = new MoveModifierBindings(MoveModifierKey.Alt, MoveModifierKey.Shift, MoveModifierKey.Control);

            Assert.That(bindings.QuantityForHeld(false, false, true), Is.EqualTo(ItemTransferQuantity.All));
            Assert.That(bindings.QuantityForHeld(true, false, false), Is.EqualTo(ItemTransferQuantity.Half));
            Assert.That(bindings.QuantityForHeld(false, true, false), Is.EqualTo(ItemTransferQuantity.One));
        }

        [Test]
        public void Constructor_OneKeyForTwoRoles_IsRefused()
        {
            Assert.That(
                () => new MoveModifierBindings(MoveModifierKey.Shift, MoveModifierKey.Shift, MoveModifierKey.Alt),
                Throws.TypeOf<ArgumentException>());
        }
    }
}
