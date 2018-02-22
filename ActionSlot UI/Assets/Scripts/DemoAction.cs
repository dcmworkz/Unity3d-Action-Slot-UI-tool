using System.Collections;
using UnityEngine;

/// <summary>
/// DEMO ONLY
/// --------------
/// This shows how you can implement the "Lairinus.UI.IActionObject" in your own game.
/// </summary>
public class DemoAction : MonoBehaviour, Lairinus.UI.IActionObject
{
    public Sprite icon = null;
    public KeyCode keycode = KeyCode.Alpha1;
    private bool _onCooldown = false;
    [SerializeField] private float _totalCooldown = 10;
    [SerializeField] private float _totalDuration = 5;
    public float remainingCooldownTime { get; private set; }
    public float remainingDurationTime { get; private set; }
    public float totalCooldownTime { get { return _totalCooldown; } }
    public float totalDurationTime { get { return _totalDuration; } }

    /// <summary>
    /// Uses the Action and starts the appropriate Coroutine
    /// </summary>
    public void UseAction()
    {
        if (!_onCooldown)
        {
            _onCooldown = true;
            StartCoroutine("ProcessDurationAndCooldownRoutine");
        }
    }

    /// <summary>
    /// Resets this action back to zero and stops it.
    /// </summary>
    public void ResetAction()
    {
        StopCoroutine("ProcessDurationAndCooldownRoutine");
        _onCooldown = false;
        remainingCooldownTime = 0;
        remainingDurationTime = 0;
    }

    /// <summary>
    /// Sets the total duration and cooldown for this Action.
    /// </summary>
    public void SetDurationAndCooldown(float duration, float cooldown)
    {
        _totalCooldown = cooldown;
        _totalDuration = duration;
        remainingCooldownTime = 0;
        remainingDurationTime = 0;
    }

    /// <summary>
    /// Simulate running an action and waiting for its' cooldown to finish.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProcessDurationAndCooldownRoutine()
    {
        // Pretend we're processing an Action with a duration...
        remainingDurationTime = _totalDuration;
        while (remainingDurationTime > 0)
        {
            remainingDurationTime -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Pretend we're processing an Action with a cooldown
        remainingCooldownTime = _totalCooldown;
        while (remainingCooldownTime > 0)
        {
            remainingCooldownTime -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Zero the times out
        remainingDurationTime = 0;
        remainingCooldownTime = 0;

        _onCooldown = false;
    }
}