using System;

namespace SqueezySurvival.Core.DragAndDrop
{
    /// <summary>How a press of the pointer ended. A press ends as exactly one of these.</summary>
    public enum PointerPressEnding
    {
        /// <summary>The pointer never moved far and was let go over what it pressed: a click.</summary>
        Click,

        /// <summary>The pointer moved far enough to drag and was let go: the end of a drag, never also a click.</summary>
        DragEnd,

        /// <summary>The pointer never moved far but was let go somewhere else: neither a click nor a drag.</summary>
        Abandoned,
    }

    /// <summary>
    /// Tells a click from a drag within one press of the pointer, so the two are separate behaviors and a press is
    /// never both (Docs/Design/UI/DragAndDrop.md, Dragging). A press that has moved <see cref="DragStartDistance"/>
    /// becomes a drag and stays one, even if the pointer returns to where it began.
    /// </summary>
    public sealed class PointerPressGesture
    {
        /// <summary>How far the pointer must move with the button held before a press becomes a drag, in pixels.</summary>
        public const double DragStartDistance = 6.0;

        private double _pressX;
        private double _pressY;

        /// <summary>Whether the current press has become a drag.</summary>
        public bool IsDrag { get; private set; }

        /// <summary>Starts a new press at a position, forgetting the one before it.</summary>
        public void Press(double x, double y)
        {
            _pressX = x;
            _pressY = y;
            IsDrag = false;
        }

        /// <summary>Reports the pointer's position while the button is held, and returns whether the press is now a drag.</summary>
        public bool Move(double x, double y)
        {
            if (!IsDrag && Math.Sqrt(((x - _pressX) * (x - _pressX)) + ((y - _pressY) * (y - _pressY))) >= DragStartDistance)
            {
                IsDrag = true;
            }

            return IsDrag;
        }

        /// <summary>How the press ends when the button is let go, given whether the pointer is over what it pressed.</summary>
        public PointerPressEnding Release(bool isOverWhatWasPressed)
        {
            if (IsDrag)
            {
                return PointerPressEnding.DragEnd;
            }

            return isOverWhatWasPressed ? PointerPressEnding.Click : PointerPressEnding.Abandoned;
        }
    }
}
