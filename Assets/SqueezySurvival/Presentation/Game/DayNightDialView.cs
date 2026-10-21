using System;
using SqueezySurvival.Core.PixelArt;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Presentation.PixelArt;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Shows the HUD's day and night dial, turned to the game clock (Docs/Design/Systems/Time.md, Display). Core
    /// paints each angle; this repaints one texture whenever time passes.
    /// </summary>
    public sealed class DayNightDialView : IDisposable
    {
        private readonly Texture2D _texture;
        private int _paintedMinute = -1;

        /// <summary>Binds the dial element and shows the given time.</summary>
        public DayNightDialView(VisualElement dial, GameClock clock)
        {
            if (dial == null)
            {
                throw new ArgumentNullException(nameof(dial));
            }

            _texture = PixelArtTextureCache.CreateTexture(DayNightDialPainter.Paint(clock.MinuteOfDay), "Day and night dial");
            _paintedMinute = clock.MinuteOfDay;
            dial.style.width = DayNightDialPainter.Width * InterfaceFrameStyles.PixelScale;
            dial.style.height = DayNightDialPainter.Height * InterfaceFrameStyles.PixelScale;
            dial.style.backgroundImage = Background.FromTexture2D(_texture);
        }

        /// <summary>Turns the dial to a new time.</summary>
        public void Show(GameClock clock)
        {
            if (clock.MinuteOfDay == _paintedMinute)
            {
                return;
            }

            PixelArtTextureCache.CopyInto(_texture, DayNightDialPainter.Paint(clock.MinuteOfDay));
            _paintedMinute = clock.MinuteOfDay;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            UnityObjectDestroyer.Destroy(_texture);
        }
    }
}
