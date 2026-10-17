using System;

namespace SqueezySurvival.Core.Options
{
    /// <summary>
    /// Loads the player's options from a store and saves them back after every change, so settings survive between
    /// sessions without an Apply button (Docs/Design/UI/Options.md).
    /// </summary>
    public sealed class PlayerOptionsPersistence
    {
        private readonly IPlayerOptionsStore _store;

        private PlayerOptionsPersistence(IPlayerOptionsStore store, PlayerOptions options)
        {
            _store = store;
            Options = options;
            Options.GridPanelLayoutChanged += _ => Save();
        }

        /// <summary>The loaded options; changes to them are saved automatically.</summary>
        public PlayerOptions Options { get; }

        /// <summary>Loads the saved options, or defaults when nothing is saved, and keeps them saved from now on.</summary>
        public static PlayerOptionsPersistence Load(IPlayerOptionsStore store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            PlayerOptions options = store.TryRead(out string text)
                ? PlayerOptionsTextSerializer.Deserialize(text)
                : new PlayerOptions();
            return new PlayerOptionsPersistence(store, options);
        }

        private void Save()
        {
            _store.Write(PlayerOptionsTextSerializer.Serialize(Options));
        }
    }
}
