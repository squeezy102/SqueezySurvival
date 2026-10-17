using System;
using System.Globalization;
using SqueezySurvival.Core.Options;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Options
{
    /// <summary>
    /// Binds one GridPanelLayoutOptionsEditor.uxml instance to a grid panel's settings in the shared
    /// <see cref="PlayerOptions"/>. Edits go straight into the options, and changes made elsewhere show here at once
    /// (Docs/Design/UI/GridPanels.md, Where they are changed).
    /// </summary>
    public sealed class GridPanelLayoutOptionsEditorController : IDisposable
    {
        private const int VerticalChoiceIndex = 0;
        private const int HorizontalChoiceIndex = 1;

        private readonly PlayerOptions _options;
        private readonly RadioButtonGroup _scrollDirectionChoice;
        private readonly Label _lineCountLabel;
        private readonly Label _lineCountValue;
        private readonly Button _decreaseButton;
        private readonly Button _increaseButton;
        private readonly Toggle _invertScrollWheelToggle;
        private GridPanel _panel;

        /// <summary>Binds the editor's controls to the given panel's settings.</summary>
        public GridPanelLayoutOptionsEditorController(VisualElement editor, PlayerOptions options, GridPanel panel)
        {
            if (editor == null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            _options = options ?? throw new ArgumentNullException(nameof(options));
            _panel = panel;

            _scrollDirectionChoice = editor.Q<RadioButtonGroup>("scroll-direction-choice");
            _lineCountLabel = editor.Q<Label>("line-count-label");
            _lineCountValue = editor.Q<Label>("line-count-value");
            _decreaseButton = editor.Q<Button>("line-count-decrease-button");
            _increaseButton = editor.Q<Button>("line-count-increase-button");
            _invertScrollWheelToggle = editor.Q<Toggle>("invert-scroll-wheel-toggle");

            _scrollDirectionChoice.RegisterValueChangedCallback(changed => Update(layout => layout.WithScrollDirection(
                changed.newValue == HorizontalChoiceIndex ? GridPanelScrollDirection.Horizontal : GridPanelScrollDirection.Vertical)));
            _decreaseButton.clicked += () => Update(layout => layout.WithFixedLineCount(layout.FixedLineCount - 1));
            _increaseButton.clicked += () => Update(layout => layout.WithFixedLineCount(layout.FixedLineCount + 1));
            _invertScrollWheelToggle.RegisterValueChangedCallback(changed => Update(layout => layout.WithInvertScrollWheel(changed.newValue)));

            _options.GridPanelLayoutChanged += OnGridPanelLayoutChanged;
            Refresh();
        }

        /// <summary>The first control, for focusing when the editor is shown.</summary>
        public Focusable FirstControl => _scrollDirectionChoice;

        /// <summary>Points the editor at another panel's settings.</summary>
        public void ShowPanel(GridPanel panel)
        {
            _panel = panel;
            Refresh();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _options.GridPanelLayoutChanged -= OnGridPanelLayoutChanged;
        }

        private void Update(Func<GridPanelLayoutOptions, GridPanelLayoutOptions> change)
        {
            _options.SetGridPanelLayout(_panel, change(_options.GridPanelLayout(_panel)));
        }

        private void OnGridPanelLayoutChanged(GridPanel panel)
        {
            if (panel == _panel)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            GridPanelLayoutOptions layout = _options.GridPanelLayout(_panel);
            bool vertical = layout.ScrollDirection == GridPanelScrollDirection.Vertical;

            _scrollDirectionChoice.SetValueWithoutNotify(vertical ? VerticalChoiceIndex : HorizontalChoiceIndex);
            _lineCountLabel.text = vertical ? "Columns" : "Rows";
            _lineCountValue.text = layout.FixedLineCount.ToString(CultureInfo.InvariantCulture);
            _decreaseButton.SetEnabled(layout.FixedLineCount > layout.MinimumFixedLineCount);
            _increaseButton.SetEnabled(layout.FixedLineCount < layout.MaximumFixedLineCount);
            _invertScrollWheelToggle.SetValueWithoutNotify(layout.InvertScrollWheel);
        }
    }
}
