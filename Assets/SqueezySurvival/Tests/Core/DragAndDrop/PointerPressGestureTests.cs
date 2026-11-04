using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class PointerPressGestureTests
    {
        [Test]
        public void Press_WithoutMoving_IsNotADrag()
        {
            var gesture = new PointerPressGesture();

            gesture.Press(100, 100);

            Assert.That(gesture.IsDrag, Is.False);
        }

        [TestCase(5.9, false)]
        [TestCase(6.0, true)]
        [TestCase(6.1, true)]
        public void Move_AlongOneAxis_BecomesADragAtSixPixels(double distance, bool expectedDrag)
        {
            var gesture = new PointerPressGesture();
            gesture.Press(100, 100);

            bool isDrag = gesture.Move(100 + distance, 100);

            Assert.That(isDrag, Is.EqualTo(expectedDrag));
            Assert.That(gesture.IsDrag, Is.EqualTo(expectedDrag));
        }

        [Test]
        public void Move_Diagonally_MeasuresStraightLineDistance()
        {
            var gesture = new PointerPressGesture();
            gesture.Press(0, 0);

            Assert.That(gesture.Move(4.2, 4.2), Is.False);
            Assert.That(gesture.Move(4.3, 4.3), Is.True);
        }

        [Test]
        public void Move_BackToWhereThePressBegan_StaysADrag()
        {
            var gesture = new PointerPressGesture();
            gesture.Press(100, 100);
            gesture.Move(120, 100);

            bool isDrag = gesture.Move(100, 100);

            Assert.That(isDrag, Is.True);
        }

        [Test]
        public void Press_AfterADrag_StartsOverAsAClick()
        {
            var gesture = new PointerPressGesture();
            gesture.Press(100, 100);
            gesture.Move(150, 100);

            gesture.Press(300, 300);

            Assert.That(gesture.IsDrag, Is.False);
            Assert.That(gesture.Move(303, 300), Is.False);
        }

        [Test]
        public void Release_PressThatNeverMovedFarLetGoOverWhatWasPressed_IsAClick()
        {
            var gesture = new PointerPressGesture();
            gesture.Press(100, 100);
            gesture.Move(103, 100);

            Assert.That(gesture.Release(isOverWhatWasPressed: true), Is.EqualTo(PointerPressEnding.Click));
        }

        [Test]
        public void Release_PressThatNeverMovedFarLetGoElsewhere_IsNeitherAClickNorADrag()
        {
            var gesture = new PointerPressGesture();
            gesture.Press(100, 100);

            Assert.That(gesture.Release(isOverWhatWasPressed: false), Is.EqualTo(PointerPressEnding.Abandoned));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Release_Drag_IsTheEndOfADragEvenOverWhatWasPressed(bool isOverWhatWasPressed)
        {
            var gesture = new PointerPressGesture();
            gesture.Press(100, 100);
            gesture.Move(130, 100);
            gesture.Move(100, 100);

            Assert.That(gesture.Release(isOverWhatWasPressed), Is.EqualTo(PointerPressEnding.DragEnd));
        }
    }
}
