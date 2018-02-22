namespace Lairinus.UI
{
    /// <summary>
    /// IActionObject - for use with Lairinus.UI.ActionSlotUI
    /// Allows easy integration between the Action (model) and the ActionSlotUI (controller)
    /// </summary>
    public interface IActionObject
    {
        /// <summary>
        /// The remaining cooldown time that needs to elapse before this action can be used again
        /// </summary>
        float remainingCooldownTime { get; }

        /// <summary>
        /// The total cooldown time that needs to elapse before this action can be used again
        /// </summary>
        float totalCooldownTime { get; }

        /// <summary>
        /// The remaining time that this Action will be considered "Active." Consider using the "Active" state for the time it takes to use an Action
        /// </summary>
        float remainingDurationTime { get; }

        /// <summary>
        /// The total time that this Action will be considered "Active." Consider using the "Active" state for the time it takes to use an Action
        /// </summary>
        float totalDurationTime { get; }

        /// <summary>
        /// Called by the IActonSlot OnClick() method
        /// When called, this will activate the Action. This can be called when the ActionSlot is clicked
        /// </summary>
        void UseAction();
    }
}