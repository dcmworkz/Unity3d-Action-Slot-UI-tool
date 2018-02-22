using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public Sprite icon = null;
    public KeyCode keycode = KeyCode.Alpha1;
    private bool _onCooldown = false;
    [SerializeField] private float _totalCooldown = 10;
    [SerializeField] private float _totalDuration = 5;
    public float remainingCooldown { get; private set; }
    public float remainingDuration { get; private set; }
    public float totalCooldown { get { return _totalCooldown; } }
    public float totalDuration { get { return _totalDuration; } }

    /// <summary>
    /// Uses the Action and starts the appropriate Coroutine
    /// </summary>
    public void StartAction()
    {
        if (!_onCooldown)
        {
            _onCooldown = true;
            StartCoroutine(ProcessDurationAndCooldownRoutine());
        }
    }

    /// <summary>
    /// Resets this action completely
    /// </summary>
    public void ResetAction()
    {
        StopAllCoroutines();
        _onCooldown = false;
        remainingCooldown = 0;
        remainingDuration = 0;
    }

    /// <summary>
    /// Sets the total duration and cooldown for this Action.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="cooldown"></param>
    public void SetDurationAndCooldown(float duration, float cooldown)
    {
        _totalCooldown = cooldown;
        _totalDuration = duration;
        remainingCooldown = 0;
        remainingDuration = 0;
    }

    /// <summary>
    /// Emulates running an action and waiting for its' cooldown to finish.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProcessDurationAndCooldownRoutine()
    {
        remainingDuration = _totalDuration;

        while (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        remainingCooldown = _totalCooldown;

        while (remainingCooldown > 0)
        {
            remainingCooldown -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        remainingDuration = 0;
        remainingCooldown = 0;

        _onCooldown = false;
    }
}