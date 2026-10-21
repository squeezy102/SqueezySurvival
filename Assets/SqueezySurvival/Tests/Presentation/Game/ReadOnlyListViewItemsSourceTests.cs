using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SqueezySurvival.Presentation.Game;

namespace SqueezySurvival.Presentation.Tests.Game
{
    [TestFixture]
    public sealed class ReadOnlyListViewItemsSourceTests
    {
        private readonly string _first = "first";
        private readonly string _second = "second";

        [Test]
        public void ReadsGoStraightToTheSourceList_SoAddedItemsAppear()
        {
            var items = new List<string> { _first };
            var source = new ReadOnlyListViewItemsSource<string>(items);

            items.Add(_second);

            Assert.That(source.Count, Is.EqualTo(2));
            Assert.That(source[1], Is.SameAs(_second));
        }

        [Test]
        public void IndexOf_FindsTheSameObjectOnly()
        {
            var source = new ReadOnlyListViewItemsSource<string>(new List<string> { _first, _second });

            Assert.That(source.IndexOf(_second), Is.EqualTo(1));
            Assert.That(source.IndexOf(new string(_second.ToCharArray())), Is.EqualTo(-1));
            Assert.That(source.IndexOf(42), Is.EqualTo(-1));
            Assert.That(source.Contains(_first), Is.True);
        }

        [Test]
        public void CopyToAndEnumeration_GiveTheItemsInOrder()
        {
            var source = new ReadOnlyListViewItemsSource<string>(new List<string> { _first, _second });
            var copy = new object[3];

            source.CopyTo(copy, 1);

            Assert.That(copy, Is.EqualTo(new object[] { null, _first, _second }));
            Assert.That((IEnumerable)source, Is.EqualTo(new[] { _first, _second }));
        }

        [Test]
        public void EveryChange_IsRefusedAndChangesNothing()
        {
            var items = new List<string> { _first };
            var source = new ReadOnlyListViewItemsSource<string>(items);

            Assert.That(source.IsReadOnly, Is.True);
            Assert.That(() => source.Add(_second), Throws.TypeOf<NotSupportedException>());
            Assert.That(() => source.Insert(0, _second), Throws.TypeOf<NotSupportedException>());
            Assert.That(() => source.Remove(_first), Throws.TypeOf<NotSupportedException>());
            Assert.That(() => source.RemoveAt(0), Throws.TypeOf<NotSupportedException>());
            Assert.That(() => source.Clear(), Throws.TypeOf<NotSupportedException>());
            Assert.That(() => source[0] = _second, Throws.TypeOf<NotSupportedException>());
            Assert.That(items, Is.EqualTo(new[] { _first }));
        }

        [Test]
        public void Constructor_NullList_IsRefused()
        {
            Assert.That(() => new ReadOnlyListViewItemsSource<string>(null), Throws.ArgumentNullException);
        }
    }
}
