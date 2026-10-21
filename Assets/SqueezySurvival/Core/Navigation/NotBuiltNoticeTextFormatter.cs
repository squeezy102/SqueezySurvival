using System;

namespace SqueezySurvival.Core.Navigation
{
    /// <summary>The notice shown when a window that isn't built yet is chosen (Docs/Design/UI/ScreenLayout.md, Nav bar).</summary>
    public static class NotBuiltNoticeTextFormatter
    {
        /// <summary>For example "Crafting isn't built yet."</summary>
        public static string Format(NavigationBarEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return $"{entry.Name} isn't built yet.";
        }
    }
}
