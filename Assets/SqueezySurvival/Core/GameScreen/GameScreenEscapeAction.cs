namespace SqueezySurvival.Core.GameScreen
{
    /// <summary>What Escape does on the game screen (Docs/Design/UI/Controls.md, Escape).</summary>
    public enum GameScreenEscapeAction
    {
        CancelDrag,
        ClearSelection,
        ReturnToGroundOverflowQuestion,
        WaitForGroundOverflowAnswer,
        CloseTopOverlay,
        OpenPauseMenu,
    }
}
