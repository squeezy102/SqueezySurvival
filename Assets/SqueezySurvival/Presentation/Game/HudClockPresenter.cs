using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Time;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Shows the HUD clock and the day and night dial, and keeps them current as time advances
    /// (Docs/Design/Systems/Time.md, Display).
    /// </summary>
    public sealed class HudClockPresenter : IDisposable
    {
        private readonly IGameTime _gameTime;
        private readonly DayPhaseSchedule _dayPhaseSchedule;
        private readonly Label _dayLabel;
        private readonly Label _timeLabel;
        private readonly Label _phaseLabel;
        private readonly DayNightDialView _dayNightDial;

        /// <summary>Binds the HUD clock in <paramref name="root"/> to <paramref name="gameTime"/>.</summary>
        public HudClockPresenter(VisualElement root, IGameTime gameTime, DayPhaseSchedule dayPhaseSchedule)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            _gameTime = gameTime ?? throw new ArgumentNullException(nameof(gameTime));
            _dayPhaseSchedule = dayPhaseSchedule ?? throw new ArgumentNullException(nameof(dayPhaseSchedule));
            _dayLabel = root.Q<Label>("hud-clock-day");
            _timeLabel = root.Q<Label>("hud-clock-time");
            _phaseLabel = root.Q<Label>("hud-clock-phase");
            _dayNightDial = new DayNightDialView(root.Q<VisualElement>("hud-day-night-dial"), gameTime.Clock);
            _gameTime.TimeAdvanced += OnTimeAdvanced;
            Refresh();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _gameTime.TimeAdvanced -= OnTimeAdvanced;
            _dayNightDial.Dispose();
        }

        private void OnTimeAdvanced(object sender, GameTimeAdvance advance) => Refresh();

        private void Refresh()
        {
            _dayNightDial.Show(_gameTime.Clock);
            IReadOnlyList<string> rows = GameClockFormatter.FormatHudClockRows(_gameTime.Clock, _dayPhaseSchedule);
            _dayLabel.text = rows[0];
            _timeLabel.text = rows[1];
            _phaseLabel.text = rows[2];
        }
    }
}
